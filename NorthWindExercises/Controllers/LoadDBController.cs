using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NorthWindExercises.Modeldto;
using NorthWindExercises.Models;

namespace NorthWindExercises.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadDBController : ControllerBase
    {
        [HttpGet("Bai1")]
        public IEnumerable<string> Bai1()
        {
            var empList = NorthwindDbContext.INSTANCE.Employees.Select(x => new EmployeeDTO(x).FullName.ToLower()).ToList();
            return empList;
        }

        [HttpGet("Bai2")]
        public IEnumerable<string> Bai2()
        {
            var empList = NorthwindDbContext.INSTANCE.Employees
                .Select(x => new EmployeeDTO(x).FullName.ToUpper()).ToList();
            return empList;
        }

        [HttpGet("Bai3/Employees/{country}")]
        public IEnumerable<EmployeeDTO> Bai3(string country)
        {
            var empList = NorthwindDbContext.INSTANCE.Employees
                .Where(x => x.Country.ToLower().Equals(country.ToLower()))
                .Select(x => new EmployeeDTO(x)).ToList();
            return empList;
        }

        [HttpGet("Bai4/Customers/{country}")] //same as Bai5 and Bai6
        public IEnumerable<CustomerDTO> Bai4(string country)
        {
            var cusList = NorthwindDbContext.INSTANCE.Customers
                .Where(x => x.Country.ToLower().Equals(country.ToLower()))
                .Select(x => new CustomerDTO(x)).ToList();
            return cusList;
        }

        [HttpGet("Bai7/Products")]
        public IEnumerable<ProductDTO> Bai7([FromQuery] int min, [FromQuery] int max)
        {
            var proList = NorthwindDbContext.INSTANCE.Products
                .Where(x => x.UnitsInStock >= min && x.UnitsInStock <= max)
                .Select(x => new ProductDTO(x)).ToList();
            return proList;
        }

        [HttpGet("Bai8/Products/{min}/{max}")]
        public IEnumerable<ProductDTO> Bai8(int min, int max)
        {
            var proList = NorthwindDbContext.INSTANCE.Products
                .Where(x => x.UnitsOnOrder >= min && x.UnitsOnOrder <= max)
                .Select(x => new ProductDTO(x)).ToList();
            return proList;
        }

        [HttpGet("Bai9/EmployeeOrderCounts/{year}")] //same as Bai10
        public IEnumerable<object> Bai9(int year)
        {
            var result = NorthwindDbContext.INSTANCE.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.LastName,
                    e.FirstName,
                    e.Title,
                    year,
                    totalOrders = e.Orders.Count(o => o.OrderDate.Value.Year == year)
                })
                .ToList();

            return result;
        }

        [HttpGet("Bai11/EmployeeOrderCountsByDateRange/{startDate}/{endDate}")] //same as Bai12
        public IEnumerable<object> Bai11(DateTime startDate, DateTime endDate)
        {
            var result = NorthwindDbContext.INSTANCE.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.LastName,
                    e.FirstName,
                    e.Title,
                    e.HireDate,
                    totalOrders = e.Orders.Count(o =>
                        o.OrderDate.Value >= startDate &&
                        o.OrderDate.Value <= endDate)
                })
                .ToList();

            return result;
        }

        [HttpGet("Bai13/OrderFreightWithTax/{startDate}/{endDate}")]
        public IEnumerable<object> Bai13(DateTime startDate, DateTime endDate)
        {
            var orders = NorthwindDbContext.INSTANCE.Orders
                .Where(o => o.OrderDate.HasValue &&
                            o.OrderDate.Value >= startDate &&
                            o.OrderDate.Value <= endDate)
                .Select(o => new
                {
                    o.OrderId,
                    o.Freight,
                    o.OrderDate.Value.Day,
                    o.OrderDate.Value.Month,
                    o.OrderDate.Value.Year,
                    TaxRate = (o.Freight.Value >= 100) ? "10%" : "5%",
                    FreightWithTax = o.Freight.Value * (1 + ((o.Freight.Value >= 100) ? 0.10m : 0.05m))

                })
                .ToList();

            return orders;
        }
        [HttpGet("Bai14/EmployeeGender")]
        public IEnumerable<object> Bai14()
        {
            var result = NorthwindDbContext.INSTANCE.Employees
                .Where(e => e.TitleOfCourtesy == "Mr." || e.TitleOfCourtesy == "Mrs." || e.TitleOfCourtesy == "Ms.")
                .Select(e => new
                {
                    FullName = e.LastName + " " + e.FirstName,
                    e.TitleOfCourtesy,
                    Sex = e.TitleOfCourtesy == "Mr." ? "Male" : "Female"
                })
                .ToList();
            return result;
        }

        [HttpGet("Bai15/EmployeeGender")]
        public IEnumerable<object> Bai15()
        {
            var result = NorthwindDbContext.INSTANCE.Employees

                .Select(e => new
                {
                    FullName = e.LastName + " " + e.FirstName,
                    e.TitleOfCourtesy,
                    Sex = (e.TitleOfCourtesy == "Mr." || e.TitleOfCourtesy == "Dr.") ? "M" : "F"
                })
                .ToList();
            return result;
        }

        [HttpGet("Bai16/EmployeeGender")] //same as Bai17 and Bai18
        public IEnumerable<object> Bai16()
        {
            var result = NorthwindDbContext.INSTANCE.Employees
                .Select(e => new
                {
                    FullName = e.LastName + " " + e.FirstName,
                    e.TitleOfCourtesy,
                    Sex = e.TitleOfCourtesy == "Mr." ? "Male"
                        : (e.TitleOfCourtesy == "Mrs." || e.TitleOfCourtesy == "Ms.") ? "Female"
                        : "Unknown"
                })
                .ToList();
            return result;
        }

        [HttpGet("Bai21/ProductRevenue/{startDate}/{endDate}")]
        public IEnumerable<object> Bai21(DateTime startDate, DateTime endDate)
        {
            var revenues = NorthwindDbContext.INSTANCE.OrderDetails
                .Where(od => od.Order.OrderDate.Value >= startDate
                    && od.Order.OrderDate.Value <= endDate)
                .Select(od => new
                {
                    od.Product.CategoryId,
                    od.Product.Category.CategoryName,
                    od.ProductId,
                    od.Product.ProductName,
                    od.Order.OrderDate.Value.Day,
                    od.Order.OrderDate.Value.Month,
                    od.Order.OrderDate.Value.Year,
                    Revenue = od.Quantity * od.UnitPrice
                })
                .OrderBy(x => x.CategoryId)
                .ThenBy(x => x.ProductId)
                .ToList();

            return revenues;
        }

        [HttpGet("Bai22/LateOrders/{lateDays}")]
        public IEnumerable<object> Bai22(int lateDays)
        {
            var lateOrders = NorthwindDbContext.INSTANCE.Orders
     .Include(o => o.Employee)
     .AsEnumerable()
     .Where(o => o.ShippedDate.HasValue && o.RequiredDate.HasValue &&
                 (o.ShippedDate.Value - o.RequiredDate.Value).TotalDays == lateDays)
     .Select(o => new
     {
         o.EmployeeId,
         o.Employee?.LastName,
         o.Employee?.FirstName,
         o.OrderId,
         o.OrderDate,
         o.RequiredDate,
         o.ShippedDate,
     })
     .ToList();

            return lateOrders;
        }

        [HttpGet("Bai23/People/{letter}")]
        public IEnumerable<object> Bai23(char letter)
        {
            var cus = NorthwindDbContext.INSTANCE.Customers
                .Where(e => e.CompanyName.StartsWith(letter.ToString()))
                .Select(e => new
                {
                    e.CompanyName,
                    e.Phone
                });
            var emp = NorthwindDbContext.INSTANCE.Employees.Select(emp => new
            {
                CompanyName = emp.LastName + " " + emp.FirstName,
                Phone = emp.HomePhone
            });
            var result = cus.Concat(emp).ToList();
            return result;
        }

        [HttpGet("Bai24/Customers/{orderId}")]
        public IEnumerable<object> Bai24(int orderId)
        {
            var result = NorthwindDbContext.INSTANCE.Customers
                .Where(c => c.Orders.Any(o => o.OrderId == orderId))
                .Select(c => new
                {
                    c.CustomerId,
                    c.CompanyName,
                    c.ContactName,
                    c.ContactTitle
                })
                .ToList();
            return result;
        }

        [HttpGet("Bai25/TopOrderedProducts/{units}")]
        public IEnumerable<object> Bai25(int units)
        {
            var result = NorthwindDbContext.INSTANCE.OrderDetails
                .GroupBy(od => new { od.ProductId, od.Product.ProductName })
                .Select(g => new
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    TotalUnitsOrdered = g.Sum(od => od.Quantity)
                })
                .Where(x => x.TotalUnitsOrdered >= units)
                .ToList();

            return result;
        }

    }
}
