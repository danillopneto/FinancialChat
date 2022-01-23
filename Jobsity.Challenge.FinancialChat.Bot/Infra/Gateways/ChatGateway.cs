using Jobsity.Challenge.FinancialChat.Bot.Domain.DataContracts.Requests;
using Jobsity.Challenge.FinancialChat.Bot.Infra.Configurations;
using Jobsity.Challenge.FinancialChat.Bot.Interfaces.Gateways;
using Jobsity.Challenge.FinancialChat.Core.Infra.Gateways;

namespace Jobsity.Challenge.FinancialChat.Bot.Infra.Gateways
{
    public class ChatGateway : BaseGateway<ChatGateway>, IChatGateway
    {
        public ChatGateway(
                           IHttpClientFactory httpClientFactory,
                           ILogger<ChatGateway> logger)
            : base(httpClientFactory, logger)
        {
        }

        public async Task SendMessageAsyc(CommandMessageRequest request, CancellationToken cancellationToken)
        {
            var url = "chat/send-command";
            await SendPostRequest(NamedHttpClients.Chat, url, request, cancellationToken);
        }
    }
}