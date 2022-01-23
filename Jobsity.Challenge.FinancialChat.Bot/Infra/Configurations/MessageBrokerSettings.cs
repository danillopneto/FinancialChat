﻿namespace Jobsity.Challenge.FinancialChat.Bot.Infra.Configurations
{
    public class MessageBrokerSettings
    {
        public string HostName { get; set; }

        public string Queue { get; set; }

        public string RoutingKey { get; set; }
    }
}