using Jobsity.Challenge.FinancialChat.Infra.Repositories;
using Jobsity.Challenge.FinancialChat.Interfaces.Repositories;
using Jobsity.Challenge.FinancialChat.Interfaces.UseCases;
using Jobsity.Challenge.FinancialChat.UseCases.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Jobsity.Challenge.FinancialChat.CrossCutting
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IChatRoomRepository, ChatRoomRepository>();
            services.AddSingleton<IUserConnectionRepository, UserConnectionRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAddToRoomUseCase, AddToRoomUseCase>();
            services.AddScoped<IGetUserUseCase, GetUserUseCase>();
            services.AddScoped<IGetRoomUseCase, GetRoomUseCase>();
            services.AddScoped<ISaveUserUseCase, SaveUserUseCase>();

            return services;
        }
    }
}