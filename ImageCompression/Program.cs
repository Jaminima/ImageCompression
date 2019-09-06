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
            BinaryCompressed B = BinaryCompression.Compress(ImageData,10,0);
            //int[,,] NewImageData = ImageManipulator.DropResolution(ImageData, 0.1f, 0.1f);
            ImageDigester.SaveImage("./NewImage.bin", B);
        }
    }
}
