# Documento de Arquitectura Técnica  
## Proyecto: Captura y Envío de Datos de Uso desde Aplicaciones VB6 a Webservice .NET 9

---

### 1. **Resumen Ejecutivo**

El presente documento describe la arquitectura para la modificación de aplicaciones de escritorio desarrolladas en Visual Basic 6 (VB6), con el objetivo de capturar ciertos datos de uso y enviarlos de forma segura y eficiente a un webservice desarrollado en .NET 9, alojado en una máquina virtual Windows en Azure bajo IIS. Todo el proceso se encuentra alineado con las mejores prácticas de seguridad y performance, garantizando que no se capturen datos sensibles de los usuarios y que no se degrade la experiencia de uso de las aplicaciones.

---

### 2. **Arquitectura General**

#### 2.1 **Componentes Principales**

- **Aplicaciones de Escritorio VB6**  
  Software instalado y ejecutado en los equipos de los clientes. Modificado para capturar eventos de uso seleccionados.

- **Módulo de Telemetría (VB6)**  
  Nuevo componente integrado a las apps existentes. Encargado de recolectar, formatear y enviar los datos de uso.

- **Webservice/API (.NET 9)**  
  Servicio web RESTful que recibe, valida y almacena los datos de uso. Desarrollado en .NET 9 y desplegado en IIS sobre una VM en Azure.

- **Servidor IIS en Azure**  
  Infraestructura que aloja y ejecuta el webservice/API, garantizando alta disponibilidad y seguridad de acceso.

- **Base de Datos de Telemetría**  
  Almacena la información de uso recibida para futuras consultas, reportes y análisis.

#### 2.2 **Diagrama de Arquitectura**

```plaintext
+--------------------+          HTTPS           +-----------------------+        +---------------------+
|                    |  (1) Solicitud POST     |                       |        |                     |
|  App Escritorio    |------------------------>|  Webservice/API (.NET)|------->|   Base de Datos     |
|  VB6 + Módulo      |   (Con datos uso,       |      en IIS/Azure     |        |   Telemetría        |
|  Telemetría        |   anonimizados)         |                       |        |                     |
+--------------------+                        +-----------------------+        +---------------------+
         |                                            ^
         |--------------------------------------------|
           (2) Respuesta HTTP (éxito/error)
```

---

### 3. **Proceso de Captura y Envío de Datos**

#### 3.1 **Captura de Datos en la App VB6**

- Se identifican los eventos relevantes de uso (ej: inicio/cierre de sesión, ejecución de funciones clave, errores críticos).
- El Módulo de Telemetría recolecta únicamente información **no sensible** y **no identificable** del usuario (ej: versión del software, timestamp, tipo de evento, identificador anónimo de máquina generado localmente).
- Se establece una cola/buffer local para garantizar que los datos se puedan reenviar en caso de fallos temporales de red.

#### 3.2 **Envío al Webservice (.NET 9)**

- El Módulo de Telemetría empaqueta los datos en formato JSON.
- Se realiza una llamada HTTPS (POST) al endpoint del Webservice, asegurando la confidencialidad e integridad de la información.
- El envío se realiza en segundo plano (thread asíncrono), evitando cualquier impacto en el rendimiento de la aplicación principal.

#### 3.3 **Procesamiento en el Webservice**

- El API recibe el payload, valida el esquema y la ausencia de datos sensibles.
- Los datos válidos se almacenan en la base de datos de telemetría para su posterior análisis.
- El API responde con un código de éxito o error según corresponda.

---

### 4. **Consideraciones de Seguridad y Privacidad**

- **Anonimización:**  
  No se capturan nombres de usuario, direcciones IP externas, ni ninguna información que permita identificar a usuarios finales.
- **Cifrado:**  
  Todo el tráfico entre las aplicaciones y el webservice se realiza mediante HTTPS.
- **Validación:**  
  El Webservice implementa validaciones estrictas para rechazar cualquier dato que potencialmente sea identificable.

---

### 5. **Consideraciones de Performance**

- **Operación Asíncrona y No Bloqueante:**  
  El envío de datos se realiza fuera del hilo principal de la aplicación, asegurando que no haya demoras perceptibles para el usuario final.
- **Batching y Reintentos:**  
  Los datos pueden agruparse y enviarse en lotes, así como reenviarse automáticamente en caso de errores temporales de red.

---

### 6. **Ventajas de la Arquitectura Propuesta**

- Modularidad y bajo acoplamiento entre componentes.
- Facilidad de despliegue y mantenimiento.
- Cumplimiento normativo respecto a privacidad.
- Escalabilidad y monitoreo centralizado en Azure.

---

### 7. **Anexo: Ejemplo de Payload de Telemetría**

```json
{
  "appVersion": "6.5.2",
  "eventType": "ModuloX_Ejecutado",
  "timestamp": "2025-07-13T14:33:00Z",
  "machineId": "a3f6b7c1-e123-4a9d-8f90-0e1a2b3c4d5e",
  "additionalData": {
    "modulo": "Facturacion",
    "duracionSegundos": 35
  }
}
```

---

**El presente documento puede ser utilizado como parte de la documentación oficial del área de tecnología o presentado ante auditorías de cumplimiento normativo.**
