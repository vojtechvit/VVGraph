using Domain.Factories.Contracts;
using System;
using System.Linq;
using WebServices.ApiModel.Mappers.Contracts;

namespace WebServices.ApiModel.Mappers
{
    public sealed class ApiModelGraphMapper : IApiModelGraphMapper
    {
        private readonly IGraphFactory graphFactory;
        private readonly IApiModelNodeMapper nodeMapper;
        private readonly IApiModelEdgeMapper edgeMapper;

        public ApiModelGraphMapper(
            IGraphFactory graphFactory,
            IApiModelNodeMapper nodeMapper,
            IApiModelEdgeMapper edgeMapper)
        {
            if (graphFactory == null)
                throw new ArgumentNullException(nameof(graphFactory));

            if (nodeMapper == null)
                throw new ArgumentNullException(nameof(nodeMapper));

            if (edgeMapper == null)
                throw new ArgumentNullException(nameof(edgeMapper));

            this.graphFactory = graphFactory;
            this.nodeMapper = nodeMapper;
            this.edgeMapper = edgeMapper;
        }

        public Domain.Model.Graph Map(Graph graph)
        {
            var domainGraph = graphFactory.Create(graph.Name);

            foreach (var node in graph.Nodes)
            {
                domainGraph.AddNode(node.Id, node.Label);
            }

            foreach (var edge in graph.Edges)
            {
                var startNode = domainGraph.Nodes[edge.StartNodeId];
                var endNode = domainGraph.Nodes[edge.EndNodeId];

                domainGraph.AddEdge(startNode, endNode);
            }

            return domainGraph;
        }

        public Graph Map(Domain.Model.Graph graph)
            => new Graph
            {
                Name = graph.Name,
                Nodes = graph.Nodes.Values.Select(nodeMapper.Map),
                Edges = graph.Edges.Select(edgeMapper.Map)
            };
    }
}