
function addfreind(element) {
    var userId = $(element).attr('user-id');
    console.log("hello freind " + userId);

    var Endpoint = '/Freind/AddFreind/';

    $.ajax({
        type: 'GET',
        url: Endpoint + '?id=' + userId,
        success: function (response) {
            if (response == "Request Sended++++" || response == "Edit Request Sended++++") {
                //alert(response);
                //$('.likebtn[user-id="' + userId + '"]').text(response);
                $('.likebtn[user-id="' + userId + '"]').text(response);
            }
            $('.likebtn[user-id="' + userId + '"]').text(response);
            console.log(response);
        },
        error: function (error) {
            // Handle the error case
            console.log("BIG ERROR COZ " + error);
        }
    });
}

function delfreind(element) {
    var userId = $(element).attr('user-id');
    console.log("hello freind " + userId);

    var Endpoint = '/Freind/DelFreind/';

    $.ajax({
        type: 'GET',
        url: Endpoint + '?id=' + userId,
        success: function (response) {
            if (response == "Entry Deleted++++" || response == "Edit Entry Deleted++++") {
                //alert(response);
                $('.likebtn[user-id="' + userId + '"]').text(response);
            }
            $('.likebtn[user-id="' + userId + '"]').text(response);
            console.log(response);
        },
        error: function (error) {
            // Handle the error case
            console.log("BIG ERROR COZ " + error);
        }
    });
}