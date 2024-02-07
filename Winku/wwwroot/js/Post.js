function likepost(id) {
    var postId = id;
    var Endpoint = '/Like/LikePost/';

    $.ajax({
        type: 'GET',
        url: Endpoint + '?id=' + postId,
        success: function (response) {
            $('.likebtn[id="' + postId + '"]').text(response);
            console.log(response);
        },
        error: function (error) {
            // Handle the error case
            console.log("BIG ERROR COZ "+error);
        }
    });
}
function showhide(element) {
    var userId = $(element).attr('post-id');
    console.log("hey" + userId);
    $('.comment-body[post-id="' + userId + '"]').slideToggle();
    console.log("end");
}

function comment(element) {
    var post_id = $(element).attr('post-id');
    var user_id = $(element).attr('user-id');
    var z=$('.get-comment[post-id="' + post_id + '"]').val();
    console.log("Z : = "+z);
    console.log("jello from comment");   
    var Endpoint = '/Comment/AddComment/';
    
    $.ajax({
        type: 'POST',
        url: Endpoint,
        data: {
            PostId: post_id,
            Comment: z,
        } ,
        success: function (response) {
            //alert("COMMENT ADDED " + response);
            var front = `
                         <div class="input-group mb-1 mt-2" post-id="@item.Id" user-id="@item.UserId">
                                    <input type="text" disabled value="${response.value.comment}" class="form-control view-comment" post-id="@item.Id" user-id="@item.UserId" aria-label="Example text with two button addons">
                                </div>       
                         `
            $('.dynamically[post-id="' + post_id + '"]').append(front);
            $('.abc[post-id="' + post_id + '"]').hide();
            $('.get-comment[post-id="' + post_id + '"]').val('');
        },
        error: function (error) {
            console.log("BIG ERROR COZ " + error);
        }
    });
}
