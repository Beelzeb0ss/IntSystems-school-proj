using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WhereIsThePiko.ModelStuff;
using WhereIsThePiko.Searches;
using WhereIsThePiko.Utility;

namespace WhereIsThePiko.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {

        #region OnPropChange
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        public ObservableCollection<Node> Graph
        {
            get
            {
                return graph.TheGraph;
            }
        }

        private ObservableCollection<Path> paths;
        public ObservableCollection<Path> Paths
        {
            get
            {
                return paths;
            }
        }

        private ToolEnum selectedTool;
        public ToolEnum SelectedTool
        {
            get
            {
                return selectedTool;
            }
            set
            {
                selectedTool = value;
                DeselectNode();
                OnPropertyChanged("SelectedTool");
            }
        }

        private Node selectedNode;
        public Node SelectedNode
        {
            get
            {
                return selectedNode;
            }
            set
            {
                if(selectedNode == value)
                {
                    return;
                }
                selectedNode = value;

                if(selectedNode == null)
                {
                    EditNodeVisibility = Visibility.Hidden;
                }
                else
                {
                    EditNodeVisibility = Visibility.Visible;
                    Debug.WriteLine("Node name: " + selectedNode.Name);
                }

                OnPropertyChanged("SelectedNode");
            }
        }

        private Visibility editNodeVisibility = Visibility.Hidden;
        public Visibility EditNodeVisibility
        {
            get
            {
                return editNodeVisibility;
            }
            set
            {
                if(editNodeVisibility == value)
                {
                    return;
                }
                editNodeVisibility = value;
                OnPropertyChanged("EditNodeVisibility");
            }
        }

        private bool isPathTwoWay = false;
        public bool IsPathTwoWay
        {
            get
            {
                return isPathTwoWay;
            }
            set
            {
                if(isPathTwoWay == value)
                {
                    return;
                }
                isPathTwoWay = value;
                OnPropertyChanged("IsPathTwoWay");
            }
        }

        private double canvasWidth = 0;
        public double MinCanvasWidth
        {
            get
            {
                return canvasWidth;
            }
            set
            {
                if(canvasWidth == value)
                {
                    return;
                }

                canvasWidth = value;
                OnPropertyChanged("MinCanvasWidth");
            }
        }

        private double canvasHeight = 0;
        public double MinCanvasHeight
        {
            get
            {
                return canvasHeight;
            }
            set
            {
                if(canvasHeight == value)
                {
                    return;
                }

                canvasHeight = value;
                OnPropertyChanged("MinCanvasHeight");
            }
        }

        #region InternalShit
        private Graph graph;
        #endregion

        #region Commands
        private CommandHandler mouseDownCommand;
        public ICommand MouseDownCommand
        {
            get
            {
                if(mouseDownCommand == null)
                {
                    mouseDownCommand = new CommandHandler(MouseDown, null);
                }
                return mouseDownCommand;
            }
        }

        private CommandHandler addPathCommand;
        public ICommand AddPathCommand
        {
            get
            {
                if(addPathCommand == null)
                {
                    addPathCommand = new CommandHandler(AddPath, null);
                }
                return addPathCommand;
            }
        }

        private CommandHandler removePathCommand;
        public ICommand RemovePathCommand
        {
            get
            {
                if(removePathCommand == null)
                {
                    removePathCommand = new CommandHandler(RemovePath, null);
                }
                return removePathCommand;
            }
        }

        private CommandHandler deleteNodeCommand;
        public ICommand DeleteNodeCommand
        {
            get
            {
                if(deleteNodeCommand == null)
                {
                    deleteNodeCommand = new CommandHandler(DeleteNode, null);
                }
                return deleteNodeCommand;
            }
        }

        private CommandHandler runDfsCommand;
        public ICommand RunDfsCommand
        {
            get
            {
                if(runDfsCommand == null)
                {
                    runDfsCommand = new CommandHandler(RunDFS, null);
                }
                return runDfsCommand;
            }
        }

        private CommandHandler runASTARCommand;
        public ICommand RunASTARCommand
        {
            get
            {
                if(runASTARCommand == null)
                {
                    runASTARCommand = new CommandHandler(RunASTAR, null);
                }
                return runASTARCommand;
            }
        }

        private CommandHandler runFewestPathsCommand;
        public ICommand RunFewestPathsCommand
        {
            get
            {
                if(runFewestPathsCommand == null)
                {
                    runFewestPathsCommand = new CommandHandler(RunFewestPaths);
                }
                return runFewestPathsCommand;
            }
        }

        private CommandHandler runCoordWeightMixCommand;
        public ICommand RunCoordWeightMixCommand
        {
            get
            {
                if(runCoordWeightMixCommand == null)
                {
                    runCoordWeightMixCommand = new CommandHandler(RunCoordWeightMix);
                }
                return runCoordWeightMixCommand;
            }
        }
        #endregion

        private void MouseDown(Object e)
        {
            switch (selectedTool)
            {
                case ToolEnum.CreateNode:
                    CreateNode(Mouse.GetPosition((IInputElement)e));
                    break;
            }
        }

        private void CreateNode(Point pos)
        {
            graph.AddNode(new Node(UtilityStuff.NextName, pos.X - 25, pos.Y - 25));

            if(pos.X - 25 > MinCanvasWidth)
            {
                MinCanvasWidth = pos.X + 50;
            }
            if(pos.Y - 25 > MinCanvasHeight)
            {
                MinCanvasHeight = pos.Y + 50;
            }
        }

        private void DeselectNode()
        {
            selectedNode = null;
        }

        private void AddPath(object e)
        {
            ComboBox cb = (ComboBox)e;
            if(cb.SelectedIndex == -1) { return; }

            AddPath(UtilityStuff.IndexInGraph(selectedNode, graph), cb.SelectedIndex, isPathTwoWay, UtilityStuff.Dist(graph.TheGraph[UtilityStuff.IndexInGraph(selectedNode, graph)], Graph[cb.SelectedIndex]));
        }

        private void AddPath(int from, int to, bool isBi, double lenght)
        {
            List<Path> pathList = graph.AddPath(from, to, isBi, lenght);
            foreach(var p in pathList)
            {
                paths.Add(p);
            }
        }

        private void RemovePath(object e)
        {
            ListBox lb = (ListBox)e;
            if(lb.SelectedIndex == -1) { return; }

            Path path = selectedNode.Paths[lb.SelectedIndex];
            paths.Remove(path);
            selectedNode.RemovePath(path);
        }

        private void DeleteNode(object e)
        {
            for(int i = paths.Count - 1; i >= 0; i--)
            {
                if(paths[i].To.Name == selectedNode.Name)
                {
                    paths[i].From.RemovePath(paths[i]);
                    paths.RemoveAt(i);
                }
                else if(paths[i].From.Name == selectedNode.Name)
                {
                    paths.RemoveAt(i);
                }
            }

            for (int i = graph.TheGraph.Count - 1; i >= 0; i--)
            {
                if (graph.TheGraph[i].Name == selectedNode.Name)
                {
                    graph.TheGraph.RemoveAt(i);
                    break;
                }
            }

            DeselectNode();
        }

        private void RunDFS(object e)
        {
            object[] cbs = (object[])e;
            ComboBox from = (ComboBox)cbs[0];
            ComboBox to = (ComboBox)cbs[1];
            if (from.SelectedIndex == -1 || to.SelectedIndex == -1) { return; }

            DFS.Search(from.SelectedIndex, to.SelectedIndex, graph);
        }

        private void RunASTAR(object e)
        {
            object[] cbs = (object[])e;
            ComboBox from = (ComboBox)cbs[0];
            ComboBox to = (ComboBox)cbs[1];
            if (from.SelectedIndex == -1 || to.SelectedIndex == -1) { return; }

            ASTAR.Search(from.SelectedIndex, to.SelectedIndex, graph, true);
        }

        private void RunFewestPaths(object e)
        {
            object[] cbs = (object[])e;
            ComboBox from = (ComboBox)cbs[0];
            ComboBox to = (ComboBox)cbs[1];
            if (from.SelectedIndex == -1 || to.SelectedIndex == -1) { return; }

            ASTAR.Search(from.SelectedIndex, to.SelectedIndex, graph, false);
        }

        private void RunCoordWeightMix(object e)
        {
            object[] cbs = (object[])e;
            ComboBox from = (ComboBox)cbs[0];
            ComboBox to = (ComboBox)cbs[1];
            if(from.SelectedIndex == -1 || to.SelectedIndex == -1) { return; }

            CoordWeightMix.Search(from.SelectedIndex, to.SelectedIndex, graph);
        }

        public MainViewModel()
        {
            graph = new Graph();
            paths = new ObservableCollection<Path>();
        }
    }
}
