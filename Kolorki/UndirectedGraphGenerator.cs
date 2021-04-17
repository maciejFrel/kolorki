using System;

namespace Kolorki
{
    public class UndirectedGraphGenerator
    {
        public UndirectedGraph GenerateRandomInstance()
        { 
            var random = new Random();
            var graph = new UndirectedGraph(100);
            var numberOfVertices = graph.GetNumberOfVertices();
            var maxEdges = numberOfVertices * (numberOfVertices - 1) / 2;

            for (int i = 0; i < 5000; i++)
            {
                graph.AddEdge(random.Next(1, numberOfVertices), random.Next(1, numberOfVertices));
            }

            return graph;
        }
    }
}
