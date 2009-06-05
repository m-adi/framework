﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Signum.Entities;
using Signum.Entities.DynamicQuery;
using Signum.Utilities;
using Signum.Entities.Reflection;
using System.Linq.Expressions;
using System.Reflection;
using Signum.Utilities.Reflection;


namespace Signum.Web
{
    public static class SearchControlHelper
    {
        public static string NewFilter(Controller controller, string filterTypeName, string columnName, string displayName, int index, string entityTypeName, string prefix)
        {
            Type searchEntityType = Navigator.NameToType[entityTypeName];

            StringBuilder sb = new StringBuilder();
            Type columnType = GetType(filterTypeName);
            //Client doesn't know about Lazys, check it ourselves
            if (typeof(IdentifiableEntity).IsAssignableFrom(columnType))
                columnType = Reflector.GenerateLazy(columnType);
            FilterType filterType = FilterOperationsUtils.GetFilterType(columnType);
            List<FilterOperation> possibleOperations = FilterOperationsUtils.FilterOperations[filterType];

            sb.Append("<tr id=\"{0}\" name=\"{0}\">\n".Formato(prefix + "trFilter_" + index.ToString()));
            
            sb.Append("<td id=\"{0}\" name=\"{0}\">\n".Formato(prefix + "td" + index.ToString() + "__" + columnName));
            sb.Append(displayName);
            sb.Append("</td>\n");
            
            sb.Append("<td>\n");
            sb.Append("<select id=\"{0}\" name=\"{0}\">\n".Formato(prefix + "ddlSelector_" + index.ToString()));
            for (int j=0; j<possibleOperations.Count; j++)
                sb.Append("<option value=\"{0}\">{1}</option>\n"
                    .Formato(possibleOperations[j], possibleOperations[j].NiceToString()));
            sb.Append("</select>\n");
            sb.Append("</td>\n");
            
            sb.Append("<td>\n");
            sb.Append(PrintValueField(CreateHtmlHelper(controller), filterType, columnType, prefix + "value_" + index.ToString(), null, searchEntityType, columnName));
            sb.Append("</td>\n");
            
            sb.Append("<td>\n");
            sb.Append("<input type=\"button\" id=\"{0}\" name=\"{0}\" value=\"X\" onclick=\"DeleteFilter('{1}','{2}');\" />\n".Formato(prefix + "btnDelete_" + index, index, prefix));
            sb.Append("</td>\n");
            
            sb.Append("</tr>\n");
            return sb.ToString();
        }

        private static Type GetType(string typeName)
        {
            if (Navigator.NameToType.ContainsKey(typeName))
                return Navigator.NameToType[typeName];

            return Type.GetType("System." + typeName, true);
        }

        private static HtmlHelper CreateHtmlHelper(Controller c)
        {
            return new HtmlHelper(
                        new ViewContext(
                            c.ControllerContext,
                            new WebFormView(c.ControllerContext.RequestContext.HttpContext.Request.FilePath),
                            c.ViewData,
                            c.TempData),
                        new ViewPage()); 
        }

        public static void NewFilter(this HtmlHelper helper, FilterOptions filterOptions, int index, string entityTypeName)
        {
            StringBuilder sb = new StringBuilder();
            
            Type searchEntityType = Navigator.NameToType[entityTypeName];

            FilterType filterType = FilterOperationsUtils.GetFilterType(filterOptions.Column.Type);
            List<FilterOperation> possibleOperations = FilterOperationsUtils.FilterOperations[filterType];
                
            sb.Append("<tr id=\"{0}\" name=\"{0}\">\n".Formato(helper.GlobalName("trFilter_" + index.ToString())));
            sb.Append("<td id=\"{0}\" name=\"{0}\">\n".Formato(helper.GlobalName("td" + index.ToString() + "__" + filterOptions.Column.Name)));
            sb.Append(filterOptions.Column.DisplayName);
            sb.Append("</td>\n");
            
            sb.Append("<td>\n");
            sb.Append("<select{0} id=\"{1}\" name=\"{1}\">\n"
                .Formato(filterOptions.Frozen ? " disabled=\"disabled\"" : "",
                        helper.GlobalName("ddlSelector_" + index.ToString())));
            for (int j=0; j<possibleOperations.Count; j++)
                sb.Append("<option value=\"{0}\" {1}>{2}</option>\n"
                    .Formato(
                        possibleOperations[j],
                        (possibleOperations[j] == filterOptions.Operation) ? "selected=\"selected\"" : "",
                        possibleOperations[j].NiceToString()));
            sb.Append("</select>\n");
            
            sb.Append("</td>\n");
            
            sb.Append("<td>\n");
            string txtId = helper.GlobalName("value_" + index.ToString());
            string txtValue = (filterOptions.Value != null) ? filterOptions.Value.ToString() : "";
            if (filterOptions.Frozen)
                sb.Append("<input type=\"text\" id=\"{0}\" name=\"{0}\" value=\"{1}\" {2}/>\n".Formato(txtId, txtValue, filterOptions.Frozen ? " readonly=\"readonly\"" : ""));
            else
            {
                sb.Append(PrintValueField(helper, filterType, filterOptions.Column.Type, txtId, filterOptions.Value, searchEntityType, filterOptions.Column.Name));
            }
            sb.Append("</td>\n");
            
            sb.Append("<td>\n");
            if (!filterOptions.Frozen)
                sb.Append(helper.Button(helper.GlobalName("btnDelete_" + index), "X", "DeleteFilter('{0}','{1}');".Formato(index, helper.ViewData[ViewDataKeys.PopupPrefix] ?? ""), "", new Dictionary<string, string>()));
            sb.Append("</td>\n");
            
            sb.Append("</tr>\n");
            helper.ViewContext.HttpContext.Response.Write(sb.ToString()); 
        }

        private static string PrintValueField(HtmlHelper helper, FilterType filterType, Type columnType, string id, object value, Type searchEntityType, string propertyName)
        {
            StringBuilder sb = new StringBuilder();
            if (filterType == FilterType.Lazy)
            {
                EntityLine el = new EntityLine();
                Navigator.ConfigureEntityBase(el, Reflector.ExtractLazy(columnType) ?? columnType, false);
                el.Create = false;

                // Convert.ChangeType(value, columnType)
                string result = (string)EntityLineHelper.InternalEntityLine(helper, id, columnType, value, el);
                sb.Append(result);
            }
            else
            {
                ValueLineType vlType = ValueLineHelper.Configurator.GetDefaultValueLineType(columnType);
                sb.Append(
                    ValueLineHelper.Configurator.constructor[vlType](
                        helper,
                        new ValueLineData(
                            id,
                            value,
                            new Dictionary<string, object>())));
            }
            return sb.ToString();
        }
    }
}
