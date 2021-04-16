﻿using System;
using System.Collections.Generic;

namespace Kolorki
{
    public class UndirectedGraph
    {
        private readonly int Vertices;
        private readonly GraphNode[,] AdjenancyMatrix;

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

        public void AddEdge(int[] vertices) => AddEdge(vertices[0], vertices[1]);

        public void AddEdge(int vertex1, int vertex2)
        {
            AdjenancyMatrix[vertex1 - 1, vertex2 - 1].Exists = true;
            AdjenancyMatrix[vertex2 - 1, vertex1 - 1].Exists = true;
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
    }
}