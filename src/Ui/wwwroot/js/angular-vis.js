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

    .factory('visHelper',
    ['angularVisConfig',
    function (angularVisConfig) {
      return {
        setNodes: function (networkName, nodes) {
          angularVisConfig.networks[networkName].body.data.nodes.clear();

          nodes.forEach(function (n) {
            angularVisConfig.networks[networkName].body.data.nodes.add([{
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

          angularVisConfig.networks[networkName].setData(angularVisConfig.networks[networkName].body.data);
        },

        setEdges: function (networkName, edges) {
          angularVisConfig.networks[networkName].body.data.edges.clear();

          angularVisConfig.networks[networkName].body.data.edges.forEach(function (e) {
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

          angularVisConfig.networks[networkName].setData(angularVisConfig.networks[networkName].body.data);
        },

        highlightPath: function (networkName, pathNodeIds) {
          angularVisConfig.networks[networkName].body.data.nodes.forEach(function (n) {
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

            angularVisConfig.networks[networkName].setData(angularVisConfig.networks[networkName].body.data);
          });

          angularVisConfig.networks[networkName].body.data.edges.forEach(function (e) {
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

            angularVisConfig.networks[networkName].setData(angularVisConfig.networks[networkName].body.data);
          });
        },

        setSelectedNodes: function (networkName, selectedNodeIds) {
          angularVisConfig.networks[networkName].setSelection({ nodes: selectedNodeIds });
        },

        setOptions: function (networkName, options) {
          angularVisConfig.networks[networkName].setOptions(options);
        },

        setEvents: function(networkName, events) {
          angular.forEach(events, function (callback, event) {
            if (angularVisConfig.networkEvents.indexOf(String(event)) >= 0) {
              angularVisConfig.networks[networkName].on(event, callback);
            }
          });

          if (events != null && events.onload != null && angular.isFunction(events.onload)) {
            events.onload(angularVisConfig.networks[networkName]);
          }
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
          name: '='
        },
        link: function (scope, element, attr) {
          if (!angularVisConfig.networks[scope.name]) {
            angularVisConfig.networks[scope.name] = new vis.Network(element[0]);
          }
        }
      };
    }])
})()