using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Schedule.Bot.Extensions;
using Schedule.Bot.Services;
using Schedule.Bot.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Schedule.Bot.Controllers
{
    [Route("")]
    public class MainController : Controller
    {
        private const string WebClientId = "__web__";
        private readonly IMessageService _messageService;
        private readonly IConfiguration _configuration;
        private readonly ITelegramService _telegramService;


        public MainController(ITelegramService telegramService, IMessageService messageService, IConfiguration configuration)
        {
            this._telegramService = telegramService;
            this._messageService = messageService;
            this._configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/{token}/update")]
        public async Task<IActionResult> Post([FromBody]Update update, CancellationToken cancellationToken)
        {
            try
            {
                await _telegramService.ReceivedMessage(update, cancellationToken);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
            }
            return NoContent();
        }

        [HttpGet("schedule/tometro")]
        public async Task<string> GetNearestToMetro(DateTime? date)
        {
            try
            {
                return await _messageService.GetNearestTimeMessage(WebClientId, _configuration.ToMetroTitle(), date);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("schedule/towork")]
        public async Task<string> GetNearestToWork(DateTime? date)
        {
            try
            {
                return await _messageService.GetNearestTimeMessage(WebClientId, _configuration.ToWorkTitle(), date);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("time/tometro")]
        public IActionResult GetTimeToMetro()
        {
            return View();
        }
    }
}
