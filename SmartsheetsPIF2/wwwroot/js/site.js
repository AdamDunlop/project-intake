// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showTracker() {
    var element = document.getElementById("show-tracker");
    element.classList.toggle("show");
}

$(function () {
    $('textarea').each(function () {
        $(this).height($(this).prop('scrollHeight'));
    });
});