using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereIsThePiko.ModelStuff;

namespace WhereIsThePiko.Utility
{
    static class UtilityStuff
    {
        static int nameCounter = 0;
        public static string NextName
        {
            get
            {
                nameCounter++;
                return nameCounter.ToString();
            }
        }

        public static double Dist(Node n1, Node n2)
        {
            //бавно
            return Math.Sqrt(Math.Pow(n1.X - n2.X, 2) + Math.Pow(n1.Y - n2.Y, 2));
        }

        public static int IndexInGraph(Node n, Graph g)
        {
            for(int i = 0; i < g.TheGraph.Count; i++)
            {
                if(g.TheGraph[i].Name == n.Name)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
