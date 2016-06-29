@Code
    ViewData("Title") = "Post data"
    Dim requestForm = Request.UrlReferrer.AbsoluteUri
End Code
<h2>
    Post data from <a href="@Html.Raw(requestForm)">@Html.Raw(requestForm)</a></h2>
@Code
    
   
    Dim result = Request.Form.AllKeys.ToDictionary(Function(k) k, Function(k) Request.Form(k))
    
    Dim srz = New System.Web.Script.Serialization.JavaScriptSerializer
    Dim resulttext = srz.Serialize(result)
    
    @<div class="well">
    @Html.Raw(resulttext)
    </div>
    
    
    
    @<div class="well">
    <dl class="dl-horizontal">
        @For Each item In Request.Form
        
            @<dt> @Html.Raw(item) &nbsp;</dt>
            @<dd>@Html.Raw(Request.Form(item)) &nbsp;</dd>
        
        
        
        
        
        Next
    </dl>
    </div>
End Code
