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

        //Tenta criar uma Conta nova, preenchendo apenas campos obrigatório
        [TestMethod]
        public void Test_CreateMinimumAccount()
        {
            try
            {
                app.Navigation.OpenApp("Hub do SAC");
                app.Navigation.OpenSubArea("Clientes", "Contas");
                app.CommandBar.ClickCommand("Criar");
                app.Entity.SetValue("name", GetRandomString(10, 30));
                app.Entity.Save();

                var objectId = app.Entity.GetObjectId();

                Assert.IsNotNull(objectId);
                TakePrint(nameof(Sample), nameof(Test_CreateMinimumAccount), objectId != null);
            }
            catch (Exception)
            {
                TakePrint(nameof(Sample), nameof(Test_CreateMinimumAccount), false);
            }
        }

        [TestMethod]
        public void Test_CreateMinimumAccountWithCustomClick()
        {
            try
            {
                app.Navigation.OpenApp("Hub do SAC");
                app.Navigation.OpenSubArea("Clientes", "Contas");
                app.Custom.ClickCommand(By.XPath("//span[text()=\"Criar\"]/..//img[@title=\"Criar\"]/../../.."));
                app.Entity.SetValue("name", GetRandomString(10, 30));
                app.Entity.Save();

                var objectId = app.Entity.GetObjectId();

                Assert.IsNotNull(objectId);
                TakePrint(nameof(Sample), nameof(Test_CreateMinimumAccount), objectId != null);
            }
            catch (Exception)
            {
                TakePrint(nameof(Sample), nameof(Test_CreateMinimumAccount), false);
            }
        }

        //Tenta criar uma Conta nova, sem preencher campos obrigatório, o que deve gerar erro
        [TestMethod]
        public void Test_ValidadeAccountRequiredFields()
        {
            try
            {
                app.Navigation.OpenApp("Hub do SAC");
                app.Navigation.OpenSubArea("Clientes", "Contas");
                app.CommandBar.ClickCommand("Criar");
                app.Entity.Save();

                var notifications = app.Entity.GetFormNotifications();

                var formAccountBlockCreate = notifications.Any(x => x.Type == FormNotificationType.Error && x.Message.Contains("Nome da Conta"));
                Assert.IsTrue(formAccountBlockCreate);

                TakePrint(nameof(Sample), nameof(Test_ValidadeAccountRequiredFields), formAccountBlockCreate);
            }
            catch (Exception)
            {
                TakePrint(nameof(Sample), nameof(Test_ValidadeAccountRequiredFields), false);
            }
        }

        //Apenas uma validação se existe contas que respondem ao parametro de busca 'contoso'
        [TestMethod]
        public void Test_HasContosoAccount()
        {
            try
            {
                app.Navigation.OpenApp("Hub do SAC");
                app.Navigation.OpenSubArea("Clientes", "Contas");
                app.Grid.Search("contoso");
                var hasGridItems = app.Grid.GetGridItems().Count > 0;

                Assert.IsTrue(hasGridItems);
                TakePrint(nameof(Sample), nameof(Test_HasContosoAccount), hasGridItems);
            }
            catch (Exception)
            {
                TakePrint(nameof(Sample), nameof(Test_HasContosoAccount), false);
            }
        }
    }
}
