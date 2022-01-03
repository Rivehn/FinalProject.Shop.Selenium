using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinalProjectSelenium.Utils
{
    public static class TestHelperClass
    {
        public static void UtilThreadSleeper(int seconds) //For all your most intense hatred of AJAX needs, just add how many seconds you want to KO the stupid machine for!
        {
            Thread.Sleep(seconds*1000);
        }

        public static void UtilUltraWaiter(IWebDriver driver, int seconds, By element)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            wait.Until(drv => drv.FindElement(element).Displayed);
        }

        public static void UtilBasketClearer(IWebDriver driver) //MUST be at the basket page before calling this method
        {
            UtilThreadSleeper(5);//First wait for any leftover AJAX not accounted for.
            //Clear Coupon
            driver.FindElement(By.CssSelector(".woocommerce-remove-coupon")).Click();
            UtilThreadSleeper(5);
            //Clear Basket
            driver.FindElement(By.CssSelector(".remove")).Click(); //ONLY works with one item in basket I believe
        }
        public static void UtilKillBill(IWebDriver driver) //Extra Killer Form Filler
        {
            //first name
            driver.FindElement(By.Id("billing_first_name")).Clear();
            driver.FindElement(By.Id("billing_first_name")).SendKeys("Peter");
            //last name
            driver.FindElement(By.Id("billing_last_name")).Clear();
            driver.FindElement(By.Id("billing_last_name")).SendKeys("Deng");
            //company name
            driver.FindElement(By.Id("billing_company")).Clear();
            driver.FindElement(By.Id("billing_company")).SendKeys("nFocus");
            //street address
            driver.FindElement(By.Id("billing_address_1")).Clear();
            driver.FindElement(By.Id("billing_address_1")).SendKeys("University Rd");
            //apartment
            driver.FindElement(By.Id("billing_address_2")).Clear();
            driver.FindElement(By.Id("billing_address_2")).SendKeys("Building 36");
            //town / city
            driver.FindElement(By.Id("billing_city")).Clear();
            driver.FindElement(By.Id("billing_city")).SendKeys("Southampton ");
            //county
            driver.FindElement(By.Id("billing_state")).Clear();
            driver.FindElement(By.Id("billing_state")).SendKeys("Hampshire");
            //postcode
            driver.FindElement(By.Id("billing_postcode")).Clear();
            driver.FindElement(By.Id("billing_postcode")).SendKeys("SO17 1BJ");
            //phone
            driver.FindElement(By.Id("billing_phone")).Clear();
            driver.FindElement(By.Id("billing_phone")).SendKeys("02380592180");
            //email address
            driver.FindElement(By.Id("billing_email")).Clear();
            driver.FindElement(By.Id("billing_email")).SendKeys("peter.deng@nfocus.co.uk");
        }

        //TODO: Potentially
        public static void SuperIdolClearer() //Will (Hopefully) clear EVERYTHING
        {
        }
    }
}
