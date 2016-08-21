(function () {
    var app = angular
        .module('app', [])

        .controller('HomeController',
            ['$scope', '$http', 'nodesToDataSet', 'edgesToDataSet', 'createGraph', 'refreshGraph',
            function ($scope, $http, nodesToDataSet, edgesToDataSet, createGraph, refreshGraph) {
                // Errors
                $scope.errorMessage = null;

                // Graph
                $scope.graphUrl = 'http://localhost:60832/api/v1/graphs/test1';
                $scope.graphName = null;
                $scope.nodes = null;
                $scope.edges = null;
                $scope.network = null;

                $scope.changeGraph = function () {
                    $http.get($scope.graphUrl)
                        .success(function (data) {
                            $scope.graphName = data.name
                            $scope.nodes = nodesToDataSet(data.nodes);
                            $scope.edges = edgesToDataSet(data.edges);
                            $scope.network = createGraph($scope.nodes, $scope.edges);
                            attachEvents($scope.network);
                        })
                        .error(function (data) {
                            $scope.errorMessage = 'Could not load the graph information.';
                        });

                    $scope.startNodeId = null;
                    $scope.endNodeId = null;
                };

                // Load graph
                $scope.changeGraph();

                // Shortest path
                $scope.startNodeId = null;
                $scope.endNodeId = null;

                $scope.findShortestPath = function () {
                    $http({
                        url: $scope.graphUrl.trim('/') + '/shortest-path',
                        method: 'GET',
                        params: { startNodeId: $scope.startNodeId, endNodeId: $scope.endNodeId }
                    }).then(function (response) {
                        refreshGraph($scope.network, $scope.nodes, $scope.edges, response.data);
                        $scope.network.unselectAll();
                        $scope.startNodeId = null;
                        $scope.endNodeId = null;
                    }, function (response) {
                        $scope.errorMessage = 'Could not load the load shortest path.';
                    });
                };

                function attachEvents(network) {
                    network.on("selectNode", function (event) {
                        if (event.nodes.length == 2) {
                            $scope.startNodeId = event.nodes[0];
                            $scope.endNodeId = event.nodes[1];
                            $scope.$apply();
                        }
                        else if (event.nodes.length > 2) {
                            network.unselectAll();
                            network.selectNodes([$scope.startNodeId, $scope.endNodeId]);
                        }
                    });

                    network.on("deselectNode", function (event) {
                        if (event.nodes.length < 2) {
                            $scope.startNodeId = null;
                            $scope.endNodeId = null;
                            $scope.$apply();
                        }
                    });
                }
            }])

        .factory("nodesToDataSet", [function () {
            return function (nodes) {
                var dsNodes = new vis.DataSet();
                nodes.forEach(function (n) {
                    dsNodes.add([{
                        id: n.id,
                        label: n.label,
                        shape: 'circle',
                        color: {
                            border: '#000000',
                            background: '#ffffff',
                            hover: {
                                border: '#000000',
                                background: '#dddddd'
                            }
                        }
                    }]);
                });

                return dsNodes;
            }
        }])

        .factory("edgesToDataSet", [function () {
            return function (edges) {
                var dsEdges = new vis.DataSet();

                edges.forEach(function (e) {
                    dsEdges.add([{
                        from: e.startNodeId,
                        to: e.endNodeId,
                        color: {
                            color: '#000000',
                            inherit: false
                        },
                        width: 1
                    }]);
                });

                return dsEdges;
            }
        }])

        .factory("createGraph", [function () {
            return function (nodes, edges) {
                container = document.getElementById('graph-container');

                var data = {
                    nodes: nodes,
                    edges: edges
                };

                var options = {
                    interaction: {
                        hover: true,
                        multiselect: true,
                        selectConnectedEdges: false,
                        navigationButtons: true
                    }
                };

                return new vis.Network(container, data, options);
            }
        }])

        .factory("refreshGraph", [function () {
            return function (network, nodes, edges, shortestPath) {
                nodes.forEach(function (n) {
                    if (shortestPath.indexOf(n.id) != -1) {
                        var border = '#32a800';
                        var background = '#e6ffdb';
                    }
                    else {
                        var border = '#000000';
                        var background = '#ffffff';
                    }

                    n.color.border = border;
                    n.color.background = background;
                });

                edges.forEach(function (e) {
                    var indexOfFrom = shortestPath.indexOf(e.from);
                    var indexOfTo = shortestPath.indexOf(e.to);

                    if ((indexOfFrom != -1 && indexOfTo != -1)
                            && (indexOfTo == indexOfFrom + 1
                                || indexOfTo == indexOfFrom - 1)) {
                        var color = '#32a800';
                        var width = 2;
                    }
                    else {
                        var color = '#000000';
                        var width = 1;
                    }

                    e.color.color = color;
                    e.width = width;
                });

                network.setData({ nodes: nodes, edges: edges });
                network.redraw();
            }
        }]);
})();