using CsvHelper;
using Jobsity.Challenge.FinancialChat.Bot.Converters;
using Jobsity.Challenge.FinancialChat.Bot.Infra.Configurations;
using Jobsity.Challenge.FinancialChat.Bot.Interfaces.Gateways;
using Jobsity.Challenge.FinancialChat.Bot.Interfaces.UseCases;
using Jobsity.Challenge.FinancialChat.Bot.Models;
using System.Globalization;

namespace Jobsity.Challenge.FinancialChat.Bot.UseCases.Services
{
    public class ProcessCommandUseCase : IProcessCommandUseCase
    {
        private readonly IGetInfoCommandGateway _getInfoCommandGateway;

        public ProcessCommandUseCase(IGetInfoCommandGateway getInfoCommandGateway)
        {
            _getInfoCommandGateway = getInfoCommandGateway ?? throw new ArgumentNullException(nameof(getInfoCommandGateway));
        }

        public async Task ProcessAsync(CommandDto command, AllowedCommandSettings allowedCommand, CancellationToken cancellationToken)
        {
            var result = await _getInfoCommandGateway.GetInfoAsync(allowedCommand, command.CommandParameter);
            using var reader = new StringReader(result);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csvReader.GetRecords<StockPrice>().ToList();
            if (records is not null && records.Any())
            {
                await PostMessage(records.First().ToString(), command);
            }
        }

        private Task PostMessage(string v, CommandDto command)
        {
            throw new NotImplementedException();
        }
    }
}