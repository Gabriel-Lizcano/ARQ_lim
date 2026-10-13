using Application.UseCases;
using Infrastructure.Data;
using Infrastructure.Logging;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddCors(options =>
{
    options.AddPolicy("SecurePolicy", policy =>
    {
        policy.WithOrigins("https://localhost:5001", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseCors("SecurePolicy");

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync("Ocurrió un error inesperado.");
    });
});

app.MapGet("/health", () =>
{
    Logger.Log("health ping");
    int secureNumber = RandomNumberGenerator.GetInt32(1, 100);
    return Results.Ok(new { status = "ok", value = secureNumber });
});

app.MapPost("/orders", async (HttpContext http) =>
{
    using var reader = new StreamReader(http.Request.Body);
    var body = await reader.ReadToEndAsync();
    var parts = (body ?? "").Split(',');

    if (parts.Length < 4)
    {
        return Results.BadRequest("Faltan datos en la orden.");
    }

    var customer = parts[0].Trim();
    var product = parts[1].Trim();

    if (!int.TryParse(parts[2], out var qty)) qty = 1;
    if (!decimal.TryParse(parts[3], out var price)) price = 0.99m;

    var order = CreateOrderUseCase.Execute(customer, product, qty, price);

    return Results.Ok(order);
});

app.MapGet("/orders/last", () => Domain.Services.OrderService.LastOrders);

app.MapGet("/info", () => new
{
    appName = "ARQ_lim API",
    version = "v0.0.1-secure",
    environment = app.Environment.EnvironmentName
});

await app.RunAsync();