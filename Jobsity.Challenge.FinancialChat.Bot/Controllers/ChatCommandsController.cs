using Jobsity.Challenge.FinancialChat.Bot.Configurations;
using Jobsity.Challenge.FinancialChat.Bot.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Jobsity.Challenge.FinancialChat.Bot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatCommandsController : ControllerBase
    {
        private readonly DataAppSettings _dataAppSettings;
        private readonly ILogger<ChatCommandsController> _logger;

        public ChatCommandsController(
                                      DataAppSettings dataAppSettings,
                                      ILogger<ChatCommandsController> logger)
        {
            _dataAppSettings = dataAppSettings ?? throw new ArgumentNullException(nameof(dataAppSettings));
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessCommand(CommandDto command, CancellationToken cancellationToken)
        {
            foreach (var allowedCommand in _dataAppSettings.AllowedCommands)
            {
                var regex = new Regex($"(?<={allowedCommand.Command}).*").Matches(command.Message);
                if (regex.Count() > 0)
                {

                    break;
                }
            }

            return Ok();
        }
    }
}