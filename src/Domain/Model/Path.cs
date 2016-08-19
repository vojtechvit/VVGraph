using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
    public sealed class Path : IReadOnlyList<Node>
    {
        private readonly IReadOnlyList<Node> _path;

        internal Path(IReadOnlyList<Node> path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            _path = path;
        }

        public Node this[int index] => _path[index];

        public int Count => _path.Count;

        public IEnumerator<Node> GetEnumerator() => _path.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _path.GetEnumerator();

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
            => !ReferenceEquals(path, null) && _path.SequenceEqual(path._path);

        public override int GetHashCode()
        {
            int hash = 17;

            unchecked
            {
                foreach (Node node in _path)
                {
                    hash = hash * 23 + node.GetHashCode();
                }
            }

            return hash;
        }
    }
}