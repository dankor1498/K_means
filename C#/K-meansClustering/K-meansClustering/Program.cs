using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_meansClustering
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Default;

            Pixel[] mas = {         //тест з наперед спланованими даними
		        new Pixel(255, 140, 50),
                new Pixel(100, 70, 1),
                new Pixel(150, 20, 200),
                new Pixel(251, 141, 51),
                new Pixel(104, 69, 3),
                new Pixel(153, 22, 210),
                new Pixel(252, 138, 54),
                new Pixel(101, 74, 4)
            };

            KMeans image_1 = new KMeans(8, mas, 3);
            image_1.Print();    //виведення на консоль
            image_1.Clustering();
        }
    }
}
