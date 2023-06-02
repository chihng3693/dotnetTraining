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
