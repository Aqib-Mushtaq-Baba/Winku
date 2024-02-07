function accept(element) {
    var userId = $(element).attr('user-id');
    var Endpoint = '/Freind/Accept/';
    $.ajax({
        type: 'GET',
        url: Endpoint + '?id=' + userId,
        success: function (response) {
            //$('.likebtn[user-id="' + userId + '"]').text(response);
            console.log("Accepted "+response);
            abc();              //here i am calling abc() which is also ajax()
        },
        error: function (error) {
            console.log("BIG ERROR COZ " + error);
        }
    });
}

function decline(element) {
    var userId = $(element).attr('user-id');
    var Endpoint = '/Freind/Decline/';

    $.ajax({
        type: 'GET',
        url: Endpoint + '?id=' + userId,
        success: function (response) {
            //$('.likebtn[user-id="' + userId + '"]').text(response);
            console.log("Deleted "+response);
            abc();
        },
        error: function (error) {
            // Handle the error case
            console.log("BIG ERROR COZ " + error);
        }
    });
}

function abc() {
    var secondEndpoint = '/Freind/AjaxFreindRequests/';
    $.ajax({
        type: 'GET',
        url: secondEndpoint,
        success: function (response) {
            //debugger
            var result = '';
            debugger
            for (var i = 0; i < response.length; i++)
            {
                result += `
                                <div>
                                            <h3>${response[i].userName}</h3>
                                            
                                            <img src="/UserProfilePictures/${response[i].profileImage}" width="100" height="100" />
                

                                            <a onclick="accept(this);" user-id="${response[i].id}" class="btn btn-success Acceptbtn">Accept</a>
                                            <a onclick="decline(this);" user-id="${response[i].id}" class="btn btn-danger Declinebtn">Decline</a>
                                            <hr />
                                </div>
                        `
            }
            $(".test").html('');
            $(".test").append(result);
                           
            console.log("<h1>successfully hit second endpoint as well</h1>" + response);
        },
        error: function (secondError) {
            console.log("Second AJAX call error: " + secondError);
        }
    });
}