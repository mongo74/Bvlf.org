/**
 * editor_plugin_src.js
 *
 * Copyright 2009, Moxiecode Systems AB
 * Released under LGPL License.
 *
 * License: http://tinymce.moxiecode.com/license
 * Contributing: http://tinymce.moxiecode.com/contributing
 */

(function () {
    var DOM = tinymce.DOM, Element = tinymce.dom.Element, Event = tinymce.dom.Event, each = tinymce.each, is = tinymce.is;

    tinymce.create('tinymce.plugins.NewsletterStudioUrlContentPlugin', {
        init: function (ed, url) {
            var standardElement = '<img src="' + url + '/img/trans.gif" class="mceNewsletterStudioUrlContent mceItemNoResize" data-url="{data-url}" />',
                cls = 'mceNewsletterStudioUrlContent',
                sep = '[RenderUrl:--id--]';
            
            //pbRE = new RegExp(sep.replace(/[\?\.\*\[\]\(\)\{\}\+\^\$\:]/g, function (a) { return '\\' + a; }), 'g');

            ed.onBeforeRenderUI.add(function (ed) {
                // Add the css-file to the array of files that should be loeaded into the editor-iframe.
                ed.contentCSS.push(url + '/css/newsletterstudiourlcontent.css');

            });

            // Register commands
            /*
            ed.addCommand('mceNewsletterStudioUrlContent', function () {
            ed.execCommand('mceInsertContent', 0, pb);
            });
            */

            ed.addCommand('mceNewsletterStudioUrlContent', function () {
                ed.windowManager.open({
                    file: url + '/../../../../umbraco/newsletterstudio/pages/plugin/dialog.aspx',
                    width: 500 + parseInt(ed.getLang('example.delta_width', 0)),
                    height: 430 + parseInt(ed.getLang('example.delta_height', 0)),
                    inline: 1
                }, {
                    plugin_url: url, // Plugin absolute URL
                    baseurl: ed.baseURI.protocol + "://" + ed.baseURI.authority // Base url. Protocol, domain and ports
                });
            });

            // Register buttons
            ed.addButton('newsletterstudiourlcontent', { title: 'Insert url content', cmd: cls, image: url + '/img/internet-btn.gif' });

            ed.onInit.add(function () {
                if (ed.theme.onResolveName) {
                    ed.theme.onResolveName.add(function (th, o) {
                        if (o.node.nodeName == 'IMG' && ed.dom.hasClass(o.node, cls))
                            o.name = 'pagebreak';
                    });
                }
            });


            ed.onClick.add(function (ed, e) {
                
                e = e.target;
                if (e.nodeName === 'IMG' && ed.dom.hasClass(e, cls))
                    ed.selection.select(e);
            });

            ed.onNodeChange.add(function (ed, cm, n) {
                
                // Väljer aktiv knapp.
                cm.setActive('newsletterstudiourlcontent', n.nodeName === 'IMG' && ed.dom.hasClass(n, cls));

            });

            ed.onBeforeSetContent.add(function (ed, o) {
                // When the page loads, finds content from database and replaces with standard element.
                o.content = o.content.replace(/\[RenderUrl:(.+?)\]/g, standardElement.replace("{data-url}", '$1'));

            });

            ed.onPostProcess.add(function (ed, o) {
            
                if (o.get)
                    o.content = o.content.replace(/<img[^>]+>/g, function (im) {
                        if (im.indexOf('class="mceNewsletterStudioUrlContent"') !== -1) {

                            var iStart = im.indexOf('data-url') + 10;
                            var sFirstPart = im.substr(iStart, im.length - iStart);
                            var sUrl = sFirstPart.substr(0, sFirstPart.indexOf('\"'));
                            im = sep.replace("--id--", sUrl);

                            /* Probably a TinyMCE bug. Even if the string that is parsed (im) contains the data-url attribute
                            the parser or the node-object will not care about the html5 value. Could been fixed in 3.5.8 according to
                            the online changelog. http://www.tinymce.com/develop/changelog/?type=tinymce
							   
                            var imgNode = new tinymce.html.DomParser().parse(im).firstChild;
                            im = sep.replace("--id--", imgNode.attr("data-url")); 
                            */

                        }

                        return im;
                    });
            });
        },

        getInfo: function () {
            return {
                longname: 'PageBrasdeak',
                author: 'Moxiecode Systems AB',
                authorurl: 'http://tinymce.moxiecode.com',
                infourl: 'http://wiki.moxiecode.com/index.php/TinyMCE:Plugins/pagebreak',
                version: tinymce.majorVersion + "." + tinymce.minorVersion
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('newsletterstudiourlcontent', tinymce.plugins.NewsletterStudioUrlContentPlugin);
})();