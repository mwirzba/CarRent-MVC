using System.Collections.Generic;
using CarRent.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CarRent.Infrastructure
{
    [HtmlTargetElement("div",Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PagingInfo PageModel { get; set; }
        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string,object> PageUrlValues { get; set; }
        = new Dictionary<string, object>();

        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

            TagBuilder result = new TagBuilder("div");

            for (int i = 1; i <= PageModel.TotalPages; i++)
            {

                if (i == 1)
                {
                    TagBuilder prevPage = new TagBuilder("a");
                    prevPage.AddCssClass(PageClass);
                    prevPage.InnerHtml.Append("Previous");
                    PageUrlValues["page"] = PageModel.CurrentPage-1;
                    if (PageModel.CurrentPage < 2)
                        prevPage.AddCssClass(PageClassNormal + "disabled");
                    else
                    {
                        prevPage.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                        prevPage.AddCssClass(PageClassNormal);
                    }
                    result.InnerHtml.AppendHtml(prevPage);

                }

                TagBuilder tag = new TagBuilder("a");
                PageUrlValues["page"] = i;
                tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                if (PageClassesEnabled)
                {
                    tag.AddCssClass(PageClass);
                    tag.AddCssClass(i == PageModel.CurrentPage
                        ? PageClassSelected : PageClassNormal);

                }

                tag.InnerHtml.Append((i.ToString()));
                result.InnerHtml.AppendHtml(tag);

            }

            TagBuilder nextPage = new TagBuilder("a");
            nextPage.AddCssClass(PageClass);
            nextPage.InnerHtml.Append("Next");
            PageUrlValues["page"] = PageModel.CurrentPage +1;
            if (PageModel.CurrentPage == PageModel.TotalPages)
                nextPage.AddCssClass(PageClassNormal + "disabled");
            else
            {
                nextPage.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                nextPage.AddCssClass(PageClassNormal);
            }
            result.InnerHtml.AppendHtml(nextPage);
            output.Content.AppendHtml(result.InnerHtml);

        }

    }
}
