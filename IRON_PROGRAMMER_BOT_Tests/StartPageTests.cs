using IRON_PROGRAMMER_BOT_Common;
using IRON_PROGRAMMER_BOT_Common.Pages;
using IRON_PROGRAMMER_BOT_Common.Pages.CoursesBranch;
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
    public class StartPageTests
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
            var startPage = services.GetRequiredService<StartPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>()]);
            var userState = new UserState(pages, new UserData());
            var update = new Update();
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("ÃŒ«√Œ ¿◊¿À ¿")],
                [InlineKeyboardButton.WithCallbackData("Õ‡¯Ë ÍÛÒ˚")] 
            };
           
            //Act
            var result = (PhotoPageResult)(await startPage.ViewAsync(update, userState));

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(startPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(2));
            Assert.That(result.Text, Is.EqualTo(IRON_PROGRAMMER_BOT_Common.Resources.StartPageText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.That(result.IsMedia, Is.EqualTo(true));
            Assert.IsInstanceOf<InputFileStream>(result.Photo);          
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyboardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup!);             
        }


        [Test]
        public async Task HandleAsync_MozgokachalkaCallback_RedirectMozgokachalkaPage()
        {
            //Arrange
            var startPage = services.GetRequiredService<StartPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), startPage]);
            var userState = new UserState(pages, new UserData());
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "ÃŒ«√Œ ¿◊¿À ¿" } };

            //Act
            var result = await startPage.HandleAsync(update, userState);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResultBase)));
            Assert.IsInstanceOf<MozgokacahlkaPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task HandleAsync_CommonCourcesCallback_RedirectCommonCourcesPage()
        {
            //Arrange
            var startPage = services.GetRequiredService<StartPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), startPage]);
            var userState = new UserState(pages, new UserData());
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "Õ‡¯Ë ÍÛÒ˚" } };


            //Act
            var result = await startPage.HandleAsync(update, userState);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResultBase)));
            Assert.IsInstanceOf<CommonCoursesPage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));
        }


        [Test]
        public async Task HandleAsync_UnexpectedMessage_ReloadStartPage()
        {
            //Arrange
            var startPage = services.GetRequiredService<StartPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), startPage]);
            var userState = new UserState(pages, new UserData());
            var update = new Update() { Message = new Message() { Text = "12345" } };
            var expectedButtons = new InlineKeyboardButton[][]
{
                [InlineKeyboardButton.WithCallbackData("ÃŒ«√Œ ¿◊¿À ¿")],
                [InlineKeyboardButton.WithCallbackData("Õ‡¯Ë ÍÛÒ˚")]
};

            //Act
            var result = (PhotoPageResult)(await startPage.HandleAsync(update, userState));

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PhotoPageResult)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(startPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(2));
            Assert.That(result.Text, Is.EqualTo(IRON_PROGRAMMER_BOT_Common.Resources.StartPageText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.That(result.IsMedia, Is.EqualTo(true));
            Assert.IsInstanceOf<InputFileStream>(result.Photo);            
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyboardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup!);
        }
    }
}