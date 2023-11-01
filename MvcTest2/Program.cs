using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
}).AddCookie("Cookies").
AddOpenIdConnect("oidc", option =>
{
    option.Authority = "https://localhost:44336";
    option.RequireHttpsMetadata = false;
    option.ClientId = "15";
    option.ClientSecret = "12345";
    option.SignInScheme = "Cookies";
    option.Scope.Add("openid");
    option.Scope.Add("profile");
    option.Scope.Add("email");
    option.Scope.Add("phone");
    option.GetClaimsFromUserInfoEndpoint = true;

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
