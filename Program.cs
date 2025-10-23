using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalStylistIA.Data;
using PersonalStylistIA.Models;
using PersonalStylistIA.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// =========================
// 1️⃣ Banco de dados
// =========================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// =========================
// 2️⃣ Identity
// =========================
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// =========================
// 3️⃣ Logging em produção
// =========================
if (builder.Environment.IsProduction())
{
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.SetMinimumLevel(LogLevel.Warning);
}

// =========================
// 4️⃣ Razor Pages e Sessão
// =========================
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// =========================
// 5️⃣ Serviços OpenAI
// =========================

// OpenAI Text Service
builder.Services.AddHttpClient<IOpenAITextService, OpenAITextService>((sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config["OpenAI:BaseUrl"];
    var apiKey = config["OpenAI:ApiKey"];

    if (string.IsNullOrWhiteSpace(baseUrl))
        throw new ArgumentNullException("OpenAI:BaseUrl", "⚠️ Endpoint base da API OpenAI não configurado.");
    if (string.IsNullOrWhiteSpace(apiKey))
        throw new ArgumentNullException("OpenAI:ApiKey", "⚠️ Chave da API OpenAI não configurada.");

    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
});

// OpenAI Image Service
builder.Services.AddHttpClient<IOpenAIImageService, OpenAIImageService>((sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var apiKey = config["OpenAI:ApiKey"];
    var baseUrl = config["OpenAI:BaseUrl"];

    if (string.IsNullOrWhiteSpace(apiKey))
        throw new ArgumentNullException("OpenAI:ApiKey", "⚠️ Chave da API OpenAI não configurada.");

    if (string.IsNullOrWhiteSpace(baseUrl))
        throw new ArgumentNullException("OpenAI:BaseUrl", "⚠️ O endpoint base da API OpenAI não foi configurado.");

    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
});


// =========================
// 6️⃣ Build da aplicação
// =========================
var app = builder.Build();

// =========================
// 7️⃣ Middlewares
// =========================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();
