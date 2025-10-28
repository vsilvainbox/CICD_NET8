# Migration from .NET Core 3.1 to .NET 8

## 1. Potential Impacts

### Compatibility and Environment
- **Operating Systems:** .NET 8 requires Windows Server 2016+ or modern Linux distributions. Install the **.NET Hosting Bundle 8.0** on IIS servers.
- **Target Framework:** change to `net8.0`. Update all NuGet packages.
- **C# 12:** new features and stricter nullable reference types, analyzers, and warnings.

### ASP.NET Core
- **Minimal Hosting Model:** `Startup` is replaced by `Program.cs` using `WebApplication.CreateBuilder`.
- **Routing:** `UseEndpoints` → `MapControllers` / `MapControllerRoute`.
- **Middleware:** changes in order and defaults; new middlewares like Rate Limiting, Output Caching.
- **CORS and Cookies:** stricter defaults (`SameSite`, `Secure`).
- **Kestrel/IIS:** default behavior changes; HTTP/2 and HTTP/3 enabled by default.

### Authentication and Authorization
- Changes in `JwtBearerOptions` and claim mappings.
- ASP.NET Identity updated: stronger password hashing, stricter policies.

### JSON Serialization
- `System.Text.Json` includes new converters and stricter rules. Check handling of `DateTime`, `Enums`, and `ReferenceHandler`.
- If you were using `Newtonsoft.Json`, add `AddNewtonsoftJson()` or migrate to the built-in serializer.

### Entity Framework Core
- **Client-side evaluation:** no longer silently allowed.
- **New features:** native many-to-many, `ExecuteUpdate/Delete`, compiled models.
- **Providers:** verify compatibility (e.g., `Microsoft.EntityFrameworkCore.SqlServer`, `Pomelo.EntityFrameworkCore.MySql`).

### Libraries and Reflection
- **Trimming/AOT:** review reflection-heavy code for compatibility.
- **System.Drawing:** no longer supported on Linux. Use SkiaSharp or ImageSharp.

### Deployment
- **Azure App Service:** select .NET 8 runtime.
- **Docker:** use `mcr.microsoft.com/dotnet/aspnet:8.0` and `sdk:8.0` base images.
- **CI/CD:** update pipelines and SDK versions.

### Testing and Tools
- Changes in `WebApplicationFactory` for integration tests.
- Swashbuckle/Swagger upgrades to 6.x/7.x required.

### Common Breaking Issues
- Broken routes due to migration from `UseEndpoints`.
- JSON serialization differences.
- JWT claim mapping changes.
- EF queries failing to translate.
- Deprecated or unmaintained NuGet packages.

---

## 2. Evaluation and Migration Tools

### 2.1. .NET Upgrade Assistant
**Installation:**
```bash
dotnet tool install -g upgrade-assistant
```
**Usage:**
```bash
upgrade-assistant analyze MyApp.sln
upgrade-assistant upgrade MyApp.sln
```
Analyzes compatibility, dependencies, and updates TFM and NuGet packages automatically.

### 2.2. Porting Assistant for .NET (AWS)
Generates detailed HTML reports identifying incompatible dependencies, deprecated APIs, and migration complexity.

### 2.3. dotnet-outdated
Checks and updates outdated NuGet packages:
```bash
dotnet tool install -g dotnet-outdated-tool
dotnet outdated -u
```

### 2.4. Analyzers and Formatters
To fix code automatically:
```bash
dotnet format
dotnet build -warnaserror
```
Recommended analyzers:
- `Microsoft.CodeAnalysis.NetAnalyzers`
- `Microsoft.DotNet.Analyzers.Compatibility`

### 2.5. CI/CD and Testing
Ensure pipeline targets .NET 8 SDK:
```yaml
- uses: actions/setup-dotnet@v4
  with:
    dotnet-version: '8.0.x'
```

---

## 3. Final Recommendations
1. Create a migration branch (`chore/upgrade-net8`).
2. Run `upgrade-assistant analyze` → fix all warnings.
3. Migrate class libraries first, then Web/API projects.
4. Validate authentication, serialization, and EF Core behavior.
5. Deploy to a staging environment using the .NET 8 runtime.
6. Monitor logs and telemetry after deployment.

---

**Author:** ChatGPT  
**Date:** 2025-10-28  
