using Npgsql;
using SurveyMinimalApi.Endpoints;
using SurveyMinimalApi.Services;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ISurveyService, SurveyService>();
var app = builder.Build();

app.MapSurveyEndpoints();

app.Run();
