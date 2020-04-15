using System;
using System.ComponentModel;
using System.IO;
using NFluent;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace MVCAsyncAwait.Tests
{
    public class E2ETest
    {
        [Fact]
        public void Should_add_update_and_delete_movie()
        {
            
            var driver = new ChromeDriver("selenium-drivers")
            {
                Url = "http://localhost:3500"
            };

            var linkCreateMovie = driver.FindElementByLinkText("Create movie");
            linkCreateMovie.Click();

            var movieNameTextBox = driver.FindElementById("Name");
            movieNameTextBox.SendKeys("test movie");
            
            var addButton = driver.FindElementById("AddM");
            addButton.Click();

            var updateButton = driver.FindElementByLinkText("Update movie");
            updateButton.Click();

            movieNameTextBox = driver.FindElementById("Name");
            movieNameTextBox.SendKeys(" 2");

            var saveButton = driver.FindElementById("updateM");
            saveButton.Click();

            var deleteButton = driver.FindElementByLinkText("Delete movie");
            deleteButton.Click();

            Check.That(driver.FindElementByLinkText("Create movie").Text).IsEqualTo("Create movie");
            Check.That(driver.Url).IsEqualTo("http://localhost:3500/");

            driver.Close();
        }

        

    }
}
