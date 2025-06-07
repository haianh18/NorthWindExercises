using Microsoft.AspNetCore.Mvc;
using NorthWindExercises.Models;

namespace NorthWindExercises.Controllers
{
    [Route("HE182182/[controller]")]
    [ApiController]
    public class LoadDBController : ControllerBase
    {
        //Basic SELECT Queries
        [HttpGet("Ex1/HE182182/ListAllProducts")]
        public List<Product> ListAllProducts()
        {
            return NorthwindDbContext.INSTANCE.Products.ToList();
        }

        [HttpGet("Ex2/HE182182/ShowCustomerName&Phone")]
        public IEnumerable<Customer> ShowCustomerNameAndPhone()
        {
            return NorthwindDbContext.INSTANCE.Customers
                .Select(c => new Customer { CompanyName = c.CompanyName, Phone = c.Phone })
                .ToList();
        }

        [HttpGet("Ex10/HE182182/ListFirstCustomers/{topNumber}")]
        public IEnumerable<Customer> List10FirstCustomers(int topNumber)
        {
            return NorthwindDbContext.INSTANCE.Customers.Take(topNumber).ToList();
        }

        //Filtering and Conditions
        [HttpGet("Ex11/HE182182/ProductsOutOfStock")]
        public IEnumerable<Product> ProductsOutOfStock()
        {
            return NorthwindDbContext.INSTANCE.Products
                .Where(p => p.UnitsInStock == 0)
                .ToList();
        }

        [HttpGet("Ex12/HE182182/Orders/{startDate}")]
        public IEnumerable<Order> GetOrdersFromDate(DateTime startDate)
        {
            var orders = NorthwindDbContext.INSTANCE.Orders.Where(o => o.ShippedDate >= startDate).ToList();
            return orders;
        }

        //JOINS
        [HttpGet("Ex16/HE182182/OrdersWithCustomerName")]
        public IEnumerable<object> OrdersWithCustomerName()
        {
            return NorthwindDbContext.INSTANCE.Orders
                .Select(o => new
                {
                    o.OrderId,
                    CustomerName = o.Customer.CompanyName,
                    o.OrderDate,
                    o.ShippedDate
                })
                .ToList();
        }

        [HttpGet("Ex20/HE182182/Employees&Supervisors")]
        public IEnumerable<object> EmployeesAndSupervisors()
        {
            return NorthwindDbContext.INSTANCE.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    Supervisor = e.ReportsToNavigation != null ? $"{e.ReportsToNavigation.FirstName} {e.ReportsToNavigation.LastName}" : "None"
                })
                .ToList();
        }

        // Aggregations and GROUP BY
        [HttpGet("Ex21/HE182182/CountCustomersPerCountry")]
        public IEnumerable<object> CountCustomersPerCountry()
        {
            return NorthwindDbContext.INSTANCE.Customers
                .GroupBy(c => c.Country)
                .Select(g => new
                {
                    Country = g.Key,
                    CustomerCount = g.Count()
                })
                .ToList();
        }

        [HttpGet("Ex24/HE182182/AverageUnitPricePerCategory")]
        public IEnumerable<object> AverageUnitPricePerCategory()
        {
            return NorthwindDbContext.INSTANCE.Products
                .GroupBy(p => p.CategoryId)
                .Select(g => new
                {
                    CategoryId = g.Key,
                    AveragePrice = g.Average(p => p.UnitPrice) ?? 0
                })
                .ToList();
        }

        //HAVING and Filtering Groups
        [HttpGet("Ex26/HE182182/CustomersWithOrdersMoreThan/{orderAmount}")]
        public IEnumerable<object> CustomersWithOrdersAboveAmount(int orderNumbers)
        {
            return NorthwindDbContext.INSTANCE.Orders
                .GroupBy(o => o.CustomerId)
                .Where(g => g.Count() > orderNumbers)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    OrderCount = g.Count()
                })
                .ToList();
        }

        [HttpGet("Ex30/HE182182/CategoriesWithAverageUnitPrice/{unitPrice}")]
        public IEnumerable<object> CategoriesWithAverageUnitPriceAbove(decimal unitPrice)
        {
            return NorthwindDbContext.INSTANCE.Products
                .GroupBy(p => p.CategoryId)
                .Where(g => g.Average(p => p.UnitPrice) > unitPrice)
                .Select(g => new
                {
                    CategoryId = g.Key,
                    AverageUnitPrice = g.Average(p => p.UnitPrice) ?? 0
                })
                .ToList();
        }

        //Subqueries
        [HttpGet("Ex31/HE182182/ProductsWithHighestPrice")]
        public IEnumerable<Product> ProductsWithHighestPrice()
        {
            var maxPrice = NorthwindDbContext.INSTANCE.Products.Max(p => p.UnitPrice);
            return NorthwindDbContext.INSTANCE.Products
                .Where(p => p.UnitPrice == maxPrice)
                .ToList();
        }

        [HttpGet("Ex35/HE182182/CustomersWithOrder/{productName}")]
        public IEnumerable<object> CustomersWithOrderForProduct(string productName)
        {
            return NorthwindDbContext.INSTANCE.Orders.Where(o => o.OrderDetails.Any(od => od.Product.ProductName == productName))
                .Select(o => new
                {
                    o.CustomerId,
                    CustomerName = o.Customer.CompanyName,
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate

                })
                .ToList();
        }

        //ORDER BY, LIMIT, TOP
        [HttpGet("Ex36/HE182182/TopProductsByPrice/{topNumber}")]
        public IEnumerable<Product> TopProductsByPrice(int topNumber)
        {
            return NorthwindDbContext.INSTANCE.Products
                .OrderByDescending(p => p.UnitPrice)
                .Take(topNumber)
                .ToList();
        }

        [HttpGet("Ex40/HE182182/CategoriesOrderedByProductts")]
        public IEnumerable<object> CategoriesOrderedByProducts()
        {
            return NorthwindDbContext.INSTANCE.Categories
                .Select(c => new
                {
                    c.CategoryId,
                    c.CategoryName,
                    ProductCount = c.Products.Count()
                })
                .OrderByDescending(c => c.ProductCount)
                .ToList();
        }

        //Date Functions and Calculations
        [HttpGet("Ex42/HE182182/EmployeeAge")]
        public IEnumerable<object> EmployeeAge()
        {
            return NorthwindDbContext.INSTANCE.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    Age = e.BirthDate.HasValue
                        ? DateTime.Now.Year - e.BirthDate.Value.Year
                        : 0
                })
                .ToList();
        }

        [HttpGet("Ex45/HE182182/OrderDetails/{orderYear}")]
        public IEnumerable<object> OrderDetailInYear(int orderYear)
        {
            var orders = NorthwindDbContext.INSTANCE.Orders
                .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == orderYear)
                .SelectMany(o => o.OrderDetails.Select(od => new
                {
                    od.Product.ProductName,
                    od.Quantity,
                    od.UnitPrice,
                    OrderDate = o.OrderDate.Value
                }))
                .ToList();
            return orders;
        }

        //String and Data Type Functions
        [HttpGet("Ex46/HE182182/CustomersWithFaxNumbers")]
        public IEnumerable<Customer> CustomersWithFaxNumbers()
        {
            return NorthwindDbContext.INSTANCE.Customers
                .Where(c => !string.IsNullOrEmpty(c.Fax))
                .ToList();
        }

        [HttpGet("Ex50/HE182182/SuppliersWhereRegionNull")]
        public IEnumerable<Supplier> SuppliersWhereRegionNull()
        {
            return NorthwindDbContext.INSTANCE.Suppliers
                .Where(s => string.IsNullOrEmpty(s.Region))
                .ToList();
        }
    }
}