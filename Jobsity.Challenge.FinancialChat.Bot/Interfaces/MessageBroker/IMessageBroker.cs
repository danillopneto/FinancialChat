using Jobsity.Challenge.FinancialChat.Bot.Infra.Configurations;

namespace Jobsity.Challenge.FinancialChat.Bot.Interfaces.MessageBroker
{
    public interface IMessageBroker
    {
        void Publish<T>(T messageData, MessageBrokerSettings messageBroker);
    }
}