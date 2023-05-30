using GraphQL;
using GraphQL.Types;
using InventoryService.Repositories;

namespace InventoryService.Queries
{
    public class ProductGLQuery:ObjectGraphType
    {
        public ProductGLQuery(IProductRepo productRepo) {
            Name = "ProductQuery";
            //get all products
            Field<ListGraphType<ProductGLType>>(
              "products",
              resolve: context => productRepo.GetProducts()
          ) ;

            //get product by id
            Field<ProductGLType>(
               "product",
               arguments: new QueryArguments(new QueryArgument<LongGraphType>
               { Name = "productId" }),
               resolve: context => productRepo.GetProduct(context.GetArgument<long>("productId"))

               );

        }
    }
}
