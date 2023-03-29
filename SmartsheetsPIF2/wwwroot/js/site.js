// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(function () {
    $("#startdatepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-5:+0",
        dateFormat: 'mm/dd/yy',
        controlType: 'select',
    });
});

$(function () {
    $("#duedatepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-5:+0",
        dateFormat: 'mm/dd/yy',
        controlType: 'select',
    });
});

$(function () {
    $('textarea').each(function () {
        $(this).height($(this).prop('scrollHeight'));
    });
});


$window = $(window);
$window.scroll(function() {
  $scroll_position = $window.scrollTop();
    if ($scroll_position > 150) { 
        $('.main-header').addClass('sticky');

        header_height = $('.main-header').innerHeight();
        $('body').css('padding-top' , header_height);
    } else {
        $('body').css('padding-top' , '0');
        $('.main-header').removeClass('sticky');
    }
});


$(document).ready(() => {
    $('.js-revealer').on('change', function () {
        var $select = $(this);
        var $selected = $select.find('option:selected');
        var hideSelector = $selected.data('r-hide-target');
        var showSelector = $selected.data('r-show-target');

        $(hideSelector).addClass('is-hidden');
        $(showSelector).removeClass('is-hidden');
    });
});


jQuery(document).ready(function ($) {
    $(".clickable-row").click(function () {
        window.location = $(this).data("href");
    });
});



$(document).ready(function () {
    $('input[type="checkbox"]').click(function () {
        var inputValue = $(this).attr("value");
        $("." + inputValue).toggle();

    });
});

$(".wbs-expand").click(function () {

    $expand = $(this);
    //getting the next element
    $content = $(".wbs-content");
    //open up the content needed - toggle the slide- if visible, slide up, if not slidedown.
    $content.slideToggle(500, function () {
        //execute this after slideToggle is done
        //change text of header based on visibility of content div
        $expand.text(function () {
            //change text based on condition
            return $content.is(":visible") ? "Collapse -" : "Expand +";
        });
    });

});

$(".box-expand").click(function () {

    $expand = $(this);
    //getting the next element
    $content = $(".box-content");
    //open up the content needed - toggle the slide- if visible, slide up, if not slidedown.
    $content.slideToggle(500, function () {
        //execute this after slideToggle is done
        //change text of header based on visibility of content div
        $expand.text(function () {
            //change text based on condition
            return $content.is(":visible") ? "Collapse -" : "Expand +";
        });
    });

});

$(".figma-expand").click(function () {

    $expand = $(this);
    //getting the next element
    $content = $(".figma-content");
    //open up the content needed - toggle the slide- if visible, slide up, if not slidedown.
    $content.slideToggle(500, function () {
        //execute this after slideToggle is done
        //change text of header based on visibility of content div
        $expand.text(function () {
            //change text based on condition
            return $content.is(":visible") ? "Collapse -" : "Expand +";
        });
    });

});


