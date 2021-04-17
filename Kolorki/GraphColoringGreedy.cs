using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kolorki
{
    public class GraphColoringGreedy
    {
        private UndirectedGraph _graph;

        public GraphColoringGreedy(UndirectedGraph graph)
        {
            _graph = graph;
        }

        public int Color()
        {
            var numberOfVertices = _graph.GetNumberOfVertices();
            var usedColors = new List<Color>();
            var result = 0;

            for (int i = 0; i < numberOfVertices; i++)
            {
                var usedNeighbourColors = GetNeighbourColors(i);
                var firstAvailableColor = GetFirstAvailableColor(usedNeighbourColors);
                _graph.SetColor(i, firstAvailableColor);
                if (!usedColors.Contains(firstAvailableColor))
                {
                    usedColors.Add(firstAvailableColor);
                    result++;
                }
            }

            return result;
        }

        private Color GetFirstAvailableColor(List<Color> usedNeighbourColors)
        {
            var tmp = Enum.GetValues(typeof(Color)).Cast<Color>();

            foreach (var color in tmp)
            {
                if (!usedNeighbourColors.Contains(color))
                {
                    return color;
                }
            }

            throw new Exception("All colors are already in use");
        }

        private List<Color> GetNeighbourColors(int vertex)
        {
            var neighbours = _graph.GetNeighbours(vertex);

            return neighbours.Select(x => x.Color).ToList();
        }
    }
}
