using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfGraphs
{
    internal class ExampleGraphCompute : IGraphCompute
    {
        public ExampleGraphCompute()
        {
            GraphComputeHandler.Instance.Register(this);
        }
        public void Compute(Graph graph)
        {
            var ca = graph.GetGraphAsCheckedArray();
            var matrix = graph.GetGraphAsMatrix();

            //(double, List<uint>) path = ComputeDepthSearchRec(graph, matrix, 0, 16, 0, new List<uint>());
            //graph.ResetHighLight();
            //HighLightPath(graph, path.Item2);

            //List<uint> path = ComputeDepthSearch(graph, ca, 0, 16);
            //graph.ResetHighLight();
            //HighLightPath(graph, path);

            List<uint> path = ComputeWidthSearch(graph, matrix, 0, 16);
            graph.ResetHighLight();
            HighLightPath(graph, path);

            return;
        }

        private List<uint> ComputeWidthSearch(Graph graph, double[,] matrix, uint nodeIdMe, uint nodeIdSearched)
        {
            throw new NotImplementedException();
        }


        private List<uint> ComputeSearchBuildPath(List<(uint, uint?)> depthSearch, uint nodeIdSearched)
        {
            List<uint> path = new();
            path.Add(nodeIdSearched);
            for (int j = depthSearch.Count() - 1; j >= 1; j--)
            {
                if (depthSearch[j].Item2 == null)
                    break;
                if (depthSearch[j].Item1 == path.Last())
                {
                    path.Add(depthSearch[j].Item2 ?? throw new NullReferenceException());
                }
            }
            path.Reverse();
            return path;
        }

        private List<uint> ComputeDepthSearch(Graph graph, (uint, double)[][] ca, uint nodeIdMe, uint nodeIdSearched)
        {
            List<(uint, uint?)> depthSearch = new List<(uint, uint?)>();
            depthSearch.Add((nodeIdMe, null));
            List<uint> bestPath = new();
            double bestCost = Double.MaxValue;
            for (int i = 0; true; i++)
            {
                if (depthSearch.Count <= i)
                    break;
                foreach (var nodeId in ca[depthSearch[i].Item1])
                {
                    System.Threading.Thread.Sleep(10);
                    if (nodeId.Item1 == nodeIdSearched)
                    {
                        depthSearch.Add((nodeIdSearched, depthSearch[i].Item1));
                        List<uint> p = ComputeSearchBuildPath(depthSearch, nodeIdSearched);
                        double cost = 0;
                        for (int j = 0; j < p.Count - 1; j++)
                        {
                            cost += ca[p[j]].Where(n => n.Item1 == p[j + 1]).Select(n => n.Item2).First();
                        }
                        if (cost < bestCost)
                        {
                            bestCost = cost;
                            bestPath = p;
                        }
                        //depthSearch.RemoveRange(1, i);
                        //i = 0;
                    }
                    /*else*/
                    if (depthSearch[i].Item2 != nodeId.Item1 && depthSearch[i].Item1 != depthSearch[i].Item2)
                    {
                        depthSearch.Insert(i + 1, (nodeId.Item1, depthSearch[i].Item1));
                        List<uint> path = ComputeSearchBuildPath(depthSearch, nodeId.Item1);
                        graph.ResetHighLight();
                        HighLightPath(graph, path);
                        List<int> count = path.Select(p => path.Count(p_ => p_ == p)).ToList();
                        if (path.Select(p => path.Count(p_ => p_ == p)).Any(c => c > 1))
                        {
                            depthSearch.RemoveAt(i + 1);
                        }
                    }
                }

            }
            return bestPath;
        }

        private (double, List<uint>) ComputeDepthSearchRec(Graph graph, double[,] matrix, uint nodeIdMe, uint nodeIdSearched, double cost, List<uint> path)
        {
            System.Threading.Thread.Sleep(500);
            if (path.Any(n => n == nodeIdMe))
                return (double.MaxValue, null);

            if (nodeIdSearched == nodeIdMe)
                return (cost, path);

            if (path.Count > 0)
                cost += matrix[path.Last(), nodeIdMe];

            path.Add(nodeIdMe);
            graph.ResetHighLight();
            HighLightPath(graph, path);

            (double, List<uint>) bestResult = (double.MaxValue, null);

            //Check if I have a direct connection
            if (matrix[nodeIdMe, nodeIdSearched] >= 0)
            {
                cost += matrix[nodeIdMe, nodeIdSearched];
                List<uint> _path = path.Select(e => e).ToList();
                _path.Add(nodeIdSearched);
                bestResult = (cost, _path);
            }

            //Check if someone who i know has a better solution
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                if (i == nodeIdMe || matrix[nodeIdMe, i] < 0 || i == nodeIdSearched)
                    continue;
                (double, List<uint>) res = ComputeDepthSearchRec(graph, matrix, (uint)i, nodeIdSearched, cost, path.Select(e => e).ToList());
                if (res.Item1 < bestResult.Item1)
                {
                    bestResult = res;
                }
            }

            if (bestResult.Item2 != null)
            {
                graph.ResetHighLight();
                HighLightPath(graph, bestResult.Item2);
            }

            return bestResult;
        }

        public void HighLightPath(Graph graph, List<uint> nodes)
        {
            if (nodes == null)
                return;
            for (int i = 1; i < nodes.Count; i++)
            {
                graph.HighLight(nodes[i - 1], nodes[i]);
            }
        }
    }
}
