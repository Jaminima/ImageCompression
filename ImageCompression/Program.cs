using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCompression
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,,] ImageData = ImageDigester.LoadImage("./Image.png");
            int[,,] NewImageData = ImageManipulator.DropResolution(ImageData, 1f, 1f);

            List<int[]> Colors = ImageManipulator.GetTopColors(NewImageData,12,8);
            NewImageData = ImageManipulator.UseOnlyColors(NewImageData, Colors);

            ImageDigester.SaveImage("NewImage.png", NewImageData);
        }
    }
}
