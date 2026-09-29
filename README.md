# Cafetería Aromas - Proyecto de Estructuras de Datos

Proyecto desarrollado en **ASP.NET Core MVC (.NET 10)** para la materia **Estructuras de Datos**. La aplicación utiliza una base de datos relacional para la gestión y persistencia principal del catálogo, recetas, inventario, clientes y ventas de la cafetería, complementada con la implementación de estructuras de datos dinámicas en memoria para resolver procesos específicos del sistema.


## 🛠️ Tecnologías Utilizadas

* **Framework:** .NET 10 (ASP.NET Core MVC)
* **Lenguaje:** C#
* **ORM:** Entity Framework Core
* **Base de Datos:** SQL Server
* **Arquitectura:** Modelo-Vista-Controlador (MVC)
* **IDE's Usados:** Rider, VS Code, Visual Studio


## 📂 Estructura del Proyecto

```text
CafeteriaAromas/
├── Controllers/         # Controladores MVC (Acceso, Menú, Pedidos, Productos, Recetas, etc.)
├── Data/                # Contexto de base de datos (EF Core) y almacenamiento en memoria
├── DataStructures/      # Implementación de estructuras de datos propias (Clases e Interfaces)
│   ├── Clases/
│   └── Interfaces/
├── Models/              # Entidades del dominio (Productos, Ventas, Inventario, Clientes, etc.)
├── ViewModels/          # Modelos de vista para la transferencia de datos a la UI
├── Views/               # Vistas Razor organizadas por módulos del sistema
├── appsettings.json     # Configuración global del proyecto y base de datos
└── Program.cs           # Punto de entrada y configuración de servicios / Middleware
```

## 📐 Diagrama de Clases (UML)

```mermaid
classDiagram
    direction LR

    class SupplyCategory {
        +int Id
        +string Name
    }

    class ProductCategory {
        +int Id
        +string Name
    }

    class Supplier {
        +int Id
        +string Name
        +string Nit
        +string Phone
        +string Address
    }

    class Supply {
        +int Id
        +string Name
        +string UnitOfMeasure
        +int SupplyCategoryId
        +decimal StoredQuantity
    }

    class Inventory {
        +int Id
        +string Name
        +decimal UnitPrice
        +string UnitOfMeasure
        +decimal Stock
        +DateTime ExpirationDate
        +DateTime ProductionDate
        +int SupplyId
    }

    class InventoryPurchase {
        +int Id
        +decimal Quantity
        +string UnitOfMeasure
        +decimal Amount
        +DateTime PurchaseDate
        +int SupplierId
        +int InventoryId
    }

    class Product {
        +int Id
        +string Name
        +decimal SellingPrice
        +decimal ProductionCost
        +int ProductCategoryId
    }

    class Recipe {
        +int Id
        +string Instructions
        +int ProductId
    }

    class RecipeSupply {
        +int Id
        +decimal IngredientQuantity
        +int SupplyId
        +int RecipeId
    }

    class JobPosition {
        +int Id
        +string Name
        +string Description
    }

    class Employee {
        +int Id
        +string FirstName
        +string LastName
        +string SecondLastName
        +string DocumentId
        +string Phone
        +decimal Salary
        +string Email
        +int JobPositionId
    }

    class Customer {
        +int Id
        +string FirstName
        +string LastName
        +string SecondLastName
        +string DocumentId
        +string Phone
    }

    class Sale {
        +int Id
        +decimal TotalAmount
        +DateTime SaleDate
        +TimeSpan SaleTime
        +bool IsInvoiced
        +string InvoiceNumber
        +int CustomerId
        +int EmployeeId
    }

    class SaleDetail {
        +int Id
        +int Quantity
        +decimal Subtotal
        +int SaleId
        +int ProductId
    }

    class Warehouse {
        +int Id
        +string Name
    }

    class Machine {
        +int Id
        +string Name
        +string MachineFunction
        +string SerialNumber
        +int WarehouseId
    }

    class MachineMaintenance {
        +int Id
        +string Diagnosis
        +DateTime MaintenanceDate
        +decimal Cost
        +int MachineId
    }

%% Relaciones con conectores C# / UML
SupplyCategory "1" o-- "0..*" Supply : clasifica
Supply "1" -- "0..*" Inventory : compone
Supplier "1" -- "0..*" InventoryPurchase : provee
Inventory "1" -- "0..*" InventoryPurchase : es comprado en

ProductCategory "1" o-- "0..*" Product : categoriza
Product "1" *-- "1" Recipe : tiene
Recipe "1" *-- "1..*" RecipeSupply : requiere
Supply "1" -- "0..*" RecipeSupply : se usa en

JobPosition "1" -- "0..*" Employee : asigna
Employee "1" -- "0..*" Sale : registra
Customer "1" -- "0..*" Sale : realiza

Sale "1" *-- "1..*" SaleDetail : contiene
Product "1" -- "0..*" SaleDetail : se vende en

Warehouse "1" o-- "0..*" Machine : almacena
Machine "1" *-- "0..*" MachineMaintenance : recibe

```
