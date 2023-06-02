using InventoryService.Configurations;
using InventoryService.Contexts;
using InventoryService.Repositories;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Swashbuckle.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using InventoryService.Schemas;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System;
using VaultSharp.V1.SystemBackend;
using Microsoft.Net.Http.Headers;
using InventoryService.Models;
using InventoryService.Services;
using InventoryService.Exceptions;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
// Add services to the container.
Dictionary<string, Object> data = new VaultConfiguration(configuration)
    .GetConfiguration().Result;
//connection string
SqlConnectionStringBuilder providerCs = new SqlConnectionStringBuilder();
//reading from Vault server
providerCs.InitialCatalog = data["dbname3"].ToString();
providerCs.UserID = data["username"].ToString();
providerCs.Password = data["password"].ToString();
//providerCs.DataSource = "DESKTOP-55AGI0I\\MSSQLEXPRESS2022";
var machineName = data["machinename"];
var serverName = data["servername"];
var datasource = machineName + "\\" + serverName;
providerCs.DataSource = datasource;
//reading via config server
//providerCs.DataSource = configuration["servername"];

//providerCs.UserID = CryptoService2.Decrypt(ConfigurationManager.
//AppSettings["UserId"]);
providerCs.MultipleActiveResultSets = true;
providerCs.TrustServerCertificate = true;

builder.Services.AddDbContext<InventoryContext>(o =>
o.UseSqlServer(providerCs.ToString()));

builder.Services.AddDbContext<InventoryIdentityContext>(options =>
options.UseSqlServer(configuration.
GetConnectionString("IdentityConn")));



//dependency Injection Register
builder.Services.AddTransient<ICategoryRepo, CategoryRepo>();
builder.Services.AddTransient<IProductRepo,ProductRepo>();

//builder.Services.Configure<IdentityOptions>(option =>
//{
//    option.Password.RequireNonAlphanumeric=true;
//    option.Password.RequireDigit = true;
//    option.Password.RequireLowercase=true;
//    option.Password.RequireUppercase=true;
  
//});

// For Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<InventoryIdentityContext>()
    .AddDefaultTokenProviders();

Dictionary<string, Object> secretData = new VaultConfiguration(configuration)
              .GetSecret().Result;


// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Add services to the container.
          
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretData["key"].ToString()))
    };
});
var policyName = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
                      builder =>
                      {
                          builder
                            .WithOrigins("http://localhost:*","")
                            //.WithOrigins("http://localhost:4200")
                             // specifying the allowed origin
                             // .WithMethods("GET") // defining the allowed HTTP method
                             //.AllowAnyOrigin()
                             // .WithHeaders(HeaderNames.ContentType, "ApiKey")
                             .AllowAnyMethod()                            
                            .AllowAnyHeader(); // allowing any header to be sent
                      });
});

//builder.Services.AddControllers();

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.ReturnHttpNotAcceptable = true;
}).AddXmlSerializerFormatters()
.AddMvcOptions(options => options.OutputFormatters.Add(new CsvOutputFormatter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddApiVersioning(x =>
//{
//    x.DefaultApiVersion = new ApiVersion(1, 0);
//    x.AssumeDefaultVersionWhenUnspecified = true;
//    x.ReportApiVersions = true;
//    x.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
//});
//builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<SwaggerConfiguration>();

builder.Services.AddScoped<CategorySchema>();
builder.Services.AddGraphQL()
               .AddSystemTextJson()
               .AddGraphTypes(typeof(CategorySchema), ServiceLifetime.Scoped);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()

    .WriteTo.Debug()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new
        Uri(configuration["ElasticConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"InventoryIndex-{DateTime.UtcNow:yyyy-MM}"
    })
    .Enrich.WithProperty("Environment", environment)
    .Enrich.WithProperty("ApplicationName","Inventory")
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Host.UseSerilog();

//Ldap
builder.Services.Configure<LdapConfig>(configuration.GetSection("Ldap"));
builder.Services.AddTransient<ILdapService, LdapService>();
var app = builder.Build();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();
app.UseCors(policyName);
app.UseAuthentication();

app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v1");
//});
app.UseGraphQL<CategorySchema>();
app.UseGraphQLPlayground(options: new PlaygroundOptions());

app.Run();
