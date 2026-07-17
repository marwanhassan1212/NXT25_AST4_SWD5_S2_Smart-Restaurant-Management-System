Create Database RestaurantDB
go
use RestaurantDB
go
-- Roles
create table Roles
(
    RoleId int primary key identity(1,1),
    RoleName nvarchar(50) not null
);
go
--Employees
create table Employees
(
    EmployeeId int primary key identity(1,1),
    FirstName nvarchar(50) not null,
    LastName nvarchar(50) not null,
    PhoneNumber nvarchar(20),
    Gender nvarchar(10),
    HireDate date not null,
    Salary decimal(10,2) not null,
    JobTitle nvarchar(50),
    UserName nvarchar(50) not null,
    Password nvarchar(100) not null,

    RoleId int not null,

    constraint FK_Employees_Roles
        foreign key (RoleId)
        references Roles(RoleId)
);
go

--Categories
create table Categories
(
    CategoryId int primary key identity(1,1),
    CategoryName nvarchar(100) not null
);
go

-- MenuItems
create table MenuItems
(
    ItemId int primary key identity(1,1),
    ItemName nvarchar(100) not null,
    Description nvarchar(250),
    Price decimal(10,2) not null,
    AvailabilityStatus bit not null,

    CategoryId int not null,

    constraint FK_MenuItems_Categories
        foreign key (CategoryId)
        references Categories(CategoryId)
);
go

-- Customers
create table Customers
(
    CustomerId int primary key identity(1,1),
    FullName nvarchar(100) not null,
    PhoneNumber nvarchar(20)
);
go

-- RestaurantTables
create table RestaurantTables
(
    TableId int primary key identity(1,1),
    Capacity int not null,
    Status nvarchar(20) not null
);
go

-- Orders
create table Orders
(
    OrderId int primary key identity(1,1),
    OrderDate datetime not null,
    OrderStatus nvarchar(30) not null,
    TotalAmount decimal(10,2) not null,

    EmployeeId int not null,
    CustomerId int not null,
    TableId int not null,

    constraint FK_Orders_Employees
        foreign key (EmployeeId)
        references Employees(EmployeeId),

    constraint FK_Orders_Customers
        foreign key (CustomerId)
        references Customers(CustomerId),

    constraint FK_Orders_RestaurantTables
        foreign key (TableId)
        references RestaurantTables(TableId)
);
go

-- OrderDetails
create table OrderDetails
(
    OrderId int not null,
    ItemId int not null,

    Quantity int not null,
    UnitPrice decimal(10,2) not null,

    constraint PK_OrderDetails
        primary key (OrderId, ItemId),

    constraint FK_OrderDetails_Orders
        foreign key (OrderId)
        references Orders(OrderId),

    constraint FK_OrderDetails_MenuItems
        foreign key (ItemId)
        references MenuItems(ItemId)
);


