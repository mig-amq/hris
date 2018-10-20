// modal javascript

$(document).ready(function(){
	$("#modal").hide();
	$(".modal-trigger").click(function(){
		//$("#modal-form input").val("");
		//$("#modal-form textarea").val("");
		//$("#modal-form select").val(-1);
		if($(this).has("data-id")){
			var contents = $(".modal-content");
			var index = $(this).attr("data-id");
			$(".modal-content").hide();

			$(contents[index]).show();
			console.log(contents[index]);
			$("#modal").show();
		}

		else $("#modal").show();
	});

	$("#modal-close").click(function(){
		$("#modal").hide();
	});

	$("#overlay-exit").click(function(){
		$("#modal").hide();
	});
});