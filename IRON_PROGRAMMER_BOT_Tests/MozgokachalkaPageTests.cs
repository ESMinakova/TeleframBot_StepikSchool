using IRON_PROGRAMMER_BOT_Common;
using IRON_PROGRAMMER_BOT_Common.Pages;
using IRON_PROGRAMMER_BOT_Common.Pages.MozgokachalkaBranch;
using IRON_PROGRAMMER_BOT_Common.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User;
using IRON_PROGRAMMER_BOT_Tests.Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace IRON_PROGRAMMER_BOT_Tests
{
    public class MozgokachalkaPageTests
    {
        private IServiceProvider services;

        [OneTimeSetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var serviceCollection = new ServiceCollection();

            ContainerConfigurator.Configure(configuration, serviceCollection);
            services = serviceCollection.BuildServiceProvider();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (services is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        [Test]
        public async Task ViewAsync_FirstEnter_CorrectTextAndKeyboard()
        {
            //Arrange
            var mozgokachalkaPage = services.GetRequiredService<MozgokacahlkaPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>()]);
            var userState = new UserState(pages, new UserData());
            var update = new Update();
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("Правила участия")],
                [InlineKeyboardButton.WithCallbackData("Регистрация")] 
            };
           
            //Act
            var result = await mozgokachalkaPage.ViewAsync(update, userState);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResultBase)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(mozgokachalkaPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));
            Assert.That(result.Text, Is.EqualTo(IRON_PROGRAMMER_BOT_Common.Resources.MozgokachalkaPageText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.That(result.IsMedia, Is.EqualTo(false));              
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyboardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup!);             
        }


        [Test]
        public async Task HandleAsync_MozgokachalkaRulesCallback_RedirectMozgokachalkaRulesPage()
        {
            //Arrange
            var mozgokachalkaPage = services.GetRequiredService<MozgokacahlkaPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(),
                                             services.GetRequiredService<StartPage>(), 
                                             mozgokachalkaPage]);
            var userState = new UserState(pages, new UserData());
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "Правила участия" } };

            //Act
            var result = await mozgokachalkaPage.HandleAsync(update, userState);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResultBase)));
            Assert.IsInstanceOf<MozgokachalkaRulesPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
        }

        [Test]
        public async Task HandleAsync_CommonCourcesCallback_RedirectCommonCourcesPage()
        {
            //Arrange
            var mozgokachalkaPage = services.GetRequiredService<MozgokacahlkaPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), 
                                            services.GetRequiredService<StartPage>(),
                                            mozgokachalkaPage]);
            var userState = new UserState(pages, new UserData());
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "Регистрация" } };


            //Act
            var result = await mozgokachalkaPage.HandleAsync(update, userState);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResultBase)));
            Assert.IsInstanceOf<RegistrationPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
        }

        [Test]
        public async Task HandleAsync_ResetMessage_ReturnToStartPage()
        {
            //Arrange
            var mozgokachalkaPage = services.GetRequiredService<MozgokacahlkaPage>();
            var startPage = services.GetRequiredService<StartPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), startPage, mozgokachalkaPage]);
            var userState = new UserState(pages, new UserData());
            var update = new Update() { Message = new Message() { Text = "/reset" } };
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("МОЗГОКАЧАЛКА")],
                [InlineKeyboardButton.WithCallbackData("Наши курсы")]
            };

            //Act
            var result = (PhotoPageResult)(await startPage.HandleAsync(update, userState));

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(startPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
            Assert.That(result.Text, Is.EqualTo(IRON_PROGRAMMER_BOT_Common.Resources.StartPageText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.That(result.IsMedia, Is.EqualTo(true));
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyboardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup!);
        }

        [Test]
        public async Task HandleAsync_UnexpectedMessage_ReloadMozgokachalkaPage()
        {
            //Arrange
            var mozgokachalkaPage = services.GetRequiredService<MozgokacahlkaPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), mozgokachalkaPage]);
            var userState = new UserState(pages, new UserData());
            var update = new Update() { Message = new Message() { Text = "12345" } };
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("Правила участия")],
                [InlineKeyboardButton.WithCallbackData("Регистрация")]
            };

            //Act
            var result = await mozgokachalkaPage.HandleAsync(update, userState);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResultBase)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(mozgokachalkaPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));
            Assert.That(result.Text, Is.EqualTo(IRON_PROGRAMMER_BOT_Common.Resources.MozgokachalkaPageText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.That(result.IsMedia, Is.EqualTo(false));
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyboardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup!);
        }
    }
}