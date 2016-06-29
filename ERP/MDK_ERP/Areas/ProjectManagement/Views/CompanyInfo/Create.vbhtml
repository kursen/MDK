@ModelType MDK_ERP.CompanyInfo

@Code
    ViewData("Title") = "Informasi Perusahaan"
    ViewBag.headIcon = "icon-building"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                            {"Home", "/"}, _
                                            {"Project Management", "/ProjectManagement"}, _
                                            {"Informasi Perusahaan", "/ProjectManagement/CompanyInfo"}, _
                                            {"Form Data Perusahaan", Nothing} _
                                         }).ToString()
End Code

@imports MDK_ERP.HtmlHelpers

<h4 class='inline'>
    <i class='icon-edit'></i> Form Perusahaan</h4>

@Using Html.RowBox("", True)
    Using Html.BeginForm("Create", "CompanyInfo", Nothing, FormMethod.Post, New With {.class = "form form-horizontal"})
 @Html.ValidationSummary(True, "Penyimpanan data tidak berhasil. Harap perbaiki kesalahan dan coba lagi.")
    
    @<div class="row">
        <div class="col-sm-6">
            <div class="form-group">
                <label class="col-md-4 control-label">Nama Perusahaan</label>
                <div class="col-md-6">
                @*@Html.Hidden("ID", 0)*@
                @Html.HiddenFor(Function(model) model.ID)
                    @Html.TextBoxFor(Function(model) model.Name, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(model) model.Name)
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">Alias</label>
                <div class="col-md-8">
                    @Html.TextBoxFor(Function(model) model.Alias, New With {.class = "form-control", .style="width:100px;"})
                    @Html.ValidationMessageFor(Function(model) model.Alias)
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">Kota</label>
                <div class="col-md-8">
                    @Html.TextBoxFor(Function(model) model.City, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(model) model.City)
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">Alamat 1</label>
                <div class="col-md-8">
                    @Html.TextAreaFor(Function(model) model.Address1, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(model) model.Address1)
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">Alamat 2</label>
                <div class="col-md-8">
                    @Html.TextAreaFor(Function(model) model.Address2, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(model) model.Address2)
                </div>
            </div>
        </div>
        <hr class="hr-normal xs-show md-hide sm-hide lg-hide" />
        <div class="col-sm-6">
            <div class="form-group">
                <label class="col-md-4 control-label">No. Telepon 1 </label>
                <div class="col-md-8">
                    @Html.TextBoxFor(Function(model) model.Phone1, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(model) model.Phone1)
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">No. Telepon 2 </label>
                <div class="col-md-8">
                    @Html.TextBoxFor(Function(model) model.Phone2, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(model) model.Phone2)
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">No. Fax </label>
                <div class="col-md-8">
                    @Html.TextBoxFor(Function(model) model.Fax, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(model) model.Fax)
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">Keterangan Lain</label>
                <div class="col-md-8">
                    @Html.TextAreaFor(Function(model) model.Annotation, 5, 20, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(model) model.Annotation)
                </div>
            </div>
        </div>
    </div>
    
    @<div class="form-actions form-actions-padding-sm">
        <div class="row">
            <div class="col-md-5 col-md-offset-5">
            <button class="btn btn-primary" type="submit"><i class="icon-save"></i> Simpan</button>
            <button class="btn" type="button" onclick="goBack();"> Kembali</button>
            </div>
        </div>
    </div>
End Using
End Using
@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/Select2/select2.css")" />
End Section

@Section jsScript
    <script src="@Url.Content("~/Content/Plugin/Select2/select2.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>
End Section