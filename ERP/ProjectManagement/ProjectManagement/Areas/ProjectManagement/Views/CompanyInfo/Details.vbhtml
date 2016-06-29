@ModelType ProjectManagement.CompanyInfo

@Code
    ViewData("Title") = "Informasi Kontraktor"
    ViewData("HeaderIcon") = "fa-building"
End Code

<style type="text/css">
    input[disabled],textarea[disabled] {
        cursor:default !important;
        border-bottom:1px solid #ccc;
    }
    a.btn {
        color:Black !important;
    }
</style>

@Using Html.BeginJUIBox("Details Data Perusahaan", iconClass:="fa fa-edit")
    Using Html.BeginForm("", "", Nothing, FormMethod.Post, New With {.class = "form form-horizontal", .autocomplete = "off"})
    @<div class="row">
        <div class="col-sm-6">
            <div class="form-group">
                <label class="col-md-4 control-label">Nama Perusahaan</label>
                <div class="col-md-8">
                    @Html.TextBoxFor(Function(model) model.Name, New With {.class = "form-control input-none", .disabled = "disabled"})
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">Singkatan</label>
                <div class="col-md-8">
                    @Html.TextBoxFor(Function(model) model.Alias, New With {.class = "form-control input-none", .disabled = "disabled"})
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">Kota</label>
                <div class="col-md-8">
                    @Html.TextBoxFor(Function(model) model.City, New With {.class = "form-control input-none", .disabled = "disabled"})
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">Alamat 1</label>
                <div class="col-md-8">
                    @Html.TextAreaFor(Function(model) model.Address1, New With {.class = "form-control input-none", .disabled = "disabled"})
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">Alamat 2</label>
                <div class="col-md-8">
                    @Html.TextAreaFor(Function(model) model.Address2, New With {.class = "form-control input-none", .disabled = "disabled"})
                </div>
            </div>
        </div>
        <hr class="hr-normal xs-show md-hide sm-hide lg-hide" />
        <div class="col-sm-6">
            <div class="form-group">
                <label class="col-md-4 control-label">No. Telepon 1 </label>
                <div class="col-md-8">
                    @Html.TextBoxFor(Function(model) model.Phone1, New With {.class = "form-control input-none", .disabled = "disabled"})
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">No. Telepon 2 </label>
                <div class="col-md-8">
                    @Html.TextBoxFor(Function(model) model.Phone2, New With {.class = "form-control input-none", .disabled = "disabled"})
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">No. Fax </label>
                <div class="col-md-8">
                    @Html.TextBoxFor(Function(model) model.Fax, New With {.class = "form-control input-none", .disabled = "disabled"})
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">Keterangan Lain</label>
                <div class="col-md-8">
                    @Html.TextAreaFor(Function(model) model.Annotation, 5, 20, New With {.class = "form-control input-none", .disabled = "disabled"})
                </div>
            </div>
        </div>
    </div>
    
    @<div class="form-actions form-actions-padding-sm">
        <div class="row">
            <div class="col-md-5 col-md-offset-5">
            <a href="@Url.Action("Index","CompanyInfo")" class="btn" type="button" onclick="goBack();"> Kembali</a>
            </div>
        </div>
    </div>
    End Using
End Using