/* date javascript */

$(document).ready(function(){
	readyMonth();
	readyDay();
	readyYear();
});

function readyMonth(){
	months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August',
	'September', 'October', 'November', 'December'];

	for(var x = 0; x < months.length; x++)
		$("select#month").append('<option value="' + x + '">' + months[x] + '</option>');
}

function readyDay(){
	for(var x = 1; x <= 31; x++)
		$("select#day").append('<option value="' + x + '">' + x + '</option>');
}

function readyYear(){
	for(var x = new Date().getFullYear() + 5; x >= 1960; x--)
		$("select#year").append('<option value="' + x + '">' + x + '</option>');
}