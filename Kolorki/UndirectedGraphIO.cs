using System.IO;
using System.Linq;

namespace Kolorki
{
    public class UndirectedGraphIO
    {
        public void SaveToFile(UndirectedGraph graph, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                filePath = "results.txt";
            }

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine(graph.GetNumberOfVertices());

                foreach (var item in graph.GetEdgesList())
                {
                    sw.WriteLine($"{item.Item1} {item.Item2}");
                }
            }
        }

        public UndirectedGraph ReadFromFile(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                var numberOfVertices = sr.ReadLine();
                var graph = new UndirectedGraph(int.Parse(numberOfVertices));

                while ((line = sr.ReadLine()) != null)
                {
                    graph.AddEdge(line.Split(' ').Select(x => int.Parse(x)).ToArray());
                }

                return graph;
            }
        }
    }
}
