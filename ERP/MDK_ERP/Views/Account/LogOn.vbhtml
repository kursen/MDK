@ModelType MDK_ERP.LogOnModel

@Code
    ViewData("Title") = "Log On"
    Layout = "~/Views/_ViewBlank.vbhtml"
End Code

<style type="text/css">
    .validation-summary-errors{
        top: 0px ! important; position: fixed; width: 100% ! important; z-index: 99;
        background-color: rgba(242, 222, 222, 0.84) !important;
    }
</style>

@Html.ValidationSummary(True, "")
<div class="middle-wrapper">
    <div class="login-container">
        <div class="text-left box-title">
            <h4>Login <span class="pull-right"><i class="icon-lock text-muted"></i></span></h4>
        </div>
        <div class="row box-content">
            <div class="" style="margin: auto; float: none;">
                <div class="row">
                    <div class=" col-xs-8 col-xs-offset-2 col-md-8 col-md-offset-2">
                        @*<div class="text-center"><img src="@Url.Content("~/Content/images/shared/logo.png")" alt="HaloteC Indonesia" /></div>
                        <p></p><p></p>*@
                        @Using Html.BeginForm("LogOn", "Account", "", FormMethod.Post, New With {.class = "validate-form", .autocomplete = "off"})
                        @<div>
                            <div class="form-group">
                                <div class="controls with-icon-over-input">
                                    @Html.TextBoxFor(Function(m) m.UserName, New With {.class = "form-control", .placeholder = "Name"})
                                    <i class="icon-user text-muted"></i>
                                    @Html.ValidationMessageFor(Function(m) m.UserName)
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="controls with-icon-over-input">
                                    @Html.PasswordFor(Function(m) m.Password, New With {.class = "form-control", .placeholder = "Password"})
                                    <i class="icon-lock text-muted"></i>
                                    @Html.ValidationMessageFor(Function(m) m.Password)
                                </div>
                            </div>
                            <div class="checkbox pull-left">
                                <label for="remember_me">
                                    <input id="remember_me" name="remember_me" value="1" type="checkbox" />
                                    @Html.CheckBoxFor(Function(m) m.RememberMe)
                                    Remember me
                                </label>
                            </div>
                            <div class="col-xs-offset-7">
                                <button type="submit" class="btn btn-block">Sign in</button>
                            </div>
                        </div>
                        End Using
                        <div class="text-center">
                        <hr class="hr-normal" />
                        <a href="#">Forgot your password?</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<script src="@Url.Content("~/Content/js/jquery-1.11.1.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Content/js/jquery.ui.touch-punch.min.js")" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('.validation-summary-errors').addClass('alert alert-danger alert-dismissable');//.insert('<a class="close" data-dismiss="alert" href="#">×</a>');
    });
</script>
