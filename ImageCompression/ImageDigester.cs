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

        static List<bool> LoadPath(BinaryReader Reader)
        {
            List<bool> Path = new List<bool> { };
            int PathCount = Reader.ReadInt32();
            for (int i = 0; i < PathCount; i++) { Path.Add(Reader.ReadBoolean()); }
            return Path;
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

        static void SaveBinary(BinaryWriter Writer, List<bool> Path)
        {
            Writer.Write(Path.Count);
            foreach (bool B in Path) { Writer.Write(B); }
        }
    }
}
