@ModelType TestModel
@Code
    ViewData("Title") = "Playground"
   ' Html.SetEditableDefaultSettings(False, "0", dataurl:="/SavePartial", formPlacement:="left")
    
    Dim theTime = New MvcHtmlString(" <div class='input-group clockpicker'>" &
                    "<input type='text' class='form-control' value='09:30'>" &
                    "<span class='input-group-addon'>" &
                        "<span class='fa fa-clock-o'></span>" &
                    "</span>" &
                    "</div>")
    
   
    Dim ListRoles = "Equipment.Supervisor, Equipment.Manager, Equipment.DataOperator".Split(",")
    Dim isProperUser As Boolean = ERPBase.ErpAuthorization.UserInAnyRoles(ListRoles, User)
    
End Code


<div class="row">
    <div class="col-lg=12 col-sm-12">
        <div class="form-horizontal">
            @Html.WriteFormInput(Html.TextBox("inputtime", Nothing, New With {.type = "time", .class = "form-control"}), "Time:")
            @Html.WriteFormInput(theTime,"Control Time")
             @Html.WriteFormInput(Html.TextBox("inputdate", Nothing, New With {.type = "date", .class = "form-control"}), "date:")
             @Html.WriteFormInput(Html.TextBox("number", Nothing, New With {.type = "number", .class = "form-control"}), "number:")
             @Html.WriteFormInput(Html.TextBox("color", Nothing, New With {.type = "color", .class = "form-control"}), "color:")
             @Html.WriteFormInput(Html.TextBox("range", Nothing, New With {.type = "range", .min = "40", .max = "100", .class = "form-control"}), "range:")
             @Html.WriteFormInput(Html.TextBox("month", Nothing, New With {.type = "month", .class = "form-control"}), "month:")
             @Html.WriteFormInput(Html.TextBox("week", Nothing, New With {.type = "week", .class = "form-control"}), "week:")
             @Html.WriteFormInput(Html.TextBox("datetime", Nothing, New With {.type = "datetime", .class = "form-control"}), "datetime:")
             @Html.WriteFormInput(Html.TextBox("datetimelocal", Nothing, New With {.type = "datetime-local", .class = "form-control"}), "datetime-local:")
             @Html.WriteFormInput(Html.TextBox("email", Nothing, New With {.type = "email", .class = "form-control"}), "email:")
             @Html.WriteFormInput(Html.TextBox("search", Nothing, New With {.type = "search", .class = "form-control"}), "search:")
             @Html.WriteFormInput(Html.TextBox("tel", Nothing, New With {.type = "tel", .class = "form-control"}), "tel:")
             @Html.WriteFormInput(Html.TextBox("url", Nothing, New With {.type = "url", .class = "form-control"}), "url:")
             
        </div>
    </div>
</div>
<link href="../../plugins/bootstrap-clockpicker/bootstrap-clockpicker.css" rel="stylesheet"
    type="text/css" />
<script src="../../plugins/bootstrap-clockpicker/default.js" type="text/javascript"></script>
<script src="../../plugins/bootstrap-clockpicker/bootstrap-clockpicker.js" type="text/javascript"></script>
<script type="text/javascript">

    $(function () {
        if (!Modernizr.inputtypes.time) {
            $('.clockpicker').clockpicker();
        }


    });
</script>