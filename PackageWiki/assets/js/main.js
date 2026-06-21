(function ($) {
    "use strict";

    $(window).on("load", function () {
        $("#gx-overlay").fadeOut("slow");
    });
    $(document).ready(function () {

        $("body").attr("data-gx-mode", "light");

        /*========== side-menu dropdown ===========*/
        var currentActiveTab = localStorage.getItem('currentActiveTab') ?? null;

        $(".gx-drop-toggle").on("click", function (e) {
            var senderElement = e.target;

            if ($(senderElement).hasClass('gx-sub-drop-toggle')) return;
            if ($(senderElement).hasClass('gx-page-link')) return;
            if ($(senderElement).hasClass('condense') && $(senderElement).parents('.gx-sub-drop-toggle').length > 0) return;

            var parent = $(this).parents('.sb-drop-item');
            currentActiveTab = $(parent).find('.gx-drop-toggle span.condense').text();

            if ($(parent).hasClass('load-active')) {
                $(parent).find(".gx-sb-drop").slideUp();
                $(parent).removeClass('load-active');
                localStorage.setItem('currentActiveTab', '');
                localStorage.setItem('currentActiveSubTab', '');
            } else {
                $('.load-active').removeClass('load-active');
                $(".gx-sb-drop").slideUp();
                $(parent).addClass('load-active');
                $(parent).find(".gx-sb-drop").slideDown();
                localStorage.setItem('currentActiveTab', currentActiveTab);
            }
        });

        /*========== Structure dowpdown ===========*/
        $('.gx-hide').slideUp();
        $('.gx-struct-drop').on("click", function () {
            $(this).parent("li").children("ul").slideToggle();
            $(this).parent().parent("ul").toggleClass("active");
        });

        /*========== Search Remix icon page ===========*/
        $('[data-search-icon]').on('keyup', function () {
            var searchVal = $(this).val().toLowerCase();
            var filterItems = $('[data-search-item]');
            var filterItemsText = $('[data-search-item]').text().toLowerCase();
            var a = $('[data-search-item]:contains(' + searchVal + ')');
            if (searchVal != '') {
                filterItems.closest(".remix-unicode-icon").addClass('hide');
                $('[data-search-item]:contains(' + searchVal + ')').closest(".remix-unicode-icon").removeClass('hide');
            } else {
                filterItems.closest(".remix-unicode-icon").removeClass('hide');
            }
        });

        /*========== Search Material icon page ===========*/
        $('[data-search-material]').on('keyup', function () {
            var searchVal = $(this).val().toLowerCase();
            var filterItems = $('.material-search-item');
            var filterItemsText = $('.material-search-item').text().toLowerCase();
            var a = $('.material-search-item:contains(' + searchVal + ')');
            if (searchVal != '') {
                filterItems.closest(".material-icon-item").addClass('hide');
                $('.material-search-item:contains(' + searchVal + ')').closest(".material-icon-item").removeClass('hide');
            } else {
                filterItems.closest(".material-icon-item").removeClass('hide');
            }
        });
    });

    /*========== On ckick card zoom (full screen) ===========*/
    $(".gx-full-card").on("click", function () {
        $(this).hide();
        $(this).parent(".header-tools").append('<a href="javascript:void(0)" class="m-l-10 gx-full-card-close"><i class="ri-close-fill"></i></a>');
        $(this).closest(".gx-card").parent().toggleClass("gx-full-screen");
        $(this).closest(".gx-card").parent().parent().append('<div class="gx-card-overlay"></div>');
    });
    $("body").on("click", ".gx-card-overlay, .gx-full-card-close", function () {
        $(".gx-card").find(".gx-full-card-close").remove();
        $(".gx-card").find(".gx-full-card").show();
        $(".gx-card").parent().removeClass("gx-full-screen");
        $(".gx-card-overlay").remove();
    });

    /*========== Tools Sidebar ===========*/
    $('.gx-tools-sidebar-toggle').on("click", function () {
        $('.gx-tools-sidebar').addClass("open-tools");
        $('.gx-tools-sidebar-overlay').fadeIn();
        $('.gx-tools-sidebar-toggle').hide();

    });
    $('.gx-tools-sidebar-overlay, .close-tools').on("click", function () {
        $('.gx-tools-sidebar').removeClass("open-tools");
        $('.gx-tools-sidebar-overlay').fadeOut();
        $('.gx-tools-sidebar-toggle').fadeIn();
    });

    /*========== color show ===========*/

    $(".gx-color li").click(function () {
        $("li").removeClass("active-variant");
        $(this).addClass("active-variant");
    });

    $(".color-primary").click(function () {
        $("#add_class").remove();
    });

    $(".color-1").click(function () {
        $("#add_class").remove();
        $("head").append(
            '<link rel="stylesheet" href="assets/css/color-1.css" id="add_class">'
        );
    });
    $(".color-2").click(function () {
        $("#add_class").remove();
        $("head").append(
            '<link rel="stylesheet" href="assets/css/color-2.css" id="add_class">'
        );
    });
    $(".color-3").click(function () {
        $("#add_class").remove();
        $("head").append(
            '<link rel="stylesheet" href="assets/css/color-3.css" id="add_class">'
        );
    });
    $(".color-4").click(function () {
        $("#add_class").remove();
        $("head").append(
            '<link rel="stylesheet" href="assets/css/color-4.css" id="add_class">'
        );
    });
    $(".color-5").click(function () {
        $("#add_class").remove();
        $("head").append(
            '<link rel="stylesheet" href="assets/css/color-5.css" id="add_class">'
        );
    });
    $(".color-6").click(function () {
        $("#add_class").remove();
        $("head").append(
            '<link rel="stylesheet" href="assets/css/color-6.css" id="add_class">'
        );
    });
    $(".color-7").click(function () {
        $("#add_class").remove();
        $("head").append(
            '<link rel="stylesheet" href="assets/css/color-7.css" id="add_class">'
        );
    });
    $(".color-8").click(function () {
        $("#add_class").remove();
        $("head").append(
            '<link rel="stylesheet" href="assets/css/color-8.css" id="add_class">'
        );
    });
    $(".color-9").click(function () {
        $("#add_class").remove();
        $("head").append(
            '<link rel="stylesheet" href="assets/css/color-9.css" id="add_class">'
        );
    });


    /*========== Topic Sidebar ===========*/
    $('.btn-topic').on("click", function () {
        $('.gx-topic').addClass("gx-open-topic");
        $('.gx-side-overlay').show();
    });
    $('.gx-side-overlay').on("click", function () {
        $('.gx-topic').removeClass("gx-open-topic");
        $('.gx-side-overlay').hide();
    });

    /*========== Tools Sidebar ===========*/
    /* Mode */
    var $link = $('<link>', {
        rel: 'stylesheet',
        href: 'assets/css/dark.css',
        id: 'dark'
    });
    $('.gx-tools-item.mode').on("click", function () {
        var modes = $(this).attr("data-gx-mode-tool");
        if (modes == "light") {
            $("body").attr("data-gx-mode", "light");
            $("#dark").remove();
            var headerModes = $(".gx-tools-item.header").attr("data-header-mode");
            if (headerModes == "light") {
                $(".gx-tools-item.header[data-header-mode='light']").addClass("active");
                $(".gx-tools-item.header[data-header-mode='dark']").removeClass("active");
                $(".gx-header").attr("data-header-mode-tool", "light");
            }
            $(".gx-mode.light").css("display", "none");
            $(".gx-mode.dark").css("display", "flex");

        } else if (modes == "dark") {
            $("body").attr("data-gx-mode", "dark");
            $("#mainCss").after($link);
            var headerModes = $(".gx-tools-item.header").attr("data-header-mode");
            if (headerModes == "light") {
                $(".gx-tools-item.header[data-header-mode='dark']").addClass("active");
                $(".gx-tools-item.header[data-header-mode='light']").removeClass("active");
                $(".gx-header").attr("data-header-mode-tool", "dark");
            }
            $(".gx-mode.dark").css("display", "none");
            $(".gx-mode.light").css("display", "flex");
        }

        $(this).parents(".gx-tools-info").find('.gx-tools-item.mode').removeClass("active")
        $(this).addClass("active");
    });
    /* Sidebar */
    $('.gx-tools-item.sidebar').on("click", function () {
        var sidebarModes = $(this).attr("data-sidebar-mode-tool");
        if (sidebarModes == "light") {
            $('.gx-sidebar').attr('data-mode', 'light');
        } else if (sidebarModes == "dark") {
            $('.gx-sidebar').attr('data-mode', 'dark');
        } else if (sidebarModes == "bg-1") {
            $('.gx-sidebar').attr('data-mode', 'bg-1');
        } else if (sidebarModes == "bg-2") {
            $('.gx-sidebar').attr('data-mode', 'bg-2');
        } else if (sidebarModes == "bg-3") {
            $('.gx-sidebar').attr('data-mode', 'bg-3');
        } else if (sidebarModes == "bg-4") {
            $('.gx-sidebar').attr('data-mode', 'bg-4');
        }
        $(this).parents(".gx-tools-info").find('.gx-tools-item.sidebar').removeClass("active")
        $(this).addClass("active");
    });
    /* Backgrounds */
    $('.gx-tools-item.bg').on("click", function () {
        var bgModes = $(this).attr("data-bg-mode");
        if (bgModes == "default") {
            $('#mainBg').remove();
        } else if (bgModes == "bg-1") {
            $('#mainBg').remove();
            $("#mainCss").after("<link id='mainBg' href='assets/css/bg-1.css' rel='stylesheet'>");
        } else if (bgModes == "bg-2") {
            $('#mainBg').remove();
            $("#mainCss").after("<link id='mainBg' href='assets/css/bg-2.css' rel='stylesheet'>");
        } else if (bgModes == "bg-3") {
            $('#mainBg').remove();
            $("#mainCss").after("<link id='mainBg' href='assets/css/bg-3.css' rel='stylesheet'>");
        } else if (bgModes == "bg-4") {
            $('#mainBg').remove();
            $("#mainCss").after("<link id='mainBg' href='assets/css/bg-4.css' rel='stylesheet'>");
        } else if (bgModes == "bg-5") {
            $('#mainBg').remove();
            $("#mainCss").after("<link id='mainBg' href='assets/css/bg-5.css' rel='stylesheet'>");
        }
        $(this).parents(".gx-tools-info").find('.gx-tools-item.bg').removeClass("active")
        $(this).addClass("active");
    });
    /* Box design */
    $('.gx-tools-item.box').on("click", function () {
        var boxModes = $(this).attr("data-box-mode-tool");
        $("#box_design").remove();
        if (boxModes == "default") {
            $("#box_design").remove();
        } else if (boxModes == "box-1") {
            $("#mainCss").after('<link id="box_design" href="assets/css/box-1.css" rel="stylesheet">');
        } else if (boxModes == "box-2") {
            $("#mainCss").after('<link id="box_design" href="assets/css/box-2.css" rel="stylesheet">');
        } else if (boxModes == "box-3") {
            $("#mainCss").after('<link id="box_design" href="assets/css/box-3.css" rel="stylesheet">');
        }
        $(this).parents(".gx-tools-info").find('.gx-tools-item.box').removeClass("active")
        $(this).addClass("active");
    });

})(jQuery);