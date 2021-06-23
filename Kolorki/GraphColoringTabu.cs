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
            var solution = new Dictionary<int, Color>();
            var random = new Random();

            for (int i = 0; i < graph.GetNumberOfVertices(); i++)
            {
                solution.Add(i, colors.ElementAt(random.Next(0, numberOfColors)));
            }

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

                //var moveCandidatesList = moveCandidates.ToList();
                int randomNode = moveCandidates.ElementAt(random.Next(0, moveCandidates.Count));
                Dictionary<int, Color> newSolution = new Dictionary<int, Color>(solution); 

                for (int r = 0; r < reps; r++)
                {
                    var newColor = colors.Where(x => x != solution[randomNode]).ElementAt(random.Next(0, colors.Count - 1));
                    randomNode = moveCandidates.ElementAt(random.Next(0, moveCandidates.Count));
                    newSolution = new Dictionary<int, Color>(solution)
                    {
                        [randomNode] = newColor
                    };

                    var newConflicts = CountConflictsAndUpdateCandidates(newSolution);

                    if (newConflicts < conflicts)
                    {
                        var tmp = 0;
                        if (!bestSolutionSoFar.ContainsKey(conflicts))
                        {
                            bestSolutionSoFar.Add(conflicts, conflicts - 1);
                            tmp = conflicts - 1;
                        }
                        else
                        {
                            tmp = bestSolutionSoFar[conflicts];
                        }

                        if (newConflicts <= tmp)
                        {
                            bestSolutionSoFar[conflicts] = newConflicts - 1;

                            var tabuElem = new Tuple<int, Color>(randomNode, newColor);
                            if (tabu.Contains(tabuElem))
                            {
                                tabu.Remove(tabuElem);
                                break;
                            }
                        }
                        else
                        {
                            if (tabu.Contains(new Tuple<int, Color>(randomNode, newColor)))
                                continue;
                        }
                        break;
                    }
                }

                tabu.Add(new Tuple<int, Color>(randomNode, solution[randomNode]));

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

        public int CountConflictsAndUpdateCandidates(Dictionary<int, Color> solution, HashSet<int> candidates = null)
        {
            var conflicts = 0;

            for (int i = 0; i < graph.GetNumberOfVertices(); i++)
            {
                for (int j = i + 1; j < graph.GetNumberOfVertices(); j++)
                {
                    if (graph.AdjenancyMatrix[i, j].Exists && solution[i] == solution[j])
                    {
                        if (candidates != null)
                        {
                            candidates.Add(i);
                            candidates.Add(j);
                        }
                        
                        conflicts++;
                    }
                }
            }

            return conflicts;
        }
    }
}
