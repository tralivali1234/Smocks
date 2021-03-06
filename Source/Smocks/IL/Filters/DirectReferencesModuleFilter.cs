#region License
//// The MIT License (MIT)
////
//// Copyright (c) 2015 Tom van der Kleij
////
//// Permission is hereby granted, free of charge, to any person obtaining a copy of
//// this software and associated documentation files (the "Software"), to deal in
//// the Software without restriction, including without limitation the rights to
//// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//// the Software, and to permit persons to whom the Software is furnished to do so,
//// subject to the following conditions:
////
//// The above copyright notice and this permission notice shall be included in all
//// copies or substantial portions of the Software.
////
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion License

using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Smocks.IL.Dependencies;
using Smocks.Utility;

namespace Smocks.IL.Filters
{
    internal class DirectReferencesModuleFilter : IModuleFilter
    {
        private readonly IEqualityComparer<ModuleReference> _moduleComparer;
        private IDependencyGraph _dependencyGraph;

        internal DirectReferencesModuleFilter(
            IDependencyGraph dependencyGraph,
            IEqualityComparer<ModuleReference> moduleComparer)
        {
            ArgumentChecker.NotNull(dependencyGraph, () => dependencyGraph);
            ArgumentChecker.NotNull(moduleComparer, () => moduleComparer);

            _dependencyGraph = dependencyGraph;
            _moduleComparer = moduleComparer;
        }

        public bool Accepts(ModuleDefinition module)
        {
            return _moduleComparer.Equals(module, _dependencyGraph.Module) ||
                _dependencyGraph.Nodes.Any(node => _moduleComparer.Equals(node.Module, module));
        }

        public void Dispose()
        {
            _dependencyGraph = null;
        }
    }
}