﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
    public sealed class Path : IReadOnlyList<Node>
    {
        private readonly IReadOnlyList<Node> nodes;

        internal Path(IEnumerable<Node> nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            this.nodes = nodes.ToList();
        }

        public Node this[int index] => nodes[index];

        public int Count => nodes.Count;

        public IEnumerator<Node> GetEnumerator() => nodes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => nodes.GetEnumerator();

        public override bool Equals(object obj)
        {
            var path = obj as Path;

            if (!ReferenceEquals(path, null))
            {
                return Equals(path);
            }

            return base.Equals(obj);
        }

        public bool Equals(Path path)
            => !ReferenceEquals(path, null) && nodes.SequenceEqual(path.nodes);

        public override int GetHashCode()
        {
            int hash = 17;

            unchecked
            {
                foreach (var node in nodes)
                {
                    hash = hash * 23 + node.GetHashCode();
                }
            }

            return hash;
        }
    }
}