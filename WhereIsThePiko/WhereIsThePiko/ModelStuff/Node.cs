using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WhereIsThePiko.ModelStuff
{
    class Node : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected string name;
        protected double x;
        protected double y;
        protected double weight;
        protected bool wasVisited;
        protected bool isFinalPath;

        private ObservableCollection<Path> paths = new ObservableCollection<Path>();
        public ObservableCollection<Path> Paths 
        {
            get
            {
                return paths;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if(name == value)
                {
                    return;
                }

                name = value;

                OnPropertyChanged("Name");
            }
        }

        public double X
        {
            get
            {
                return x;
            }
            set
            {
                if(x == value)
                {
                    return;
                }

                x = value;

                OnPropertyChanged("X");
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                if(y == value)
                {
                    return;
                }

                y = value;

                OnPropertyChanged("Y");
            }
        }

        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                if(weight == value)
                {
                    return;
                }

                weight = value;

                OnPropertyChanged("Weight");
            }
        }

        public bool WasVisited
        {
            get
            {
                return wasVisited;
            }
            set
            {
                if(wasVisited == value)
                {
                    return;
                }

                wasVisited = value;

                OnPropertyChanged("WasVisited");
            }
        }

        public bool IsFinalPath
        {
            get
            {
                return isFinalPath;
            }
            set
            {
                if(isFinalPath == value)
                {
                    return;
                }

                isFinalPath = value;

                OnPropertyChanged("IsFinalPath");
            }
        }

        public void AddPath(Path path)
        {
            if(path == null)
            {
                Debug.WriteLine("Null path | " + name);
                return;
            }
            paths.Add(path);
        }

        public void RemovePath(Path p)
        {
            paths.Remove(p);
        }

        public List<Node> GetRelated()
        {
            List<Node> related = new List<Node>();
            foreach(var p in paths)
            {
                if (!related.Contains(p.To))
                {
                    related.Add(p.To);
                }
            }
            return related;
        }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public Node(string name, double x, double y)
        {
            this.name = name;
            this.x = x;
            this.y = y;
        }
    }
}
