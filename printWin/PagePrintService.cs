﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace printWin
{
    class PagePrintService
    {
        private PrintDocument DocPrinter=new PrintDocument();

        private int PageNo = 0;

        private PageData PrintData;


        public PagePrintService(PageData printData)
        {
            PrintData = printData;

            //纸张大小 大于0
            if (printData.PaperHeight>0&&printData.PaperWidth>0)
            {
                DocPrinter.DefaultPageSettings.PaperSize=new PaperSize("自定义",get100InByMm(printData.PaperWidth),get100InByMm(printData.PaperHeight));
            }

           
            if (!string.IsNullOrEmpty(printData.PrintName))
            {
                DocPrinter.PrinterSettings.PrinterName = printData.PrintName;
            }

            DocPrinter.PrintPage += DocPrinter_PrintPage;

            DocPrinter.OriginAtMargins = true;
//            DocPrinter.DefaultPageSettings.Margins.Top = 100;
        }

        public void print()
        {
            DocPrinter.Print();
            
        }

        private int get100InByMm(float num)
        {
            return (int) Math.Ceiling(num / 25.4 * 100);
        }

        private void DocPrinter_PrintPage(object sender, PrintPageEventArgs e)
        {
            string pageImage = PrintData.Page[PageNo];
            //            throw new NotImplementedException();
//            float dpiX = e.Graphics.DpiX;
            Image image = CommonTool.getImageByString(pageImage);
//            image = CommonTool.ResizeUsingGDIPlus((Bitmap)image, image.Width * 10, image.Height);
            image = CommonTool.Smooth((Bitmap) image);
            Rectangle srcImg = new Rectangle(0, 0, image.Width, image.Height);
//                        var f = dpiX / image.Width *image.Height;
                        Rectangle desRec = new Rectangle(0, 0, image.Width*2, image.Height*2);
//            image = CommonTool.DealImage(image);
           
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
//            e.Graphics.DrawImage(image,new Point(0,0));
//            e.Graphics.PageUnit=GraphicsUnit.;
            e.Graphics.DrawImage(image,desRec, srcImg, GraphicsUnit.Pixel);
            PageNo++;
            if (PageNo < PrintData.Page.Count)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }

        }
    }

    class PageData
    {
        public List<String> Page { get; set; }
     

        public String PrintName { get; set; }

        public float PaperHeight { get; set; }
        public float PaperWidth { get; set; }
        

    }
}
