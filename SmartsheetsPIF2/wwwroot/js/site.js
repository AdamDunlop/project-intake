// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//function showTracker() {
//    var element = document.getElementById("show-tracker");
//    element.classList.toggle("show");
//}

$(function () {
    $('textarea').each(function () {
        $(this).height($(this).prop('scrollHeight'));
    });
});

//let start = $('#startDate').val;
//let end = $('#endData').text;

//console.log(start);

//function startDate() {
//    //var start = document.getElementById('startDate')
//    //console.log("start=", start)

//    var str = 'some text that you send to the console...';
//    document.getElementById("startDate").innerHTML = str;
//}


//let start = .slice(0, 5); 

$('#startDate').each(function (i, text) {
    var $this = $(this),
        data = $this.text().slice(0, 5);

    var arr = data.split(',');
    $.each(arr, function (i, strip) {
        $this.text(strip);
    });
})

$('#endDate').each(function (i, text) {
    var $this = $(this),
        data = $this.text().slice(0, 5);

    var arr = data.split(',');
    $.each(arr, function (i, strip) {
        $this.text(strip);
    });
})
//console.log(start)

//start.each(function (i) {
//    strip = i.text().slice(0, 5);
//    document.getElementById("startDate").innerHTML = strip;
//})


//let end = $('#endDate').text().slice(0, 5);
//document.getElementById("endDate").innerHTML = end;
