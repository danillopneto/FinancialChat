using Jobsity.Challenge.FinancialChat.Bot.Infra.Configurations;
using Jobsity.Challenge.FinancialChat.Core.Infra.Configurations;

namespace Jobsity.Challenge.FinancialChat.Bot.Interfaces.MessageBroker
{
    public interface IMessageBroker
    {
        void Publish<T>(T message, MessageBrokerSettings messageBroker);
    }
}