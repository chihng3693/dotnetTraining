using GraphQL;
using GraphQL.Types;
using InventoryService.Models;
using InventoryService.Queries;
using InventoryService.Repositories;

namespace InventoryService.Mutations
{
    public class RootMutation:ObjectGraphType
    {
        private ICategoryRepo _categoryRepo;

        public RootMutation(ICategoryRepo categoryRepo) { 
            
          _categoryRepo = categoryRepo;

            Name = "EcommerceMutation";
            Field<CategoryGLQuery>("insertCategory",
            arguments: new QueryArguments(

                new QueryArgument<CategoryGLInputType> { Name = "category" }),

            resolve: context =>
            {

                var category = context.GetArgument<Category>("category");

                return InsertCatalog(category);
            });


        }

        private Task<Category> InsertCatalog(Category Category)
        {
            if (Category == null)
                return null;
            else
            {
                return _categoryRepo.AddCategory(Category);
            }
        }
    }
}
