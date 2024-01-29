using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Configuration;
using System.IO;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Custom.Sample.Base
{
    [TestClass]
    public class LoginSingleton
    {
        public LoginSingleton() { }

        public static LoginSingleton _instance;
        public static readonly object _lock = new object();

        private static Uri _xrmUri = new Uri(ConfigurationManager.AppSettings["OnlineCrmUrl"]);
        private static SecureString _username = ConfigurationManager.AppSettings["OnlineUsername"]?.ToSecureString();
        private static SecureString _password = ConfigurationManager.AppSettings["OnlinePassword"]?.ToSecureString();
        private static SecureString _mfaSecretKey = ConfigurationManager.AppSettings["MfaSecretKey"]?.ToSecureString();
        private static string directoryPath = ConfigurationManager.AppSettings["DirectoryPath"].ToString();

        public XrmApp xrmApp;
        public WebClient client;
        public string evidencePath;
        public ExtentReports report;

        public static LoginSingleton GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new LoginSingleton();

                        if (_instance.client == null)
                            _instance.client = new WebClient(TestSettings.Options);

                        if (_instance.xrmApp == null)
                            _instance.xrmApp = new XrmApp(_instance.client);

                        if (string.IsNullOrEmpty(_instance.evidencePath))
                            _instance.evidencePath = getCurrentEvidencePath();

                        if(_instance.report == null)
                        {
                            var fullPath = Path.GetFullPath($@"{_instance.evidencePath}\report.html");
                            _instance.report = new ExtentReports();
                            _instance.report.AttachReporter(new ExtentSparkReporter(fullPath));
                        }
                            
                        _instance.xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                    }
                }

            }
            return _instance;
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            _instance.xrmApp.Dispose();
            _instance.client.Dispose();
        }

        private static string getCurrentEvidencePath()
        {
            var date = DateTime.Now.Date.ToString("yyyy.MM.dd");

            int i = -1;
            string folder;
            do
            {
                i++;
                folder = directoryPath + $@"\Dynamics_Evidence\{date}_{i}\";
            } while (Directory.Exists(folder));

            return folder;
        }
    }
}
