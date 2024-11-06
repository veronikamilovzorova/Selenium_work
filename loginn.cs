using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace Selenium_Work
{
    [TestFixture]
    public class LoginTest
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver(@"C:\Users\opilane\source\repos\Selenium_work\drivers"); 
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void TestLoginPage()
        {
            driver.Navigate().GoToUrl("http://localhost:7275/login.php");

            // Проверим, что форма входа содержит поля для логина и пароля
            var loginField = driver.FindElement(By.Name("login"));
            var passwordField = driver.FindElement(By.Name("pass"));
            Assert.That(loginField, Is.Not.Null, "Login field is missing.");
            Assert.That(passwordField, Is.Not.Null, "Password field is missing.");

            // Заполним поля и отправим форму
            loginField.SendKeys("testuser");
            passwordField.SendKeys("testpass");
            var submitButton = driver.FindElement(By.CssSelector("input[type='submit']"));
            submitButton.Click();

            // Ожидаем редирект на страницу управления (например, haldus.php)
            var pageTitle = driver.Title;
            Assert.That(pageTitle, Is.EqualTo("Haldus Page"), "Login failed, incorrect page title.");
        }

        [Test]
        public void TestRegistrationPage()
        {
            driver.Navigate().GoToUrl("http://localhost:7275/login.php");

            // Ожидаем появления кнопки регистрации
            var registerButton = driver.FindElement(By.CssSelector("button[onclick='openRegistrationPanel()']"));
            Assert.That(registerButton, Is.Not.Null, "Registration button is missing.");

            // Нажмем на кнопку регистрации
            registerButton.Click();

            // Используем явное ожидание для проверки, что форма регистрации появилась
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var registrationForm = wait.Until(driver => driver.FindElement(By.Id("registrationPanel")));

            // Проверим, что форма регистрации отображается
            Assert.That(registrationForm.Displayed, Is.True, "Registration form did not appear.");

            // Заполним форму регистрации
            var registerLoginField = driver.FindElement(By.Name("register-login"));
            var registerPassField = driver.FindElement(By.Name("register-pass"));
            registerLoginField.SendKeys("newuser");
            registerPassField.SendKeys("newpass");

            // Отправим форму
            var submitButton = driver.FindElement(By.CssSelector("input[type='submit']"));
            submitButton.Click();

            // Проверим, что регистрация прошла успешно
            var confirmationMessage = driver.FindElement(By.TagName("body"));
            Assert.That(confirmationMessage.Text, Does.Contain("Registration successful"), "Registration failed.");
        }

        [TearDown]
        public void Teardown()
        {
            driver?.Quit();
            driver?.Dispose();
        }
    }
}
