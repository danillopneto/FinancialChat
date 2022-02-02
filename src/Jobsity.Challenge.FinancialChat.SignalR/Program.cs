using Jobsity.Challenge.FinancialChat.CrossCutting;
using Jobsity.Challenge.FinancialChat.Domain.Constants;
using Jobsity.Challenge.FinancialChat.Infra.Configurations;
using Jobsity.Challenge.FinancialChat.SignalR.Hubs;
using Jobsity.Challenge.FinancialChat.UseCases.Automappers;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
const string CorsPolicy = "signalr";

builder.Services.AddCors(options =>
{
    options.AddPolicy(
                      CorsPolicy, 
                      builder => builder.AllowAnyMethod()
                                        .AllowAnyHeader()
                                        .AllowCredentials()
                                        .SetIsOriginAllowed(hostName => true));
});

// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddServices();

// Add configurations.
builder.Services.Configure<DataAppSettings>(builder.Configuration);
builder.Services.AddScoped(c => c.GetService<IOptionsSnapshot<DataAppSettings>>().Value);

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(nameof(DataAppSettings.Apis)));
builder.Services.AddScoped(c => c.GetService<IOptionsSnapshot<ApiSettings>>().Value);

// Add repositories to the container.
builder.Services.AddRepositories();

// Add HttpClients.
builder.Services.AddHttpClients();

// Add Automapper config
builder.Services.AddAutoMapper(typeof(ConnectionsProfile));

// Add DataBaseConfiguration
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Add Gateways
builder.Services.AddGateways();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();

app.UseCors(CorsPolicy);

app.MapHub<ChatHub>($"/{ConstantsHubs.DefaultChat}");

app.Run();