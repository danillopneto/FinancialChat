using Jobsity.Challenge.FinancialChat.Core.Infra.Gateways;
using Jobsity.Challenge.FinancialChat.Domain.DataContracts.Requests;
using Jobsity.Challenge.FinancialChat.Infra.Configurations;
using Jobsity.Challenge.FinancialChat.Interfaces.Gateways;
using Microsoft.Extensions.Logging;

namespace Jobsity.Challenge.FinancialChat.Infra.Gateways
{
    public class BotGateway : BaseGateway<BotGateway>, IBotGateway
    {
        public BotGateway(
                          IHttpClientFactory httpClientFactory,
                          ILogger<BotGateway> logger)
            : base(httpClientFactory, logger)
        {
            Client = "commands";
        }

        public async Task PublishCommandAsync(CommandRequest command)
        {
            await SendPostRequest(NamedHttpClients.BotApi, Client, command);
        }
    }
}