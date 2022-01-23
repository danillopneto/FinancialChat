using Jobsity.Challenge.FinancialChat.Core.Infra.Configurations;

namespace Jobsity.Challenge.FinancialChat.Consumer.Interfaces.MessageBroker
{
    public interface IMessageBroker
    {
        void ProcessMessage(MessageBrokerSettings messageBroker);
    }
}