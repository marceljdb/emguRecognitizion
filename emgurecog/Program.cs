using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emgurecog
{
    class Program
    {
        static void Main(string[] args)
        {

            var recog = new Recog();
            recog.Trainer(1, new List<Bitmap>() { new Bitmap(@"Lib\bill.jpg"), new Bitmap(@"Lib\bill2.jpg") } );
            //var isValid = recog.Execute(new Bitmap(@"Lib\moss_pic2.jpg"));
            var isValid = recog.Execute(new Bitmap(@"Lib\bill.jpg"));
            

            

            Console.WriteLine($"{"Bill Gates é você?: "}{isValid}"); 
            Console.ReadKey();
        }
    }
}
