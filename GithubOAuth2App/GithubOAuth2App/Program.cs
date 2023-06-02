
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Text.Json;

void CheckSameSite(HttpContext httpContext, CookieOptions options)
{
    if (options.SameSite == SameSiteMode.None)
    {
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        // TODO: Use your User Agent library of choice here.
        // if (/* UserAgent doesn’t support new behavior */)
        // {
        // For .NET Core < 3.1 set SameSite = (SameSiteMode)(-1)
        options.SameSite = SameSiteMode.Unspecified;
        //}
    }
}

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    options.OnAppendCookie = cookieContext =>
        CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
    options.OnDeleteCookie = cookieContext =>
        CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
});
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "GitHub";
})
   .AddCookie()
   .AddOAuth("GitHub", options =>
   {
       options.ClientId = configuration["GitHub:ClientId"];
       options.ClientSecret = configuration["GitHub:ClientSecret"];
       options.CallbackPath = new PathString("/github-oauth");

       options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
       options.TokenEndpoint = "https://github.com/login/oauth/access_token";
       options.UserInformationEndpoint = "https://api.github.com/user";

       options.SaveTokens = true;

       options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
       options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
       options.ClaimActions.MapJsonKey("urn:github:login", "login");
       options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
       options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");

       options.Events = new OAuthEvents
       {
           OnCreatingTicket = async context =>
           {
               var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
               request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

               var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
               response.EnsureSuccessStatusCode();

               var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

               context.RunClaimActions(json.RootElement);
           }
       };
   });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapControllerRoute(
       name: "default",
       pattern: "{controller}/{action=Index}/{id?}");
/*
 * MapRoute(
   name: "api", 
   template: "api/{controller}/{action}/{id?}",
   defaults: new { Controller = "Messages", action="My" },
   constraints: new { id = new IntRouteConstraint() });
 */
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
