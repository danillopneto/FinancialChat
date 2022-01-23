using Jobsity.Challenge.FinancialChat.Bot.Infra.Gateways;
using Jobsity.Challenge.FinancialChat.Bot.Interfaces.Gateways;
using Jobsity.Challenge.FinancialChat.Bot.Interfaces.UseCases;
using Microsoft.Extensions.Options;
using Jobsity.Challenge.FinancialChat.Bot.Infra.Configurations;
using System.Net.Http.Headers;
using Jobsity.Challenge.FinancialChat.Core.Utils;
using Jobsity.Challenge.FinancialChat.Bot.UseCases.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add configurations.
builder.Services.Configure<DataAppSettings>(builder.Configuration);
builder.Services.AddScoped(c => c.GetService<IOptionsSnapshot<DataAppSettings>>().Value);

// Add gateways.
builder.Services.AddScoped<IChatGateway, ChatGateway>();
builder.Services.AddScoped<IGetInfoCommandGateway, GetInfoCommandGateway>();

// Add services.
builder.Services.AddScoped<IFilterCommandUseCase, FilterCommandUseCase>();
builder.Services.AddScoped<IProcessCommandUseCase, ProcessCommandUseCase>();

var configuration = builder.Configuration.Get<DataAppSettings>();
foreach (var command in configuration.AllowedCommands)
{
    builder.Services.AddHttpClient(command.Name)
        .ConfigureHttpClient(
            client =>
            {
                client.BaseAddress = new Uri(command.BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
        .AddTransientHttpErrorPolicy(HttpUtils.PollyConfiguration());
}
builder.Services.AddHttpClient(NamedHttpClients.Chat)
    .ConfigureHttpClient(
        client =>
        {
            client.BaseAddress = new Uri(configuration.ChatApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
    .AddTransientHttpErrorPolicy(HttpUtils.PollyConfiguration());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
