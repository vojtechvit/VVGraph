using Domain.Factories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using WebServices.DataAccess.Neo4j.Mappers.Contracts;

namespace WebServices.DataAccess.Neo4j.Mappers
{
    public sealed class Neo4jGraphMapper : INeo4jGraphMapper
    {
        private readonly IGraphFactory graphFactory;

        public Neo4jGraphMapper(
            IGraphFactory graphFactory)
        {
            if (graphFactory == null)
                throw new ArgumentNullException(nameof(graphFactory));

            this.graphFactory = graphFactory;
        }

        public Domain.Model.Graph Map(Model.Graph graph, IEnumerable<Model.Node> nodes, IEnumerable<Model.Edge> edges)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            if (edges == null)
                throw new ArgumentNullException(nameof(edges));

            var domainGraph = graphFactory.Create(graph.Name);

            foreach (var node in nodes)
            {
                domainGraph.AddNode(node.Id, node.Label);
            }

            foreach (var edge in edges)
            {
                domainGraph.AddEdge(domainGraph.Nodes[edge.StartNodeId], domainGraph.Nodes[edge.EndNodeId]);
            }

            return domainGraph;
        }

        public Model.Graph MapGraph(Domain.Model.Graph graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            return new Model.Graph { Name = graph.Name };
        }

        public IEnumerable<Model.Node> MapNodes(Domain.Model.Graph graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            return graph.Nodes.Values.Select(n => new Model.Node { Id = n.Id, Label = n.Label });
        }
    }
}