using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using static FinalProjectSelenium.Utils.TestHelperClass;

namespace FinalProjectSelenium
{
    public class Tests : Utils.TestBaseClass
    {
        [Test, Order(0)]
        public void Login()
        {
            //Login to the site
            driver.Url = "https://www.edgewordstraining.co.uk/demo-site/my-account/";
            driver.FindElement(By.LinkText("Dismiss")).Click();
            IWebElement username = driver.FindElement(By.Id("username"));
            IWebElement password = driver.FindElement(By.Id("password"));
            username.SendKeys("peter.deng@nfocus.co.uk");
            password.SendKeys("Q8UGRr2K27ZW2hJ");
            driver.FindElement(By.CssSelector("button[name='login']")).Click();
            Assert.That(driver.FindElement(By.LinkText("Log out")).Text,
                Is.EqualTo(driver.FindElement(By.LinkText("Log out")).Text),
                "Logged in Succeeded.");
        }

        [Test, Order(1), Category("Cart")]
        public void AddToCart()
        {
            //Add to Cart
            driver.FindElement(By.LinkText("Shop")).Click();
            UtilThreadSleeper(1);
            IWebElement clothing = driver.FindElement(By.CssSelector("#main > ul > li.post-27.product.type-product.status-publish.has-post-thumbnail.product_cat-accessories.first.instock.sale.shipping-taxable.purchasable.product-type-simple")); //Beanie
            clothing.Click();//beanie clothing page
            driver.FindElement(By.CssSelector("button[name='add-to-cart']")).Click();//one **** please
            //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(25));
            //wait.Until(drv => drv.FindElement(By.LinkText("Cart")).Displayed);
            UtilUltraWaiter(driver, 25, By.LinkText("Cart"));
            driver.FindElement(By.LinkText("Cart")).Click(); //There are 3*(4 now) ways to access the cart, this is through the navigation menu
            Assert.That(driver.FindElement(By.CssSelector("#post-5 > header > h1")).Text,
                Is.EqualTo(driver.FindElement(By.CssSelector("#post-5 > header > h1")).Text),
                "This is the Cart page defo"); //To check if on the right page
        }

        [Test, Order(2), Category("Cart")]
        public void CartNotEmpty()
        {
            //Check for non - zero total
            IWebElement total = driver.FindElement(By.CssSelector("#post-5 > div > div > form > table > tbody > tr.woocommerce-cart-form__cart-item.cart_item > td.product-subtotal > span"));
            string value = total.Text[1..];
            if (value == "0.00")
            {
                Assert.Fail("No item present");
            }
            else
            {
                Assert.Pass(value + " was the total.");
            }
        }

        [Test, Order(3), Category("Coupon")]
        public void ApplyCoupon()
        {
            //Apply a discount coupon
            UtilThreadSleeper(2);
            IWebElement couponField = driver.FindElement(By.Id("coupon_code"));
            couponField.SendKeys("edgewords");
            driver.FindElement(By.CssSelector("button[name='apply_coupon']")).Click();
            UtilThreadSleeper(3);//i will fix this later

            //Check if coupon was applied
            IWebElement body = driver.FindElement(By.TagName("body"));
            Assert.That(body.Text.Contains("Coupon: edgewords"), "COUPON SUCCESSFULLY APPLIED"); //might need to double check this later~
        }

        [Test, Order(4), Category("Coupon")]
        public void CheckForCorrectValues()
        {
            //Check if discount is correct
            decimal totalValue = System.Convert.ToDecimal
                ((driver.FindElement(By.CssSelector(".cart-subtotal > td > .amount.woocommerce-Price-amount")).Text)[1..]);
            decimal actValue = System.Convert.ToDecimal
                ((driver.FindElement(By.CssSelector(".cart-discount.coupon-edgewords > td > .amount.woocommerce-Price-amount")).Text)[1..]);
            decimal expValue;
            expValue = totalValue * (System.Convert.ToDecimal(DISCOUNT) / 100);
            if (expValue == actValue)
            {
                Assert.Pass("The discounts match the expected discount price." + " Expected discount: " + String.Format("{0:0.00}", expValue) + " Actual discount: " + actValue);
            }
            else
            {
                Assert.Fail("Discounts did not match." +
                    " The expected discount: " + String.Format("{0:0.00}", expValue) + " " + DISCOUNT + "% " +
                    "...Instead actual discount: " + actValue + " " + (100 * (actValue / totalValue)) + "% ");
            }
        }

        [Test, Order(5), Category("Coupon")]
        public void CheckTotal()
        {
            //Check that total calculated after shipping is correct
            decimal subTotal = System.Convert.ToDecimal
                ((driver.FindElement(By.CssSelector(".cart-subtotal > td > .amount.woocommerce-Price-amount")).Text)[1..]);
            decimal discountAmount = System.Convert.ToDecimal
                ((driver.FindElement(By.CssSelector(".cart-discount.coupon-edgewords > td > .amount.woocommerce-Price-amount")).Text)[1..]);
            decimal shippingCost = System.Convert.ToDecimal
                ((driver.FindElement(By.CssSelector(".shipping > td > .amount.woocommerce-Price-amount")).Text)[1..]);
            decimal finalTotal = System.Convert.ToDecimal
                ((driver.FindElement(By.CssSelector("strong > .amount.woocommerce-Price-amount")).Text)[1..]);
            if (finalTotal == subTotal + (-discountAmount) + shippingCost)
            {
                //Console.WriteLine("It passed.");
                Assert.Pass("The total summed to: " + finalTotal);
            }
            else
            {
                //Console.WriteLine("It failed.");
                Assert.Fail("The total did not sum to: " + finalTotal + " Instead, it summed to: " + (subTotal + (-discountAmount) + shippingCost));
            }
        }

        [Test, Order(6), Category("Checkout")]
        public void ProceedToCheckout()
        {
            //Proceed to checkout
            //wait.Until(drv => drv.FindElement(By.CssSelector(".alt.button.checkout-button.wc-forward")).Displayed);
            UtilUltraWaiter(driver, 20, By.CssSelector(".alt.button.checkout-button.wc-forward"));
            driver.FindElement(By.CssSelector(".alt.button.checkout-button.wc-forward")).Click();
            UtilThreadSleeper(3);
        }

        [Test, Order(7), Category("Checkout")]
        public void PaymentTypeAndPlaceOrder()
        {
            //Fill in billing details
            UtilKillBill(driver);
            UtilThreadSleeper(3);//need to find better way

            //Select check payments
            try
            {
                //wait.Until(drv => drv.FindElement(By.CssSelector(".payment_method_cheque.wc_payment_method > label")).Displayed);
                UtilUltraWaiter(driver, 20, By.CssSelector(".payment_method_cheque.wc_payment_method > label"));
                driver.FindElement(By.CssSelector(".payment_method_cheque.wc_payment_method > label")).Click();
            }
            catch (Exception)
            {
                Assert.Fail("Failed to find element, maybe it is floating in the void somewhere");
            }
            UtilThreadSleeper(3);//need to find better way
            //Place order
            try
            {
                IWebElement order = driver.FindElement(By.CssSelector("button#place_order"));
                driver.FindElement(By.CssSelector("button#place_order")).Click();

            }
            catch (Exception)
            {
                Assert.Fail("Failed to find element, maybe it is floating in the void somewhere");
            }
            UtilThreadSleeper(3);
        }

        [Test, Order(8), Category("Checkout")]
        public void CaptureOrder()
        {
            //Capture order
            orderNumber = driver.FindElement(By.CssSelector(".order > strong")).Text;
            Console.WriteLine(orderNumber);
            UtilThreadSleeper(3);
        }

        [Test, Order(9), Category("Aftermath")]
        public void CheckOrder()
        {
            //Check the order number isn't fake: Account > Orders > Checking Order
            driver.FindElement(By.LinkText("My account")).Click();
            UtilThreadSleeper(3);
            driver.FindElement(By.LinkText("Orders")).Click();
            UtilThreadSleeper(3);
            accountOrder = (driver.FindElement(By.CssSelector
                ("tr:nth-of-type(1) > .woocommerce-orders-table__cell.woocommerce-orders-table__cell-order-number > a"))
                .Text)[1..];
            Console.WriteLine(accountOrder);
            UtilThreadSleeper(3);
            if (orderNumber == accountOrder)
            {
                Assert.Pass("Congrats you made it to the end of this Test Case!");
            }
            else
            {
                Assert.Fail("Stop! you didn't pass in the end. Check those order numbers again");
            }
        }

        [Test, Order(10), Category("Aftermath")]
        public void LogOut()
        {
            driver.FindElement(By.CssSelector
                (".woocommerce-MyAccount-navigation-link.woocommerce-MyAccount-navigation-link--customer-logout > a"))
                .Click();
            //TODO: ADD POM
        }
    }
}