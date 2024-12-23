using haberPortali1.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using RefikHaber.Repostories;
using RefikHaber_Portal.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Sadece API için gerekli

// Add DbContext with SQL Server
builder.Services.AddDbContext<UygulamaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<UygulamaDbContext>()
.AddDefaultTokenProviders();

// Add Razor Pages
builder.Services.AddRazorPages();

// Register repositories
builder.Services.AddScoped<IHaberTuruRepository, HaberTuruRepository>();
builder.Services.AddScoped<IHaberRepository, HaberRepository>();
builder.Services.AddScoped<RoleManagerRepository>();


// Email sender service
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Authentication middleware
app.UseAuthorization();

// Map Razor Pages
app.MapRazorPages();

// Map API routes
app.MapControllers(); // API controller'larını haritalandır

// Default route for MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
