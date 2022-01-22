

using Jobsity.Challenge.FinancialChat.Domain.Constants;
using Jobsity.Challenge.FinancialChat.SignalR.Hubs;

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
