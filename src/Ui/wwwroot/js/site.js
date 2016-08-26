(function () {
  var app = angular
    .module('app', ['ngMessages', 'angularVis'])

    .constant('config', {
      graphName: 'main',
      defaultGraphUrl: 'http://localhost:5001/api/v1/graphs/test1',
      shortestPathEndpoint: '/shortest-path'
    })

    constant('')

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

        visHelper.setOptions(
          config.graphName,
          {
            interaction: {
              hover: true,
              multiselect: true,
              selectConnectedEdges: false,
              navigationButtons: true
            }
          });

        visHelper.setEvents(
          config.graphName, 
          {
            selectNode: function (event) {
              $scope.$apply(function () { $rootScope.$broadcast('nodeSelected', { selectedNodeIds: event.nodes }) });
            },
            deselectNode: function (event) {
              $scope.$apply(function () { $rootScope.$broadcast('nodeUnselected', { selectedNodeIds: event.nodes }) });
            }
          });

        $scope.$on('graphSelected', function (event, args) {
          graphApi.getGraph(args.graphUrl)
            .then(self.loadGraph, self.graphError);
        });

        self.loadGraph = function (response) {
          var graph = response.data;

          visHelper.setNodes(config.graphName, graph.nodes);
          visHelper.setEdges(config.graphName, graph.edges);
          visHelper.redraw();

          $rootScope.$broadcast('graphLoaded', { graph: graph });
        };

        self.graphError = function () {
          $rootScope.$broadcast('error', { getGraphApiError: true });
        };

        $scope.$on('pathFound', function (event, args) {
          visHelper.highlightPath(config.graphName, args.pathNodeIds);
          visHelper.redraw();
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