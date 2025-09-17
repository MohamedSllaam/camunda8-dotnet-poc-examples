using camunda8_dotnet_poc_examples.Services;
using camunda8_dotnet_poc_examples.Workers;
using Zeebe.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Register Zeebe Client as singleton
//builder.Services.AddSingleton<IZeebeClient>(sp =>
//{
//    return ZeebeClient.Builder()
//        .UseGatewayAddress("127.0.0.1:26500") // Zeebe from Docker Compose
//        .UsePlainText()
//        .Build();
//});

builder.Services.AddScoped<IEmailService, EmailService>();
// Register worker services
builder.Services.AddHostedService<AddToDeptAWorker>();
builder.Services.AddHostedService<MoveToDeptBWorker>();
builder.Services.AddHostedService<SendEmailWorker>();

builder.Services.AddSingleton<IZeebeService, ZeebeService>();

builder.Services.AddHostedService<ProcessDeploymentService>();

//builder.Services.AddHostedService<ZeebeWorkerService>();




builder.Services.AddSingleton<IZeebeClient>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var gatewayAddress = config["Zeebe:GatewayAddress"] ?? "localhost:26500";

    return ZeebeClient.Builder()
        .UseGatewayAddress(gatewayAddress)
        .UsePlainText()
        .Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.DefaultModelsExpandDepth(-1));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
