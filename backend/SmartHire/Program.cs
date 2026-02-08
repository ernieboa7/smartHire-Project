using DotNetEnv;
using MongoDB.Driver; // for MongoDB health check

using Microsoft.Extensions.Options;
using SmartHire.Infrastructure.MongoDb;
using SmartHire.Integrations.OpenAI;
using SmartHire.Repositories.Implementations;
using SmartHire.Repositories.Interfaces;
using SmartHire.Services.Implementations;
using SmartHire.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Load .env from the project root (same folder as the .csproj)
Env.Load();

// Read environment variables
var openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
var openAiModel = Environment.GetEnvironmentVariable("OPENAI_MODEL");
var openAiBaseUrl = Environment.GetEnvironmentVariable("OPENAI_BASE_URL");

// Map env variables into the "OpenAI" config section
if (!string.IsNullOrEmpty(openAiApiKey))
{
    builder.Configuration["OpenAI:ApiKey"] = openAiApiKey;
}

if (!string.IsNullOrEmpty(openAiModel))
{
    builder.Configuration["OpenAI:Model"] = openAiModel;
}

if (!string.IsNullOrEmpty(openAiBaseUrl))
{
    builder.Configuration["OpenAI:BaseUrl"] = openAiBaseUrl;
}

// Add configuration
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDb"));

builder.Services.Configure<OpenAiSettings>(
    builder.Configuration.GetSection("OpenAI"));

// MongoDb context
builder.Services.AddSingleton<MongoDbContext>();

// Repositories
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IChatSessionRepository, ChatSessionRepository>();

// Services
builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IChatService, ChatService>();

// OpenAI (Gemini) integration
builder.Services.AddHttpClient<OpenAiClient>();
builder.Services.AddScoped<OpenAiClient>();

// Controllers
builder.Services.AddControllers();

// Swagger (useful while developing)
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// CORS for Vite frontend (adjust origin if needed)
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendCors", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseCors("FrontendCors");

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

//
// HEALTH CHECK ENDPOINTS
//

// Simple API health
app.MapGet("/health", () =>
    Results.Ok(new
    {
        status = "OK",
        message = "SmartHire API is running"
    })
);

// MongoDB connectivity health
app.MapGet("/health/mongo", async (IOptions<MongoDbSettings> mongoOptions) =>
{
    try
    {
        var settings = mongoOptions.Value;

        // Basic validation of config
        if (string.IsNullOrWhiteSpace(settings.ConnectionString) ||
            string.IsNullOrWhiteSpace(settings.DatabaseName))
        {
            return Results.Problem("MongoDB configuration is missing ConnectionString or DatabaseName.");
        }

        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        // Try listing collections as a lightweight connectivity test
        var collections = await database.ListCollectionNames().ToListAsync();

        return Results.Ok(new
        {
            status = "OK",
            message = "Connected to MongoDB successfully.",
            database = settings.DatabaseName,
            collectionCount = collections.Count,
            collections
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"MongoDB connectivity error: {ex.Message}");
    }
});

// OpenAI configuration health (checks configuration, not a live API call)
app.MapGet("/health/openai", (IOptions<OpenAiSettings> openAiOptions) =>
{
    var settings = openAiOptions.Value;

    var hasApiKey = !string.IsNullOrWhiteSpace(settings.ApiKey);
    var hasModel = !string.IsNullOrWhiteSpace(settings.Model);
    var hasBaseUrl = !string.IsNullOrWhiteSpace(settings.BaseUrl);

    if (hasApiKey && hasModel && hasBaseUrl)
    {
        return Results.Ok(new
        {
            status = "OK",
            message = "OpenAI is configured.",
            model = settings.Model,
            baseUrl = settings.BaseUrl
        });
    }

    return Results.Problem("OpenAI is not fully configured. Check ApiKey, Model, and BaseUrl in environment/appsettings.");
});

app.Run();
