// dashboard javascript

$(document).ready(function(){
	var hid = true;
	$("#account-dropdown").hide();
	$("#account-wrapper").click(function(){
		if(hid == true){
			$("#account-dropdown").show();
			hid = false;
		}

		else{
			$("#account-dropdown").hide();
			hid = true;
		}
	});
});

function ready_application_status(){
	// show entries
	for(var x = 1; x <= 30; x++)
		$("#show-entries").append('<option value="' + x + '">' + x + '</option>');
}