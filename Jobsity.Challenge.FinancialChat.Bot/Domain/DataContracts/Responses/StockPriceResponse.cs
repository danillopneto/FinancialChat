namespace Jobsity.Challenge.FinancialChat.Bot.Domain.DataContracts.Responses
{
    public class StockPriceResponse
    {
        public string Symbol { get; set; }

        public decimal Close { get; set; }

        public override string ToString()
        {
            return $"{Symbol} quote is ${Close:N2} per share";
        }
    }
}