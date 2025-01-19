using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.Json;

namespace Laba_8
{
    class Progect
    {
        public static string adress = "https://oz.by/";
        public static string logFilePath = "log.txt";

        public static void Main()
        {
            string myNumb = "256034469";
            bool end = false;
            string testNumber;
            List<string> types = DesirializeFixtur();

            while (!end)
            {
                Console.WriteLine("\nEnter test:" +
                    "\n1) Login;" +
                    "\n2) Find;" +
                    "\n3) Check box;" +
                    "\n4) End tests\n");
                testNumber = Console.ReadLine() ?? "";

                switch (testNumber)
                {
                    case "1":
                        FirstTest(myNumb);
                        break;
                    case "2":
                        SecondTest(types[new Random().Next(0, 2)]);
                        break;
                    case "3":
                        ThreedTest();
                        break;
                    case "4":
                        end = true;
                        break;
                    default:
                        Console.WriteLine("Error number\n");
                        break;
                }
            }

            Console.ReadKey();
        }

        public static void FirstTest(string myNumb)
        {
            var options = GetOptions();
            DateTime startTime = DateTime.Now;

            WriteLog("Login started", startTime);

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

                    var cookies = driver.Manage().Cookies.AllCookies;

                    WriteLog("Login completed successfully", DateTime.Now);

                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    WriteLog($"Login failed: {ex.Message}", DateTime.Now);
                    Console.ReadKey();
                }
            }
        }

        public static void SecondTest(string type)
        {
            var options = GetOptions();
            DateTime startTime = DateTime.Now;

            WriteLog("Select started", startTime);

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

                    findText.SendKeys(type);
                    System.Threading.Thread.Sleep(2000);
                    findButton[0].Click();

                    System.Threading.Thread.Sleep(5000);

                    var check = driver.FindElements(By.ClassName("digi-facet-option__checkmark"));
                    var elem = driver.FindElements(By.ClassName("digi-product"));
                    bool dict = true;

                    foreach (var ele in elem)
                    {
                        if (!ele.Text.Contains("словарь") && !ele.Text.Contains("словарик") && !ele.Text.Contains("Словарь") 
                            && !ele.Text.Contains("dictionary") && !ele.Text.Contains("Dictionary"))
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

                    WriteLog("Select completed successfully", DateTime.Now);
 
                    busketButt.Click();
                    System.Threading.Thread.Sleep(3000);
                    busket.Click();
                    
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    WriteLog($"Select failed: {ex.Message}", DateTime.Now);
                    Console.ReadKey();
                }
            }
        }

        public static void ThreedTest()
        {
            var options = GetOptions();
            DateTime startTime = DateTime.Now;

            WriteLog("Check box started", startTime);

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

                    WriteLog("Check box completed successfully", DateTime.Now);
               
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    WriteLog($"Check box failed: {ex.Message}", DateTime.Now);
                    Console.ReadKey();
                }
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

        public static void WriteLog(string message, DateTime timestamp)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"[{timestamp:yyyy-MM-dd HH:mm:ss}] {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }
        public static List<string> DesirializeFixtur()
        {
            var list = new List<string>();
            try
            {
                using (var file = new FileStream("fixture.json", FileMode.Open))
                {

                    list = JsonSerializer.Deserialize<List<string>>(file) ?? new List<string>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to read fixture: {ex.Message}");
            }

            if(list.Count <= 0)
            {
                list.Add("Словарь");
                list.Add("Dictionary");
            }

            return list;
        }
    }
}