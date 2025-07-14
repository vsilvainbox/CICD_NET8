# Documento de Arquitectura: Sistema de Telemetría para Aplicaciones de Escritorio

**Versión:** 1.0
**Fecha:** 2025-07-13
**Autor:** vsilvainbox
**Estado:** Borrador

## 1. Introducción y Propósito

Este documento describe la arquitectura de la solución diseñada para capturar y analizar datos de uso (telemetría) de las aplicaciones de escritorio existentes desarrolladas en Visual Basic 6.

El objetivo principal es obtener información valiosa sobre cómo se utilizan las aplicaciones para guiar futuras mejoras y decisiones de negocio, sin comprometer el rendimiento de las aplicaciones cliente ni la privacidad de los usuarios.

## 2. Antecedentes y Contexto

La organización cuenta con una base de aplicaciones de escritorio (VB6) instaladas en las estaciones de trabajo de los clientes. Actualmente, no existe un mecanismo para recopilar datos de uso, lo que limita la visibilidad sobre las funcionalidades más utilizadas, los flujos de trabajo comunes y los posibles puntos de fricción.

### Requerimientos Clave:

*   **R-1:** La modificación debe integrarse en las aplicaciones VB6 existentes.
*   **R-2:** Los datos se centralizarán a través de un servicio web (API).
*   **R-3:** La API se desarrollará en .NET 9 y se hospedará en un servidor IIS sobre una VM de Windows en Azure.
*   **R-4 (Privacidad):** Queda estrictamente prohibida la captura de datos sensibles o de identificación personal (PII). Toda la telemetría debe ser anónima.
*   **R-5 (Rendimiento):** La implementación no debe introducir una degradación perceptible en el rendimiento de las aplicaciones cliente.

## 3. Arquitectura de la Solución

La solución se compone de tres elementos principales: el **Módulo de Captura** integrado en la aplicación cliente, una **API de Telemetría** como punto de recepción central y la **Infraestructura en la Nube** que soporta el servicio.

### 3.1. Diagrama de Arquitectura

El siguiente diagrama ilustra la relación entre los componentes de la solución:

```mermaid
graph TD
    subgraph "Cliente (Estación de Trabajo)"
        A[Aplicación de Escritorio VB6]
        B(Módulo de Captura de Telemetría)
        A -- Invoca eventos --> B
        B -- Acumula datos --> C{Buffer de Datos Local}
        C -- Envía datos periódicamente/al cerrar --> D{Cliente HTTP}
    end

    subgraph "Azure (Nube)"
        E[API de Telemetría (.NET 9 en IIS)]
        F[(Base de Datos Anónima)]
        G[Servidor Virtual Windows]
        E -- Almacena datos --> F
        E -- Se ejecuta en --> G
    end

    D -- HTTPS Request (JSON) --> E

    style A fill:#D6EAF8,stroke:#333,stroke-width:2px
    style E fill:#D5F5E3,stroke:#333,stroke-width:2px
```

### 3.2. Descripción de Componentes

1.  **Aplicación de Escritorio (VB6):** Es la aplicación existente que será modificada para incluir el módulo de captura.
2.  **Módulo de Captura de Telemetría (VB6):** Un nuevo componente (módulo `.bas` o clase `.cls`) que se agregará al código fuente de las aplicaciones. Sus responsabilidades son:
    *   Exponer funciones simples para registrar eventos (ej. `RegistrarUsoFormulario("frmClientes")`, `RegistrarClickBoton("btnGuardar")`).
    *   Gestionar un buffer interno para acumular eventos de forma eficiente.
    *   Implementar la lógica de envío de datos al API de forma asíncrona o en momentos de baja actividad.
3.  **API de Telemetría (.NET 9):** Un servicio web RESTful que actúa como el único punto de entrada para los datos de telemetría. Sus responsabilidades son:
    *   Exponer un endpoint seguro (ej. `POST /api/telemetry/events`).
    *   Validar la estructura de los datos recibidos.
    *   Asegurar el anonimato de los datos, descartando cualquier información que pudiera identificar al usuario o la máquina.
    *   Persistir los datos validados en una base de datos optimizada para este fin.
4.  **Infraestructura Azure:**
    *   **Máquina Virtual Windows Server:** Aloja el servidor web IIS donde se ejecutará la aplicación de la API.
    *   **Base de Datos:** Un servicio de base de datos (como Azure SQL Database o Azure Table Storage) para almacenar los datos de telemetría de forma estructurada y escalable.

## 4. Funcionamiento del Proceso de Captura

El proceso se ha diseñado para ser robusto, asíncrono y minimizar el impacto en el usuario final.

1.  **Inicio y Registro de Eventos:**
    *   Al iniciar la aplicación VB6, el Módulo de Captura se inicializa.
    *   Durante la interacción del usuario, el código de la aplicación invoca al módulo para registrar eventos clave (ej. apertura de una ventana, clic en un botón, ejecución de un reporte).
    *   **Ejemplo:** `Telemetry.LogEvent("ButtonClick", "frmVentas", "btnCalcularTotal")`

2.  **Acumulación de Datos (Buffering):**
    *   Para cumplir con el requisito de no degradar el rendimiento (**R-5**), los eventos no se envían a la API en tiempo real.
    *   El Módulo de Captura acumula estos eventos en una colección o un archivo temporal en la máquina local (buffer). Esto evita realizar una llamada de red por cada acción del usuario, lo cual sería ineficiente.

3.  **Envío Asíncrono de Datos:**
    *   El envío de los datos acumulados se activa bajo condiciones predefinidas para no interrumpir al usuario:
        *   **Al cerrar la aplicación:** Es el momento ideal para enviar todos los datos de la sesión.
        *   **Por umbral de eventos:** Cuando se acumula un número determinado de eventos (ej. 20 eventos).
        *   **Por temporizador:** Cada cierto intervalo de tiempo (ej. cada 15 minutos).
    *   El módulo empaqueta los eventos en un formato ligero (como JSON) y utiliza un componente HTTP (como `MSXML2.XMLHTTP`) para realizar una llamada `POST` asíncrona al endpoint de la API de Telemetría. El envío se realiza sobre HTTPS para garantizar la seguridad en el tránsito.

4.  **Recepción y Procesamiento en el Servidor:**
    *   La API de Telemetría recibe la solicitud HTTP.
    *   Valida que el cuerpo de la solicitud (payload) tenga el formato JSON esperado.
    *   **Filtro de Anonimato:** La API procesa cada evento y se asegura de que no contenga PII (**R-4**). Si se detectara algún dato no permitido, se descarta o se anonimiza antes de guardarlo.
    *   Los datos limpios y validados se almacenan en la base de datos para su posterior análisis.

5.  **Manejo de Errores:**
    *   Si la máquina del cliente no tiene conexión a internet o la API no está disponible, el envío fallará. El Módulo de Captura debe manejar este error silenciosamente, reteniendo los datos en el buffer para intentar enviarlos en la próxima oportunidad, sin notificar al usuario.

Este enfoque garantiza que la experiencia del usuario no se vea afectada, la privacidad está protegida y los datos se recopilan de manera fiable y eficiente.

---