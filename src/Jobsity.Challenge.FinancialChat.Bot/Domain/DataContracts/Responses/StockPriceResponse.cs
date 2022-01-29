namespace Jobsity.Challenge.FinancialChat.Bot.Domain.DataContracts.Responses
{
    public class StockPriceResponse
    {
        public string Symbol { get; set; }

        public decimal Close { get; set; }

        public static implicit operator string(StockPriceResponse stock) => $"{stock.Symbol} quote is ${stock.Close:N2} per share";
    }
}