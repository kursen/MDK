@Code
    Response.StatusCode = 500
    ViewBag.Title = "Server Error 500"
End Code
<h1>
    SERVER ERROR</h1>
<div class="row">
<div class="col-lg-12 col-sm-12">
    <div id='errMsgArea' style="max-height:200px;overflow: scroll">
        <p>@Html.Raw(ViewBag.ErrorMessage)</p>
        <p>@Html.Raw(ViewBag.InnerMesage)</p>
    </div>
</div>

</div>
<div>
    <pre style="max-height: 200px; overflow: scroll">
            <code>
                @Html.Raw(ViewBag.StackTrace)
            </code>
</pre>
</div>
