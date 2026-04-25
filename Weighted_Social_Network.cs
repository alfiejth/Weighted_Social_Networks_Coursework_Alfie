using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weighted_Social_Networks_Coursework_Alfie
{
    internal class Weighted_Social_Network
    { }

namespace SocialNetworkInfluence
    {
        class Program
        {
            static void Main()
            {
                // Build graph
                Graph graph = new(5);

                graph.AddEdge(0, 1, 1.0);
                graph.AddEdge(1, 2, 2.0);
                graph.AddEdge(2, 3, 1.5);
                graph.AddEdge(3, 4, 1.0);
                graph.AddEdge(0, 4, 3.0);

                // Display network structure
                Console.WriteLine("SOCIAL NETWORK GRAPH (ASCII REPRESENTATION)");
                Console.WriteLine("===========================================");
                graph.PrintGraph();

                Console.WriteLine();
                Console.WriteLine("INFLUENCE SCORES");
                Console.WriteLine("================");

                double maxInfluence = 0.0;
                int mostInfluentialNode = -1;

                for (int i = 0; i < graph.VertexCount; i++)
                {
                    double influence = ComputeInfluenceScore(graph, i);
                    Console.WriteLine($"User {i}: Influence Score = {influence:F4}");

                    if (influence > maxInfluence)
                    {
                        maxInfluence = influence;
                        mostInfluentialNode = i;
                    }
                }

                Console.WriteLine();
                Console.WriteLine("MOST INFLUENTIAL USER");
                Console.WriteLine("=====================");
                Console.WriteLine($"User {mostInfluentialNode} has the highest influence score ({maxInfluence:F4})");

                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }

            // ---------------- GRAPH AND ALGORITHMS ----------------

            static double ComputeInfluenceScore(Graph graph, int source)
            {
                double[] distances = Dijkstra(graph, source);
                double sum = 0.0;

                for (int i = 0; i < distances.Length; i++)
                {
                    if (i == source)
                        continue;

                    if (double.IsPositiveInfinity(distances[i]))
                        return 0.0; // unreachable node

                    sum += distances[i];
                }

                return (graph.VertexCount - 1) / sum;
            }

            static double[] Dijkstra(Graph graph, int source)
            {
                double[] distances = new double[graph.VertexCount];
                bool[] visited = new bool[graph.VertexCount];

                for (int i = 0; i < distances.Length; i++)
                    distances[i] = double.PositiveInfinity;

                distances[source] = 0.0;

                for (int count = 0; count < graph.VertexCount - 1; count++)
                {
                    int u = MinDistance(distances, visited);
                    visited[u] = true;

                    foreach (Edge edge in graph.AdjacencyList[u])
                    {
                        if (!visited[edge.To] &&
                            distances[u] + edge.Weight < distances[edge.To])
                        {
                            distances[edge.To] = distances[u] + edge.Weight;
                        }
                    }
                }

                return distances;
            }

            static int MinDistance(double[] distances, bool[] visited)
            {
                double min = double.PositiveInfinity;
                int minIndex = -1;

                for (int i = 0; i < distances.Length; i++)
                {
                    if (!visited[i] && distances[i] <= min)
                    {
                        min = distances[i];
                        minIndex = i;
                    }
                }

                return minIndex;
            }
        }

        // ---------------- SUPPORTING CLASSES ----------------

        class Edge(int to, double weight)
        {
            public int To { get; } = to;
            public double Weight { get; } = weight;
        }

        class Graph
        {
            public int VertexCount { get; }
            public List<Edge>[] AdjacencyList { get; }

            public Graph(int vertexCount)
            {
                VertexCount = vertexCount;
                AdjacencyList = new List<Edge>[vertexCount];

                for (int i = 0; i < vertexCount; i++)
                    AdjacencyList[i] = [];
            }

            public void AddEdge(int from, int to, double weight)
            {
                AdjacencyList[from].Add(new Edge(to, weight));
                AdjacencyList[to].Add(new Edge(from, weight)); // Undirected
            }

            public void PrintGraph()
            {
                for (int i = 0; i < VertexCount; i++)
                {
                    Console.Write($"User {i} --> ");

                    for (int j = 0; j < AdjacencyList[i].Count; j++)
                    {
                        Edge edge = AdjacencyList[i][j];
                        Console.Write($"[User {edge.To}, w={edge.Weight}]");

                        if (j < AdjacencyList[i].Count - 1)
                            Console.Write(" -- ");
                    }

                    Console.WriteLine();
                }
            }
        }
    }

}
