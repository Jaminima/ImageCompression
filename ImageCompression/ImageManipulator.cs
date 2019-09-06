using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCompression
{
    public static class ImageManipulator
    {
        public static int[,,] DropResolution(int[,,] OriginalImageData, float WidthMultiplyer = 1, float HeightMultiplyer = 1)
        {
            int[,,] ImageData = new int[(int)Math.Round(OriginalImageData.GetLength(0) * WidthMultiplyer), (int)Math.Round(OriginalImageData.GetLength(1) * HeightMultiplyer), 3];

            int WidthRatio = (int)Math.Round(1 / WidthMultiplyer), HeightRatio = (int)Math.Round(1 / HeightMultiplyer), xEquivOffset = 0, yEquivOffset = 0;

            if (WidthRatio >= 3) { xEquivOffset = -(int)Math.Round((double)(WidthRatio - 0.01f) / 2); }
            if (HeightRatio >= 3) { yEquivOffset = -(int)Math.Round((double)(HeightRatio - 0.01f) / 2); }

            for (int x = 0, y = 0; x < ImageData.GetLength(0) && y < ImageData.GetLength(1); x++)
            {
                int xEquiv = (int)Math.Round(x / WidthMultiplyer) - xEquivOffset, yEquiv = (int)Math.Round(y / HeightMultiplyer) - yEquivOffset;
                int[] AverageColor = GetAverageOfSet(OriginalImageData, xEquiv, xEquiv + WidthRatio, yEquiv, yEquiv + HeightRatio);
                ImageData[x, y, 0] = AverageColor[0]; ImageData[x, y, 1] = AverageColor[1]; ImageData[x, y, 2] = AverageColor[2];
                if (x + 1 == ImageData.GetLength(0)) { x = -1; y++; }
            }
            return ImageData;
        }

        public static int[] GetAverageOfSet(int[,,] ImageData, int XStart,int XEnd,int YStart,int YEnd)
        {
            int[] AverageColor = new int[3];
            int PixelCount = 0;
            for (int xn = XStart, yn = YStart; xn < XEnd && yn < YEnd; xn++)
            {
                if (xn >= 0 && yn >= 0 && xn < ImageData.GetLength(0) && yn < ImageData.GetLength(1))
                {
                    AverageColor[0] += ImageData[xn, yn, 0];
                    AverageColor[1] += ImageData[xn, yn, 1];
                    AverageColor[2] += ImageData[xn, yn, 2];
                    PixelCount++;
                }
                if (xn + 1 == XEnd) { xn = XStart; yn++; }
            }
            AverageColor[0] = (int)Math.Round((double)AverageColor[0] / PixelCount);
            AverageColor[1] = (int)Math.Round((double)AverageColor[1] / PixelCount);
            AverageColor[2] = (int)Math.Round((double)AverageColor[2] / PixelCount);
            return AverageColor;
        }

        public static int[] GetAverageOfImage(int[,,] ImageData)
        {
            return GetAverageOfSet(ImageData, 0, ImageData.GetLength(0), 0, ImageData.GetLength(1));
        }
    }
}
