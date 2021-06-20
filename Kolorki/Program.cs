using System;
using System.IO;

namespace Kolorki
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose instance:\n1 - queen6\n2 - miles250\n3 - gc1000_300013\n4 - gc500\n5 - lo450_5a");
            var instanceNumber = Convert.ToInt32(Console.ReadLine());
            var filename = (Instance)instanceNumber switch
            {
                Instance.queen6 => "queen6.txt", // also the default valie
                Instance.miles250 => "miles250.txt",
                Instance.gc_1000_300013 => "gc_1000_300013.txt",
                Instance.gc500 => "gc500.txt",
                Instance.le450_5a => "le450_5a.txt",
                _ => "queen6.txt",
            };

            var graph = new UndirectedGraphIO().ReadFromFile("../../../../grafy/" + filename);
            var geneticAlgorithm = new GeneticAlgorithm(50, graph);
            geneticAlgorithm.Run();
            // var c = new GraphColoringGreedy(graph).Color();
            // Console.WriteLine(c);
        }
    }

    public enum Instance
    {
        queen6 = 1,
        miles250,
        gc_1000_300013,
        gc500,
        le450_5a,
    }
}
