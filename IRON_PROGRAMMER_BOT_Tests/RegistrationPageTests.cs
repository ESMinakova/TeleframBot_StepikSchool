using IRON_PROGRAMMER_BOT_Tests.Properties;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using IRON_PROGRAMMER_BOT_Common.Pages.MozgokachalkaBranch;
using IRON_PROGRAMMER_BOT_Common.Pages;
using IRON_PROGRAMMER_BOT_Common.User;
using IRON_PROGRAMMER_BOT_Common.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IRON_PROGRAMMER_BOT_Tests
{
    public class RegistrationPageTests
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
            var registrationPage = services.GetRequiredService<RegistrationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(),
                                            services.GetRequiredService<StartPage>(), 
                                            services.GetRequiredService<MozgokacahlkaPage>()]);        
            var userState = new UserState(pages, new UserData());
            var update = new Update();
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("Назад")]
            };

            //Act
            var result = await registrationPage.ViewAsync(update, userState);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResultBase)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(registrationPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
            Assert.That(result.Text, Is.EqualTo(IRON_PROGRAMMER_BOT_Common.Resources.RegistrationPageText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.That(result.IsMedia, Is.EqualTo(false));
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyboardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup!);
        }


        [Test]
        public async Task HandleAsync_RegistrationPageCallback_RedirectMozgokachalkaPage()
        {
            //Arrange
            var registrationPage = services.GetRequiredService<RegistrationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), 
                                            services.GetRequiredService<StartPage>(),
                                            services.GetRequiredService<MozgokacahlkaPage>(), 
                                            registrationPage]);
            var userState = new UserState(pages, new UserData());
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "Назад" } };

            //Act
            userState.Pages.Pop();
            var result = await userState.CurrentPage.ViewAsync(update, userState);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResultBase)));
            Assert.IsInstanceOf<MozgokacahlkaPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));
        }


        [Test]
        public async Task HandleAsync_ResetMessage_ReturnToStartPage()
        {
            //Arrange
            var registrationPage = services.GetRequiredService<RegistrationPage>();
            var startPage = services.GetRequiredService<StartPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), 
                                            startPage, 
                                            services.GetRequiredService<MozgokacahlkaPage>(), 
                                            registrationPage]);
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
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));
            Assert.That(result.Text, Is.EqualTo(IRON_PROGRAMMER_BOT_Common.Resources.StartPageText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.That(result.IsMedia, Is.EqualTo(true));
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyboardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup!);
        }

        [Test]
        public async Task HandleAsync_UnexpectedMessage_ReloadRegistrationPage()
        {
            //Arrange
            var registrationPage = services.GetRequiredService<RegistrationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(),
                                            services.GetRequiredService<StartPage>(),
                                            services.GetRequiredService<MozgokacahlkaPage>(), 
                                            registrationPage]);
            var userState = new UserState(pages, new UserData());
            var update = new Update() { Message = new Message() { Text = "12345fd" } };
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("Назад")]
            };

            //Act
            var result = await registrationPage.HandleAsync(update, userState);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResultBase)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(registrationPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.That(result.IsMedia, Is.EqualTo(false));
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyboardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup!);
        }

        [Test]
        public async Task HandleAsync_CorrectStepikID_UpdateUserStateAndRedirectToSuccessfulRegistrationPage()
        {
            //Arrange
            var registrationPage = services.GetRequiredService<RegistrationPage>();
            var successfulRegistrationPage = services.GetRequiredService<SuccessfulRegistrationPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), 
                                            services.GetRequiredService<StartPage>(),
                                            services.GetRequiredService<MozgokacahlkaPage>(), 
                                            registrationPage]);
            var userState = new UserState(pages, new UserData());
            var input = "12345";     
            var update = new Update() { Message = new Message() { Text = input } };
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("В начало")]
            };


            //Act
            var result = await registrationPage.HandleAsync(update, userState);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResultBase)));
            Assert.That(result.UpdatedUserState.CurrentPage.GetType(), Is.EqualTo(successfulRegistrationPage.GetType()));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(5));
            Assert.That(result.Text, Is.EqualTo(IRON_PROGRAMMER_BOT_Common.Resources.SuccessfulRegistrationPageText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.That(result.IsMedia, Is.EqualTo(false));
            Assert.That(result.UpdatedUserState.UserData.StepikId, !Is.Null);
            Assert.That(result.UpdatedUserState.UserData.StepikId, Is.EqualTo(input));
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyboardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup!);

        }
    }
}
