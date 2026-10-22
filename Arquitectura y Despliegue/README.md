# SIRALAB — Sistema de Recepción de Análisis de Laboratorios

Sistema desarrollado para la Comisión Nacional del Agua (CONAGUA) que permite recibir, gestionar y evaluar los reportes de análisis de calidad del agua realizados por laboratorios acreditados, verificando el cumplimiento de las Normas Oficiales Mexicanas aplicables a descargas de aguas residuales en cuerpos receptores.

---

## Tabla de Contenidos

- [Descripción](#descripción)
- [Requisitos Previos](#requisitos-previos)
- [Instalación](#instalación)
- [Uso](#uso)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Despliegue](#despliegue)
- [Tecnologías Utilizadas](#tecnologías-utilizadas)
- [Contribución](#contribución)
- [Licencia](#licencia)

---

## Descripción

SIRALAB gestiona el ciclo completo de recepción de análisis de laboratorio para CONAGUA:

- Recepción y validación de resultados de muestreo y análisis de agua
- Administración de órdenes de trabajo vinculadas a normas oficiales (NOM-001-SEMARNAT-2021, entre otras)
- Evaluación de parámetros de descargas en cuerpos receptores
- Generación de informes de laboratorio y reportes mensuales con firma electrónica (FIEL)
- Gestión de concesiones, anexos y medidores de contribuyentes
- Control de apercibimientos y reincidencias a laboratorios
- Integración con SIRFIC para datos de contribuyentes y concesiones
- Integración con sistema de firma electrónica (IDTrust / GlobalTrust)
- Integración con sistema documental Amara/Archiva

---

## Requisitos Previos

### Aplicación Web Principal (ASP.NET WebForms)

| Componente | Versión |
|---|---|
| .NET Framework | 4.5 |
| Visual Studio | 2017 o superior |
| IIS | 7.5 o superior |
| SQL Server | 2012 o superior |
| Entity Framework | 5.0 |

### API de Reportes (ASP.NET Core)

| Componente | Versión |
|---|---|
| .NET SDK | 5.0 |
| Visual Studio | 2019 o superior |

### Servicios Externos Requeridos

- SQL Server con base de datos `Siralab_Produccion_5`
- SQL Server con base de datos `CNA_SIRFIC_PRODUC2` (integración SIRFIC)
- Servidor SMTP (puerto 25) para envío de correos
- Servicio de Firma Electrónica (IDTrust / GlobalTrust)
- Servicio de Sellado de Tiempo
- SQL Server Reporting Services (SSRS)
- Servicio documental Amara/Archiva
- Web Service WsSiralabSirfic
- Web Service FileTransfer

---

## Instalación

### 1. Clonar el repositorio

```bash
git clone <url-del-repositorio>
git checkout develop
```

### 2. Restaurar paquetes NuGet

Abrir la solución `Infotec.SIRALAB.sln` en Visual Studio y restaurar paquetes NuGet automáticamente, o ejecutar:

```bash
nuget restore Infotec.SIRALAB.sln
```

### 3. Configurar cadenas de conexión

**Aplicación Web Principal** — Editar `SIRALAB/Web.config`:

```xml
<connectionStrings>
    <add name="Siralab_Entities" connectionString="..." />
    <add name="CNA_SIRFICEntities" connectionString="..." />
    <add name="SiralabDbContext" connectionString="..." />
</connectionStrings>
```

**API de Reportes** — Editar `API/Infotec.SIRALAB.ReportesWS/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<servidor>;Database=<base>;User ID=<usuario>;Password=<contraseña>;Connection Timeout=1800"
  }
}
```

### 4. Configurar variables de entorno

Editar `SIRALAB/WebApplication.config` con:

- Credenciales SMTP
- URLs de servicios externos (IDTrust, Amara, SSRS, FileTransfer)
- Rutas de archivos y reportes
- Configuraciones de certificados de firma electrónica

### 5. Compilar la solución

```bash
msbuild Infotec.SIRALAB.sln /p:Configuration=Release
```

---

## Uso

### Desarrollo

1. Abrir `Infotec.SIRALAB.sln` en Visual Studio
2. Establecer `SIRALAB` como proyecto de inicio para la aplicación web
3. Establecer `Infotec.SIRALAB.ReportesWS` como proyecto de inicio para la API de reportes
4. Ejecutar con IIS Express (F5)

| Componente | URL Local |
|---|---|
| Aplicación Web | `http://localhost:<puerto>/Login.aspx` |
| API Reportes | `http://localhost:<puerto>/swagger` |

### QA

- Rama: `Qa`
- Servidor: Según configuración institucional

### Producción

- Rama: `master`
- Servidor de aplicación: IIS en servidor institucional
- API Reportes: `http://172.29.209.54/ReportesSiralab/`
- Reporting Services: `http://172.29.209.11/ReportServer`

---

## Estructura del Proyecto

```
Infotec.SIRALAB.sln
│
├── SIRALAB/                          # Aplicación Web Principal (ASP.NET WebForms .NET 4.5)
│   ├── App_Start/                    # Configuración de inicio
│   ├── App_Themes/                   # Temas visuales
│   ├── ClienteAPI/                   # Clientes de APIs externas
│   ├── Common/                       # Clases comunes (seguridad, utilidades)
│   ├── Controles/                    # Controles de usuario (.ascx)
│   ├── Entidades/                    # Entidades de transferencia
│   ├── FirmaElectronica/             # Módulo de firma electrónica (FIEL)
│   ├── js/                           # Scripts JavaScript
│   ├── Reportes/                     # Reportes RDLC locales
│   ├── Service References/           # Referencias a servicios WCF/ASMX
│   ├── style/                        # Hojas de estilo CSS
│   ├── Login.aspx                    # Página de login
│   ├── Main.Master                   # Master page principal
│   ├── Application.master            # Master page de la aplicación autenticada
│   ├── Web.config                    # Configuración principal
│   └── WebApplication.config         # Configuración de aplicación (appSettings)
│
├── SIRALAB_BL/                       # Capa de Lógica de Negocio
│   ├── negocio/                      # Lógica de negocio por módulo
│   │   ├── analisis/                 # Gestión de análisis
│   │   ├── cancelacion/              # Solicitudes de cancelación
│   │   ├── login/                    # Autenticación
│   │   └── ordentrabajo/             # Órdenes de trabajo
│   ├── seguridad/                    # Control de acceso (roles, permisos, páginas)
│   ├── catalogos/                    # Operaciones de catálogos
│   └── correo/                       # Envío de correos electrónicos
│
├── SIRALAB_DAL/                      # Capa de Acceso a Datos (Entity Framework 5)
│   ├── AccesoEntidades/              # Repositorio genérico
│   ├── Extensiones/                  # Métodos de extensión
│   ├── SIRALAB_Model.edmx            # Modelo Entity Framework (EDMX)
│   └── *.cs                          # Entidades generadas
│
├── API/
│   ├── Infotec.SIRALAB.ReportesWS/   # API REST de Reportes (ASP.NET Core 5.0)
│   │   ├── Controllers/              # Controladores API
│   │   ├── Reportes/                 # Plantillas Razor para reportes PDF
│   │   └── Startup.cs                # Configuración del API
│   └── Infotec.SIRALAB.ReportesWS.DAL/  # DAL del API (EF Core)
│
├── SIRALAB_WCF/                      # Servicio WCF para laboratorios externos
│   ├── Implementacion/               # Implementación del servicio
│   ├── Modelos/                      # Modelos de datos WCF
│   └── Enumeraciones/                # Enumeraciones compartidas
│
├── SIRALAB_WS_FileTransfer/          # Web Service ASMX para transferencia de archivos
│
├── SIRALABService/                   # Servicio WCF adicional
│
├── SIRALAB_SIRFIC/                   # Integración con SIRFIC (contribuyentes/concesiones)
│
├── WS_SIRALAB_SIRFIC/                # Web Service de integración SIRFIC
│   ├── BL_WS_SIRALAB/                # Lógica de negocio del WS
│   ├── DL_WS_SIRALAB/                # Acceso a datos del WS
│   └── COM_WS_SIRALAB/               # Componentes comunes del WS
│
├── FIEL/                             # Librería de Firma Electrónica
│
├── SIRALAB_RS/                       # Reportes SQL Server Reporting Services (.rdl)
│
├── DepuraACE/                        # Servicio Windows de depuración de colas
│
├── SIRALAB_FIRMA/                    # Librería de firma digital
│
└── ArchivosComunes/                  # DLLs compartidas (OpenXml, QR, MimeKit)
```

---

## Despliegue

### Aplicación Web Principal (IIS)

1. Publicar desde Visual Studio o línea de comandos:

```bash
msbuild SIRALAB/SIRALAB.csproj /p:Configuration=Release /p:DeployOnBuild=true /p:PublishProfile=<perfil>
```

2. Configurar sitio en IIS:
   - Application Pool: .NET Framework 4.5, modo integrado
   - Binding: Puerto 80/443 según certificado
   - Documento predeterminado: `Login.aspx`

3. Configurar permisos de escritura en carpetas de archivos temporales y evidencias.

### API de Reportes (IIS)

1. Publicar:

```bash
dotnet publish API/Infotec.SIRALAB.ReportesWS -c Release -o ./publish
```

2. Configurar sitio en IIS:
   - Application Pool: Sin código administrado (.NET Core)
   - Instalar ASP.NET Core Hosting Bundle en el servidor

### Servicio Windows (DepuraACE)

1. Compilar en modo Release
2. Instalar con `installutil`:

```bash
installutil DepuraACE.exe
```

### Variables de Entorno de Producción

| Variable/Config | Descripción |
|---|---|
| `ConnectionStrings:Siralab_Entities` | Cadena de conexión a BD principal |
| `ConnectionStrings:CNA_SIRFICEntities` | Cadena de conexión a SIRFIC |
| `smtpServer` | Servidor SMTP |
| `smtpPort` | Puerto SMTP |
| `RSServidorURL` | URL de Reporting Services |
| `urlApiReportes` | URL del API de reportes |
| `UrlAmaraArchiva` | URL del servicio documental |

---

## Tecnologías Utilizadas

| Tecnología | Uso |
|---|---|
| ASP.NET WebForms 4.5 | Aplicación web principal |
| ASP.NET Core 5.0 | API REST de reportes |
| Entity Framework 5 (EDMX) | ORM - Aplicación principal |
| Entity Framework Core | ORM - API de reportes |
| WCF | Servicios para laboratorios externos |
| ASMX Web Services | Transferencia de archivos |
| SQL Server | Base de datos relacional |
| SQL Server Reporting Services | Reportes institucionales |
| iTextSharp / iText7 | Generación de PDFs |
| Razor Templating | Plantillas HTML para reportes |
| BouncyCastle | Criptografía y firma electrónica |
| AjaxControlToolkit | Componentes UI |
| Bootstrap 3 | Framework CSS (vía Gráfica Base) |
| jQuery 1.12 | Manipulación DOM y AJAX |
| EPPlus | Generación de archivos Excel |
| RestSharp | Cliente HTTP |
| Newtonsoft.Json | Serialización JSON |
| Gráfica Base (CDN gob.mx) | Identidad institucional |
| Google Analytics | Métricas de uso |
| IIS | Servidor web |

---

## Contribución

Este proyecto sigue la política **CNA-GTIC-SS-POL-05** de Gestión, Gobernanza y Control de Repositorio de Código Fuente.

### Ramas

| Rama | Propósito |
|---|---|
| `master` | Producción — código estable desplegado |
| `Qa` | Aseguramiento de calidad — pruebas previas a producción |
| `develop` | Desarrollo — integración de cambios |

### Flujo de trabajo

1. Crear rama desde `develop` con nomenclatura descriptiva (feature/bugfix)
2. Desarrollar y hacer commits descriptivos referenciando la OT cuando aplique
3. Crear Merge Request hacia `develop`
4. Revisión de código por parte de un par
5. Merge a `develop` → pruebas en ambiente de desarrollo
6. Merge a `Qa` → pruebas de QA
7. Merge a `master` → despliegue a producción

### Convención de commits

```
<tipo>: <descripción breve>

Tipos: feat, fix, refactor, docs, style, chore
Ejemplo: fix: incidencia caudal diferente en análisis
```

### Reglas

- No se permite push directo a `master` ni a `Qa`
- Todo cambio debe pasar por Merge Request
- Las ramas principales están protegidas
- Los commits deben ser descriptivos y atómicos

---

## Licencia

Todos los derechos patrimoniales de este software pertenecen al **Estado Mexicano**, representado por la **Comisión Nacional del Agua (CONAGUA)**.

Queda prohibida la reproducción, distribución o uso de este código fuente sin la autorización expresa de CONAGUA. El desarrollo y mantenimiento es realizado por INFOTEC — Centro de Investigación e Innovación en Tecnologías de la Información y Comunicación.
