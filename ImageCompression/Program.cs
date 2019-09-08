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
            int[,,] NewImageData = ImageManipulator.DropResolution(ImageData, 0.3f, 0.3f);



            //BinaryCompressed B = BinaryCompression.Compress(NewImageData, 1, 0);
            //ImageDigester.SaveImage("./NewImage.png", BinaryDecompression.ToIntImage(B));
            //ImageDigester.SaveBinary("./NewImage.bin", B);
        }
    }
}
