#pragma checksum "C:\Users\Deltaforce1405\Desktop\api\AppCursos\AppCursos\Views\Cursoes\Create.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c7ba977a63acbeeb0dca99b9d4d9b6e1046d9801"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Cursoes_Create), @"mvc.1.0.view", @"/Views/Cursoes/Create.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c7ba977a63acbeeb0dca99b9d4d9b6e1046d9801", @"/Views/Cursoes/Create.cshtml")]
    public class Views_Cursoes_Create : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<AppCursos.Models.Curso>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Deltaforce1405\Desktop\api\AppCursos\AppCursos\Views\Cursoes\Create.cshtml"
  
    ViewData["Title"] = "Create";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<h1>Create</h1>

<h4>Curso</h4>
<hr />
<div class=""row"">
    <div class=""col-md-4"">
        <form asp-action=""Create"">
            <div asp-validation-summary=""ModelOnly"" class=""text-danger""></div>
            <div class=""form-group"">
                <label asp-for=""Codigo"" class=""control-label""></label>
                <input asp-for=""Codigo"" class=""form-control"" />
                <span asp-validation-for=""Codigo"" class=""text-danger""></span>
            </div>
            <div class=""form-group"">
                <label asp-for=""Descripcion"" class=""control-label""></label>
                <input asp-for=""Descripcion"" class=""form-control"" />
                <span asp-validation-for=""Descripcion"" class=""text-danger""></span>
            </div>
            <div class=""form-group"">
                <label asp-for=""Estado"" class=""control-label""></label>
                <input asp-for=""Estado"" class=""form-control"" />
                <span asp-validation-for=""Estado"" class=""text-danger""></span>");
            WriteLiteral(@"
            </div>
            <div class=""form-group"">
                <input type=""submit"" value=""Create"" class=""btn btn-primary"" />
            </div>
        </form>
    </div>
</div>

<div>
    <a asp-action=""Index"">Back to List</a>
</div>

");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 42 "C:\Users\Deltaforce1405\Desktop\api\AppCursos\AppCursos\Views\Cursoes\Create.cshtml"
      await Html.RenderPartialAsync("_ValidationScriptsPartial");

#line default
#line hidden
#nullable disable
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<AppCursos.Models.Curso> Html { get; private set; }
    }
}
#pragma warning restore 1591
