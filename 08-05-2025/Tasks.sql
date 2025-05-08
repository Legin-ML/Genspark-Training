
--1) List all orders with the customer name and the employee who handled the order.
--(Join Orders, Customers, and Employees)

SELECT o.OrderID, c.CompanyName as 'Customer Name', concat(e.FirstName, ' ', e.LastName) as 'Employee Name' from Orders as o join Employees as e on o.EmployeeID = e.EmployeeID join Customers as c on o.CustomerID = c.CustomerID

--2) Get a list of products along with their category and supplier name.
--(Join Products, Categories, and Suppliers)

SELECT p.ProductName, c.CategoryName, s.CompanyName as Supplier from Products as p
join Categories as c on p.CategoryID = c.CategoryID
join Suppliers as s on s.SupplierID = p.SupplierID

--3) Show all orders and the products included in each order with quantity and unit price.
--(Join Orders, Order Details, Products)

SELECT o.OrderID, p.ProductName, od.UnitPrice, od.Quantity from Orders as o
join [Order Details] as od on o.OrderID = od.OrderID
join Products as p on od.ProductID = p.ProductID

--4) List employees who report to other employees (manager-subordinate relationship).
--(Self join on Employees)

SELECT concat(e.FirstName, ' ', e.LastName) as 'Employee Name' from Employees as e where e.ReportsTo IS NOT NULL

SELECT 
  CONCAT(e.FirstName, ' ', e.LastName) AS 'Employee Name',
  CONCAT(m.FirstName, ' ', m.LastName) AS 'Manager Name'
FROM Employees AS e
JOIN Employees AS m ON e.ReportsTo = m.EmployeeID

--5) Display each customer and their total order count.
--(Join Customers and Orders, then GROUP BY)

SELECT c.CompanyName, COUNT(o.OrderID) as 'Order Count' from Customers as c left join Orders as o on o.CustomerID = c.CustomerID GROUP BY c.CompanyName

--6) Find the average unit price of products per category.
--Use AVG() with GROUP BY

SELECT c.CategoryName, AVG(o.UnitPrice) as 'Average' from [Order Details] as o
join Products as p on o.ProductID = p.ProductID
join Categories as c on c.CategoryID = p.CategoryID
GROUP BY c.CategoryName

--7) List customers where the contact title starts with 'Owner'.
--Use LIKE or LEFT(ContactTitle, 5)

SELECT CustomerID, CompanyName FROM Customers 
WHERE ContactTitle LIKE 'Owner%'

--8) Show the top 5 most expensive products.
--Use ORDER BY UnitPrice DESC and TOP 5

SELECT TOP 5 ProductName, UnitPrice FROM Products ORDER BY UnitPrice DESC

--9) Return the total sales amount (quantity × unit price) per order.
--Use SUM(OrderDetails.Quantity * OrderDetails.UnitPrice) and GROUP BY

SELECT OrderID, SUM(Quantity * UnitPrice) as 'Amount' FROM [Order Details] GROUP BY OrderID


--10) Create a stored procedure that returns all orders for a given customer ID.
--Input: @CustomerID
GO
CREATE OR ALTER PROC proc_GetCustomerOrders(@CustomerID nvarchar(max))
AS
BEGIN
	SELECT * FROM Orders WHERE CustomerID = @CustomerID
END

EXEC proc_GetCustomerOrders 'VINET'


--11) Write a stored procedure that inserts a new product.
--Inputs: ProductName, SupplierID, CategoryID, UnitPrice, etc.

GO
CREATE OR ALTER PROC proc_InsertProduct @PID int, @PName nvarchar(40), @SID int = NULL, @CID int = NULL, 
@QuantityPU nvarchar(20) = NULL, @UnitP money = NULL, @InStock smallint = NULL,
@OnOrder smallint = NULL, @ReorderLevel smallint = NULL, @Discontinued BIT
AS
BEGIN
	BEGIN TRY
		INSERT INTO Products (
		ProductID,
		ProductName,
		SupplierID,
		CategoryID,
		QuantityPerUnit,
		UnitPrice,
		UnitsInStock,
		UnitsOnOrder,
		ReorderLevel,
		Discontinued) VALUES (
		    @PID, 
			@PName,
			@SID, 
			@CID, 
			@QuantityPU, 
			@UnitP, 
			@InStock, 
			@OnOrder, 
			@ReorderLevel, 
			@Discontinued)
		END TRY
		BEGIN CATCH
        PRINT 'Error occurred while inserting the product: ' + ERROR_MESSAGE();
    END CATCH
END

--12) Create a stored procedure that returns total sales per employee.
--Join Orders, Order Details, and Employees
GO
CREATE OR ALTER PROC proc_GetSales(@EmployeeID int)
AS
BEGIN
SELECT CONCAT(e.FirstName, ' ', e.LastName) as EmployeeName, SUM((od.UnitPrice * od.Quantity) - ((od.UnitPrice * od.Quantity) * od.Discount)) as 'Total Sales' 
from Employees as e join Orders as o on o.EmployeeID = e.EmployeeID
join [Order Details] as od on od.OrderID = o.OrderID
WHERE o.EmployeeID = @EmployeeID
GROUP BY e.FirstName, e.LastName
ORDER BY 'Total Sales' DESC
END

exec proc_GetSales 1


--13) Use a CTE to rank products by unit price within each category.
--Use ROW_NUMBER() or RANK() with PARTITION BY CategoryID
GO
WITH ProductRankings AS (
    SELECT 
        ProductID,
        ProductName,
        CategoryID,
        UnitPrice,
        RANK() OVER (PARTITION BY CategoryID ORDER BY UnitPrice DESC) AS RankByPrice
    FROM Products
)
SELECT *
FROM ProductRankings
ORDER BY CategoryID, RankByPrice ASC

--14) Create a CTE to calculate total revenue per product and filter products with revenue > 10,000.
GO
WITH ProductRevenue AS (
    SELECT 
        ProductID,
        SUM(UnitPrice * Quantity) AS TotalRevenue
    FROM [Order Details]
    GROUP BY ProductID
)
SELECT *
FROM ProductRevenue
WHERE TotalRevenue > 10000

--15) Use a CTE with recursion to display employee hierarchy.
--Start from top-level employee (ReportsTo IS NULL) and drill down
GO
WITH EmployeeHierarchy AS (

    SELECT 
        EmployeeID, 
        ReportsTo, 
        CONCAT(FirstName, ' ', LastName) as EmployeeName, 
        0 AS Level  
    FROM Employees
    WHERE ReportsTo IS NULL

    UNION ALL

    SELECT 
        e.EmployeeID, 
        e.ReportsTo, 
        CONCAT(e.FirstName, ' ', e.LastName) as EmployeeName, 
        eh.Level + 1 AS Level  
    FROM Employees e
    INNER JOIN EmployeeHierarchy eh
        ON e.ReportsTo = eh.EmployeeID
)

SELECT 
    EmployeeID, 
    EmployeeName, 
    ReportsTo, 
    Level
FROM EmployeeHierarchy
ORDER BY Level
