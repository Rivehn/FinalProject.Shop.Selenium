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
        public IWebDriver driver;
        public const int DISCOUNT = 10; // 100 = 100%
        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new();
            options.AddArgument("start-maximized");
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);

        }
        [TearDown]
        public void TearDown()
        {
           //driver.Quit();
        }
    }
}