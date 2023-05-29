// See https://aka.ms/new-console-template for more information
//top level statements
global using EcommerceConsoleApp.Models;

Console.WriteLine("Rocking with .net core 7!!!");

//instance for Category
var electronicCategory = new Category(1, "Electronic", DateTime.Now, "User1");

//immutability
//electronicCategory.CategoryId = 47868;
//init
//electronicCategory.ModifiedBy = "User2";
//set
//electronicCategory.ModifiedBy = "User2";
//electronicCategory.ModifiedDate = new DateTime(2023, 5, 5);


//var furnitureCategory = electronicCategory with
//{
//    CategoryId = new Random().NextInt64(20000),
//    CategoryName = "Furniture"
//};
//Console.WriteLine($"Furniture Category{ furnitureCategory}");

Console.WriteLine($"Electronc Category {electronicCategory}");

//Line Feed
long month = new Random().NextInt64(11);
string Season = $"The season is {month switch
{
    1 or 2 or 12 => "winter",
    > 2 and < 6 => "spring",
    > 5 and < 9 => "summer",
    > 8 and < 12 => "autumn",
    _ => "Unknown. Wrong month number",
}}.";

Console.WriteLine(Season);

Console.ReadKey();
