using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace ImageCompression
{
    public static class ImageDigester
    {
        public static int[,,] LoadImage(string FilePath)
        {
            Bitmap ImageRaw = (Bitmap)Bitmap.FromFile(FilePath);
            int[,,] ImageData = new int[ImageRaw.Width,ImageRaw.Height,3];
            for (int x = 0, y = 0; x < ImageRaw.Width && y < ImageRaw.Height; x++)
            {
                Color C = ImageRaw.GetPixel(x, y);
                ImageData[x, y, 0] = C.R;
                ImageData[x, y, 1] = C.G;
                ImageData[x, y, 2] = C.B;
                if (x + 1 == ImageRaw.Width) { x = -1; y++; }
            }
            return ImageData;
        }

        public static void SaveImage(string FilePath, int[,,] ImageData)
        {
            Bitmap ImageRaw = new Bitmap(ImageData.GetLength(0), ImageData.GetLength(1));
            for (int x = 0, y = 0; x < ImageRaw.Width && y < ImageRaw.Height; x++)
            {
                Color C = Color.FromArgb(ImageData[x, y, 0], ImageData[x, y, 1], ImageData[x, y, 2]);
                ImageRaw.SetPixel(x, y, C);
                if (x + 1 == ImageRaw.Width) { x = -1; y++; }
            }
            ImageRaw.Save(FilePath);
        }

        public static int[,,] DropResolution(int[,,] OriginalImageData,float WidthMultiplyer=1,float HeightMultiplyer=1)
        {
            int[,,] ImageData = new int[(int)Math.Round(OriginalImageData.GetLength(0)*WidthMultiplyer), (int)Math.Round(OriginalImageData.GetLength(1) * HeightMultiplyer), 3];

            int WidthRatio = (int)Math.Round(1/WidthMultiplyer), HeightRatio = (int)Math.Round(1/HeightMultiplyer), xEquivOffset=0, yEquivOffset=0;

            if (WidthRatio >= 3) { xEquivOffset = -(int)Math.Round((double)(WidthRatio - 0.01f) / 2); }
            if (HeightRatio >= 3) { yEquivOffset = -(int)Math.Round((double)(HeightRatio - 0.01f) / 2); }

            for (int x = 0, y = 0; x < ImageData.GetLength(0) && y < ImageData.GetLength(1); x++)
            {
                int xEquiv = (int)Math.Round(x / WidthMultiplyer)-xEquivOffset, yEquiv = (int)Math.Round(y / HeightMultiplyer)-yEquivOffset,PixelCount =0;
                int[] AverageColor = new int[3];
                for (int xn = xEquiv, yn = yEquiv; xn < xEquiv + WidthRatio&& yn < yEquiv + HeightRatio; xn++)
                {
                    if (xn >= 0 && yn >= 0 && xn < OriginalImageData.GetLength(0) && yn < OriginalImageData.GetLength(1))
                    {
                        AverageColor[0] += OriginalImageData[xn, yn, 0];
                        AverageColor[1] += OriginalImageData[xn, yn, 1];
                        AverageColor[2] += OriginalImageData[xn, yn, 2];
                        PixelCount++;
                    }
                    if (xn + 1 == xEquiv + WidthRatio) { xn = xEquiv; yn++; }
                }
                ImageData[x, y, 0] = (int)Math.Round((double)AverageColor[0] / PixelCount);
                ImageData[x, y, 1] = (int)Math.Round((double)AverageColor[1] / PixelCount);
                ImageData[x, y, 2] = (int)Math.Round((double)AverageColor[2] / PixelCount);
                if (x + 1 == ImageData.GetLength(0)) { x = -1; y++; }
            }

            return ImageData;
        }
    }
}
