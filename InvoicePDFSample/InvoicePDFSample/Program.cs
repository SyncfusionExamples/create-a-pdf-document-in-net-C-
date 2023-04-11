﻿using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;

namespace InvoicePDFSample {
    internal class Program {
        static void Main(string[] args) 
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBPh8sVXJ0S0J+XE9Hd1RDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3xSdEVkWHhfcnVTRGNbWQ==");

            PdfDocument pdfDocument = new PdfDocument();
            PdfPage currentPage = pdfDocument.Pages.Add();
            SizeF clientSize = currentPage.GetClientSize();
            FileStream imageStream = new FileStream("../../../Data/icon.png", FileMode.Open, FileAccess.Read);
            PdfImage icon= new PdfBitmap(imageStream);
            SizeF iconSize = new SizeF(40, 40);
            PointF iconLocation = new PointF(14, 13);
            PdfGraphics graphics= currentPage.Graphics;
            graphics.DrawImage(icon,iconLocation,iconSize);
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20, PdfFontStyle.Bold);
            var text = new PdfTextElement("INVOICE", font, new PdfSolidBrush(Color.FromArgb(1, 53, 67, 168)));
            text.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            PdfLayoutResult result = text.Draw(currentPage, new PointF(clientSize.Width - 25, iconLocation.Y + 10));

            font = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
            text = new PdfTextElement("To ", font);
            result = text.Draw(currentPage, new PointF(14, result.Bounds.Bottom + 30));
            font = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold);
            text = new PdfTextElement("John Smith,", font);
            result = text.Draw(currentPage, new PointF(14, result.Bounds.Bottom + 3));
            font = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
            text = new PdfTextElement(string.Format("{0}, {1}", "398 W Broadway, Evanston Ave Fargo,", "\nNorth Dakota, 10012"), font);
            result = text.Draw(currentPage, new PointF(14, result.Bounds.Bottom + 3));

            font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
            text = new PdfTextElement("Invoice No.#23698720 ", font);
            text.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            text.Draw(currentPage, new PointF(clientSize.Width - 25, result.Bounds.Y - 20));

            PdfGrid grid=new PdfGrid();
            font= new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Regular);
            grid.Style.Font = font;
            grid.Columns.Add(4);
            grid.Columns[1].Width = 70;
            grid.Columns[2].Width = 70;
            grid.Columns[3].Width = 70;

            grid.Headers.Add(1);
            PdfStringFormat stringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
            PdfGridRow header = grid.Headers[0];

            header.Cells[0].Value = "Item & description";
            header.Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            header.Cells[1].Value = "Hrs/Qty";
            header.Cells[1].StringFormat = stringFormat;
            header.Cells[2].Value = "Rate($)";
            header.Cells[2].StringFormat = stringFormat;
            header.Cells[3].Value = "Amount($)";
            header.Cells[3].StringFormat = stringFormat;

            PdfGridRow row = grid.Rows.Add();
            row.Cells[0].Value = "API Development";
            row.Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

            row.Cells[1].Value = $"{25}";
            row.Cells[1].StringFormat = stringFormat;

            row.Cells[2].Value = $"{24.46}";
            row.Cells[2].StringFormat = stringFormat;

            decimal amount=(decimal)(25*24.46);
            row.Cells[3].Value = String.Format("{0:0.##}", amount);
            row.Cells[3].StringFormat = stringFormat;

            decimal sum = 0;
            sum += amount;

            row = grid.Rows.Add();
            row.Cells[0].Value = "Desktop Software Development";
            row.Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            row.Cells[1].Value = $"{25}";
            row.Cells[1].StringFormat = stringFormat;
            row.Cells[2].Value = $"{47.83}";
            row.Cells[2].StringFormat = stringFormat;
            amount = (decimal)(25 * 47.83);
            row.Cells[3].Value = String.Format("{0:0.##}", amount);
            row.Cells[3].StringFormat = stringFormat;

            sum += amount;

            row = grid.Rows.Add();
            row.Cells[0].Value = "Site admin development";
            row.Cells[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            row.Cells[1].Value = $"{33}";
            row.Cells[1].StringFormat = stringFormat;
            row.Cells[2].Value = $"{85.1}";
            row.Cells[2].StringFormat = stringFormat;
            amount = (decimal)(33 * 85.1);
            row.Cells[3].Value = String.Format("{0:0.##}", amount);
            row.Cells[3].StringFormat = stringFormat;

            sum += amount;

            grid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent5);
            PdfGridStyle gridStyle = new PdfGridStyle();
            gridStyle.CellPadding=new PdfPaddings(5, 5, 5, 5);
            grid.Style = gridStyle;

            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            layoutFormat.Layout = PdfLayoutType.Paginate;
            result = grid.Draw(currentPage, 14, result.Bounds.Bottom + 30, clientSize.Width - 35, layoutFormat);

            currentPage.Graphics.DrawRectangle(new PdfSolidBrush(Color.FromArgb(255, 239, 242, 255)),
               new RectangleF(result.Bounds.Right - 100, result.Bounds.Bottom + 20, 100, 20));

            PdfTextElement element = new PdfTextElement("Total", font);
            element.Draw(currentPage, new RectangleF(result.Bounds.Right - 100, result.Bounds.Bottom + 22, result.Bounds.Width, result.Bounds.Height));

            var totalPrice = $"$ {Math.Round(sum, 2)}";
            element = new PdfTextElement(totalPrice, font);
            element.StringFormat = new PdfStringFormat(PdfTextAlignment.Right);
            element.Draw(currentPage, new RectangleF(15, result.Bounds.Bottom + 22, result.Bounds.Width, result.Bounds.Height));

            MemoryStream stream = new MemoryStream();
            pdfDocument.Save(stream);
            pdfDocument.Close(true);
            stream.Position = 0;
            File.WriteAllBytes("Output.pdf",stream.ToArray());
        }
    }
}