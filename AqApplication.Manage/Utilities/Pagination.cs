using AqApplication.Core.Type;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Text;

namespace AqApplication.Manage.Utilities
{
    public static class PaginationHelper
    {
        public static string Paginate(ViewContext context, Paginition pagination)
        {
            string controllername = context.RouteData.Values["controller"].ToString();
            string actionname = context.RouteData.Values["action"].ToString();
            if (pagination == null)
                return string.Empty;
            var sb = new StringBuilder();
            var urlHelper = new UrlHelper(context);
            sb.Append("<ul class=\"pagination\">");
            int pagecount = (int)Math.Ceiling(pagination.TotalCount / (double)pagination.PageSize);
            if (pagecount > 1)
            {
                if (pagination.CurrentPage > 0)
                {
                    int previouslimit = 0;

                    if (pagination.CurrentPage > 1)
                    {
                        sb.Append("<li class=\"page-item\"><a class=\"page-link\" href=\"" +
                              urlHelper.Action(actionname, controllername, GetRouteValues(1, pagination)) + "\"><<</a></li>");
                        sb.Append("<li class=\"page-item\"><a class=\"page-link\" href=\"" +
                                  urlHelper.Action(actionname, controllername, GetRouteValues((pagination.CurrentPage - 1), pagination)) +
                                  "\"><</a></li>");
                        previouslimit = (pagination.CurrentPage <= 4) ? pagination.CurrentPage : 3;
                    }

                    for (int i = previouslimit - 1; i > 0; i--)
                    {
                        sb.Append("<li class=\"page-item\"><a class=\"page-link\" href=\"" + urlHelper.Action(actionname, controllername, GetRouteValues((pagination.CurrentPage - i), pagination)) + "\">" + (pagination.CurrentPage - i).ToString() + "</a></li>");
                    }

                    sb.Append("<li class=\"active page-item\"><a class=\"page-link\" href=\"" + urlHelper.Action(actionname, controllername, GetRouteValues((pagination.CurrentPage), pagination)) + "\">" + pagination.CurrentPage + "</a></li>");
                    int upperlimit = pagecount - pagination.CurrentPage;
                    upperlimit = (upperlimit >= 4) ? 3 : upperlimit;
                    for (int i = 1; i < upperlimit + 1; i++)
                    {
                        sb.Append("<li class=\"page-item\"><a class=\"page-link\" href=\"" + urlHelper.Action(actionname, controllername, GetRouteValues(((pagination.CurrentPage + i)), pagination)) + "\">" + (pagination.CurrentPage + i) + " </a></li>");
                    }
                    if (pagecount != pagination.CurrentPage)
                    {
                        sb.Append("<li class=\"page-item\"><a class=\"page-link\" href=\"" +
                                  urlHelper.Action(actionname, controllername, GetRouteValues((pagination.CurrentPage + 1), pagination)) +
                                  "\">></a></li>");
                        sb.Append("<li class=\"page-item\"><a class=\"page-link\" href=\"" +
                                  urlHelper.Action(actionname, controllername, GetRouteValues((pagecount), pagination)) +
                                  "\">>></a></li>");
                    }
                }
                else
                {
                    int upperlimit = pagecount - pagination.CurrentPage;
                    upperlimit = (upperlimit > 3) ? 3 : upperlimit;
                    for (int i = 1; i < upperlimit + 1; i++)
                    {
                        sb.Append("<li class=\"page-item\"><a class=\"page-link\" href=\"" + urlHelper.Action(actionname, controllername, new { CurrentPage = (pagination.CurrentPage + i) }) + "\">" + (pagination.CurrentPage + i) + "</a></li>");
                    }
                    if (pagecount != pagination.CurrentPage)
                    {
                        sb.Append("<li class=\"page-item\"><a class=\"page-link\" href=\"" +
                                  urlHelper.Action(actionname, controllername, new { CurrentPage = (pagination.CurrentPage + 1) }) +
                                  "\">></a></li>");
                        sb.Append("<li class=\"page-item\"><a class=\"page-link\" href=\"" + urlHelper.Action(actionname, controllername, new { CurrentPage = pagecount }) +
                                  "\">>></a></li>");
                    }
                }

            }
            else
            {
                sb.Append("");
            }

            sb.Append("</ul> ");
            return sb.ToString();
        }

        private static RouteValueDictionary GetRouteValues(int currentPage, Paginition pagination)
        {
            var routeParms = new RouteValueDictionary();
        
            routeParms.Add("CurrentPage", currentPage);
            if (!string.IsNullOrEmpty(pagination.Name))
                routeParms.Add("Name", pagination.Name);
            if (pagination.StartDate.ToDate().HasValue)
                routeParms.Add("StartDate", pagination.StartDate.ToString());
            if (pagination.EndDate.ToDate().HasValue)
                routeParms.Add("EndDate", pagination.EndDate.ToString());

            return routeParms;
        }
    }
}
