using System;
using System.Collections.Generic;
using System.Linq;

namespace Kolorki
{
    public class GraphColoringTabu
    {
        private readonly UndirectedGraph graph;
        private readonly int tabuSize;
        private readonly int reps;
        private readonly int maxIterations;
        private readonly int numberOfColors;

        public GraphColoringTabu(UndirectedGraph graph, int tabuSize, int reps, int maxIterations, int numberOfColors)
        {
            this.graph = graph;
            this.tabuSize = tabuSize;
            this.reps = reps;
            this.maxIterations = maxIterations;
            this.numberOfColors = numberOfColors;
        }

        public void Run()
        {
            var colors = Enum.GetValues(typeof(Color)).Cast<Color>().Take(numberOfColors).ToList();
            var solution = CreateRandomInitSolution(colors);
            var random = new Random();

            var bestSolutionSoFar = new Dictionary<int, int>();
            var iterations = 0;
            var tabu = new List<Tuple<int, Color>>();
            var conflicts = 0;

            while (iterations < maxIterations)
            {
                var moveCandidates = new HashSet<int>();
                conflicts = CountConflictsAndUpdateCandidates(solution, moveCandidates);

                if (conflicts == 0)
                    break;
                Console.WriteLine($"Conflicts {conflicts}");

                var randomMoveCandidates = moveCandidates.ElementAt(random.Next(0, moveCandidates.Count));
                var newSolution = new Dictionary<int, Color>(solution); 

                for (int r = 0; r < reps; r++)
                {
                    var newColor = colors.Where(x => x != solution[randomMoveCandidates]).ElementAt(random.Next(0, colors.Count - 1));
                    randomMoveCandidates = moveCandidates.ElementAt(random.Next(0, moveCandidates.Count));
                    newSolution = new Dictionary<int, Color>(solution)
                    {
                        [randomMoveCandidates] = newColor
                    };

                    var newConflicts = CountConflictsAndUpdateCandidates(newSolution);

                    if (newConflicts >= conflicts)
                    {
                        continue;
                    }

                    if (newConflicts <= DictInserIfNotExistsAndGetValue(bestSolutionSoFar, conflicts))
                    {
                        bestSolutionSoFar[conflicts] = newConflicts - 1;
                        var tabuElem = new Tuple<int, Color>(randomMoveCandidates, newColor);

                        if (tabu.Contains(tabuElem))
                        {
                            tabu.Remove(tabuElem);
                            break;
                        }
                    }
                    else
                    {
                        if (tabu.Contains(new Tuple<int, Color>(randomMoveCandidates, newColor)))
                            continue;
                    }
                    break;
                }

                tabu.Add(new Tuple<int, Color>(randomMoveCandidates, solution[randomMoveCandidates]));

                if (tabu.Count > tabuSize)
                {
                    tabu.RemoveAt(0);
                }

                solution = newSolution;
                iterations++;
            }

            if (conflicts != 0)
                Console.WriteLine($"No solution found!, {conflicts}");
            else
                Console.WriteLine("Solution found!");
        }

        public int DictInserIfNotExistsAndGetValue(Dictionary<int, int> dict, int value)
        {
            int res;
            if (!dict.ContainsKey(value))
            {
                dict.Add(value, value - 1);
                res = value - 1;
            }
            else
            {
                res = dict[value];
            }

            return res;
        }

        public int CountConflictsAndUpdateCandidates(Dictionary<int, Color> solution, HashSet<int> candidates = null)
        {
            var conflicts = 0;

            //Since it's undirect graph go just over upper triangle of adjacency matrix
            for (int i = 0; i < graph.GetNumberOfVertices(); i++)
            {
                for (int j = i + 1; j < graph.GetNumberOfVertices(); j++)
                {
                    if (!IsBadEdge(i, j, solution))
                    {
                        continue;
                    }
                    if (candidates != null)
                    {
                        candidates.Add(i);
                        candidates.Add(j);
                    }

                    conflicts++;
                }
            }

            return conflicts;
        }

        public bool IsBadEdge(int i, int j, Dictionary<int, Color> solution) =>
            graph.AdjenancyMatrix[i, j].Exists && solution[i] == solution[j];

        public Dictionary<int, Color> CreateRandomInitSolution(IEnumerable<Color> colors)
        {
            var solution = new Dictionary<int, Color>();
            var random = new Random();

            for (int i = 0; i < graph.GetNumberOfVertices(); i++)
            {
                solution.Add(i, colors.ElementAt(random.Next(0, numberOfColors)));
            }

            return solution;
        }

        public Dictionary<int, Color> GenerateGreedySolutionDict(UndirectedGraph graph)
        {
            var greedy = new GraphColoringGreedy(graph);
            greedy.Color();
            var res = new Dictionary<int, Color>();

            for (int i = 0; i < graph.GetNumberOfVertices(); i++)
            {
                res.Add(i, (Color)graph.GetColor(i));
            }

            return res;
        }
    }
}
