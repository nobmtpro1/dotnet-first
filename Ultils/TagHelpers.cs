
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Blog.Ultils
{
    public static class TagHelpers
    {
        public static string IsActive(this IHtmlHelper html, string control, string action)
        {
            var routeData = html.ViewContext.RouteData;
            var routeAction = (string)routeData.Values["action"]!;
            var routeControl = (string)routeData.Values["controller"]!;
            var returnActive = control == routeControl &&
                               action == routeAction;
            return returnActive ? "active" : "";
        }

        public static string IsActiveMultiple(this IHtmlHelper html, List<dynamic> routes)
        {
            var result = "";
            var routeData = html.ViewContext.RouteData;
            var routeAction = (string)routeData.Values["action"]!;
            var routeControl = (string)routeData.Values["controller"]!;
            foreach (dynamic item in routes)
            {
                if (item.controller == routeControl && item.action == routeAction)
                {
                    result = "active";
                }
            }

            return result;
        }
    }
}