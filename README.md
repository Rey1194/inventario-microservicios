# Inventario Microservicios - .NET Core

## 📌 Requisitos

Antes de ejecutar el proyecto, asegúrate de cumplir con los siguientes requisitos:

### 🔹 Software Necesario
1. **Sistema Operativo**: Windows 10/11 o Linux con .NET Core
2. **.NET SDK**: [Descargar .NET 6.0+](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
3. **SQL Server**: [Descargar SQL Server 2019+](https://www.microsoft.com/es-es/sql-server/sql-server-downloads)
4. **SQL Server Management Studio (SSMS)**: [Descargar SSMS](https://aka.ms/ssmsfullsetup)
5. **Visual Studio 2022** con la carga de trabajo "Desarrollo de ASP.NET y web"
6. **Postman o cURL** para probar las APIs

### 🔹 Configuración de Base de Datos
1. **Iniciar SQL Server**
2. **Ejecutar el script `InventoryDB.sql`** en SQL Server Management Studio (SSMS) para crear la base de datos.
   
   ```sql
   USE master;
   GO
   CREATE DATABASE InventoryDB;
   GO
   USE InventoryDB;
   GO
   ```
3. **Modificar `appsettings.json` en cada microservicio** si es necesario.

   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

---

## 🚀 Ejecución del Backend

### 1️⃣ Clonar el Repositorio
```sh
git clone https://github.com/usuario/inventario-microservicios.git
cd inventario-microservicios
```

### 2️⃣ Restaurar Dependencias
```sh
dotnet restore
```

### 3️⃣ Generar la Base de Datos con Entity Framework
```sh
dotnet ef migrations add InitialCreate --project ProductsService --startup-project ProductsService
dotnet ef database update --project ProductsService --startup-project ProductsService
```
```sh
dotnet ef migrations add InitialCreate --project TransactionsService --startup-project TransactionsService
dotnet ef database update --project TransactionsService --startup-project TransactionsService
```

### 4️⃣ Ejecutar los Microservicios
En Visual Studio 2022, **selecciona "Multiple Startup Projects"** para ejecutar `ProductsService` y `TransactionsService` juntos.

O usa la terminal:
```sh
dotnet run --project ProductsService
```
```sh
dotnet run --project TransactionsService
```

### 5️⃣ Probar los Endpoints

📌 **Productos**
```sh
curl -X GET https://localhost:5001/api/products
```
📌 **Transacciones**
```sh
curl -X GET https://localhost:5002/api/transactions
```

Si ves respuestas JSON, ¡la configuración está lista! ✅

---

## 🛠 Troubleshooting

### ❌ Error SSL (La cadena de certificación no es confiable)
- **Solución**: Agregar `TrustServerCertificate=True` en la cadena de conexión en `appsettings.json`.

### ❌ Base de Datos No Se Conecta
- **Verificar**: Que el SQL Server está en ejecución.
- **Ejecutar en SSMS**:
  ```sql
  SELECT name FROM sys.databases;
  ```
  Debe mostrar `InventoryDB`.

### ❌ Puerto en Uso
- Cambia los puertos en `Properties/launchSettings.json` o usa:
  ```sh
  dotnet run --urls=https://localhost:5003
  ```
---

## 📜 Licencia
Este proyecto está bajo la licencia MIT. ¡Úsalo libremente!

