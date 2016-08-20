using Domain.Factories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using WebServices.ApiModel.Mappers.Contracts;

namespace WebServices.ApiModel.Mappers
{
    public sealed class NodeMapper : INodeMapper
    {
        private readonly INodeFactory nodeFactory;

        public NodeMapper(
            INodeFactory nodeFactory)
        {
            if (nodeFactory == null)
                throw new ArgumentNullException(nameof(nodeFactory));

            this.nodeFactory = nodeFactory;
        }

        public IEnumerable<Domain.Model.Node> GetNodes(Graph graph)
            => graph.Nodes.Select(node =>
            {
                var adjacentNodes = new HashSet<int>(
                        graph.Edges
                            .Where(e => e.EndNodeId == node.Id)
                            .Select(e => e.StartNodeId)
                            .Union(graph.Edges
                                .Where(e => e.StartNodeId == node.Id)
                                .Select(e => e.EndNodeId)));

                return nodeFactory.Create(graph.Name, node.Id, node.Label, adjacentNodes);
            });

        public Node Map(Domain.Model.Node node)
            => new Node { Id = node.Id, Label = node.Label };

        public IEnumerable<Edge> GetEdges(IEnumerable<Domain.Model.Node> nodes)
            => new HashSet<Domain.Model.Edge>(nodes.SelectMany(node => node.Edges))
                .Select(edge => new Edge { StartNodeId = edge.StartNode.NodeId, EndNodeId = edge.EndNode.NodeId });
    }
}