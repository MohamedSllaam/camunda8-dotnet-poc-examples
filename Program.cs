using Zeebe.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Register Zeebe Client as singleton
builder.Services.AddSingleton<IZeebeClient>(sp =>
{
    return ZeebeClient.Builder()
        .UseGatewayAddress("127.0.0.1:26500") // Zeebe from Docker Compose
        .UsePlainText()
        .Build();
});

builder.Services.AddHostedService<ZeebeWorkerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
