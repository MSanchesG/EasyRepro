using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Configuration;
using System.IO;
using System.Security;

namespace Custom.Sample.Base
{
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

                        _instance.xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);
                    }
                }

            }
            return _instance;
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
