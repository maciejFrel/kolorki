using System;
using System.Collections.Generic;
using System.Linq;

namespace Kolorki
{
    public class GeneticAlgorithm
    {
        private List<UndirectedGraph> population { get; set; }
        private int PopulationCycles = 0;
        private readonly List<Color> _colors;
        private int PopulationSize = 0;

        public GeneticAlgorithm(
            int populationNumber,
            UndirectedGraph undirectedGraph)
        {
            population = new List<UndirectedGraph>();
            PopulationCycles = populationNumber;
            PopulationSize = populationNumber;

            _colors = Enum.GetValues(typeof(Color)).Cast<Color>().ToList();

            for (int i = 0; i < populationNumber; i++)
            {
                population.Add(undirectedGraph.Clone());
            }

            ColorRandomly();
        }

        public void Run()
        {
            var i = 0;

            while (PopulationCycles < 20000)
            {
                if (i != 0)
                {
                    population = GetBestHalf();
                    var currentPopulatioSize = population.Count;
                    while (currentPopulatioSize < PopulationSize)
                    {
                        var a = population[0].Clone();
                        ColorRandomly(a);
                        population.Add(a);
                        currentPopulatioSize++;
                    }
                    PopulationCycles += population.Count / 2;
                }
                var bestParents = GetBestParents(population);
                var child = Crossover(bestParents.Item1, bestParents.Item2);
                Mutate(child);
                population.Add(child);
                i++;
                Console.WriteLine("Population cycles: "
                    + PopulationCycles
                    + ", fitness of the best graph: "
                    + Fitness(GetBestParent(population)));
            }

            var bestGraph = WisdomOfArtificialCrowds();
            Console.WriteLine("Best result: " + Fitness(bestGraph));
        }

        public int Fitness(UndirectedGraph graph)
        {
            var usedColors = new List<Color>();
            for (int i = 0; i < graph.GetNumberOfVertices(); i++)
            {
                if (!usedColors.Contains((Color)graph.GetColor(i)))
                {
                    usedColors.Add((Color)graph.GetColor(i));
                }
            }
            return usedColors.Count;
        }

        public UndirectedGraph WisdomOfArtificialCrowds()
        {
            var bestHalf = GetBestHalf();
            var bestOne = GetBestParent(population);

            var mostPopularColors = GetMostPopularColors(bestHalf);
            for (int i = 0; i < bestOne.GetNumberOfVertices(); i++)
            {
                if (IsBadEdge(i, bestOne))
                {
                    foreach (var mostPopularColor in mostPopularColors.Keys)
                    {
                        var bestOneColor = bestOne.GetColor(i);
                        var neightbourColors = bestOne.GetNeighbourColors(i);
                        if (!neightbourColors.Contains(mostPopularColor) && mostPopularColor != bestOneColor)
                        {
                            bestOne.SetColor(i, mostPopularColor);
                            break;
                        } 
                    }
                }
            }
            return bestOne;
        }

        public List<UndirectedGraph> GetBestHalf()
        {
            var results = new List<int>();
            var numberOfBestParents = population.Count / 2;
            foreach (var parent in population)
            {
                parent.Fitness = Fitness(parent);
            }
            var aa = population.OrderBy(x => x.Fitness).Take(numberOfBestParents).ToList();
            var min = population.Min(x => x.Fitness);
            return aa;
        }

        public UndirectedGraph GetBestParent(List<UndirectedGraph> parents)
        {
            var results = new List<int>();
            foreach (var parent in parents)
            {
                results.Add(Fitness(parent));
            }

            var lowestFitnessIndex = results.IndexOf(results.Min());

            return parents[lowestFitnessIndex];
        }

        public Tuple<UndirectedGraph, UndirectedGraph> GetBestParents(List<UndirectedGraph> parents)
        {
            var results = new List<int>(); 
            foreach (var parent in parents)
            {
                results.Add(Fitness(parent));
            }

            var lowestFitnessIndex = results.IndexOf(results.Min());

            var bestParent1 = parents[lowestFitnessIndex];
            results.RemoveAt(lowestFitnessIndex);
            var parents2 = parents.Where((x, i) => i != lowestFitnessIndex).ToList();
            lowestFitnessIndex = results.IndexOf(results.Min());

            return new Tuple<UndirectedGraph, UndirectedGraph>(bestParent1, parents2[lowestFitnessIndex]);
        }

        public bool ContainsBadEdge(UndirectedGraph graph)
        {
            for (int i = 0; i < graph.GetNumberOfVertices(); i++)
            {
                var color = graph.GetColor(i);
                if (graph.GetNeighbourColors(i).Contains(color))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsBadEdge(int i, UndirectedGraph graph)
        {
            var color = graph.GetColor(i);
            if (graph.GetNeighbourColors(i).Contains(color))
            {
                return true;
            }
            return false;
        }

        public Dictionary<Color, int> GetMostPopularColors(List<UndirectedGraph> graphs)
        {
            var dict = new Dictionary<Color, int>();
            foreach (var graph in graphs)
            {
                for (int i = 0; i < graph.GetNumberOfVertices(); i++)
                {
                    var color = graph.GetColor(i);
                    if (dict.ContainsKey((Color)color))
                    {
                        dict[(Color)color]++;
                    }
                    else
                    {
                        dict.Add((Color)color, 1);
                    }
                }
            }
            return dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        public UndirectedGraph Crossover(UndirectedGraph parent1, UndirectedGraph parent2)
        {
            var random = new Random();
            var crosspoint = random.Next(0, parent1.GetNumberOfVertices() - 1);
            var graph = new UndirectedGraph(parent1.GetNumberOfVertices());
            for (int i = 0; i < parent1.GetNumberOfVertices(); i++)
            {
                if (i < crosspoint)
                {
                    graph.SetColor(i, parent1.GetColor(i).Value);
                }
                else
                {
                    graph.SetColor(i, parent2.GetColor(i).Value);
                }
            }
            return graph;
        }

        public void Mutate(UndirectedGraph graph)
        {
            var random = new Random();
            for (int i = 0; i < graph.GetNumberOfVertices(); i++)
            {
                var neightboursColors = graph.GetNeighbourColors(i);
                var color = graph.GetColor(i);
                if (neightboursColors.Contains(color))
                {
                    var colors = Enum.GetValues(typeof(Color)).Cast<Color>().ToList();
                    foreach (var neighbourColor in neightboursColors)
                    {
                        colors.Remove((Color)neighbourColor);
                    }
                    
                    colors.OrderBy(x => random.Next(0, colors.Count - 1));
                    graph.SetColor(i, colors.FirstOrDefault());
                }
            }
        }

        public void ColorRandomly(UndirectedGraph graph)
        {
            var random = new Random();
            for (int i = 0; i < graph.GetNumberOfVertices(); i++)
            {
                var neightboursColors = graph.GetNeighbourColors(i);
                while (true)
                {
                    var colors = Enum.GetValues(typeof(Color)).Cast<Color>().ToList();
                    foreach (var neighbourColor in neightboursColors)
                    {
                        colors.Remove((Color)neighbourColor);
                    }
                    if (colors.Count == 0)
                    {
                        throw new Exception("Not enough colors");
                    }
                    colors.OrderBy(x => random.Next(0, colors.Count - 1));
                    graph.SetColor(i, colors.FirstOrDefault());
                    break;
                }
            }
        }

        public void ColorRandomly()
        {
            var random = new Random();
            foreach (var graph in population)
            {
                for(int i = 0; i < graph.GetNumberOfVertices(); i ++)
                {
                    var neightboursColors = graph.GetNeighbourColors(i);
                    while (true)
                    {
                        var color = _colors[random.Next(0, _colors.Count - 1)];
                        if (!neightboursColors.Contains(color))
                        {
                            graph.SetColor(i, color);
                            break;
                        }
                    }
                }
            }
        }
    }
}
