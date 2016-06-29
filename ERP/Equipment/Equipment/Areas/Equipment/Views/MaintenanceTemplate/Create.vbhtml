@ModelType Equipment.MaintenanceTemplateItem

@Functions
    Function CreateItemInput() As MvcHtmlString
        Dim openDiv = New MvcHtmlString("<div class='row'><div class='col-sm-4 col-lg-3'>")
        Dim input2 = Html.DropDownList("Unit", Nothing, New With {.class = "form-control"})
     
        Dim sp = New MvcHtmlString("</div><div class='col-sm-4 col-lg-3'>")
        Dim input1 = Html.TextBoxFor(Function(model) model.Value, New With {.class = "form-control text-right numeric", .Value = 0})
        Dim closeDiv = New MvcHtmlString("</div></div>")
        Return HaloUIHelpers.Helpers.Helpers.Concat(openDiv, input1, sp, input2, closeDiv)
    End Function
End Functions

@Using Html.BeginJUIBox("Form Poin Pemeriksaan", False, False, False, False, False, "fa fa-plus")
    Using Html.BeginForm("Save", "MaintenanceTemplate", Nothing, FormMethod.Post, New With {.class = "form form-horizontal", .id = "form-data", .autocomplete = "off"})
    @Html.ValidationSummary(True, "Penyimpanan data tidak berhasil. Harap perbaiki kesalahan dan coba lagi.")

    @<div class="row">
        @Html.HiddenFor(Function(m) m.ID, New With {.Value = 0})
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Item, New With {.class = "form-control"}), "Point Pemeriksaan")
        @Html.WriteFormInput(CreateItemInput, "Pemeriksaan Setiap", lgControlWidth:=6)
        
        @Html.WriteFormInput(Html.DropDownList("MachineTypeID", Nothing, New With {.class = "form-control"}), "Untuk Mesin")
    </div>

    @<div class="row">
        <div class="col-lg-offset-5 col-sm-offset-5">
            <button class="btn btn-primary btn-label-left" type="submit" id="btnSave">
                <span><i class="fa fa-save"></i></span>Simpan</button>
            <button class="btn btn-default btn-label-left" type="reset" id="btnCancel">Batal</button>
        </div>
    </div>
        '</div>
    End Using
End Using
<!-- end form-->