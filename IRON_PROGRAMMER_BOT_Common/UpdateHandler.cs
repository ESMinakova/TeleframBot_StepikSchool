using IRON_PROGRAMMER_BOT_Common.Pages;
using IRON_PROGRAMMER_BOT_Common.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.Storage;
using IRON_PROGRAMMER_BOT_Common.User;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common
{
    public class UpdateHandler(UserStateStorage storage, IServiceProvider services) : IUpdateHandler
    {

        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Type != Telegram.Bot.Types.Enums.UpdateType.Message
                && update.Type != Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
                return;

            var tgUserID = update.Type == Telegram.Bot.Types.Enums.UpdateType.Message
                ? update.Message?.From?.Id
                : update.CallbackQuery?.From.Id;
            if (tgUserID == null) return;
            var telegramUserID = (long)tgUserID;
            Console.WriteLine($"update_id{update.Id}, telegramUserId = {telegramUserID}");

            var userState = await storage.TryGetAsync(telegramUserID);
            if (userState == null)
            {
                userState = new UserState(new Stack<IPage>([services.GetRequiredService<NotStatedPage>()]), new UserData());
            }
            Console.WriteLine($"update_id{update.Id}, CURRENT_userState = {userState}");

            var result = await userState!.CurrentPage.HandleAsync(update, userState);

            Console.WriteLine($"update_id{update.Id}, text = {result.Text}, UPDATED_State = {result.UpdatedUserState}");

            var lastMessage = await SendResultAsync(client, update, telegramUserID, result);
            result.UpdatedUserState.UserData.LastMessage = new MessageData(lastMessage.MessageId, result.IsMedia);
            await storage.AddOrUpdate(telegramUserID, result.UpdatedUserState);
        }
        public async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine(exception.Message);
        }
        private static async Task<Message> SendResultAsync(ITelegramBotClient client, Update update, long telegramUserID, PageResultBase result)
        {
            switch (result)
            {
                case PhotoPageResult photoPageResult:
                    return await SendPhotoAsync(client, update, telegramUserID, photoPageResult);
                case AudioPageResult audioPageResult:
                    return await SendAudioAsync(client, update, telegramUserID, audioPageResult);
                case DocumentPageResult documentPageResult:
                    return await SendDocumentAsync(client, update, telegramUserID, documentPageResult);
                default:
                    return await SendTextAsync(client, update, telegramUserID, result);

            }
        }

        private static async Task<Message> SendTextAsync(ITelegramBotClient client, Update update, long telegramUserID, PageResultBase result)
        {
            if (update.CallbackQuery != null && (!result.UpdatedUserState.UserData.LastMessage?.IsMedia ?? false))
            {
                return await client.EditMessageTextAsync(
                    chatId: telegramUserID,
                    messageId: result.UpdatedUserState.UserData.LastMessage!.Id,
                    text: result.Text,
                    replyMarkup: (InlineKeyboardMarkup)result!.ReplyMarkup!,
                    parseMode: result.ParseMode,
                    disableWebPagePreview: true);
            }
            if (result.UpdatedUserState.UserData.LastMessage != null)
            {
                await client.DeleteMessageAsync(
                    chatId: telegramUserID,
                    messageId: result.UpdatedUserState.UserData.LastMessage.Id);
            }

            return await client.SendTextMessageAsync(
                    chatId: telegramUserID,
                    text: result.Text,
                    replyMarkup: result.ReplyMarkup,
                    disableWebPagePreview: true,
                    parseMode: result.ParseMode);
        }

        private static async Task<Message> SendPhotoAsync(ITelegramBotClient client, Update update, long telegramUserID, PhotoPageResult result)
        {
            if (update.CallbackQuery != null && (result.UpdatedUserState.UserData.LastMessage?.IsMedia ?? false))
            {
                return await client.EditMessageMediaAsync(
                    chatId: telegramUserID,
                    messageId: result.UpdatedUserState.UserData.LastMessage.Id,
                    media: new InputMediaPhoto(result.Photo)
                    {
                        Caption = result.Text,
                        ParseMode = result.ParseMode
                    },
                    replyMarkup: (InlineKeyboardMarkup)result.ReplyMarkup!);
            }
            if (result.UpdatedUserState.UserData.LastMessage != null)
            {
                await client.DeleteMessageAsync(
                    chatId: telegramUserID,
                    messageId: result.UpdatedUserState.UserData.LastMessage.Id);
            }
            return await client.SendPhotoAsync(
                    chatId: telegramUserID,
                    photo: result.Photo,
                    caption: result.Text,
                    replyMarkup: result.ReplyMarkup,
                    parseMode: result.ParseMode);

        }
        private static async Task<Message> SendAudioAsync(ITelegramBotClient client, Update update, long telegramUserID, AudioPageResult result)
        {
            if (update.CallbackQuery != null && (result.UpdatedUserState.UserData.LastMessage?.IsMedia ?? false))
            {
                return await client.EditMessageMediaAsync(
                    chatId: telegramUserID,
                    messageId: result.UpdatedUserState.UserData.LastMessage.Id,
                    media: new InputMediaPhoto(result.Audio)
                    {
                        Caption = result.Text,
                        ParseMode = result.ParseMode
                    },
                    replyMarkup: (InlineKeyboardMarkup)result.ReplyMarkup!);
            }
            if (result.UpdatedUserState.UserData.LastMessage != null)
            {
                await client.DeleteMessageAsync(
                    chatId: telegramUserID,
                    messageId: result.UpdatedUserState.UserData.LastMessage.Id);
            }
            return await client.SendAudioAsync(
                chatId: telegramUserID,
                audio: result.Audio,
                caption: result.Text,
                replyMarkup: result.ReplyMarkup,
                parseMode: result.ParseMode);

        }
        private static async Task<Message> SendDocumentAsync(ITelegramBotClient client, Update update, long telegramUserID, DocumentPageResult result)
        {
            if (update.CallbackQuery != null && (result.UpdatedUserState.UserData.LastMessage?.IsMedia ?? false))
            {
                return await client.EditMessageMediaAsync(
                    chatId: telegramUserID,
                    messageId: result.UpdatedUserState.UserData.LastMessage.Id,
                    media: new InputMediaPhoto(result.Document)
                    {
                        Caption = result.Text,
                        ParseMode = result.ParseMode
                    },
                    replyMarkup: (InlineKeyboardMarkup)result.ReplyMarkup!);
            }
            if (result.UpdatedUserState.UserData.LastMessage != null)
            {
                await client.DeleteMessageAsync(
                    chatId: telegramUserID,
                    messageId: result.UpdatedUserState.UserData.LastMessage.Id);
            }
            return await client.SendDocumentAsync(
                chatId: telegramUserID,
                document: result.Document,
                caption: result.Text,
                replyMarkup: result.ReplyMarkup,
                disableContentTypeDetection: true,
                parseMode: result.ParseMode
            );
        }
    }
}