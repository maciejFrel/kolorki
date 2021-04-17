using System;

namespace Kolorki
{
    public class UndirectedGraphGenerator
    {
        public UndirectedGraph GenerateRandomInstance()
        { 
            var random = new Random();
            var graph = new UndirectedGraph(random.Next(1, 30));
            var numberOfVertices = graph.GetNumberOfVertices();
            var maxEdges = numberOfVertices * (numberOfVertices - 1) / 2;

            for (int i = 0; i < random.Next(1, maxEdges); i++)
            {
                graph.AddEdge(random.Next(1, numberOfVertices), random.Next(1, numberOfVertices));
            }

            return graph;
        }
    }
}
