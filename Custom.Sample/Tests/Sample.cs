using Custom.Sample.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Custom.Sample.Tests
{
    [TestClass]
    public class Sample : BaseTest
    {
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
