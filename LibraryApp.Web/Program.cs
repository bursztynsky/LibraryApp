using LibraryApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Tutaj dajemy wszystkie serwisy jakie chcemy
// uzyc jako Dependency Injection
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
   builder.Configuration.GetConnectionString("DefaultConnection")
   ));

var app = builder.Build();

// Configure the HTTP request pipeline.
// Pipeline precyzuje jak aplikacje powinny reagowac na zapytania
// Kiedy przegladarka wysyla zapytanie do aplikacji to trafia do pipeline
// W samym pipeline znajduja sie middleware czyli mniejsze komponenty aplikacji odpowiedzialne za konkretne zadanie np. autoryzacje
// Wszystkie te obiekty napisane miedzy builder.Build() oraz app.Run() to sa middleware!

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
    name: "default",
    // to schemat w jaki sposob uzywamy linkow by zostawac sie do kontrolerow
    // jesli nie podamy nic, to uzyty zostanie Home\Index
    // id jest opcjonalne
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
