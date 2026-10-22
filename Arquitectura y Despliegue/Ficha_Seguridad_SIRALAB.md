# Ficha de Seguridad — SIRALAB

## Sistema de Recepción de Análisis de Laboratorios

| # | Campo | Valor |
|---|---|---|
| 1 | **Nombre del sistema** | SIRALAB — Sistema de Recepción de Análisis de Laboratorios |
| 2 | **Criticidad** | Alta |
| 3 | **Justificación de la criticidad** | El sistema procesa datos personales (RFC, CURP, nombre, dirección, correo), gestiona firmas electrónicas (FIEL) de contribuyentes y laboratorios, y alimenta información que se utiliza para la evaluación de cumplimiento de normas ambientales con implicaciones fiscales y legales (Ley Federal de Derechos). Además se integra con sistemas de misión crítica de CONAGUA (SIRFIC, Declar@gua). |
| 4 | **¿El sistema procesa o almacena Datos Personales?** | Sí — Nombre, Apellidos, RFC, CURP, Correo electrónico, Dirección (calle, número, CP, colonia, municipio, estado), Teléfonos de contacto |
| 5 | **¿El sistema procesa o almacena Datos Personales Sensibles?** | No — No almacena datos de salud, orientación sexual, religión, origen étnico ni datos biométricos |
| 6 | **¿El sistema procesa o almacena Datos Patrimoniales o Financieros?** | No directamente — Sin embargo, los resultados de análisis tienen implicaciones fiscales (acreditamientos, exenciones y descuentos para efectos del pago de derechos) |
| 7 | **Número de cuentas totales en el sistema** | Ejecutar query: `SELECT COUNT(*) AS TotalCuentas FROM dbo.Usuario WHERE Activo = 1` en base de datos `Siralab_Produccion_5` |
| 8 | **Unidad Responsable** | Subdirección General Técnica — Gerencia de Calidad del Agua / CONAGUA |
| 9 | **¿Existen cuentas privilegiadas?** | Sí — Rol `Administrador_SIRALAB` (UsuarioRolID = 5) con acceso completo a catálogos, módulos, páginas y permisos |
| 10 | **Número total de cuentas privilegiadas** | Ejecutar query: `SELECT COUNT(*) AS CuentasPrivilegiadas FROM dbo.Usuario WHERE UsuarioRolID = 5 AND Activo = 1` |
| 11 | **¿Existen cuentas de terceros?** | Sí — Los laboratorios acreditados externos (Rol `Laboratorio`, UsuarioRolID = 1) son entidades terceras que acceden al sistema para capturar análisis |
| 12 | **Número total de cuentas de terceros** | Ejecutar query: `SELECT COUNT(*) AS CuentasTerceros FROM dbo.Usuario WHERE UsuarioRolID = 1 AND Activo = 1` |
| 13 | **Número de cuentas protegidas con MFA** | 0 — No existe implementación de MFA en el sistema |
| 14 | **Solución MFA** | No implementada |
| 15 | **Primer método de autenticación** | Usuario y contraseña almacenada con encriptación simétrica en base de datos (tabla `Usuario`, campo `Contrasenia`) |
| 16 | **Segundo método de autenticación** | No existe — Para el servicio WCF externo se utiliza un Token estático almacenado en `LaboratorioAcreditacion.Token`, pero no constituye un segundo factor de autenticación |
| 17 | **¿Existe solución PAM?** | No |
| 18 | **Solución PAM** | No implementada |
| 19 | **Solución PAM integrada con MFA** | No aplica |
| 20 | **Número de cuentas privilegiadas administradas por PAM** | 0 |
| 21 | **Número de cuentas restantes por implementar MFA** | Todas las cuentas activas. Query: `SELECT COUNT(*) FROM dbo.Usuario WHERE Activo = 1` |
| 22 | **Número de cuentas privilegiadas restantes por administrar en PAM** | Todas las cuentas privilegiadas. Query: `SELECT COUNT(*) FROM dbo.Usuario WHERE UsuarioRolID = 5 AND Activo = 1` |
| 23 | **Gestión del ciclo de vida de identidades** | **Creación:** Manual por Administrador SIRALAB desde la interfaz web (catálogo de usuarios). **Desactivación:** Campo `Activo = false` en tabla `Usuario`; para laboratorios se valida adicionalmente vigencia de aprobación y representante activo. **Eliminación:** No se eliminan registros, solo se desactivan (borrado lógico). No existe integración con Directorio Activo ni LDAP. No hay proceso automatizado de depuración de cuentas inactivas. |
| 24 | **Controles compensatorios implementados** | 1) Validación de sesión activa en cada página mediante `Application.master` (redirige a Login si `Session.Count == 0`). 2) Control de acceso basado en roles (RBAC) con permisos por página y módulo (`CatPermisosRol`). 3) Permisos de lectura/escritura por rol y página. 4) Validación de vigencia de aprobación y representante activo para laboratorios. 5) Contraseñas almacenadas con encriptación (no en texto plano). 6) Timeout de sesión configurado a 540 minutos. 7) Token de acceso para servicios WCF externos. 8) Bitácora de operaciones con firma electrónica (`TBitacora`). |
| 25 | **Fecha compromiso para implementar MFA** | Pendiente de definir |
| 26 | **Observaciones** | **Hallazgos críticos de seguridad:** 1) La base de datos se accede con usuario `sa` (superadministrador SQL Server), violando el principio de mínimo privilegio. 2) Las cadenas de conexión con credenciales están en texto plano en `Web.config` y `appsettings.json`. 3) No existe bloqueo de cuenta tras intentos fallidos de autenticación. 4) No hay políticas de complejidad ni expiración de contraseñas en código. 5) `UseHttpsRedirection` está comentado en el API de Reportes. 6) `UseDeveloperExceptionPage` está habilitado sin condición de entorno. 7) El API de Reportes no tiene autenticación ni autorización (`UseAuthorization` comentado). 8) No hay registro de intentos fallidos de login ni logs de auditoría de acceso. 9) No hay integración con Directorio Activo ni proveedor de identidad centralizado. 10) La encriptación de contraseñas es simétrica (reversible vía `Crypt.Desencriptar`), no se usa hashing unidireccional. |

---

## Roles del Sistema

| UsuarioRolID | Nombre | Tipo | Descripción |
|---|---|---|---|
| 1 | Laboratorio | Tercero (externo) | Laboratorios acreditados que capturan análisis |
| 2 | Recaudación | Interno | Personal de CONAGUA que gestiona órdenes de trabajo |
| 3 | Calidad | Interno | Control de calidad |
| 4 | Administrador_PALA | Interno | Administrador de PALA |
| 5 | Administrador_SIRALAB | Privilegiado | Acceso total al sistema, catálogos y permisos |
| 7 | Habilitar | Interno | Habilitación de análisis |

---

## Datos Personales Identificados

| Entidad | Campos con Datos Personales |
|---|---|
| `Cliente` | Nombre, ApellidoPaterno, ApellidoMaterno, RFC, CURP, Correo |
| `Persona` | Nombre, ApellidoPaterno, ApellidoMaterno, RFC, CURP |
| `Usuario` | Usuario1 (RFC), Contrasenia, Correo |
| `Contacto` | Contacto1 (teléfono/correo), Extension |
| `Direccion` | Calle, NumeroExterior, NumeroInterior, CP |
| `LaboratorioRepresentante` | Vinculado a Persona (datos del representante legal) |
| `LaboratorioSignatario` | Vinculado a Persona (datos del signatario) |

---

## Mecanismos de Autenticación por Componente

| Componente | Mecanismo | Detalle |
|---|---|---|
| Aplicación Web (WebForms) | Sesión basada en cookies (InProc) | Usuario/contraseña validados contra BD, sesión almacenada en memoria del servidor |
| API Reportes (ASP.NET Core) | Ninguno | Sin autenticación ni autorización configurada |
| Servicio WCF (Laboratorios) | Token estático | `TokenDatamodel.Token` validado contra `LaboratorioAcreditacion.Token` en BD |
| Web Service SIRFIC | Ninguno visible | Comunicación interna sin autenticación adicional |
