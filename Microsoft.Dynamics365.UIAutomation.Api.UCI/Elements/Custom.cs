using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class Custom : Element
    {
        private readonly WebClient _client;

        public Custom(WebClient client)
        {
            _client = client;
        }

        public void ClickCommand(By by)
        {
            _client.ClickCommand(by);
        }

        public void DoubleClickCommand(By by)
        {
            _client.DoubleClickCommand(by);
        }

        public IWebElement FindElement(By by)
        {
            return _client.FindElement(by).Value;
        }

        public List<IWebElement> FindElements(By by)
        {
            return _client.FindElements(by).Value;
        }

        public bool IsVisible(By by)
        {
            return _client.IsVisible(by);
        }

        public bool IsEnabled(By by)
        {
            return _client.IsEnabled(by);
        }

        public bool hasElement(By by)
        {
            return _client.hasElement(by);
        }

        public int countElements(By by)
        {
            return _client.countElements(by);
        }

        public void SwitchToDefaultFrame(string frame)
        {
            _client.SwitchToFrame(frame);
        }

        public void SwitchToDefaultFrame()
        {
            _client.SwitchToDefaultFrame();
        }
    }
}
