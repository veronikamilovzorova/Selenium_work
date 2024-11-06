using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace Selenium_Work
{
    [TestFixture]
    public class DancePairTest
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            // Убедитесь, что путь к ChromeDriver правильный или используйте стандартный путь
            driver = new ChromeDriver(@"C:\Users\opilane\source\repos\Selenium_work\drivers");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void TestAddDancePair()
        {
            driver.Navigate().GoToUrl("http://localhost:7275/index.php");

            // Явное ожидание для поля ввода новой пары
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var newPairInput = wait.Until(d => d.FindElement(By.XPath("//input[@id='paarinimi']")));
            Assert.That(newPairInput, Is.Not.Null, "Поле для ввода танцевальной пары отсутствует.");

            // Вводим имя новой танцевальной пары
            newPairInput.SendKeys("New Dance Pair");

            // Ищем кнопку и кликаем на неё
            var submitButton = driver.FindElement(By.CssSelector("input[type='submit']"));
            submitButton.Click();

            // Явное ожидание для таблицы с танцевальными парами
            var table = wait.Until(d => d.FindElement(By.TagName("table")));
            Assert.That(table.Text, Does.Contain("New Dance Pair"), "Новая танцевальная пара не была добавлена.");
        }

        [Test]
        public void TestIncreasePoints()
        {
            driver.Navigate().GoToUrl("http://localhost:7275/index.php");

            // Ожидаем появления кнопки для увеличения баллов
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var increaseButton = wait.Until(d => d.FindElement(By.CssSelector("a[href*='heatants']")));
            Assert.That(increaseButton, Is.Not.Null, "Кнопка для увеличения баллов отсутствует.");

            // Нажимаем кнопку для увеличения баллов
            increaseButton.Click();

            // Ожидаем появления таблицы с баллами
            var pointsCell = wait.Until(d => d.FindElement(By.XPath("//td[text()='Points']/following-sibling::td")));
            Assert.That(pointsCell.Text, Is.EqualTo("1"), "Баллы не увеличились.");
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
