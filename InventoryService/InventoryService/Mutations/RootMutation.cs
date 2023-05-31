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
        private IProductRepo _productRepo;

        public RootMutation(ICategoryRepo categoryRepo,IProductRepo productRepo) { 
            
          _categoryRepo = categoryRepo;
            _productRepo = productRepo;

            Name = "EcommerceMutation";
            Field<CategoryGLType>("insertCategory",
            arguments: new QueryArguments(

                new QueryArgument<CategoryGLInputType> { Name = "category" }),

            resolve: context =>
            {

                var category = context.GetArgument<Category>("category");

                return InsertCategory(category);
            });
            
            Field<CategoryGLType>(
               "UpdateCategory",
               arguments: new QueryArguments(new QueryArgument<NonNullGraphType<LongGraphType>>
               { Name = "CategoryId" },
               new QueryArgument<NonNullGraphType<StringGraphType>>
               { Name = "CategoryName" }
               ),
               resolve: context =>
               {
                   var categoryId = context.GetArgument<long>("CategoryId");
                   var categoryName = context.GetArgument<string> ("CategoryName");
                  
                   return UpdateCategory(categoryId, categoryName);
               }
           );

            Field<StringGraphType>(
                "DeleteCategory",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<LongGraphType>>
                { Name = "CategoryId" }),
                resolve: context =>
                {
                    var categoryId = context.GetArgument<long>("CategoryId");

                    categoryRepo.DeleteCategory(categoryId);
                    return $"CategoryId {categoryId} is successfully deleted";
                }
            );


            Field<ProductGLType>("insertProduct",
            arguments: new QueryArguments(

                new QueryArgument<ProductGLInputType> { Name = "product" },
                new QueryArgument<NonNullGraphType<LongGraphType>> { Name = "catalogId" }),

            resolve: context =>
            {

                var product = context.GetArgument<Product>("product");
                var catalogId = context.GetArgument<long>("catalogId");

                return InsertProduct(product,catalogId);
            });
            

        }

        private async Task<Category> InsertCategory(Category Category)
        {
            if (Category == null)
                return null;
            else
            {
                return await _categoryRepo.AddCategory(Category);
            }
        }
        
        private async Task<Category> UpdateCategory(long categoryId,string categoryName)
        {

            return await _categoryRepo.UpdateCategory(categoryId, categoryName);

        }

        private async Task<Product> InsertProduct(Product Product,long CatalogId)
        {
            if (Product == null)
                return null;
            else
            {
                return await _productRepo.AddProduct(Product, CatalogId);
            }
        }
        
    }
}
