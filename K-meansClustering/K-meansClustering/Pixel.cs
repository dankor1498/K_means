using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_meansClustering
{
    class Pixel
    {
        public const int N = 50;//кількість пікселів для випадкової генерації

        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }

        public Pixel(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }

        public Pixel()
        {
            R = -1;
            G = -1;
            B = -1;
        }
    }
}
