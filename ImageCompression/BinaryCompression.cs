using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCompression
{
    public static class BinaryCompression
    {
        public static BinaryCompressed Compress(int[,,] ImageData, int MaxPathLength=10, int ApproximationVariance=20)
        {
            int xSize = ImageData.GetLength(0), ySize = ImageData.GetLength(1);
            BinaryCompressed Result = new BinaryCompressed();
            Result.BaseColor = ImageManipulator.GetAverageOfImage(ImageData);
            Result.PixelTreePaths = new List<bool>[xSize, ySize, 3];
            for (int x = 0, y = 0; x < xSize && y < ySize; x++)
            {
                Result.PixelTreePaths[x, y, 0] = GetColorPaths(ImageData[x,y,0], Result.BaseColor[0], MaxPathLength, ApproximationVariance);
                Result.PixelTreePaths[x, y, 1] = GetColorPaths(ImageData[x, y, 1], Result.BaseColor[1], MaxPathLength, ApproximationVariance);
                Result.PixelTreePaths[x, y, 2] = GetColorPaths(ImageData[x, y, 2], Result.BaseColor[2], MaxPathLength, ApproximationVariance);
                if (x + 1 == xSize) { x = -1; y++; }
            }
            return Result;
        }

        static List<bool> GetColorPaths(int ActualColor,int AverageColor, int MaxPathLength, int ApproximationVariance)
        {
            List<bool> Path = new List<bool> { };
            int PathColor = AverageColor;
            for (int Paths = 0; Paths < MaxPathLength; Paths++)
            {
                int PathPower = (int)(Paths+2)/*Math.Pow(2, Paths + 1)*/;
                if (IsWithinVariance(ActualColor, AverageColor, ApproximationVariance)) { return Path; }
                if (ActualColor > AverageColor) { Path.Add(true); PathColor = PathColor*(1+(1/PathPower)); }
                else { Path.Add(false); PathColor = PathColor * (1 / PathPower); }
            }
            return Path;
        }

        static bool IsWithinVariance(int ActualColor,int EquivilantColor,int ApproximationVariance)
        {
            return Math.Abs(ActualColor - EquivilantColor) <= ApproximationVariance;
        }
    }

    public static class BinaryDecompression
    {
        public static int[,,] ToIntImage(BinaryCompressed Compressed)
        {
            int xSize = Compressed.PixelTreePaths.GetLength(0), ySize = Compressed.PixelTreePaths.GetLength(1);
            int[,,] ImageData = new int[xSize, ySize, 3];
            for (int x = 0, y = 0; x < xSize && y < ySize; x++)
            {
                ImageData[x, y, 0] = PathToValue(Compressed.BaseColor[0], Compressed.PixelTreePaths[x, y, 0]);
                ImageData[x, y, 1] = PathToValue(Compressed.BaseColor[1], Compressed.PixelTreePaths[x, y, 1]);
                ImageData[x, y, 2] = PathToValue(Compressed.BaseColor[2], Compressed.PixelTreePaths[x, y, 2]);
                if (x + 1 == xSize) { x = -1; y++; }
            }
            return ImageData;
        }

        static int PathToValue(int AverageColor, List<bool> Path)
        {
            for (int i=0;i<Path.Count;i++)
            {
                int PathPower = (int)(i+2)/*Math.Pow(2, i + 1)*/;
                if (Path[i]) { AverageColor = AverageColor * (1 + (1 / PathPower)); }
                else { AverageColor = AverageColor * (1 / PathPower); }
            }
            return AverageColor;
        }
    }

    public struct BinaryCompressed
    {
        public int[] BaseColor;
        public List<bool>[,,] PixelTreePaths;
    }
}
