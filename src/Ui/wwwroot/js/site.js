(function () {
  var app = angular
    .module('app', ['ngMessages', 'ngVis'])

    .constant('config', {
      defaultGraphUrl: 'http://localhost:5001/api/v1/graphs/test1',
      shortestPathEndpoint: '/shortest-path'
    })

    .controller('HomeController',
      ['$scope',
      function ($scope) {
        var self = this;

        self.graph = null;

        $scope.$on('graphSelected', function () {
          self.graph = null;
        });

        $scope.$on('graphLoaded', function (event, args) {
          self.graph = args.graph;
        });
      }])

    .controller('GraphSelectorController',
      ['$rootScope', 'config',
      function ($rootScope, config) {
        var self = this;

        self.graphUrl = config.defaultGraphUrl;

        self.selectGraph = function () {
          $rootScope.$broadcast('graphSelected', { graphUrl: self.graphUrl });
        }
      }])

    .controller('ErrorController',
      ['$scope',
      function ($scope) {
        var self = this;

        self.error = null;

        $scope.$on('error', function (event, args) {
          self.error = args;
        });

        $scope.$on('graphSelected', function () {
          self.error = null;
        });
      }])

    .controller('GraphController',
      ['$rootScope', '$scope', 'graphApi', 'visDataSetHelper',
      function ($rootScope, $scope, graphApi, visDataSetHelper) {
        var self = this;

        self.graphData = {};

        self.graphOptions = {
          interaction: {
            hover: true,
            multiselect: true,
            selectConnectedEdges: false,
            navigationButtons: true
          }
        };

        self.graphEvents = {
          selectNode: function (event) {
            $rootScope.$broadcast('nodeSelected', { selectedNodeIds: event.nodes });
          },
          deselectNode: function (event) {
            $rootScope.$broadcast('nodeUnselected', { selectedNodeIds: event.nodes });
          }};

        $scope.$on('graphSelected', function (event, args) {
          graphApi.getGraph(args.graphUrl)
            .then(self.loadGraph, self.graphError);
        });

        self.loadGraph = function (response) {
          var graph = response.data;
          var nodes = visDataSetHelper.mapNodes(graph.nodes);
          var edges = visDataSetHelper.mapEdges(graph.edges);

          self.graphData = {
            nodes: nodes,
            edges: edges
          };

          $rootScope.$broadcast('graphLoaded', { graph: graph });
        };

        self.graphError = function () {
          $rootScope.$broadcast('error', { getGraphApiError: true });
        };

        $scope.$on('pathFound', function (event, args) {
          visDataSetHelper.highlightPath(self.graphData, args.pathNodeIds);
          self.graphData = { nodes: self.graphData.nodes, edges: self.graphData.edges };
        });
      }])

    .controller('GraphActionsController',
      ['$rootScope', '$scope', 'graphApi',
      function ($rootScope, $scope, graphApi) {
        var self = this;

        self.graphUrl = null;
        self.selectedNodeIds = [];

        self.findShortestPath = function () {
          graphApi.findShortestPath(self.graphUrl, self.selectedNodeIds[0], self.selectedNodeIds[1])
            .then(self.shortestPathFound, self.shortestPathError);
        }

        self.shortestPathFound = function (response) {
          self.selectedNodeIds = [];
          $rootScope.$broadcast('pathFound', { pathNodeIds: response.data });
        }

        self.shortestPathError = function () {
          $rootScope.$broadcast('error', { shortestPathApiError: true });
        }

        $scope.$on('graphSelected', function (event, args) {
          self.graphUrl = args.graphUrl;
          self.selectedNodeIds = [];
        });

        $scope.$on('nodeSelected', function (event, args) {
          /*if (args.selectedNodeIds.length > 2) {
            //$rootScope.$broadcast('setGraphNodes', { nodeIds: self.selectedNodeIds });
            self.selectedNodeIds = args.selectedNodeIds.slice();
          }
          else {
            self.selectedNodeIds = args.selectedNodeIds.slice();
          }*/
          $scope.$apply(function () { self.selectedNodeIds = args.selectedNodeIds.slice(); });
        });

        $scope.$on('nodeUnselected', function (event, args) {
          $scope.$apply(function () { self.selectedNodeIds = args.selectedNodeIds.slice(); });
        });
      }])

    .factory('graphApi',
      ['$http', 'config',
      function ($http, config) {
        return {
          getGraph: function (graphUrl) {
            return $http({
              url: graphUrl,
              method: 'GET'
            })
          },
          findShortestPath: function (graphUrl, startNodeId, endNodeId) {
            return $http({
              url: graphUrl.trim('/') + config.shortestPathEndpoint,
              method: 'GET',
              params: { startNodeId: startNodeId, endNodeId: endNodeId }
            })
          }
        }}])

    .factory('visDataSetHelper',
    [
    function () {
      return {
        mapNodes: function (nodes) {
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
        },
        mapEdges: function (edges) {
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
        },
        highlightPath: function(data, pathNodeIds) {
          data.nodes.forEach(function (n) {
            if (pathNodeIds.indexOf(n.id) != -1) {
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

          data.edges.forEach(function (e) {
            var indexOfFrom = pathNodeIds.indexOf(e.from);
            var indexOfTo = pathNodeIds.indexOf(e.to);

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

          return data;
        }
      }}])
})();