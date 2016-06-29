@modeltype RegisterModel
    
@Code
    ViewData("Title") = "Menambah User"
End Code

<div class="row">
<div class="col-lg-12 col-sm-12">

    <div class="btn-group pull-right">
    <button class="btn btn-danger" onclick="window.location='/account/Userlist'">Kembali</button>
    </div>

</div>
</div>

    @Using Html.BeginJUIBox("Menambah user")
        Using Html.BeginForm("Register", "Account", FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmRegister"})
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.UserName),"Username")
            @Html.WriteFormInput(Html.PasswordFor(Function(m) m.Password),"Password")
            @Html.WriteFormInput(Html.PasswordFor(Function(m) m.ConfirmPassword),"Konfirmasi password")
@Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Email),"Email")
                                 @Html.WriteFormInput(Html.DropDownList("OfficeId", Nothing, Nothing, New With {.class = "form-control"}), "Unit Kerja")
            
        @<div class="row">
        <div class="col-sm-offset-4 col-sm-2">
            <button type="button" id="btnSave" class="btn btn-primary btn-label-left">
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
                var _data = $("#frmRegister").serialize();
                var _url = $("#frmRegister").attr("action");
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
                window.location = "/Account/UserDetail/" + data.userid;
                return; 
            }
            btnSubmit.button("Submit");

            showNotificationSaveError(data);
        }
    
    </script>