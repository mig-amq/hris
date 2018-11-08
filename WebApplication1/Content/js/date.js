/* date javascript */

$(document).ready(function(){
	readyMonth();
	readyDay();
	readyYear();
});

$(document).on("change", ".date > select[name*=start]", (e) => {
    var year = -1;
    var month = -1;
    var day = -1;
    
    var parent = $(e.target).parent();
    var prevY = parent.find("select[name*=end-year]").val();
    var prevM = parent.find("select[name*=end-month]").val();
    var prevD = parent.find("select[name*=end-day]").val();

    parent.find("select[name*=end-month] option:not(:first-child)").remove();
    parent.find("select[name*=end-day] option:not(:first-child)").remove();
    parent.find("select[name*=end-year] option:not(:first-child)").remove();

    readyMonth(parent.find("select[name*=end-month]"));
    readyYear(parent.find("select[name*=end-year]"));
    readyDay(parent.find("select[name*=end-day]"));

    if (parseInt(parent.find("select[name*=start-month]").val()) != -1) {
        month = parseInt(parent.find("select[name*=start-month]").val());
    }

    if (parseInt($("select[name*=start-year]").val()) != -1) {
        year = parseInt(parent.find("select[name*=start-year]").val());
    }

    if (parseInt(parent.find("select[name*=start-day]").val()) != -1) {
        day = parseInt(parent.find("select[name*=start-day]").val());
    }

    parent.find("select[name*=end-month] option").each((i, o) => {
        if (prevY == year) {
            if ($(o).val() < month && $(o).val() != -1) {
                $(o).remove();
            }
        }
    });

    parent.find("select[name*=end-year] option").each((i, o) => {
        if ($(o).val() != -1 && $(o).val() < year) {
            $(o).remove();
        }
    });

    parent.find("select[name*=end-day] option").each((i, o) => {
        if (prevY == year && prevM == month) {
            if ($(o).val() != -1 && $(o).val() < day) {
                $(o).remove();
            }
        }
    });

    if (prevM >= month && prevY >= year)
        parent.find("select[name*=end-month]").val(prevM);
    else
        parent.find("select[name*=end-month]").val(-1);

    if (prevM == month && prevY == year && prevD >= day)
        parent.find("select[name*=end-day]").val(prevD);
    else
        parent.find("select[name*=end-day]").val(-1);

    if (prevY >= year)
        parent.find("select[name*=end-year]").val(prevY);
    else
        parent.find("select[name*=end-year]").val(-1);
});

$(document).on("change", ".date > select#month, .date > select#year", (e) => {
    var type;

    var parent = $(e.target).parent();

    if ($(e.target).attr("name").indexOf("month") != -1) {
        type = $(e.target).attr("name").substring(0, $(e.target).attr("name").indexOf("month") - 1);
    } else {
        type = $(e.target).attr("name").substring(0, $(e.target).attr("name").indexOf("year") - 1);
    }

    var year = -1;
    var month = 0;
    if (type === "") {
        year = parseInt(parent.find("#year").val(), 10);
        month = parseInt(parent.find("#month").val(), 10);
    } else {
        year = parseInt(parent.find("select[name=" + type + "-year]").val(), 10);
        month = parseInt(parent.find("select[name=" + type + "-month]").val(), 10);
    }

    var prevD = $("select[name*=day]") != null ? $("select[name*=day]").val() : null;
    if (year != -1 && month != -1) {
        let d = new Date(year, month + 1, 0);
        var max = d.getDate();
        
        if (type === "") {
            parent.find("#day option:not(:first-child)").remove();
        } else {
            parent.find("select[name=" + type + "-day] option:not(:first-child)").remove();
        }

        parent.find("select[name*=day]").attr("data-max", max);
        readyDay();

        if (prevD)
            parent.find("select[name*=day]").val(prevD);
    }
});


function readyMonth(end) {
    var max = $("select#month").attr("data-max") || 12;
    var min = $("select#month").attr("data-min") || 0;
    var obj = (end) ? end : $("select#month");

	months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August',
	'September', 'October', 'November', 'December'];
    
    for (var x = parseInt(min); x < parseInt(max); x++)
		obj.append('<option value="' + x + '">' + months[x] + '</option>');
}

function readyDay(end) {
    let max = $("select#day").attr("data-max") || 31;
    let min = $("select#day").attr("data-min") || 1;
    var obj = (end) ? end : $("select#day");

    for (var x = parseInt(min); x <= parseInt(max) ; x++)
	    obj.append('<option value="' + x + '">' + x + '</option>');
}

function readyYear(end) {
    let max = $("select#year").attr("data-max") || new Date().getFullYear() + 5;
    let min = $("select#year").attr("data-min") || 1960;
    var obj = (end) ? end : $("select#year");

    for (var x = parseInt(max); x >= parseInt(min); x--)
		obj.append('<option value="' + x + '">' + x + '</option>');
}