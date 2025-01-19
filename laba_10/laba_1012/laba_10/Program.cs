using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Laba_8
{
    class Progect
    {
        public static string adress = "https://oz.by/?refsource=google_books&utm_source=google&utm_medium=cpc&utm_campaign=%7Bcampaign_id%7D&utm_content=%7Bad_id%7D&utm_term=&gad_source=1&gclid=CjwKCAiA9IC6BhA3EiwAsbltOCFbiehgRizTcrPscg9tij5afTM4rKUl7VrylkcEvPzjGEyLsv6HPxoCsXEQAvD_BwE&disallowedCookie=1";
        public static void Main()
        {
            string myNumb = "256034469";

            FirstTest(myNumb);
            SecondTest();
            ThreedTest();

            Console.ReadKey();
        }

        public static void FirstTest(string myNumb)
        {
            var options = GetOptions();

            using (var driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Navigate().GoToUrl(adress);

                    var cookieLose = driver.FindElement(By.CssSelector("button.btn.btn-lg.btn-outline-tertiary.w-100.m-0"));
                    cookieLose.Click();

                    var login = driver.FindElement(By.CssSelector("a.link.user-bar__item[href=\"#\"]"));
                    login.Click();

                    System.Threading.Thread.Sleep(2000);

                    var phoneNumb = driver.FindElement(By.ClassName("form-control"));
                    phoneNumb.SendKeys(myNumb);

                    var sendNumb = driver.FindElements(By.CssSelector("button.btn"));

                    TakeScreenshot(driver, "screen_1");

                    //sendNumb[2].Click();

                    var cookies = driver.Manage().Cookies.AllCookies;
                    var cookieList = new List<Dictionary<string, string>>();

                    foreach (var cookie in cookies)
                    {
                        Console.WriteLine($"Name: {cookie.Name ?? null}");
                        Console.WriteLine($"Value: {cookie.Value ?? null}");
                        Console.WriteLine($"Domain: {cookie.Domain ?? null}");
                        Console.WriteLine($"Path: {cookie.Path ?? null}");
                        Console.WriteLine($"Expiry: {cookie.Expiry ?? null}");
                        Console.WriteLine($"Secure: {cookie.Secure.ToString() ?? null}");
                        Console.WriteLine($"HttpOnly: {cookie.IsHttpOnly.ToString() ?? null}");

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
                    var path = "cookie.json";
                    File.WriteAllText(path, json);

                    Console.ReadKey();
                }
                catch (Exception)
                {
                    Console.WriteLine("Error");
                }
            }
        }

        public static void SecondTest()
        {
            var options = GetOptions();

            using (var driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Navigate().GoToUrl(adress);

                    var cookieLose = driver.FindElement(By.CssSelector("button.btn.btn-lg.btn-outline-tertiary.w-100.m-0"));
                    cookieLose.Click();

                    System.Threading.Thread.Sleep(2000);
                    var findText = driver.FindElement(By.Id("top-s"));
                    var findButton = driver.FindElements(By.ClassName("search-form__submit"));

                    findText.SendKeys("Словарь");
                    System.Threading.Thread.Sleep(2000);
                    findButton[0].Click();

                    System.Threading.Thread.Sleep(5000);
                    var check = driver.FindElements(By.ClassName("digi-facet-option__checkmark"));
                    var elem = driver.FindElements(By.ClassName("digi-product"));
                    bool dict = true;

                    foreach (var ele in elem)
                    {
                        if (!ele.Text.Contains("словарь") && !ele.Text.Contains("словарик") && !ele.Text.Contains("Словарь"))
                        {
                            Console.WriteLine($"Not all dictionary: {ele.Text}");
                            dict = false;
                            break;
                        }
                    }
                    if (dict)
                    {
                        Console.WriteLine("All dictionary");
                    }
                    System.Threading.Thread.Sleep(3000);

                    check[60].Click();
                    elem[10].Click();

                    System.Threading.Thread.Sleep(4000);
                    var busketButt = driver.FindElement(By.CssSelector("button.b-product-control__button.i-button.i-button_large.i-button_orange.addtocart-btn.first-button"));
                    var busket = driver.FindElement(By.PartialLinkText("Корзина"));

                    TakeScreenshot(driver, "screen_2");

                    busketButt.Click();
                    System.Threading.Thread.Sleep(3000);
                    busket.Click();
                }
                catch (Exception)
                {
                    Console.WriteLine("Error");
                }

                Console.ReadKey();
            }
        }

        public static void ThreedTest()
        {
            var options = GetOptions();

            using (var driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Navigate().GoToUrl(adress);

                    var cookieLose = driver.FindElement(By.CssSelector("button.btn.btn-lg.btn-outline-tertiary.w-100.m-0"));
                    cookieLose.Click();

                    var typeOfButton = driver.FindElement(By.CssSelector("a.menu-link-action.main-nav__list__item[href=\"/books/\"]"));
                    typeOfButton.Click();

                    System.Threading.Thread.Sleep(3000);
                    var allProd = driver.FindElement(By.ClassName("main-nav__header"));
                    allProd.Click();

                    System.Threading.Thread.Sleep(3000);
                    var typeOfButton2 = driver.FindElement(By.CssSelector("a.menu-link-action.main-nav__list__item[href=\"/souvenir/\"]"));
                    typeOfButton2.Click();

                    TakeScreenshot(driver, "screen_3");
                }
                catch (Exception)
                {
                    Console.WriteLine("Error");
                }

                Console.ReadKey();
            }
        }

        public static ChromeOptions GetOptions()
        {
            var options = new ChromeOptions();

            options.AddArgument("--start-maximized");

            return options;
        }

        public static void TakeScreenshot(IWebDriver driver, string fileName)
        {
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{fileName}.png");
                screenshot.SaveAsFile(filePath);
                Console.WriteLine($"Screenshot saved: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to take screenshot: {ex.Message}");
            }
        }
    }
}


//using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium;
//using System.Text.Json;

//namespace Laba_8
//{
//    class Progect
//    {
//        public static string adress = "https://oz.by/";

//        public static void Main()
//        {
//            string myNumb = "256034469";

//            var testConfigs = new[]
//            {
//        new {Language = "ru", PhoneNumber = myNumb, SearchTerm = "Словарь"},
//        new {Language = "en", PhoneNumber = myNumb, SearchTerm = "Dictionary"}
//    };

//            foreach (var config in testConfigs)
//            {
//                FirstTest(config.Language, config.PhoneNumber);
//                SecondTest(config.Language, config.SearchTerm);
//                ThirdTest(config.Language);
//            }

//            Console.ReadKey();
//        }

//        public static void SwitchLanguage(IWebDriver driver, string language)
//        {
//            try
//            {
//                var langButton = driver.FindElement(By.CssSelector("a.lang-switch__link"));
//                if ((language == "en" && langButton.Text.ToLower().Contains("en")) ||
//                    (language == "ru" && langButton.Text.ToLower().Contains("ru")))
//                {
//                    langButton.Click();
//                    System.Threading.Thread.Sleep(2000);
//                }
//            }
//            catch (NoSuchElementException)
//            {
//                Console.WriteLine("Language switch button not found. Skipping language switch.");
//            }
//        }

//        public static void FirstTest(string language, string myNumb)
//        {
//            var options = GetOptions();

//            using (var driver = new ChromeDriver(options))
//            {
//                try
//                {
//                    driver.Navigate().GoToUrl(adress);
//                    SwitchLanguage(driver, language);

//                    var cookieLose = driver.FindElement(By.CssSelector("button.btn.btn-lg.btn-outline-tertiary.w-100.m-0"));
//                    cookieLose.Click();

//                    var login = driver.FindElement(By.CssSelector("a.link.user-bar__item[href=\"#\"]"));
//                    login.Click();

//                    System.Threading.Thread.Sleep(2000);

//                    var phoneNumb = driver.FindElement(By.ClassName("form-control"));
//                    phoneNumb.SendKeys(myNumb);

//                    var sendNumb = driver.FindElements(By.CssSelector("button.btn"));

//                    TakeScreenshot(driver, $"screen_1_{language}");

//                    var cookies = driver.Manage().Cookies.AllCookies;
//                    var cookieList = new List<Dictionary<string, string>>();

//                    foreach (var cookie in cookies)
//                    {
//                        cookieList.Add(new Dictionary<string, string>
//                {
//                    { "Name", cookie.Name ?? "null" },
//                    { "Value", cookie.Value ?? "null" },
//                    { "Domain", cookie.Domain ?? "null" },
//                    { "Path", cookie.Path ?? "null" },
//                    { "Expiry", cookie.Expiry?.ToString() ?? "null" },
//                    { "Secure", cookie.Secure.ToString() ?? "null" },
//                    { "HttpOnly", cookie.IsHttpOnly.ToString() ?? "null" }
//                });
//                    }

//                    var json = JsonSerializer.Serialize(cookieList, new JsonSerializerOptions { WriteIndented = true });
//                    var path = $"cookie_{language}.json";
//                    File.WriteAllText(path, json);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error in FirstTest ({language}): {ex.Message}");
//                }
//            }
//        }

//        public static void SecondTest(string language, string searchTerm)
//        {
//            var options = GetOptions();

//            using (var driver = new ChromeDriver(options))
//            {
//                try
//                {
//                    driver.Navigate().GoToUrl(adress);
//                    SwitchLanguage(driver, language);

//                    var cookieLose = driver.FindElement(By.CssSelector("button.btn.btn-lg.btn-outline-tertiary.w-100.m-0"));
//                    cookieLose.Click();

//                    System.Threading.Thread.Sleep(2000);
//                    var findText = driver.FindElement(By.Id("top-s"));
//                    var findButton = driver.FindElements(By.ClassName("search-form__submit"));

//                    findText.SendKeys(searchTerm);
//                    System.Threading.Thread.Sleep(2000);
//                    findButton[0].Click();

//                    System.Threading.Thread.Sleep(5000);
//                    var elem = driver.FindElements(By.ClassName("digi-product"));
//                    bool allMatching = elem.All(e => e.Text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

//                    Console.WriteLine(allMatching ? "All results match search term." : "Some results do not match search term.");
//                    TakeScreenshot(driver, $"screen_2_{language}");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error in SecondTest ({language}): {ex.Message}");
//                }
//            }
//        }

//        public static void ThirdTest(string language)
//        {
//            var options = GetOptions();

//            using (var driver = new ChromeDriver(options))
//            {
//                try
//                {
//                    driver.Navigate().GoToUrl(adress);
//                    SwitchLanguage(driver, language);

//                    var cookieLose = driver.FindElement(By.CssSelector("button.btn.btn-lg.btn-outline-tertiary.w-100.m-0"));
//                    cookieLose.Click();

//                    var typeOfButton = driver.FindElement(By.CssSelector("a.menu-link-action.main-nav__list__item[href=\"/books/\"]"));
//                    typeOfButton.Click();

//                    System.Threading.Thread.Sleep(3000);
//                    var allProd = driver.FindElement(By.ClassName("main-nav__header"));
//                    allProd.Click();

//                    System.Threading.Thread.Sleep(3000);
//                    var typeOfButton2 = driver.FindElement(By.CssSelector("a.menu-link-action.main-nav__list__item[href=\"/souvenir/\"]"));
//                    typeOfButton2.Click();

//                    TakeScreenshot(driver, $"screen_3_{language}");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error in ThirdTest ({language}): {ex.Message}");
//                }
//            }
//        }
//        public static ChromeOptions GetOptions()
//        {
//            var options = new ChromeOptions();

//            options.AddArgument("--start-maximized");

//            return options;
//        }

//        public static void TakeScreenshot(IWebDriver driver, string fileName)
//        {
//            try
//            {
//                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
//                var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{fileName}.png");
//                screenshot.SaveAsFile(filePath);
//                Console.WriteLine($"Screenshot saved: {filePath}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Failed to take screenshot: {ex.Message}");
//            }
//        }
//    }
//}