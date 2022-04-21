using Payment.Gateway.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:8080");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
var gcpSettings = builder.Configuration.BindConfigurationSection<GcpSettings>("GCP");
builder.Services.AddSingleton<IGcpSettings>(gcpSettings);

builder.Services
    .AddMongo(builder.Configuration, builder.Environment)
    .AddMessaging(builder.Configuration)
    .AddMappers()
    .AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.


const string healthCheckEndpointPath = "/api/health";
app.MapHealthChecks(healthCheckEndpointPath);

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();