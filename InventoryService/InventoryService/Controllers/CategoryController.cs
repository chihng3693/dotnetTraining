using InventoryService.Models;
using InventoryService.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryService.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    // [ApiVersion("1.1")]
    // [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoryController : ControllerBase
    {
        private ICategoryRepo _categoryRepo;

        public CategoryController(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }



        // GET: api/<CategoryController>
        [HttpGet]
        [MapToApiVersion("2.0")]
        public async Task<IEnumerable<Category>> Get()
        {
           return await _categoryRepo.GetCategories();
        }

        // GET api/<CategoryController>/5
        [HttpGet("{Id}")]
        public async Task<Category> Get(int Id)
        {
           return await _categoryRepo.GetCategoryById(Id);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category Category)
        {
            await  _categoryRepo.AddCategory(Category);
            return CreatedAtAction(nameof(Get),
                         new { id = Category.CategoryId }, Category);

        }

        // PUT api/<CategoryController>/5
        [HttpPut("{Id}")]
        public async Task<Category> Put(int Id, [FromBody] string CategoryName)
        {
            return await _categoryRepo.UpdateCategory(Id, CategoryName);
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{Id}")]
        public async Task<bool> Delete(int Id)
        {
            return await _categoryRepo.DeleteCategory(Id);    
        }
    }
}
