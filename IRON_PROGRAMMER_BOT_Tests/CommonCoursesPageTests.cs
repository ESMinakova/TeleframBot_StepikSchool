using IRON_PROGRAMMER_BOT_Common;
using IRON_PROGRAMMER_BOT_Common.Pages;
using IRON_PROGRAMMER_BOT_Common.Pages.CoursesBranch;
using IRON_PROGRAMMER_BOT_Common.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User;
using IRON_PROGRAMMER_BOT_Tests.Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reactive.Disposables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace IRON_PROGRAMMER_BOT_Tests
{
    public class CommonCoursesPageTests
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
            var commonCourcesPage = services.GetRequiredService<CommonCoursesPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>()]);
            var userState = new UserState(pages, new UserData());
            var update = new Update();
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("œÓ‰„ÓÚÓ‚Í‡ Í ≈√› Ë Œ√›")], 
            };
           
            //Act
            var result = await commonCourcesPage.ViewAsync(update, userState);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResultBase)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(commonCourcesPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));
            Assert.That(result.Text, Is.EqualTo(IRON_PROGRAMMER_BOT_Common.Resources.CommonCoursesPageText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.That(result.IsMedia, Is.EqualTo(false));                   
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyboardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup!);             
        }


        [Test]
        public async Task HandleAsync_OgeAndEgeCallback_RedirectOgeAngEgePage()
        {
            //Arrange
            var commonCourcesPage = services.GetRequiredService<CommonCoursesPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), commonCourcesPage]);
            var userState = new UserState(pages, new UserData());
            var update = new Update() { CallbackQuery = new CallbackQuery() { Data = "œÓ‰„ÓÚÓ‚Í‡ Í ≈√› Ë Œ√›" } };

            //Act
            var result = await commonCourcesPage.HandleAsync(update, userState);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResultBase)));
            Assert.IsInstanceOf<OgeAndEgePage>(result.UpdatedUserState.CurrentPage);
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
        }


        [Test]
        public async Task HandleAsync_UnexpectedMessage_ReloadCommonCourcesPagePage()
        {
            //Arrange
            var commonCourcesPage = services.GetRequiredService<CommonCoursesPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), services.GetRequiredService<StartPage>(), commonCourcesPage]);
            var userState = new UserState(pages, new UserData());
            var update = new Update() { Message = new Message() { Text = "12345" } };
            var expectedButtons = new InlineKeyboardButton[][]
            {
                [InlineKeyboardButton.WithCallbackData("œÓ‰„ÓÚÓ‚Í‡ Í ≈√› Ë Œ√›")]      
            };

            //Act
            var result = await commonCourcesPage.HandleAsync(update, userState);

            //Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(PageResultBase)));
            Assert.That(result.UpdatedUserState.CurrentPage, Is.EqualTo(commonCourcesPage));
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(3));
            Assert.That(result.Text, Is.EqualTo(IRON_PROGRAMMER_BOT_Common.Resources.CommonCoursesPageText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.That(result.IsMedia, Is.EqualTo(false));                     
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyboardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup!);
        }
        [Test]
        public async Task HandleAsync_ResetMessage_ReturnToStartPage()
        {
            //Arrange
            var commonCourcesPage = services.GetRequiredService<CommonCoursesPage>();
            var startPage = services.GetRequiredService<StartPage>();
            var pages = new Stack<IPage>([services.GetRequiredService<NotStatedPage>(), startPage, commonCourcesPage]);
            var userState = new UserState(pages, new UserData());
            var update = new Update() { Message = new Message() { Text = "/reset" } };
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
            Assert.That(result.UpdatedUserState.Pages.Count, Is.EqualTo(4));
            Assert.That(result.Text, Is.EqualTo(IRON_PROGRAMMER_BOT_Common.Resources.StartPageText));
            Assert.That(result.ParseMode, Is.EqualTo(ParseMode.Html));
            Assert.That(result.IsMedia, Is.EqualTo(true));
            Assert.IsInstanceOf<InlineKeyboardMarkup>(result.ReplyMarkup);
            KeyboardHelper.AssertKeyboard(expectedButtons, (InlineKeyboardMarkup)result.ReplyMarkup!);
        }
    }
}