@ModelType  ERPBase.LogOnModel
@Code
    Layout = "~/Views/Shared/_LogonLayout.vbhtml"
    ViewData("Title") = "Log On"
End Code
@Using Html.BeginForm("logon", "Account", FormMethod.Post, New With {.autocomplete = "Off", .onsubmit = "return submitForm(this)"})

  
    @<div id="page-login" class="row">
        <div class="col-xs-12 col-md-4 col-md-offset-4 col-sm-6 col-sm-offset-3">
            <div class="box">
                <div class="box-content">
                    <div class="text-center">
                        <h3 class="page-header">
                            MEGA DUTA KONSTRUKSI - LOGON</h3>
                    </div>
                    <div class="form-group">
                        <label class="control-label">
                            Username</label>
                        <input type="text" class="form-control" name="username" />
                    </div>
                    <div class="form-group">
                        <label class="control-label">
                            Password</label>
                        <input type="password" class="form-control" name="password" />
                    </div>
                    <input type="hidden" name="rememberme" value="false" />
                    <div class="text-center">
                        <button class="btn btn-primary" type="submit" id="submitbutton">Log on</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
End Using
<script type="text/javascript">
    var btnSubmit = null;

    submitForm = function (obj) {
        btnSubmit = $("#submitbutton").button("logging in...");
        showSavingNotification("Melakukan login...");
        var _data = $(obj).serialize();
        var _url = $(obj).attr("action");
        $.ajax({
            type: 'POST',
            url: _url,
            data: _data,
            success: submitFormCallback,
            error: ajax_error_callback,
            dataType: 'json'
        });
        return false;
    }
    submitFormCallback = function (data) {
        if (data.stat == 1) {
            window.location = data.url;
            return;
        }
        btnSubmit.button("reset");

        showNotificationSaveError(data, "Login gagal.");
    }
</script>