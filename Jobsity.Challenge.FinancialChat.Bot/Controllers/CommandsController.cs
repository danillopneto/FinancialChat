using Jobsity.Challenge.FinancialChat.Bot.Interfaces.UseCases;
using Microsoft.AspNetCore.Mvc;
using Jobsity.Challenge.FinancialChat.Bot.Domain.Dtos;

namespace Jobsity.Challenge.FinancialChat.Bot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandsController : ControllerBase
    {
        #region " FIELDS "

        private readonly IFilterCommandUseCase _filterCommandUseCase;

        private readonly ILogger<CommandsController> _logger;

        private readonly IProcessCommandUseCase _processCommandUseCase;

        #endregion " FIELDS "

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
                var (allowedCommand, commandParameter) = _filterCommandUseCase.Filter(command.Message);
                if (allowedCommand is not null)
                {
                    command.CommandParameter = commandParameter;
                    _processCommandUseCase.ProcessAsync(command, allowedCommand, cancellationToken);
                    return Ok();
                }
                else
                {
                    return BadRequest($"The command {command.Message} is not allowed.");
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