//const serviceUrl = 'http://85.105.160.53:81/webapi/api';
//const challangeSocketUrl = 'ws://85.105.160.53:81/webapi';

const serviceUrl = 'http://localhost:50999/api';
const challangeSocketUrl = 'ws://localhost:50999';

var examId = '3';
var questionData = null;
var currentQuestion = 0;
var mySwiper = null;
var aqfw = function () {
    var currentPage = null;
    var navigation = function () {
        const routes = {
            SingIn: "signin2.html",
            SingUp: "signup2.html",
            Index: "menu.html",
            Challenge: "challenge.html",
            Statistics: "statistics.html",
            History: "history.html",
            Antreman: "antreman.html"
        };
        var redirect = function (route) {
            $('.loader').show();
            $(".wrapper .container-wrapper").load(route, function (response, status, xhr) {
                if (route === routes.SingIn || route === routes.SingUp || route === routes.Statistics || route === routes.Index) {
                    $('.loader').hide();
                }
                if (xhr.status !== 200) {
                    messageBox().ShowWarning('Hata', 'Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz');
                }
                else {
                    SetStyleContainer(route);
                    RouteScript(route);
                }
                currentPage = route;
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
            else if (route === routes.Statistics) {
                $("#footer").hide();
                SetCssClass('content-color-secondary', 'content-color-secondary-white', false);
                $("#page-boxes").parent().hide();
            }
            else if (route === routes.Antreman) {
                $("#footer").hide();
                SetCssClass('content-color-secondary', 'content-color-secondary-white', false);
                $("#page-boxes").parent().hide();
            }
            else if (route === routes.History) {
                $("#footer").hide();
                SetCssClass('content-color-secondary', 'content-color-secondary-white', false);
                $("#page-boxes").parent().hide();
            }
            else if (route === routes.Challenge) {
                $('.loader').show();
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
            Redirect: redirect,
            CurrentPage: currentPage
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
        var showWarningMessageBoxOk = function (heading, message, buttonText) {
            $("#messageBoxHeader").text(heading);
            $("#messageBoxContent").text(message);
            $("#messageBoxCloseButton").text(buttonText);
            $("#messageBox").modal("show");
        };
        return {
            ShowWarning: showWarning,
            ShowSucess: showSucess,
            ShowWarningMessageBoxOk: showWarningMessageBoxOk
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
var intervalInstance_next = null;
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
        aqfw().MessageBox().ShowWarningMessageBoxOk("Uyarı", 'Lütfen internet bağlantınızı kontrol ediniz!', "Kapat");
    };
    socket.onclose = function () {
        // setTimeout(function () { socketConnection(); }, 5000);
    };
    socket.onmessage = function (e) {
        console.log(e.data);
        var cnt = 0;
        var socketResponse = JSON.parse(e.data);
        $('.loader').hide();
        if (socketResponse.ChallengeSocketResult === 1) { //error
            aqfw().MessageBox().ShowWarningMessageBoxOk("Uyarı", socketResponse.Message, "Kapat");
            aqfw().Navigation().Redirect(aqfw().Navigation().Routes.Index);
            return;
        }
        if (socketResponse.ChallengeSocketResult === 2) { // next request

            setTimeout(function () {
                ChallengeEntry();
            }, socketResponse.Next * 1000);

            var elapsed_seconds_next = socketResponse.LeftSecond;
            if (intervalInstance_next !== null) {
                clearInterval(intervalInstance_next);
            }
            intervalInstance_next = setInterval(function () {
                elapsed_seconds_next = elapsed_seconds_next - 1;
                $("#timeClock").show();
                $('#timeClock').text(getElapsedTimeString(elapsed_seconds_next));
                if (elapsed_seconds_next === 0) {
                    clearInterval(intervalInstance_next);
                }
            }, 1000);


            return;
        }
        if (challengeId === 0) { //entry success
            var $table = $("#participantTable tbody");
            $table.html('');
            console.log(socketResponse);
            if (socketResponse.SocketClientModelList.length) {
                $.each(socketResponse.SocketClientModelList, function (i, v) {
                    cnt++;
                    var str = ' <tr>' +
                        '<td style="text-align:center;">' +
                        '<h6 class="my-0 mt-1">' + cnt + '</h6>' +
                        '</td>' +
                        '<td class="footable-first-visible footable-last-visible" style = "display: table-cell;" >' +
                        '<div class="media">' +
                        '<figure class="avatar avatar-50 mr-3">' +
                        '<img src="img/user1.png" alt="Generic placeholder image">' +
                        '</figure>' +
                        '<div class="media-body">' +
                        '<h6 class="my-0 mt-1">' + v.FullName + ' <span class="status vm bg-success"></span></h6>' +
                        '<p class="small">Yeni Kullanıcı</p>' +
                        '</div>' +
                        '</div>' +
                        '</td>' +

                        '<td style="display: none;">' +
                        '<h6 class="my-0 mt-1"></h6>' +
                        '</td>' +
                        '</tr >';

                    $table.append(str);
                });
            }
            if (socketResponse.LeftSecond > -1) {
                if (intervalInstance !== null) { //!! important otherwise can be set only one time
                    intervalInstance = null;
                }
                var elapsed_seconds = socketResponse.LeftSecond;
                if (intervalInstance !== null) {
                    clearInterval(intervalInstance);
                }
                if (intervalInstance_next !== null) {
                    clearInterval(intervalInstance_next);
                }
                intervalInstance = setInterval(function () {
                    elapsed_seconds = elapsed_seconds - 1;
                    $("#timeClock").show();
                    $('#timeClock').text(getElapsedTimeString(elapsed_seconds));
                    if (elapsed_seconds === 0) {
                        clearInterval(intervalInstance);
                        SliderInit(socketResponse.QuizDuration, socketResponse.ChallengeId, 1);
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
                    var str = ' <tr>' +
                        '<td class="footable-first-visible footable-last-visible" style = "display: table-cell;" >' +
                        '<div class="media">' +
                        '<figure class="avatar avatar-50 mr-3">' +
                        '<img src="img/user1.png" alt="Generic placeholder image">' +
                        '</figure>' +
                        '<div class="media-body">' +
                        '<h6 class="my-0 mt-1">' + v.FullName + ' <span class="status vm bg-success"></span></h6>' +
                        '<p class="small">Yeni Kullanıcı</p>' +
                        '</div>' +
                        '</div>' +
                        '</td>' +
                        '<td style="text-align:center;">' +
                        '<h6 class="my-0 mt-1">' + v.TotalMark + '</h6>' +
                        '<p class="content-color-secondary small mb-0">Puan</p>' +
                        '</td>' +
                        '<td style="display: none;">' +
                        '<h6 class="my-0 mt-1">' + v.TotalMark + '</h6>' +
                        '</td>' +

                        '</tr >';
                    $tableResult.append(str);
                });

            }
        }
    };



    function ChallengeEntry() {
        if (!socket || socket.readyState !== WebSocket.OPEN) {
            console.error("Houston we have a problem! Socket not connected.");
        }
        var enterRoom = {
            'userId': aqfw().Auth().GetuserId()
        };
        console.log(JSON.stringify(enterRoom));
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


};

var RouteScript = function (route) {
    if (route === aqfw().Navigation().Routes.Challenge) {
        socketConnection(0);
    }
    else if (route === aqfw().Navigation().Routes.Antreman) {
        LoadAntremanModeLectures(examId);
    }
    else if (route === aqfw().Navigation().Routes.History) {
        LoadStatisticChartData();
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
function LoadAntremanModeLectures(examId) {

    $.ajax({
        type: "GET", //GET, POST, PUT   
        url: aqfw().ServiceUrl + '/Question/GetPractiveModeLectures?ExamId=' + examId,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {   //Include the bearer token in header
            xhr.setRequestHeader("Authorization", 'Bearer ' + aqfw().Auth().GetToken());
        },
        success: function (data) {
            var str = '';
            var $el = $("#lectures-wrapper");
            $el.html("");
            $.each(data, function (i, v) {
                console.log(i + ' ' + v);
                str = '  <div class="answer-option-item questionBtn lectureOption" data-lectureId=' + i + '>' +
                    '<div class="card" style = "padding:0.5em 1.95em">' +
                    '<div class="media">' +
                    '<div class="media-body">' +
                    '<h4 class="mb fleft">' + v + '</h4>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div >';
                $el.append(str);
            });

            $(".lectureOption").click(function () {
                var lectureId = $(this).attr('data-lectureId');
                console.log(lectureId);
                GetPractiveModeQuestion(lectureId);
            });

            $('.loader').hide();

        },
        error: function (jqXHR, textStatus, errorThrown) {
            aqfw().MessageBox().ShowWarningMessageBoxOk("Uyarı", 'Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz', "Kapat");
            aqfw().Navigation().Redirect(aqfw().Navigation().Routes.Index);
        }
    });
    return;

}

function GetPractiveModeQuestion(lectureId) {
    $.ajax({
        type: "GET", //GET, POST, PUT   
        url: aqfw().ServiceUrl + '/Question/CreatePractiveModeChallenge?lectureId=' + lectureId + '&userId=' + aqfw().Auth().GetuserId(),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {   //Include the bearer token in header
            xhr.setRequestHeader("Authorization", 'Bearer ' + aqfw().Auth().GetToken());
            $('.loader').show();
        },
        success: function (data) {
            if (!data.success) {
                $('.loader').hide();
                aqfw().MessageBox().ShowWarningMessageBoxOk("Üzgünüz", data.message, "Kapat");
                return;
            }

            $("#challengeIdHdn").val(data.challangeId);
            StartQuiz(data.data);
            SliderInit(data.duration, data.challangeId, 2);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            aqfw().MessageBox().ShowWarningMessageBoxOk("Uyarı", 'Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz', "Kapat");
            aqfw().Navigation().Redirect(aqfw().Navigation().Routes.Index);
        }
    });
}
function StartQuiz(questionList) {
    questionData = questionList;

    $("#page-boxes").html("");
    $.each(questionData, function (i, v) {
        var questionDiv = '<div class="swiper-slide text-center">' +
            ' <p class="questionMainTitle">' + v.mainText + '</p>' +
            ' <input type="hidden" value="' + v.questionId + '" class="questionIds"/>';
        $.each(v.challengeAnswerViewModel, function (t, k) {
            var classNameOption = '';
            if (k.index === 0) {
                classNameOption = 'optionAHidden';
            }
            else if (k.index === 1) {
                classNameOption = 'optionBHidden';
            }
            else if (k.index === 2) {
                classNameOption = 'optionCHidden';
            }
            else if (k.index === 3) {
                classNameOption = 'optionDHidden';
            }
            else if (k.index === 4) {
                classNameOption = 'optionEHidden';
            }
            questionDiv += ' <input type="hidden" value="' + k.title + '" class="' + classNameOption + '"/>';
        });
        questionDiv += ' <input type="hidden" value="' + v.answerCount + '" class="optionAnswerCount"/>';
        questionDiv += ' <input type="hidden" value="" class="selectedAnswer"/>' +
            '</div>';
        var $questionDiv = $(questionDiv);
        $questionDiv.appendTo('#caruselSlider #sliderWrapper');

        var $pageBtn = $('<button type="button" class="btn btn-sm btn-outline-dark mr-1 pagedBox">' + (i + 1) + '</button>');
        if (i === 0) {
            $pageBtn.addClass('active');
        }
        $pageBtn.appendTo('#page-boxes');
    });
    $("#totalQuestion").text(questionData.length);
    $("#questionLenHdn").val(questionData.length);
    $("#answeredQuestionCount").text('0');

}
var quizintervalInstance = null;
function SliderInit(duration, challengeId, challengeType) {
    if (challengeType === 2) {
        $("#lectures-wrapper").hide();
        $("#antreman-header").hide();
    }
    else {
        $("#participantTable").hide();
    }
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
            console.log("slidechange");
            $(".pagedBox").removeClass('active');
            $('#page-boxes .pagedBox:eq(' + mySwiper.activeIndex + ')').addClass('active');
            $(".questionBtn").removeClass('answer-option-item-choiced');
            OptionInt(mySwiper.activeIndex);
            $('#page-boxes').animate({ scrollLeft: $('#page-boxes .pagedBox:eq(' + mySwiper.activeIndex + ')').position().left - 13 }, 500);
        });


        var elapsed_seconds = duration;
        quizintervalInstance = setInterval(function () {
            elapsed_seconds = elapsed_seconds - 1;
            $('#timeClockQuestion').text(getElapsedTimeString(elapsed_seconds));
            if (elapsed_seconds === 0) {
                ResultPage(challengeId, challengeType);
                clearInterval(quizintervalInstance);
            }
        }, 1000);
        OptionInt(0);
        $('.loader').hide();
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

    function OptionInt(index) {
        var $currectSliderEl = $('#caruselSlider #sliderWrapper .swiper-slide:eq(' + index + ')');
        var answerIndex = $currectSliderEl.find('.selectedAnswer').val();
        var optionAnwerCount = $currectSliderEl.find(".optionAnswerCount").val();
        var asdsd = $currectSliderEl.find('.optionAHidden').val();
        $("#option0Text").text(asdsd);
        $("#option1Text").text($currectSliderEl.find('.optionBHidden').val());
        $("#option2Text").text($currectSliderEl.find('.optionCHidden').val());

        if (optionAnwerCount === '3') {
            $("#fourOption").hide();
            $("#fiveOption").hide();
        }
        else if (optionAnwerCount === '4') {
            $("#fiveOption").hide();
            $("#fourOption").show();
            $("#option3Text").text($currectSliderEl.find('.optionDHidden').val());
        }
        else if (optionAnwerCount === '5') {
            $("#fourOption").show();
            $("#fiveOption").show();
            $("#option3Text").text($currectSliderEl.find('.optionDHidden').val());
            $("#option4Text").text($currectSliderEl.find('.optionEHidden').val());
        }
        if (answerIndex !== '') {
            var answerDiv = '#option' + answerIndex;
            $(answerDiv).addClass('answer-option-item-choiced');
        }
    }


    function SetAnswer(challengeId, answerIndex, questionId, sliderIndex) {
        var obj = {
            'ChallengeId': challengeId,
            'AnswerIndex': answerIndex,
            'QuestionId': questionId,
            'userId': aqfw().Auth().GetuserId()
        };
        $.ajax({
            type: "POST",
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
                    $("#answeredQuestionCount").text(parseInt(answered) + 1);
                }
                var answerDiv = '#option' + answerIndex;
                $(".questionBtn").removeClass('answer-option-item-choiced');
                $(answerDiv).addClass('answer-option-item-choiced');
                var questionLen = $("#questionLenHdn").val();
                setTimeout(function () {
                    if ((sliderIndex + 1) !== parseInt(questionLen)) {
                        mySwiper.slideTo(sliderIndex + 1, 1000, false);
                    }
                    else {
                        // alert('Tüm soruları cevapladın. Geri kalan süre içerisinde cevaplarını kontrol edebilirsin');
                        ResultPage(challengeId, challengeType);
                    }
                }, 1000);
                $('#page-boxes .pagedBox:eq(' + sliderIndex + ')').css({
                    'color': 'black',
                    'background-color': '#ccc'
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                aqfw().MessageBox().ShowWarningMessageBoxOk("Uyarı", 'Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz', "Kapat");
                aqfw().Navigation().Redirect(aqfw().Navigation().Routes.Index);
            }
        });
    }
}

function GetQuestion(challengeId) {
    $.ajax({
        type: "GET", //GET, POST, PUT   
        url: aqfw().ServiceUrl + '/Question/GetChallengeQuestions?ChallengeId=' + challengeId + '&userId=' + aqfw().Auth().GetuserId(),  //the url to call
        contentType: "application/json; charset=utf-8",
        beforeSend: function (xhr) {   //Include the bearer token in header
            xhr.setRequestHeader("Authorization", 'Bearer ' + aqfw().Auth().GetToken());
        },
        success: function (data) {
            StartQuiz(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            aqfw().MessageBox().ShowWarningMessageBoxOk("Uyarı", 'Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz', "Kapat");
            aqfw().Navigation().Redirect(aqfw().Navigation().Routes.Index);
        }
    });
}
function ResultPage(challengeId, challengeType) {
    $('.loader').show();
    if (challengeType === 1) {
        socketConnection(challengeId);
        $("#participantTable").hide();
        $("#question_wrapper").hide();
        $("#challenge_result").show();
        $("#page-boxes").parent().hide();
    }
    else { // or practive mode


        $("#challenge_result").load('result.html', function (response, status, xhr) {

            if (xhr.status !== 200) {
                messageBox().ShowWarning('Hata', 'Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz');
            }
            else {
                $.ajax({
                    type: "GET",
                    url: aqfw().ServiceUrl + '/Question/GetResultPractiveModeChallenge?ChallengeId=' + challengeId + '&userId=' + aqfw().Auth().GetuserId(),  //the url to call
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function (xhr) {   //Include the bearer token in header
                        xhr.setRequestHeader("Authorization", 'Bearer ' + aqfw().Auth().GetToken());
                    },
                    success: function (data) {
                        $("#question_wrapper").hide();

                        $("#resultMark").text(data.challengeUserViewModel.mark);
                        $("#resultCorrectcount").text(data.challengeUserViewModel.correct);
                        $("#resultDuration").text(data.challengeUserViewModel.duration);

                        $("#resultMarkProgressBar").attr('aria-valuenow', '50');
                        $("#resultCorrectcountProgressBar").attr('aria-valuenow', '50');
                        $("#resultDurationProgressBar").css('width', data.challengeUserViewModel.durationPercentage + '%').attr('aria-valuenow', data.challengeUserViewModel.durationPercentage);

                        var sortedQuestionAnswerViewModel = data.questionAnswerViewModel.sort(function (a, b) {
                            if (a.seo > b.seo) {
                                return 1;
                            }
                            if (a.seo < b.seo) {
                                return -1;
                            }
                            return 0;
                        });

                        $("#answerTabContent").html(GenerateResultOpticalAnswer(sortedQuestionAnswerViewModel));

                        $("#challenge_result").show();
                        $('.loader').hide();

                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        aqfw().MessageBox().ShowWarningMessageBoxOk("Uyarı", 'Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz', "Kapat");
                        aqfw().Navigation().Redirect(aqfw().Navigation().Routes.Index);
                    }
                });
            }
        });
    }


}


function GenerateResultOpticalAnswer(questionAnswerViewModel) {

    var optionList = "";

    $.each(questionAnswerViewModel, function (i, v) {
        optionList += OpticalAnswerRow(v.answerCount, v.seo, v.correctAnswer, v.userAnswer, v.questionId);
    });

    return optionList;
}

function OpticalAnswerRow(answerCount, seo, correctAnswer, userAnswer, questionId) {
    var row = '<div class="opticalAnswerRow">' +
        '<div class="col-sm-10" style="max-width:100% !important;">' +
        '<div class="bmd-form-group" style="cursor:pointer;">' +
        '<div class="form-check" style="display: inline-flex;">' +
        '<a style="padding-right: 10px; padding-top: 4px;" href="#">' + seo + '.</a>';

    for (var i = 0; i < answerCount; i++) {
        row += OpticalAnswerRadio(i, i === correctAnswer, i === userAnswer);
    }

    row += '<input type="button" class="btn btn-default btn-sm" value="?"  onclick="ShowAnsweredQuestion(' + seo + ');" /></div>' +
        '</div>' +
        '</div>' +
        '</div>';
    return row;
}
function OpticalAnswerRadio(optionIndex, isCorrectAnswer, isUserAnswer) {
    var letter = '';
    if (optionIndex === 0) {
        letter = 'A';
    }
    else if (optionIndex === 1) {
        letter = 'B';
    }
    else if (optionIndex === 2) {
        letter = 'C';
    }
    else if (optionIndex === 3) {
        letter = 'D';
    }
    else if (optionIndex === 4) {
        letter = 'E';
    }

    var answerClass = '';

    if (isCorrectAnswer && isUserAnswer) {
        answerClass = ' correct-confirmed';
    }
    else if (!isCorrectAnswer && isUserAnswer) {
        answerClass = ' wrong-answer';
    }

    var option = '<label class="form-check-label" style="padding-right: 10px !important;">' +
        '<input class="form-check-input" type="radio" name="yanit122" id="yanit122" value="' + optionIndex + '">' +
        '<span class="circle';

    if (answerClass !== '') {
        option += answerClass;
    }

    option += '">' +
        '<span class="check-textqqx2">' + letter + '</span><span class="check"></span>' +
        '</span>' +
        '</label>';

    return option;
}
function ShowAnsweredQuestion(seo) {
    var $currectSliderEl = $('#caruselSlider #sliderWrapper .swiper-slide:eq(' + (seo - 1) + ')');
    var questionMainTitle = $currectSliderEl.find('.questionMainTitle').html();
    var optionAnwerCount = $currectSliderEl.find(".optionAnswerCount").val();
    $("#questionPreviewModalContent").text(questionMainTitle);

    $("#option0TextModal").text('A) ' + $currectSliderEl.find('.optionAHidden').val());
    $("#option1TextModal").text('B) ' + $currectSliderEl.find('.optionBHidden').val());
    $("#option2TextModal").text('C) ' + $currectSliderEl.find('.optionCHidden').val());

    if (optionAnwerCount === '3') {
        $("#option3TextModal").hide();
        $("#option4TextModal").hide();
    }
    else if (optionAnwerCount === '4') {
        $("#option4TextModal").hide();
        $("#option3TextModal").show();
        $("#option3TextModal").text('D) ' + $currectSliderEl.find('.optionDHidden').val());
    }
    else if (optionAnwerCount === '5') {
        $("#option3TextModal").show();
        $("#option4TextModal").show();
        $("#option3TextModal").text('D) ' + $currectSliderEl.find('.optionDHidden').val());
        $("#option4TextModal").text('E) ' + $currectSliderEl.find('.optionEHidden').val());
    }
    $("#questionPreviewModal").modal('show');


}
function LoadChallangeHistory() {
    $.ajax({
        type: 'GET',
        url: aqfw().ServiceUrl + '/Question/GetHistoryChallenges?userId=' + aqfw().Auth().GetuserId(),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {   //Include the bearer token in header
            xhr.setRequestHeader("Authorization", 'Bearer ' + aqfw().Auth().GetToken());
        },
        success: function (data) {
            var $table = $("#historyTable tbody");
            $table.html("");

            if (typeof data !== 'undefined' && data) {
                var sortedHistroyChallengeList = data.sort(function (a, b) {
                    if (a.challengeId > b.challengeId) {
                        if (a.challengeId > b.challengeId) {
                            return 1;
                        }
                        if (a.challengeId < b.challengeId) {
                            return -1;
                        }
                        return 0;
                    }
                });

                $.each(sortedHistroyChallengeList, function (i, v) {
                    $table.append("<tr><td>" + v.date + "</td><td>" + v.hour + "</td><td>" + v.challengeType + "</td><td>" + v.mark + "</td></tr>");
                });
            };

            $('.loader').hide();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            aqfw().MessageBox().ShowWarningMessageBoxOk("Uyarı", 'Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz', "Kapat");
            aqfw().Navigation().Redirect(aqfw().Navigation().Routes.Index);
        }
    });


    $('#historyTable').dataTable({
        "language": {
            "url": "/_assets/packages/datatables/datatable.turkish.json"
        },
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "pageLength": 10,
        "ajax": {
            "url": aqfw().ServiceUrl + '/Question/GetHistoryChallenges?userId=' + aqfw().Auth().GetuserId(),
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "name": "",
                "data": "ImagePath",
                "width": "50px",
                "orderable": false,
                "render": function (data, type, full, meta) {
                    return '<a href="' + full.ImagePath + '" target="_blank">' +
                        '<img src="' + full.ImagePath + '" width="50" height="50" class="rounded-circle">' +
                        '</a>';
                }
            },
            {
                "data": "FirstName", "name": "Ad Soyad", "autoWidth": true,
                "render": function (data, type, full, meta) {
                    return full.FirstName + " " + full.LastName;
                }
            },
            {
                "data": "DepartmenName", "name": "Departman", "autoWidth": true, "orderable": false
            },
            {
                "name": "Durum",
                "autoWidth": true,
                "data": "IsActive",
                "render": function (data, type, full, meta) {
                    return full.IsActive ? "Aktif" : "Pasif";
                }
            },
            {
                "orderable": false,
                "width": "100px",
                "render": function (data, type, full, meta) {
                    var returnVal = '';
                    returnVal += '<a class="btn btn-sm btn-edit" href="/employee/Edit/' + full.Id + '" style="margin-right:10px;">Güncelle</a>';
                    returnVal += '<a class="btn btn-sm btn-danger" data-toggle="confirmation" href="/employee/Delete/' + full.Id + '">Sil</a>';

                    return returnVal;
                }
            }
        ],
        "initComplete": function (settings, json) {
            $('[data-toggle=confirmation]').confirmation({
                rootSelector: '[data-toggle=confirmation]',
                title: '@L.ARE_U_SURE'
            });
        }
    });
}

function LoadStatisticChartData() {
    $.ajax({
        type: 'GET',
        url: aqfw().ServiceUrl + '/Question/GetStatisticChartData?userId=' + aqfw().Auth().GetuserId() + '&Day=180',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {   //Include the bearer token in header
            xhr.setRequestHeader("Authorization", 'Bearer ' + aqfw().Auth().GetToken());
        },
        success: function (data) {
            var $table = $("#historyTable tbody");
            $table.html("");

            var sortedChartDataList = data.sort(function (a, b) {
                if (a.challengeId > b.challengeId) {
                    if (a.challengeId > b.challengeId) {
                        return 1;
                    }
                    if (a.challengeId < b.challengeId) {
                        return -1;
                    }
                    return 0;
                }
            });
            var dateArr = [];
            var challengeType = [];
            var datasetArray = [];

            for (var i = 0; i < sortedChartDataList.length; i++) {
                if (dateArr.indexOf(sortedChartDataList[i].date) === -1) {
                    dateArr.push(sortedChartDataList[i].date);
                }
                var challengeTypeIndex = challengeType.indexOf(sortedChartDataList[i].challengeTypeId);
                if (challengeTypeIndex === -1) {
                    challengeType.push(sortedChartDataList[i].challengeTypeId);
                    datasetArray.push({
                        label: sortedChartDataList[i].challengeType,
                        backgroundColor: dynamicColors(),
                        borderColor: dynamicColors(),
                        data: [{
                            x: (challengeTypeIndex * 10),
                            y: sortedChartDataList[i].mark
                        }]
                    });
                }
                else {
                    datasetArray[challengeTypeIndex].data.push({
                        x: (challengeTypeIndex * 10),
                        y: sortedChartDataList[i].mark
                    });
                }
            }
       
            function dynamicColors() {
                var r = Math.floor(Math.random() * 255);
                var g = Math.floor(Math.random() * 255);
                var b = Math.floor(Math.random() * 255);
                return "rgba(" + r + "," + g + "," + b + ", 0.5)";
            }
            var ctx = document.getElementById('myChart').getContext('2d');

            var config = {
                type: 'line',
                data: {
                    labels: dateArr,
                    datasets: datasetArray
                },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'Başarı Grafiği'
                    },
                    tooltips: {
                        mode: 'index',
                        intersect: false
                    },
                    hover: {
                        mode: 'nearest',
                        intersect: true
                    },
                    scales: {
                        xAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                labelString: 'Tarih'
                            }
                        }],
                        yAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                labelString: 'Puan'
                            }
                        }]
                    }
                }
            };

            var myChart = new Chart(ctx, config);

            $('.loader').hide();
            LoadChallangeHistory();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            aqfw().MessageBox().ShowWarningMessageBoxOk("Uyarı", 'Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz', "Kapat");
            aqfw().Navigation().Redirect(aqfw().Navigation().Routes.Index);
        }
    });
}