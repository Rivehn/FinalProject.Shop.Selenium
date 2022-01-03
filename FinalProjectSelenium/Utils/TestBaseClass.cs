using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectSelenium.Utils
{
    public class TestBaseClass
    {
        public IWebDriver? driver;
        public const int DISCOUNT = 15; // 100 = 100% ACTUAL DISCOUNT = 15
        public string? orderNumber; //Captured after checkout
        public string? accountOrder; //Captured in My accounts
        [OneTimeSetUp]
        public void Setup()
        {
            ChromeOptions options = new();
            options.AddArgument("start-maximized");
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
            options.AddUserProfilePreference("profile.default_content_setting_values.cookies", 2);
        }
        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}