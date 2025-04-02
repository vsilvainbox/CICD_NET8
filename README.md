# Capacitación: Nuevo Workflow de Desarrollo con GitHub y GitHub Actions

## Introducción

Esta capacitación está dirigida a desarrolladores que trabajarán con nuestros repositorios configurados con workflows de GitHub Actions para build y deploy. El objetivo es entender el flujo de trabajo diario y cómo interactuar con el sistema.

## Conceptos Básicos

### ¿Qué es GitHub Actions?

GitHub Actions es una plataforma de integración continua y entrega continua (CI/CD) que permite automatizar tareas dentro del ciclo de vida del desarrollo de software. Algunos beneficios incluyen:

- Automatización de pruebas
- Compilación y empaquetado de código
- Despliegue en ambientes de desarrollo, prueba y producción
- Todo integrado directamente en GitHub

### Flujo de Trabajo Configurado

Los repositorios ya tienen configurados los workflows de build y deploy, lo que significa que:

1. Los procesos de compilación se ejecutan automáticamente
2. Los despliegues se realizan sin intervención manual
3. Ustedes como desarrolladores solo necesitan enfocarse en escribir código y seguir el workflow

## Workflow del Desarrollador

### 1. Clonar el Repositorio

```bash
git clone https://github.com/[organización]/[repositorio].git
cd [repositorio]
```

### 2. Crear una Rama para tu Tarea

Siempre trabajamos en ramas separadas, nunca directamente en `main`:

```bash
git checkout -b feature/nombre-descriptivo
```

Convenciones de nombres para ramas:
- `feature/descripcion-corta` - Para nuevas características
- `bugfix/descripcion-del-problema` - Para corrección de errores
- `hotfix/descripcion-urgente` - Para correcciones urgentes
- `refactor/descripcion` - Para cambios de código sin cambiar funcionalidad

### 3. Desarrollar y Hacer Commits

Trabaja normalmente en tu entorno de desarrollo y realiza commits frecuentes:

```bash
git add .
git commit -m "Descripción clara del cambio realizado"
```

Buenas prácticas para mensajes de commit:
- Usar verbos en presente ("Añade función X" en lugar de "Añadido función X")
- Ser descriptivo pero conciso
- Incluir el número de ticket/tarea si se utiliza un sistema de gestión de proyectos

### 4. Subir Cambios y Crear Pull Request

Cuando tu funcionalidad esté lista:

```bash
git push origin feature/nombre-descriptivo
```

Luego, en GitHub:
1. Ve al repositorio
2. GitHub mostrará un botón para crear un Pull Request desde tu rama
3. Crea el PR con una descripción clara de los cambios realizados

### 5. El Workflow de GitHub Actions en Acción

Al crear el PR, automáticamente:

1. Se ejecutará el workflow de **build**
   - Compilará el código
   - Ejecutará pruebas unitarias
   - Verificará el estilo de código
   - Realizará análisis estático

2. Podrás ver el progreso en la pestaña "Actions" del repositorio o directamente en el PR
   - ✅ Marca verde: Todo está bien
   - ❌ Marca roja: Hay problemas que debes resolver

### 6. Revisión de Código y CI

1. Solicita revisión a tus compañeros
2. Atiende los comentarios y realiza los cambios necesarios
3. Cada nuevo push a la rama activará nuevamente los workflows de CI
4. El PR no se puede fusionar hasta que:
   - Los workflows pasen correctamente
   - Se obtengan las aprobaciones necesarias

### 7. Merge y Despliegue

Una vez aprobado el PR:

1. Realiza el merge (preferiblemente usando "Squash and merge" para mantener un historial limpio)
2. Automáticamente se activará el workflow de **deploy**
   - El código se desplegará al ambiente correspondiente (desarrollo, pruebas o producción)
   - Puedes seguir el progreso en la pestaña "Actions"

3. Verifica que el despliegue se haya completado correctamente

### 8. Actualiza tu Entorno Local

Después del merge:

```bash
git checkout main
git pull
git branch -d feature/nombre-descriptivo  # Elimina la rama local
```

## Manejo de Errores Comunes

### El Workflow Falla

Si el workflow de CI falla:

1. Revisa los logs en la pestaña "Actions"
2. Identifica el error específico
3. Corrige el problema en tu rama local
4. Haz commit y push nuevamente
5. El workflow se ejecutará automáticamente

### Conflictos de Merge

Si hay conflictos al intentar hacer merge:

1. Actualiza tu rama con los últimos cambios de main:
   ```bash
   git checkout feature/nombre-descriptivo
   git fetch origin
   git merge origin/main
   ```
2. Resuelve los conflictos manualmente
3. Haz commit de los cambios
4. Haz push nuevamente

## Verificación del Estado del Despliegue

Para verificar el estado actual de los despliegues:

1. Ve a la pestaña "Actions" en GitHub
2. Filtra por el workflow de despliegue
3. Revisa el último despliegue completado

## Mejores Prácticas

1. **Pull Frecuente**: Mantén tu rama actualizada con `main` para evitar conflictos grandes
2. **Commits Pequeños**: Realiza commits frecuentes y enfocados en cambios específicos
3. **Pruebas Locales**: Ejecuta pruebas localmente antes de hacer push
4. **Revisión de Código**: Participa activamente en las revisiones de código de tus compañeros
5. **Monitoreo**: Verifica el estado de los workflows y despliegues

## Recursos Adicionales

- [Documentación oficial de GitHub Actions](https://docs.github.com/es/actions)
- [Guía de mejores prácticas de Git](https://www.git-tower.com/learn/git/ebook/en/command-line/appendix/best-practices)
- [Visualizador de flujos de trabajo de GitHub](https://githubflow.github.io/)

## Preguntas Frecuentes

### ¿Puedo ejecutar los workflows manualmente?
Sí, puedes ir a la pestaña "Actions", seleccionar el workflow deseado y usar el botón "Run workflow".

### ¿Qué hago si necesito revertir un despliegue?
En caso de problemas, se puede crear un PR de reversion o solicitar ayuda al equipo de DevOps.

### ¿Se despliega automáticamente en producción?
Depende de la configuración específica. Generalmente, los despliegues a producción requieren aprobación manual adicional.

### ¿Cómo veo los logs de un despliegue?
En la pestaña "Actions", haz clic en la ejecución específica del workflow y podrás expandir cada paso para ver sus logs detallados.
