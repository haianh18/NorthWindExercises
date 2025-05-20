using NorthWindExercises.Models;

namespace NorthWindExercises.Modeldto
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public int? SupplierId { get; set; }

        public int? CategoryId { get; set; }

        public string? QuantityPerUnit { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public ProductDTO(Product product)
        {
            this.ProductId = product.ProductId;
            this.ProductName = product.ProductName;
            this.SupplierId = product.SupplierId;
            this.CategoryId = product.CategoryId;
            this.QuantityPerUnit = product.QuantityPerUnit;
            this.UnitPrice = product.UnitPrice;
            this.UnitsInStock = product.UnitsInStock;
            this.UnitsOnOrder = product.UnitsOnOrder;
            this.ReorderLevel = product.ReorderLevel;
            this.Discontinued = product.Discontinued;
        }
    }
}
