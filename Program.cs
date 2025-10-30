using Microsoft.EntityFrameworkCore;
using DotNetEnv;  
using System;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

 
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
var openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

 
builder.Configuration["OpenAI:ApiKey"] = openAiApiKey;

 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

 
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

builder.Services.AddControllers();

var app = builder.Build();
 
app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
