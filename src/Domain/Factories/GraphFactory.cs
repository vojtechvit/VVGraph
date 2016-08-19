using Domain.Algorithms.Contracts;
using Domain.Exceptions;
using Domain.Factories.Contracts;
using Domain.Model;
using System;
using System.Text.RegularExpressions;

namespace Domain.Factories
{
    public sealed class GraphFactory : IGraphFactory
    {
        private readonly IPathFinder _pathFinder;

        public GraphFactory(IPathFinder pathFinder)
        {
            if (pathFinder == null)
            {
                throw new ArgumentNullException(nameof(pathFinder));
            }

            _pathFinder = pathFinder;
        }

        public Graph Create(string name)
        {
            if (name == null)
            {
                throw new ModelValidationException("Graph name is required.");
            }

            if (name.Length <= 0)
            {
                throw new ModelValidationException("Graph name must be at least 1 character long.");
            }

            if (!Regex.IsMatch(name, "[a-z0-9]"))
            {
                throw new ModelValidationException("Graph name may contain only lowercase alphanumeric characters.");
            }

            return new Graph(name, _pathFinder);
        }
    }
}