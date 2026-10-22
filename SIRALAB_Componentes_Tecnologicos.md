# SIRALAB - Información de Componentes Tecnológicos

## Tipo de Sistema
**Web / API / Servicio Windows**

El proyecto SIRALAB es una solución multi-componente que incluye:
- **Aplicación Web ASP.NET** (componente principal)
- **Web Services ASMX** (servicios web legacy)
- **WCF Services** (servicios de comunicación)
- **Servicio Windows** (DepuraACE)
- **API REST** (Infotec.SIRALAB.ReportesWS)
- **Base de Datos SQL Server**

---

## Framework y Plataforma Base

### .NET Framework
- **Versión:** .NET Framework 4.5
- **Estado:** ⚠️ **END OF LIFE (EOL)**
- **Fecha fin de soporte:** 12 de enero de 2016
- **Recomendación:** Migrar a .NET Framework 4.8 (soporte hasta 2028) o .NET 6/8 LTS

---

## Listado de Componentes Open Source

### Librerías de Terceros (NuGet Packages)

#### 1. **AjaxControlToolkit**
- **Versión:** 18.1.0
- **Licencia:** BSD-3-Clause
- **Estado:** ⚠️ Última versión estable, pero con soporte limitado
- **Uso:** Controles AJAX para ASP.NET WebForms

#### 2. **BouncyCastle / BouncyCastle-Ext**
- **Versión:** 1.8.6.1 / 1.7.0
- **Licencia:** MIT
- **Estado:** ⚠️ Versiones desactualizadas (actual: 2.x)
- **Uso:** Criptografía y manejo de certificados digitales (FIEL)

#### 3. **EPPlus**
- **Versión:** 4.0.5
- **Licencia:** LGPL (versiones antiguas)
- **Estado:** ⚠️ **CAMBIO DE LICENCIA** - Versiones 5.0+ requieren licencia comercial
- **Uso:** Generación y manipulación de archivos Excel

#### 4. **iTextSharp**
- **Versión:** 5.5.13.2
- **Licencia:** AGPL v3
- **Estado:** ⚠️ **END OF LIFE** - Descontinuado en 2016
- **Fecha fin de soporte:** 2016
- **Uso:** Generación de documentos PDF
- **Recomendación:** Migrar a iText 7+ (requiere licencia comercial) o alternativas como QuestPDF

#### 5. **itextsharp.xmlworker**
- **Versión:** 5.5.13.2
- **Licencia:** AGPL v3
- **Estado:** ⚠️ **END OF LIFE** - Descontinuado en 2016
- **Fecha fin de soporte:** 2016
- **Uso:** Conversión de HTML a PDF

#### 6. **EntityFramework**
- **Versión:** 5.0.0 (DAL) / 6.2.0 (DepuraACE)
- **Licencia:** Apache 2.0
- **Estado:** ⚠️ EF 5.0 está obsoleto, EF 6.2.0 aún tiene soporte
- **Uso:** ORM para acceso a datos

#### 7. **Newtonsoft.Json**
- **Versión:** 6.0.4
- **Licencia:** MIT
- **Estado:** ⚠️ Versión muy antigua (actual: 13.x)
- **Uso:** Serialización/deserialización JSON

#### 8. **RestSharp**
- **Versión:** 105.2.3
- **Licencia:** Apache 2.0
- **Estado:** ⚠️ Versión muy antigua (actual: 110.x)
- **Uso:** Cliente HTTP para consumo de APIs REST

#### 9. **ZXing.Net**
- **Versión:** 0.16.6
- **Licencia:** Apache 2.0
- **Estado:** ✅ Activo (versión relativamente reciente)
- **Uso:** Generación de códigos QR y códigos de barras

#### 10. **Microsoft.ReportViewer**
- **Versión:** 11.0.0.0 / 150.1449.0 / 150.1484.0
- **Licencia:** Microsoft
- **Estado:** ⚠️ Versiones mixtas (11.0 es muy antigua)
- **Uso:** Generación de reportes RDLC

#### 11. **Microsoft.SqlServer.Types**
- **Versión:** 14.0.314.76
- **Licencia:** Microsoft
- **Estado:** ✅ Versión SQL Server 2017, aún soportada
- **Uso:** Tipos de datos espaciales de SQL Server

#### 12. **Microsoft.AspNet.Mvc**
- **Versión:** 5.2.9
- **Licencia:** Apache 2.0
- **Estado:** ✅ Última versión de ASP.NET MVC 5
- **Uso:** Framework MVC para ASP.NET

#### 13. **Microsoft.Office.Interop.Word**
- **Versión:** 15.0.4797.1004
- **Licencia:** Microsoft
- **Estado:** ⚠️ Requiere Office instalado en servidor
- **Uso:** Generación de documentos Word

#### 14. **Bootstrap**
- **Versión:** 3.3.4
- **Licencia:** MIT
- **Estado:** ⚠️ Versión antigua (actual: 5.x)
- **Uso:** Framework CSS para interfaz de usuario

#### 15. **jQuery**
- **Versión:** 1.9.1
- **Licencia:** MIT
- **Estado:** ⚠️ Versión muy antigua (actual: 3.x)
- **Uso:** Librería JavaScript

---

## Componentes con END OF LIFE (EOL)

### ⚠️ CRÍTICO - Componentes EOL Identificados:

| Componente | Versión Actual | Fecha Fin de Soporte | Riesgo |
|------------|----------------|----------------------|--------|
| **.NET Framework 4.5** | 4.5 | **12 de enero de 2016** | 🔴 CRÍTICO |
| **iTextSharp** | 5.5.13.2 | **2016** | 🔴 ALTO |
| **itextsharp.xmlworker** | 5.5.13.2 | **2016** | 🔴 ALTO |
| **EntityFramework 5.0** | 5.0.0 | **2016** | 🟡 MEDIO |
| **Microsoft.ReportViewer 11.0** | 11.0.0.0 | **2016** | 🟡 MEDIO |
| **jQuery** | 1.9.1 | **2013** | 🟡 MEDIO |
| **Bootstrap** | 3.3.4 | **2019** | 🟡 BAJO |

---

## FECHA FIN DE SOPORTE FABRICANTE (ÚLTIMA FECHA)

### 🔴 **12 de enero de 2016**

**Componente crítico:** .NET Framework 4.5

Esta es la fecha más crítica ya que el framework base de toda la aplicación dejó de recibir soporte de Microsoft hace más de 8 años.

---

## Tecnologías Adicionales Identificadas

### Base de Datos
- **SQL Server** (versión no especificada en archivos revisados)
- **Entity Framework** como ORM

### Servicios Web
- **ASMX Web Services** (tecnología legacy)
- **WCF Services** (Windows Communication Foundation)
- **SOAP Services**

### Componentes de Interfaz
- **ASP.NET WebForms** (tecnología legacy)
- **ASP.NET MVC 5**
- **AJAX Control Toolkit**
- **jqGrid** (grillas JavaScript)

### Seguridad y Firma Electrónica
- **FIEL (Firma Electrónica)** - Applet Java
- **BouncyCastle** para criptografía
- **Certificados digitales X.509**

---

## Recomendaciones Prioritarias

### 🔴 URGENTE
1. **Actualizar .NET Framework 4.5 → 4.8** (mínimo)
2. **Reemplazar iTextSharp 5.x** por alternativa soportada
3. **Actualizar EntityFramework 5.0 → 6.4.4**

### 🟡 IMPORTANTE
4. Actualizar Newtonsoft.Json a versión reciente
5. Actualizar RestSharp a versión actual
6. Actualizar jQuery y Bootstrap
7. Evaluar migración de WebForms a tecnología moderna

### 🟢 RECOMENDADO
8. Considerar migración a .NET 6/8 LTS a largo plazo
9. Modernizar servicios ASMX a Web API REST
10. Evaluar alternativas a Microsoft.Office.Interop.Word

---

**Fecha de análisis:** Marzo 2026  
**Proyecto:** SIRALAB  
**Organización:** CONAGUA / INFOTEC
