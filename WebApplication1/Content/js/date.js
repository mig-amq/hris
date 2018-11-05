/* date javascript */

$(document).ready(function(){
	readyMonth();
	readyDay();
	readyYear();
});

function readyMonth() {
    var max = $("select#month").attr("data-max") || 12;
    var min = $("select#month").attr("data-min") || 0;

	months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August',
	'September', 'October', 'November', 'December'];
    
    for (var x = parseInt(min); x < parseInt(max); x++)
		$("select#month").append('<option value="' + x + '">' + months[x] + '</option>');
}

function readyDay() {
    let max = $("select#day").attr("data-max") || 31;
    let min = $("select#day").attr("data-min") || 1;

    for (var x = parseInt(min); x <= parseInt(max) ; x++)
		$("select#day").append('<option value="' + x + '">' + x + '</option>');
}

function readyYear() {
    let max = $("select#year").attr("data-max") || new Date().getFullYear() + 5;
    let min = $("select#year").attr("data-min") || 1960;

    for (var x = parseInt(max); x >= parseInt(min); x--)
		$("select#year").append('<option value="' + x + '">' + x + '</option>');
}