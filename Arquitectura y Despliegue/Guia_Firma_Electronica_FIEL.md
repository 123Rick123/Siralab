# Guía de Implementación — Módulo de Firma Electrónica (FIEL)

Documentación técnica para replicar el módulo de firma electrónica de SIRALAB en otro proyecto ASP.NET WebForms/.NET Framework 4.5+.

---

## Tabla de Contenidos

- [Arquitectura General](#arquitectura-general)
- [Dependencias](#dependencias)
- [Configuración Web.config](#configuración-webconfig)
- [Estructura de Archivos](#estructura-de-archivos)
- [Clase 1: CadenaOriginal](#clase-1-cadenaoriginal)
- [Clase 2: CommonFirma](#clase-2-commonfirma)
- [Clase 3: Constantes](#clase-3-constantes)
- [Clase 4: Contenedor](#clase-4-contenedor)
- [Clase 5: FirmaElectronica](#clase-5-firmaelectronica)
- [Clase 6: SelloDigital](#clase-6-sellodigital)
- [Clase 7: FIEL (Librería de Seguridad)](#clase-7-fiel-librería-de-seguridad)
- [Control de Usuario: wucAppletFIEL](#control-de-usuario-wucappletfiel)
- [Modelo de Base de Datos](#modelo-de-base-de-datos)
- [Flujo Completo de Firma](#flujo-completo-de-firma)
- [Integración en una Página](#integración-en-una-página)
- [Servicios Web Requeridos](#servicios-web-requeridos)

---

## Arquitectura General

```
┌─────────────────────────────────────────────────────────────────┐
│                    USUARIO (Laboratorio)                         │
│           Sube: .cer + .key + contraseña + RFC                  │
└──────────────────────────┬──────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────────┐
│              wucAppletFIEL (Control de Usuario)                  │
│  - Recibe archivos .cer y .key via AsyncFileUpload              │
│  - Recibe contraseña y RFC                                      │
│  - Gestiona colección de contenedores a firmar                  │
└──────────────────────────┬──────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────────┐
│              Contenedor (Clase de firma)                         │
│  - Valida correspondencia llave pública/privada via FIEL.dll    │
│  - Genera firma digital (SHA1/SHA256 según tamaño de llave)     │
│  - Extrae certificado digital                                   │
└──────────────────────────┬──────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────────┐
│              SelloDigital (Sello CONAGUA)                        │
│  - Invoca WS IDTrust/GlobalTrust para sellado institucional     │
│  - Envía cadena original en UTF8                                │
│  - Recibe sello digital de la autoridad certificadora           │
└──────────────────────────┬──────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────────┐
│              Base de Datos (AnalisisFirma)                       │
│  - RFC, Firma, Cadena, Contenedor (cert), SelloDigital, Fecha   │
└─────────────────────────────────────────────────────────────────┘
```

---

## Dependencias

| Paquete NuGet / DLL | Versión | Uso |
|---|---|---|
| `BouncyCastle` | 1.8.6.1 | Criptografía: lectura de llaves .key, codificación hex |
| `FIEL.dll` (CONAGUA.Seguridad) | 1.0.x | Librería interna: valida FIEL, firma con llave privada |
| `AjaxControlToolkit` | 18.1.0 | AsyncFileUpload para archivos .cer/.key |
| Service Reference: `wsIDTrust` | — | WS GlobalTrust para sellado digital |
| Service Reference: `SelladoSirficWS` | — | WS alternativo de sellado |

### Instalar BouncyCastle:
```
Install-Package BouncyCastle -Version 1.8.6.1
```

### Instalar AjaxControlToolkit:
```
Install-Package AjaxControlToolkit -Version 18.1.0
```

---

## Configuración Web.config

Agregar las siguientes keys en `<appSettings>`:

```xml
<!-- Certificado de la Autoridad Certificadora para sellado -->
<add key="IdCertificadoWS" value="CSD-CNA-2014" />
<add key="NumSerieCerficadoWS" value="00001000000303343395" />
<add key="AlgoritmoHash" value="SHA1" />
<add key="RfcCertificado" value="CNA890116SF2" />
```

Agregar el endpoint del servicio de firma en `<system.serviceModel>`:

```xml
<client>
    <endpoint 
        address="http://firmaelectronica.conagua.gob.mx:9080/WsGlobalTrust/GlobalTrustService" 
        binding="basicHttpBinding" 
        bindingConfiguration="GlobalTrustPortTypePortBinding" 
        contract="wsIDTrust.GlobalTrustPortType" 
        name="GlobalTrustPortTypePort" />
    <endpoint 
        address="http://firmaelectronica.conagua.gob.mx:9080/WS_SELLADORA/ServicioSellado.asmx" 
        binding="basicHttpBinding" 
        bindingConfiguration="ServicioSelladoSoap1" 
        contract="SelladoSirficWS.ServicioSelladoSoap" 
        name="ServicioSelladoSoap" />
</client>
```

---

## Estructura de Archivos

```
FirmaElectronica/
├── CadenaOriginal.cs          # Construye la cadena original ||dato1|dato2||
├── CommonFirma.cs             # Utilidades: Base64, UTF8, Hex
├── Constantes.cs              # Lee config del web.config (certificado AC)
├── Contenedor.cs              # Firma individual + validación FIEL
├── FirmaElectronica.cs        # Colección de contenedores a firmar
├── SelloDigital.cs            # Invoca WS de sellado institucional
├── wucAppletFIEL.ascx         # Control visual (upload .cer/.key)
├── wucAppletFIEL.ascx.cs      # Code-behind del control
└── wucAppletFIEL.ascx.designer.cs
```

---

## Clase 1: CadenaOriginal

Construye la cadena original con formato SAT: `||dato1|dato2|...|datoN||`

```csharp
namespace TuProyecto.FirmaElectronica
{
    public class CadenaOriginal
    {
        public CadenaOriginal()
        {
            Datos = new List<string>();
        }

        public List<string> Datos { get; set; }

        /// <summary>
        /// Genera la cadena concatenada con formato ||campo1|campo2|...|campoN||
        /// </summary>
        public string CadenaConcatenada
        {
            get
            {
                var cadena = "||";
                if (this.Datos.Count > 0)
                {
                    foreach (string dato in this.Datos)
                    {
                        cadena += dato + "|";
                    }
                    cadena += "|";
                }
                else
                    cadena = string.Empty;
                return cadena;
            }
        }
    }
}
```

**Uso:**
```csharp
var cadenaOriginal = new CadenaOriginal();
cadenaOriginal.Datos.Add(analisis.AnalisisID.ToString());
cadenaOriginal.Datos.Add(analisis.RFC);
cadenaOriginal.Datos.Add(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
// Resultado: ||123|RFC123456ABC|19-08-2026 10:30:00||
```

---

## Clase 2: CommonFirma

Utilidades de conversión para firma electrónica.

```csharp
using Org.BouncyCastle.Utilities.Encoders;
using System.Text;

namespace TuProyecto.FirmaElectronica
{
    public static class CommonFirma
    {
        /// <summary>
        /// Convierte bytes a representación hexadecimal
        /// </summary>
        public static string ToHexString(byte[] byteArray)
        {
            byte[] hexEncodedArray = Hex.Encode(byteArray, 0, byteArray.Length);
            return Encoding.Default.GetString(hexEncodedArray);
        }

        /// <summary>
        /// Convierte string a Base64 codificado en UTF8
        /// </summary>
        public static string ToUTF8Base64String(string cadena)
        {
            try { return Convert.ToBase64String(UTF8Bytes(cadena)); }
            catch { return null; }
        }

        /// <summary>
        /// Convierte string a byte array en UTF8
        /// </summary>
        public static byte[] UTF8Bytes(string cadena)
        {
            try { return Encoding.UTF8.GetBytes(cadena); }
            catch { return null; }
        }
    }
}
```

---

## Clase 3: Constantes

Lee la configuración del certificado de la Autoridad Certificadora desde `web.config`.

```csharp
using System.Configuration;

namespace TuProyecto.FirmaElectronica
{
    public static class Constantes
    {
        public static string IdCertificado =>
            ConfigurationManager.AppSettings["IdCertificadoWS"];

        public static string SerieCertificado =>
            ConfigurationManager.AppSettings["NumSerieCerficadoWS"];

        public static string AlgoritmoHash =>
            ConfigurationManager.AppSettings["AlgoritmoHash"];

        public static string RfcCertificado =>
            ConfigurationManager.AppSettings["RfcCertificado"];
    }
}
```

---

## Clase 4: Contenedor

Contiene los datos de una firma individual. Valida la FIEL y genera la firma digital.

```csharp
using CONAGUA.Seguridad;
using System.IO;

namespace TuProyecto.FirmaElectronica
{
    public class Contenedor
    {
        // Propiedades de entrada
        public string primaryKey { get; set; }       // ID del registro a firmar
        public string cadenaAFirmar { get; set; }    // Cadena original
        public string cadenaCMS { get; set; }        // Resultado PKCS7/CMS
        public string parametroExtra { get; set; }   // Parámetro opcional
        public byte[] ArchivoKey { get; set; }       // Archivo .key en bytes
        public byte[] ArchivoCer { get; set; }       // Archivo .cer en bytes
        public string txtContra { get; set; }        // Contraseña del .key
        public string txtRFC { get; set; }           // RFC del firmante

        // Propiedades de salida (se calculan automáticamente)
        public string cadenaAFirmarBase64 => CommonFirma.ToUTF8Base64String(cadenaAFirmar);

        /// <summary>
        /// Genera la firma digital usando la llave privada (.key)
        /// </summary>
        public string firmaDigital
        {
            get
            {
                Stream privateKey = new MemoryStream(ArchivoKey);
                Stream publicKey = new MemoryStream(ArchivoCer);
                FIEL fiel = new FIEL(privateKey, publicKey, txtContra, txtRFC);
                if (fiel.Login())
                {
                    byte[] firmaBytes = fiel.firma(cadenaCMS, Encoding.UTF8);
                    return CommonFirma.ToHexString(firmaBytes).ToUpper();
                }
                return null;
            }
        }

        /// <summary>
        /// Obtiene información del certificado digital
        /// </summary>
        public string cetificadoDigital
        {
            get
            {
                Stream privateKey = new MemoryStream(ArchivoKey);
                Stream publicKey = new MemoryStream(ArchivoCer);
                FIEL fiel = new FIEL(privateKey, publicKey, txtContra, txtRFC);
                if (fiel.Login())
                    return fiel.Cert.ToString();
                return null;
            }
        }

        /// <summary>
        /// Valida que el .cer y .key correspondan y estén vigentes
        /// </summary>
        public bool valido
        {
            get
            {
                try
                {
                    Stream publicKey = new MemoryStream(ArchivoCer);
                    Stream privateKey = new MemoryStream(ArchivoKey);
                    FIEL fiel = new FIEL(privateKey, publicKey, txtContra, txtRFC);
                    return fiel.Login();
                }
                catch { return false; }
            }
        }

        // Constructores
        public Contenedor() { }
        public Contenedor(string pk, string cadena)
        {
            primaryKey = pk;
            cadenaAFirmar = cadena;
        }
    }

    /// <summary>
    /// Argumentos del evento de firma
    /// </summary>
    public class DatosFirma_Args : EventArgs
    {
        public FirmaElectronica DatosAFirmar { get; set; }
        public string RFCFirma { get; set; }
        public byte[] ArchivoKey { get; set; }
        public byte[] ArchivoCer { get; set; }
        public string txtContra { get; set; }
    }
}
```

---

## Clase 5: FirmaElectronica

Gestiona la colección de contenedores a firmar.

```csharp
namespace TuProyecto.FirmaElectronica
{
    public class FirmaElectronica
    {
        public char separador { get; set; }
        public List<Contenedor> listaContenedores { get; set; }
        public Exception exception { get; set; }

        public FirmaElectronica()
        {
            separador = ',';
            listaContenedores = new List<Contenedor>();
        }

        /// <summary>
        /// Agrega una cadena a firmar identificada por una llave primaria
        /// </summary>
        public void AddCadena(string primaryKey, string cadena)
        {
            var existente = listaContenedores
                .SingleOrDefault(a => a.primaryKey == primaryKey);
            if (existente != null)
                listaContenedores.Remove(existente);

            listaContenedores.Add(new Contenedor(primaryKey, cadena));
        }

        /// <summary>
        /// Procesa las cadenas firmadas y las asigna a cada contenedor
        /// </summary>
        public static FirmaElectronica obtenerListaFirmada(
            FirmaElectronica coleccion, string cadenaResult, char separador)
        {
            try
            {
                string[] cadenas = cadenaResult.Split(separador);
                for (int i = 0; i < cadenas.Length; i++)
                {
                    coleccion.listaContenedores[i].cadenaCMS = cadenas[i];
                }
            }
            catch (Exception ex)
            {
                coleccion.exception = ex;
            }
            return coleccion;
        }
    }
}
```

---

## Clase 6: SelloDigital

Invoca el Web Service de la autoridad certificadora (IDTrust/GlobalTrust) para obtener el sello institucional.

```csharp
namespace TuProyecto.FirmaElectronica
{
    public class SelloDigital
    {
        public string cadenaOriginal { get; set; }
        public string idCertificado { get; set; }
        public string serieCertificado { get; set; }
        public string algoritmoHash { get; set; }
        public string rfcCertificado { get; set; }
        public Exception erroExecption { get; set; }

        private wsIDTrust.FirmaResponseType repuestaOperacionFirma;

        public int codigoError =>
            repuestaOperacionFirma?.resultado.codigo ?? -1;

        public string mensajeError =>
            repuestaOperacionFirma?.resultado.texto ?? "No hubo respuesta del servidor.";

        /// <summary>
        /// Sello digital resultado (solo si codigoError == 0)
        /// </summary>
        public string sello =>
            codigoError == 0 ? repuestaOperacionFirma.firma : string.Empty;

        public SelloDigital()
        {
            idCertificado = Constantes.IdCertificado;
            serieCertificado = Constantes.SerieCertificado;
            algoritmoHash = Constantes.AlgoritmoHash;
            rfcCertificado = Constantes.RfcCertificado;
        }

        /// <summary>
        /// Invoca el WS de IDTrust para sellar la cadena original
        /// </summary>
        public void SellarCadenaOriginal()
        {
            var webService = new wsIDTrust.GlobalTrustPortTypeClient();
            try
            {
                var request = new wsIDTrust.FirmaRequestType
                {
                    id = this.idCertificado,
                    numeroSerie = this.serieCertificado,
                    cadenaOriginal = CommonFirma.UTF8Bytes(this.cadenaOriginal),
                    algHash = this.algoritmoHash,
                    rfc = this.rfcCertificado
                };
                this.repuestaOperacionFirma = webService.Firma(request);
            }
            catch (Exception ex)
            {
                this.erroExecption = ex;
                this.repuestaOperacionFirma = new wsIDTrust.FirmaResponseType
                {
                    resultado = new wsIDTrust.ResultadoType
                    {
                        codigo = ex.HResult,
                        texto = ex.Message
                    }
                };
            }
            finally
            {
                webService.Close();
            }
        }

        /// <summary>
        /// Método alternativo: sella usando el WS de SIRFIC
        /// </summary>
        public string SellarCadenaOriginalSirfic()
        {
            var cliente = new SelladoSirficWS.ServicioSelladoSoapClient();
            try
            {
                cliente.Open();
                return cliente.Generar(this.cadenaOriginal);
            }
            finally
            {
                cliente.Close();
            }
        }
    }
}
```

---

## Clase 7: FIEL (Librería de Seguridad)

La librería `CONAGUA.Seguridad.FIEL` (archivo `FIEL.dll`) es el núcleo criptográfico. Usa BouncyCastle para:

1. Leer y desencriptar la llave privada (.key) con la contraseña
2. Leer el certificado público (.cer)
3. Validar correspondencia llave pública ↔ privada
4. Validar vigencia del certificado
5. Validar que el RFC coincida con el certificado
6. Firmar datos con SHA1 (1024 bits) o SHA256 (2048 bits)

**Métodos principales:**

| Método | Descripción |
|---|---|
| `FIEL(Stream key, Stream cer, string password, string rfc)` | Constructor: inicializa llaves |
| `bool Login()` | Valida vigencia + RFC + correspondencia llaves |
| `byte[] firma(string cadena, Encoding codificacion)` | Firma un string, retorna bytes |
| `string firmastrBase64(string cadena, Encoding codificacion)` | Firma y retorna en Base64 |

**Para replicar necesitas:**
- El archivo `FIEL.dll` (incluido en el proyecto en `FIEL/bin/`)
- O reimplementar usando BouncyCastle directamente (ver código fuente en `FIEL/FIEL.cs`)

---

## Control de Usuario: wucAppletFIEL

Control visual que gestiona la carga de archivos y la interacción con el usuario.

**Registro en la página .aspx:**
```aspx
<%@ Register Src="~/FirmaElectronica/wucAppletFIEL.ascx" 
    TagPrefix="uc1" TagName="wucAppletFIEL" %>
```

**Uso en la página .aspx:**
```aspx
<uc1:wucAppletFIEL runat="server" ID="wucAppletFIEL" 
    OnFirmarColeccion="wucAppletFIEL_FirmarColeccion" />
```

**Métodos públicos del control:**

| Método | Descripción |
|---|---|
| `enviarAFirma(FirmaElectronica firma)` | Envía la colección de cadenas a firmar al control (muestra el modal) |
| `obtenerColeccionFirma()` | Retorna la colección con las firmas procesadas |

---

## Modelo de Base de Datos

Tabla `AnalisisFirma`:

```sql
CREATE TABLE AnalisisFirma (
    AnalisisFirmaID INT PRIMARY KEY,
    AnalisisID INT NOT NULL,           -- FK al registro firmado
    RFC VARCHAR(20) NOT NULL,          -- RFC del firmante
    Firma VARCHAR(MAX) NOT NULL,       -- Firma digital (hex uppercase)
    Cadena VARCHAR(MAX) NOT NULL,      -- Cadena original firmada
    Contenedor VARCHAR(MAX) NULL,      -- Info del certificado digital
    SelloDigital VARCHAR(MAX) NULL,    -- Sello de CONAGUA (WS IDTrust)
    FechaRegistro DATETIME NULL,       -- Fecha/hora de la firma
    FOREIGN KEY (AnalisisID) REFERENCES Analisis(AnalisisID)
);
```

Campo `Firmada` en tabla principal (`Analisis`):
```sql
ALTER TABLE Analisis ADD Firmada BIT NOT NULL DEFAULT 0;
ALTER TABLE Analisis ADD FechaFirma DATETIME NULL;
```

---

## Flujo Completo de Firma

### Paso 1: Construir la cadena original

```csharp
var cadenaOriginal = new CadenaOriginal();
cadenaOriginal.Datos.Add(analisis.AnalisisID.ToString());
cadenaOriginal.Datos.Add(laboratorio.RFC);
cadenaOriginal.Datos.Add(laboratorio.Nombre);
cadenaOriginal.Datos.Add(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
// Agrega todos los datos relevantes del documento
```

### Paso 2: Crear la colección de firmas y enviar al control

```csharp
var firma = new FirmaElectronica();
firma.AddCadena(analisis.AnalisisID.ToString(), cadenaOriginal.CadenaConcatenada);
// Si hay múltiples análisis:
// firma.AddCadena(otroAnalisis.AnalisisID.ToString(), otraCadena.CadenaConcatenada);

wucAppletFIEL.enviarAFirma(firma);
wucAppletFIEL.Visible = true;
```

### Paso 3: Manejar el evento de firma completada

```csharp
protected void wucAppletFIEL_FirmarColeccion(object sender, DatosFirma_Args e)
{
    FirmaElectronica datosFirma = e.DatosAFirmar;
    byte[] archivoKey = e.ArchivoKey;
    byte[] archivoCer = e.ArchivoCer;
    string contrasena = e.txtContra;

    foreach (Contenedor contenedor in datosFirma.listaContenedores)
    {
        // Asignar credenciales al contenedor
        contenedor.ArchivoCer = archivoCer;
        contenedor.ArchivoKey = archivoKey;
        contenedor.txtRFC = e.RFCFirma;
        contenedor.txtContra = contrasena;

        if (contenedor.valido)
        {
            // Obtener firma digital y certificado
            string firmaDigital = contenedor.firmaDigital;
            string certificado = contenedor.cetificadoDigital;

            // Sellar con CONAGUA (WS IDTrust)
            SelloDigital sello = new SelloDigital();
            sello.cadenaOriginal = contenedor.cadenaAFirmar;
            sello.SellarCadenaOriginal();
            string selloCNA = sello.sello;

            // Persistir en base de datos
            var args = new AnalisisFirmaArgs
            {
                AnalisisID = int.Parse(contenedor.primaryKey),
                RFC = e.RFCFirma,
                Firma = firmaDigital,
                Cadena = contenedor.cadenaAFirmar,
                Contenedor = certificado,
                SelloCNA = selloCNA,
                FechaRegistro = DateTime.Now
            };

            // Guardar firma
            AnalisisFirma_BL firmaBL = new AnalisisFirma_BL();
            firmaBL.Guarda(args);

            // Marcar registro como firmado
            Analisis_BL analisisBL = new Analisis_BL();
            var analisis = analisisBL.Obten_Analisis(args.AnalisisID);
            analisis.Firmada = true;
            analisis.FechaFirma = DateTime.Now;
            // SaveChanges...
        }
        else
        {
            // Manejar error: contenedor.mensajeError
        }
    }
}
```

### Paso 4: Incluir firma en el documento PDF/Reporte

```csharp
// Al generar el reporte, pasar el sello como parámetro
string selloDigitalCNA = analisis.AnalisisFirmas
    .Where(a => a.AnalisisID == analisis.AnalisisID)
    .Last().SelloDigital;

ReportParameter paramSello = new ReportParameter("SelloCONAGUA", selloDigitalCNA);
reportViewer.LocalReport.SetParameters(paramSello);

// Para generar QR con la cadena de firma (en API de reportes)
string qr = analisis.Firmada 
    ? qrService.GenerarQR(analisis.AnalisisFirmas.FirstOrDefault()?.Cadena) 
    : "";
```

---

## Servicios Web Requeridos

### 1. WS IDTrust (GlobalTrust) — Firma y Sellado

**WSDL:** `http://firmaelectronica.conagua.gob.mx:9080/WsGlobalTrust/GlobalTrustService?wsdl`

**Operación:** `Firma`

| Parámetro | Tipo | Descripción |
|---|---|---|
| `id` | string | Identificador del certificado AC (`IdCertificadoWS`) |
| `numeroSerie` | string | Número de serie del certificado AC |
| `cadenaOriginal` | byte[] | Cadena original en UTF8 |
| `algHash` | string | Algoritmo hash (SHA1) |
| `rfc` | string | RFC del certificado AC |

**Respuesta:**
| Campo | Descripción |
|---|---|
| `resultado.codigo` | 0 = éxito |
| `resultado.texto` | Mensaje de error si aplica |
| `firma` | Sello digital resultante |

### 2. WS Sellado SIRFIC (Alternativo)

**Endpoint:** `http://firmaelectronica.conagua.gob.mx:9080/WS_SELLADORA/ServicioSellado.asmx`

**Operación:** `Generar(string cadenaOriginal)` → retorna string con el sello

---

## Agregar Service Reference en Visual Studio

1. Click derecho en el proyecto → "Add Service Reference"
2. Para IDTrust: URL = `http://firmaelectronica.conagua.gob.mx:9080/WsGlobalTrust/GlobalTrustService?wsdl`, Namespace = `wsIDTrust`
3. Para Sellado: URL = `http://firmaelectronica.conagua.gob.mx:9080/WS_SELLADORA/ServicioSellado.asmx?wsdl`, Namespace = `SelladoSirficWS`

---

## Checklist para Replicar

- [ ] Agregar paquete NuGet `BouncyCastle` 1.8.6.1
- [ ] Agregar paquete NuGet `AjaxControlToolkit`
- [ ] Copiar `FIEL.dll` al directorio `bin/` o agregar como referencia el proyecto FIEL
- [ ] Crear carpeta `FirmaElectronica/` y copiar las 7 clases
- [ ] Agregar Service Reference `wsIDTrust` (WS GlobalTrust)
- [ ] Agregar Service Reference `SelladoSirficWS` (WS Sellado)
- [ ] Configurar `<appSettings>` con datos del certificado AC
- [ ] Configurar `<system.serviceModel>` con endpoints de los WS
- [ ] Crear tabla `AnalisisFirma` en BD (o equivalente)
- [ ] Registrar el control `wucAppletFIEL` en las páginas que requieran firma
- [ ] Implementar evento `FirmarColeccion` en el code-behind
- [ ] Validar que el servidor tenga acceso de red al WS de firma electrónica
