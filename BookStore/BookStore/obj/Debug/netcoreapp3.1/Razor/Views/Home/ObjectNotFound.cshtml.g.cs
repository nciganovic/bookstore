#pragma checksum "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\Home\ObjectNotFound.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "693ae3eca020480a82ffb9b4b040f1b4c95094ae"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_ObjectNotFound), @"mvc.1.0.view", @"/Views/Home/ObjectNotFound.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"693ae3eca020480a82ffb9b4b040f1b4c95094ae", @"/Views/Home/ObjectNotFound.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"33dcf04a7c93db80d64d3ad2400a708777a538af", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_ObjectNotFound : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<int>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\Home\ObjectNotFound.cshtml"
   
    ViewBag.Title = "404 Error";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"col-12 alert alert-danger m-3\">\r\n    <p>");
#nullable restore
#line 7 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\Home\ObjectNotFound.cshtml"
  Write(ViewBag.Object);

#line default
#line hidden
#nullable disable
            WriteLiteral(" with id = ");
#nullable restore
#line 7 "C:\Users\nikol\source\repos\BookStore\bookstore\BookStore\BookStore\Views\Home\ObjectNotFound.cshtml"
                            Write(Model);

#line default
#line hidden
#nullable disable
            WriteLiteral(" not found</p>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<int> Html { get; private set; }
    }
}
#pragma warning restore 1591
