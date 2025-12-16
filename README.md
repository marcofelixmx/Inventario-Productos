# API de Gestión de Inventario de productos

## Descripción del Proyecto
Este proyecto implementa una API RESTful para gestionar un inventario de productos, adhiriéndose a buenas prácticas de programación, patrones de diseño y principios SOLID. La solución está dockerizada y utiliza una base de datos relacional SQL Server.

## Tecnologías Utilizadas
*   .NET 6.0
*   ASP.NET Core Web API
*   Entity Framework Core (para operaciones de lectura)
*   Dapper (para operaciones de escritura)
*   MediatR (para la implementación de CQRS)
*   Docker & Docker Compose
*   SQL Server
*   Autenticación JWT Bearer (oAuth2)
*   Swagger/OpenAPI (para documentación de la API)
*   xUnit, Moq, FluentAssertions (para pruebas unitarias)

## Características
*   **Gestión de Productos:** Operaciones CRUD para productos.
*   **Gestión de Categorías:** Operaciones CRUD para categorías.
*   **Movimientos de Inventario:** Registro de cambios de stock de productos (entrada/salida).
*   **Autenticación:** Endpoints de la API protegidos usando tokens JWT Bearer.
*   **Dockerizado:** Fácil configuración y despliegue usando Docker Compose.

## Instrucciones de Configuración

### Prerrequisitos
*   .NET 6.0 SDK (o superior, pero el proyecto apunta a 6.0)
*   Docker Desktop (o Docker Engine)
*   Un cliente de SQL (ej. Azure Data Studio, SQL Server Management Studio) para la configuración manual de la base de datos.

### 1. Clonar el Repositorio
```bash
git clone <url-del-repositorio>
cd Inventario-Productos
```

### 2. Configurar e Iniciar los Contenedores Docker
El proyecto incluye un archivo `docker-compose.yml` para configurar la base de datos SQL Server y la API.

**Importante:** La contraseña `SA_PASSWORD` para SQL Server está actualmente configurada como `Password123!` en `docker-compose.yml` y `appsettings.Development.json`. Para entornos de producción, asegúrate de usar una contraseña fuerte y única y de gestionarla de forma segura (ej. a través de variables de entorno o un gestor de secretos).

```bash
docker-compose up -d
```
Este comando hará lo siguiente:
*   Descargará la imagen de SQL Server.
*   Construirá la imagen Docker de la API.
*   Iniciará ambos contenedores: el de SQL Server (`redarbor-db`) y el de la API (`redarbor-api`).

### 3. Configuración Manual de la Base de Datos

1.  **Conectar a SQL Server:**
    *   Abre tu cliente de SQL (Azure Data Studio, SSMS).
    *   Conéctate a: `localhost,1433` (o `127.0.0.1,1433`)
    *   Autenticación: SQL Server Authentication
    *   Login: `sa`
    *   Contraseña: `Password123!`

2.  **Crear la Base de Datos:**
    Ejecuta el siguiente comando SQL:
    ```sql
    CREATE DATABASE Redarbor;
    GO
    ```

3.  **Aplicar Migraciones (Crear Tablas):**
    *   Asegúrate de que la base de datos `Redarbor` esté seleccionada como la base de datos activa en tu cliente SQL.
    *   Abre el archivo `InitialCreate.sql` ubicado en la raíz de este proyecto.
    *   Copia y ejecuta todo el contenido de `InitialCreate.sql`. Esto creará las tablas `Products`, `Categories` y `InventoryMovements`.

## Cómo Ejecutar la Aplicación


*   **Swagger UI:** Una vez que el contenedor `redarbor-api` esté en ejecución, puedes acceder a la UI de Swagger en `https://localhost:8081/swagger` o `http://localhost:8080/swagger`.

## Autenticación

La API utiliza autenticación JWT Bearer. Todos los endpoints están protegidos.

### Obtener un Token (Simulación Manual)
Para fines de prueba, normalmente obtendrías un token JWT de un Proveedor de Identidad. Como en este proyecto no se ha implementado un Proveedor de Identidad completo, puedes generar manualmente un token JWT usando una herramienta como [jwt.io](https://jwt.io/) con lo siguiente (que coincide con `appsettings.json`):

*   **Header:**
    ```json
    {
      "alg": "HS256",
      "typ": "JWT"
    }
    ```
*   **Payload (Ejemplo):**
    ```json
    {
      "sub": "testuser",
      "name": "Test User",
      "jti": "a unique guid",
      "exp": 1734000000, 
      "iss": "RedarborApi",
      "aud": "RedarborUsers"
    }
    ```
*   **Verify Signature (Secreto):**
    ```
    ThisIsMySuperSecretKeyForJwtAuthentication
    ```
    (Asegúrate de que esto coincida con la `Jwt:Key` en `appsettings.json`)

### Usar el Token en Swagger UI
1.  Ve a la UI de Swagger (`https://localhost:8081/swagger`).
2.  Haz clic en el botón "Authorize" (normalmente un icono de candado).
3.  En el diálogo, pega tu token JWT generado en el campo de valor (ej. `eyJhbGciOiJIUzI1Ni...`).
4.  Haz clic en "Authorize" y luego en "Close".
5.  Ahora deberías poder realizar peticiones autenticadas a los endpoints de la API.

## Cómo Ejecutar las Pruebas

Para ejecutar las pruebas unitarias:

```bash
dotnet test tests/Redarbor.Tests/Redarbor.Tests.csproj
```

## Cumplimiento de Reglas de Código Limpio
Este proyecto se adhiere a las siguientes reglas de código limpio:
1.  Todo el código y los comentarios están 100% en inglés.
2.  No hay espacios en blanco entre líneas de código, excepto una línea antes de un `return`.
3.  Los nombres de métodos y funciones son autodescriptivos.
4.  Las funciones tienen una única responsabilidad (métodos generalmente de menos de 25 líneas, sin bucles/condicionales anidados profundamente, sin necesidad de comentarios para secciones de código).
5.  Las variables de configuración se leen desde variables de entorno (o `appsettings.json` como fallback local).
6.  Una clase o enum por archivo.
7.  Cero código comentado.
8.  Se pasan objetos como parámetros en lugar de múltiples propiedades individuales cuando es posible.
