(function () {
  var app = angular
    .module('app', ['ngMessages', 'angularVis'])

    .constant('config', {
      graphName: 'main',
      defaultGraphUrl: 'http://localhost:5001/api/v1/graphs/test1',
      shortestPathEndpoint: '/shortest-path',
      nodeOptions: {
        shape: 'circle',
        color: {
          border: '#000000',
          background: '#ffffff',
          hover: {
            border: '#000000',
            background: '#dddddd'
          }
        }
      },
      edgeOptions: {
        color: {
          color: '#000000',
          inherit: false
        },
        width: 1
      }
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
      ['$rootScope', '$scope', 'config', 'graphApi', 'visHelper',
      function ($rootScope, $scope, config, graphApi, visHelper) {
        var self = this;

        $scope.$on('graphSelected', function (event, args) {
          graphApi.getGraph(args.graphUrl)
            .then(self.loadGraph, self.graphError);
        });

        self.loadGraph = function (response) {
          visHelper.initGraph(
          config.graphName,
          {
            interaction: {
              hover: true,
              multiselect: true,
              selectConnectedEdges: false,
              navigationButtons: true
            }
          },
          {
            selectNode: function (event) {
              $scope.$apply(function () { $rootScope.$broadcast('nodeSelected', { selectedNodeIds: event.nodes }) });
            },
            deselectNode: function (event) {
              $scope.$apply(function () { $rootScope.$broadcast('nodeUnselected', { selectedNodeIds: event.nodes }) });
            }
          });

          var graph = response.data;
          
          visHelper.setNodes(config.graphName, graph.nodes.map(function (n) {
            angular.extend(n, config.nodeOptions);
            return n;
          }))

          visHelper.setEdges(config.graphName, graph.edges.map(function (e) {
            var edge = { from: e.startNodeId, to: e.endNodeId };
            angular.extend(edge, config.edgeOptions);
            return edge;
          }));

          visHelper.redraw(config.graphName);

          $rootScope.$broadcast('graphLoaded', { graph: graph });
        };

        self.graphError = function () {
          $rootScope.$broadcast('error', { getGraphApiError: true });
        };

        $scope.$on('pathFound', function (event, args) {
          var higlightOptions = {
            normal: {
              color: {
                border: '#000000',
                background: '#ffffff'
              },
              width: 1
            },
            highlighted: {
              color: {
                border: '#32a800',
                background: '#e6ffdb'
              },
              width: 2
            }
          };

          visHelper.highlightPath(config.graphName, args.pathNodeIds, higlightOptions);
          visHelper.redraw(config.graphName);
        });
      }])

    .controller('GraphActionsController',
      ['$rootScope', '$scope', 'config', 'graphApi', 'visHelper',
      function ($rootScope, $scope, config, graphApi, visHelper) {
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
          if (args.selectedNodeIds.length > 2) {
            visHelper.setSelectedNodes(config.graphName, self.selectedNodeIds);
          }
          else {
            self.selectedNodeIds = args.selectedNodeIds.slice();
          }
        });

        $scope.$on('nodeUnselected', function (event, args) {
          self.selectedNodeIds = args.selectedNodeIds.slice();
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
})();