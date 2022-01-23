using Polly;

namespace Jobsity.Challenge.FinancialChat.Core.Utils
{
    public static class HttpUtils
    {
        public static Func<PolicyBuilder<HttpResponseMessage>, IAsyncPolicy<HttpResponseMessage>> PollyConfiguration()
        {
            return builder => builder.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            });
        }
    }
}