

using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));

});
builder.Services.AddIdentity<AppUser, IdentityRole>(IdentityOptions =>
{
    IdentityOptions.Password.RequiredLength = 8;   /*simvol say?n?n min uzunlu?u*/
    IdentityOptions.Password.RequireNonAlphanumeric = false;  /*simvollar olmasada olar*/
    IdentityOptions.Password.RequireUppercase = true;          /* vacibdir*/
    IdentityOptions.Password.RequireLowercase = true;
    IdentityOptions.Lockout.MaxFailedAccessAttempts = 4;  /*nece defe kodu sehv yazmaq*/
    IdentityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);  /*nece d?qiq?lik bloklanmaq*/
    IdentityOptions.Lockout.AllowedForNewUsers = true;      /*qeydiyyatdan kecmeni m?v?qq?ti dayand?rmaq*/
    IdentityOptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";  /*bunlardan yaln?z istifad? et*/
    IdentityOptions.User.RequireUniqueEmail = true;
    //IdentityOptions.SignIn.RequireConfirmedAccount = true;
    //IdentityOptions.SignIn.RequireConfirmedEmail = false;
    //IdentityOptions.SignIn.RequireConfirmedPhoneNumber = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
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

app.UseAuthorization();
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
