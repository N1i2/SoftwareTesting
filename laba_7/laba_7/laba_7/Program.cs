using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

class Program
{
    public static void Main()
    {
        using (IWebDriver driver = new ChromeDriver())
        {
            try
            {
                driver.Navigate().GoToUrl("https://www.wildberries.by");

                var elem1 = driver.FindElement(By.CssSelector("a.j-wba-header-item[data-wba-header-name=\"Job\"]"));
                Console.WriteLine("Element 1:" + elem1.Text);

                var elem2 = driver.FindElement(By.CssSelector("a.navbar-pc__link.j-wba-header-item"));
                Console.WriteLine("Element 2:" + elem2.Text);

                var elem3 = driver.FindElement(By.CssSelector("span[data-link=\"currentCurrency\"]"));
                Console.WriteLine("Element 3:" + elem3.Text);

                var elem4 = driver.FindElement(By.XPath("//a[@class='navbar-pc__link j-wba-header-item' and @href=\"/lk/basket\"]"));
                Console.WriteLine("Element 4:" + elem4.Text);

                var elem5 = driver.FindElement(By.XPath("//a[contains(@class, 'header__balance') and contains(@id, 'balanceBlock')]"));
                Console.WriteLine("Element 5:" + elem5.Text);

                var elem6 = driver.FindElement(By.XPath("//a[contains(@text(), wildb)]"));
                Console.WriteLine("Element 6:" + elem6.Text);

                var elem7 = driver.FindElement(By.TagName("a"));
                Console.WriteLine("Element 7:" + elem7.Text);

                var elem8 = driver.FindElement(By.PartialLinkText("wildb"));
                Console.WriteLine("Element 8:" + elem8.Text);

            }
            catch (Exception)
            {
                Console.WriteLine("Object not exists");
            }

            Console.ReadKey();
        }
    }
}