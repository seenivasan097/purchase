using System.Collections.Generic;
using System.Linq.Expressions;
using  System.Web.Mvc;
using System;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;

namespace InvoiceApp.Helpers.Attributes
{
    ///<summary> 
    ///  write a simple extension method for the built in html helper class. 
    /// this will enable us to use the same Html object to use our custom helper method.
    /// </summary> 
    public static class HtmlHelperExtensions
    {

        private const string RequiredHtml = "<span class=\"required\"> *</span>";
        /// <summary>
        /// used to Create Customer MVC Extension Methods 
        /// it will get helper message in label without html attribute
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="helpMessage"></param>
        /// <returns></returns>
        public static MvcHtmlString ForLabelWithHelp<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression,  string helpMessage)
        {

           return ForLabelWithHelp(html, expression, null, helpMessage);
        }
       /// <summary>
        /// used to Create Customer MVC Extension Methods 
        /// it will get helper message in label with html attribute
       /// </summary>
       /// <typeparam name="TModel"></typeparam>
       /// <typeparam name="TValue"></typeparam>
       /// <param name="html"></param>
       /// <param name="expression"></param>
       /// <param name="htmlAttributes"></param>
       /// <param name="helpMessage"></param>
       /// <returns></returns>
        public static MvcHtmlString ForLabelWithHelp<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes, string helpMessage)
        {
            var id = html.IdFor(expression).ToString();
            var label = FieldLabel(htmlAttributes, id);
            label.InnerHtml = html.DisplayNameFor(expression) + HelpButton(helpMessage, id).ToString();
            return new MvcHtmlString(label.ToString());
        }
        /// <summary>
        /// used to Create Customer MVC Extension Methods 
        /// it will get Requirement helper message in label with html attribute and without any comment
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="helpMessage"></param>
        /// <returns></returns>
 
        public static MvcHtmlString ForRequiredLabelWithHelp<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes, string helpMessage)
        {
            var id = html.IdFor(expression).ToString();
            var label = FieldLabel(htmlAttributes, id);
            label.InnerHtml = html.DisplayNameFor(expression) + RequiredHtml + HelpButton(helpMessage, id);

            return new MvcHtmlString(label.ToString());
        }
        /// <summary>
        /// used to Create Customer MVC Extension Methods 
        /// it will get Requirement helper message in label with html attribute and with any comment
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="helpMessage"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        public static MvcHtmlString ForRequiredLabelWithHelp<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes, string helpMessage,string comments)
        {
            var id = html.IdFor(expression).ToString();
            var label = FieldLabel(htmlAttributes, id);
            label.InnerHtml = html.DisplayNameFor(expression) + RequiredHtml + comments + HelpButton(helpMessage, id);
            return new MvcHtmlString(label.ToString());
        }

        /// <summary>
        /// used to Create Customer MVC Extension Methods 
        /// it will build new tags for feilds with html attribute based on thier ID
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static TagBuilder FieldLabel(object htmlAttributes, string id)
        {
            var label = new TagBuilder("label");
            label.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            label.Attributes.Add(new KeyValuePair<string, string>("for", id));
            label.Attributes.Add(new KeyValuePair<string, string>("id", string.Format("id_{0}", id)));
            return label;

        }
        /// <summary>
        /// used to Create Customer MVC Extension Methods 
        /// it will build new tags for button with html attribute based on thier ID
        /// </summary>
        /// <param name="helpMessage"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static TagBuilder HelpButton(string helpMessage,string id)
        {
            var button = new TagBuilder("button");
            button.AddCssClass("glyphicon glyphicon-question-sign not-button");
            button.Attributes.Add(new KeyValuePair<string, string>("type", "button"));
            button.Attributes.Add(new KeyValuePair<string, string>("data-container", "body"));
            button.Attributes.Add(new KeyValuePair<string, string>("data-trigger", "focus"));
            button.Attributes.Add(new KeyValuePair<string, string>("data-placement", "bottom"));
            button.Attributes.Add(new KeyValuePair<string, string>("data-content", helpMessage));
            button.Attributes.Add(new KeyValuePair<string, string>("data-original-title", ""));
            button.Attributes.Add(new KeyValuePair<string, string>("title", ""));
            button.Attributes.Add(new KeyValuePair<string, string>("data-toggle", "popover"));
            button.Attributes.Add(new KeyValuePair<string, string>("aria-labelledby",string.Format("id_{0}", id)));

            return button;
        }

        #region Script Builder

        private const string SCRIPTBLOCK_BUILDER = "ScriptBlockBuilder";

        /// <summary>
        /// Helper to write all partial view script block at the bottom of page when rendering
        /// This helper will be used inside the partial view. 
        /// Usage: this.ScriptBlock(@<text><script type="text/javascript" href="../script.js"/></text>)
        /// </summary>
        /// <param name="webPage"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static MvcHtmlString ScriptBlock(this WebViewPage webPage,
          Func<dynamic, HelperResult> template)
        {
            if (!webPage.IsAjax)
            {
                var scriptBuilder = webPage.Context.Items[SCRIPTBLOCK_BUILDER]
                     as StringBuilder ?? new StringBuilder();

                scriptBuilder.Append(template(null).ToHtmlString());

                webPage.Context.Items[SCRIPTBLOCK_BUILDER] = scriptBuilder;

                return new MvcHtmlString(string.Empty);
            }
            var test = new MvcHtmlString(template(null).ToHtmlString().ToString());
            return test;
        }


        /// <summary>
        /// Helper to write all partial view script block at the bottom of page when rendering
        /// This helper is used in layout where we want the blocks to be written
        /// Usage: this.WriteScriptBlocks() in layout
        /// </summary>
        /// <param name="webPage"></param>
        /// <returns></returns>
        public static MvcHtmlString WriteScriptBlocks(this WebViewPage webPage)
        {
            var scriptBuilder = webPage.Context.Items[SCRIPTBLOCK_BUILDER]
                   as StringBuilder ?? new StringBuilder();

            return new MvcHtmlString(scriptBuilder.ToString());
        }



        #endregion
    }
}