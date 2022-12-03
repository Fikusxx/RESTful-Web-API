using CleanArchitecture.UI.MVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using MVC.Contracts;
using MVC.Services;
using System.Reflection;



var builder = WebApplication.CreateBuilder();
var services = builder.Services;
var configuration = builder.Configuration;


services.AddHttpContextAccessor();

services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
services.AddTransient<IAuthenticationService, AuthenticationService>();

services.AddHttpClient<IClient, Client>(x =>
{
    x.BaseAddress = new Uri("https://localhost:7270/");
});

services.AddAutoMapper(Assembly.GetExecutingAssembly());

services.AddScoped<ILeaveTypeService, LeaveTypeService>();
services.AddScoped<ILeaveAllocationService, LeaveAllocationService>();
services.AddScoped<ILeaveRequestService, LeaveRequestService>();

services.AddSingleton<ILocalStorageService, LocalStorageService>();

services.AddControllersWithViews();





var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCookiePolicy();
app.UseAuthentication();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();