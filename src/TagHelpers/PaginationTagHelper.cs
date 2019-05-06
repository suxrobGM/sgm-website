using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SuxrobGM_Resume.TagHelpers
{
    [HtmlTargetElement("pagination")]
    public class PaginationTagHelper : TagHelper
    {
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public string BaseUrl { get; set; }
        public string PageMethod { get; set; }
        public string PageFragment { get; set; }

        public PaginationTagHelper()
        {
            PageMethod = "pageIndex";
            PageFragment = "";
            BaseUrl = "./Index";
            TotalPages = 10;
            PageIndex = 1;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var prevDisabled = PageIndex > 1 ? "" : "disabled";
            var nextDisabled = PageIndex < TotalPages ? "" : "disabled";

            output.TagName = "pagination";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent("<ul class='pagination pagination-sm text-body mb-0'>");
            output.Content.AppendHtml($"<li class='page-item {prevDisabled}'><a class='page-link' href='{BaseUrl}?{PageMethod}={PageIndex - 1}#{PageFragment}'>Previous</a></li>");

            if (TotalPages <= 10)
            {
                for (int i = 1; i <= TotalPages; i++)
                {
                    string activeClassName = i == PageIndex ? "active" : "";
                    output.Content.AppendHtml($"<li class='page-item {activeClassName}'><a class='page-link' href='{BaseUrl}?{PageMethod}={i}#{PageFragment}'>{i}</a></li>");
                }
            }
            else
            {
                var activeClassName = PageIndex == 1 ? "active" : "";

                if ((PageIndex - 4) > 1)
                {
                    output.Content.AppendHtml($"<li class='page-item {activeClassName}'><a class='page-link' href='{BaseUrl}?{PageMethod}=1#{PageFragment}'>1</a></li>");
                    output.Content.AppendHtml("<li class='page-item disabled'><a class='page-link'>...</a></li>");
                }               

                for (int i = PageIndex - 4; i <= PageIndex + 4; i++)
                {
                    if (i > TotalPages)
                        break;

                    if (i > 0)
                    {
                        activeClassName = i == PageIndex ? "active" : "";
                        output.Content.AppendHtml($"<li class='page-item {activeClassName}'><a class='page-link' href='{BaseUrl}?{PageMethod}={i}#{PageFragment}'>{i}</a></li>");
                    }           
                }

                if ((TotalPages - PageIndex) > 4)
                {
                    activeClassName = PageIndex == TotalPages ? "active" : "";
                    output.Content.AppendHtml("<li class='page-item disabled'><a class='page-link'>...</a></li>");
                    output.Content.AppendHtml($"<li class='page-item {activeClassName}'><a class='page-link' href='{BaseUrl}?{PageMethod}={TotalPages}#{PageFragment}'>{TotalPages}</a></li>");
                }
            }
            
            output.Content.AppendHtml($"<li class='page-item {nextDisabled}'><a class='page-link' href='{BaseUrl}?{PageMethod}={PageIndex + 1}#{PageFragment}'>Next</a></li>");
            output.Content.AppendHtml("</ul>");
        }
    }
}
