using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WhereIsThePiko.ModelStuff;
using WhereIsThePiko.Utility;

namespace WhereIsThePiko.Searches
{
    static class ASTAR
    {

        static List<Node> openSet;
        static Dictionary<string, Node> visitedFrom;
        static Dictionary<string, double> gScore;
        static Dictionary<string, double> fScore;

        public static bool Search(int from, int to, Graph graph, bool useActualLen)
        {
            graph.ResetNodes();
            InitStuff(graph.TheGraph.ToList());

            Node start = graph.TheGraph[from];
            Node end = graph.TheGraph[to];

            gScore[start.Name] = 0;
            fScore[start.Name] = Heuristic(start, end, useActualLen);

            openSet.Add(start);

            Node current;
            double toNext;
            while(openSet.Count > 0)
            {
                current = openSet[0];
                if(current == end)
                {
                    SetPath(current);
                    return true;
                }

                openSet.Remove(current);
                foreach(Path p in current.Paths)
                {
                    if (useActualLen)
                    {
                        toNext = gScore[current.Name] + p.Lenght;
                    }
                    else
                    {
                        toNext = gScore[current.Name] + 1;
                    }

                    if(toNext < gScore[p.To.Name])
                    {
                        visitedFrom[p.To.Name] = current;
                        gScore[p.To.Name] = toNext;
                        fScore[p.To.Name] = gScore[p.To.Name] + Heuristic(p.To, end, useActualLen);

                        if (!openSet.Contains(p.To))
                        {
                            AddToOpenSet(p.To);
                        }
                    }
                }
            }

            return false;
        }

        private static void InitStuff(List<Node> nodes)
        {
            openSet = new List<Node>();
            visitedFrom = new Dictionary<string, Node>();
            gScore = new Dictionary<string, double>();
            fScore = new Dictionary<string, double>();

            foreach(Node n in nodes)
            {
                n.WasVisited = true;
                gScore[n.Name] = float.MaxValue;
                fScore[n.Name] = float.MaxValue;
            }

        }

        private static void SetPath(Node current)
        {
            int nodeCount = 1;
            while (current != null)
            {
                Debug.WriteLine("Path: " + current.Name);
                current.IsFinalPath = true;
                if (visitedFrom.ContainsKey(current.Name))
                {
                    current = visitedFrom[current.Name];
                    nodeCount++;
                }
                else
                {
                    current = null;
                    Debug.WriteLine("Node count: " + nodeCount);
                }
            }
        }

        private static double Heuristic(Node current, Node end, bool useActualLen)
        {
            if (useActualLen)
            {
                return UtilityStuff.Dist(current, end);
            }
            else
            {
                return 1;
            }
        }

        private static void AddToOpenSet(Node node)
        {
            for(int i = 0; i < openSet.Count; i++)
            {
                if(fScore[openSet[i].Name] > fScore[node.Name])
                {
                    openSet.Insert(i, node);
                    return;
                }
            }
            openSet.Add(node);
        }

    }
}
