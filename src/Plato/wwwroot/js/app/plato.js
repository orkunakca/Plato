﻿// <reference path="~/js/vendors/jquery.js" />
// <reference path="~/js/vendors/bootstrap.js" />

if (typeof jQuery === "undefined") {
    throw new Error("Plato requires jQuery");
}

$(function (win, doc, $) {

    "use strict";

    /* $.Plato */
    /* ---------------------------------------------*/

    win.$.Plato = {};

    /* Default options */
    win.$.Plato.Options = {
        debug: true,
        url: "",
        apiKey: "",
        csrfHeaderName: "X-Csrf-Token",
        csrfCookieName: "",
        // UI tooltips
        BSToolTipEnabled: true,
        BSToolTipSelector: "[data-toggle='tooltip']",
        MagnificSelector: "[data-toggle='dialog']",
        AvatarUploadSelector: "[data-upload='avatar']",
        getCsrfCookieToken: function () {
            if (this.csrfCookieName !== "") {
                var storage = win.$.Plato.Storage;
                var cookie = storage.getCookie(this.csrfCookieName);
                if (cookie) {
                    return cookie;
                }
            }
            return "";
        }
    };

    /* Simple logging */
    win.$.Plato.Logger = {
        info: "Info",
        warning: "Warning",
        error: "Error",
        prevLogDate: null,
        logInfo: function(message) {
            this.log(this.info, message);
        },
        logWarning: function(message) {
            this.log(this.warning, message);
        },
        logError: function(message) {
            this.log(this.error, message);
        },
        log: function(level, message) {
            if (!$.Plato.Options.debug) {
                return;
            }
            var difference = this.getDifferenceInMilliseconds();
            this.prevLogDate = new Date();
            console.log(level + ": " + message + " - " + difference + "ms elapsed since previous log entry.");
        },
        getDifferenceInMilliseconds: function() {
            if (this.prevLogDate === null) {
                this.prevLogDate = new Date();
            } else {
                return (this.prevLogDate - new Date()) / 10000;
            }
            return 0;
        }
    };
    
    // access to options & core functionality
    win.$.Plato.Context = {
        options: function() {
            // Extend the options if external options exist
            if (typeof window.PlatoOptions !== "undefined") {
                $.extend(true, $.Plato.Options, window.PlatoOptions);
            }
            return $.Plato.Options;
        },
        logger: $.Plato.Logger
    };

    /* Client side localization */
    win.$.Plato.Locale = {
        lang: "en-US"
    };

    /* Plato UI */
    win.$.Plato.UI = {
        context: win.$.Plato.Context,
        init: function() {

            // init
            this.initToolTips();
            this.initDropDowns();
            this.initAvatar();

        },
        logInfo: function(message) {
            this.context.logger.logInfo(message);
        },
        logError: function(message) {
            this.context.logger.logError(message);
        },
        initToolTips: function() {

            this.logInfo("initToolTips()");

            // Enable bootstratp tooltips
            if (this.context.options().BSToolTipEnabled) {
                $(this.context.options().BSToolTipSelector).tooltip();
                this.logInfo("Bootstratp tooltipss initialized.");
            }

        },
        initDropDowns: function() {

            this.logInfo("initDropDowns()");

            // Enable
            // Enable nested dropdown support
            $("ul.dropdown-menu [data-toggle='dropdown']").on("click",
                function(event) {

                    // Avoid following the href location when clicking
                    event.preventDefault();
                    // Avoid having the menu to close when clicking
                    event.stopPropagation();
                    // If a menu is already open we close it
                    $("ul.dropdown-menu [data-toggle='dropdown']").parent().removeClass('show');
                    // opening the one you clicked on
                    $(this).parent().addClass('show');

                });


        },
        initAvatar: function() {

            this.logInfo("initAvatar()");

            // Avatar upload selector with preview
            $(this.context.options().AvatarUploadSelector).change(function() {
                function readUrl(input) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function(e) {
                            var previewSelector = $(input).attr("data-upload-preview-selector");
                            var $preview = $(previewSelector).find("span");
                            $preview.css('background-image', ' url(' + e.target.result + ')');
                            $preview.hide()
                                .fadeIn(650);
                        };
                        reader.readAsDataURL(input.files[0]);
                    }
                }

                readUrl(this);
            });

        }
    };

    /* Plato Http */
    win.$.Plato.Http = function(config) {

        var context = win.$.Plato.Context;
        if (!context) {
            throw new Error("Plato.Http requires a valid Plato.Context object");
        }

        var opts = context.options();
        if (!opts) {
            throw new Error("Plato.Http requires a valid Plato.Options object");
        }

        // update URL to include absolute URL
        config.url = opts.url + config.url;

        // add basic authentication headers
        var apiKey = opts.apiKey;
        if (apiKey) {
            context.logger.logInfo("ApiKey: " + apiKey);
            config.beforeSend = function(xhr) {
                xhr.setRequestHeader("Authorization", "Basic " + apiKey);
            };
        } else {
            context.logger.logInfo("No api key was supplied");
        }

        // set content type & API version
        config.headers = {
            'Content-Type': 'application/json',
            'X-Api-Version': '1',
            "X-Csrf-Token": opts.getCsrfCookieToken()
        };

        var http = (function() {

            var onError = function(config, xhr, ajaxOptions, thrownError) {
                    if (context) {
                        context.logger.logInfo("$.Plato.Http - Error: " +
                            JSON.stringify(xhr, null, "     ") +
                            thrownError);
                    }
                },
                onAlways = function(xhr, textStatus) {
                    if (context) {
                        context.logger.logInfo("$.Plato.Http - Completed: " +
                            JSON.stringify(xhr, null, "     "));
                    }
                };

            return {
                onError: function(onError) {
                    onError = onError;
                },
                onAlways: function(onAlways) {
                    onAlways = onAlways;
                },
                promise: function(config) {
                    return $.ajax(config)
                        .fail(function(xhr, ajaxOptions, thrownError) {
                            if (onError) {
                                onError(config, xhr, ajaxOptions, thrownError);
                            }
                        })
                        .always(function(xhr, textStatus) {
                            if (onAlways) {
                                onAlways(xhr, textStatus);
                            }
                        });
                }
            };
        }());

        context.logger.logInfo("$.Plato.Http - Starting Request: " + JSON.stringify(config, null, "     "));

        return http.promise(config);

    };

    /* Plato Storage */
    win.$.Plato.Storage = {
        setCookie: function (key, value, expireDays, toJson, path) {

            toJson = toJson || false;
            if (toJson) { value = encodeURIComponent(JSON.stringify(value)); }
            if (!path) { path = "/"; }

            var data = "";
            if (expireDays) {
                var dateExpires = new Date();
                dateExpires.setTime(dateExpires.getTime() + 1000 * 60 * 60 * 24 * expireDays);
                data = '; expires=' + dateExpires.toGMTString();
            }
            document.cookie = key + "=" + value + data + ";path=" + path;
            return this;
        },
        getCookie: function (key) {
            var ckName = key + "=";
            var ckPos = document.cookie.indexOf(ckName);
            if (ckPos !== -1) {
                var ckStart = ckPos + ckName.length;
                var ckEnd = document.cookie.indexOf(";", ckStart);
                if (ckEnd === -1) { ckEnd = document.cookie.length; }
                return unescape(document.cookie.substring(ckStart, ckEnd));
            }
            return null;
        },
        updateCookie: function (key, value, update, delimiter) {
            // if update is false the value will be removed from the cookie
            // if update is true the value will be appended to the cookie
            var cookie = this.getCookie(key),
                temp = new Array();
            delimiter = delimiter || ",";
            if (cookie) {
                // read existing excluding value into temp array
                var values = cookie.split(delimiter);
                for (var i in values) {
                    if (values.hasOwnProperty(i)) {
                        if (values[i] !== value && values[i] !== "") {
                            temp[temp.length] = values[i];
                        }
                    }
                }
            }
            // should we append the current value?
            if (update) { temp[temp.length] = value; }
            // update the cookie
            this.setCookie(key, temp.join(delimiter));
            return this;
        },
        clearCookie: function (key) {
            this.setCookie(key, null, -1);
            return this;
        }
    };

    /* jQuery Plugins */
    /* ---------------------------------------------*/

    /* scrollTo */
    var scrollTo = function () {

        var dataKey = "scrollTo",
            dataIdKey = dataKey + "Id";

        var defaults = {
            event: "click",
            onBeforeComplete: function() {},
            onComplete: function() {}
        };

        var methods = {
            init: function($caller, methodName) {
                if (methodName) {
                    if (this[methodName]) {
                        this[methodName].apply(this, [$caller]);
                    } else {
                        alert(methodName + " is not a valid method!");
                    }
                    return;
                }

                methods.bind($caller);

            },
            bind: function($caller) {

                var event = $caller.data(dataKey).event;
                if (event) {
                    $caller.on(event,
                        function(e) {
                            e.preventDefault();
                            methods.scrollTo($caller);


                        });
                }

            },
            scrollTo: function($caller) {

                jQuery.extend(jQuery.easing,
                    {
                        def: 'easeOutQuad',
                        easeInOutExpo: function(x, t, b, c, d) {
                            if (t === 0) return b;
                            if (t === d) return b + c;
                            if ((t /= d / 2) < 1) return c / 2 * Math.pow(2, 10 * (t - 1)) + b;
                            return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b;
                        }
                    });

                var href = $caller.attr("href");
                if (href) {
                    var $target = $(href);
                    if ($target.length > 0) {
                        $('html, body').stop().animate({
                                scrollTop: $target.offset().top - 10
                            },
                            250,
                            'easeInOutExpo',
                            function() {
                                $caller.data(dataKey).onComplete($caller, $target);
                            });
                        $caller.data(dataKey).onBeforeComplete($caller, $target);
                    }
                }

            }
        };

        return {
            init: function() {

                var options = {};
                var methodName = null;
                for (var i = 0; i < arguments.length; ++i) {
                    var a = arguments[i];
                    switch (a.constructor) {
                    case Object:
                        $.extend(options, a);
                        break;
                    case String:
                        methodName = a;
                        break;
                    case Boolean:
                        break;
                    case Number:
                        break;
                    case Function:
                        break;
                    }
                }

                if (this.length > 0) {
                    // $(selector).markdownEditor
                    return this.each(function() {
                        if (!$(this).data(dataIdKey)) {
                            var id = dataKey + parseInt(Math.random() * 100) + new Date().getTime();
                            $(this).data(dataIdKey, id);
                            $(this).data(dataKey, $.extend({}, defaults, options));
                        } else {
                            $(this).data(dataKey, $.extend({}, $(this).data(dataKey), options));
                        }
                        methods.init($(this), methodName);
                    });
                } else {
                    // $().markdownEditor 
                    if (methodName) {
                        if (methods[methodName]) {
                            var $caller = $("body");
                            $caller.data(dataKey, $.extend({}, defaults, options));
                            methods[methodName].apply(this, [$caller]);
                        } else {
                            alert(methodName + " is not a valid method!");
                        }
                    }
                }

            }

        };

    }();

    /* selectDropdown */
    var selectDropdown = function () {

        var dataKey = "selectDropdown",
            dataIdKey = dataKey + "Id";

        var defaults = {
            event: "click",
            onBeforeComplete: function () { },
            onComplete: function () { }
        };

        var methods = {
            preview: {
                template: '<li class="list-group-item">{text}</li>',
                empty: function($caller) {
                    var text = methods.getDropdownPreviewText($caller);
                    var html = methods.preview.template;
                    return html.replace("{text}", text);
                }
            },
            init: function($caller, methodName) {

                if (methodName) {
                    if (this[methodName]) {
                        this[methodName].apply(this, [$caller]);
                    } else {
                        alert(methodName + " is not a valid method!");
                    }
                    return;
                }

                // Bind events
                methods.bind($caller);

            },
            bind: function($caller) {

                var self = this;

                if ($caller.hasClass("dropdown")) {

                    var $menu = this.getDropdownMenu($caller),
                        $input = this.getDropdownSearchInput($caller);

                    // On dropdown shown focus search
                    $caller.on('shown.bs.dropdown',
                        function() {
                            $input.focus();
                        });

                    // On keyup filter items
                    $input.on('keyup',
                        function(e) {
                            self.filterItems($caller);
                        });

                    // On checkbox or radiobutton change update preview
                    $menu.on('change',
                        'input[type="checkbox"], input[type="radio"]',
                        function(e) {

                            var $preview = self.getDropdownPreview($caller),
                                $items = self.getDropdownListGroupItems($caller);

                            $items.each(function() {
                                $(this).removeClass("active");
                            });

                            // Clear selection preview
                            $preview.empty();

                            // Get all checked labels
                            var $selectedLabels = [];
                            $($menu).find('input:checked').each(function () {
                                var $localLabel = $(this).next();
                                $localLabel.addClass("active");
                                $selectedLabels.push($localLabel);
                            });

                            if ($selectedLabels.length === 0) {

                                $preview
                                    .empty()
                                    .html(self.preview.empty($caller));

                            } else {

                                var html = "";
                                for (var i = 0; i < $selectedLabels.length; i++) {
                                    html += '<div class="list-group-item">';
                                    html += $($selectedLabels[i]).html();
                                    html += "</div>";
                                }

                                $preview.html(html);
                                $preview.show();
                            }

                        });

                    // Prevent bootstrap dropdowns from closing when clicking within dropdowns
                    $menu.on('click',
                        'label, a',
                        function (e) {
                            e.stopPropagation();
                        });

                }

            },
            filterItems: function($caller) {

                var $items = this.getDropdownListGroupItems($caller),
                    $input = this.getDropdownSearchInput($caller),
                    $noResults = this.getDropdownSearchNoResults($caller),
                    word = $input.val().trim(),
                    length = $items.length,
                    hidden = 0;
                
                if (word.length === 0) {
                    $items.show();
                    $caller.treeView("collapseAll");
                } 

                var i = 0, $label = null;

                // first ensure all matches are visible
                for (i = 0; i < length; i++) {
                    $label = $($items[i]);
                    if ($label.length > 0 && $label.data("filterValue")) {
                        if ($label.data("filterValue").toLowerCase().startsWith(word)) {
                            $label.parent(".list-group").show();
                            $label.show();
                        }}
                }

                // Next hide all others if children are not visible
                for (i = 0; i < length; i++) {
                    $label = $($items[i]);
                    if ($label.length > 0 && $label.data("filterValue")) {
                        if (!$label.data("filterValue").toLowerCase().startsWith(word)) {
                            if (!$label.find(".list-group").is(":visible")) {
                                $label.hide();
                                hidden++;
                            }
                        
                        }
                    }

                }


                //If all items are hidden, show the empty view
                if (hidden === length) {
                    $noResults.show();
                } else {
                    $noResults.hide();
                }

            },
            getDropdownButton: function($caller) {
                return $caller.find('[data-toggle="dropdown"]');
            },
            getDropdownMenu: function($caller) {
                return $caller.find(".dropdown-menu");
            },
            getDropdownListGroupItems: function($caller) {
                var $menu = this.getDropdownMenu($caller);
                return $menu.find(".list-group-item");
            },
            getDropdownSearchInput: function($caller) {
                var $menu = this.getDropdownMenu($caller);
                return $menu.find('[type="search"]');
            },
            getDropdownSearchNoResults: function($caller) {
                return $caller.find(".empty");
            },
            getDropdownPreview: function($caller) {
                // Attempt to find a P tag within our .dropdown element
                // If not found use the next element after the .dropdown 
                // for the selection preview
                var $preview = $caller.find(".select-dropdown-preview");
                if ($preview.length === 0) {
                    $preview = $caller.next();
                    if (!$preview.hasClass("select-dropdown-preview")) {
                        throw new Error("A preview area coulod not be found for the select dropdown.");
                    }
                }
                return $preview;
            },
            getDropdownPreviewText: function($caller) {
                var $preview = this.getDropdownPreview($caller);
                if ($preview.length > 0) {
                    return $preview.attr("data-empty-preview-text");
                }
                return "No selection";
            }
        };

        return {
            init: function() {

                var options = {};
                var methodName = null;
                for (var i = 0; i < arguments.length; ++i) {
                    var a = arguments[i];
                    switch (a.constructor) {
                    case Object:
                        $.extend(options, a);
                        break;
                    case String:
                        methodName = a;
                        break;
                    case Boolean:
                        break;
                    case Number:
                        break;
                    case Function:
                        break;
                    }
                }

                if (this.length > 0) {
                    // $(selector).markdownEditor
                    return this.each(function() {
                        if (!$(this).data(dataIdKey)) {
                            var id = dataKey + parseInt(Math.random() * 100) + new Date().getTime();
                            $(this).data(dataIdKey, id);
                            $(this).data(dataKey, $.extend({}, defaults, options));
                        } else {
                            $(this).data(dataKey, $.extend({}, $(this).data(dataKey), options));
                        }
                        methods.init($(this), methodName);
                    });
                } else {
                    // $().markdownEditor 
                    if (methodName) {
                        if (methods[methodName]) {
                            var $caller = $("body");
                            $caller.data(dataKey, $.extend({}, defaults, options));
                            methods[methodName].apply(this, [$caller]);
                        } else {
                            alert(methodName + " is not a valid method!");
                        }
                    }
                }

            }

        };

    }();

    /* treeView */
    var treeView = function () {

        var dataKey = "treeView",
            dataIdKey = dataKey + "Id";

        var defaults = {
            selectedNodeId: null,
            event: "click",
            toggleSelector: '[data-toggle="tree"]',
            linkSelector: ".tree-link",
            enableCheckBoxes: true,
            onBeforeComplete: function () { },
            onClick: function (a) {}
        };

        var methods = {
            init: function ($caller, methodName) {

                if (methodName) {
                    if (this[methodName]) {
                        this[methodName].apply(this, [$caller]);
                    } else {
                        alert(methodName + " is not a valid method!");
                    }
                    return;
                }
                
                // Bind events
                methods.bind($caller);

            },
            bind: function ($caller) {

                var linkSelector = $caller.data(dataKey).linkSelector,
                    toggleSelector = $caller.data(dataKey).toggleSelector,
                    onClick = $caller.data(dataKey).onClick,
                    event = $caller.data(dataKey).event;

                // Bind toggler events
                $(toggleSelector).unbind(event).bind(event,
                    function (e) {
                        e.preventDefault();
                        methods._toggleNode($caller, $(this).attr("data-node-id"));
                    });

                $caller.on('change',
                    'input[type="checkbox"], input[type="radio"]',
                    function (e) {

                        $caller.find("li").each(function () {
                            $(this).removeClass("active");
                        });

                        var nodeId = $(this).attr("data-node-id"),
                            $li = methods.getNodeListItem($caller, nodeId);
                        if ($(this).is(":checked")) {
                            $li.addClass("active");
                        } 


                    });

                // Check / Uncheck child inputs
                $caller.on('change',
                    'input[type="checkbox"]',
                    function (e) {
                        var nodeId = $(this).attr("data-node-id"),
                            $li = methods.getNodeListItem($caller, nodeId),
                            $firstChild = $li.find("ul"),
                            $inputs = $firstChild.find('[type="checkbox"]');
                        if ($inputs.length > 0) {
                            if ($(this).is(":checked")) {
                                $inputs.prop("checked", true);
                            } else {
                                $inputs.prop("checked", false);
                            }
                        }
                    });

                // Bind link click events
                $caller.find(linkSelector).unbind(event).bind(event,
                    function(e) {
                        if (onClick) {
                            onClick($(this));
                        }
                    });


            },
            expand: function ($caller) {
                var nodeId = $caller.data(dataKey).selectedNodeId;
                if (!nodeId) {
                    return;
                }
                if (!$caller.hasClass("show")) {
                    methods._expand($caller, nodeId);
                }
            },
            collapse: function ($caller) {
                var nodeId = $caller.data(dataKey).selectedNodeId;
                if (!nodeId) {
                    return;
                }
                if ($caller.hasClass("show")) {
                    methods._collapse($caller, nodeId);
                }
            },
            toggle: function ($caller) {

                if ($caller.hasClass("show")) {
                    methods.collapse($caller);
                } else {
                    methods.expand($caller);
                }

            },
            expandAll: function ($caller) {
                $caller.find(".list-group-item").each(function() {
                    methods._expand($caller, $(this).attr("id"));
                });
            },
            collapseAll: function ($caller) {
                $caller.find(".list-group-item").each(function () {
                    methods._collapse($caller, $(this).attr("id"));
                });
            },
            _toggleNode: function ($caller, nodeId) {

                var $li = methods.getNodeListItem($caller, nodeId);
                if ($li.hasClass("show")) {
                    methods._collapse($caller, nodeId);
                } else {
                    methods._expand($caller, nodeId);
                }
              
            },
            _toggleNodeAndParents: function($caller, nodeId) {
                methods._toggleNode($caller, nodeId);
            },
            _expand: function ($caller, nodeId) {

                var $li = methods.getNodeListItem($caller, nodeId),
                    $child = $li.find("ul").first();
                $li.addClass("show");
                $child.slideDown("fast");  

            },
            _collapse: function ($caller, nodeId) {
                var $li = methods.getNodeListItem($caller, nodeId),
                    $child = $li.find("ul").first();
                $li.removeClass("show");
                $child.slideUp("fast");
            },
            getNodeListItem: function ($caller, nodeId) {
                return $caller.find("#" + nodeId);
            },
            getNodeToggler: function($caller, nodeId) {
                var $toggler = $caller.find('[data-node-id="' + nodeId + '"]');
                if ($toggler.length > 0) {
                    if ($toggler[0].tagName === "I") {
                        return $toggler;
                    }
                    return $toggler.find("i");
                }
            }
        };

        return {
            init: function () {

                var options = {};
                var methodName = null;
                for (var i = 0; i < arguments.length; ++i) {
                    var a = arguments[i];
                    switch (a.constructor) {
                        case Object:
                            $.extend(options, a);
                            break;
                        case String:
                            methodName = a;
                            break;
                        case Boolean:
                            break;
                        case Number:
                            break;
                        case Function:
                            break;
                    }
                }

                if (this.length > 0) {
                    // $(selector).markdownEditor
                    return this.each(function () {
                        if (!$(this).data(dataIdKey)) {
                            var id = dataKey + parseInt(Math.random() * 100) + new Date().getTime();
                            $(this).data(dataIdKey, id);
                            $(this).data(dataKey, $.extend({}, defaults, options));
                        } else {
                            $(this).data(dataKey, $.extend({}, $(this).data(dataKey), options));
                        }
                        methods.init($(this), methodName);
                    });
                } else {
                    // $().markdownEditor 
                    if (methodName) {
                        if (methods[methodName]) {
                            var $caller = $("body");
                            $caller.data(dataKey, $.extend({}, defaults, options));
                            methods[methodName].apply(this, [$caller]);
                        } else {
                            alert(methodName + " is not a valid method!");
                        }
                    }
                }

            }

        };

    }();


    /* Register jQuery Plugins */
    $.fn.extend({
        scrollTo: scrollTo.init,
        selectDropdown: selectDropdown.init,
        treeView: treeView.init

    });
    
    /* Initialize */
    /* ---------------------------------------------*/
    
    $(doc).ready(function () {

        var context = win.$.Plato.Context;
        context.logger.logInfo("$.Plato.Options = " + JSON.stringify(context.options(), null, "     "));
        $.Plato.UI.init();

        /* plug-ins */

        /* Scolls to a specific element. Typical usage...
         * <a href="#somelement" data-provide="scroll"> */      
        $('[data-provide="scroll"]').scrollTo();

        /* select dropdown */
        $('[data-provide="select-dropdown"]').selectDropdown();

        /* treeView */
        $('[data-provide="tree"]').treeView();

    });

}(window, document, jQuery));
