@ModelType ProjectManagement.CompanyInfo

@Code
    ViewData("Title") = "Informasi Kontraktor"
    ViewData("HeaderIcon") = "fa-building"
End Code

@Using Html.BeginJUIBox("Form Data Perusahaan", iconClass:="fa fa-edit")
    Using Html.BeginForm("Save", "CompanyInfo", Nothing, FormMethod.Post, New With {.class = "form form-horizontal", .id = "frmcompanyinfo", .autocomplete = "off"})
 @Html.ValidationSummary(True, "Penyimpanan data tidak berhasil. Harap perbaiki kesalahan dan coba lagi.")
    
    @<div class="row">
        <div class="col-sm-6">
            <div class="form-group">
                <label class="col-md-4 control-label">Nama Perusahaan</label>
                <div class="col-md-6">
                @Html.HiddenFor(Function(model) model.ID)
                    @Html.TextBoxFor(Function(model) model.Name, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(model) model.Name)
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-4 control-label">Singkatan</label>
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
            <button class="btn btn-primary" type="button" id="btnSave"><i class="fa fa-save"></i> Simpan</button>
            <button class="btn" type="button" onclick="goBack();"> Kembali</button>
            </div>
        </div>
    </div>
    End Using
End Using
<script type="text/javascript">
    var btnSubmit = null;
    submitFormCallback = function (data) {
        if (data.stat == 1) {
            window.location = "/ProjectManagement/CompanyInfo/";
            return;
        }
        btnSubmit.button("Submit");

        showNotificationSaveError(data);
    }
    $(function () {

        $("#btnSave").click(function () {
            btnSubmit = $(this).button("menyimpan data...");
            showSavingNotification();
            var _data = $("#frmcompanyinfo").serialize();
            var _url = $("#frmcompanyinfo").attr("action");
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


    
    </script>