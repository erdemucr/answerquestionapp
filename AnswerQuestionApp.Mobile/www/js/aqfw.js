var aqfw = function () {

    const serviceUrl = 'http://localhost:50999/api';
    const challangeSocketUrl = 'ws://localhost:50999/challenge';


    var navigation = function () {
        const routes = {
            SingIn: "signin2.html",
            SingUp: "signup2.html",
            Index: "menu.html",
            Challenge: "challenge.html"
        };
        var redirect = function (route) {
            $(".wrapper .container-wrapper").load(route, function (response, status, xhr) {
                if (xhr.status !== 200) {
                    messageBox().ShowWarning('Hata', 'Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz');
                }
                else {
                    SetStyleContainer(route);
                    RouteScript(route);
                }
            }); 
        };
        function SetStyleContainer(route) {
            if (route === routes.SingIn) {
                $(".main-header").hide();
                SetCssClass('wrapper', 'pink-gradient', true);
                SetCssClass('wrapper', 'align-items-center', true);
            }
            else if (route === routes.SingUp) {
                $(".main-header").hide();
                SetCssClass('wrapper', 'pink-gradient', true);
                SetCssClass('wrapper', 'align-items-center', true);
            }
         
            else {
                SetCssClass('wrapper', 'pink-gradient', false);
                SetCssClass('wrapper', 'align-items-center', false);
                $(".main-header").show();
                $("#fullNameTitle").html(aqfw().Auth().GetfullName());

            }

        }
       
        function SetCssClass(containerClass, cssClass, add) {
            containerClass = '.' + containerClass;
            if (!$(containerClass).hasClass(cssClass) && add) {
                $(containerClass).addClass(cssClass);
            }
            if ($(containerClass).hasClass(cssClass) && !add) {
                $(containerClass).removeClass(cssClass);
            }
        }
        return {
            Routes: routes,
            Redirect: redirect
        };
    };

    var auth = function () {
        var isLogged = function () {
            var token = window.localStorage.getItem('token');
            if (token !== null && token !== undefined && token !== '') {
                return true;
            }
            return false;
        };
        var getuserName = function () {
            return window.localStorage.getItem('username');
        };
        var getfullName = function () {
            return window.localStorage.getItem('fullname');
        };
        var getuserId = function () {
            return window.localStorage.getItem('fullname');
        };
        var checkLoginRedirectLogin = function () {
            if (!isLogged()) {
                navigation().Redirect(navigation().Routes.SingIn);
            }
            else {
                navigation().Redirect(navigation().Routes.Index);
            }
        };
        var logout = function () {
            window.localStorage.removeItem('token');
            window.localStorage.removeItem('username');
            navigation().Redirect(navigation().Routes.SingIn);
        };

        return {
            IsLogged: isLogged,
            CheckLoginRedirectLogin: checkLoginRedirectLogin,
            Logout: logout,
            GetfullName: getfullName,
            GetuserName: getuserName,
            GetuserId: getuserId
        };
    };

  

    var messageBox = function () {
        var showSucess = function (heading, message) {
            $.toast({
                heading: heading,
                text: message,
                position: 'bottom-center',
                stack: false,
                hideAfter: 6000,
                icon: 'success'
            });
        };
        var showWarning = function (heading, message) {
            $.toast({
                heading: heading,
                text: message,
                position: 'bottom-center',
                stack: false,
                hideAfter: 6000,
                icon: 'warning'
            });
        };
        return {
            ShowWarning: showWarning,
            ShowSucess: showSucess
        };
    };
    var validation = function (form) {
        function validateEmail(email) {
            var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(String(email).toLowerCase());
        }

        var checkAllFields = function () {
            form = '#' + form;
            var $form = $(form);
            var message = '';

            $form.find('input').each(function (i) {
                var $el = $(this);
                if ($el.attr('data-required') === 'true' && $el.val().trim() === '' && message==='') {
                    message= $el.attr('data-required-message');
                    return message;
                }
            });
            $form.find('input').each(function (i) {
                var $el = $(this);
                if ($el.attr('data-email') === 'true' && !validateEmail($el.val().trim()) && message=== '') {
                    alert($el.attr('data-email-message'));
                    message = $el.attr('data-required-message');
                    return message;
                }
            });
            return message;

        };
        return {
            CheckAllFields: checkAllFields
        };
    };

    return {
        Navigation: navigation,
        MessageBox: messageBox,
        ServiceUrl: serviceUrl,
        ChallangeSocketUrl: challangeSocketUrl,
        Auth: auth,
        Validation: validation
    };
};

var socketConnection = function () {
    socket = new WebSocket(aqfw().ChallangeSocketUrl);
    socket.onopen = function (e) {
        console.log("Connected", e);
        ChallengeEntry();
    };
    socket.onclose = function (e) {
        console.log("Disconnected", e);
    };
    socket.onerror = function (e) {
        alert('Lütfen internet bağlantınızı kontrol ediniz!');
    };
    socket.onmessage = function (e) {
        console.log(e.data);
    };
    socket.onclose = function () {
       // setTimeout(function () { socketConnection(); }, 5000);
    };

    function ChallengeEntry() {
        if (!socket || socket.readyState !== WebSocket.OPEN) {
            console.error("Houston we have a problem! Socket not connected.");
        }
        var enterRoom = {
            'userId': aqfw().Auth().GetuserId()
        };
        socket.send(JSON.stringify(enterRoom));
    }
};

var RouteScript = function (route) {
    if (route === aqfw().Navigation().Routes.Challenge) {
        socketConnection();
    }
};