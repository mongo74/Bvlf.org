window.myObject = (function($, myObject) {
    // 1. ECMA-262/5
    "use strict";

    // 2. config
    var cfg = {
        ajaxUrl: "/Umbraco/surface/MemberShipApi/",
        ajaxSearchUrl: "GetAllMembersJson",
        ajaxMemberByIdUrl: "GetMemberById",
        ajaxUpdateCredentials: "UpdateCredentialsForUser",
        ajaxOptions: {
            // dataType: 'json',
            contentType: "application/json; charset=utf-8",
            processData: true,
            cache: false,
            async: false
        },
        cache: {
            showResult: "#ShowResult",
            statusMessage: "#statusMessage"
        },
        tpl: {
            memberItem: "<p>{{id}}</p>"
        },
        event: {
            click: "click",
            change: "change"
        }

    }; //end cfg

    myObject.Temp = {
        resultCount: 0,
        data: [],
        querystring: [],
        init: function() {
            this.cacheItems();
            this.bindEvents();
        },
        cacheItems: function() {
            var cache = cfg.cache;
            this.showResult = $(cache.showResult);
            this.statusMessage = $(cache.statusMessage);

        },

        bindEvents: function() {
            var self = this,
                event = cfg.event;

            $(window).load(function(e) {
                self.GetAllMembers();
            });
        },

        GetAllMembers: function() {
            var self = this;
            cfg.ajaxOptions.url = cfg.ajaxUrl + cfg.ajaxSearchUrl;
            $.ajax(cfg.ajaxOptions).done(function(data) {
                self.data = data;
                self.ShowDataItems(self.data);
            }).fail(function(jqXhr, textStatus, errorThrown) {
                self.showResult.html("error : " + textStatus);
            });

        },

        ShowDataItems: function() {
            var self = this,
                data = self.data,
                template = cfg.tpl.memberItem,
                members = [],
                resultCount = self.resultCount;
            // clear the content field
            self.showResult.empty();
            self.statusMessage.empty();

            //set 'started' message
            self.statusMessage.append("<br/>Starting workflows at : " + new Date() + "<br/>");
            [].forEach.call(data, function(member) {
                var keyVal = "Id",
                    memberid = member[keyVal];
                self.showResult.queue("AjaxCalls", function(next) {
                    self.updateCredentials(memberid, next);
                });
                self.showResult.dequeue("AjaxCalls");
            });
        },

        updateCredentials: function(memberid, next) {
            var self = this;
            cfg.ajaxOptions.url = cfg.ajaxUrl + cfg.ajaxUpdateCredentials + "/" + memberid;
            $.ajax(cfg.ajaxOptions).done(function(data) {
                self.showResult.append(memberid + " - " + data.Success + " - " + data.Message + "<br />");
            }).fail(function(data) {
                self.showResult.append(memberid + " - error : " + data.Message + "<br />");
            }).always(next());
        },

        getMemberInfo: function(memberid, next) {
            var self = this;
            cfg.ajaxOptions.url = cfg.ajaxUrl + cfg.ajaxMemberByIdUrl + "/" + memberid;
            $.ajax(cfg.ajaxOptions).done(function(data) {
                self.statusMessage.append(memberid + " - " + data.FullName + "<br />");
            }).fail(function(jqXhr, textStatus, errorThrown) {
                self.statusMessage.append("error : " + textStatus + "<br />");
            }).always(next());
        },

        renderTemplate: function(obj, template) {
            var tempKey, reg, key;
            for (key in obj) {
                if (obj.hasOwnProperty(key)) {
                    tempKey = String("{{" + key + "}}");
                    reg = new RegExp(tempKey, "g");
                    template = template.replace(reg, obj[key]);
                }
            }
            return template;
        }

    }; //end MyObject.Temp

    return myObject;

}(window.jQuery, window.myObject || {}));

var tempjs = {
    init: function() {
        window.myObject.Temp.init();
    }
};
$(tempjs.init);