using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Jobsity.Challenge.FinancialChat.Web.Pages
{
    public class IndexModel : PageModel
    {
        public const string SignalRUrl = "SignalRUrl";

        private readonly IConfiguration _configuration;

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void OnGet()
        {
            try
            {
                ViewData[SignalRUrl] = _configuration.GetValue<string>(SignalRUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting SignalRUrl");
            }
        }
    }
}