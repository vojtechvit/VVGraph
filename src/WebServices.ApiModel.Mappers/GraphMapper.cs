using Domain.Factories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using WebServices.ApiModel.Mappers.Contracts;

namespace WebServices.ApiModel.Mappers
{
    public sealed class GraphMapper : IGraphMapper
    {
        private readonly IGraphFactory graphFactory;
        private readonly INodeMapper nodeMapper;

        public GraphMapper(
            IGraphFactory graphFactory,
            INodeMapper nodeMapper)
        {
            if (graphFactory == null)
                throw new ArgumentNullException(nameof(graphFactory));

            if (nodeMapper == null)
                throw new ArgumentNullException(nameof(nodeMapper));

            this.graphFactory = graphFactory;
            this.nodeMapper = nodeMapper;
        }

        public Domain.Model.Graph Map(Graph graph)
            => graphFactory.Create(graph.Name);

        public Graph Map(Domain.Model.Graph graph, IEnumerable<Domain.Model.Node> nodes)
            => new Graph
            {
                Name = graph.Name,
                Nodes = nodes.Select(nodeMapper.Map),
                Edges = nodeMapper.GetEdges(nodes)
            };
    }
}