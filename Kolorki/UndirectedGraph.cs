using System;
using System.Text.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Kolorki
{
    public class UndirectedGraph
    {
        public int Vertices;
        public GraphNode[,] AdjenancyMatrix;
        public int Fitness;

        public UndirectedGraph(int numberOfVertices)
        {
            Vertices = numberOfVertices;
            AdjenancyMatrix = new GraphNode[numberOfVertices, numberOfVertices];

            for (int i = 0; i < numberOfVertices; i++)
            {
                for (int j = 0; j < numberOfVertices; j++)
                {
                    AdjenancyMatrix[i, j] = new GraphNode();
                }
            }
        }

        public UndirectedGraph(int numberOfVertices, GraphNode[,] martix)
        {
            Vertices = numberOfVertices;
            AdjenancyMatrix = new GraphNode[numberOfVertices, numberOfVertices];

            for (int i = 0; i < numberOfVertices; i++)
            {
                for (int j = 0; j < numberOfVertices; j++)
                {
                    AdjenancyMatrix[i, j] = new GraphNode();
                    AdjenancyMatrix[i, j].Exists = martix[i, j].Exists;
                }
            }
        }

        public void AddEdge(int[] vertices) => AddEdge(vertices[0], vertices[1]);

        public void AddEdge(int vertex1, int vertex2)
        { 
            AdjenancyMatrix[vertex1 - 1, vertex2 - 1].Exists = true;
            AdjenancyMatrix[vertex1 - 1, vertex2 - 1].Color = null;
            AdjenancyMatrix[vertex2 - 1, vertex1 - 1].Exists = true;
            AdjenancyMatrix[vertex2 - 1, vertex1 - 1].Color = null;
        }

        public List<Tuple<int, int>> GetEdgesList()
        {
            var results = new List<Tuple<int, int>>();

            for (int i = 0; i < Vertices; i++)
            {
                for (int j = i; j < Vertices; j++)
                {
                    if (AdjenancyMatrix[i, j].Exists)
                    {
                        results.Add(new Tuple<int, int>(i + 1, j + 1));
                    }
                }
            }

            return results;
        }

        public int GetNumberOfVertices() => Vertices;

        public void ColorNode(int row, int column, Color color) => AdjenancyMatrix[row - 1, column - 1].Color = color;

        public void Print()
        {
            for (int i = 0; i < Vertices; i++)
            {
                for (int j = 0; j < Vertices; j++)
                {
                    Console.Write($"{Convert.ToInt32(AdjenancyMatrix[i, j].Exists)}{(j < Vertices - 1 ? "," : "")} ");
                }

                Console.WriteLine(Environment.NewLine);
            }
        }

        public List<GraphNode> GetNeighbours(int vertex)
        {
            var neighbours = new List<GraphNode>();

            for (int i = 0; i < GetNumberOfVertices(); i++)
            {
                if (AdjenancyMatrix[vertex, i].Exists && i != vertex)
                {
                    neighbours.Add(AdjenancyMatrix[vertex, i]);
                }
            }
            
            return neighbours;
        }

        internal void SetColor(int vertex, Color color)
        {
            for (int i = 0; i < GetNumberOfVertices(); i++)
            {
                AdjenancyMatrix[vertex, i].Color = color;
                AdjenancyMatrix[i, vertex].Color = color;
            }
        }

        public Color? GetColor(int vertice)
        {
            if (AdjenancyMatrix[vertice, vertice].Color == null)
            {
                var a = 1;
            }
            return AdjenancyMatrix[vertice, vertice].Color;
        }

        public List<Color?> GetNeighbourColors(int vertex)
        {
            var neighbours = GetNeighbours(vertex);

            return neighbours.Where(x => x.Color != null).Select(x => x.Color).ToList();
        }

        public UndirectedGraph Clone()
        {
            return new UndirectedGraph(Vertices, AdjenancyMatrix);
        }
    }
}
