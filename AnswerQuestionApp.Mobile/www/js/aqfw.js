var aqfw = function () {

    const serviceUrl = 'http://85.105.160.53:81/webapi/api';
    const challangeSocketUrl = 'ws://85.105.160.53:81/webapi';


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
                SetCssClass('content-color-secondary', 'content-color-secondary-white', true);
                $("#footer").show();
                $("#page-boxes").parent().hide();
            }
            else if (route === routes.SingUp) {
                $(".main-header").hide();
                SetCssClass('wrapper', 'pink-gradient', true);
                SetCssClass('wrapper', 'align-items-center', true);
                SetCssClass('content-color-secondary', 'content-color-secondary-white', true);
                $("#footer").show();
                $("#page-boxes").parent().hide();
            }
            else if (route === routes.Challenge) {
                $("#footer").hide();
                SetCssClass('content-color-secondary', 'content-color-secondary-white', false);
                $("#page-boxes").parent().hide();
            }
            else {
                SetCssClass('wrapper', 'pink-gradient', false);
                SetCssClass('wrapper', 'align-items-center', false);
                SetCssClass('content-color-secondary', 'content-color-secondary-white', false);
                $(".main-header").show();
                $("#fullNameTitle").html(aqfw().Auth().GetfullName());
                $("#footer").show();
                $("#page-boxes").parent().hide();
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
        var getToken = function () {
            return window.localStorage.getItem('token');
        };
        var getuserName = function () {
            return window.localStorage.getItem('username');
        };
        var getfullName = function () {
            return window.localStorage.getItem('fullname');
        };
        var getuserId = function () {
            return window.localStorage.getItem('userId');
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
            GetuserId: getuserId,
            GetToken: getToken
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
                if ($el.attr('data-required') === 'true' && $el.val().trim() === '' && message === '') {
                    message = $el.attr('data-required-message');
                    return message;
                }
            });
            $form.find('input').each(function (i) {
                var $el = $(this);
                if ($el.attr('data-email') === 'true' && !validateEmail($el.val().trim()) && message === '') {
                    message = $el.attr('data-email-message');
                    return message;
                }
                if ($el.attr('data-password') === 'true' && !validateEmail($el.val().trim()) && message === '') {
                    message = $el.attr('data-password-message');
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
var intervalInstance = null;
var socket = null;

var socketConnection = function (challengeId) {
    if (challengeId === 0) {
        socket = new WebSocket(aqfw().ChallangeSocketUrl + '/challenge');
    }
    else {
        socket = new WebSocket(aqfw().ChallangeSocketUrl + '/result');
    }


    socket.onopen = function (e) {
        console.log("Connected", e);
        if (challengeId === 0) {
            ChallengeEntry();
        }
        else {
            ChallengeResult(challengeId);
        }
    };
    socket.onclose = function (e) {
        console.log("Disconnected", e);
    };
    socket.onerror = function (e) {
        alert('Lütfen internet bağlantınızı kontrol ediniz!');
    };
    socket.onclose = function () {
        // setTimeout(function () { socketConnection(); }, 5000);
    };
    socket.onmessage = function (e) {
        console.log(e.data);
        var cnt = 0;
        var socketResponse = JSON.parse(e.data);

        if (challengeId === 0) {
            var $table = $("#participantTable tbody");
            $table.html('');
            console.log(socketResponse);
            if (socketResponse.SocketClientModelList.length) {
                $.each(socketResponse.SocketClientModelList, function (i, v) {
                    cnt++;
                    $table.append("<tr><td>" + cnt + "</td><td>" + v.FullName + "</td><td></td></tr>");
                });
            }
            if (socketResponse.LeftSecond > -1) {
                if (intervalInstance !== null) { //!! important otherwise can be set only one time
                    intervalInstance = null;
                }
                var elapsed_seconds = socketResponse.LeftSecond;
                intervalInstance = setInterval(function () {
                    elapsed_seconds = elapsed_seconds - 1;
                    $("#timeClock").show();
                    $('#timeClock').text(getElapsedTimeString(elapsed_seconds));
                    if (elapsed_seconds === 0) {
                        clearInterval(intervalInstance);
                        SliderInit(socketResponse.QuizDuration, socketResponse.ChallengeId);
                        setTimeout(function () {
                            $("#timeClock").hide();
                        }, 1500);
                    }
                }, 1000);
                $("#challengeIdHdn").val(socketResponse.ChallengeId);
                GetQuestion(socketResponse.ChallengeId);
            }
        }
        else {
            var $tableResult = $("#challenge_result tbody");
            $tableResult.html("");
            var cnt1 = 0;
            if (socketResponse.ResultList.length) {
                $.each(socketResponse.ResultList, function (i, v) {
                    cnt1++;
                    $tableResult.append("<tr><td>" + cnt1 + "</td><td colspan='2'>" + v.FullName + "</td><td>" + v.TotalMark + "</td></tr>");
                });
            }
        }
    };

    var questionData = null;
    var currentQuestion = 0;
    var mySwiper = null;

    function ChallengeEntry() {
        if (!socket || socket.readyState !== WebSocket.OPEN) {
            console.error("Houston we have a problem! Socket not connected.");
        }
        var enterRoom = {
            'userId': aqfw().Auth().GetuserId()
        };
        socket.send(JSON.stringify(enterRoom));
    }
    function ChallengeResult(challengeId) {
        if (!socket || socket.readyState !== WebSocket.OPEN) {
            console.error("Houston we have a problem! Socket not connected.");
        }
        var enterRoom = {
            'userId': aqfw().Auth().GetuserId(),
            'challengeId': challengeId
        };
        socket.send(JSON.stringify(enterRoom));
    }
    function StartQuiz(questionList) {
        questionData = questionList;
        console.log(questionList);

        $("#page-boxes").html("");
        $.each(questionData, function (i, v) {
            var questionDiv = '<div class="swiper-slide text-center">' +
                ' <img src="' + v.image + '" alt="" class="questionImage" />' +
                ' <input type="hidden" value="' + v.questionId + '" class="questionIds"/>' +
                ' <input type="hidden" value="" class="selectedAnswer"/>' +
                '</div>';
            var $questionDiv = $(questionDiv);
            $questionDiv.appendTo('#caruselSlider #sliderWrapper');

            var $pageBtn = $('<button type="button" class="btn btn-sm btn-outline-dark mr-1 pagedBox">' + (i + 1) + '</button>');
            var $pageBtn1 = $('<button type="button" class="btn btn-sm btn-outline-dark mr-1 pagedBox">' + (i + 1) + '</button>');
            var $pageBtn2 = $('<button type="button" class="btn btn-sm btn-outline-dark mr-1 pagedBox">' + (i + 1) + '</button>');
            if (i === 0) {
                $pageBtn.addClass('active');
            }
            $pageBtn.appendTo('#page-boxes');
            $pageBtn1.appendTo('#page-boxes');
            $pageBtn2.appendTo('#page-boxes');
        });
        $("#totalQuestion").text(questionData.length);
        $("#questionLenHdn").val(questionData.length);
        $("#answeredQuestionCount").text('0');
        $("#notAnsweredQuesitonCount").text(questionData.length);

    }
    var quizintervalInstance = null;
    function SliderInit(duration, challengeId) {
        $("#participantTable").hide();
        $("#question_wrapper").show();
        $("#page-boxes").parent().show();
        // question slider initializer
        setTimeout(function () {

            mySwiper = new Swiper('.swiper-product', {
                slidesPerView: 1,
                spaceBetween: 0,
                loop: false,
                slideToClickedSlide: false,
                pagination: {
                    el: '.swiper-pagination',
                    clickable: false
                }
            }).on('slideChange', function () {
                console.log('slide changed ' + mySwiper.activeIndex);
                $(".pagedBox").removeClass('active');
                $('#page-boxes .pagedBox:eq(' + mySwiper.activeIndex + ')').addClass('active');
                $(".questionBtn").removeClass('answer-option-item-choiced');
                var answerIndex = $('#caruselSlider #sliderWrapper .swiper-slide:eq(' + mySwiper.activeIndex + ')').find('.selectedAnswer').val();
                if (answerIndex !== '') {
                    var answerDiv = '#option' + answerIndex;
                    $(".icon-circle").hide();
                    $(answerDiv).find(".icon-circle").show();
                }
            });

            quizintervalInstance = setInterval(function () {
                elapsed_seconds = elapsed_seconds - 1;
                $("#timeClock").show();
                $('#timeClock').text(getElapsedTimeString(duration));
                if (elapsed_seconds === 0) {
                    ResultPage(challengeId);
                    clearInterval(quizintervalInstance);
                    setTimeout(function () {
                        $("#timeClock").hide();
                    }, 1500);
                }
            }, 1000);
        }, 100);

        $(document).on("click", ".pagedBox", function () {
            var ind = $(this).html().trim();
            console.log("pagedBox click " + ind);
            mySwiper.slideTo(parseInt(ind) - 1, 1000, false);
        });
        $(document).on("click", ".questionBtn", function () {
            var $questionBtn = $(this);
            var ind = $questionBtn.attr('id');
            ind = ind.replace('option', '');
            var questionId = $('#caruselSlider #sliderWrapper .swiper-slide:eq(' + mySwiper.activeIndex + ')').find('.questionIds').val();
            var challengeId = $("#challengeIdHdn").val();
            SetAnswer(challengeId, ind, questionId, mySwiper.activeIndex);
        });

        $('.swiper-pagination').css("display", "none");


        if (questionData[currentQuestion] === 5) {
            $("#fiveOption").show();
        }
        else {
            $("#fiveOption").hide();
        }

        function SetAnswer(challengeId, answerIndex, questionId, sliderIndex) {
            var obj = {
                'ChallengeId': challengeId,
                'AnswerIndex': answerIndex,
                'QuestionId': questionId,
                'userId': aqfw().Auth().GetuserId()
            };
            $.ajax({
                type: "POST", //GET, POST, PUT   
                url: aqfw().ServiceUrl + '/Question/SetChallengeAnswer',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(obj),
                beforeSend: function (xhr) {   //Include the bearer token in header
                    xhr.setRequestHeader("Authorization", 'Bearer ' + aqfw().Auth().GetToken());
                },
                success: function (data) {
                    var currentAnswer = $('#caruselSlider #sliderWrapper .swiper-slide:eq(' + sliderIndex + ')').find('.selectedAnswer').val();
                    $('#caruselSlider #sliderWrapper .swiper-slide:eq(' + sliderIndex + ')').find('.selectedAnswer').val(answerIndex);

                    if (currentAnswer === '') { // if this action is first answer
                        var answered = $("#answeredQuestionCount").text();
                        var unanswered = $("#notAnsweredQuesitonCount").text();
                        $("#answeredQuestionCount").text(parseInt(answered) + 1);
                        $("#notAnsweredQuesitonCount").text(parseInt(unanswered) - 1);
                    }
                    var answerDiv = '#option' + answerIndex;
                    $(".icon-circle").hide();
                    $(".questionBtn").removeClass('answer-option-item-choiced');
                    $(answerDiv).find(".icon-circle").show();
                    $(answerDiv).addClass('answer-option-item-choiced');
                    var questionLen = $("#questionLenHdn").val();
                    setTimeout(function () {
                        if ((sliderIndex + 1) !== parseInt(questionLen)) {
                            mySwiper.slideTo(sliderIndex + 1, 1000, false);
                            $(".icon-circle").hide();
                        }
                        else {
                            // alert('Tüm soruları cevapladın. Geri kalan süre içerisinde cevaplarını kontrol edebilirsin');
                            ResultPage(challengeId);
                        }
                    }, 1000);

                }
            });
        }
    }
    function GetQuestion(challengeId) {
        $.ajax({
            type: "GET", //GET, POST, PUT   
            url: aqfw().ServiceUrl + '/Question/GetChallengeQuestions?ChallengeId=' + challengeId,  //the url to call
            contentType: "application/json; charset=utf-8",
            beforeSend: function (xhr) {   //Include the bearer token in header
                xhr.setRequestHeader("Authorization", 'Bearer ' + aqfw().Auth().GetToken());
            },
            success: function (data) {
                StartQuiz(data);
            }
        });
    }
    function ResultPage(challengeId) {

        socketConnection(challengeId);
        $("#participantTable").hide();
        $("#question_wrapper").hide();
        $("#challenge_result").show();
        $("#page-boxes").parent().hide();
    }
};

var RouteScript = function (route) {
    if (route === aqfw().Navigation().Routes.Challenge) {
        socketConnection(0);
    }
};


function getElapsedTimeString(total_seconds) {
    function pretty_time_string(num) {
        return (num < 10 ? "0" : "") + num;
    }

    var hours = Math.floor(total_seconds / 3600);
    total_seconds = total_seconds % 3600;

    var minutes = Math.floor(total_seconds / 60);
    total_seconds = total_seconds % 60;

    var seconds = Math.floor(total_seconds);

    hours = pretty_time_string(hours);
    minutes = pretty_time_string(minutes);
    seconds = pretty_time_string(seconds);

    var currentTimeString = hours + ":" + minutes + ":" + seconds;

    return currentTimeString;
}
