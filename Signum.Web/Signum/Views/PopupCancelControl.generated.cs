﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Signum.Web.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Signum.Entities;
    using Signum.Utilities;
    using Signum.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.5.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Signum/Views/PopupCancelControl.cshtml")]
    public class PopupCancelControl : System.Web.Mvc.WebViewPage<Context>
    {
        public PopupCancelControl()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n<div id=\"");


            
            #line 3 "..\..\Signum\Views\PopupCancelControl.cshtml"
    Write(Model.Compose("panelPopup"));

            
            #line default
            #line hidden
WriteLiteral("\" class=\"sf-popup-control\" data-prefix=\"");


            
            #line 3 "..\..\Signum\Views\PopupCancelControl.cshtml"
                                                                        Write(Model.ControlID);

            
            #line default
            #line hidden
WriteLiteral("\" data-title=\"");


            
            #line 3 "..\..\Signum\Views\PopupCancelControl.cshtml"
                                                                                                       Write((string)ViewData[ViewDataKeys.Title]);

            
            #line default
            #line hidden
WriteLiteral("\">\r\n");


            
            #line 4 "..\..\Signum\Views\PopupCancelControl.cshtml"
       
        string partialView = (string)ViewData[ViewDataKeys.PartialViewName]; 
        if (partialView.HasText())
        {
            
            
            #line default
            #line hidden
            
            #line 8 "..\..\Signum\Views\PopupCancelControl.cshtml"
       Write(Html.Partial(partialView, Model));

            
            #line default
            #line hidden
            
            #line 8 "..\..\Signum\Views\PopupCancelControl.cshtml"
                                             ;
        }    
        else 
        {
            
            
            #line default
            #line hidden
            
            #line 12 "..\..\Signum\Views\PopupCancelControl.cshtml"
       Write(ViewData[ViewDataKeys.CustomHtml]);

            
            #line default
            #line hidden
            
            #line 12 "..\..\Signum\Views\PopupCancelControl.cshtml"
                                                  
        }
    

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n");


        }
    }
}
#pragma warning restore 1591
