@ModelType MembershipUser
@Code
    ViewData("Title") = "Profile"
    
End Code
<h2>
    Profile</h2>
    @Using Html.BeginJUIBox("User", iconClass:="fa fa-user")
        @<dl class="dl-horizontal">
            <dt>Username</dt>
            <dd>@Model.UserName</dd>
            <dt>Email</dt>
            <dd>@Model.Email</dd>
            <dt>Picture</dt>
            <dd>
                <img class="img-rounded" alt="user image" src="@Url.content("~/account/UserPhoto?userid=" & User.Identity.Name)" style="height:60px;" /> <a href="EditPhoto">Ubah Foto</a>
            </dd>
        </dl>
            
    End Using

@Using Html.BeginJUIBox("Mengubah Password", iconClass:="fa fa-user")
    Using Html.BeginForm("ChangePassword", "Account", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .id = "frmChangePassword"})
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