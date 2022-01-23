using Jobsity.Challenge.FinancialChat.Core.Utils;
using Jobsity.Challenge.FinancialChat.Infra.Configurations;
using Jobsity.Challenge.FinancialChat.Infra.Contexts;
using Jobsity.Challenge.FinancialChat.Infra.Gateways;
using Jobsity.Challenge.FinancialChat.Infra.Repositories;
using Jobsity.Challenge.FinancialChat.Interfaces.Gateways;
using Jobsity.Challenge.FinancialChat.Interfaces.Repositories;
using Jobsity.Challenge.FinancialChat.Interfaces.UseCases;
using Jobsity.Challenge.FinancialChat.UseCases.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Jobsity.Challenge.FinancialChat.CrossCutting
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services)
        {
            services.AddDbContext<ChatContext>(opt => opt.UseInMemoryDatabase("FinancialChat"));

            services.AddScoped<ChatContext>();

            return services;
        }

        public static IServiceCollection AddGateways(this IServiceCollection services)
        {
            services.AddScoped<IBotGateway, BotGateway>();

            return services;
        }

        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient(NamedHttpClients.BotApi)
                .ConfigureHttpClient(
                    client =>
                    {
                        var provider = services.BuildServiceProvider();
                        var apiSettings = provider.GetService<ApiSettings>();

                        client.BaseAddress = new Uri(apiSettings.UrlBot);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    })
                .AddTransientHttpErrorPolicy(HttpUtils.PollyConfiguration());

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
            services.AddScoped<IUserConnectionRepository, UserConnectionRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAddToRoomUseCase, AddToRoomUseCase>();
            services.AddScoped<IDispatchCommandUseCase, DispatchCommandUseCase>();
            services.AddScoped<IGetUserUseCase, GetUserUseCase>();
            services.AddScoped<IGetRoomUseCase, GetRoomUseCase>();
            services.AddScoped<ISaveMessageIntoRoomUseCase, SaveMessageIntoRoomUseCase>();
            services.AddScoped<ISaveUserUseCase, SaveUserUseCase>();

            return services;
        }
    }
}