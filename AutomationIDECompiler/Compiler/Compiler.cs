﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutomationIDELibrary.ReservedKeyWords;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;
using OpenQA.Selenium;

namespace AutomationIDELibrary.Compiler
{
    public class Compiler
    {
        public static List<string> Lines = new List<string>();
        public static FirefoxDriver FireFoxDriver;
        public static ChromeDriver ChromeDriver;
        public static ChromeDriverService ChromeDriverService;
        public static WebDriverWait ChromeDriverWait;
        public static WebDriverWait FireFoxDriverWait;
        private bool _dispose = true;

        private static Compiler _instance;

        public static Compiler GetInstance()
        {
            var creator = _instance;
            if (creator != null)
            {
                return creator;
            }
            return (_instance = new Compiler());
        }

        public void BuildFireFox(string webpage = null)
        {
            new Compiler().StartFireFoxDriversAsync(webpage);
        }

        public void BuildChrome(string webpage = null)
        {
            new Compiler().StartChromeDriversAsync(webpage);
        }

        public void ReadScript()
        {
            if (!Directory.Exists("Project")) return;
            foreach (var file in Directory.GetFiles("Project"))
            {
                if (!file.Contains(".AL")) continue;
                foreach (var line in File.ReadAllLines(file)) Lines.Add(line);
            }
        }

        public Task StartChromeDriversAsync(string webpage = null)
        {
            ChromeDriverService = ChromeDriverService.CreateDefaultService();
            var hideCmdIndex = Lines.IndexOf(Lines.FirstOrDefault(l => l.StartsWith("--hideCMD")));
            if (hideCmdIndex != -1 && Lines[hideCmdIndex].StartsWith("--hideCMD", StringComparison.Ordinal))
                ChromeDriverService.HideCommandPromptWindow = true;

            var options = new ChromeOptions();
            ChromeDriver = new ChromeDriver(ChromeDriverService, options);
            ChromeDriverWait = new WebDriverWait(ChromeDriver, TimeSpan.FromSeconds(15));
            BaseDriversTask(driver: ChromeDriver, webDriverWait: ChromeDriverWait, chromeOptions: options, webpage: webpage);
            return Task.CompletedTask;
        }

        public Task StartFireFoxDriversAsync(string webpage = null)
        {
            var options = new FirefoxOptions();
            FireFoxDriver = new FirefoxDriver(options);
            FireFoxDriverWait = new WebDriverWait(FireFoxDriver, TimeSpan.FromSeconds(15));
            BaseDriversTask(driver: FireFoxDriver, webDriverWait: FireFoxDriverWait, firefoxOptions: options, webpage: webpage);
            return Task.CompletedTask;
        }

        public Task BaseDriversTask(IWebDriver driver, WebDriverWait webDriverWait, FirefoxOptions firefoxOptions = null, ChromeOptions chromeOptions = null, string webpage = null)
        {
            var targetIndex = Lines.IndexOf(Lines.FirstOrDefault(l => l.StartsWith("--target")));
            if (webpage == null)
                if (targetIndex != -1 && Lines[targetIndex].StartsWith("--target", StringComparison.Ordinal)) webpage = Lines[targetIndex].Remove(0, 8);

            var headlessIndex = Lines.IndexOf(Lines.FirstOrDefault(l => l.StartsWith("--headless")));
            if (headlessIndex != -1 && Lines[headlessIndex].StartsWith("--headless", StringComparison.Ordinal))
            {
                firefoxOptions?.AddArguments("--headless");
                chromeOptions?.AddArguments("--headless");
            }

            var noDisposeIndex = Lines.IndexOf(Lines.FirstOrDefault(l => l.StartsWith("--noDispose")));
            if (noDisposeIndex != -1 && Lines[noDisposeIndex].StartsWith("--noDispose", StringComparison.Ordinal)) _dispose = false;
            driver.Navigate().GoToUrl(webpage);

            var function = new Functions();
            foreach (var line in Lines)
            {
                if (line.Contains("Click")) _ = function.ClickAsync(line, webDriverWait);
                else if (line.Contains("SendKeys")) _ = function.SendKeysAsync(line, webDriverWait);
                else if (line.Contains("Submit")) _ = function.SubmitAsync(line, webDriverWait);
                else if (line.Contains("Redirect")) _ = function.RedirectAsync(line, driver);
                else if (line.Contains("JScript")) _ = function.InjectJavaScriptAsync(line, driver);
                else if (line.Contains("P:JScript")) _ = function.InjectJavaScriptAsync(line, driver);
                else if (line.Contains("CSScript")) _ = function.RunCSScriptAsync(line);
                else if (line.Contains("P:CSScript")) _ = function.RunCSScriptAsync(line);
                else if (line.Contains("Sleep")) _ = function.SleepAsync(line);
                else if (line.Contains("Message")) _ = function.MessageBoxAsync(line);
            }

            Lines.RemoveRange(0, Lines.Count);
            if (_dispose) driver.Dispose();
            return Task.CompletedTask;
        }

        public Task ReadTextBoxLinesAsync(TextBox textBox)
        {
            foreach (var line in textBox.Lines) Lines.Add(line);
            return Task.CompletedTask;
        }
    }
}