# Consola de Activación Web (ConsolaActivacionWeb)

Sistema web de gestión de licencias y activaciones para productos HyperRenta.

## 📋 Descripción

Aplicación web desarrollada en ASP.NET Core que centraliza y automatiza la gestión completa del ciclo de vida de licencias de software HyperRenta (software tributario chileno). Permite gestionar clientes, configurar licencias, activar/desactivar claves de productos, administrar certificados digitales y generar métricas de uso.

### Características Principales

- ✅ **Gestión de Clientes**: CRUD completo con validación de RUT chileno
- ✅ **Configuración de Licencias**: Gestión de máquinas, productos y certificados por cliente
- ✅ **Activación de Claves**: Activación, desactivación y actualización de claves de software
- ✅ **Gestión de Usuarios**: Sistema completo de autenticación y autorización
- ✅ **Perfiles y Permisos**: Control de acceso basado en roles y funcionalidades
- ✅ **Métricas y Reportes**: Dashboard con gráficos de IVA, empresas y contribuyentes
- ✅ **Auditoría Completa**: Trazabilidad de todas las operaciones mediante logs
- ✅ **Integración Salesforce**: Consulta de órdenes y datos comerciales

## 🛠️ Tecnologías

### Backend
- **.NET 8** - Framework principal
- **ASP.NET Core MVC** - Patrón arquitectónico
- **Entity Framework Core 8.0.15** - ORM
- **ASP.NET Core Identity** - Autenticación y autorización
- **SQL Server** (Azure) - Base de datos
- **HyperRenta.dll** - Biblioteca propietaria para gestión de licencias

### Frontend
- **Tailwind CSS 4.1.11** - Framework CSS utility-first
- **DaisyUI 5.0.50** - Componentes UI
- **ApexCharts 5.3.2** - Librería de gráficos

### Paquetes NuGet Principales
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.15" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.15" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.11" />
<PackageReference Include="OneOf" Version="3.0.271" />
```

## 📁 Estructura del Proyecto

```
ConsolaActivacionWeb/
├── Features/                    # Vertical Slice Architecture
│   ├── Account/                # Gestión de usuarios
│   ├── Cliente/                # Gestión de clientes
│   ├── Configuracion/          # Licencias y activaciones
│   ├── Metrica/                # Reportes y métricas
│   ├── Profile/                # Perfiles de usuario
│   ├── Login/                  # Autenticación
│   ├── Home/                   # Página principal
│   └── Shared/                 # Vistas compartidas
├── Infrastructure/             # Servicios transversales
│   ├── Identity/              # ASP.NET Core Identity
│   ├── Logger/                # Sistema de logs
│   ├── Security/              # Servicios de seguridad
│   └── Extensions/            # Extensiones de utilidad
├── wwwroot/                   # Archivos estáticos
├── App.csproj                 # Proyecto .NET
├── Program.cs                 # Punto de entrada
└── appsettings.json          # Configuración
```

## 🚀 Requisitos Previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [Visual Studio Code](https://code.visualstudio.com/)
- [Node.js](https://nodejs.org/) (para Tailwind CSS)
- [SQL Server](https://www.microsoft.com/sql-server) o acceso a Azure SQL Database

## ⚙️ Instalación y Configuración

### 1. Clonar el repositorio

```bash
git clone https://github.com/tr/ConsolaActivacionWeb.git
cd ConsolaActivacionWeb
```

### 2. Restaurar dependencias

```bash
# Dependencias .NET
dotnet restore

# Dependencias Node.js (Tailwind CSS)
npm install
```

### 3. Configurar la base de datos

#### Opción A: Usar User Secrets (Recomendado para desarrollo)

```bash
# Inicializar user secrets
dotnet user-secrets init

# Configurar connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=HRActivacionASI;Integrated Security=true;TrustServerCertificate=true;"
```

#### Opción B: Modificar appsettings.json (Solo para desarrollo local)

**⚠️ IMPORTANTE**: Nunca commitear credenciales reales en appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HRActivacionASI;Integrated Security=true;TrustServerCertificate=true;"
  }
}
```

### 4. Aplicar migraciones de base de datos

```bash
# Crear la base de datos y aplicar migraciones
dotnet ef database update
```

### 5. Compilar CSS con Tailwind

```bash
# Modo desarrollo (watch)
npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/output.css --watch

# Modo producción (minificado)
npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/output.css --minify
```

### 6. Ejecutar la aplicación

```bash
dotnet run
```

La aplicación estará disponible en: `https://localhost:5001` o `http://localhost:5000`

## 🏗️ Arquitectura

El proyecto utiliza **Vertical Slice Architecture**, donde cada feature es autónomo y contiene:
- Controller (MVC)
- Service (lógica de negocio)
- Models (DTOs y ViewModels)
- Database (DbContext y entidades)
- Views (Razor Pages)

### Flujo de una petición

```
Usuario → Controller → Service → Database Context → SQL Server
                ↓
              View (Razor)
```

### Servicios de Infraestructura

- **Identity**: Manejo de autenticación y autorización con ASP.NET Core Identity
- **Logger**: Middleware personalizado que registra todas las operaciones
- **Security**: Servicios de seguridad y validación
- **Extensions**: Métodos de extensión (ej: validación de RUT chileno)

## 📊 Funcionalidades

### Módulo de Clientes (`/Cliente`)
- Listado de clientes con paginación
- Crear nuevo cliente con validación de RUT
- Actualizar información de cliente
- Eliminar cliente (con confirmación)
- Ver detalles completos de cliente

### Módulo de Configuración (`/Configuracion`)
- **Máquinas**: Crear y gestionar máquinas por cliente
- **Productos**: Asignar productos a máquinas específicas
- **Certificados**: Administrar certificados digitales
- **Claves**: Activar, desactivar y actualizar claves de licencia
- **Facturas**: Consultar facturas desde Salesforce
- **Historial**: Visualizar historial completo de activaciones

### Módulo de Usuarios (`/Account`)
- Crear cuentas de usuario
- Asignar perfiles y permisos
- Actualizar información de usuario
- Desactivar/activar usuarios

### Módulo de Perfiles (`/Profile`)
- Gestión de perfiles de acceso
- Asignación de funcionalidades por perfil
- Control granular de permisos

### Módulo de Métricas (`/Metrica`)
- **IVA**: Archivos generados por mes
- **Empresas**: Estados (activas, inactivas, eliminadas)
- **Empresas Creadas**: Tendencia mensual
- **Filtros**: Por año y RUT de cliente
- **Gráficos**: Visualización interactiva con ApexCharts

## 🔒 Seguridad

- **Autenticación**: ASP.NET Core Identity con cookies
- **Autorización**: Atributo `[Authorize]` en todos los controllers
- **Anti-forgery**: Validación automática de tokens CSRF
- **HTTPS**: Redirección automática a HTTPS
- **Validación de RUT**: Validación de RUT chileno en el lado del servidor

## 📝 Logging y Auditoría

El sistema registra todas las operaciones en dos niveles:

1. **Application Logs**: Mediante `LoggerMiddleware` en tabla `HRLogAppWeb`
2. **Activation Logs**: Operaciones de activación en tabla `HRLOGACT`

## 🔧 Desarrollo

### Convenciones de Código

- **Naming**: PascalCase para clases, métodos y propiedades
- **Async/Await**: Usar para todas las operaciones de BD
- **OneOf**: Usar para retornos que pueden ser éxito o error
- **Dependency Injection**: Preferir inyección por constructor

### Agregar una nueva Feature

1. Crear carpeta en `/Features/NombreFeature/`
2. Crear Controller, Service, Model
3. Crear carpeta `Database/` con Context y entidades
4. Registrar servicios en `Program.cs`

## 📚 Recursos Adicionales

- [Documentación de .NET 8](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8)
- [ASP.NET Core MVC](https://learn.microsoft.com/aspnet/core/mvc/overview)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [Tailwind CSS](https://tailwindcss.com/docs)
- [DaisyUI](https://daisyui.com/)
- [ApexCharts](https://apexcharts.com/docs/)

## 🤝 Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto es propiedad de [Nombre de la Empresa]. Todos los derechos reservados.

## 👥 Contacto

- **Equipo de Desarrollo**: [email]
- **Soporte**: [email]
- **Issues**: [GitHub Issues](https://github.com/tr/ConsolaActivacionWeb/issues)

---

**Versión**: 1.0.0  
**Estado**: En producción