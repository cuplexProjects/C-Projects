function htmlEncodeText(inputFieldId, outputFieldId) {
    var text = $("#"+inputFieldId).val();
    var postUrl = apiBaseURL + "utility/htmlEncode/";
    var postData = new Object();
    postData.Content = text;

    $.ajax
    ({
        url: postUrl,
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(postData),
        complete: function(  jqXHR, textStatus) {
            $("#" + outputFieldId).val(jqXHR.responseText);
        }
    });
}

function generateAlphanumericRandom(length) {	
    var postUrl = apiBaseURL + "randomdata/generateAlphanumericRandom/"+length;


    $.get(postUrl, function (data) {
        $("#alphanumeric-random-output").val(data.replace(/\"/g, ''));
    });
}


function initPage() {

    $('#main-menubar a').click(function(e) {
        e.preventDefault();
        $(this).tab('show');
    });


}
window.onload = initPage;