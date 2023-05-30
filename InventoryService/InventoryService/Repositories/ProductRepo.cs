using InventoryService.Models;

namespace InventoryService.Repositories
{
    public class ProductRepo : IProductRepo
    {
        public Task<Product> AddProduct(Product Product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProduct(Product Product)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProduct(long Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            throw new NotImplementedException();
        }

        public Task<Product> UpdateProduct(Product Product)
        {
            throw new NotImplementedException();
        }
    }
}
