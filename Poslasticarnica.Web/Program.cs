var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient();

builder.Services.AddSession(opcije =>
{
    opcije.IdleTimeout = TimeSpan.FromMinutes(30);
    opcije.Cookie.HttpOnly = true;
    opcije.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Pocetna/Greska");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "podrazumevana",
    pattern: "{controller=Korisnik}/{action=Prijava}/{id?}");

app.Run();