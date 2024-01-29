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
        public void Test_01_CreateMinimumAccount()
        {
            CreateTest(nameof(Test_01_CreateMinimumAccount), "descrição");
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
                LogImage("Tela de ocorrência", objectId != null);
                test.Pass();
            }
            catch (Exception e)
            {
                test.Fail(e.Message);
                LogImage(e.Message, false);

                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void Test_02_CreateMinimumAccountWithCustomClick()
        {
            CreateTest(nameof(Test_02_CreateMinimumAccountWithCustomClick), "descrição 2");
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
                LogImage("Tela de ocorrência", objectId != null);
                test.Pass();
            }
            catch (Exception e)
            {
                test.Fail(e.Message);
                LogImage(e.Message, false);

                Assert.Fail(e.Message);
            }
        }

        //Tenta criar uma Conta nova, sem preencher campos obrigatório, o que deve gerar erro
        [TestMethod]
        public void Test_03_ValidadeAccountRequiredFields()
        {
            CreateTest(nameof(Test_03_ValidadeAccountRequiredFields), "descrição 3");
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
                LogImage("Notificação de erro", formAccountBlockCreate);
                test.Pass();
            }
            catch (Exception e)
            {
                test.Fail(e.Message);
                LogImage(e.Message, false);

                Assert.Fail(e.Message);
            }
        }

        //Apenas uma validação se existe contas que respondem ao parametro de busca 'contoso'
        [TestMethod]
        public void Test_04_HasContosoAccount()
        {
            CreateTest(nameof(Test_04_HasContosoAccount), "descrição 4");
            try
            {
                test.Info("start");
                app.Navigation.OpenApp("Hub do SAC");
                app.Navigation.OpenSubArea("Clientes", "Contas");
                app.Grid.Search("contoso");
                var hasGridItems = app.Grid.GetGridItems().Count > 0;

                Assert.IsTrue(hasGridItems);
                LogImage("grid de ocorrência", hasGridItems);
                test.Pass();
            }
            catch (Exception e)
            {
                test.Fail(e.Message);
                LogImage(e.Message, false);

                Assert.Fail(e.Message);
            }
        }
    }
}
