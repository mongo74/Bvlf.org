/// <reference path="~/Scripts/jquery-3.1.1.min.js" />
/// <reference path="gmap3.min.js" />
//(function($, google) {
//    // Config Settings
//    var cfg = {
//        root: "#map-control",
//        mapOptions: {
//            zoom: 8,
//            maxZoom: 12,
//            center: [48.8620722, 2.352047],
//        }
//    };

//    //window object
//    window.Profff = window.Profff || {},
//    window.Profff.gmaps = {
//        settings: window.Profff.gmaps.settings || {},

//        // init asap
//        init: function () {
//            // create cache items
//            this.createCacheItems();
//        },

//        // Fill Cache
//        createCacheItems: function () {
//            this.root = $(cfg.root);
//        },

//        // activate
//        activate: function () {
//            if (this.root.length) {
//                this.initMap();
//             //   this.initEvents();
//            }
//        },

//        // 2.6 Initialize the map
//        // @param (options): object
//        initMap: function (options) {
//            this.root.gmap3({
//                map: { options: options || cfg.mapOptions }
//            });
//        }

//    };
//   // return window.Profff;

//    // 3. INIT ASAP
//    Profff.gmaps.init();

//    // 3. ACTIVATE ON DOM READY
//    $(function () {
//        alert("so far so good");
//        Profff.gmaps.activate();
//    });

//}(jQuery, google));


window.profff = (function($, profff) {
    // 1. ECMA-262/5
    "use strict";

    // 2. config
    var cfg = {
        root: "#map-control",
        mapOptions: {
            zoom: 8,
            maxZoom: 12,
            center: [51.269477777, 4.26253888888888]
            //   mapTypeId: google.maps.MapTypeId.ROADMAP
        }
    };

    profff.gmap = {
        init: function() {
            //   this.createCacheItems();
            //  this.initMap();
        },

        //    // Fill Cache
        createCacheItems: function() {
            this.root = $(cfg.root);
        },

        activate: function() {
            if (this.root.length) {
                this.initMap();
            }
        },

        // @param (options): object
        initMap: function(options) {
            this.root.gmap3({
                map: { options: options } //|| cfg.mapOptions 
            });
        }
    };
    return profff;

}(window.jQuery, window.profff || {}));

var bvlfjs = {
    init: function() {
        window.profff.gmap.init();
    }
};
$(bvlfjs.init);

//$(function() {
//    $("#map-control")
//         .gmap3({
//             center: [48.8620722, 2.352047],
//             zoom: 4
//         })
//         .marker([
//           { position: [48.8620722, 2.352047] },
//           { address: "86000 Poitiers, France" },
//           { address: "66000 Perpignan, France", icon: "http://maps.google.com/mapfiles/marker_grey.png" }
//         ])
//         ;
//});