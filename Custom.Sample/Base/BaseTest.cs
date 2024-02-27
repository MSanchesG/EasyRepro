using AventStack.ExtentReports;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Custom.Sample.Base
{
    [TestClass]
    public abstract class BaseTest
    {
        public readonly LoginSingleton singleton;
        public readonly XrmApp app;
        public readonly WebClient client;
        public readonly string evidencePath;
        private readonly ExtentReports report;
        public ExtentTest test;
        private string testName;

        public BaseTest()
        {
            singleton = LoginSingleton.GetInstance();
            app = singleton.xrmApp;
            client = singleton.client;
            evidencePath = singleton.evidencePath;
            report = singleton.report;
        }

        public void CreateTest(string descr)
        {
            testName = new StackTrace(true).GetFrame(1).GetMethod().Name;
            test = report.CreateTest(testName, descr);
        }

        private void LogImage(string message, string type)
        {
            string screenshotFilePath = TakePrint(type == "info" || type == "pass");
            screenshotFilePath = Path.GetFullPath(screenshotFilePath);
            MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotFilePath);
            if (type == "pass")
            {
                test.Pass(message, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotFilePath).Build());
                test.AddScreenCaptureFromPath(screenshotFilePath);
            }
            else if (type == "info")
                test.Info(message, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotFilePath).Build());
            else
            {
                test.Fail(message, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotFilePath).Build());
                test.AddScreenCaptureFromPath(screenshotFilePath);
            }
        }

        [TestCleanup]
        public void SaveReport()
        {
            report.Flush();
        }

        public void Info(string message = "")
        {
            LogImage(message, "info");
        }

        public void Pass(string message = "")
        {
            LogImage(message, "pass");
        }

        public void Fail(string message = "")
        {
            LogImage(message, "fail");
        }

        private string TakePrint(bool success)
        {
            Directory.CreateDirectory(evidencePath);
            string file = getNextFileName(evidencePath, $@"\{(success ? "Success" : "Error")}_{testName}", "jpg");

            singleton.client.Browser.TakeWindowScreenShot(file);
            return file;
        }

        public string getTestName()
        {
            testName = new StackTrace(true).GetFrame(1).GetMethod().Name;
            return testName;
        }

        public string GetRandomString(int minLen, int maxLen)
        {
            char[] Alphabet = ("ABCDEFGHIJKLMNOPQRSTUVWXYZabcefghijklmnopqrstuvwxyz0123456789").ToCharArray();
            Random m_randomInstance = new Random();
            object m_randLock = new object();

            int alphabetLength = Alphabet.Length;
            int stringLength;
            lock (m_randLock) { stringLength = m_randomInstance.Next(minLen, maxLen); }
            char[] str = new char[stringLength];

            // max length of the randomizer array is 5
            int randomizerLength = (stringLength > 5) ? 5 : stringLength;

            int[] rndInts = new int[randomizerLength];
            int[] rndIncrements = new int[randomizerLength];

            // Prepare a "randomizing" array
            for (int i = 0; i < randomizerLength; i++)
            {
                int rnd = m_randomInstance.Next(alphabetLength);
                rndInts[i] = rnd;
                rndIncrements[i] = rnd;
            }

            // Generate "random" string out of the alphabet used
            for (int i = 0; i < stringLength; i++)
            {
                int indexRnd = i % randomizerLength;
                int indexAlphabet = rndInts[indexRnd] % alphabetLength;
                str[i] = Alphabet[indexAlphabet];

                // Each rndInt "cycles" characters from the array, 
                // so we have more or less random string as a result
                rndInts[indexRnd] += rndIncrements[indexRnd];
            }
            return (new string(str));
        }

        private string getNextFileName(string directory, string fileName, string extension)
        {
            int i = -1;
            string file;
            do
            {
                i++;
                file = directory + $@"\{fileName}_{i}.{extension}";
            } while (File.Exists(file));

            return file;
        }
    }
}
