using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfGraphs
{
    [Serializable]
    public class Edge : INotifyPropertyChanged
    {
        public Edge() { }

        private static uint _id = 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        private static uint _Id
        {
            get
            {
                _id++;
                return _id - 1;
            }
            set
            {
                _id = value;
            }
        }

        public static void ResetId()
        {
            _Id = 0;
        }

        public uint Id { get; } = Edge._Id;
        public Node NodeConnected { get; set; }
        public Node NodeBase { get; set; }
        public double Weight { get; set; } = 0;
        public bool IsDirectional { get; set; } = false;
        public Edge(Node nodeBase, Node nodeConnected)
        {
            this.NodeConnected = nodeConnected;
            this.NodeBase = nodeBase;
        }
        public Edge(Node nodeBase, Node nodeConnected, double weight)
        {
            this.NodeBase = nodeBase;
            this.NodeConnected = nodeConnected;
            this.Weight = weight;
        }

        public Edge(Node nodeBase, Node nodeConnected, bool isDirectional)
        {
            this.NodeConnected = nodeConnected;
            this.NodeBase = nodeBase;
            IsDirectional = isDirectional;
        }
        public Edge(Node nodeBase, Node nodeConnected, double weight, bool isDirectional)
        {
            this.NodeBase = nodeBase;
            this.NodeConnected = nodeConnected;
            this.Weight = weight;
            IsDirectional = isDirectional;
        }

        #region graphics
        private bool _highlight = false;
        public bool Highlight
        {
            get => _highlight;
            set
            {
                _highlight = value;
                PropertyChange();
            }
        }

        [JsonIgnore]
        public Thickness DisplayMarginMiddle
        {
            get => new Thickness((NodeBase.X + NodeConnected.X) / 2 - 10, (NodeBase.Y + NodeConnected.Y) / 2 - 10, -1, -1);
        }

        public (double, double) GetPointParam()
        {
            double X1 = (double)NodeBase.X;
            double Y1 = (double)NodeBase.Y;
            double X2 = (double)NodeConnected.X;
            double Y2 = (double)NodeConnected.Y;
            double a = ViewModelMain.NodeSize / 2;

            double k = (Y1 - Y2) / (X1 - X2);
            double dX = Math.Sqrt(Math.Abs(Math.Pow(a, 2) / (1 + Math.Pow(k, 2))));
            double dY = k * dX;
            return (dX, dY);
        }

        public double XStart
        {
            get
            {
                (double, double) delta = GetPointParam();

                double nX1 = NodeBase.X - delta.Item1 * (NodeBase.X - NodeConnected.X) / Math.Abs(NodeBase.X - NodeConnected.X);
                return nX1;
            }
        }
        public double XEnd
        {
            get
            {
                (double, double) delta = GetPointParam();

                double nX2 = NodeConnected.X + delta.Item1 * (NodeBase.X - NodeConnected.X) / Math.Abs(NodeBase.X - NodeConnected.X);
                return nX2;
            }
        }
        public double YStart
        {
            get
            {
                (double, double) delta = GetPointParam();

                double nY1 = NodeBase.Y - delta.Item2 * (NodeBase.X - NodeConnected.X) / Math.Abs(NodeBase.X - NodeConnected.X);
                return nY1;
            }
        }
        public double YEnd
        {
            get
            {
                (double, double) delta = GetPointParam();

                double nY2 = NodeConnected.Y + delta.Item2 * (NodeBase.X - NodeConnected.X) / Math.Abs(NodeBase.X - NodeConnected.X);
                return nY2;
            }
        }
        #endregion

        public void PropertyChange([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
