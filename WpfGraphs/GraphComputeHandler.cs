using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfGraphs
{
    public class GraphComputeHandler
    {
        private GraphComputeHandler() { }
        public static GraphComputeHandler Instance { get; private set; } = new GraphComputeHandler();

        private List<IGraphCompute> graphComputes = new List<IGraphCompute>();

        public void Compute(Graph graph)
        {
            Thread thread = new Thread(() =>
            {
                foreach (IGraphCompute handler in graphComputes)
                {
                    handler.Compute(graph);
                }
            });
            thread.Start();
        }

        public void Register(IGraphCompute graphCompute)
        {
            graphComputes.Add(graphCompute);
        }

        public void OnStart()
        {
            _ = new ExampleGraphCompute();
        }
    }
}
