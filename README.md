# pruebaDoubleV

## Resumen del proyecto
Este repositorio contiene la solución desarrollada para el ejercicio técnico descrito en `PT_WTW_Desarrollador .NET.pdf`. La solución incluye:

- Backend en .NET (ASP.NET Core) con endpoints REST, autenticación y persistencia.
- Frontend (SPA) que consume la API.
- Base de datos con datos de prueba y un usuario administrador seeded.
- Dockerización de los tres componentes y orquestación con Docker Compose.

Este README explica cómo levantar el proyecto con Docker, qué rutas usar y cómo desplegar sin Docker (por separado).

---

## Qué implementé
- Backend (ASP.NET Core):
  - Endpoints CRUD solicitados.
  - Autenticación/Autorización implementada.
  - Persistencia con EF Core (migrations y seed data).
  - Documentación con Swagger/OpenAPI.
- Frontend (SPA):
  - Login, listados, creación/edición según requerimientos.
  - Configuración para apuntar a la API mediante variable de entorno.
- Orquestación:
  - Dockerfiles para backend y frontend y un `docker-compose.yml` para levantar db + backend + frontend.
- Datos de prueba:
  - Usuario administrador seeded en la base de datos: user `admin` / password `Pass123*`.

---

## Credenciales de prueba
- Usuario: `admin`  
- Contraseña: `Pass123*`
- 
---

## Ejecución con Docker / Docker Compose (recomendado)
Requisitos:
- Docker instalado
- Docker Compose (o `docker compose` en Docker Engine moderno)

1. Desde la raíz del repositorio:
```bash
# Levanta y construye los servicios en background
docker compose up --build -d
```

2. Comprueba que los contenedores estén corriendo:
```bash
docker ps
```

3. Ver logs:
```bash
docker compose logs -f
```

Puertos:
- Frontend: http://localhost:4200
  - Login: http://localhost:4200/login
- Backend (API): http://localhost:5000 (o http://localhost:5001 si está HTTPS)
  - Swagger: http://localhost:5000/swagger
- Base de datos:
  - SQL Server típico: puerto 1433

---

## Desplegar sin Docker (pasos separados)

1) Base de datos
- Instalar/levantar la base de datos (SQL Server / Postgres según lo use el proyecto).
- Crear la base de datos con el nombre usado por la app (ver `appsettings.json` / variables de conexión).
- Aplicar migraciones (si aplica EF Core):
```bash
# Instala dotnet-ef si no lo tienes
dotnet tool install --global dotnet-ef

# Desde la carpeta del proyecto backend (ajusta la ruta al proyecto)
cd src/Proyecto.Backend
dotnet ef database update
```
2) Backend (.NET)
- Instalar el .NET SDK compatible con el proyecto (ver global.json o README interno).
- Configurar la cadena de conexión en `appsettings.Development.json` o usar variable de entorno:
  - Ejemplo variable: `ConnectionStrings__DefaultConnection="Server=...;Database=...;User Id=...;Password=...;"`
- Restaurar y ejecutar:
```bash
cd src/Proyecto.Backend
dotnet restore
dotnet run --urls "http://localhost:5000"
```
- Para producción: `dotnet publish -c Release -o ./publish` y desplegar con Kestrel/IIS o detrás de un reverse-proxy.

3) Frontend (Angular)
- Instalar dependencias y ejecutar en modo desarrollo:
```bash
cd /client
npm install
npm start
```
- Para producción:
```bash
npm run build
# Servir la carpeta build con nginx o con 'serve'
npx serve -s build -l 4200
```
---

## Comprobación de endpoints y pruebas
- Abrir Swagger (http://localhost:5000/swagger) para listar y probar endpoints.
- Iniciar frontend y usar la UI para probar login / flujos CRUD.

---