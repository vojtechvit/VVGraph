(function () {
    var app = angular.module('app', []);

    app.controller('HomeController', ['$http', function ($http) {
        var home = this;

        // Load graph
        this.graph = null;

        $http.get('http://localhost:60832/api/v1/graphs/test1')
            .success(function (data) {
                home.graph = data;
                home.refreshGraph();
            })
            .error(function (response) {
                home.errorMessage = 'Could not load graph information. Response code returned: ' + response.status;
            });

        // Shortest path
        this.startNodeId = 0;
        this.endNodeId = 0;
        this.shortestPath = [2, 3, 4];

        this.canFindShortestPath = false;

        this.findShortestPath = function () {
            var url = 'http://localhost:60832/api/v1/graphs/test1/shortest-path';
            var queryParams = { startNodeId: home.startNodeId, endNodeId: home.endNodeId };

            $http.get(url, queryParams)
                .success(function (data) {
                    home.shortestPath = data;
                    this.refreshGraph();
                })
                .error(function (response) {
                    home.errorMessage = 'Could not the load shortest path. Response code returned: ' + response.status;
                });
        };

        // Errors
        this.errorMessage = null;

        // Visualize graph
        this.refreshGraph = function () {
            var nodes = new vis.DataSet();
            home.graph.nodes.forEach(function (n) {
                if (home.shortestPath.indexOf(n.id) != -1) {
                    var border = '#32a800';
                    var background = '#e6ffdb';
                }
                else {
                    var border = '#000000';
                    var background = '#ffffff';
                }

                nodes.add([{
                    id: n.id,
                    label: n.label,
                    shape: 'circle',
                    color: {
                        border: border,
                        background: background,
                        hover: {
                            border: '#000000',
                            background: '#dddddd'
                        }
                    }
                }]);
            });

            var edges = new vis.DataSet();

            home.graph.edges.forEach(function (e) {
                var indexOfStartNodeId = home.shortestPath.indexOf(e.startNodeId);
                var indexOfEndNodeId = home.shortestPath.indexOf(e.endNodeId);

                if (indexOfStartNodeId != -1 && indexOfEndNodeId != -1
                && !(indexOfStartNodeId == 0 && indexOfEndNodeId == home.shortestPath[home.shortestPath.length - 1])) {
                    var color = '#32a800';
                }
                else {
                    var color = '#000000';
                }

                edges.add([{
                    from: e.startNodeId,
                    to: e.endNodeId,
                    color: {
                        color: color,
                        inherit: false
                    }
                }]);
            });

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

            this.network = new vis.Network(container, data, options);
        }
    }]);
})();