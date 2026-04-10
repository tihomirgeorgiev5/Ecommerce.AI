using AiService.Repositories;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

//Enable Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
            policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
    
});

var cfg = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//PostGresSQL + Vector

var dataSourceBuilder = new Npgsql.NpgsqlDataSourceBuilder(cfg.GetConnectionString("PgVector"));
dataSourceBuilder.UseVector();
var dataSource = dataSourceBuilder.Build();
builder.Services.AddSingleton(dataSource);
builder.Services.AddSingleton<IPgVectorRepository, PgVectorRepository>();

//Embedding + Chat Provider
var provider = cfg["EmbeddingProvider"]?.ToLowerInvariant() ?? "Ollama";

//TODO: Add more providers and switch between them based on configuration 
//switch (provider)
//{
    
//}


