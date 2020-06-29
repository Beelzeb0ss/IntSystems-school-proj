using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using WhereIsThePiko.ModelStuff;
using WhereIsThePiko.Utility;

namespace WhereIsThePiko.Searches
{
    static class CoordWeightMix
    {
        static Dictionary<string, Node> visitedFrom;
        static Dictionary<string, double> score;
        static List<Node> deadEnds;

        public static bool Search(int from, int to, Graph graph)
        {
            graph.ResetNodes();
            visitedFrom = new Dictionary<string, Node>();
            score = new Dictionary<string, double>();
            deadEnds = new List<Node>();

            Node start = graph.TheGraph[from];
            Node end = graph.TheGraph[to];

            if (start == null || end == null) { return false; }
            if (start == end)
            {
                start.WasVisited = true;
                return true;
            }

            Node current = start;
            Node nextNode;
            while(current != null)
            {
                nextNode = null;
                current.WasVisited = true;
                if(current == end)
                {
                    SetPath(current);
                    return true;
                }
                foreach(Node n in current.GetRelated())
                {
                    /*
                    if(n == end)
                    {
                        n.WasVisited = true;
                        visitedFrom[n.Name] = current;
                        SetPath(n);
                        return true;
                    }
                    */

                    if (!n.WasVisited && !deadEnds.Contains(n))
                    {
                        visitedFrom[n.Name] = current;
                        CalculateScore(n, end);
                        if (nextNode == null)
                        {
                            nextNode = n;
                            continue;
                        }
                        if(score[nextNode.Name] > score[n.Name])
                        {
                            nextNode = n;
                        }
                    }

                }
                if (nextNode == null && !deadEnds.Contains(current))
                {
                    deadEnds.Add(current);
                    if (visitedFrom.ContainsKey(current.Name))
                    {
                        nextNode = visitedFrom[current.Name];
                    }
                }

                current = nextNode;
            }

            return false;
        }

        private static void CalculateScore(Node node, Node end)
        {
            Debug.WriteLine(node.Name + " dist|weight " + UtilityStuff.Dist(end, node) * 0.8f + " | " + (node.Weight * 0.6f));
            score[node.Name] = UtilityStuff.Dist(end, node) * 0.8f + (node.Weight * 0.6f);
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

    }
}
