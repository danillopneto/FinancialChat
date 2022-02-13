using Jobsity.Challenge.FinancialChat.Infra.Contexts;
using Jobsity.Challenge.FinancialChat.Infra.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Jobsity.Challenge.FinancialChat.CrossCutting
{
    public static class ServiceProviderExtensions
    {
        public static IServiceProvider SetupDataBase(this IServiceProvider services, LayerPresenter layerPresenter)
        {
            using var scope = services.CreateScope();

            switch (layerPresenter)
            {
                case LayerPresenter.Web:
                    MigrateDataBase<IdentityContext>(scope);
                    break;

                case LayerPresenter.SignalR:
                    MigrateDataBase<ChatContext>(scope);
                    break;
            }

            return services;
        }

        private static void MigrateDataBase<TContext>(IServiceScope scope)
            where TContext : DbContext
        {
            var context = scope.ServiceProvider.GetService<TContext>();
            if (context is not null)
            {
                context.Database.Migrate();
            }
        }
    }
}
