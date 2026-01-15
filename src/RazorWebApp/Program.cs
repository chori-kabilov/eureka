using Microsoft.AspNetCore.Authentication.Cookies;
using RazorWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// HttpContextAccessor для получения токена
builder.Services.AddHttpContextAccessor();

// AuthTokenHandler — добавляет JWT токен в запросы
builder.Services.AddTransient<AuthTokenHandler>();

// HttpClient для API с автоматической авторизацией
builder.Services.AddHttpClient<ApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? "http://localhost:5000");
})
.AddHttpMessageHandler<AuthTokenHandler>();

// Сервисы
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<StudentsService>();
builder.Services.AddScoped<TeachersService>();
builder.Services.AddScoped<ChildrenService>();
builder.Services.AddScoped<ParentsService>();
builder.Services.AddScoped<CoursesService>();

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

// Обработка ошибок — всегда показываем красивую страницу
app.UseExceptionHandler("/Error");
app.UseStatusCodePagesWithReExecute("/Error/{0}");

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
