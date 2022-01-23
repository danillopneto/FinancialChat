using Jobsity.Challenge.FinancialChat.CrossCutting;
using Jobsity.Challenge.FinancialChat.Domain.Constants;
using Jobsity.Challenge.FinancialChat.Infra.Configurations;
using Jobsity.Challenge.FinancialChat.SignalR.Hubs;
using Jobsity.Challenge.FinancialChat.UseCases.Automappers;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        p =>
        {
            p.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials()
            ;
        });
});

// Add services to the container.
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
builder.Services.AddDatabaseConfiguration();

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

app.MapRazorPages();

app.UseCors();

app.MapHub<ChatHub>($"/{ConstantsHubs.DefaultChat}");

app.Run();