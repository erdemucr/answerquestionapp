using AqApplication.Entity.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

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
        public static string DifficultText(this int? diff)
        {
            switch (diff)
            {
                case 1: return "Kolay";
                case 2: return "Orta";
                case 3: return "Zor";
                default: return string.Empty;
            }
        }
        public static string ChallengeTypeEnumText(this  ChallengeTypeEnum type)
        {
            switch (type)
            {
                case ChallengeTypeEnum.RandomMode: return "Challenge Mode";
                case ChallengeTypeEnum.PracticeMode: return "Practice Mode";
                default: return string.Empty;
            }
        }
        public static string ArrayToText(this string[] arr)
        {
            var sb = new StringBuilder();
           foreach(var item in arr)
            {
                sb.Append(item + ", ");
            }
            return sb.ToString();
        }

        public static class Messages
        {

            public const string FormNotValidError = "Lütfen alanları kontrol ederek tekrar deneyiniz";
            public const string UserLockError = "Kullanıcı hesabı kilitli, lütfen sistem yöneticinize danışınız";
            public const string NotValidUserError = "Kullanıcı bilgileri doğru değil lütfen tekrar deneyiniz";
            public const string EPostaConfrim = "Doğrulama linki e-posta adresinize gönderilmiştir";
            public const string EPostaConfrimed = "E-posta adresiniz doğrulanmıştır. Giriş yapabilirsiniz";
            public const string EPostaExits = "Bu E-Posta adresi kullanımdadır";
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
            public static List<SelectListItem> TimeDropDown()
            {
                var list = new List<SelectListItem>();
                string vl = string.Empty;
                for(int i = 0; i <= 23; i++)
                {
                    vl = i.ToString().Length == 1 ? "0" + i : i.ToString();
                    list.Add(new SelectListItem { Text = vl + ":00", Value = vl + ":00" });
                    list.Add(new SelectListItem { Text = vl + ":30", Value = vl + ":30" });
                }
                return list;
            }
        }
    }
}