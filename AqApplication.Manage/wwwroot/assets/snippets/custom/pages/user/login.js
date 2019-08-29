var SnippetLogin = function () {
    var e = $("#m_login"),
        i = function (e, i, a) {
            var l = $('<div class="m-alert m-alert--outline alert alert-' + i + ' alert-dismissible" role="alert">\t\t\t<button type="button" class="close" data-dismiss="alert" aria-label="Close"></button>\t\t\t<span></span>\t\t</div>');
            e.find(".alert").remove(), l.prependTo(e), mUtil.animateClass(l[0], "fadeIn animated"), l.find("span").html(a)
        },
        a = function () {
            e.removeClass("m-login--forget-password"), e.removeClass("m-login--signup"), e.addClass("m-login--signin"), mUtil.animateClass(e.find(".m-login__signin")[0], "flipInX animated")
        },
        l = function () {
            $("#m_login_forget_password").click(function (i) {
                i.preventDefault(), e.removeClass("m-login--signin"), e.removeClass("m-login--signup"), e.addClass("m-login--forget-password"), mUtil.animateClass(e.find(".m-login__forget-password")[0], "flipInX animated")
            }), $("#m_login_forget_password_cancel").click(function (e) {
                e.preventDefault(), a()
            }), $("#m_login_signup").click(function (i) {
                i.preventDefault(), e.removeClass("m-login--forget-password"), e.removeClass("m-login--signin"), e.addClass("m-login--signup"), mUtil.animateClass(e.find(".m-login__signup")[0], "flipInX animated")
            }), $("#m_login_signup_cancel").click(function (e) {
                e.preventDefault(), a()
            })
        };
    return {
        init: function () {
            l(), $("#m_login_signin_submit").click(function (e) {
                e.preventDefault();
                var a = $(this),
                    l = $(this).closest("form");
                l.validate({
                    rules: {
                        email: {
                            required: !0,
                            email: !0
                        },
                        password: {
                            required: !0
                        }
                    }
                }), l.valid() && (a.addClass("m-loader m-loader--right m-loader--light").attr("disabled", !0), l.ajaxSubmit({
                    type:"POST",
                    url: "/Account/Login",
                    data: {
                        "Email": $("#login_Email").val(),
                        "Password": $("#login_Password").val(),
                        "RememberMe": $("#login_RememberMe").is(':checked'),
                        "returnUrl": $("#login_ReturnUrl").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            window.location.href = data.returnUrl;
                        }
                        else {
                            setTimeout(function () {
                                a.removeClass("m-loader m-loader--right m-loader--light").attr("disabled", !1), i(l, "danger", data.message);
                            }, 2e3);
                        }
                    }
                }))
            }), $("#m_login_signup_submit").click(function (l) {
                l.preventDefault();
                var t = $(this),
                    r = $(this).closest("form");
                r.validate({
                    rules: {
                        firstName: {
                            required: !0
                        },
                        lastName: {
                            required: !0
                        },
                        email: {
                            required: !0,
                            email: !0
                        },
                        password: {
                            required: !0
                        },
                        rpassword: {
                            required: !0
                        },
                        agree: {
                            required: !0
                        }
                    }
                }), r.valid() && (t.addClass("m-loader m-loader--right m-loader--light").attr("disabled", !0), r.ajaxSubmit({
                    type: "POST",
                    url: "/Account/Register",
                    data: {
                        "FirstName": $("#register_FirstName").val(),
                        "LastName": $("#register_LastName").val(),
                        "Email": $("#register_Email").val(),
                        "Password": $("#register_Password").val(),
                        "ConfirmPassword": $("#register_RePassword").val(),
                        "returnUrl": $("#login_ReturnUrl").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            $("#login_form")
                                .prepend('<div class="m-alert m-alert--outline alert alert-success alert-dismissible animated fadeIn" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"></button><span>' + data.message + '</span></div>');
                            setTimeout(function () {
                                t.removeClass("m-loader m-loader--right m-loader--light").attr("disabled", !1), r.clearForm(), r.validate().resetForm(), a();
                                var l = e.find(".m-login__signin");
                                l.clearForm(), l.validate().resetForm(),
                                    i(l, "success", data.message);
                            }, 2e3);

                        }
                        else {
                            setTimeout(function () {
                                t.removeClass("m-loader m-loader--right m-loader--light").attr("disabled", !1);
                            }, 2e3);
                            var l = e.find(".m-login__signin");
                            i(l, "danger", data.message);

                        }
                    }
                }));
            }), $("#m_login_forget_password_submit").click(function (l) {
                l.preventDefault();
                var t = $(this),
                    r = $(this).closest("form");
                r.validate({
                    rules: {
                        email: {
                            required: !0,
                            email: !0
                        }
                    }
                }), r.valid() && (t.addClass("m-loader m-loader--right m-loader--light").attr("disabled", !0), r.ajaxSubmit({
                    url: "",
                    success: function (l, s, n, o) {
                        setTimeout(function () {
                            t.removeClass("m-loader m-loader--right m-loader--light").attr("disabled", !1), r.clearForm(), r.validate().resetForm(), a();
                            var l = e.find(".m-login__signin form");
                            l.clearForm(), l.validate().resetForm(), i(l, "success", "Cool! Password recovery instruction has been sent to your email.")
                        }, 2e3)
                    }
                }))
            })
        }
    }
}();
jQuery(document).ready(function () {
    SnippetLogin.init();
});