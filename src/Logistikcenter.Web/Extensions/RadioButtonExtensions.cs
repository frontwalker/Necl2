using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace Logistikcenter.Web.Extensions
{
    public static class RadioButtonExtensions
    {
        private static string Input(string id, string name, string value, string label)
        {
            var button = string.Format(@"<input type=""radio"" id=""{0}"" name=""{1}"" value=""{2}"" /><label for=""{0}"">{3}</label>", id, name, value, label);
            return button;
        }

        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper, string propertyName, IEnumerable<SelectListItem> choises)
        {
            var radiobuttons = new StringBuilder();

            radiobuttons.Append(@"<div class=""radio"">");

            int i = 0;
            foreach (var choise in choises)
            {
                radiobuttons.Append(Input(propertyName + i, propertyName, choise.Value, choise.Text));
                i++;
            }

            radiobuttons.Append(@"</div>");

            return new MvcHtmlString(radiobuttons.ToString());
        }
        
        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper, string propertyName, params string[] choises)
        {          
            var selectListItems = choises.Select((t, i) => new SelectListItem {Text = t, Value = i.ToString()}).ToList();
            return RadioButtonList(htmlHelper, propertyName, selectListItems);
        }

        public static MvcHtmlString RadioButtonListFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, int>> expression, params string[] choises)
        {
            var propertyName = ((MemberExpression)expression.Body).Member.Name;
            return RadioButtonList(htmlHelper, propertyName, choises);
        }

        public static MvcHtmlString RadioButtonListFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, int>> expression, IEnumerable<SelectListItem> choises)
        {
            var propertyName = ((MemberExpression) expression.Body).Member.Name;
            return RadioButtonList(htmlHelper, propertyName, choises);
        }
    }
}