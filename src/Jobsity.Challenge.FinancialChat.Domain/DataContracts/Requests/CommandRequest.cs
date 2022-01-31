namespace Jobsity.Challenge.FinancialChat.Domain.DataContracts.Requests
{
    public class CommandRequest
    {
        public Guid Destination { get; set; }

        public string Message { get; set; }

        public Guid SenderId { get; set; }
    }
}