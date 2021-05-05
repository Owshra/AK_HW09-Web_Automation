using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace Aušra_Kasiulionytė_HW09___WEB_Automation
{
    public class Tests
    {
        private IWebDriver driver;
        // Adding these private strings so that the Tester can easily change inputs.
        private string email = "megan282@gmail.com";
        private string password = "ausrapractice123";
        private string searchItem = "Summer dress"; // Any word, but if the product doesn't exist - test will fail.
        private string quantity = "5"; // Any whole number.
        private string size = "M"; // S, M, L.
        private string color = "Blue"; // Blue, Black, Orange, but not everywhere.

        [SetUp] // Creating a new webdriver, so that we don't have to create it for each separate Test.
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize(); // Maximize browser.
            driver.Url = "http://automationpractice.com/index.php";
        }

        [Test]
        public void TestHappyPath()
        {
            TestLogin(email, password);
            TestSearch(searchItem);
            TestBuy(quantity, size, color);
        }

        private void TestLogin(string email, string password)
        {
            CssClicker(By.ClassName("login"));

            driver.FindElement(By.Id("email")).SendKeys(email);
            driver.FindElement(By.Id("passwd")).SendKeys(password);

            CssClicker(By.Id("SubmitLogin"));

            IWebElement accountInfo = driver.FindElement(By.CssSelector(".header_user_info span"));
            Assert.AreEqual("Owshra Tutanaityte", accountInfo.Text, "Account info is correct");
        }

        private void TestSearch(string query)
        {
            driver.FindElement(By.Id("search_query_top")).SendKeys(query);
            CssClicker(By.CssSelector("#searchbox > button"));
            int productCount = driver.FindElements(By.CssSelector("ul.product_list > li")).Count;
            Assert.Greater(productCount, 0, "The {searchItem} has been found");
        }

        private void TestBuy(string quantity, string size, string color)
        {
            // Select item.
            CssClicker(By.CssSelector("ul.product_list > li .product_img_link"));

            // Select quantity.
            driver.FindElement(By.Id("quantity_wanted")).Clear();
            driver.FindElement(By.Id("quantity_wanted")).SendKeys(quantity);

            // Select size.
            var selectElement = new SelectElement(driver.FindElement(By.Id("group_1")));
            selectElement.SelectByText(size);

            // Select color.
            driver.FindElement(By.Id("color_to_pick_list")).FindElement(By.Name(color)).Click();

            // Add to cart.
            CssClicker(By.Id("add_to_cart"));

            // Go to checkout. Waiting 5 seconds for the button to be active.
            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[title='Proceed to checkout']"))).Click();

            // Buys items in the cart, click click click.
            CssClicker(By.ClassName("standard-checkout"));
            CssClicker(By.Name("processAddress"));
            CssClicker(By.ClassName("checker")); // "I agree with everything".
            CssClicker(By.ClassName("standard-checkout"));
            CssClicker(By.ClassName("bankwire"));
            CssClicker(By.CssSelector(".cart_navigation button"));

            IWebElement orderInfo = driver.FindElement(By.CssSelector(".cheque-indent > strong"));
            Assert.AreEqual("Your order on My Store is complete.", orderInfo.Text, "The order is completed");
        }

        private void CssClicker(By selector)
        {
            driver.FindElement(selector).Click();
        }

        [TearDown] // Closes browser.
        public void CloseBrowser()
        {
            driver.Close();
        }
    }
} // To make code pretty, click ctrl+k and then ctrl+d.