using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace FinalProjectSelenium
{
    public class Tests : Utils.TestBaseClass
    {
        [Test]
        public void Login()
        {
            //login test
            driver.Url = "https://www.edgewordstraining.co.uk/demo-site/my-account/";
            driver.FindElement(By.LinkText("Dismiss")).Click();
            IWebElement username = driver.FindElement(By.Id("username"));
            IWebElement password = driver.FindElement(By.Id("password"));
            username.SendKeys("peter.deng@nfocus.co.uk");
            password.SendKeys("Q8UGRr2K27ZW2hJ");
            driver.FindElement(By.CssSelector("button[name='login']")).Click();
            //Assert.That(driver.FindElement(By.LinkText("Log out")).Text, Is.EqualTo(driver.FindElement(By.LinkText("Log out")).Text), "logged in lul");

            //Add to Cart
            driver.FindElement(By.LinkText("Shop")).Click();
            Thread.Sleep(1000);
            IWebElement clothing = driver.FindElement(By.CssSelector("#main > ul > li.post-27.product.type-product.status-publish.has-post-thumbnail.product_cat-accessories.first.instock.sale.shipping-taxable.purchasable.product-type-simple")); //Beanie
            clothing.Click();//beanie clothing page
            driver.FindElement(By.CssSelector("button[name='add-to-cart']")).Click();//one **** please
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(drv => drv.FindElement(By.LinkText("Cart")).Displayed);
            driver.FindElement(By.LinkText("Cart")).Click(); //There are 3*(4 now) ways to access the cart, this is through the navigation menu
            //Assert.That(driver.FindElement(By.CssSelector("#post-5 > header > h1")).Text, Is.EqualTo(driver.FindElement(By.CssSelector("#post-5 > header > h1")).Text), "This is the Cart page defo"); //To check if on the right page

            //Check for non-zero total
            //IWebElement total = driver.FindElement(By.CssSelector("#post-5 > div > div > form > table > tbody > tr.woocommerce-cart-form__cart-item.cart_item > td.product-subtotal > span"));
            //string value = total.Text.Substring(1, (total.Text.Length - 1));
            //if (value == "0.00")
            //{
            //    Assert.Fail("No item present");
            //}
            //else
            //{
            //    Assert.Pass(value + " was the total.");
            //}

            //Apply a discount coupon
            Thread.Sleep(2000);
            IWebElement couponField = driver.FindElement(By.Id("coupon_code"));
            couponField.SendKeys("edgewords");
            driver.FindElement(By.CssSelector("button[name='apply_coupon']")).Click();
            //Assert if coupon was applied or not HERE
            //TODO:SPLIT THE TEST CASES, ADD THE POM, MOVE BLOAT CODE INTO POM, ADD EXP WAIT TO HELPER

            //Check that discount is correct
            //Assert.Pass("End of Test Case");
        }

        [Test, Ignore("Ignored")]
        public void AddToCart()
        {

        }
    }
}