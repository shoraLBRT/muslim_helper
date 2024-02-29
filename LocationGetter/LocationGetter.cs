using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace muslim_helper.LocationGetter
{
    internal class LocationGetter
    {
        private static async Task HttpRequestFromApi(double latitude, double longitude)
        {
            var client = new HttpClient();
            HttpResponseMessage response =
                client.GetAsync(@$"https://geocode-maps.yandex.ru/1.x/?apikey=852ca5cf-0c67-43a9-a7da-bcc7c6366a26&geocode={longitude},{latitude}&kind=locality&results=1").Result;
            await Console.Out.WriteLineAsync(response.Content.ReadAsStringAsync().Result);
            return;
        }
        public static async Task GetLocationFromUser(ITelegramBotClient bot, Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                if (message.Location != null)
                {
                    var location = message.Location;

                    Console.WriteLine($"lat: {location.Latitude}\nlon: {location.Longitude}");
                    await HttpRequestFromApi(location.Latitude, location.Longitude);
                }
                else
                {
                    var list = new List<List<KeyboardButton>>();
                    list.Add([KeyboardButton.WithRequestLocation("Отправить геолокацию")]);

                    var keyboard = new ReplyKeyboardMarkup(list);
                    keyboard.ResizeKeyboard = true;

                    await bot.SendTextMessageAsync(message.Chat.Id, "Отправьте геолокацию", replyMarkup: keyboard);
                }
            }
        }
    }
}
