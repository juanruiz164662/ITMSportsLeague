# SportsLeague ⚽

API REST y aplicación web para gestionar torneos de fútbol: equipos, jugadores, árbitros, partidos, goles, tarjetas, alineaciones y tabla de posiciones calculada automáticamente.

![.NET 8](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![Angular 21](https://img.shields.io/badge/Angular-21-DD0031?logo=angular)

<!-- Agrega aquí una captura de Swagger o de la app Angular: -->
<!-- ![Vista de la aplicación](docs/captura.png) -->

## ¿Qué problema resuelve?

Organizar un torneo implica llevar a mano resultados, goleadores, tarjetas y la tabla de posiciones. SportsLeague centraliza esa información: registras el resultado y los eventos de cada partido, y la API calcula la clasificación y las estadísticas.

## Funcionalidades

- **Torneos:** creación, cambio de estado e inscripción de equipos.
- **Equipos y jugadores:** CRUD completo y consulta de jugadores por equipo.
- **Árbitros y patrocinadores:** gestión y asociación de patrocinadores a torneos.
- **Partidos:** programación por torneo y control de estado del partido.
- **Eventos del partido:** resultado, goles, tarjetas y alineaciones por equipo.
- **Estadísticas:** tabla de posiciones, tabla de goleadores y estadísticas de tarjetas.

## Tecnologías

| Capa | Tecnologías |
| --- | --- |
| Backend | C#, .NET 8, ASP.NET Core Web API, AutoMapper, Swagger |
| Datos | Entity Framework Core 8, SQL Server, migraciones y *seeder* de datos |
| Frontend | Angular 21, Angular Material, TypeScript |

## Arquitectura

Solución en capas con separación de responsabilidades:

```
SportsLeague.API          → Controladores, DTOs de request/response, AutoMapper
SportsLeague.Domain       → Entidades, enums, interfaces y servicios de negocio
SportsLeague.DataAccess   → DbContext, repositorios, migraciones y seeder
sports-league-frontend    → Cliente web en Angular
```

- Patrón **Repository** (repositorio genérico + repositorios específicos).
- **Inyección de dependencias** para repositorios y servicios.
- **DTOs** para no exponer las entidades del dominio.
- Validaciones de negocio centralizadas (`MatchValidationHelper`).

## Endpoints principales

| Recurso | Ruta base | Operaciones |
| --- | --- | --- |
| Torneos | `/api/tournament` | CRUD, `PATCH /{id}/status`, `/{id}/teams` |
| Equipos | `/api/team` | CRUD |
| Jugadores | `/api/player` | CRUD, `/team/{teamId}` |
| Árbitros | `/api/referee` | CRUD |
| Patrocinadores | `/api/sponsor` | CRUD, `/{id}/tournaments` |
| Partidos | `/api/match` | CRUD, `/tournament/{id}`, `PATCH /{id}/status` |
| Eventos | `/api/match/{matchId}` | `result`, `goals`, `cards`, `lineup` |
| Estadísticas | `/api` | `standings`, `stats/scorers`, `stats/cards` |

La documentación completa está en Swagger al ejecutar el proyecto.

## Cómo ejecutarlo

**Requisitos:** .NET 8 SDK, SQL Server (local o Express) y Node.js 20+.

```bash
# 1. Backend: aplica las migraciones y carga datos de ejemplo al iniciar
dotnet run --project SportsLeague.API
# Swagger: http://localhost:5175/swagger
```

La cadena de conexión `DefaultConnection` está en `SportsLeague.API/appsettings.json` (por defecto usa `localhost` con autenticación de Windows).

```bash
# 2. Frontend
cd sports-league-frontend
npm install
npm start
# App: http://localhost:4200
```

## Autor

**Juan David Ruiz Murillo**: [LinkedIn](https://www.linkedin.com/in/juan-david-ruiz-murillo-13221a188/) · [GitHub](https://github.com/juanruiz164662)

Proyecto académico, Tecnología en Desarrollo de Software (ITM).
