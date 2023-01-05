﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


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