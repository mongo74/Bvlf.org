
window.bvlf = (function($, bvlf) {
    // 1. ECMA-262/5
    "use strict";

    // 2. config
    var cfg = {
        ajaxUrl: "/Umbraco/surface/MemberShipApi/",
        ajaxSearchUrl: "GetAllMembersJson",
        ajaxRefineSearch: "GetMembersBySearchCriteria",
        ajaxGetMemberDetails: "GetMemberDetails",
        ajaxUpdateProfile: "UpdateProfile",
        ajaxOptions: {
            // dataType: 'json',
            contentType: "application/json; charset=utf-8",
            processData: true,
            cache: false
        },

        cache: {
            changeStatus: ".ChangeStatus",
            removeMember: ".RemoveMember",
            searchResult: "#SearchResult",
            pagination: ".pagination--dark",
            messageBlock: "#MessageBlock",
            refineSearch: "#cmdRefineSearch",

            filterContainer: ".filterContainer",
            searchQuery: "input[type=text]",
            filterRadios: "input[type=\"radio\"]",
            filterSelects: "#MemberStatus",
            filterGroupCheckbox: ".filter-group--checkbox",
            searchCriteriaBox: "#SearchCriterium",
            details: "[data-details]",
            confirmPayment: "[data-confirmpayment]",
            loading: "#Loading",
            modalwindow: "#MemberModal",
            memberDetailsDialog: ".memberDetailsDialog",
            closeModal: "#cmdCloseModal",
            modifyMemberInfo: "#cmdModifyMemberInfo",
            updatePersonalInfo: "#UpdatePersonalInfo",
            loadingSave: "#LoadingSave"
        },
        event: {
            click: "click",
            change: "change",
        },
        tpl: {
            memberItem: "<div class=\"row listRow\"><div class=\"col-md-1\"><h3><a href=\"#\" alt=\"Details\" data-details=\"{{Id}}\"><i class=\"fi-magnifying-glass style4\" ></i></a></h3><span></div><div class=\"col-md-2\">{{Lidnr}}<br/>{{FullName}}</span></div><div class=\"col-md-3\">{{Email}}</div><div class=\"col-md-1s\">{{SubscriptionDateString}}</div><div class=\"col-md-1s\">{{SubscriptionExpiryString}}</div><div class=\"col-md-2\">{{School}}</div><div class=\"col-md-2\">{{Status}} </div></div>",
            memberIngeschreven: "<div class=\"row listRow\"><div class=\"col-md-1\"><h3><a href=\"#\" alt=\"Details\" data-details=\"{{Id}}\"><i class=\"fi-magnifying-glass style4\" ></i></a></h3><span></div><div class=\"col-md-2\">{{Lidnr}}<br/>  {{FullName}}</span></div><div class=\"col-md-3\">{{Email}}</div><div class=\"col-md-1s\">{{SubscriptionDateString}}</div><div class=\"col-md-1\">{{SubscriptionExpiryString}}</div><div class=\"col-md-2\">{{School}}</div><div class=\"col-md-1\">{{Status}}<h3><a href=\"#\" alt=\"Bevestig Betaling\" data-confirmpayment=\"{{Id}}\"><i class=\"fi-credit-card style4\" ></i></a></h3> </div></div>"
        },
        classes: {
            isActive: "active",
            isHidden: "hidden",
            isToggled: "is-toggled",
            isUsed: "is-used",
            odd: "odd",
            disabledClass: "disabled"
        },
        data: {
            prev: "prev",
            next: "next",
            first: "first",
            last: "last",
            checkbox: "checkbox",
            search: "search",
            dropdown: "dropdown",
            refineSearchdata: "RefineSearch",
            details: "details",
            confirmPayment: "confirmpayment"
        },
        pagerOptions: {
            maxPageItems: 20
        }
    };

    bvlf.Membership = {
        data: [],
        querystring: [],
        init: function() {
            this.cacheItems();
            this.bindEvents();
        },

        cacheItems: function() {
            var cache = cfg.cache;

            this.changeStatus = $(cache.changeStatus);
            this.removeMember = $(cache.removeMember);
            this.searchResult = $(cache.searchResult);
            this.pagination = $(cache.pagination);
            this.messageBlock = $(cache.messageBlock);
            this.refineSearch = $(cache.refineSearch);
            this.filterContainer = $(cache.filterContainer);
            this.filterSelects = this.filterContainer.find(cache.filterSelects);
            this.filterGroupCheckbox = $(cache.filterGroupCheckbox);
            this.searchCriteriaBox = $(cache.searchCriteriaBox);
            this.details = $(cache.details);
            this.modalwindow = $(cache.modalwindow);
            this.loading = $(cache.loading);
            this.memberDetailsDialog = $(cache.memberDetailsDialog);
            this.closeModal = $(cache.closeModal);
            this.modifyMemberInfo = $(cache.modifyMemberInfo);
            this.updatePersonalInfo = $(cache.updatePersonalInfo);
            this.loadingSave = $(cache.loadingSave);
        },

        bindEvents: function() {
            var self = this,
                event = cfg.event;

            $(window).load(function(e) {
                self.getAllMembers();
                self.bindDetails();
            });

            self.refineSearch.on(event.click, function(e) {
                e.preventDefault();
                self.filter();
            });
        },

        bindDetails: function() {
            var self = this;

            $(document).on(cfg.event.click, cfg.cache.details, function() {
                var id = $(this).data(cfg.data.details);
                self.ShowDetails(id);
            });

            $(document).on(cfg.event.click, cfg.cache.confirmPayment, function() {
                var id = $(this).data(cfg.data.confirmPayment);
            });

            $(document).on(cfg.event.click, cfg.cache.closeModal, function() {
                // var id = $(this).data(cfg.data.details);
                self.CloseModal();
            });

            $(document).on(cfg.event.click, cfg.cache.modifyMemberInfo, function() {
                self.ModifyMemberInfo();
            });
        },

        ShowDetails: function(id) {
            $(cfg.cache.loading).html("<span class=\"memberDetailsDialog\"><img src=\"/Media/progress_s.gif\" /> loading...</span>");
            $(cfg.cache.memberDetailsDialog).fadeIn("fast");
            cfg.ajaxOptions.url = cfg.ajaxUrl + cfg.ajaxGetMemberDetails + "/" + id;
            $.ajax(cfg.ajaxOptions).done(function(data) {
                $(cfg.cache.memberDetailsDialog).html(data).dialog({
                    modal: true,
                    closeText: "X",
                    draggable: true,
                    resizable: false,
                    title: "Detail info",
                    width: 730,
                    height: 450,
                    close: function() {
                        $(cfg.cache.memberDetailsDialog).dialog("destroy");
                        $(cfg.cache.memberDetailsDialog).remove();
                    }
                });
            }).fail(function(jqXhr, textStatus, errorThrow) {
                self.searchResult.html("error : " + textStatus);
            });
        },

        CloseModal: function() {
            $(cfg.cache.memberDetailsDialog).remove();
        },

        ModifyMemberInfo: function() {
            var postdata = $(cfg.cache.updatePersonalInfo).serialize();
            cfg.ajaxOptions.url = cfg.ajaxUrl + cfg.ajaxUpdateProfile + "/";
            cfg.ajaxOptions.type = "post";
            cfg.ajaxOptions.postdata = postdata;
            $(cfg.cache.loading).html("<img src=\"/Media/progress_s.gif\" /> loading...");
            $.ajax(cfg.ajaxOptions).done(function(data) {
                e.preventDefault();
                if (data.msg === "success") {
                    $("." + data.userid + "_FullName").html(data.Fullname);
                    $("." + data.userid + "_School").html(data.school);

                    $(cfg.cache.memberDetailsDialog).dialog("destroy");
                    $(cfg.cache.memberDetailsDialog).remove();
                } else {
                    $(cfg.cache.loading).html(data.msg);
                }
            }).fail(function(jqXhr, textStatus, errorThrow) {
                self.searchResult.html("error : " + textStatus);
            });


        },

        getAllMembers: function() {
            var self = this;
            cfg.ajaxOptions.url = cfg.ajaxUrl + cfg.ajaxSearchUrl;
            $.ajax(cfg.ajaxOptions).done(function(data) {
                self.data = data;
                self.renderSearchResult();
                self.paginate();
            }).fail(function(jqXhr, textStatus, errorThrown) {
                self.searchResult.html("error : " + textStatus);
            });
        },

        refineSeach: function(searchCriteria) {
            var self = this;
            cfg.ajaxOptions.url = cfg.ajaxUrl + cfg.ajaxRefineSearch + "/?" + searchCriteria;
            $.ajax(cfg.ajaxOptions).done(function(data) {
                self.data = data;
                self.renderSearchResult();
                self.paginate();
            }).fail(function(jqXhr, textStatus, errorThrown) {
                self.searchResult.html("error : " + textStatus);
            });
        },

        renderSearchResult: function() {
            var self = this,
                data = self.data,
                members = [],
                template = cfg.tpl.memberItem,
                ingeschrevenTemplate = cfg.tpl.memberIngeschreven,
                resultcount = 0;

            // clear the content field
            self.searchResult.empty();

            [].forEach.call(data, function(member) {
                //member.Status == "Ingeschreven"
                var item;
                if (member.Status === "Ingeschreven") {
                    item = self.renderTemplate(member, ingeschrevenTemplate);
                } else {
                    item = self.renderTemplate(member, template);
                }
                members.push(item);
            });
            self.searchResult.append(members);
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
        },

        paginate: function() {
            var self = this,
                classes = cfg.classes,
                data = cfg.data,
                resultCount = self.searchResult.children().length,
                pagination = self.pagination,
                pageCount = Math.ceil(resultCount / cfg.pagerOptions.maxPageItems);
            if (resultCount == 1) {
                self.messageBlock.text(resultCount + " resultaat gevonden");
            } else {
                self.messageBlock.text(resultCount + " resultaten gevonden");
            }


            if (pageCount > 1) {
                pagination.removeClass(classes.isHidden);
                if (typeof pagination.data("twbsPagination") !== "undefined") {
                    pagination.twbsPagination("destroy");
                }

                var options = {
                    activeClass: classes.isActive,
                    prev: pagination.data(data.prev),
                    next: pagination.data(data.next),
                    first: pagination.data(data.first),
                    last: pagination.data(data.last),
                    totalPages: pageCount,
                    visiblePages: 5,
                    onPageClick: function(event, page) {
                        self.changePage(page);
                    }
                }; //
                pagination.twbsPagination(options);
                self.changePage(1);
            } else {
                pagination.addClass(classes.isHidden);
                self.searchResult.children().filter(":odd").addClass(classes.odd);
            }
        },

        changePage: function(page) {
            var classes = cfg.classes,
                pagerOptions = cfg.pagerOptions,
                start = (page - 1) * pagerOptions.maxPageItems,
                children = this.searchResult.children();

            children
                .addClass(classes.isHidden)
                .slice(start, start + pagerOptions.maxPageItems)
                .removeClass(classes.isHidden)
                .filter(":odd").addClass(classes.odd);

            // Hide previous and/or next buttons			
            bvlf.Membership.hidePrevNextButtons();
        },

        hidePrevNextButtons: function() {
            var disabledElements = $(".is-active.disabled");
            $.each(disabledElements, function(index, value) {
                $(this).hide();
            });
        },

        filter: function() {
            var self = this,
                classes = cfg.classes,
                filterstring = [];

            // Reset querystring.
            self.querystring = [];
            //
            // SearchCriteriaBox
            self.querystring.push("searchString=" + self.searchCriteriaBox.val());

            // dropdownlist - memberstatus
            if (self.filterSelects.val()) {
                self.querystring.push(self.filterSelects.prop("name") + "=" + self.filterSelects.val());
            }
            var filterGroups = [
                self.filterGroupCheckbox
            ];

            $.each(filterGroups, function(index, group) {
                self.filterGroup(group);
            });

            self.refineSeach(self.querystring.join("&"));
        },

        filterGroup: function(group) {
            var self = this;

            $.each(group.find("input"), function(index, input) {
                //only filter on the selected inputs
                var $input = $(input);
                if ($input.prop("checked")) {
                    self.querystring.push("cb=" + $input.val());
                }
            });
        },

    };
    return bvlf;

    // 3 Object


}(window.jQuery, window.bvlf || {}));


var bvlfjs = {
    init: function() {
        window.bvlf.Membership.init();
    }
};
$(bvlfjs.init);