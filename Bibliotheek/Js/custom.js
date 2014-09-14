jQuery(document).ready(function (e) {
    $ = jQuery;

    $('.main-nav li').hover(function () {
        $(this).children('ul').stop(true, true).slideDown(500);
    }, function () {
        $(this).children('ul').stop(true, true).slideUp(500);
    });

    $('.sub-menu li').click(function () {
        window.location = $(this).children('a').attr('href');
    });

    $("#searchform #s").addClass("span3 search-query");
    $("#searchform #searchsubmit").addClass("btn");

    $(".pages-nav a").addClass("btn");

    $('.tagcloud a').removeAttr('style').addClass('btn btn-mini');

    if (jQuery().prettyPhoto) {
        $(".pretty-photo").prettyPhoto({
            deeplinking: false,
            social_tools: false
        });

        $('a[data-rel]').each(function () {
            $(this).attr('rel', $(this).data('rel'));
        });

        $("a[rel^='prettyPhoto']").prettyPhoto({
            deeplinking: false,
            social_tools: false
        });
    }

    $(function () {
        $('.accordion dt').click(function () {
            $(this).siblings('dt').removeClass('current');
            $(this).addClass('current').next('dd').slideDown(500).siblings('dd').slideUp(500);
        });
    });

    $(function () {
        $('dl.toggle dt').click(function () {
            if ($(this).hasClass('current')) {
                $(this).removeClass('current').next('dd').slideUp(500);
            }
            else {
                $(this).addClass('current').next('dd').slideDown(500);
            }
        });
    });

    $(function () {
        $(window).scroll(function () {
            if (!$('body').hasClass('probably-mobile')) {
                if ($(this).scrollTop() > 50) {
                    $('a#scroll-top').fadeIn();
                } else {
                    $('a#scroll-top').fadeOut();
                }
            }
            else {
                $('a#scroll-top').fadeOut();
            }
        });

        $('a#scroll-top').on('click', function () {
            if (!$('body').hasClass('probably-mobile')) {
                $('html, body').animate({ scrollTop: 0 }, 'slow');
                return false;
            }
        });
    });

    $(function () {
        var $tabsNav = $('.tabs-nav'),
            $tabsNavLis = $tabsNav.children('li');

        $tabsNav.each(function () {
            var $this = $(this);
            $this.next().children('.tab-content').stop(true, true).hide()
                                                 .first().show();
            $this.children('li').first().addClass('active').stop(true, true).show();
        });

        $tabsNavLis.on('click', function (e) {
            var $this = $(this);
            $this.siblings().removeClass('active').end()
                 .addClass('active');
            var idx = $this.parent().children().index($this);
            $this.parent().next().children('.tab-content').stop(true, true).hide().eq(idx).fadeIn();
            e.preventDefault();
        });
    });

    var switched = false;
    var updateTables = function () {
        if (($(window).width() < 767) && !switched) {
            switched = true;
            $("table.responsive").each(function (i, element) {
                splitTable($(element));
            });
            return true;
        }
        else if (switched && ($(window).width() > 767)) {
            switched = false;
            $("table.responsive").each(function (i, element) {
                unsplitTable($(element));
            });
        }
    };

    $(window).load(updateTables);
    $(window).bind("resize", updateTables);

    function splitTable(original) {
        original.wrap("<div class='table-wrapper' />");

        var copy = original.clone();
        copy.find("td:not(:first-child), th:not(:first-child)").css("display", "none");
        copy.removeClass("responsive");

        original.closest(".table-wrapper").append(copy);
        copy.wrap("<div class='pinned' />");
        original.wrap("<div class='scrollable' />");
    }

    function unsplitTable(original) {
        original.closest(".table-wrapper").find(".pinned").remove();
        original.unwrap();
        original.unwrap();
    }

    $('#like-it-form .like-it').click(function () {
        var likeButton = $(this);
        var likeHtml = likeButton.html();
        var likeNum = parseInt(likeHtml, 10);
        likeNum++;
        likeButton.html(likeNum);
    });

    $('.faq-item').not('.faq-item.active').find('.faq-answer').slideUp('slow');
    $('.faq-item').first().addClass('active').find('.faq-answer').slideDown('slow');

    $('.faq-question, .faq-icon').click(function (e) {
        e.preventDefault();

        var $this = $(this);
        var $parent = $this.parent('.faq-item');

        if ($parent.hasClass('active')) {
            $parent.removeClass('active').find('.faq-answer').slideUp('slow');
        }
        else {
            $parent.addClass('active').find('.faq-answer').slideDown('slow');
        }
    });

    $('.wpcf7-textarea').addClass('span6');
    $('.wpcf7-submit').addClass('btn');

    //$('#search-form').validate({
    //    errorLabelContainer: $("#search-error-container")
    //});

    var $mainNav = $('.main-nav > div').children('ul');
    var optionsList = '<option value="" selected>Go to...</option>';

    $mainNav.find('li').each(function () {
        var $this = $(this),
                $anchor = $this.children('a'),
                depth = $this.parents('ul').length - 1,
                indent = '';
        if (depth) {
            while (depth > 0) {
                indent += ' - ';
                depth--;
            }
        }
        optionsList += '<option value="' + $anchor.attr('href') + '">' + indent + ' ' + $anchor.text() + '</option>';
    }).end();

    $('.main-nav > div').after('<select class="responsive-nav">' + optionsList + '</select>');

    $('.responsive-nav').on('change', function () {
        window.location = $(this).val();
    });

    if (jQuery().validate && jQuery().ajaxSubmit) {
        var contact_options = {
            target: '#message-sent',
            beforeSubmit: function () {
                $('#contact-loader').fadeIn('fast');
                $('#message-sent').fadeOut('fast');
            },
            success: function () {
                $('#contact-loader').fadeOut('fast');
                $('#message-sent').fadeIn('fast');
                $('#contact-form').resetForm();
            }
        };

        $('#contact-form').validate({
            errorLabelContainer: $("div.error-container"),
            submitHandler: function (form) {
                $(form).ajaxSubmit(contact_options);
            }
        });
    }

    if (jQuery().liveSearch) {
        jQuery('#s').liveSearch({ url: 'search.php?livesearch=used&s=' });
    }
});