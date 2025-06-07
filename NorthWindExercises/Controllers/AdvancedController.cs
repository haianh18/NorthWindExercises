using Microsoft.AspNetCore.Mvc;
using NorthWindExercises.Models;

namespace NorthWindExercises.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvancedController : ControllerBase
    {
        //List all products and each product includes the customers who ordered it
        //returns [{pro1, [cus1, cus2]}, {pro2}] for example
        //in which customers' id and company name will be shown
        [HttpGet("Ex1/ProductsWithCustomers")]
        public IActionResult ProductsWithCustomers()
        {
            var productsWithCustomers = NorthwindDbContext.INSTANCE.Products
                .Select(p => new
                {
                    Product = p,
                    Customers = NorthwindDbContext.INSTANCE.OrderDetails
                        .Where(od => od.ProductId == p.ProductId)
                        .Select(od => new
                        {
                            od.Order.Customer.CustomerId,
                            od.Order.Customer.CompanyName
                        })
                        .Distinct()
                        .ToList()
                })
                .ToList();
            return Ok(productsWithCustomers);
        }

        //In ra thong tin cua san pham khi nhap vao Id, thong tin bao gom them danh sach nhung khach hang da mua san pham do
        //va hien thi them thong tin tat ca cac Shipper da ship san pham do
        [HttpGet("Ex2/ProductDetails/{productId}")]
        public IActionResult Products(int productId)
        {
            var result = NorthwindDbContext.INSTANCE.Products.Where(pro => pro.ProductId == productId)
                .Select(p => new
                {
                    Product = p,
                    Customers = p.OrderDetails.Select(od => new
                    {
                        od.Order.Customer.CustomerId,
                        od.Order.Customer.CompanyName
                    }).Distinct().ToList(),
                    Shippers = p.OrderDetails
                    .Select(od => od.Order.ShipViaNavigation)
                    .Distinct()
                    .ToList()
                });
            return Ok(result);
        }

    }
}
