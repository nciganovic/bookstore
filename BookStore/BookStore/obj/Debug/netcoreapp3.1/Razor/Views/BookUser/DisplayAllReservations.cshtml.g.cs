#pragma checksum "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\BookUser\DisplayAllReservations.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e793401d010299f516784147deee97729e3ab71a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_BookUser_DisplayAllReservations), @"mvc.1.0.view", @"/Views/BookUser/DisplayAllReservations.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\_ViewImports.cshtml"
using BookStore.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\_ViewImports.cshtml"
using BookStore.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\_ViewImports.cshtml"
using BookStore.Models.Dto;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\_ViewImports.cshtml"
using BookStore.Models.Tables;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\_ViewImports.cshtml"
using System.Security.Principal;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e793401d010299f516784147deee97729e3ab71a", @"/Views/BookUser/DisplayAllReservations.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3d54e9df42473a8c7a08e0fdeb2f6191e235954d", @"/Views/_ViewImports.cshtml")]
    public class Views_BookUser_DisplayAllReservations : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DisplayAllReservationsViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\BookUser\DisplayAllReservations.cshtml"
   
    ViewBag.Title = "Reservations";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<div class=\"container mt-3\">\r\n    <div class=\"row\">\r\n        <h1>Your reservations</h1>\r\n        <div class=\"col-12\">\r\n            <h2>Current reservations</h2>\r\n            <ul>\r\n");
#nullable restore
#line 13 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\BookUser\DisplayAllReservations.cshtml"
                 foreach (var reservation in @Model.CurrentReservations)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <li> ");
#nullable restore
#line 15 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\BookUser\DisplayAllReservations.cshtml"
                    Write(reservation.BookName);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </li>\r\n");
#nullable restore
#line 16 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\BookUser\DisplayAllReservations.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </ul>\r\n        </div>\r\n\r\n        <div class=\"col-12\">\r\n            <h2>Past reservations</h2>\r\n            <ul>\r\n");
#nullable restore
#line 23 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\BookUser\DisplayAllReservations.cshtml"
                 foreach (var reservation in @Model.PastReservations)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <li> ");
#nullable restore
#line 25 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\BookUser\DisplayAllReservations.cshtml"
                    Write(reservation.BookName);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </li>\r\n");
#nullable restore
#line 26 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\BookUser\DisplayAllReservations.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </ul>\r\n        </div>\r\n    </div>\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<DisplayAllReservationsViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
