using Custom.Sample.Base;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace Custom.Sample.Tests
{
    [TestClass]
    public class DesafiosZUP : BaseTest
    {
        private static string title;
        public string dateTimeNowToReport;

        [TestMethod]
        public void Test_01_ValidarCriacaoNovaOcorrencia()
        {
            CreateTest("Validar Criação de uma nova ocorrencia com Sucesso");

            try
            {
                app.Navigation.OpenApp("Zup - Treinamento");
                app.Navigation.OpenSubArea("Clientes", "Contas");

                app.Grid.Search("Desafio Zup");
                app.Grid.OpenRecord(0);
                app.Entity.SelectTab("Ocorrências");
                app.Entity.SubGrid.ClickCommand("Ocorrencias", "Criar Ocorrência");
                title = GetRandomString(10, 30);
                app.QuickCreate.SetValue("title", title);

                Info("Titúlo preenchido");

                var consulta = new LookupItem();
                consulta.Name = "zup_campo_consulta";
                consulta.Value = "Reclamação";
                app.QuickCreate.SetValue(consulta);

                Info("Campos obrigatórios preenchidos");

                app.QuickCreate.Save();
                app.Grid.Search(title);
                app.Grid.OpenRecord(0);

                var objectId = app.Entity.GetValue("ticketnumber");

                Assert.IsNotNull(objectId);
                Pass("Sucesso");
            }
            catch (Exception ex)
            {
                Fail(ex.Message);
                Assert.Fail("Teste falhou devido a uma exceção: " + ex.Message);
            }
        }

        [TestMethod]
        public void Test_02_ValidarTentativaCriacaoOcorrenciaComValorInteiroNegativo()
        {
            CreateTest("Validar tentativa de edição de uma ocorrencia passando valor inteiro negativo");

            try
            {
                if (string.IsNullOrEmpty(title))
                    title = "yegGNm7BMaZahSoN4CY1BXif";

                app.Navigation.OpenApp("Zup - Treinamento");
                app.Navigation.OpenSubArea("Ocorrências");

                app.Grid.Search(title);
                app.Grid.OpenRecord(0);

                app.Entity.SetValue("zup_campo_data", DateTime.Now);
                app.Entity.SetValue("zup_campo_decimal", "100.00");
                app.Entity.SetValue("zup_campo_inteiro", "-100");

                app.CommandBar.ClickCommand("Salvar");

                var lista = app.Entity.GetFormNotifications();
                var achoErro = lista.Any(x => x.Message.Contains("Campo Inteiro : Insira um número entre 0 e 100."));

                Assert.IsTrue(achoErro);
                Pass("Sucesso");

                client.Browser.Navigate(Microsoft.Dynamics365.UIAutomation.Browser.NavigationOperation.Reload);
            }
            catch (Exception ex)
            {
                Fail(ex.Message);
                Assert.Fail("Teste falhou devido a uma exceção: " + ex.Message);
            }
        }

        [TestMethod]
        public void Test_03_ValidarCriacaoOcorrenciaComCampoOpcaoSim()
        {
            CreateTest("Validar edição de uma ocorrencia passando o opcao SIM no campo Primeira Resposta Enviada");

            try
            {
                if (string.IsNullOrEmpty(title))
                    title = "NyXAEamvAIoZIAM1Ng";

                app.Navigation.OpenApp("Zup - Treinamento");
                app.Navigation.OpenSubArea("Ocorrências");

                app.Grid.Search(title);
                app.Grid.OpenRecord(0);

                var date = DateTime.Now;
                Random random = new Random();

                double randomNumberDec = Math.Floor(random.NextDouble() * 10);
                string campoDec = randomNumberDec.ToString();

                int randomNumberInt = random.Next(0, 101);
                string campoInt = randomNumberInt.ToString();

                app.Entity.SetValue("zup_campo_data", date);
                app.Entity.SetValue("zup_campo_decimal", "10,00");
                app.Entity.SetValue("zup_campo_inteiro", campoInt);
                app.Entity.SetValue(new OptionSet() { Name = "zup_campo_opcao", Value = "Sim" });
                app.CommandBar.ClickCommand("Salvar");

                client.Browser.Navigate(Microsoft.Dynamics365.UIAutomation.Browser.NavigationOperation.Reload);
                var date1 = app.Entity.GetValue(new DateTimeControl("zup_campo_data"));
                var nDecimal = app.Entity.GetValue("zup_campo_decimal");
                var nInteiro = app.Entity.GetValue("zup_campo_inteiro");
                var opcao = app.Entity.GetValue(new OptionSet() { Name = "zup_campo_opcao" });

                Assert.IsTrue("10,00" == nDecimal);
                Assert.IsTrue(campoInt == nInteiro);
                Assert.IsTrue(opcao == "Sim");
                Assert.IsTrue(date.ToString("yyyy/MM/dd HH:mm") == date1.Value.ToString("yyyy/MM/dd HH:mm"));
                Pass("Sucesso");
            }
            catch (Exception ex)
            {
                Fail(ex.Message);
                Assert.Fail("Teste falhou devido a uma exceção: " + ex.Message);
            }
        }

        [TestMethod]
        public void Test_04_ValidarCampoTimerOcorrencia()
        {
            CreateTest("Validar que o campo primeira resposta é um timer");

            try
            {
                if (string.IsNullOrEmpty(title))
                    title = "NyXAEamvAIoZIAM1Ng";

                app.Navigation.OpenApp("Zup - Treinamento");
                app.Navigation.OpenSubArea("Ocorrências");

                app.Grid.Search(title);
                app.Grid.OpenRecord(0);
                app.Entity.SelectTab("Detalhes do SLA Avançado");
                string msgAtual = app.Custom.FindElement(By.XPath("//label[contains(@data-id, 'First_Response_In.fieldControl')]")).Text;
                Assert.IsTrue(msgAtual.StartsWith("23h"));

                Pass("Sucesso");
            }
            catch (Exception ex)
            {
                Fail(ex.Message);
                Assert.Fail("Teste falhou devido a uma exceção: " + ex.Message);
            }
        }

        [TestMethod]
        public void Test_05_ValidarCampoPrimeiraRespostaComExito()
        {
            CreateTest("Validar Campo Primeira Resposta 'Com exito'");

            try
            {
                if (string.IsNullOrEmpty(title))
                    title = "Teste cenario 05";

                app.Navigation.OpenApp("Zup - Treinamento");

                app.Navigation.OpenSubArea("Ocorrências");
                app.Grid.Search(title);
                app.Grid.OpenRecord(0);
                app.Entity.SetValue(new OptionSet() { Name = "firstresponsesent", Value = "Sim" });
                app.CommandBar.ClickCommand("Salvar");

                app.Entity.SelectTab("Detalhes do SLA Avançado");
                string msgAtual = app.Custom.FindElement(By.XPath("//label[contains(@data-id, 'First_Response_In.fieldControl')]")).Text;

                Assert.IsTrue(msgAtual == "Com êxito");

                Pass("Sucesso");
            }
            catch (Exception ex)
            {
                Fail(ex.Message);
                Assert.Fail("Teste falhou devido a uma exceção: " + ex.Message);
            }
        }

        [TestMethod]
        public void Test_06_ValidarStatusOcorrenciaCancelada()
        {
            CreateTest("Validar cancelamento de uma ocorrencia existente");

            try
            {
                if (string.IsNullOrEmpty(title))
                    title = "TaDcEn1G5I6RJXMP";

                app.Navigation.OpenApp("Zup - Treinamento");
                app.Navigation.OpenSubArea("Ocorrências");

                app.Grid.Search(title);
                app.Grid.OpenRecord(0);
                app.CommandBar.ClickCommand("Cancelar Ocorrência");

                app.Dialogs.PublishDialog(true);

                var statusCancelado = app.Entity.GetHeaderValue(new OptionSet() { Name = "statuscode" });

                Assert.IsTrue(statusCancelado == "Cancelado");
                Pass("Sucesso");
            }
            catch (Exception ex)
            {
                Fail(ex.Message);
                Assert.Fail("Teste falhou devido a uma exceção: " + ex.Message);
            }
        }
    }
}