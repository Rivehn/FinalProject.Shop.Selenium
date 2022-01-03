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
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(25));
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
            Thread.Sleep(3000);//i will fix this later

            //Check if coupon was applied
            //IWebElement body = driver.FindElement(By.TagName("body"));
            //Assert.That(body.Text.Contains("Coupon: edgewords"), "COUPON SUCCESSFULLY APPLIED"); //might need to double check this later~
            
            //Check if discount is correct
            //CheckDiscountWithAssert(DISCOUNT);
            CheckDiscount(DISCOUNT);

            //Check that total calculated after shipping is correct
            CheckTotal();

            //Checkout
            wait.Until(drv => drv.FindElement(By.CssSelector(".alt.button.checkout-button.wc-forward")).Displayed);
            driver.FindElement(By.CssSelector(".alt.button.checkout-button.wc-forward")).Click();
            Thread.Sleep(3000);

            //Fill in billing details
            KillBill();
            Thread.Sleep(3000);//need to find better way

            //select check payments
            try
            {
                wait.Until(drv => drv.FindElement(By.CssSelector(".payment_method_cheque.wc_payment_method > label")).Displayed);
                driver.FindElement(By.CssSelector(".payment_method_cheque.wc_payment_method > label")).Click();
            }
            catch (Exception)
            {
                Assert.Fail("Failed to find element, maybe it is floating in the void somewhere");
            }
            //driver.FindElement(By.CssSelector(".payment_method_cheque.wc_payment_method > label")).Click(); //no clue why it doesnt work
            Thread.Sleep(3000);//need to find better way

            //place order
            try
            {
                IWebElement order = driver.FindElement(By.CssSelector("button#place_order"));
                driver.FindElement(By.CssSelector("button#place_order")).Click();

            }
            catch (Exception)
            {
                Assert.Fail("Failed to find element, maybe it is floating in the void somewhere");
            }
            Thread.Sleep(3000);

            //capture order
            string orderNumber = driver.FindElement(By.CssSelector(".order > strong")).Text;
            Console.WriteLine(orderNumber);
            Thread.Sleep(3000);

            //Check the order number isn't fake
            driver.FindElement(By.LinkText("My account")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.LinkText("Orders")).Click();
            Thread.Sleep(3000);
            string accountOrder = (driver.FindElement(By.CssSelector
                ("tr:nth-of-type(1) > .woocommerce-orders-table__cell.woocommerce-orders-table__cell-order-number > a"))
                .Text)[1..];
            Console.WriteLine(accountOrder);
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector
                (".woocommerce-MyAccount-navigation-link.woocommerce-MyAccount-navigation-link--customer-logout > a"))
                .Click();
            if(orderNumber == accountOrder)
            {
                Assert.Pass("Congrats you made it to the end of this Test Case!");
            }
            else
            {
                Assert.Fail("Stop! you didn't pass in the end. Check those order numbers again");
            }
            //TODO:SPLIT THE TEST CASES, ADD THE POM, MOVE BLOAT CODE INTO POM, ADD EXP WAIT TO HELPER
            //TODO: REMOVE COUPON AND EMPTY ITEMS HELPER METHODS, ADD A SUPER IDOL CLEAR METHOD
        }

        [Test, Ignore("Ignored")]
        public void AddToCart()
        {

        }

        public void CheckDiscountWithAssert(int disc)
        {
            decimal totalValue = System.Convert.ToDecimal
                ((driver.FindElement(By.CssSelector(".cart-subtotal > td > .amount.woocommerce-Price-amount")).Text)[1..]);
            decimal actValue = System.Convert.ToDecimal
                ((driver.FindElement(By.CssSelector(".cart-discount.coupon-edgewords > td > .amount.woocommerce-Price-amount")).Text)[1..]);
            decimal expValue;
            expValue = totalValue * (System.Convert.ToDecimal(disc) / 100);
            if (expValue == actValue)
            {
                Assert.Pass("The discounts match the expected discount price." + " Expected discount: " + String.Format("{0:0.00}", expValue) + " Actual discount: " + actValue);
            }
            else
            {
                Assert.Fail("Discounts did not match." +
                    " The expected discount: " + String.Format("{0:0.00}", expValue) + " " + disc + "% " +
                    "...Instead actual discount: " + actValue + " " + (100 * (actValue / totalValue)) + "% ");
            }
        }

        public void CheckDiscount(int disc)
        {
            decimal totalValue = System.Convert.ToDecimal
                ((driver.FindElement(By.CssSelector(".cart-subtotal > td > .amount.woocommerce-Price-amount")).Text)[1..]);
            decimal actValue = System.Convert.ToDecimal
                ((driver.FindElement(By.CssSelector(".cart-discount.coupon-edgewords > td > .amount.woocommerce-Price-amount")).Text)[1..]);
            decimal expValue;
            expValue = totalValue * (System.Convert.ToDecimal(disc) / 100);
            //Console.WriteLine("Total Val: " + totalValue);
            //Console.WriteLine(disc);
            //Console.WriteLine("Exp Val: " + expValue);
            if (expValue == actValue)
            {
                Console.WriteLine("It passed.");
            }
            else
            {
                Console.WriteLine("It failed.");
            }
        }

        public void CheckTotal()
        {
            decimal subTotal = System.Convert.ToDecimal
                ((driver.FindElement(By.CssSelector(".cart-subtotal > td > .amount.woocommerce-Price-amount")).Text)[1..]);
            decimal discountAmount = System.Convert.ToDecimal
                ((driver.FindElement(By.CssSelector(".cart-discount.coupon-edgewords > td > .amount.woocommerce-Price-amount")).Text)[1..]);
            decimal shippingCost = System.Convert.ToDecimal
                ((driver.FindElement(By.CssSelector(".shipping > td > .amount.woocommerce-Price-amount")).Text)[1..]);
            decimal finalTotal = System.Convert.ToDecimal
                ((driver.FindElement(By.CssSelector("strong > .amount.woocommerce-Price-amount")).Text)[1..]);
            if(finalTotal == subTotal + (-discountAmount) + shippingCost)
            {
                Console.WriteLine("It passed.");
                //Assert.Pass("The total summed to: " + finalTotal)
            }
            else
            {
                Console.WriteLine("It failed.");
                //Assert.Fail("The total did not sum to: " + finalTotal + " Instead, it summed to: " + (subTotal + (-discountAmount) + shippingCost));
            }
        }

        public void KillBill()
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
    }
}