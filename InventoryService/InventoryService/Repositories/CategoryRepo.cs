using InventoryService.Contexts;
using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Category>> GetCategories()
        {
            //select * from category;
            return await _db.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(long Id)
        {
           //select * from category where categoryId=Id
           var result= await _db.Categories.FirstOrDefaultAsync
                (c => c.CategoryId == Id);
           
            return result;
        }

        public Task<Category> UpdateCategory(Category Category)
        {
            throw new NotImplementedException();
        }
    }
}
