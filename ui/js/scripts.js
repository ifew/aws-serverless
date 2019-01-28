$(document).ready(function () {
    var api_endpoint = "https://0077rsct8a.execute-api.ap-southeast-1.amazonaws.com/dev";
    $.ajax({
        url: api_endpoint+"/profile",
        async: true,
        type: 'GET',
        crossDomain: true,
        cache: false,
        error: function(msg) {         
            alert(JSON.stringify(msg));
            console.log("error");
        },
        success: function(res) {
            alert(JSON.stringify(res));
        }
    });
});