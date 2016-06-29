@ModelType MDK_ERP.NewUser
    
@Code
    ViewData("Title") = "Pengaturan User"
    ViewBag.headIcon = "icon-user"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Pengaturan User", "/UserManagement"}, _
                                             {"Form User", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

<h4 class='inline'>
    <i class='icon-user-md'></i> Form User</h4>

@Using Html.RowBox("", True)
    Using Html.BeginForm("Create", "UserManagement", Nothing, FormMethod.Post, New With {.class = "form form-horizontal"})
    @Html.ValidationSummary(True, "Pembuatan account tidak berhasil. Harap perbaiki kesalahan dan coba lagi.")
    @<div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label class="col-md-3 control-label">Nama User</label>
                <div class="col-md-8">
                    @Html.Hidden("ID")
                    @Html.TextBox("UserName", Nothing, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(m) m.UserName)
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-3 control-label">Password</label>
                <div class="col-md-8">
                    @Html.Password("Password", Nothing, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(m) m.Password)
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-3 control-label">Konfirmasi Password</label>
                <div class="col-md-8">
                    @Html.Password("PasswordConfirm", Nothing, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(m) m.PasswordConfirm)
                </div>
            </div>
            <hr class="hr-normal" />
        </div>
        <div class="col-md-6">
            @code
                Dim allRoles = CType(ViewData("allRoles"), String())
                Dim start As Integer = 1
                Dim checked As String = ""
            End Code
            <div class="form-group">
                <label class="col-md-3 control-label">Hak Akses</label>
                <div class="col-md-8">
                     @For Each r In allRoles
                             Try
                                 checked = IIf(String.IsNullOrEmpty(Model.UserName), "", IIf(Roles.IsUserInRole(Model.UserName, r), "checked='checked'", ""))
                           @<div class="checkbox">
                                <label>
                                    <input type="checkbox" id='userroles-@start' value="@r" name='UserRole' @checked />
                                    @r
                                </label>
                            </div>
                        Catch ex As Exception
                            @<div class="checkbox">
                                <label>
                                    <input type="checkbox" id='userroles-@start' name='UserRole' value="@r" /> 
                                    @r
                                </label>
                            </div>
                     End Try
                     Next
                    @Html.ValidationMessageFor(Function(m) m.UserRole)
                </div>
            </div>
        </div>
        <div class="form-actions form-actions-padding-sm">
            <div class="col-md-8 col-md-offset-5">
            <button class="btn btn-primary" type="submit"><i class="icon-save"></i> Simpan</button>
            <button class="btn" type="button" onclick="goBack();"> Kembali</button>
            </div>
        </div>
        </div>
    
     End Using
End Using

@Section jsScript
<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>
End Section