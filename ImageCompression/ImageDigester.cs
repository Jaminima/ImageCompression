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

        //public static BinaryCompressed LoadBinary(string FilePath)
        //{
        //    BinaryCompressed ImageData = new BinaryCompressed();
        //    BinaryReader Reader = new BinaryReader(File.Open(FilePath, FileMode.Open));
        //    int Width = Reader.ReadInt32(), Height = Reader.ReadInt32();
        //    ImageData.PixelTreePaths = new List<bool>[Width, Height, 3];
        //    ImageData.BaseColor = new int[] { Reader.ReadInt32(), Reader.ReadInt32(), Reader.ReadInt32() };
        //    for (int x = 0, y = 0; x < Width && y < Height; x++)
        //    {

        //        if (x + 1 == Width) { x = -1; y++; }
        //    }
        //}

        //static List<bool> LoadPath()
        //{

        //}

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

        public static void SaveBinary(string FilePath, BinaryCompressed ImageData)
        {
            int Width = ImageData.PixelTreePaths.GetLength(0), Height = ImageData.PixelTreePaths.GetLength(1);
            BinaryWriter Writer = new BinaryWriter(File.Open(FilePath, FileMode.Create));
            Writer.Write(Width); Writer.Write(Height);
            Writer.Write(ImageData.BaseColor[0]); Writer.Write(ImageData.BaseColor[1]); Writer.Write(ImageData.BaseColor[2]);
            for (int x = 0, y = 0; x < Width && y < Height; x++)
            {
                SaveBinary(Writer, ImageData.PixelTreePaths[x, y, 0]);
                SaveBinary(Writer, ImageData.PixelTreePaths[x, y, 1]);
                SaveBinary(Writer, ImageData.PixelTreePaths[x, y, 2]);
                if (x + 1 == Width) { x = -1; y++; }
            }
        }

        static void SaveBinary(BinaryWriter Writer, List<bool> Path)
        {
            Writer.Write(Path.Count);
            foreach (bool B in Path) { Writer.Write(B); }
        }
    }
}
