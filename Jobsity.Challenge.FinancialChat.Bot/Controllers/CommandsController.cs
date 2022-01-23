using Jobsity.Challenge.FinancialChat.Bot.Interfaces.UseCases;
using Jobsity.Challenge.FinancialChat.Bot.Models;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Challenge.FinancialChat.Bot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly IFilterCommandUseCase _filterCommandUseCase;
        private readonly IProcessCommandUseCase _processCommandUseCase;

        private readonly ILogger<CommandsController> _logger;

        public CommandsController(
                                  IFilterCommandUseCase filterCommandUseCase,
                                  IProcessCommandUseCase processCommandUseCase,
                                  ILogger<CommandsController> logger)
        {
            _filterCommandUseCase = filterCommandUseCase ?? throw new ArgumentNullException(nameof(filterCommandUseCase));
            _processCommandUseCase = processCommandUseCase ?? throw new ArgumentNullException(nameof(processCommandUseCase));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> ProcessCommand(CommandDto command, CancellationToken cancellationToken)
        {
            try
            {
                var (allowedCommand, commandParameter) = _filterCommandUseCase.Filter(command.Command);
                if (allowedCommand is not null)
                {
                    command.CommandParameter = commandParameter;
                    _processCommandUseCase.ProcessAsync(command, allowedCommand, cancellationToken);
                    return Ok();
                }
                else
                {
                    return BadRequest($"The command {command.Command} is not allowed.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro trying to process the command.");
                return BadRequest();
            }
        }
    }
}