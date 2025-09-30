# Overview proyecto Consola HR (60 min)
---
## 1. Introducción (10 min)

### Nombre del proyecto
**Consola de Activación Web (ConsolaActivacionWeb)** - Sistema de gestión de licencias y activaciones para productos HyperRenta

### Contexto del proyecto

**Situación inicial:** Existía una aplicación legacy que gestionaba las licencias de software de los productos HyperRenta (software tributario chileno). Esta aplicación funcionaba con tecnologías antiguas y presentaba limitaciones en usabilidad, mantenibilidad y escalabilidad.

**Problema:** Los clientes solicitaron un upgrade del sistema legacy debido a:
- Tecnologías obsoletas y difíciles de mantener
- Interfaz poco amigable y flujos de trabajo ineficientes
- Limitaciones en la gestión de múltiples clientes y máquinas
- Falta de trazabilidad y auditoría completa
- Dificultad para agregar nuevas funcionalidades

**Solución:** Modernización y rediseño completo del sistema como una aplicación web moderna (.NET 8 + Tailwind CSS), que:
- **Mantiene** todas las funcionalidades core del sistema legacy (activación de licencias, gestión de claves)
- **Mejora** la experiencia de usuario con interfaz moderna y responsive
- **Agrega** nuevas funcionalidades:
  - Dashboard de métricas y reportes interactivos
  - Sistema robusto de perfiles y permisos
  - Integración con Salesforce para consulta de órdenes
  - Auditoría completa de todas las operaciones
  - Gestión multi-cliente mejorada
- **Moderniza** la arquitectura con tecnologías actuales y mejores prácticas

### Stakeholders claves
- **Equipo de Soporte:** Usuarios principales que gestionan activaciones y configuraciones de clientes
- **Clientes:** Empresas con licencias de productos HyperRenta
- **Administradores del Sistema:** Responsables de gestión de usuarios, perfiles y permisos
- **Área Comercial:** Consulta de facturas y órdenes de Salesforce
- **Área de Desarrollo:** Mantenimiento y evolución del sistema

---

## 2. Arquitectura y Tecnología (10 min)

### Diagrama de arquitectura
```
┌─────────────────────────────────────────────────────────────┐
│                      Usuario Web (Browser)                  │
└────────────────────────┬────────────────────────────────────┘
                         │ HTTPS
┌────────────────────────▼────────────────────────────────────┐
│              ASP.NET Core MVC App (.NET 8)                  │
│  ┌──────────────────────────────────────────────────────┐   │
│  │ Features (Vertical Slice Architecture)               │   │
│  │ - Cliente      - Configuracion   - Metrica           │   │
│  │ - Account      - Profile         - Login/Logout      │   │
│  └──────────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────────┐   │
│  │ Infrastructure                                       │   │
│  │ - Identity (Auth)  - Logger   - Security             │   │
│  └──────────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────────┐   │
│  │ HyperRenta.dll (Control de Licencias)                │   │
│  │ - Encriptación   - CopyControl   - Productos         │   │
│  └──────────────────────────────────────────────────────┘   │
└────────────────────────┬────────────────────────────────────┘
                         │ Entity Framework Core
┌────────────────────────▼────────────────────────────────────┐
│     Azure SQL Server (sql-server-instance-prod-hr)          │
│                Database: HRActivacionASI                    │
│  - Clientes         - Productos      - Certificados         │
│  - Licencias        - Logs           - Métricas             │
│  - Identity/Users   - Perfiles       - Funcionalidades      │
└─────────────────────────────────────────────────────────────┘
```

### Tecnologías principales utilizadas

**Backend:**
- `.NET 8` - Framework principal
- `ASP.NET Core MVC` - Patrón MVC con Razor Pages
- `Entity Framework Core 8.0.15` - ORM para acceso a datos
- `ASP.NET Core Identity` - Sistema de autenticación y autorización
- `OneOf 3.0.271` - Manejo de tipos de retorno funcionales
- `HyperRenta.dll` - Biblioteca propietaria para gestión de licencias

**Frontend:**
- `Tailwind CSS 4.1.11` - Framework CSS utility-first
- `DaisyUI 5.0.50` - Componentes UI para Tailwind
- `ApexCharts 5.3.2` - Librería de gráficos para métricas

**Base de Datos:**
- `SQL Server` (Azure Cloud)
- Server: `sql-server-instance-prod-hr.0afb5158da99.database.windows.net`
- Database: `HRActivacionASI`

**Arquitectura:**
- Vertical Slice Architecture (Features)
- Dependency Injection nativa de .NET
- Razor Runtime Compilation para desarrollo
- Middleware personalizado para logging

### Estructura del código fuente (alto nivel)

```
ConsolaActivacionWeb/
├── Features/                         # Funcionalidades del negocio (Vertical Slices)
│   ├── Account/                     # Gestión de cuentas de usuario (CRUD)
│   │   ├── AccountController.cs
│   │   ├── AccountService.cs
│   │   ├── AccountModel.cs
│   │   ├── Database/                # Context + Entidades (HRProfile, HRFunctionality)
│   │   └── *.cshtml                 # Vistas (Create, Update, Delete, Index)
│   ├── Cliente/                     # Gestión de clientes
│   │   ├── ClienteController.cs
│   │   ├── ClienteService.cs
│   │   ├── ClienteModel.cs
│   │   ├── Database/                # Context + Entidades (HRCliente, HRClienteDetalle)
│   │   └── *.cshtml
│   ├── Configuracion/               # Core del sistema: Licencias y activaciones
│   │   ├── ConfiguracionController.cs
│   │   ├── ConfiguracionService.cs
│   │   ├── ConfiguracionModel.cs
│   │   ├── HyperRenta.dll           # ⭐ Biblioteca propietaria de licencias
│   │   ├── Database/                # Context + Entidades (HRProdCli, HRCfgCer, HRRegClave)
│   │   └── *.cshtml                 # Vistas (Maquinas, Productos, Certificados, Claves)
│   ├── Metrica/                     # Dashboard de métricas y reportes
│   │   ├── MetricaController.cs
│   │   ├── MetricaService.cs
│   │   ├── Database/                # Context + Entidades (IVA, ADM, RAD métricas)
│   │   └── Index.cshtml
│   ├── Profile/                     # Gestión de perfiles y permisos
│   ├── Login/                       # Autenticación
│   ├── Logout/                      # Cierre de sesión
│   ├── Home/                        # Página principal y menú
│   └── Shared/                      # Vistas compartidas (_Layout, _Nav, _Error)
│
├── Infrastructure/                   # Servicios transversales
│   ├── Identity/                    # ASP.NET Core Identity (ApplicationUser, IdentityContext)
│   ├── Logger/                      # Sistema de logs y auditoría (LoggerMiddleware)
│   ├── Security/                    # Validación de permisos y seguridad
│   └── Extensions/                  # Utilidades (ej: validación de RUT chileno)
│
├── wwwroot/                         # Archivos estáticos
│   ├── css/                         # Tailwind CSS (input.css, output.css)
│   ├── img/                         # Imágenes (logo.png)
│   └── favicon.ico
│
├── App.csproj                       # Configuración del proyecto .NET 8
├── App.sln                          # Solución de Visual Studio
├── Program.cs                       # ⭐ Punto de entrada y configuración DI
├── appsettings.json                 # Configuración (connection strings, logging)
├── package.json                     # Dependencias frontend (Tailwind, ApexCharts)
└── global.json                      # Versión del SDK .NET
```

### Patrón arquitectónico: Vertical Slice Architecture

El proyecto implementa **Vertical Slice Architecture**, organizando el código por funcionalidad de negocio en lugar de por tipo técnico:

**Estructura de cada Feature:**
```
Features/Cliente/
├── ClienteController.cs      # Controlador MVC
├── ClienteService.cs          # Lógica de negocio
├── ClienteModel.cs            # DTOs/ViewModels
├── Database/                  # DbContext + Entidades
│   ├── Context.cs
│   └── HRCliente.cs
└── *.cshtml                   # Vistas Razor
```

**Beneficios:**
- ✅ Alta cohesión: código relacionado agrupado
- ✅ Bajo acoplamiento: features independientes
- ✅ Mantenibilidad: cambios localizados en una carpeta
- ✅ Escalabilidad: nuevas features no afectan existentes

### Capas de la aplicación

#### 1. **Presentación (Controllers + Views)**
- Reciben requests HTTP
- Validan entrada básica
- Invocan servicios de negocio
- Retornan vistas Razor o JSON (AJAX)
- **Características**: `[Authorize]`, `[AutoValidateAntiforgeryToken]`

#### 2. **Lógica de Negocio (Services)**
- Implementan reglas de negocio
- Validaciones complejas (ej: RUT chileno)
- Coordinación de operaciones
- Transformación de datos
- **Patrón Result**: Uso de `OneOf<Success, string>` para manejo de errores

#### 3. **Acceso a Datos (DbContext + Entities)**
- Entity Framework Core 8
- **DbContext por Feature**: Separación de concerns
- Database-First approach
- Migraciones independientes por contexto

### Flujo de datos (Request Pipeline)

```
1. Browser (HTTPS) 
   ↓
2. Middleware Pipeline
   - LoggerMiddleware (auditoría)
   - Authentication (validar sesión)
   - Authorization (verificar permisos)
   - Anti-Forgery (CSRF)
   ↓
3. Controller → Service → DbContext
   ↓
4. SQL Server (Azure)
   ↓
5. ← Response (View Razor o JSON)
```

### Infraestructura transversal

**Identity (Autenticación/Autorización):**
- ASP.NET Core Identity con usuarios extendidos (`ApplicationUser`)
- Cookie authentication con sliding expiration
- Sistema propio de Perfiles y Funcionalidades
- Control de acceso granular por feature

**Logger (Auditoría):**
- `LoggerMiddleware`: registra todas las peticiones HTTP
- Tabla `HRLogAppWeb`: logs de aplicación
- Tabla `HRLOGACT`: logs específicos de activaciones
- Asociación automática con usuario autenticado

**Security:**
- Validación de permisos basada en perfiles
- Extensiones de validación (RUT chileno)
- Servicios de seguridad compartidos

### Patrones de diseño implementados

1. **Dependency Injection**: Todos los servicios registrados en IoC container
2. **Result Pattern**: `OneOf<T>` para retornos tipo Either (éxito/error)
3. **Repository Pattern** (implícito): DbContext actúa como Unit of Work
4. **ViewModel/DTO Pattern**: Separación entidades BD vs objetos de transferencia
5. **Middleware Pipeline**: Cadena de responsabilidad para procesamiento de requests

### Decisiones arquitectónicas clave

**✅ Múltiples DbContexts** (uno por feature)
- **Razón**: Separación de concerns, migraciones independientes, mejor performance

**✅ Razor MVC (no API REST pura)**
- **Razón**: Server-side rendering, mejor SEO, menos complejidad que SPA

**✅ No Repository Pattern explícito**
- **Razón**: EF Core ya es un repository, YAGNI (You Ain't Gonna Need It)

**✅ Scoped Services**
- **Razón**: Ciclo de vida por request, instancia nueva en cada petición HTTP

### Seguridad en la arquitectura

- **Authentication First**: Todos los controllers requieren `[Authorize]`
- **CSRF Protection**: Tokens anti-forgery en todos los formularios
- **SQL Injection**: Protección automática con EF Core (queries parametrizadas)
- **XSS Protection**: Razor sanitiza salidas automáticamente
- **HTTPS Only**: Redirección obligatoria a conexiones seguras
- **Connection Strings**: Configurables vía User Secrets (desarrollo) y Azure Key Vault (producción recomendado)

### Optimizaciones de rendimiento

- `AsNoTracking()`: Consultas de solo lectura sin tracking de cambios
- `async/await`: Todas las operaciones de I/O son asíncronas
- Connection Pooling: Por defecto en EF Core
- Compiled Queries: EF Core compila y cachea automáticamente
- Razor Runtime Compilation: Solo en desarrollo, deshabilitado en producción

### Integraciones con sistemas externos

1. **Salesforce:**
   - Consulta de órdenes de venta (`HROrdenSalesforce`)
   - Sincronización de datos comerciales de clientes

2. **HyperRenta.dll (Sistema Propietario):**
   - `HyperRenta.Encriptacion` - Encriptación de claves y certificados
   - `HyperRenta.CopyControl.Clave` - Gestión de claves de activación
   - `HyperRenta.CopyControl.Producto` - Configuración de productos

3. **Base de Datos de Métricas:**
   - Integración con tablas de métricas de productos HyperRenta:
     - IVA (archivos generados, ingresos)
     - ADM (empresas creadas, cantidad de empresas)
     - RAD (contribuyentes, funcionalidades, versiones)

---
## 3. Operación y Mantenimiento (10 min)

### Acceso al sistema

**URL de Producción:** [Agregar URL del sistema en producción]


### Procedimientos básicos

**Deploy:**
- Aplicación web ASP.NET Core desplegada en Azure/servidor web Windows
- Connection string configurada en `appsettings.json`
- Requiere acceso a SQL Server en Azure
- Uso de User Secrets para desarrollo (`UserSecretsId: aspnet-App-6948f0ea-404b-40d9-bcf1-9bc24845a08d`)

**Monitoreo:**
- Logs de aplicación mediante `LoggerMiddleware`
- Tabla `HRLogAppWeb` registra todas las operaciones
- Logs de activación en tabla `HRLOGACT`
- ASP.NET Core logging a nivel `Information` (configurado en appsettings)

**Backups:**
- Base de datos SQL Server en Azure (gestión de backups según políticas de Azure)
- Respaldos automáticos de Azure SQL Database

**Soporte:**
- Sistema de perfiles y funcionalidades para control de acceso
- Gestión de usuarios mediante ASP.NET Core Identity
- Trazabilidad completa: historial de activaciones por cliente
- Validación de RUT chileno integrada

**Mantenimiento:**
- Runtime compilation habilitado para desarrollo (`AddRazorRuntimeCompilation`)
- Entity Framework Migrations para cambios de esquema
- Dependencia de `HyperRenta.dll` (ubicada en `Features/Configuracion/`)

---
## 4. Estado Actual del Proyecto (10 min)

### Módulos o funcionalidades implementadas

✅ **Gestión de Clientes (`/Cliente`):**
- CRUD completo de clientes
- Validación de RUT chileno
- Gestión de detalles de cliente

✅ **Configuración de Licencias (`/Configuracion`):**
- Gestión de máquinas por cliente (crear, consultar)
- Configuración de productos por máquina
- Gestión de certificados digitales
- Activación/desactivación de claves de software
- Actualización de claves
- Consulta de facturas (integración Salesforce)
- Historial completo de activaciones

✅ **Gestión de Usuarios (`/Account`):**
- CRUD de cuentas de usuario
- Sistema de autenticación (login/logout)
- Integración con ASP.NET Core Identity

✅ **Gestión de Perfiles (`/Profile`):**
- CRUD de perfiles de usuario
- Asignación de funcionalidades por perfil
- Control de acceso basado en roles

✅ **Métricas y Reportes (`/Metrica`):**
- Métricas de IVA (archivos generados)
- Empresas por estado (activas, inactivas, eliminadas)
- Empresas creadas por mes
- Filtros por año y RUT
- Gráficos interactivos con ApexCharts

✅ **Infraestructura:**
- Sistema de logging centralizado
- Middleware de auditoría
- Servicios de seguridad
- Extensiones de validación (RUT)

### Pendientes críticos o funcionalidades en curso

⚠️ **Pendientes Técnicos:**
- Documentación técnica del proyecto (README, diagramas de BD)
- Tests unitarios y de integración
- Manejo de errores más robusto (actualmente solo developer exception page)
- Configuración de ambientes (desarrollo, QA, producción)
- Implementación de patrón Repository si se requiere mayor abstracción

⚠️ **Mejoras Potenciales:**
- API REST para integraciones externas
- Dashboard ejecutivo con KPIs
- Notificaciones por email (activaciones, vencimientos)
- Gestión de vencimiento de licencias
- Reportes exportables (PDF, Excel)
- Auditoría más detallada (quién, cuándo, qué cambió)

⚠️ **Seguridad:**
- Revisión de credenciales en `appsettings.json` (migrar a Azure Key Vault)
- Implementar HTTPS obligatorio en producción
- Rate limiting para prevenir ataques
- Validación de entradas más estricta

---
## 5. Repositorios y Documentación del Proyecto (10 min)

### Repositorios de código
- **GitHub:** `https://github.com/tr/ConsolaActivacionWeb`
- **Branch principal:** `main`
- **Archivos clave:**
  - `App.sln` - Solución de Visual Studio
  - `App.csproj` - Proyecto .NET
  - `Program.cs` - Punto de entrada y configuración
  - `appsettings.json` - Configuración de la aplicación

### Estructura del proyecto
```
ConsolaActivacionWeb/
├── Features/               # Vertical Slices
│   ├── Account/           # Gestión de usuarios
│   ├── Cliente/           # Gestión de clientes
│   ├── Configuracion/     # Licencias y activaciones
│   ├── Home/              # Página principal
│   ├── Login/Logout/      # Autenticación
│   ├── Metrica/           # Reportes y métricas
│   ├── Profile/           # Perfiles de usuario
│   └── Shared/            # Vistas compartidas
├── Infrastructure/        # Servicios transversales
│   ├── Identity/          # ASP.NET Core Identity
│   ├── Logger/            # Sistema de logs
│   ├── Security/          # Servicios de seguridad
│   └── Extensions/        # Extensiones de utilidad
├── wwwroot/               # Archivos estáticos
│   ├── css/              # Estilos (Tailwind)
│   └── img/              # Imágenes
└── Program.cs             # Configuración y startup
```

### Ubicación de la documentación oficial
- **Código fuente:** GitHub repository
- **Documentación técnica:** Pendiente de crear (sugerencia: Wiki del repositorio)

### Otros artefactos relevantes
- `package.json` - Dependencias de Node.js (Tailwind, ApexCharts)
- `global.json` - Configuración de SDK de .NET
- `HyperRenta.dll` - Biblioteca propietaria (ubicada en `Features/Configuracion/`)

## 6. Próximos Pasos (5 min)

### Acciones inmediatas para el nuevo equipo/responsables

**Onboarding Técnico (Primera Semana):**
1. ✅ Configurar entorno de desarrollo local:
   - Visual Studio 2022 o superior
   - .NET 8 SDK
   - SQL Server / Azure Data Studio
   - Node.js (para Tailwind CSS)

2. ✅ Clonar repositorio y restaurar dependencias:
   ```bash
   git clone https://github.com/tr/ConsolaActivacionWeb.git
   dotnet restore
   npm install
   ```

3. ✅ Configurar conexión a base de datos de desarrollo:
   - Actualizar `appsettings.json` con connection string de desarrollo
   - Ejecutar migrations si es necesario

4. ✅ Familiarizarse con la estructura del proyecto:
   - Revisar `Program.cs` para entender la configuración
   - Explorar cada Feature (Cliente, Configuracion, Metrica)
   - Revisar modelos de base de datos en carpetas `Database/`

**Acciones de Mejora (Primer Mes):**
1. 📝 Crear documentación técnica completa (README.md detallado)
2. 🔒 Migrar credenciales de BD a Azure Key Vault o User Secrets
3. 🧪 Implementar tests unitarios para servicios críticos
4. 📊 Revisar y optimizar queries de base de datos (performance)
5. 🐛 Configurar manejo de errores robusto para producción
6. 📋 Documentar procedimientos de deploy y rollback
7. 🔐 Implementar políticas de contraseñas más estrictas

**Conocimiento del Negocio:**
1. Entender el flujo de activación de licencias de HyperRenta
2. Familiarizarse con la integración de Salesforce (órdenes)
3. Conocer los productos HyperRenta y sus configuraciones
4. Revisar reportes de métricas con stakeholders

**Contactos Clave:**
- Equipo de soporte (usuarios principales del sistema)
- Responsable de licencias HyperRenta
- Administrador de base de datos Azure
- Product Owner / Business Analyst

---
## 7. Espacio de Preguntas (5 min)

Espacio abierto para preguntas, dudas, aclaraciones y comentarios del equipo.

---

