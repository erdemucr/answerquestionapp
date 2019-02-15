using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace AqApplication.Manage.Models
{
    public static class Utilities
    {
        public static string StatusText(this bool status)
        {

            return status ? "Aktif" : "Pasif";

        }
        public static string TrueText(this bool isTrue)
        {

            return isTrue ? "Doğru" : "Yanlış";

        }
        public static class Messages
        {

            public const string FormNotValidError = "Lütfen alanları kontrol ederek tekrar deneyiniz";
            public const string UserLockError = "Kullanıcı hesabı kilitli, lütfen sistem yöneticinize danışınız";
            public const string NotValidUserError = "Kullanıcı bilgileri doğru değil lütfen tekrar deneyiniz";
            public const string EPostaConfrim = "Doğrulama linki e-posta adresinize gönderilmiştir";
        }

        public static class SelectLists
        {
            public static List<SelectListItem> Difficult()
            {

                return new List<SelectListItem>
               {
                   new SelectListItem{ Value="1", Text="Kolay" },
                   new SelectListItem{ Value="2", Text="Orta"},
                   new SelectListItem{ Value="3", Text="Zor"}
               };
            }

            public static List<SelectListItem> Licence()
            {

                return new List<SelectListItem>
               {
                   new SelectListItem{ Value="false", Text="Lisanslı" },
                   new SelectListItem{ Value="true", Text="Linsansız"}
               };
            }

        }
    }
}