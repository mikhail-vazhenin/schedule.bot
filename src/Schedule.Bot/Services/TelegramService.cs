using Microsoft.Extensions.Configuration;
using Schedule.Bot.Extensions;
using Schedule.Bot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace Schedule.Bot.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IConfiguration _configuration;
        private readonly IMessageService _messageService;

        public TelegramService(IMessageService messageService, ITelegramBotClient telegramBotClient, IConfiguration configuration)
        {
            this._messageService = messageService;
            this._telegramBotClient = telegramBotClient;
            this._configuration = configuration;
        }


        public async Task ReceivedMessage(Update update, CancellationToken cancellationToken)
        {
            try
            {
                if (update.Type == UpdateType.Message && update.Message.Type == MessageType.Text)
                {
                    await GetSchedule(update.Message.Text, update.Message.Chat, update.Message.From, cancellationToken);
                }
                else if (update.Type == UpdateType.InlineQuery)
                {
                    await GetInlineAvailableRoutes(update.InlineQuery);
                }
                else
                    await WelcomeMessage(update.Message.Chat, cancellationToken);
            }
            catch
            {
                throw;
            }
        }

        private async Task GetInlineAvailableRoutes(InlineQuery inlineQuery)
        {
            var userName = inlineQuery.From.GetUserName();
            var results = new InlineQueryResultBase[] {
                await GetInlineVariant(userName, _configuration.ToMetroTitle()),
                await GetInlineVariant(userName, _configuration.ToWorkTitle())
            };


            await _telegramBotClient.AnswerInlineQueryAsync(inlineQuery.Id, results, cacheTime: 1);
        }

        private async Task<InlineQueryResultBase> GetInlineVariant(string userName, string commandText)
        {
            var time = await _messageService.GetNearestTimeMessage(userName, commandText);
            return new InlineQueryResultArticle(
                commandText,
                commandText,
                new InputTextMessageContent(time)
            );
        }

        private async Task GetSchedule(string text, Chat chat, User user, CancellationToken cancellationToken)
        {
            var userName = user.GetUserName();
            var welcomeText = $"Привет, {userName}!";

            var schedule = await _messageService.GetNearestTimeMessage(userName, text);

            if (string.IsNullOrWhiteSpace(schedule))
            {
                await WelcomeMessage(chat, cancellationToken);
            }
            else
            {
                await _telegramBotClient.SendTextMessageAsync(chat.Id, schedule ?? welcomeText, cancellationToken: cancellationToken);
            }
        }

        private async Task WelcomeMessage(ChatId chatId, CancellationToken cancellationToken)
        {
            await _telegramBotClient.SendTextMessageAsync(chatId, "Куда поедем?",
                   replyMarkup: new ReplyKeyboardMarkup(new[]
                       {
                            new KeyboardButton(_configuration.ToMetroTitle()),
                            new KeyboardButton(_configuration.ToWorkTitle())
                       },
                       resizeKeyboard: true),
                   cancellationToken: cancellationToken);
        }
    }
}
