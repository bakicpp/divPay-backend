using Business.Abstract;
using Business.Concrete;
using Confluent.Kafka;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Polly;

namespace ConsoleDeneme
{


    class Program
    {

        static void Main(string[] args)
        {
            string[] A = { "Ömer", "Faruk", "Deniz" };
            string[] B = { "Deniz", "Ömer", "Faruk" };
            string res = JaccardSimilarity(A, B);

            Console.WriteLine(res);
        }

        static string JaccardSimilarity(string[] A, string[] B)
        {
            List<string> commons = new List<string>();

            foreach (var item in A)
            {
                foreach (var item2 in B)
                {
                    if (item == item2)
                    {
                        commons.Add(item2);
                    }
                }
            }

            List<string> combination = new List<string>();

            List<string> tempA = new List<string>();

            tempA = A.ToList();

            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] != B[i])
                {
                    tempA.Add(B[i]);
                }
            }

            combination = tempA;

            float rate = ((float)commons.Count / combination.Count); //0.5 ise aynı metinler

            return rate.ToString();
        }
    }
}

