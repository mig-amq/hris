// modal javascript

function clearModalForm() {
    $("#modal form")[0].reset();

    if ($("#alert") != null) {
        $("#alert").attr('class', '');
        $("#alert").text("");
    }
}

$(document).on('click', '.modal-trigger', () => {
    $("#modal").show();
})

$(document).on('click', '#modal-close', () => {
    $("#modal").hide();
    clearModalForm();
})

$(document).on('click', '#overlay-exit', () => {
    $("#modal").hide();
    clearModalForm();
})

$(document).ready(function(){
	$("#modal").hide();
    
});