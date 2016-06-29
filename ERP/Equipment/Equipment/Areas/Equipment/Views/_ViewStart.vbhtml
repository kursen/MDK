@Code
    If Request.IsAjaxRequest Then
        Layout = "~/Views/Shared/_Layout.vbhtml"
        
    Else
        Layout = "~/Views/Shared/_BaseLayout.vbhtml"
    End If

End Code

<script type="text/javascript">
    var fnReset = function (objform) {
        $('[name="ID"], [name="Id"], [name="id"]').val(0);
        $(objform).trigger("reset");
        $('.form-group,.col-md-5', $(objform)).removeClass("has-error");
    }
</script>