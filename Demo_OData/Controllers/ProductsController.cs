using Demo_OData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace Demo_OData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly NorthwindDbContext _context;

        public ProductsController(NorthwindDbContext context)
        {
            _context = context;
        }

        [EnableQuery]
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        //1- Liệt kê tất cả thông tin của customer kèm theo danh sách product 
        //mà người đó đã mua

        //GET: api/Products/CustomersWithProducts
        [HttpGet("CustomersWithProducts")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<object>>> GetCustomersWithProducts()
        {
            var customersWithProducts = await _context.Customers
                .Select(c => new
                {
                    c.CustomerId,
                    c.CompanyName,
                    Products = c.Orders.SelectMany(o => o.OrderDetails.Select(od => od.Product)).Distinct()
                })
                .ToListAsync();
            return Ok(customersWithProducts);
        }


        //2 - trả về thông tin của một khách hàng (nhập id) và danh sách 
        //những sản phẩm mà người đó đã mua

        // GET: api/Products/CustomerWithProducts/{id}
        [HttpGet("CustomerWithProducts/{id}")]
        [EnableQuery]
        public async Task<ActionResult<object>> GetCustomerWithProducts(string id)
        {
            var customerWithProducts = await _context.Customers
                .Where(c => c.CustomerId == id)
                .Select(c => new
                {
                    c.CustomerId,
                    c.CompanyName,
                    Products = c.Orders.SelectMany(o => o.OrderDetails.Select(od => od.Product)).Distinct()
                })
                .FirstOrDefaultAsync();
            if (customerWithProducts == null)
            {
                return NotFound();
            }
            return Ok(customerWithProducts);
        }

        //3- Liệt kê tất cả thông tin khách hàng (customer) và danh sách 
        //các category (id, name) mà người đó đã mua 

        // GET: api/Products/CustomersWithCategories
        [HttpGet("CustomersWithCategories")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<object>>> GetCustomersWithCategories()
        {
            var customersWithCategories = await _context.Customers
                .Select(c => new
                {
                    c.CustomerId,
                    c.CompanyName,
                    Categories = c.Orders.SelectMany(o => o.OrderDetails.Select(od => new
                    {
                        od.Product.Category.CategoryId,
                        od.Product.Category.CategoryName
                    })).Distinct()
                })
                .ToListAsync();
            return Ok(customersWithCategories);
        }

        //4- trả về thông tin của một customer và danh sách các category 
        //mà người đó đã mua

        // GET: api/Products/CustomerWithCategories/{id}
        [HttpGet("CustomerWithCategories/{id}")]
        [EnableQuery]
        public async Task<ActionResult<object>> GetCustomerWithCategories(string id)
        {
            var customerWithCategories = await _context.Customers
                .Where(c => c.CustomerId == id)
                .Select(c => new
                {
                    c.CustomerId,
                    c.CompanyName,
                    Categories = c.Orders.SelectMany(o => o.OrderDetails.Select(od => new
                    {
                        od.Product.Category.CategoryId,
                        od.Product.Category.CategoryName
                    })).Distinct()
                })
                .FirstOrDefaultAsync();
            if (customerWithCategories == null)
            {
                return NotFound();
            }
            return Ok(customerWithCategories);
        }


        //5- Liệt kê tất cả thông tin khách hàng (customer) và danh sách 
        //các supplier (Supplierid, Companyname) mà người đó đã mua 

        // GET: api/Products/CustomersWithSuppliers
        [HttpGet("CustomersWithSuppliers")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<object>>> GetCustomersWithSuppliers()
        {
            var customersWithSuppliers = await _context.Customers
                .Select(c => new
                {
                    c.CustomerId,
                    c.CompanyName,
                    Suppliers = c.Orders.SelectMany(o => o.OrderDetails.Select(od => new
                    {
                        od.Product.Supplier.SupplierId,
                        od.Product.Supplier.CompanyName
                    })).Distinct()
                })
                .ToListAsync();
            return Ok(customersWithSuppliers);
        }

        //6- trả về thông tin của một customer và danh sách các supplier
        //(Supplierid, Companyname) mà người đó đã mua 
        //7- Liệt kê tất cả các product mà danh sách shipper đã ship 
        //product đó
        //8- trả về thông tin của 1 product và danh sách các shipper 
        //đã ship product đó
        //9- liệt kê tất cả thông tin của các employee và danh sách 
        //customer mà employee đó bán hàng
        //10 - trả về thông tin của employee bán được nhiều hàng nhất 
        // và danh sách các hàng đã ban được (cộng dồn các product)
    }
}
