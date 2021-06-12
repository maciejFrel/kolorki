using System;

namespace Kolorki
{
    class Program
    {
        static void Main(string[] args)
        {
            // var graph = new UndirectedGraphIO().ReadFromFile("kolorowanie_w100.txt");
            var graph = new UndirectedGraphIO().ReadFromFile("gc_1000_300013.txt");
            var geneticAlgorithm = new GeneticAlgorithm(2, graph);
            Console.WriteLine(geneticAlgorithm.Run());
            //graph.Print();
            //var c = new GraphColoringGreedy(graph).Color();
            //Console.WriteLine(c);
        }
    }
}
