using System;
using System.IO;

namespace Kolorki
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose algorithm:\n1 - Tabu\n2 - greedy");
            var algorithmChoice = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Choose instance:\n1 - queen6\n2 - miles250\n3 - gc1000_300013\n4 - gc500\n5 - lo450_5a\n6 - miles1000\n7 - myciel4\n8 - myciel7\n9 - homer\n10 - queen13");
            var instanceNumber = Convert.ToInt32(Console.ReadLine());

            var filename = (Instance)instanceNumber switch
            {
                Instance.queen6 => "queen6.txt", // also the default value
                Instance.miles250 => "miles250.txt",
                Instance.gc_1000_300013 => "gc_1000_300013.txt",
                Instance.gc500 => "gc500.txt",
                Instance.le450_5a => "le450_5a.txt",
                Instance.miles1000 => "miles1000.txt",
                Instance.myciel4 => "myciel4.txt",
                Instance.myciel7 => "myciel7.txt",
                Instance.homer => "homer.txt",
                Instance.queen13 => "queen13.txt",
                _ => "queen6.txt",
            };

            var graph = new UndirectedGraphIO().ReadFromFile("../../../../grafy/" + filename);

            if (algorithmChoice == 1)
            {
                Console.WriteLine("Number of iterations:");
                var numberOfIterations = Convert.ToInt32(Console.ReadLine());
                var tabu = new GraphColoringTabu(graph, 7, 100, numberOfIterations, 7);
                tabu.Run();
            }
            else if (algorithmChoice == 2)
            {   
                var c = new GraphColoringGreedy(graph).Color();
                Console.WriteLine(c);
            }
            else
            {
                var geneticAlgorithm = new GeneticAlgorithm(50, graph);
                geneticAlgorithm.Run();
            }
        }
    }

    public enum Instance
    {
        queen6 = 1,
        miles250,
        gc_1000_300013,
        gc500,
        le450_5a,
        miles1000, // 42
        myciel4, // 5
        myciel7, // 8
        homer, // 13
        queen13 // 13
    }
}
