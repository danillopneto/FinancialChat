using Jobsity.Challenge.FinancialChat.Bot.Configurations;
using Jobsity.Challenge.FinancialChat.Bot.Interfaces.UseCases;
using Jobsity.Challenge.FinancialChat.Bot.Models;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Challenge.FinancialChat.Bot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly DataAppSettings _dataAppSettings;

        private readonly IProcessCommandUseCase _processCommandUseCase;

        private readonly ILogger<CommandsController> _logger;

        public CommandsController(
                                  IProcessCommandUseCase processCommandUseCase,
                                  ILogger<CommandsController> logger)
        {
            _processCommandUseCase = processCommandUseCase ?? throw new ArgumentNullException(nameof(processCommandUseCase));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> ProcessCommand(CommandDto command, CancellationToken cancellationToken)
        {
            try
            {
                _processCommandUseCase.ProcessAsync(command, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro trying to process the command.");
                return BadRequest();
            }
        }
    }
}