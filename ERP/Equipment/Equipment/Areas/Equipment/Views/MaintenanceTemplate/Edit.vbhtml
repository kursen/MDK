@Code
    ViewData("Title") = "Maintenance Template"
End Code

@ModelType Equipment.MaintenanceTemplateItem
@Code
    ViewData("Title") = "Kendaraan"
End Code
@Using Html.BeginJUIBox("Data Kendaraan", False, False, False, False, False, "fa fa-plus")
    Using Html.BeginForm("Save", "MaintenanceTemplate", Nothing, FormMethod.Post, New With {.class = "form form-horizontal", .id = "form-data", .autocomplete = "off"})
    @Html.ValidationSummary(True, "Penyimpanan data tidak berhasil. Harap perbaiki kesalahan dan coba lagi.")

    @<div class="row">
        @Html.HiddenFor(Function(m) m.ID)
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Item, New With {.class = "form-control"}), "Nama Item")
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Value, New With {.class = "form-control text-right"}), "Nilai", smControlWidth:=2, lgControlWidth:=1)
        @Html.WriteFormInput(Html.DropDownList("Unit", Nothing, New With {.class = "form-control"}), "Satuan")
        @Html.WriteFormInput(Html.DropDownList("MachineTypeID", Nothing, New With {.class = "form-control"}), "Tipe Mesin")
    </div>

    @<div class="row">
        <div class="col-lg-offset-5 col-sm-offset-5">
            <button class="btn btn-primary btn-label-left" type="submit" id="btnSave">
                <span><i class="fa fa-save"></i></span>Simpan</button>
            @Html.ActionLink("Batal", "Index", "MaintenanceTemplate", Nothing, New With {.class = "btn btn-default"})
        </div>
    </div>
        '</div>
    End Using
End Using
<!-- end form-->

<script type="text/javascript">
    submitFormCallback = function (data) {
        if (data.stat == 0) {
            showNotificationSaveError(data);
            return
        } else {
            window.location = "/Equipment/MaintenanceTemplate/";
        }
    }

    $(function () {
        //form submit
        $("#form-data").submit(function (e) {
            e.preventDefault();
            showSavingNotification();
            var _data = $(this).serialize();
            var _url = $(this).attr('action');
            $.ajax({
                type: 'POST',
                url: _url,
                data: _data,
                success: submitFormCallback
            });
        });
    });
</script>