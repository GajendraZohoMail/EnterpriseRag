using EnterpriseRag.Api.Application;
using EnterpriseRag.Api.Application.Generation;
using EnterpriseRag.Api.Application.Ingestion;
using EnterpriseRag.Api.Application.Retrieval;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Embeddings;
using System.ClientModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var endpoint = builder.Configuration["Foundry:Endpoint"]
    ?? throw new InvalidOperationException(
        "Foundry:Endpoint is missing.");

var chatDeployment = builder.Configuration["Foundry:ChatDeployment"]
    ?? throw new InvalidOperationException(
        "Foundry:ChatDeployment is missing.");

var embeddingDeployment =
    builder.Configuration["Foundry:EmbeddingDeployment"]
    ?? throw new InvalidOperationException(
        "Foundry:EmbeddingDeployment is missing.");

var apiKey = builder.Configuration["Foundry:ApiKey"]
    ?? throw new InvalidOperationException(
        "Foundry:ApiKey is missing.");

var clientOptions = new OpenAIClientOptions
{
    Endpoint = new Uri(endpoint)
};

// Real Foundry-hosted chat model through MEAI.
builder.Services.AddSingleton<IChatClient>(_ =>
    new ChatClient(
        chatDeployment,
        new ApiKeyCredential(apiKey),
        clientOptions).AsIChatClient());

// Real Foundry-hosted embedding model through MEAI.
builder.Services.AddSingleton<
    IEmbeddingGenerator<string, Embedding<float>>>(_ =>
    new EmbeddingClient(
        embeddingDeployment,
        new ApiKeyCredential(apiKey),
        clientOptions).AsIEmbeddingGenerator());

// Your application and retrieval services.
builder.Services.AddSingleton<TextChunker>();
builder.Services.AddSingleton<QueryRewriter>();

builder.Services.AddScoped<IndexingService>();
builder.Services.AddScoped<RetrievalService>();
builder.Services.AddScoped<RagService>();

// Register your real document loader and vector-store adapters here.
builder.Services.AddSingleton<IDocumentLoader, DummyDocumentLoader>();
builder.Services.AddSingleton<IVectorStore, DummyVectorStore>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

//using EnterpriseRag.Api.Application.Generation;
//using EnterpriseRag.Api.Application.Ingestion;
//using EnterpriseRag.Api.Application.Retrieval;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<TextChunker>();

//// Register these implementations in your infrastructure layer:
//// IChatClient                         -> configured model provider
//// IEmbeddingGenerator<string, Embedding<float>>
////                                     -> configured embedding provider
//// IVectorStore                        -> Qdrant/Azure AI Search adapter
//// IDocumentLoader                     -> SharePoint/blob/database adapter

//builder.Services.AddSingleton<QueryRewriter>();
//builder.Services.AddScoped<RetrievalService>();
//builder.Services.AddScoped<RagService>();
//builder.Services.AddScoped<IndexingService>();

//var app = builder.Build();

//app.UseHttpsRedirection();
//app.MapControllers();

//app.Run();

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast =  Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");

//app.Run();

//record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}
