using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.EntityFrameworkCore;
using SovaCloud.Data;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option => {
        option.LoginPath = "/Account/SignIn";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.EUCentral1);
GetSecretValueRequest request = new GetSecretValueRequest
{
    SecretId = "Development_SovaCloudStorage_Database__ConnectionString"
};

GetSecretValueResponse response = await client.GetSecretValueAsync(request);
string connectionStringSecretValue = response.SecretString;

builder.Services.AddDbContext<SovaCloudDbContext>(options =>
    options.UseSqlServer(connectionStringSecretValue));


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
