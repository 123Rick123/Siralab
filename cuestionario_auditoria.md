# Cuestionario de Auditoría de Sistemas

Cuando el usuario te comparta este archivo, analiza el proyecto abierto en el workspace y responde cada pregunta con: **Sí**, **No**, **Parcial**, **No aplica**, o **⚠️** (cuando no sea visible desde el código y requiera que el usuario lo confirme).

Basa tus respuestas en el código fuente, configuraciones, estructura del repositorio y archivos del proyecto. Marca con ⚠️ solo lo que genuinamente no puedas determinar desde el código.

---

## 1. Componentes Tecnológicos
- ¿Se especifican tecnologías utilizadas en cada componente?
- ¿El diagrama está actualizado y corresponde a la versión actual del sistema?
- ¿Incluye flujo de datos entre componentes?
- ¿Considera aspectos de seguridad (firewalls, autenticación, etc.)?

## 2. GitLab / Control de Versiones
- ¿El proyecto se encuentra en un repositorio de GitLab?
- ¿Se ha creado el archivo CHANGELOG.md?
- ¿Se utilizan ramas (branches) para desarrollo feature / bugfix?
- ¿Los cambios están documentados mediante commits descriptivos?
- ¿Se gestionan versiones etiquetadas (tags/releases) conforme a la númeración SEMVER?
- ¿Se controlan accesos y permisos al repositorio?
- ¿Se controlan los accesos a través de roles?
- ¿Se cuentan con ramas de producción/main, qa y developer?

## 3. Seguridad y Autenticación
- ¿Existe un mecanismo de autenticación para el acceso al sistema?
- ¿Se implementa control de acceso basado en roles (RBAC)?
- ¿Los usuarios solo tienen acceso a los recursos necesarios (principio de mínimo privilegio)?
- ¿Se cuenta con políticas de contraseñas seguras (mayúsculas, números, caracteres especiales): Longitud mínima?
- ¿Expiración periódica?
- ¿Se implementa bloqueo de cuenta tras intentos fallidos de acceso?
- ¿Se utilizan mecanismos de autenticación segura? (por ejemplo, HTTPS)
- ¿Las sesiones de usuario tienen tiempo de expiración definido?
- ¿Se previene el acceso no autorizado a recursos (validación en backend)?
- ¿Se cuenta con control de acceso a nivel de: Aplicación?
- ¿Se cuenta con control de acceso a nivel de: Base de datos?
- ¿Se cuenta con control de acceso a nivel de: Servidor?
- ¿Se registran eventos de acceso (logs de auditoría)?

## 4. HTML / Accesibilidad Web
- ¿El HTML utiliza correctamente etiquetas semánticas: `<header>`, `<nav>`, `<main>`, `<section>`, `<article>`, `<footer>`?
- ¿El contenido es comprensible para lectores de pantalla?
- ¿El código HTML ha sido validado con herramientas W3C (validator)?
- ¿El HTML está libre de errores críticos de validación?
- ¿El documento HTML declara correctamente el idioma: `<html lang="es">`?
- ¿Se incluyen metadato `<meta charset="UTF-8">`?
- ¿Se incluyen metadato `<meta name="viewport" content="width=device-width, initial-scale=1.0">`?
- ¿Se incluyen metadato `<meta name="description">`?

## 5. Pruebas Unitarias
- ¿Existen pruebas unitarias para los componentes críticos del sistema?
- ¿Las pruebas cubren lógica de negocio, servicios y utilidades?
- ¿Se utiliza un framework de pruebas (ej. NUnit, xUnit, MSTest, etc.)?
- ¿Las pruebas se ejecutan antes de liberar nuevas versiones?
- ¿Se documentan los casos de prueba relevantes?

## 6. Órdenes de Trabajo (OT)
- ¿Cada desarrollo o cambio está respaldado por una Orden de Trabajo (OT)?
- ¿La OT contiene: Descripción clara del requerimiento como; Justificación del cambio, Alcance definido, Fecha de solicitud y atención?
- ¿Los commits hacen referencia a la OT correspondiente?
- ¿Se puede identificar quién realizó cada cambio?
- ¿Se cuenta con control de versiones alineado a las OT?
- ¿Las OTs son aprobadas por el usuario antes de su implementación a producción?
- ¿Se registran evidencias de pruebas asociadas a la OT?
- ¿Existe seguimiento del estatus de cada OT (pendiente, en proceso, finalizada)?
- ¿Se asegura la reversibilidad del cambio (rollback si aplica)?

## 7. COIS
- ¿El sistema/proyecto está registrado en COIS?
- ¿El registro incluye información vigente, completa y actualizada del sistema?
- ¿Se identifica al responsable del sistema?
- ¿Se documenta el objetivo y alcance del sistema?
- ¿Se mantiene evidencia del registro (folio, captura, documento)?
- ¿Se actualiza el registro ante cambios relevantes?

## 8. Diccionario de Datos
- ¿Existe un diccionario de datos del sistema?
- ¿Se incluye definición de todas las tablas?
- ¿Se describe campos/columnas con: Nombre, Tipo de dato, Longitud y Descripción?
- ¿Se define llaves primarias y foráneas?
- ¿Se encuentra actualizado conforme a la BD?
- ¿Es accesible para el equipo técnico?

## 9. Diagrama Entidad-Relación
- ¿Existe diagrama entidad-relación (ER)?
- ¿Representa correctamente las entidades del sistema?
- ¿Se define relaciones entre tablas?
- ¿Se encuentra alineado con la base de datos real?

## 10. Diagrama de Arquitectura
- ¿Existe diagrama de arquitectura técnica documentado?
- ¿Se identifican componentes del sistema?
- ¿Muestra interacción entre capas (presentación, lógica, datos)?
- ¿Se incluye integraciones externas?
- ¿Refleja la infraestructura (servidores, servicios, red)?

## 11. Guiones de Prueba
- ¿Existen guiones de prueba documentados?
- ¿Se documentan resultados obtenidos?
- ¿Se evidencia ejecución de pruebas (capturas, logs, reportes)?
- ¿Se registran incidencias detectadas?
- ¿Se valida la corrección de errores?
- ¿Las pruebas están vinculadas a requerimientos u OT?
- ¿Se cuenta con aprobación de pruebas (si aplica)?

## 12. Bitácora de Cambios
- ¿Se cuenta con bitácora de cambios?
- ¿Se está registrando la fecha y hora del cambio?
- ¿Se asigna un número de folio o ID del cambio?
- ¿Se especifica el módulo, página o sección en la que se aplicó el cambio?
- ¿Se registra el nombre del usuario responsable?
- ¿Se registra la operación realizada en BD (Insert, Update, Delete)?
- ¿Se especificó el tipo de cambio? (correctivo, preventivo, mejora)
- ¿Se documenta la justificación o motivo del cambio?
- ¿Se registran intentos fallidos de acceso a la bitácora o al sistema?
- ¿Se detectan y reportan accesos no autorizados?
- ¿El acceso a la bitácora está restringido mediante roles y permisos?
- ¿Los registros están protegidos contra modificaciones o eliminaciones no autorizadas?
- ¿Se cuenta con mecanismos de integridad de los datos (hash, control de cambios, auditoría)?
- ¿Se garantiza que los registros no pueden ser alterados sin dejar evidencia?
- ¿El sistema registra de forma automática los eventos relevantes en la bitácora?
- ¿Se registra el usuario responsable de cada operación?
- ¿Se registra la dirección IP o identificador del equipo de origen?
- ¿Se registran cambios de estado o estatus dentro del sistema?
- ¿Se registran operaciones críticas realizadas por los usuarios?
- ¿Se registran errores técnicos del sistema o excepciones?
- ¿Las bitácoras se centralizan en una base de datos o repositorio único?
- ¿Los registros permiten la trazabilidad completa de las operaciones realizadas?
- ¿La bitácora permite buscar e identificar eventos por criterios como: fecha, usuario u otros filtros?
- ¿Solo personal autorizado tiene permisos de consulta?
- ¿Se registra un historial de consultas a la bitácora (logs de acceso)?
- ¿La bitácora se encuentra disponible para consulta cuando se requiere?
- ¿Los permisos de acceso están definidos por roles?
- ¿La bitácora está correctamente implementada y configurada en el sistema?
- ¿El sistema genera registros de eventos en la bitácora de forma automática?
- ¿Se realizan respaldos automáticos de la bitácora mediante tareas programadas en intervalos definidos?
- ¿Los respaldos se almacenan en un medio seguro y controlado?
- ¿La creación o modificación de la bitácora está respaldada por una Orden de Trabajo (OT)?

## 13. Pruebas Funcionales / Navegadores
- ¿La interfaz se muestra correctamente (estructura, menús, botones, formularios)?
- ¿Existen elementos desalineados, cortados o sobrepuestos?
- ¿Los estilos (colores, tipografías, iconos) se visualizan de manera uniforme en todos los navegadores?
- ¿Los enlaces y botones ejecutan correctamente su acción?
- ¿Los formularios permiten capturar, validar y enviar información sin errores?
- ¿El sistema permite navegar correctamente sin mostrar mensajes de error inesperados?
- ¿Las funcionalidades críticas operan correctamente en todos los navegadores evaluados?
- ¿Los tiempos de carga son adecuados?
- ¿El sistema opera sin bloqueos, congelamientos o cierres inesperados?
- ¿El sistema mantiene estabilidad durante la navegación continua?

## 14. Certificado Digital
- ¿El sistema cuenta con un certificado digital instalado y configurado correctamente?
- ¿El certificado corresponde al domicilio oficial del sistema?
- ¿La Autoridad Certificadora emisora es válida y reconocida?
- ¿El certificado se encuentra vigente?
- ¿El sistema se visualiza sin advertencias de seguridad en el navegador?
- ¿La cadena de certificación es válida y completa?
- ¿El sistema opera bajo protocolo seguro (https)?
- ¿El acceso al sistema no presenta alertas de conexión no segura?
- ¿Existe control y seguimiento de la fecha de expiración del certificado?

## 15. Gráfica Base / Identidad Institucional
- ¿El sistema cumple con la última versión vigente de la Guía de Gráfica Base?
- ¿El portal integra la cabecera oficial conforme a los lineamientos vigentes?
- ¿El pie de página cumple con la estructura y contenido institucional requerido?
- ¿Se respeta la jerarquía visual establecida para títulos, subtítulos y contenido?
- ¿Se utiliza la paleta de colores oficial?
- ¿La tipografía corresponde a la establecida en la normativa?
- ¿Los logotipos institucionales están actualizados y correctamente aplicados?
- ¿Los elementos gráficos utilizados corresponden a la identidad institucional?
- ¿Cumple con criterios de usabilidad y experiencia de usuario (UX)?
- ¿El diseño mantiene uniformidad con otros portales institucionales?
- ¿La implementación respeta criterios de accesibilidad?
- ¿La Gráfica Base se presenta conforme a los componentes oficiales autorizados?

## 16. Diseño Responsivo
- ¿El contenido se adapta automáticamente a diferentes tamaños de pantalla?
- ¿El contenido se ajusta adecuadamente a la pantalla sin desplazamiento horizontal innecesario?
- ¿Los textos son legibles sin necesidad de hacer zoom?
- ¿Las imágenes y elementos gráficos se ajustan proporcionalmente?
- ¿Los botones y enlaces son fácilmente seleccionables en dispositivos táctiles?
- ¿Los menús son accesibles y funcionales en versión móvil?
- ¿Los formularios pueden capturarse correctamente desde dispositivos móviles?
- ¿Las funcionalidades principales operan correctamente en dispositivos móviles?
- ¿El sistema funciona correctamente en teléfonos y tabletas sin presentar errores?
- ¿Los tiempos de carga son adecuados en redes móviles?

## 17. Accesibilidad
- ¿El sitio permite navegación mediante teclado?
- ¿Existe contraste adecuado entre texto y fondo?
- ¿Las imágenes cuentan con texto alternativo (atributo ALT)?
- ¿Los tamaños de fuente permiten adecuada legibilidad?
- ¿Los formularios cuentan con etiquetas claras y asociadas correctamente?
- ¿Los mensajes de error son comprensibles y orientan al usuario?
- ¿Los elementos del sistema son accesibles sin requerir exclusivamente el uso del mouse?
- ¿Los tiempos de sesión permiten completar acciones sin afectar a usuarios con limitaciones?
- ¿El sistema es compatible con lectores de pantalla?
- ¿La navegación y operación del sistema se realizan sin incidencias al usar herramientas de asistencia?
- ¿La estructura del código permite que tecnologías de apoyo, como lectores de pantalla y navegación por teclado, interpreten correctamente el contenido y la funcionalidad del sistema?

## 18. Aviso de Privacidad
- ¿El Aviso de Privacidad se encuentra visible y accesible desde el portal?
- ¿Está disponible antes de la recopilación de datos personales?
- ¿Puede consultarse fácilmente desde cualquier sección relevante del sistema?
- ¿Se especifica la identidad y domicilio del responsable?
- ¿Se informa sobre transferencias de datos, en su caso?
- ¿Se indica el fundamento legal que sustenta el tratamiento?
- ¿El sistema solicita consentimiento cuando es requerido?
- ¿Se implementan mecanismos para proteger los datos personales?
- ¿Existe procedimiento para actualización del Aviso de Privacidad cuando sea necesario?
