using Jobsity.Challenge.FinancialChat.Consumer.Domain.DataContracts.Requests;
using Jobsity.Challenge.FinancialChat.Consumer.Infra.Configuration;
using Jobsity.Challenge.FinancialChat.Consumer.Interfaces.Gateways;
using Jobsity.Challenge.FinancialChat.Core.Infra.Gateways;

namespace Jobsity.Challenge.FinancialChat.Consumer.Infra.Gateways
{
    public class ChatGateway : BaseGateway<ChatGateway>, IChatGateway
    {
        public ChatGateway(
                           IHttpClientFactory httpClientFactory,
                           ILogger<ChatGateway> logger)
            : base(httpClientFactory, logger)
        {
        }

        public async Task SendMessageAsyc(CommandMessageRequest request)
        {
            var url = "chat/send-command";
            await SendPostRequest(NamedHttpClients.Chat, url, request);
        }
    }
}