using System;

namespace Kolorki
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = new UndirectedGraphIO().ReadFromFile("test.txt");
            graph.Print();
            var c = new GraphColoringGreedy(graph).Color();
        }
    }
}
