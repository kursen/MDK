@Code
    ViewBag.Title = "ServerError"
End Code


<h1>SERVER ERROR</h1>

    <p>@Html.Raw(ViewBag.ErrorMessage)</p>
    <p>@Html.Raw(ViewBag.InnerMesage)</p>
    <div >

        <code>
            <pre>
    @Html.Raw(ViewBag.StackTrace)
</pre>
        </code>
    </div>



