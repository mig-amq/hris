// modal javascript

$(document).ready(function(){
	$("#modal").hide();
	$(".modal-trigger").click(function(){
		//$("#modal-form input").val("");
		//$("#modal-form textarea").val("");
		//$("#modal-form select").val(-1);
		$("#modal").show();
	});

	$("#modal-close").click(function(){
		$("#modal").hide();
	});

	$("#overlay-exit").click(function(){
		$("#modal").hide();
	});
});