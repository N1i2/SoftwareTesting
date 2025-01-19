using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace Laba_8
{
    [TestFixture]
    public class Progect : IDisposable
    {
        private IWebDriver _driver;
        private const string Address = "https://oz.by/?refsource=google_books&utm_source=google&utm_medium=cpc&utm_campaign=%7Bcampaign_id%7D&utm_content=%7Bad_id%7D&utm_term=&gad_source=1&gclid=CjwKCAiA9IC6BhA3EiwAsbltOCFbiehgRizTcrPscg9tij5afTM4rKUl7VrylkcEvPzjGEyLsv6HPxoCsXEQAvD_BwE&disallowedCookie=1";

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            _driver = new ChromeDriver(options);
        }

        [TearDown]
        public void TearDown()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }

        [Test]
        public void FirstTest()
        {
            string myNumb = "256034469";

            try
            {
                _driver.Navigate().GoToUrl(Address);

                var cookieLose = _driver.FindElement(By.CssSelector("button.btn.btn-lg.btn-outline-tertiary.w-100.m-0"));
                cookieLose.Click();

                var login = _driver.FindElement(By.CssSelector("a.link.user-bar__item[href=\"#\"]"));
                login.Click();

                Thread.Sleep(2000);

                var phoneNumb = _driver.FindElement(By.ClassName("form-control"));
                phoneNumb.SendKeys(myNumb);

                var cookies = _driver.Manage().Cookies.AllCookies;
                var cookieList = new List<Dictionary<string, string>>();

                foreach (var cookie in cookies)
                {
                    cookieList.Add(new Dictionary<string, string>
                    {
                        { "Name", cookie.Name ?? "null" },
                        { "Value", cookie.Value ?? "null" },
                        { "Domain", cookie.Domain ?? "null" },
                        { "Path", cookie.Path ?? "null" },
                        { "Expiry", cookie.Expiry?.ToString() ?? "null" },
                        { "Secure", cookie.Secure.ToString() ?? "null" },
                        { "HttpOnly", cookie.IsHttpOnly.ToString() ?? "null" }
                    });
                }

                var json = JsonSerializer.Serialize(cookieList, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("cookie.json", json);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Test failed with exception: {ex.Message}");
            }
        }

        [Test]
        public void SecondTest()
        {
            try
            {
                _driver.Navigate().GoToUrl(Address);

                var cookieLose = _driver.FindElement(By.CssSelector("button.btn.btn-lg.btn-outline-tertiary.w-100.m-0"));
                cookieLose.Click();

                Thread.Sleep(2000);

                var findText = _driver.FindElement(By.Id("top-s"));
                findText.SendKeys("Словарь");

                var findButton = _driver.FindElement(By.ClassName("search-form__submit"));
                findButton.Click();

                Thread.Sleep(5000);

                var elements = _driver.FindElements(By.ClassName("digi-product"));
                bool allAreDictionaries = true;

                foreach (var elem in elements)
                {
                    if (!elem.Text.Contains("словарь", StringComparison.OrdinalIgnoreCase))
                    {
                        allAreDictionaries = false;
                        Console.WriteLine($"Not a dictionary: {elem.Text}");
                    }
                }

                Assert.IsTrue(allAreDictionaries, "Not all products are dictionaries.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Test failed with exception: {ex.Message}");
            }
        }

        [Test]
        public void ThirdTest()
        {
            try
            {
                _driver.Navigate().GoToUrl(Address);

                var cookieLose = _driver.FindElement(By.CssSelector("button.btn.btn-lg.btn-outline-tertiary.w-100.m-0"));
                cookieLose.Click();

                var booksCategory = _driver.FindElement(By.CssSelector("a.menu-link-action.main-nav__list__item[href=\"/books/\"]"));
                booksCategory.Click();

                Thread.Sleep(3000);

                var souvenirsCategory = _driver.FindElement(By.CssSelector("a.menu-link-action.main-nav__list__item[href=\"/souvenir/\"]"));
                souvenirsCategory.Click();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Test failed with exception: {ex.Message}");
            }
        }
    }
}
