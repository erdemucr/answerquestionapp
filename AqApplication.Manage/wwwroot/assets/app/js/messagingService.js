var socketConnection = function () {
    socket = new WebSocket(window.socketUrl);
    var $personTg = $("#messenger_persons");
    var $chatBox = $("#chat-messages");
    var image = '/assets/images/user.png';
    socket.onopen = function (e) {
        var enterRoom = {
            'userId': window.current,
            'action': 0
        };
        socket.send(JSON.stringify(enterRoom));

        $("#send_message_btn").click(function () {
            $(this).addClass("m-loader m-loader--right m-loader--light");
            var message = $('#chat_message_txt').val();
            var to = $("#chat_rec").val();
            if (message !== "" && message.length) {
                var enterRoom = {
                    'userId': window.current,
                    'action': 1,
                    'to': to,
                    'message': message
                };
                socket.send(JSON.stringify(enterRoom));
                $('#chat_message_txt').val("");
            }
        });
    };
    socket.onclose = function (e) {
        console.log("Disconnected", e);
        $('#send_message_btn').off('click');
    };
    socket.onerror = function (e) {

    };
    socket.onclose = function () {
        alert("Please check internet connection!");
        setTimeout(function () { socketConnection(); }, 5000);
        $('#send_message_btn').off('click');
    };
    socket.onmessage = function (e) {
        console.log(e.data);
        var cnt = 0;
        var socketResponse = JSON.parse(e.data);
        console.log(socketResponse);

        //if (socketResponse.SocketClientModelList !== null) {
        //    $personTg.html("");
        //    if (socketResponse.SocketClientModelList.length) {
        //        for (var i = 0; i < socketResponse.SocketClientModelList.length; i++) {
        //            if (socketResponse.SocketClientModelList[i].UserId !== window.current) {
        //                if (socketResponse.SocketClientModelList[i].Image !== null && socketResponse.SocketClientModelList[i].Image !== '') {
        //                    image = socketResponse.SocketClientModelList[i].Image;
        //                }
        //                $personTg.append(PersonItem(image, socketResponse.SocketClientModelList[i].FullName, '', 'Online', socketResponse.SocketClientModelList[i].UserId));
        //            }
        //        }
        //    }
        //}
        if (socketResponse.Message !== null) {
            var isOwn = false;
            if (window.current === socketResponse.FromIdendity) {
                isOwn = true;
                if (socketResponse.Image !== '' && socketResponse.Image !== null && typeof socketResponse.Image !== 'undefined') {
                    image = socketResponse.SocketClientModelList[i].Image;
                }
                $("#send_message_btn").removeClass("m-loader m-loader--right m-loader--light");
            }
            $chatBox.append(ChatDialogBox(socketResponse.FromFullName, socketResponse.Message, image, socketResponse.MessageDate, isOwn));
            const container = document.querySelector('#chat-messages');
            container.scrollTop = 99999999999;
        }
        $(".kt-widget__username").click(function () {
            var to = $(this).attr("data-id");
            var name = $(this).attr("data-name");
            $("#chat_rec").val(to);
            $("#chat-user").val(name);

        });
    };
};
var messenger = function () {
    var $personTg = $("#messenger_persons");
    var loadClients = function () {
        $.ajax({
            type: "GET",
            url: "/Messages/GetPastMesagesClients",
            success: function (data) {
                if (data.success) {
                    $personTg.html("");
                    if (data.length) {
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].ProfilPicture !== null && data[i].ProfilPicture !== '' && typeof data[i].ProfilPicture !== 'undefined') {
                                image = data.Image;
                            }
                            $personTg.append(PersonItem(image, data.FullName, '', 'Online', data.UserId));
                        }
                    }
                }
                else {
                    alert('Error');
                }
            }
        });
    };
    return {
        LoadClients: loadClients
    };
};
function PersonItem(img, name, title, status, ids) {
    return '<div class="kt-widget__item" data-id=' + ids + '>' +
        '<span class="kt-media kt-media--circle">' +
        '<img src="' + img + '" alt="image">' +
        '</span>' +
        '<div class="kt-widget__info">' +
        '<div class="kt-widget__section">' +
        '<a href="javascript:void(0);" class="kt-widget__username" data-id="' + ids + '" data-name="' + name + '">' + name + '</a>' +
        '<span class="kt-badge kt-badge--success kt-badge--dot"></span>' +
        '</div>' +
        '<span class="kt-widget__desc">' +
        title +
        '</span>' +
        '</div>' +
        '<div class="kt-widget__action">' +
        '<span class="kt-widget__date">' + status + '</span>' +
        '<span class="kt-badge kt-badge--success kt-font-bold"></span>' +
        '</div>' +
        '</div>';

}

function ChatDialogBox(name, message, img, time, isOwn) {
    console.log(message);
    var right = '';
    var dialogCls = ' kt-bg-light-success';
    if (isOwn) {
        right = ' kt-chat__message--right';
        dialogCls = ' kt-bg-light-brand';
    }
    var str = '<div class="kt-chat__message' + right + '">' +
        '<div class="kt-chat__user">' +
        '<span class="kt-media kt-media--circle kt-media--sm">' +
        '<img src="' + img + '" alt="image">' +
        '</span>' +
        '<a href="#" class="kt-chat__username">' + name + '</a>' +
        '<span class="kt-chat__datetime">' + time + '</span>' +
        '</div>' +
        '<div class="kt-chat__text ' + dialogCls + '">' +
        message +
        '</div>' +
        '</div>';
    return str;
}