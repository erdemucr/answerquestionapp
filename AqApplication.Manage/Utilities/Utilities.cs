using AqApplication.Entity.Constants;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AqApplication.Manage.Models
{
    public static class Utilities
    {
        public static List<SelectListItem> MemberTypeSelectList()
        {
            Array values = Enum.GetValues(typeof(MemberType));
            List<SelectListItem> items = new List<SelectListItem>(values.Length);

            foreach (var i in values)
            {
                items.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(MemberType), i),
                    Value = ((int)i).ToString()
                });
            }

            return items;
        }
        public static IHtmlContent UseChinaRegisterInfo(this RazorPage page)
        {
            var requestCultureFeature = page.Context.Features.Get<IRequestCultureFeature>();
            var requestCulture = requestCultureFeature.RequestCulture.UICulture.IetfLanguageTag;

            var content = new HtmlContentBuilder();
            if (requestCulture == "tr-TR")
            {
                content.SetHtmlContent("<img style=\"height: 47px; border-radius:74 %;\" src=\"/assets/images/turkbayrak.png\" alt=\"\" class=\"m--img-rounded m--marginless m--img-centered\" />");
            }
            else if (requestCulture == "en-US")
            {
                content.SetHtmlContent("<img style=\"height: 47px; border-radius:74 %;\" src=\"/assets/images/ABD_Bayrağı.png\" alt=\"\" class=\"m--img-rounded m--marginless m--img-centered\" />");
            }
            return content;
        }
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
        public static string ChallengeTypeEnumText(this ChallengeTypeEnum type)
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
            foreach (var item in arr)
            {
                sb.Append(item + ", ");
            }
            return sb.ToString();
        }
        public static string TrimTelnoMask(this string telno)
        {
            if (!string.IsNullOrEmpty(telno))
            {
                telno = telno.Contains(' ') ? telno.Replace(" ", "") : telno;
                telno = telno.Contains('(') ? telno.Replace("(", "") : telno;
                telno = telno.Contains(')') ? telno.Replace(")", "") : telno;
            }
            return telno;
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
                for (int i = 0; i <= 23; i++)
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