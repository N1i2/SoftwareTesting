using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace Laba_8
{
    public static class Constants
    {
        public const string Adress = "https://oz.by/";
    }

    public static class Optionas
    {
        public static ChromeOptions GetOptions()
        {
            var options = new ChromeOptions();

            options.AddArgument("--start-maximized");

            return options;
        }
    }

    public class BasePage
    {
        protected IWebDriver Driver;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void NavigateTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public void AcceptCookies()
        {
            var cookieButton = Driver.FindElement(By.CssSelector("button.btn.btn-lg.btn-outline-tertiary.w-100.m-0"));
            cookieButton.Click();
        }

        public void EndOneTest()
        {
            Console.WriteLine("Enter some key to continue ...");
            Console.ReadKey();
        }
    }

    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver) { }

        public void Login(string phoneNumber)
        {
            var loginButton = Driver.FindElement(By.CssSelector("a.link.user-bar__item[href='#']"));
            loginButton.Click();

            Thread.Sleep(2000);
            var phoneInput = Driver.FindElement(By.ClassName("form-control"));
            phoneInput.SendKeys(phoneNumber);

            var sendButtons = Driver.FindElements(By.CssSelector("button.btn"));
            //sendButtons[2].Click();
        }
    }

    public class SearchPage : BasePage
    {
        public SearchPage(IWebDriver driver) : base(driver) { }

        public void SearchForItem(string query)
        {
            var searchInput = Driver.FindElement(By.Id("top-s"));
            var searchButton = Driver.FindElements(By.ClassName("search-form__submit"));

            searchInput.SendKeys(query);
            Thread.Sleep(2000);
            searchButton[0].Click();
        }

        public bool ValidateResultsContain()
        {
            Thread.Sleep(3000);
            var results = Driver.FindElements(By.ClassName("digi-product"));

            foreach (var result in results)
            {
                if (!result.Text.Contains("Словарь", StringComparison.OrdinalIgnoreCase) &&
                    !result.Text.Contains("словарь", StringComparison.OrdinalIgnoreCase) &&
                    !result.Text.Contains("Словарик", StringComparison.OrdinalIgnoreCase) &&
                    !result.Text.Contains("словарик", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Not all results contain keyword: {result.Text}");
                    return false;
                }
            }

            Console.WriteLine("All results contain the keyword.");
            return true;
        }

        public void AddItemToCart()
        {
            var item = Driver.FindElements(By.ClassName("digi-product"))[10];
            item.Click();

            Thread.Sleep(4000);
            var addToCartButton = Driver.FindElement(By.CssSelector("button.b-product-control__button.i-button.i-button_large.i-button_orange.addtocart-btn.first-button"));
            addToCartButton.Click();

            Thread.Sleep(3000);
            var cartButton = Driver.FindElement(By.PartialLinkText("Корзина"));
            cartButton.Click();
        }
    }

    public class NavigationPage : BasePage
    {
        public NavigationPage(IWebDriver driver) : base(driver) { }

        public void NavigateToBooks()
        {
            var booksLink = Driver.FindElement(By.CssSelector("a.menu-link-action.main-nav__list__item[href='/books/']"));
            booksLink.Click();
            Thread.Sleep(3000);
        }

        public void NavigateToSouvenirs()
        {
            var souvenirsLink = Driver.FindElement(By.ClassName("main-nav__header"));
            souvenirsLink.Click();

            Thread.Sleep(3000);
            var souvenirsType = Driver.FindElement(By.CssSelector("a.menu-link-action.main-nav__list__item[href=\"/souvenir/\"]"));
            souvenirsType.Click();
        }
    }

    class Program
    {
        public static void Main()
        {
            const string phoneNumber = "256034469";
            const string searchWord = "Словарь";

            var opt = Optionas.GetOptions();

            using (var driver = new ChromeDriver(opt))
            {
                try
                {
                    var loginPage = new LoginPage(driver);
                    loginPage.NavigateTo(Constants.Adress);
                    loginPage.AcceptCookies();
                    loginPage.Login(phoneNumber);
                    loginPage.EndOneTest();

                    var searchPage = new SearchPage(driver);
                    searchPage.NavigateTo(Constants.Adress);
                    searchPage.SearchForItem(searchWord);
                    if (searchPage.ValidateResultsContain())
                    {
                        searchPage.AddItemToCart();
                    }
                    searchPage.EndOneTest();

                    var navigationPage = new NavigationPage(driver);
                    navigationPage.NavigateTo(Constants.Adress);
                    navigationPage.NavigateToBooks();
                    navigationPage.NavigateToSouvenirs();
                    navigationPage.EndOneTest();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    driver.Quit();
                    Console.ReadKey();
                }
            }
        }
    }
}
