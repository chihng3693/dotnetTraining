using InventoryService.Configurations;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
Dictionary<string, Object> data = new VaultConfiguration(configuration)
    .GetConfiguration().Result;
//connection string
SqlConnectionStringBuilder providerCs = new SqlConnectionStringBuilder();
//reading from Vault server
providerCs.InitialCatalog = data["dbname3"].ToString();
providerCs.UserID = data["username"].ToString();
providerCs.Password = data["password"].ToString();
//providerCs.DataSource = "DESKTOP-55AGI0I\\MSSQLEXPRESS2021";
//reading via config server
providerCs.DataSource = configuration["servername"];

//providerCs.UserID = CryptoService2.Decrypt(ConfigurationManager.
//AppSettings["UserId"]);
providerCs.MultipleActiveResultSets = true;
providerCs.TrustServerCertificate = false;


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
