using Microsoft.EntityFrameworkCore;
using DotNetEnv; // 👈 ye add karo
using System;

var builder = WebApplication.CreateBuilder(args);

// ✅ Load .env file
Env.Load();

// ✅ Read values from environment variables
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
var openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

// ✅ Save API key in configuration for other files
builder.Configuration["OpenAI:ApiKey"] = openAiApiKey;

// ✅ Database setup
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// ✅ CORS setup
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ✅ Controller setup
builder.Services.AddControllers();

var app = builder.Build();

// ✅ Use CORS
app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
