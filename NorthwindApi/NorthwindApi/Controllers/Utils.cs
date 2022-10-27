using NorthwindApi.Models;
using NorthwindAPI.Models.DTO;

namespace NorthwindApi.Controllers
{
    public static class Utils
    {
        public static SupplierDTO SupplierToDTO(Supplier supplier) =>
            new SupplierDTO
            {
                ContactName = supplier.ContactName,
                SupplierId = supplier.SupplierId,
                CompanyName = supplier.CompanyName,
                ContactTitle = supplier.ContactTitle,
                Country = supplier.Address,
                TotalProducts = supplier.Products.Count(),
                Products = supplier.Products.Select(x => ProductToDTO(x)).ToList()
            };
        public static ProductDTO ProductToDTO(Product product) =>
            new ProductDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                SupplierId = product.SupplierId,
                CategoryId = product.CategoryId,
                UnitPrice = product.UnitPrice
            };
    }
}
