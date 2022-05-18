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

            (double, List<uint>) path = ComputeDepthSearchRec(graph, matrix, 0, 6, 0, new List<uint>());
            graph.ResetHighLight();
            HighLightPath(graph, path.Item2);
            return;
        }

        private (double, List<uint>) ComputeDepthSearchRec(Graph graph, double[,] matrix, uint nodeIdMe, uint nodeIdSearched, double cost, List<uint> path)
        {
            System.Threading.Thread.Sleep(500);
            if (path.Any(n => n == nodeIdMe))
                return (double.MaxValue, null);

            if(nodeIdSearched == nodeIdMe)
                return (cost, path);

            if(path.Count > 0)
                cost += matrix[path.Last(), nodeIdMe];

            path.Add(nodeIdMe);
            graph.ResetHighLight();
            HighLightPath(graph, path);

            (double, List<uint>) bestResult = (double.MaxValue, null);

            //Check if I have a direct connection
            if (matrix[nodeIdMe, nodeIdSearched] >= 0)
            {
                cost += matrix[nodeIdMe, nodeIdSearched];
                List<uint> _path = path.Select(e=>e).ToList();
                _path.Add(nodeIdSearched);
                bestResult =(cost, _path);
            }

            //Check if someone who i know has a better solution
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                if (i == nodeIdMe || matrix[nodeIdMe, i] < 0 || i == nodeIdSearched)
                    continue;
                (double, List<uint>) res = ComputeDepthSearchRec(graph, matrix, (uint)i, nodeIdSearched, cost, path.Select(e=>e).ToList());
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
            for(int i = 1; i < nodes.Count; i++)
            {
                graph.HighLight(nodes[i-1], nodes[i]);
            }
        }
    }
}
