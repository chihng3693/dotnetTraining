// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using Grpc.Net.Client;
using GrpcServiceDemo;

var data = new HelloRequest { Name = "Parameswari" };
var grpcChannel = GrpcChannel.ForAddress("https://localhost:7286");
//delete grpc service in .csproj build and then add
var client = new Greeter.GreeterClient(grpcChannel);
var response = await client.SayHelloAsync(data);
Console.WriteLine(response.Message);
var orderdata = new OrderRequest { OrderId = 1 };
var grpcOrderChannel = GrpcChannel.ForAddress("https://localhost:7286");
var orderclient = new OrderProcessing.OrderProcessingClient(grpcOrderChannel);
var orderresponse = await orderclient.GetOrderAsync(orderdata);
Console.WriteLine("Order Quantity: {0}", orderresponse.Order.OrderQuantity);



Console.ReadKey();
