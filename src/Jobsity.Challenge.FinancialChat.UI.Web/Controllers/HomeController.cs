using Jobsity.Challenge.FinancialChat.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Jobsity.Challenge.FinancialChat.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        public const string SignalRUrl = "SignalRUrl";

        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IActionResult Index()
        {

            ViewData[SignalRUrl] = _configuration.GetValue<string>(SignalRUrl);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}