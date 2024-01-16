using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace RNG
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine(RandomNumberGenerator.IntUniformRand(1,100).ToString());
        }
    }

    class RandomNumberGenerator
    {
        private static readonly Random rnd = new Random();

        public static double NextDouble()
        {
            return rnd.NextDouble();
        }

        public static double UniformRand(double lowerbound = 0, double upperbound = 1)
        {
            if (upperbound < lowerbound) { throw new Exception("Upper bound needs to be higher"); }
            return lowerbound + NextDouble() * (upperbound - lowerbound);
        }//Uniform Random Variable

        public static double GaussianRand(double mu = 0, double sigma = 1)
        {
            if (sigma < 0) { throw new Exception("Standard deviation cannot be negative"); }
            double U1 = NextDouble();
            double U2 = NextDouble();
            return mu + sigma * Math.Sqrt(-2 * Math.Log(U1) * Math.Cos(2 * Math.PI * U2));//Box muller transformation
        }//Gaussian Random Variable

        public static double ExponentialRand(double lambda = 1)
        {
            double p = NextDouble();
            return -Math.Log(1-p)/lambda;
        }

        public static bool BinaryRand(double p = 0.5)
        {
            if (!(p <= 1 && p >= 0)) { throw new Exception("Invalid probaility"); }

            return NextDouble() < p;
        }//Probability p of returning true

        public static T FairDraw<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentException("List is null or empty");
            }

            int len = list.Count;
            int index = (int)Math.Floor(NextDouble() * len); // Cast to int
            return list[index];
        }//Draw from input list

        private static TKey DrawKey<TKey>(Dictionary<TKey, double> items) where TKey : notnull
        {
            // Normalize the probabilities
            double total = items.Values.Sum();
            var normalizedItems = items.ToDictionary(item => item.Key, item => item.Value / total);

            // Draw a key based on normalized probabilities
            double randomPoint = new Random().NextDouble();
            double cumulative = 0.0;
            foreach (var item in normalizedItems)
            {
                cumulative += item.Value;
                if (randomPoint < cumulative)
                {
                    return item.Key;
                }
            }

            // In case of rounding errors, return the last item
            return normalizedItems.Last().Key;
        }//Draw a key according to probability value

        public static void Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }//Shuffle list by Fisher-Yates algorithm
        public static int IntUniformRand(int lowerbound, int upperbound)
        {
            List<int> TempList = new List<int>();
            for (int i = lowerbound; i <= upperbound; i++)
            {
                TempList.Add(i);
            }
            return FairDraw(TempList);
        }//Uniform integer draw from lowerbound to upperbound

    }
}
