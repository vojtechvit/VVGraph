using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace WebServices.AspNetCore.IntegrationTests.Graphs.SampleData
{
    public sealed class AllValidGraphsAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
            => ValidGraphFactory.CreateValidGraphs().Select(g => new[] { g });
    }
}