using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereIsThePiko.ModelStuff
{
    class Path : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected Node from;
        protected Node to;
        
        public double Lenght { get; set; }

        public Node From
        {
            get
            {
                return from;
            }
            set
            {
                if(from == value)
                {
                    return;
                }

                from = value;

                OnPropertyChanged("From");
            }
        }

        public Node To
        {
            get
            {
                return to;
            }
            set
            {
                if(to == value)
                {
                    return;
                }

                to = value;

                OnPropertyChanged("To");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public Path(Node from, Node to)
        {
            this.from = from;
            this.to = to;
        }

        public Path(Node from, Node to, double lenght)
        {
            this.from = from;
            this.to = to;
            Lenght = lenght;
        }
    }
}
