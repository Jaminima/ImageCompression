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
            int[,,] NewImageData = ImageDigester.DropResolution(ImageData, 0.1f, 0.1f);
            ImageDigester.SaveImage("./NewImage.png", NewImageData);
        }
    }
}
