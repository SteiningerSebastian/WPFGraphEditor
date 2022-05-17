using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfGraphs
{
    public class Graph : INotifyPropertyChanged
    {
        public ObservableCollection<Node> Nodes { get; private set; } = new ObservableCollection<Node>();
        public ObservableCollection<Edge> Edges { get; private set; } = new ObservableCollection<Edge>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public Edge? GetEdgeExist(Edge edge)
        {
            return Edges.FirstOrDefault(e => ((e.NodeBase.Id == edge.NodeBase.Id && e.NodeConnected.Id == edge.NodeConnected.Id) || (e.NodeBase.Id == edge.NodeConnected.Id && e.NodeConnected.Id == edge.NodeBase.Id)));
        }

        public Node AddNode()
        {
            Node n = new Node();
            Nodes.Add(n);
            PropertyChange(nameof(Nodes));
            return n;
        }
        public Edge AddEdge(Node nodeBase, Node nodeConnected, bool isDirectional = true)
        {
            Edge edge = new Edge(nodeBase, nodeConnected, isDirectional);
            Edge? e = GetEdgeExist(edge);
            if (e != null)
                return e;
            Edges.Add(edge);
            return edge;
        }
        public Edge AddEdge(Node nodeBase, Node nodeConnected, double weight, bool isDirectional = true)
        {
            Edge edge = new Edge(nodeBase, nodeConnected, weight, isDirectional);
            Edge? e = GetEdgeExist(edge);
            if (e != null)
                return e;
            Edges.Add(edge);
            return edge;
        }

        public Edge AddEdge(uint idBase, uint idConnected, bool isDirectional = true)
        {
            Node nodeBase = Nodes.FirstOrDefault(n => n.Id == idBase) ?? throw new ArgumentOutOfRangeException("A node with the given id does not exist!");
            Node nodeConnected = Nodes.FirstOrDefault(n => n.Id == idConnected) ?? throw new ArgumentOutOfRangeException("A node with the given id does not exist!");

            Edge edge = new Edge(nodeBase, nodeConnected, isDirectional);
            Edge? e = GetEdgeExist(edge);
            if (e != null)
                return e;
            Edges.Add(edge);
            return edge;
        }

        public Edge AddEdge(uint idBase, uint idConnected, double weight, bool isDirectional = true)
        {
            Node nodeBase = Nodes.FirstOrDefault(n => n.Id == idBase) ?? throw new ArgumentOutOfRangeException("A node with the given id does not exist!");
            Node nodeConnected = Nodes.FirstOrDefault(n => n.Id == idConnected) ?? throw new ArgumentOutOfRangeException("A node with the given id does not exist!");

            Edge edge = new Edge(nodeBase, nodeConnected, weight, isDirectional);
            Edge? e = GetEdgeExist(edge);
            if (e != null)
                return e;
            Edges.Add(edge);
            return edge;
        }

        public Edge GetEdgeByNode(Node nodeBase, Node nodeConnected) => Edges.FirstOrDefault(e => (e.NodeBase.Id == nodeBase.Id && e.NodeConnected.Id == nodeConnected.Id) || (e.IsDirectional == false && e.NodeBase.Id == nodeConnected.Id && e.NodeConnected.Id == nodeBase.Id)) ?? throw new ArgumentOutOfRangeException("No edge with the given param. could be found!");

        public void HighLight(uint nodeBase, uint nodeConnected) => HighLight(GetNodeById(nodeBase), GetNodeById(nodeConnected));
        public void HighLight(Node nodeBase, Node nodeConnected)
        {
            Edge e = GetEdgeByNode(nodeBase, nodeConnected);
            e.Highlight = true;
        }

        public void StopHighLight(uint nodeBase, uint nodeConnected) => StopHighLight(GetNodeById(nodeBase), GetNodeById(nodeConnected));
        public void StopHighLight(Node nodeBase, Node nodeConnected)
        {
            Edge e = GetEdgeByNode(nodeBase, nodeConnected);
            e.Highlight = false;
        }

        public void PropertyChange([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public Node GetNodeById(uint Id)
        {
            return Nodes.FirstOrDefault(n => n.Id == Id) ?? throw new ArgumentOutOfRangeException("A node with the given id does not exist!");
        }

        public Node GetEdgeById(uint Id)
        {
            return Nodes.FirstOrDefault(e => e.Id == Id) ?? throw new ArgumentOutOfRangeException("A edge with the given id does not exist!");
        }

        public uint[][] GetGraphAsCheckedArray()
        {
            uint[][] graph = new uint[Nodes.Count][];
            SortedDictionary<uint, HashSet<(uint, double)>> dicHsGraph = new SortedDictionary<uint, HashSet<(uint, double)>>();

            foreach (Edge edge in Edges)
            {
                if (!dicHsGraph.ContainsKey(edge.NodeBase.Id))
                    dicHsGraph.Add(edge.NodeBase.Id, new HashSet<(uint, double)>());
                dicHsGraph[edge.NodeBase.Id].Add((edge.NodeConnected.Id, edge.Weight));
                if (edge.IsDirectional == false)
                {
                    if (!dicHsGraph.ContainsKey(edge.NodeConnected.Id))
                        dicHsGraph.Add(edge.NodeConnected.Id, new HashSet<(uint, double)>());
                    dicHsGraph[edge.NodeConnected.Id].Add((edge.NodeBase.Id, edge.Weight));
                }

            }
            return graph;
        }
    }
}
