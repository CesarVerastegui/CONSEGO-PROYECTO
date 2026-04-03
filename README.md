# CONSEGO — Sistema de Gestión de Solicitudes de Acceso

**IDM Technology** — Control de solicitudes de acceso a plataformas tecnológicas (cloud e inhouse).

## Stack Tecnológico

- ASP.NET Core MVC (.NET 9)
- Entity Framework Core 9 (Pomelo.EntityFrameworkCore.MySql)
- MySQL 9.x
- Bootstrap 5
- Cookie Authentication
- Docker (opcional)

---

## Requisitos Previos

| Herramienta | Versión mínima | Notas |
|-------------|---------------|-------|
| [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) | 9.0 | Requerido |
| [Docker Desktop](https://www.docker.com/products/docker-desktop/) | Cualquiera | Requerido para MySQL en contenedor |
| [Visual Studio 2022](https://visualstudio.microsoft.com/) | 17.8+ | Opcional (se puede usar `dotnet CLI`) |
| [dotnet-ef](https://learn.microsoft.com/ef/core/cli/dotnet) | 9.x | Para ejecutar migraciones |

Instalar la herramienta de migraciones (si no la tienes):

```bash
dotnet tool install --global dotnet-ef
```

---

## Guía de Instalación Rápida

### 1. Clonar el repositorio

```bash
git clone https://github.com/CesarVerastegui/CONSEGO-PROYECTO-DSW1.git
cd CONSEGO-PROYECTO-DSW1/CONSEGO
```

### 2. Levantar MySQL con Docker

```bash
docker run -d \
  --name mysql-consego \
  -e MYSQL_ROOT_PASSWORD=123 \
  -e MYSQL_DATABASE=CONSEGO_DB \
  -p 3306:3306 \
  mysql:latest
```

> Espera unos segundos a que MySQL termine de inicializar. Puedes verificar con:
> ```bash
> docker logs mysql-consego
> ```
> Busca el mensaje `ready for connections` antes de continuar.

### 3. Configurar la cadena de conexión

Edita `appsettings.json` y ajusta el valor de `Server` según tu entorno:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1;Port=3306;Database=CONSEGO_DB;User=root;Password=123;CharSet=utf8mb4;"
  }
}
```

> ⚠️ **Importante:** Si ejecutas la aplicación directamente en tu máquina (Visual Studio o `dotnet run`), usa `Server=127.0.0.1`. Ver la sección [Ejecución con Docker](#ejecución-con-docker-app--mysql-en-contenedores) si ambos corren en contenedores.

### 4. Aplicar migraciones

```bash
dotnet ef database update
```

Esto crea todas las tablas y carga los **datos iniciales** (roles, usuarios demo y plataformas).

### 5. Ejecutar la aplicación

```bash
dotnet run
```

O presionar **F5** en Visual Studio.

La aplicación estará disponible en: `https://localhost:7046` o `http://localhost:5065`

---

## Credenciales Demo

Una vez aplicadas las migraciones, estos usuarios están disponibles:

| Rol | Email | Contraseña |
|-----|-------|------------|
| Admin | `admin@idmtechnology.pe` | `Demo123!` |
| Analista de Seguridad | `analista@idmtechnology.pe` | `Demo123!` |
| Solicitante | `solicitante@idmtechnology.pe` | `Demo123!` |

---

## Ejecución con Docker (App + MySQL en contenedores)

Si deseas ejecutar **tanto la aplicación como MySQL** en contenedores Docker (por ejemplo, al depurar con Visual Studio en modo Docker), debes tener en cuenta lo siguiente:

> ⚠️ Dentro de un contenedor, `localhost` y `127.0.0.1` apuntan al **propio contenedor**, no al host ni a otros contenedores. Por eso la app no puede conectarse a MySQL usando esas direcciones.

### 1. Crear una red Docker compartida

```bash
docker network create consego-net
```

### 2. Conectar ambos contenedores a la red

```bash
docker network connect consego-net mysql-consego
docker network connect consego-net CONSEGO
```

> Reemplaza `mysql-consego` y `CONSEGO` por los nombres reales de tus contenedores. Puedes verificarlos con `docker ps`.

### 3. Cambiar la cadena de conexión

En `appsettings.json`, usa el **nombre del contenedor de MySQL** como `Server`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=mysql-consego;Port=3306;Database=CONSEGO_DB;User=root;Password=123;CharSet=utf8mb4;"
  }
}
```

### 4. Reiniciar el contenedor de la app

```bash
docker restart CONSEGO
```

### Resumen de valores de `Server` según el entorno

| Entorno | Valor de `Server` |
|---------|-------------------|
| App local + MySQL en Docker | `127.0.0.1` |
| App en Docker + MySQL en Docker (misma red) | Nombre del contenedor MySQL (ej: `mysql-consego`) |
| App local + MySQL local | `localhost` o `127.0.0.1` |

---

## Funcionalidades

### Roles y Permisos

| Funcionalidad | Admin | Analista | Solicitante | Infra |
|---------------|-------|----------|-------------|-------|
| Gestionar Roles | ✅ | ❌ | ❌ | ❌ |
| Gestionar Usuarios | ✅ | ❌ | ❌ | ❌ |
| Gestionar Plataformas | ✅ | ❌ | ❌ | ❌ |
| Crear Solicitud | ✅ | ❌ | ✅ | ❌ |
| Ver todas las Solicitudes | ✅ | ✅ | ❌ (solo las suyas) | ❌ (solo aprobadas) |
| Tomar Solicitud (EnAnálisis) | ✅ | ✅ | ❌ | ❌ |
| Aprobar/Rechazar | ✅ | ✅ | ❌ | ❌ |
| Marcar Implementado | ✅ | ❌ | ❌ | ✅ |

### Flujo de Solicitudes

```
Solicitante crea → Registrado
        ↓
Analista toma → EnAnálisis
        ↓
Analista decide → Aprobado / Rechazado
        ↓
Infra/Admin → Implementado (solo si fue Aprobado)
```

### Código Autogenerado

Las solicitudes reciben un código automático: `ACC-YYYY-0001`, `ACC-YYYY-0002`, etc.

### Filtros y Paginación

El listado de solicitudes permite filtrar por:
- Estado
- Plataforma
- Rango de fechas

Con paginación de 10 registros por página.

---

## Estructura del Proyecto

```
CONSEGO/
├── Controllers/
│   ├── AuthController.cs
│   ├── HomeController.cs
│   ├── RolesController.cs
│   ├── UsuariosController.cs
│   ├── PlataformasController.cs
│   └── SolicitudesController.cs
├── Data/
│   └── AppDbContext.cs
├── Migrations/
├── Models/
│   ├── Enums/
│   │   ├── Criticidad.cs
│   │   ├── EstadoSolicitud.cs
│   │   ├── TipoAcceso.cs
│   │   └── TipoPlataforma.cs
│   ├── ViewModels/
│   │   ├── LoginViewModel.cs
│   │   ├── SolicitudFiltroViewModel.cs
│   │   ├── UsuarioCreateViewModel.cs
│   │   └── UsuarioEditViewModel.cs
│   ├── Plataforma.cs
│   ├── Rol.cs
│   ├── SolicitudAcceso.cs
│   └── Usuario.cs
├── Views/
│   ├── Auth/          (Login, AccessDenied)
│   ├── Home/          (Index)
│   ├── Plataformas/   (CRUD)
│   ├── Roles/         (CRUD)
│   ├── Solicitudes/   (Index, Create, Details, Resolver)
│   ├── Usuarios/      (CRUD)
│   └── Shared/        (_Layout)
├── wwwroot/           (CSS, JS, Bootstrap)
├── Dockerfile
├── Program.cs
├── appsettings.json
└── CONSEGO.csproj
```

---

## Seeds Iniciales

La aplicación incluye datos iniciales que se cargan automáticamente con las migraciones:

- **4 Roles:** Admin, AnalistaSeguridad, Solicitante, Infra
- **3 Usuarios:** admin, analista, solicitante (contraseña: `Demo123!`)
- **8 Plataformas:** GitHub Org, AWS, Azure, M365, Cloudflare, WordPress, GoDaddy, VMs On-Premise

---

## Solución de Problemas

| Problema | Causa | Solución |
|----------|-------|----------|
| `Unable to connect to any of the specified MySQL hosts` | MySQL no accesible desde la app | Verificar que el contenedor MySQL esté corriendo (`docker ps`) y que `Server` en `appsettings.json` sea correcto según tu entorno |
| `RetryLimitExceededException` | La app reintentó conectarse 5 veces sin éxito | Mismo problema de conexión. Revisar el valor de `Server` y la red Docker |
| La app en Docker no conecta a MySQL en Docker | `127.0.0.1` apunta al propio contenedor | Usar el nombre del contenedor MySQL como `Server` y crear una red compartida (ver [Ejecución con Docker](#ejecución-con-docker-app--mysql-en-contenedores)) |
| `dotnet ef` no reconocido | Herramienta no instalada | `dotnet tool install --global dotnet-ef` |
| Las tablas no existen en MySQL | Migraciones no aplicadas | `dotnet ef database update` |

### 2. Ejecución local (sin Docker para la app)

Si ejecutas la aplicación **directamente en tu máquina** (Visual Studio / `dotnet run`) y MySQL corre en Docker, usa `127.0.0.1` como servidor

### 3. Ejecución con Docker (app y MySQL en contenedores)

Si tanto la aplicación como MySQL corren en contenedores Docker, deben estar en la **misma red Docker** y la cadena de conexión debe usar el **nombre del contenedor** de MySQL como servidor.

#### a. Crear una red compartida y conectar los contenedores

```bash
docker network create consego-network
docker network connect consego-network mysql-consego
docker run --name consego-app --network consego-network -p 5000:80 -d idmvc:latest
```

#### b. Actualizar cadena de conexión

En `appsettings.json`, usar el nombre del contenedor de MySQL:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=mysql-consego;Port=3306;Database=CONSEGO_DB;User=root;Password=123;CharSet=utf8mb4;"
}
```

#### c. Ejecutar aplicación

````````
dotnet run
```

O presionar **F5** en Visual Studio.

## Docker Compose (opcional)

Como alternativa, se puede usar Docker Compose con el siguiente `docker-compose.yml`:

```yaml
version: '3.8'
services:
  db:
    image: mysql:latest
    container_name: mysql-consego
    restart: always
    environment:
      MYSQL_ROOT_PASSWORD: 123
      MYSQL_DATABASE: CONSEGO_DB
    ports:
      - "3306:3306"
  app:
    image: idmvc:latest
    container_name: consego-app
    restart: always
    ports:
      - "5000:80"
    depends_on:
      - db
networks:
  default:
    external:
      name: consego-network
```

Para ejecutar:

```bash
docker-compose up -d
```

Luego verificar que los contenedores estén corriendo y acceder a la aplicación en `http://localhost:5000`.