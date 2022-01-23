using Jobsity.Challenge.FinancialChat.Domain.DataContracts.Requests;
using Jobsity.Challenge.FinancialChat.Infra.Configurations;
using Jobsity.Challenge.FinancialChat.Interfaces.Gateways;
using Microsoft.Extensions.Logging;

namespace Jobsity.Challenge.FinancialChat.Infra.Gateways
{
    public class BotGateway : BaseGateway<BotGateway>, IBotGateway
    {
        protected override string BaseClient => NamedHttpClients.BotApi;

        public BotGateway(
                          IHttpClientFactory httpClientFactory,
                          ILogger<BotGateway> logger)
            : base(httpClientFactory, logger)
        {
            Client = "chatCommands";
        }

        public async Task PublishCommandAsync(CommandRequest command)
        {
            await SendPostRequest(Client, command);
        }
    }
}