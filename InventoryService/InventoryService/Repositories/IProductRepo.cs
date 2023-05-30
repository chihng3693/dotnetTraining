using InventoryService.Models;

namespace InventoryService.Repositories
{
    public interface IProductRepo
    {
        Task<Product> AddProduct(Product Product);
        Task<Product> UpdateProduct(Product Product);
        Task<bool> DeleteProduct(Product Product);
        Task<Product> GetProduct(long Id);
        Task<IEnumerable<Product>> GetProducts();
    }
}
