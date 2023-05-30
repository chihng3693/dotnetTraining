using InventoryService.Models;

namespace InventoryService.Repositories
{
    public interface ICategoryRepo
    {
        Task<Category> AddCategory(Category Category);
        Task<Category> UpdateCategory(Category Category);
        Task<bool> DeleteCategory(Category Category);
        Task<Category> GetCategoryById(long  Id);
        Task<IEnumerable<Category>> GetCategories();
    }
}
