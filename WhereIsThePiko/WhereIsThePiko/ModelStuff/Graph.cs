using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace WhereIsThePiko.ModelStuff
{
    class Graph
    {
        private ObservableCollection<Node> graph = new ObservableCollection<Node>();

		public ObservableCollection<Node> TheGraph
		{
			get
			{
				return graph;
			}
		}

		public void AddNode(Node node)
		{
			if (node == null)
			{
				Debug.WriteLine("Node is null");
				return;
			}
			if (graph.Contains(node))
			{
				Debug.WriteLine(node.Name + " is already in graph");
				return;
			}
			graph.Add(node);
		}

		public List<Path> AddPath(int from, int to, bool isBi, double lenght)
		{
			Node node1 = graph[from];
			Node node2 = graph[to];

			List<Path> paths = new List<Path>();
			if (node1 != null && node2 != null)
			{
				Path p = new Path(node1, node2, lenght);
				node1.AddPath(p);
				paths.Add(p);
				if (isBi)
				{
					p = new Path(node2, node1, lenght);
					node2.AddPath(p);
					paths.Add(p);
				}
			}
			else
			{
				Debug.WriteLine("Null node");
			}

			return paths;
		}

		public void ResetNodes()
		{
			Debug.WriteLine("Graph reset");
			foreach(var node in graph)
			{
				node.WasVisited = false;
				node.IsFinalPath = false;
			}
		}

	}
}
