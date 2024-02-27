using Custom.Sample.Base;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace Custom.Sample.Tests
{
    [TestClass]
    public class Sample : BaseTest
    {
        [TestMethod]
        public void Test_01_CreateMinimumAccount()
        {
            CreateTest("Tenta criar uma Conta nova, preenchendo apenas campos obrigatório");
            try
            {
                test.Info("start");
                app.Navigation.OpenApp("Hub do SAC");
                app.Navigation.OpenSubArea("Clientes", "Contas");
                app.CommandBar.ClickCommand("Criar");
                app.Entity.SetValue("name", GetRandomString(10, 30));
                app.Entity.Save();

                var objectId = app.Entity.GetObjectId();

                Assert.IsNotNull(objectId);
                Pass();
            }
            catch (Exception e)
            {
                Fail(e.Message);
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void Test_02_CreateMinimumAccountWithCustomClick()
        {
            CreateTest("Tenta criar uma Conta nova, preenchendo apenas campos obrigatório, usando xpath para clicar no 'criar'");
            try
            {
                test.Info("start");
                app.Navigation.OpenApp("Hub do SAC");
                app.Navigation.OpenSubArea("Clientes", "Contas");
                app.Custom.ClickCommand(By.XPath("//span[text()=\"Criar\"]/..//img[@title=\"Criar\"]/../../.."));
                app.Entity.SetValue("name", GetRandomString(10, 30));
                app.Entity.Save();

                var objectId = app.Entity.GetObjectId();

                Assert.IsNotNull(objectId);
                Pass();
            }
            catch (Exception e)
            {
                Fail(e.Message);
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void Test_03_ValidadeAccountRequiredFields()
        {
            CreateTest("Tenta criar uma Conta nova, sem preencher campos obrigatório, o que deve gerar erro");
            try
            {
                test.Info("start");
                app.Navigation.OpenApp("Hub do SAC");
                app.Navigation.OpenSubArea("Clientes", "Contas");
                app.CommandBar.ClickCommand("Criar");
                app.Entity.Save();

                var notifications = app.Entity.GetFormNotifications();

                var formAccountBlockCreate = notifications.Any(x => x.Type == FormNotificationType.Error && x.Message.Contains("Nome da Conta"));

                Assert.IsTrue(formAccountBlockCreate);
                Pass();
                client.Browser.Navigate(Microsoft.Dynamics365.UIAutomation.Browser.NavigationOperation.Reload);
            }
            catch (Exception e)
            {
                Fail(e.Message);
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void Test_04_HasContosoAccount()
        {
            CreateTest("Apenas uma validação se existe contas que respondem ao parametro de busca 'contoso");
            try
            {
                test.Info("start");
                app.Navigation.OpenApp("Hub do SAC");
                app.Navigation.OpenSubArea("Clientes", "Contas");
                app.Grid.Search("contoso");
                var hasGridItems = app.Grid.GetGridItems().Count > 0;

                Assert.IsTrue(hasGridItems);
                Pass();
            }
            catch (Exception e)
            {
                Fail(e.Message);
                Assert.Fail(e.Message);
            }
        }
    }
}
