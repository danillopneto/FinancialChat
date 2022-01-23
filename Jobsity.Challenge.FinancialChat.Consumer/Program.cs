using Jobsity.Challenge.FinancialChat.Consumer;
using Jobsity.Challenge.FinancialChat.Consumer.Infra.Configuration;
using Jobsity.Challenge.FinancialChat.Consumer.Infra.Gateways;
using Jobsity.Challenge.FinancialChat.Consumer.Infra.MessageBroker;
using Jobsity.Challenge.FinancialChat.Consumer.Interfaces.Gateways;
using Jobsity.Challenge.FinancialChat.Consumer.Interfaces.MessageBroker;
using Jobsity.Challenge.FinancialChat.Core.Utils;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // Add configurations.
        services.AddSingleton(hostContext.Configuration.Get<DataAppSettings>());

        services.AddHttpClient(NamedHttpClients.Chat)
            .ConfigureHttpClient(
                client =>
                {
                    client.BaseAddress = new Uri(hostContext.Configuration.Get<DataAppSettings>().ChatApiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                })
            .AddTransientHttpErrorPolicy(HttpUtils.PollyConfiguration());

        services.AddSingleton<IMessageBroker, RabbitMQBroker>();
        services.AddSingleton<IChatGateway, ChatGateway>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();