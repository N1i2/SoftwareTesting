using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
            using (var driver = new ChromeDriver())
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

                    sendNumb[2].Click();

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
            using (var driver = new ChromeDriver())
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

                    System.Threading.Thread.Sleep(3000);
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

                    check[60].Click();
                    elem[10].Click();

                    System.Threading.Thread.Sleep(4000);
                    var busketButt = driver.FindElement(By.CssSelector("button.b-product-control__button.i-button.i-button_large.i-button_orange.addtocart-btn.first-button"));
                    var busket = driver.FindElement(By.PartialLinkText("Корзина"));

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
            using (var driver = new ChromeDriver())
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
                }
                catch (Exception)
                {
                    Console.WriteLine("Error");
                }

                Console.ReadKey();
            }
        }
    }
}