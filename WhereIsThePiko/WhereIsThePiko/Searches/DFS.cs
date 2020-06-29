using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereIsThePiko.ModelStuff;

namespace WhereIsThePiko.Searches
{
    static class DFS
    {

        private static Dictionary<string, Node> visitedFrom;
        private static Stack<Node> q;

        public static bool Search(int from, int to, Graph graph)
        {
            graph.ResetNodes();
            visitedFrom = new Dictionary<string, Node>();

            Node start = graph.TheGraph[from];
            Node end = graph.TheGraph[to];

            if (start == null || end == null) { return false; }
            if (start == end) 
            {
                start.WasVisited = true;
                return true; 
            }

            q = new Stack<Node>();
            q.Push(start);

            Node current;
            while(q.Count > 0)
            {
                current = q.Pop();

                if(current == end) 
                {
                    current.WasVisited = true;

                    while(current != null)
                    {
                        //bugva se ako ima cikul
                        Debug.WriteLine("Path: " + current.Name);
                        current.IsFinalPath = true;
                        if (visitedFrom.ContainsKey(current.Name))
                        {
                            current = visitedFrom[current.Name];
                        }
                        else
                        {
                            current = null;
                        }
                    }

                    return true; 
                }

                if (!current.WasVisited)
                {
                    current.WasVisited = true;
                    foreach(var node in current.GetRelated())
                    {
                        q.Push(node);
                        if (node != start && !visitedFrom.ContainsKey(node.Name))
                        {
                            visitedFrom[node.Name] = current;
                        }
                    }
                }
            }

            return false;
        }

    }
}
