CREATE TABLE Roles (
    RoleID INT PRIMARY KEY,
    RoleName VARCHAR(100)
);

CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    BirthDate DATE,
    PhoneNumber VARCHAR(20),
    Gender VARCHAR(10),
    HireDate DATE,
    Salary DECIMAL(10,2),
    JobTitle VARCHAR(100),
    RoleID INT,
    SupervisorID INT,

    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID),
    FOREIGN KEY (SupervisorID) REFERENCES Employees(EmployeeID)
);

CREATE TABLE Attendance (
    AttendanceID INT PRIMARY KEY,
    WorkDate DATE,
    CheckInTime TIME,
    CheckOutTime TIME,
    EmployeeID INT,

    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);

CREATE TABLE AuditLog (
    LogID INT PRIMARY KEY,
    ActionType VARCHAR(100),
    ActionDateTime DATETIME,
    EmployeeID INT,

    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);

CREATE TABLE Category (
    CategoryID INT PRIMARY KEY,
    CategoryName VARCHAR(100)
);

CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100),
    Description TEXT,
    Price DECIMAL(10,2),
    CostPrice DECIMAL(10,2),
    CategoryID INT,

    FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID)
);

CREATE TABLE Inventory (
    ProductID INT PRIMARY KEY,
    QuantityInStock INT,
    MinimumLevel INT,
    LastUpdatedDate DATE,

    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    OrderDate DATE,
    OrderStatus VARCHAR(50),
    SubTotal DECIMAL(10,2),
    Discount DECIMAL(10,2),
    Tax DECIMAL(10,2),
    TotalAmount DECIMAL(10,2),
    EmployeeID INT,

    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);

CREATE TABLE OrderDetails (
    OrderID INT,
    ProductID INT,
    Quantity INT,
    UnitPrice DECIMAL(10,2),

    PRIMARY KEY (OrderID, ProductID),

    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

CREATE TABLE Suppliers (
    SupplierID INT PRIMARY KEY,
    SupplierName VARCHAR(100),
    Phone VARCHAR(20),
    Email VARCHAR(100),
    Address VARCHAR(200)
);

CREATE TABLE PurchaseTransaction (
    PurchaseID INT PRIMARY KEY,
    PurchaseDate DATE,
    PaymentStatus VARCHAR(50),
    TotalAmount DECIMAL(10,2),
    SupplierID INT,

    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID)
);

CREATE TABLE PurchaseDetails (
    PurchaseID INT,
    ProductID INT,
    Quantity INT,
    UnitCost DECIMAL(10,2),

    PRIMARY KEY (PurchaseID, ProductID),

    FOREIGN KEY (PurchaseID) REFERENCES PurchaseTransaction(PurchaseID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

CREATE TABLE SupplierProducts (
    SupplierID INT,
    ProductID INT,

    PRIMARY KEY (SupplierID, ProductID),

    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);



