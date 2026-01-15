using Microsoft.AspNetCore.Authentication.Cookies;
using RazorWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// HttpClient для API
builder.Services.AddHttpClient<ApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? "http://localhost:5000");
});

// Сервисы
builder.Services.AddScoped<AuthService>();

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

builder.Services.AddAuthorization();

// Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Редирект с / на Dashboard или Login
app.MapGet("/", context =>
{
    if (context.User.Identity?.IsAuthenticated == true)
        context.Response.Redirect("/Dashboard");
    else
        context.Response.Redirect("/Auth/Login");
    return Task.CompletedTask;
});

app.Run();
