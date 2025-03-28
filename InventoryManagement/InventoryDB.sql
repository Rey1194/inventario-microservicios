-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'InventoryDB')
BEGIN
    CREATE DATABASE InventoryDB;
END
GO

-- Usar la base de datos
USE InventoryDB;
GO

-- Crear tabla de productos
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
BEGIN
    CREATE TABLE Products (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(255) NOT NULL,
        Description NVARCHAR(500),
        Category NVARCHAR(100),
        ImageUrl NVARCHAR(500),
        Price DECIMAL(18,2) NOT NULL,
        Stock INT NOT NULL CHECK (Stock >= 0) -- No permitir stock negativo
    );
END
GO

-- Crear tabla de transacciones (compras/ventas)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Transactions' AND xtype='U')
BEGIN
    CREATE TABLE Transactions (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Date DATETIME DEFAULT GETDATE(),
        TransactionType NVARCHAR(10) CHECK (TransactionType IN ('compra', 'venta')),
        ProductId INT NOT NULL,
        Quantity INT NOT NULL CHECK (Quantity > 0), -- La cantidad debe ser positiva
        UnitPrice DECIMAL(18,2) NOT NULL,
        TotalPrice AS (Quantity * UnitPrice) PERSISTED, -- Campo calculado
        Details NVARCHAR(500),
        FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE
    );
END
GO

-- Insertar datos de prueba en Products
IF NOT EXISTS (SELECT * FROM Products)
BEGIN
    INSERT INTO Products (Name, Description, Category, ImageUrl, Price, Stock)
    VALUES 
        ('Laptop', 'Laptop Dell Inspiron', 'Electronics', 'laptop.jpg', 1200.50, 10),
        ('Mouse', 'Mouse inalámbrico Logitech', 'Electronics', 'mouse.jpg', 25.99, 50),
        ('Teclado', 'Teclado mecánico RGB', 'Electronics', 'keyboard.jpg', 75.00, 30);
END
GO

-- Insertar datos de prueba en Transactions
IF NOT EXISTS (SELECT * FROM Transactions)
BEGIN
    INSERT INTO Transactions (TransactionType, ProductId, Quantity, UnitPrice, Details)
    VALUES 
        ('compra', 1, 5, 1200.50, 'Compra inicial de laptops'),
        ('venta', 2, 3, 25.99, 'Venta de mouse inalámbrico');
END
GO
