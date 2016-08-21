namespace WebServices.DataAccess.Neo4j.Model
{
    public sealed class Edge
    {
        public int StartNodeId { get; set; }

        public int EndNodeId { get; set; }
    }
}