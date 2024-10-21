using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Tests.Properties
{
    public static class KeyboardHelper
    {
        public static void AssertKeyboard(InlineKeyboardButton[][] expectedButtons, InlineKeyboardMarkup actualKeyboard)
        {
            var actualButtons = actualKeyboard.InlineKeyboard;

            Assert.IsNotNull(actualButtons);
            Assert.That(actualButtons.Count(), Is.EqualTo(expectedButtons.Length));

            for (var i = 0; i < expectedButtons.Length; i++)
            {
                Assert.That(actualButtons.ElementAt(i).Count(), Is.EqualTo(expectedButtons[i].Length), $"Количество кнопок в ряду {i} не совпадает");
                for (var j = 0; j < expectedButtons[i].Length; j++)
                {
                    var expectedButton = expectedButtons[i][j];
                    var actualButton = actualButtons.ElementAt(i).ElementAt(j);

                    Assert.That(actualButton.Text, Is.EqualTo(expectedButton.Text));
                    Assert.That(actualButton.CallbackData, Is.EqualTo(expectedButton.CallbackData));
                    Assert.That(actualButton.Url, Is.EqualTo(expectedButton.Url));
                }
            }

        }
    }
}
