(function () {
  var app = angular
    .module('angularVis', [])

    .constant('angularVisConfig', {
      networks: {},
      networkEvents: [
        'click',
        'doubleClick',
        'oncontext',
        'hold',
        'release',
        'selectNode',
        'selectEdge',
        'deselectNode',
        'deselectEdge',
        'dragStart',
        'dragging',
        'dragEnd',
        'hoverNode',
        'blurNode',
        'zoom',
        'showPopup',
        'hidePopup',
        'startStabilizing',
        'stabilizationProgress',
        'stabilizationIterationsDone',
        'stabilized',
        'resize',
        'initRedraw',
        'beforeDrawing',
        'afterDrawing',
        'animationFinished'
      ]
    })

    .factory('visHelper', ['angularVisConfig', function (angularVisConfig) {
      return {
        initGraph: function (networkName, options, events) {
          angularVisConfig.networks[networkName].setOptions(options);

          angular.forEach(events, function (callback, event) {
            if (angularVisConfig.networkEvents.indexOf(String(event)) >= 0) {
              angularVisConfig.networks[networkName].on(event, callback);
            }
          });

          if (events != null && events.onload != null && angular.isFunction(events.onload)) {
            events.onload(angularVisConfig.networks[networkName]);
          }
        },

        setNodes: function (networkName, nodes) {
          angularVisConfig.networks[networkName].body.data.nodes.clear();
          angularVisConfig.networks[networkName].body.data.nodes.add(nodes);
          angularVisConfig.networks[networkName].setData({
            nodes: angularVisConfig.networks[networkName].body.data.nodes,
            edges: angularVisConfig.networks[networkName].body.data.edges
          });
        },

        setEdges: function (networkName, edges) {
          angularVisConfig.networks[networkName].body.data.edges.clear();
          angularVisConfig.networks[networkName].body.data.edges.add(edges);
          angularVisConfig.networks[networkName].setData({
            nodes: angularVisConfig.networks[networkName].body.data.nodes,
            edges: angularVisConfig.networks[networkName].body.data.edges
          });
        },

        highlightPath: function (networkName, pathNodeIds, options) {
          var nodes = angularVisConfig.networks[networkName].body.data.nodes.map(function (n) {
            var color = angular.copy(n.color);

            if (pathNodeIds.indexOf(n.id) != -1) {
              color.border = options.highlighted.color.border.slice();
              color.background = options.highlighted.color.background.slice();
            }
            else {
              color.border = options.normal.color.border.slice();
              color.background = options.normal.color.background.slice();
            }

            n.color = color;

            return n;
          });

          var edges = angularVisConfig.networks[networkName].body.data.edges.map(function (e) {
            var indexOfFrom = pathNodeIds.indexOf(e.from);
            var indexOfTo = pathNodeIds.indexOf(e.to);
            var color = angular.copy(e.color);

            if ((indexOfFrom != -1 && indexOfTo != -1)
                    && (indexOfTo == indexOfFrom + 1
                        || indexOfTo == indexOfFrom - 1)) {
              color.color = options.highlighted.color.border.slice();
              e.width = options.highlighted.width;
            }
            else {
              color.color = options.normal.color.border.slice();
              e.width = options.normal.width;
            }

            e.color = color;

            return e;
          });

          angularVisConfig.networks[networkName].setData({
            nodes: nodes,
            edges: edges
          });
        },

        setSelectedNodes: function (networkName, selectedNodeIds) {
          angularVisConfig.networks[networkName].setSelection({ nodes: selectedNodeIds });
        },

        redraw: function (networkName) {
          angularVisConfig.networks[networkName].redraw();
        }
      };
    }])

    .directive('visNetwork', ['angularVisConfig', function (angularVisConfig) {
      return {
        restrict: 'EA',
        transclude: false,
        scope: {
          name: '@'
        },
        link: function (scope, element, attr) {
          scope.$watch('name', function () {
            if (!angularVisConfig.networks.hasOwnProperty(scope.name) || !angularVisConfig.networks[scope.name]) {
              angularVisConfig.networks[scope.name] = new vis.Network(element[0]);
            }
          });
        }
      };
    }])
})()