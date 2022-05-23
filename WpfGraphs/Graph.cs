using Newtonsoft.Json;
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
    [Serializable]
    public class Graph : INotifyPropertyChanged
    {
        public Graph()
        {

        }
        public ObservableCollection<Node> Nodes { get; private set; } = new ObservableCollection<Node>();
        public ObservableCollection<Edge> Edges { get; private set; } = new ObservableCollection<Edge>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public Edge? GetEdgeExist(Edge edge)
        {
            return Edges.FirstOrDefault(e => ((e.NodeBase.Id == edge.NodeBase.Id && e.NodeConnected.Id == edge.NodeConnected.Id) || (e.NodeBase.Id == edge.NodeConnected.Id && e.NodeConnected.Id == edge.NodeBase.Id && !e.IsDirectional && !edge.IsDirectional)) && e.IsDirectional == edge.IsDirectional);
        }

        public bool EdgeExist(Edge? edge)
        {
            if(edge == null)
                return false;
            return GetEdgeExist(edge)==null?false:true;
        }

        public bool EdgeExist(Node nodeBase, Node nodeConnected, bool isDirectional = true)
        {
            Edge edge = new Edge(nodeBase, nodeConnected, isDirectional);
            Edge? e = GetEdgeExist(edge);
            return EdgeExist(e);
        }
        public bool EdgeExist(Node nodeBase, Node nodeConnected, double weight, bool isDirectional = true)
        {
            Edge edge = new Edge(nodeBase, nodeConnected, weight, isDirectional);
            Edge? e = GetEdgeExist(edge);
            return EdgeExist(e);
        }

        public bool EdgeExist(uint idBase, uint idConnected, bool isDirectional = true)
        {
            Node nodeBase = Nodes.FirstOrDefault(n => n.Id == idBase) ?? throw new ArgumentOutOfRangeException("A node with the given id does not exist!");
            Node nodeConnected = Nodes.FirstOrDefault(n => n.Id == idConnected) ?? throw new ArgumentOutOfRangeException("A node with the given id does not exist!");

            Edge edge = new Edge(nodeBase, nodeConnected, isDirectional);
            Edge? e = GetEdgeExist(edge);
            return EdgeExist(e);
        }

        public bool EdgeExist(uint idBase, uint idConnected, double weight, bool isDirectional = true)
        {
            Node nodeBase = Nodes.FirstOrDefault(n => n.Id == idBase) ?? throw new ArgumentOutOfRangeException("A node with the given id does not exist!");
            Node nodeConnected = Nodes.FirstOrDefault(n => n.Id == idConnected) ?? throw new ArgumentOutOfRangeException("A node with the given id does not exist!");

            Edge edge = new Edge(nodeBase, nodeConnected, weight, isDirectional);
            Edge? e = GetEdgeExist(edge);
            return EdgeExist(e);
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

        public void RemoveEdge(Node nodeBase, Node nodeConnected, bool isDirectional = true)
        {
            Edge edge = new Edge(nodeBase, nodeConnected, isDirectional);
            Edge? e = GetEdgeExist(edge);
            if (e == null)
                return;
            Edges.Remove(e);
        }
        public void RemoveEdge(Node nodeBase, Node nodeConnected, double weight, bool isDirectional = true)
        {
            Edge edge = new Edge(nodeBase, nodeConnected, weight, isDirectional);
            Edge? e = GetEdgeExist(edge);
            if (e == null)
                return;
            Edges.Remove(e);
        }

        public void RemoveEdge(uint idBase, uint idConnected, bool isDirectional = true)
        {
            Node nodeBase = Nodes.FirstOrDefault(n => n.Id == idBase) ?? throw new ArgumentOutOfRangeException("A node with the given id does not exist!");
            Node nodeConnected = Nodes.FirstOrDefault(n => n.Id == idConnected) ?? throw new ArgumentOutOfRangeException("A node with the given id does not exist!");

            Edge edge = new Edge(nodeBase, nodeConnected, isDirectional);
            Edge? e = GetEdgeExist(edge);
            if (e == null)
                return;
            Edges.Remove(e);
        }

        public void RemoveEdge(uint idBase, uint idConnected, double weight, bool isDirectional = true)
        {
            Node nodeBase = Nodes.FirstOrDefault(n => n.Id == idBase) ?? throw new ArgumentOutOfRangeException("A node with the given id does not exist!");
            Node nodeConnected = Nodes.FirstOrDefault(n => n.Id == idConnected) ?? throw new ArgumentOutOfRangeException("A node with the given id does not exist!");

            Edge edge = new Edge(nodeBase, nodeConnected, weight, isDirectional);
            Edge? e = GetEdgeExist(edge);
            if (e == null)
                return;
            Edges.Remove(e);
        }

        public Edge GetEdgeByNode(Node nodeBase, Node nodeConnected) => Edges.FirstOrDefault(e => (e.NodeBase.Id == nodeBase.Id && e.NodeConnected.Id == nodeConnected.Id) || (e.IsDirectional == false && e.NodeBase.Id == nodeConnected.Id && e.NodeConnected.Id == nodeBase.Id)) ?? throw new ArgumentOutOfRangeException("No edge with the given param. could be found!");

        public void HighLight(uint nodeBase, uint nodeConnected) => HighLight(GetNodeById(nodeBase), GetNodeById(nodeConnected));
        public void HighLight(Node nodeBase, Node nodeConnected)
        {
            Edge e = GetEdgeByNode(nodeBase, nodeConnected);
            e.Highlight = true;
        }

        public void ResetHighLight()
        {
            foreach(Edge edge in Edges)
            {
                edge.Highlight = false;
            }
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

        public (uint, double)[][] GetGraphAsCheckedArray()
        {
            (uint, double)[][] graph = new (uint, double)[Nodes.Count][];
            SortedDictionary<uint, HashSet<(uint, double)>> dicHsGraph = new SortedDictionary<uint, HashSet<(uint, double)>>();

            foreach (Node node in Nodes)
            {
                if (!dicHsGraph.ContainsKey(node.Id))
                    dicHsGraph.Add(node.Id, new HashSet<(uint, double)>());
            }

            foreach (Edge edge in Edges)
            {
                dicHsGraph[edge.NodeBase.Id].Add((edge.NodeConnected.Id, edge.Weight));
                if (edge.IsDirectional == false)
                {
                    dicHsGraph[edge.NodeConnected.Id].Add((edge.NodeBase.Id, edge.Weight));
                }

            }

            int i = 0;
            foreach (var kvp in dicHsGraph)
            {
                List<(uint, double)> edgeList = new();
                HashSet<(uint, double)> edges = kvp.Value;
                graph[i] = edges.OrderBy(e => e.Item1).ToArray();
                i++;
            }

            return graph;
        }

        public double[,] GetGraphAsMatrix()
        {
            double[,] graph = new double[Nodes.Count, Nodes.Count];
            List<uint> nodes = Nodes.OrderBy(n => n.Id).Select(n => n.Id).ToList();

            (uint, double)[][] graphCA = GetGraphAsCheckedArray();
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    graph[i,j] = -1;
                }
            }
            for (int i = 0; i < graphCA.Length; i++)
            {
                foreach (var edge in graphCA[i])
                {
                    graph[i, nodes.IndexOf(edge.Item1)] = edge.Item2;
                }
            }
            return graph;
        }
    }
}
