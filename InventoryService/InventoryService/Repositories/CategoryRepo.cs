using InventoryService.Contexts;
using InventoryService.Models;

namespace InventoryService.Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        private InventoryContext _db;

        public CategoryRepo(InventoryContext db)
        {
            _db = db;
        }

        public async Task<Category> AddCategory(Category Category)
        {
            //insert query
            //Id will be generated
           var Result= await _db.Categories.AddAsync(Category);
            await _db.SaveChangesAsync();
            return Result.Entity;
        }

        public Task<bool> DeleteCategory(Category Category)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetCategories()
        {
           
        }

        public Task<Category> GetCategoryById(long Id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> UpdateCategory(Category Category)
        {
            throw new NotImplementedException();
        }
    }
}
