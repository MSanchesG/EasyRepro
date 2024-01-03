// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Configuration;

namespace Custom.Sample.Base
{
    public static class TestSettings
    {
        private static readonly string Type = ConfigurationManager.AppSettings["BrowserType"];

        public static BrowserOptions SharedOptions = new BrowserOptions
        {
            BrowserType = (BrowserType)Enum.Parse(typeof(BrowserType), Type),
        };

        public static BrowserOptions Options => new BrowserOptions
        {
            BrowserType = SharedOptions.BrowserType,
            PrivateMode = SharedOptions.PrivateMode,
            FireEvents = SharedOptions.FireEvents,
            Headless = SharedOptions.Headless,
            Kiosk = SharedOptions.Kiosk,
            UserAgent = SharedOptions.UserAgent,
            DefaultThinkTime = SharedOptions.DefaultThinkTime,
            RemoteBrowserType = SharedOptions.RemoteBrowserType,
            RemoteHubServer = SharedOptions.RemoteHubServer,
            UCITestMode = SharedOptions.UCITestMode,
            UCIPerformanceMode = SharedOptions.UCIPerformanceMode,
            DriversPath = SharedOptions.DriversPath,
            DisableExtensions = SharedOptions.DisableExtensions,
            DisableFeatures = SharedOptions.DisableFeatures,
            DisablePopupBlocking = SharedOptions.DisablePopupBlocking,
            DisableSettingsWindow = SharedOptions.DisableSettingsWindow,
            EnableJavascript = SharedOptions.EnableJavascript,
            NoSandbox = SharedOptions.NoSandbox,
            DisableGpu = SharedOptions.DisableGpu,
            DumpDom = SharedOptions.DumpDom,
            EnableAutomation = SharedOptions.EnableAutomation,
            DisableImplSidePainting = SharedOptions.DisableImplSidePainting,
            DisableDevShmUsage = SharedOptions.DisableDevShmUsage,
            DisableInfoBars = SharedOptions.DisableInfoBars,
            TestTypeBrowser = SharedOptions.TestTypeBrowser
        };
    }
}
