using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_meansClustering
{
    class KMeans
    {
        public const int KK = 10;             //кількість кластерів
        public const int MaxIterations = 100; //максимальна кількість ітерацій

        private List<Pixel> pixcels = new List<Pixel>(); //масив пікселів, з трьох кольорів кожен
        private int qKlaster;           //кількість кластерів
        private int kPixcel;            //кількість пікселів
        private List<Pixel> centrs = new List<Pixel>();  //центри кластеризації

        private void IdentifyCenters()//метод випадкового вибору початкових центрів
        {
            Random random = new Random();
            Pixel temp = new Pixel();
            Pixel[] mas = new Pixel[qKlaster];

            for (int i = 0; i < qKlaster; i++)
            {
                mas[i] = new Pixel();
            }

            for (int i = 0; i < qKlaster; i++)
            {
                temp = pixcels[random.Next(0, kPixcel)];
                for (int j = i; j < qKlaster; j++)
                {
                    if (temp.R != mas[j].R && temp.G != mas[j].G && temp.B != mas[j].B)
                    {
                        mas[j] = new Pixel(temp.R, temp.G, temp.B);
                    }
                    else
                    {
                        i--;
                        break;
                    }
                }
            }
            for (int i = 0; i < qKlaster; i++)
            {
                centrs.Add(mas[i]);
            }
        } 

        private double Compute(Pixel k1, Pixel k2)
        {
            return Math.Sqrt(Math.Pow((k1.R - k2.R), 2) + Math.Pow((k1.G - k2.G), 2) + Math.Pow((k1.B - k2.B), 2));
        }

        private double ComputeS(double a, double b)
        {
            return (a + b) / 2;
        }

        public KMeans()
        {
            qKlaster = 0;
            kPixcel = 0;
        }

        public KMeans(int n, Pixel[] mas, int nKlaster)
        {
            for (int i = 0; i < n; i++)
            {
                pixcels.Add(mas[i]);
            }
            qKlaster = nKlaster;
            kPixcel = n;
            IdentifyCenters();
        }

        public void Clustering() //метод кластеризації
        {
            Console.WriteLine("\n\nПочаток кластеризації:");

            List<int> check_1 = new List<int>(kPixcel);
            List<int> check_2 = new List<int>(kPixcel);

            for (int i = 0; i < kPixcel; i++)
            {
                check_1.Add(-1);
                check_2.Add(-2);
            }

            int iter = 0;

            while (true)
            {
                Console.WriteLine("\n\n---------------- Ітерація №" + iter 
                    + " ----------------\n");
                {
                    for (int j = 0; j < kPixcel; j++)
                    {
                        double[] mas = new double[qKlaster];

                        for (int i = 0; i < qKlaster; i++)
                        {
                            mas[i] = Compute(pixcels[j], centrs[i]);
                            Console.WriteLine($"Відстань від пікселя {j} до центру #{i}: {mas[i]:f2}"); 
                        }

                        double min_dist = mas[0];
                        int m_k = 0;
                        for (int i = 0; i < qKlaster; i++)
                        {
                            if (min_dist > mas[i])
                            {
                                min_dist = mas[i];
                                m_k = i;
                            }
                        }
                        Console.WriteLine("Мінімальна відстань до центру #" + m_k);
                        Console.WriteLine("Перераховуємо центр #" + m_k + ": ");
                        centrs[m_k].R = ComputeS(pixcels[j].R, centrs[m_k].R);
                        centrs[m_k].G = ComputeS(pixcels[j].G, centrs[m_k].G);
                        centrs[m_k].B = ComputeS(pixcels[j].B, centrs[m_k].B);
                        Console.WriteLine($"{centrs[m_k].R:f2} {centrs[m_k].G:f2} {centrs[m_k].B:f2}\n"); 
                    }

                    int[] mass = new int[kPixcel];
                    Console.WriteLine("\nЗробимо класифікацію пікселів: "); 
                    for (int k = 0; k < kPixcel; k++)
                    {
                        double[] mas = new double[qKlaster];

                        for (int i = 0; i < qKlaster; i++)
                        {
                            mas[i] = Compute(pixcels[k], centrs[i]);
                            Console.WriteLine($"Відстань від пікселя №{k} до центру #{i}: {mas[i]:f2}");
                        }

                        double min_dist = mas[0];
                        int m_k = 0;
                        for (int i = 0; i < qKlaster; i++)
                        {
                            if (min_dist > mas[i])
                            {
                                min_dist = mas[i];
                                m_k = i;
                            }
                        }
                        mass[k] = m_k;
                        Console.WriteLine("Піксель №" + k + " найближче до центру #" + m_k + "\n");
                    }

                    Console.WriteLine("Масив відповідності пікселів і центрів: "); 
                    for (int i = 0; i < kPixcel; i++)
                    {
                        Console.Write(mass[i] + " ");
                        check_1[i] = mass[i];
                    }
                    Console.WriteLine('\n');

                    Console.WriteLine("Результат кластеризації: ");
                    int itr = KK + 1;
                    for (int i = 0; i < qKlaster; i++)
                    {
                        Console.WriteLine("Кластер #" + i);
                        for (int j = 0; j < kPixcel; j++)
                        {
                            if (mass[j] == i)
                            {
                                Console.WriteLine(pixcels[j].R + " " + pixcels[j].G
                                    + " " + pixcels[j].B); 
                                mass[j] = ++itr;
                            }
                        }
                    }

                    Console.WriteLine("Нові центри: ");
                    for (int i = 0; i < qKlaster; i++)
                    {
                        Console.WriteLine($"{centrs[i].R:f2} {centrs[i].G:f2} {centrs[i].B:f2} - #{i}"); 
                    }
                }
                iter++;
                if (check_1 == check_2 || iter >= MaxIterations)
                {
                    break;
                }
                check_2 = check_1;
            }
            Console.WriteLine("\n\nКінець кластеризації."); 
        }

        public void Print() //метод виведення інформації
        {
            Console.WriteLine("Вхідні пікселі: ");
            for (int i = 0; i < kPixcel; i++)
            {
                Console.WriteLine(pixcels[i].R + " " + pixcels[i].G
                    + " " +pixcels[i].B + " - №" + i); 
            }
            Console.WriteLine("\nВипадкові початкові центри кластеризації: ");
            for (int i = 0; i < qKlaster; i++)
            {
                Console.WriteLine(centrs[i].R + " " + centrs[i].G + " "
                   + centrs[i].B + " - #" + i);
            }
            Console.WriteLine("\nКількість кластерів: " + qKlaster);
            Console.WriteLine("Кількість пікселів: " + kPixcel); 
        } 
    }
}
