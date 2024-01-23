using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System;
using System.IO;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace Custom.Sample.Base
{
    internal class WordIntegration
    {
        private readonly string evidencePath;
        public WordIntegration(string evidencePath)
        {
            this.evidencePath = evidencePath;
        }

        public void LogText(string text, string feature, string testName, bool success, bool breakLine = false)
        {
            Directory.CreateDirectory(evidencePath);
            var file = $@"{evidencePath}\results.docx";
            if (!File.Exists(file))
            {
                using (var doc = WordprocessingDocument.Create(file, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = doc.AddMainDocumentPart();
                    new Document(new Body()).Save(mainPart);
                };
            }

            using (WordprocessingDocument doc = WordprocessingDocument.Open(file, true))
            {
                Body body = doc.MainDocumentPart.Document.AppendChild(new Body());
                Paragraph para = body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());
                run.AppendChild(new Text("Funcionalidade: " + feature));
                run.AppendChild(new Break());
                run.AppendChild(new Text("Teste: " + testName));
                run.AppendChild(new Break());
                run.AppendChild(new Text("Resultado"));


                run = para.AppendChild(new Run());
                RunProperties runPro = new RunProperties();
                runPro.Append(new Color() { Val = (success ? "15bd1d" : "c70606") });
                run.Append(runPro);
                run.Append(new Text(success ? ": Passou" : ": Falhou"));

                run = para.AppendChild(new Run());
                run.AppendChild(new Break());
                if (!string.IsNullOrEmpty(text))
                {
                    run.AppendChild(new Text("Mais informações: " + text));
                    run.AppendChild(new Break());
                }
                if (breakLine)
                {
                    run.AppendChild(new Break());
                    run.AppendChild(new Break());
                }
            }
        }

        public void LogImage(string fileName)
        {
            Directory.CreateDirectory(evidencePath);
            var file = $@"{evidencePath}\results.docx";
            if (!File.Exists(file))
            {
                using (var doc = WordprocessingDocument.Create(file, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = doc.AddMainDocumentPart();
                    new Document(new Body()).Save(mainPart);
                };
            }

            using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(file, true))
            {
                MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;

                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

                int iWidth = 0;
                int iHeight = 0;
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(fileName))
                {
                    iWidth = (int)Math.Round((decimal)bmp.Width * 9525 * 0.4m);
                    iHeight = (int)Math.Round((decimal)bmp.Height * 9525 * 0.4m);
                }

                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    imagePart.FeedData(stream);
                }
                AddImageToBody(wordprocessingDocument, iWidth, iHeight, mainPart.GetIdOfPart(imagePart));
                var body = mainPart.Document.AppendChild(new Body());
                Paragraph para = body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());
                run.AppendChild(new Break());
                run.AppendChild(new Break());
            }
        }

        private void AddImageToBody(WordprocessingDocument wordDoc, int iWidth, int iHeight, string relationshipId)
        {
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = iWidth, Cy = iHeight },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = iWidth, Cy = iHeight }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            // Append the reference to body, the element should be in a Run.
            var body = wordDoc.MainDocumentPart.Document.AppendChild(new Body());
            body.AppendChild(new Paragraph(new Run(element)));
        }
    }
}
