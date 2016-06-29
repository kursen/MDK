@ModelType MembershipUser
@Code
    ViewData("Title") = "Profile"
    Dim profile = ERPBase.ErpUserProfile.GetUserProfile()
End Code

@Using Html.BeginJUIBox("User", iconClass:="fa fa-user")
        
    @<div class="row">
        <div class="col-lg-2 col-sm-4">
            <div class="row text-center">
                <img class="img-rounded" alt="user image" src="@Url.Content("~/account/UserPhoto?userid=" & User.Identity.Name)" style="height:120px;" />
            </div>
            <div class="row text-center" style="margin-top:20px">
                <a href="/Account/EditPhoto" class='btn btn-info'>Ubah Foto</a>
            </div>
        </div>
        <div class="col-lg-10">
            <div class="form-horizontal">
                @Html.WriteFormInput(New MvcHtmlString("<span class='form-control'>" & Model.UserName & "</span>"), "Username")
                @Html.WriteFormInput(Html.EditableInputTextBox("Fullname", profile.Fullname, "text", "Nama Lengkap ", dataurl:="/Account/UpdateUserProfile", datapk:=Model.UserName), "Nama Lengkap")
                @Html.WriteFormInput(New MvcHtmlString("<span class='form-control'>" & profile.WorkUnitName & "</span>"), "Kantor/Unit Kerja")
                @Html.WriteFormInput(Html.EditableInputTextBox("Email", Model.Email, "text", "Email ", dataurl:="/Account/UpdateUserProfile", datapk:=Model.UserName), "Email")
            </div>
        </div>
    </div>
        
        
  
            
End Using
@Using Html.BeginJUIBox("Mengubah Password", iconClass:="fa fa-user")
    Using Html.BeginForm("ChangePassword", "Account", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "off", .id = "frmChangePassword"})
    @Html.WriteFormInput(New MvcHtmlString("<input type='password' name='OldPassword' class='form-control'>"), "Password Lama")
    @Html.WriteFormInput(New MvcHtmlString("<input type='password' name='NewPassword' class='form-control'>"), "Password Baru")
    @Html.WriteFormInput(New MvcHtmlString("<input type='password' name='ConfirmPassword' class='form-control'>"), "Konfirmasi Password Baru")
    
    @<div class="row">
        <div class="col-sm-offset-4 col-sm-2">
            <button type="button" class="btn btn-primary btn-label-left" id="btnSave">
                <span><i class="fa fa-clock-o"></i></span>Submit
            </button>
        </div>
    </div>
    
    End Using
End Using
<script type="text/javascript">

    _savepartialresponse = function (data) {

    }
    var btnSubmit = null;
    $(function () {

        $("#btnSave").click(function () {
            btnSubmit = $(this).button("menyimpan data...");
            showSavingNotification();
            var _data = $("#frmChangePassword").serialize();
            var _url = $("#frmChangePassword").attr("action");
            $.ajax({
                type: 'POST',
                url: _url,
                data: _data,
                success: submitFormCallback,
                error: ajax_error_callback,
                dataType: 'json'
            });


        });


    });

    submitFormCallback = function (data) {
        if (data.stat == 1) {
            showNotification("Password telah berubah.")
            $("#frmChangePassword").trigger("reset");
            return;
        }
        btnSubmit.button("Submit");

        showNotificationSaveError(data);
    }
</script>
