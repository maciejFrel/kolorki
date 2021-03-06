using System;
using System.Collections.Generic;
using System.Linq;

namespace Kolorki
{
    public class GraphColoringGreedy
    {
        private UndirectedGraph _graph;
        private IEnumerable<Color> _colors;

        public GraphColoringGreedy(UndirectedGraph graph)
        {
            _graph = graph;
            _colors = Enum.GetValues(typeof(Color)).Cast<Color>();
        }

        public int Color()
        {
            var numberOfVertices = _graph.GetNumberOfVertices();
            var usedColors = new List<Color>();
            var result = 0;

            for (int i = 0; i < numberOfVertices; i++)
            {
                var usedNeighbourColors = _graph.GetNeighbourColors(i);
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

        private Color GetFirstAvailableColor(List<Color?> usedNeighbourColors)
        {
            foreach (var color in _colors)
            {
                if (!usedNeighbourColors.Contains(color))
                {
                    return color;
                }
            }

            throw new Exception("All colors are already in use");
        }
    }
}
