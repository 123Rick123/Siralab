# Minuta técnica — Análisis firmados con sello en blanco (`AnalisisFirma`)

**Sistema:** SIRALAB (CONAGUA / INFOTEC)
**Tema:** Análisis que fueron firmados por el laboratorio pero quedaron guardados en `AnalisisFirma` con el campo `SelloDigital` en blanco, porque la selladora (`ServicioSelladoSoap` de SIRFIC/declarAgua) no respondió.
**Objetivo de la reunión:** Decidir entre **(A)** crear una API/proceso que regenere el sello de CONAGUA a partir de los datos ya guardados, o **(B)** sólo cambiar el estatus de firma del análisis para que el laboratorio vuelva a firmar.
**Fecha de elaboración:** 22 de octubre de 2026

---

## 1. Resumen ejecutivo (para abrir la reunión)

- Durante el firmado, el análisis se firma con la **FIEL del laboratorio** (esto SÍ quedó bien guardado) y, por separado, se solicita el **sello de CONAGUA** a un servicio externo.
- La selladora **`ServicioSelladoSoap`** (operación `Generar`, namespace `http://www.conagua.gob.mx/declarAgua`) **no estaba disponible**. El código asignó su resultado directamente al sello (`args.SelloCNA = sello.SellarCadenaOriginalSirfic();`) y, al fallar/devolver vacío, **el registro se guardó igual, con `SelloDigital` en blanco**.
- El análisis quedó marcado como **Firmado (estatus 10)** y `Analisi.Firmada = true`, aun sin sello de CONAGUA → es un estado inconsistente: "firmado por el laboratorio pero sin sellar por CONAGUA".
- Hay **dos caminos** para corregir, con implicaciones muy distintas en validez jurídica/criptográfica. El punto clave de la reunión es: **¿el sello de CONAGUA es una firma criptográfica sobre una cadena original, o un simple folio/consecutivo?** De eso depende si la opción A es viable.

---

## 2. Cómo funciona hoy el firmado y el sellado (evidencia en código)

Flujo en `SIRALAB/Queue/FirmaQueue.cs` → `FirmaAnalisis(Contenedor)`:

1. Se arma la **cadena original** (`CadenaOriginal` / `contenedor.cadenaAFirmar`) concatenando datos del análisis (ver sección 5).
2. El laboratorio firma con su FIEL → se obtienen: `firmaDigital`, `cadenaCMS`/PKCS7 y `cetificadoDigital`.
3. Se arma `AnalisisFirmaArgs` con `RFC`, `Firma`, `Cadena`, `Contenedor`, `FechaRegistro`.
4. Se solicita el **sello de CONAGUA**. Aquí está el problema:

```csharp
SelloDigital sello = new SelloDigital();
sello.cadenaOriginal = contenedor.cadenaAFirmar;
sello.SellarCadenaOriginal();                         // IDTrust/GlobalTrust (no se usa su resultado)
args.SelloCNA = sello.SellarCadenaOriginalSirfic();   // <-- Selladora SIRFIC (ServicioSelladoSoap.Generar)
var analisis = analisisFirmaBL.Guardar(args);         // Guarda AUNQUE SelloCNA venga vacío
```

5. `AnalisisFirma_BL.Guardar(...)` inserta el registro con `SelloDigital = argBase.SelloCNA` y llama `ActualizarAnalisisAFirmado(AnalisisID)` → pone `Analisi.Firmada = true`, `FechaFirma = DateTime.Now` y estatus **Firmado (10)**.

### Hallazgos clave

- **No hay validación** de que `SelloCNA` venga con valor antes de guardar. Por eso quedaron registros con sello en blanco pero marcados como firmados.
- Existen **dos mecanismos de sellado** en `SIRALAB/FirmaElectronica/SelloDigital.cs`:
  - `SellarCadenaOriginal()` → web service **IDTrust / GlobalTrust** (`wsIDTrust.Firma`). Devuelve una firma criptográfica (`FirmaResponseType.firma`) usando el **certificado de la AC de CONAGUA** configurado en `Web.config` (ver sección 4). En la cola, se ejecuta pero **su resultado no se asigna** a `SelloCNA`.
  - `SellarCadenaOriginalSirfic()` → **`ServicioSelladoSoap.Generar(cadenaOriginal)`** (la "selladora" que falló). **Éste es el que alimenta `SelloCNA`** en `FirmaQueue`.
- En `OrdenTrabajo.aspx.cs` (firma individual, no por cola) es al revés: usa `SellarCadenaOriginal()` (IDTrust) y la línea de SIRFIC está comentada. → **Inconsistencia**: el sello se genera distinto según la ruta de firma (cola vs. individual). Hay que confirmar cuál se considera el sello "oficial".

---

## 3. Estado actual de los datos afectados

Entidad `AnalisisFirma` (tabla con los registros dañados). Estado de un registro afectado:

| Campo | Estado en registros afectados |
|---|---|
| `AnalisisFirmaID` | OK |
| `AnalisisID` | OK |
| `RFC` | OK (RFC del laboratorio que firmó) |
| `Firma` | **OK** — firma FIEL del laboratorio |
| `Cadena` | **OK** — cadena original firmada (clave para regenerar, ver sección 5) |
| `Contenedor` | OK — certificado digital del laboratorio |
| `SelloDigital` | **EN BLANCO / vacío** ← problema |
| `FechaRegistro` | OK |

Entidad `Analisi`: `Firmada = true`, `FechaFirma` con valor, `EstatusID = 10 (Firmado)`.
> Es decir, para el sistema estos análisis están "firmados y finalizados", aunque les falte el sello de CONAGUA.

**Primer entregable para la reunión** (query a correr en `Siralab_Produccion_5`):
```sql
SELECT af.AnalisisFirmaID, af.AnalisisID, af.RFC, af.FechaRegistro, a.EstatusID, a.Firmada
FROM dbo.AnalisisFirma af
INNER JOIN dbo.Analisis a ON a.AnalisisID = af.AnalisisID
WHERE af.SelloDigital IS NULL OR LTRIM(RTRIM(af.SelloDigital)) = '';
```
Esto da el **universo exacto** de análisis afectados (cuántos y cuáles), dato imprescindible para dimensionar cualquiera de las dos opciones.

---

## 4. La pregunta que define todo: ¿qué es el "sello de CONAGUA"?

Antes de elegir A o B hay que responder esto (idealmente con el área de Firma Electrónica / SIRFIC):

- **Si el sello es una firma criptográfica** (IDTrust `Firma` o la selladora SIRFIC `Generar`) hecha con el **certificado/sellos de CONAGUA** sobre la `cadenaOriginal`:
  - La **Opción A es técnicamente viable** porque `AnalisisFirma.Cadena` conserva la cadena original exacta que se firmó. Se puede re-sellar esa misma cadena → el sello seguirá siendo válido y consistente con la firma del laboratorio.
  - **Condición indispensable:** usar el **mismo certificado/serie/algoritmo de CONAGUA** vigentes en el momento de la firma original (ver constantes abajo).
- **Si el sello es sólo un folio/consecutivo/sellado de tiempo generado por SIRFIC** (no derivado de los datos del análisis):
  - Regenerarlo "a posteriori" puede no tener el mismo valor legal (p. ej. si implica una marca de tiempo del momento real del firmado). Aquí la **Opción B** (re-firma) suele ser la más segura jurídicamente.

### Datos del certificado de sello de CONAGUA (configurables en `WebApplication.config` / appSettings)

Usados por `SIRALAB/FirmaElectronica/Constantes.cs` para el sellado IDTrust:

| AppSetting | Significado |
|---|---|
| `IdCertificadoWS` | Identificador del certificado de la AC que sella (ej. `ID-SELLOS-PBAS`) |
| `NumSerieCerficadoWS` | Número de serie del certificado de sello de CONAGUA |
| `AlgoritmoHash` | Algoritmo de digestión (ej. `SHA512`) |
| `RfcCertificado` | RFC del dueño del certificado de sello |

> ⚠️ **Verificar en la reunión:** si estos valores (certificado/serie) **cambiaron** desde la fecha en que se firmaron los análisis afectados, un sello regenerado hoy podría no corresponder al certificado original. Hay que confirmar vigencia y continuidad del certificado.

---

## 5. Campos que se deben considerar para regenerar un sello SIN afectar (Opción A)

La **cadena original** es lo que se sella. Se construye (ver `wucOrdenAnalisis.ascx.cs`, `AnalisisParametro.aspx.cs`, `EstatusFirmado.aspx.cs`) concatenando con el patrón `|| dato | dato | ... |` (clase `CadenaOriginal`). Los campos que la componen, **en este orden**, son:

1. `RFC Laboratorio` → `Analisi.Laboratorio.RFC`
2. `Nombre Laboratorio` → `Analisi.Laboratorio.Nombre`
3. `RFC Contribuyente` → `Analisi.OrdenTrabajo.Cliente.RFC`
4. `Nombre Contribuyente` → `Analisi.OrdenTrabajo.Cliente.Nombre`
5. `Fecha Análisis` → `Analisi.OrdenTrabajo.FechaEmision` (formato `ToShortDateString()`)
6. `Título de concesión` → `Analisi.Consecion.Titulo`
7. `Número de anexo` → `Analisi.Anexo.Anexo1`
8. `Nombre del punto de descarga` → `Analisi.Anexo.NombreCuerpoReceptor`
9. `Fecha de presentación del informe` → `DateTime.Now` en formato `dd-MM-yyyy HH:mm:ss` ← **campo sensible, ver abajo**
10. `Número de Informe` → `Analisi.OrdenTrabajo.NumeroInforme`
11. `Norma` → `Analisi.OrdenTrabajo.Norma.Nombre`
12. `Frecuencia de muestreo` → `Analisi.OrdenTrabajo.FrecuenciaMuestreo.Nombre`

### ⚠️ El punto crítico para "no afectar": la cadena original ya está guardada

**No hay que reconstruir la cadena desde cero** — eso sería arriesgado porque:

- El campo #9 **"Fecha de presentación del informe"** usa `DateTime.Now` del momento del firmado. Si se reconstruye hoy, cambiaría y el sello **ya no correspondería** a la firma del laboratorio.
- Otros datos (nombre de laboratorio, concesión, norma, etc.) **pudieron cambiar** en catálogos desde entonces.

**Recomendación técnica:** regenerar el sello usando **`AnalisisFirma.Cadena`** (la cadena original EXACTA que ya quedó guardada con la firma del laboratorio), NO reconstruyéndola. Ese campo es la fuente de verdad. En `FirmaQueue` la cadena sellada es la misma que se firma (`contenedor.cadenaAFirmar`), y esa cadena se persiste en `AnalisisFirma.Cadena`.

**Resumen de campos a considerar para la Opción A:**

| Campo / dato | Fuente | Por qué importa |
|---|---|---|
| `AnalisisFirma.Cadena` | BD (ya guardado) | **Es la cadena original a re-sellar; usarla tal cual, no reconstruir** |
| `AnalisisFirma.Firma` | BD (ya guardado) | Confirma que la firma del laboratorio sigue intacta |
| `AnalisisFirma.RFC` | BD | Identifica al firmante original |
| `IdCertificadoWS`, `NumSerieCerficadoWS`, `AlgoritmoHash`, `RfcCertificado` | appSettings | Certificado de sello de CONAGUA: debe ser el **vigente al momento del firmado** |
| Servicio de sellado | IDTrust (`Firma`) o SIRFIC (`Generar`) | Definir cuál es el sello "oficial" y cuál regenerar |
| `AnalisisFirma.SelloDigital` | BD (destino) | **Sólo escribir si estaba vacío**; respaldar antes |

---

## 6. Las dos opciones — análisis comparativo

### Opción A — API/proceso que regenera el `SelloDigital` desde los datos existentes

**En qué consiste:** un proceso (job o endpoint) que recorre los `AnalisisFirma` con sello vacío, toma la `Cadena` ya guardada, la envía a la selladora (una vez restablecida) y escribe el `SelloDigital` resultante.

**Implicaría:**
- Confirmar que la selladora (`ServicioSelladoSoap.Generar` o IDTrust) **ya está operativa y estable**.
- Confirmar que el **certificado de sello de CONAGUA** es el mismo/vigente que correspondía a esos análisis (sección 4).
- Desarrollar el proceso de reproceso (idealmente **idempotente**: sólo actualiza si `SelloDigital` está vacío).
- **Respaldar** la tabla `AnalisisFirma` antes de actualizar.
- **Validar** que el sello generado sea criptográficamente verificable contra la cadena y el certificado.
- Decidir dónde vive: puede ser un endpoint en el **API de Reportes (ASP.NET Core)** ya existente, o un script/servicio controlado. ⚠️ Nota de seguridad: ese API **hoy no tiene autenticación** (`UseAuthorization` comentado); un endpoint que modifica firmas **debe** protegerse.

**Ventajas:**
- No requiere molestar a los laboratorios ni re-firmar.
- Conserva la firma original del laboratorio y la fecha original.
- Rápido de aplicar en lote una vez validado.

**Riesgos / contras:**
- **Validez jurídica**: si el sello es un sellado de tiempo del momento real, regenerarlo con fecha posterior puede ser cuestionable. Requiere visto bueno del área normativa/jurídica.
- Depende de que el certificado original siga disponible y vigente.
- Si la selladora firma la cadena con marca de tiempo propia, el sello no coincidirá con el "momento" de la firma del laboratorio.

### Opción B — Cambiar el estatus para que el laboratorio vuelva a firmar

**En qué consiste:** revertir el estatus de firma de los análisis afectados para que el laboratorio repita el proceso de firmado (ya con la selladora operativa).

**Implicaría:**
- Cambiar `Analisi.EstatusID` de **Firmado (10)** a un estatus previo que permita re-firmar (ej. **Activo (3)** / **EnProceso (9)** — a confirmar cuál habilita el botón de firma en la UI).
- Poner `Analisi.Firmada = false` y limpiar `FechaFirma`.
- **Eliminar** (o invalidar) el registro `AnalisisFirma` incompleto. Ya existe lógica: `AnalisisFirma_BL.DeleteByAnalisisID(analisisID)`.
- Avisar a los laboratorios para que vuelvan a entrar y firmar.
- Validar que, al re-firmar, **ahora sí** se valide que el sello no venga vacío (ver corrección recomendada, sección 7).

**Ventajas:**
- **Máxima validez jurídica**: la firma y el sello se generan juntos, en el mismo acto, como debe ser.
- No hay que decidir sobre regeneración de sellos "a posteriori".
- Se apoya en el flujo normal ya existente.

**Riesgos / contras:**
- **Carga operativa** para los laboratorios (volver a firmar; puede requerir su FIEL y disponibilidad).
- La **nueva fecha de firma** será la actual, no la original (puede importar si hay plazos normativos NOM-001 / Ley Federal de Derechos).
- Riesgo de que algún laboratorio ya no esté vigente/activo (el login valida vigencia de aprobación y representante) → esos no podrían re-firmar sin reactivación.

---

## 7. Corrección de raíz (independiente de A o B)

El bug de fondo debe corregirse para que no vuelva a pasar: en `FirmaQueue.FirmaAnalisis`, **validar el sello antes de guardar** y marcar el análisis como fallido si la selladora no respondió, en lugar de guardar con sello en blanco:

```csharp
args.SelloCNA = sello.SellarCadenaOriginalSirfic();
if (string.IsNullOrWhiteSpace(args.SelloCNA) ||
    args.SelloCNA.Contains("Hubo algún problema")) 
{
    // NO guardar como firmado; marcar FirmaFallida (12) para reintentar
    throw new Exception("La selladora de CONAGUA no devolvió sello; se aborta el guardado.");
}
var analisis = analisisFirmaBL.Guardar(args);
```

Además conviene:
- Unificar **qué servicio** genera el sello oficial (IDTrust vs. SIRFIC) — hoy difiere entre la cola y el firmado individual.
- Agregar **reintentos / monitoreo** de la selladora.

---

## 8. Preguntas para resolver en la reunión (checklist)

1. ¿El sello de CONAGUA es **firma criptográfica sobre la cadena** o un **folio/sellado de tiempo**? (define si A es válida)
2. ¿Cuál es el **servicio oficial** de sellado: IDTrust/GlobalTrust o `ServicioSelladoSoap` de SIRFIC?
3. ¿El **certificado de sello** (serie/RFC) sigue **vigente** y es el mismo que correspondía a los análisis afectados?
4. ¿Jurídicamente es aceptable un sello **regenerado con fecha posterior**, o se exige re-firma en un mismo acto?
5. ¿Cuántos análisis están afectados? (correr query de la sección 3)
6. ¿Los laboratorios afectados **siguen activos/vigentes** para poder re-firmar (si se elige B)?
7. ¿Hay **plazos normativos** (NOM-001 / Ley Federal de Derechos) que se vean afectados por cambiar la fecha de firma?

---

## 9. Recomendación técnica preliminar

1. **Corregir el bug de raíz** (sección 7) ya mismo, para no generar más registros en blanco.
2. **Correr la query** para dimensionar el universo afectado.
3. Si el sello es una **firma criptográfica reproducible** sobre `AnalisisFirma.Cadena` y el certificado sigue vigente → **Opción A** (regenerar en lote usando la cadena ya guardada), con respaldo previo y validación del sello. Es la de menor impacto operativo.
4. Si el sello es un **sellado de tiempo** o el área jurídica exige acto único → **Opción B** (re-firma), eliminando el `AnalisisFirma` incompleto y revirtiendo estatus.
