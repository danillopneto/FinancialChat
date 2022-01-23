using CsvHelper.Configuration.Attributes;

namespace Jobsity.Challenge.FinancialChat.Bot.Converters
{
    public class StockPrice
    {
        public string Symbol { get; set; }

        public decimal Close { get; set; }

        public override string ToString()
        {
            return $"{Symbol} quote is ${Close:N2} per share";
        }
    }
}