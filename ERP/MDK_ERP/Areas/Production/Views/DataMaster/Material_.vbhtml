@ModelType MDK_ERP.MstMaterials
  
@Code
    ViewData("Title") = "Konfigurasi"
    ViewBag.headIcon = "icon-wrench"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Konfigurasi", "/Production/DataMaster"}, _
                                            {"Daftar Material", "/Production/DataMaster/Material"}, _
                                            {"Form Data Material", Nothing}
                                         }).ToString()
End Code

@imports MDK_ERP.HtmlHelpers

<h4 class='inline'>
    <i class='icon-book'></i> Material - Form Material</h4>

@Using Html.RowBox("", True)
    Using Html.BeginForm("Material_", "DataMaster", Nothing, FormMethod.Post, New With {.class = "form form-horizontal"})
 @Html.ValidationSummary(True, "Penyimpanan data Material tidak berhasil. Harap perbaiki kesalahan dan coba lagi.")
    @<div class="form-group">
        <label class="col-md-2 control-label">Kode</label>
        <div class="col-md-5">
        @Html.Hidden("ID", 0)
            @Html.TextBox("Code", Nothing, New With {.class = "form-control", .Style = "max-width:120px;"})
            @Html.ValidationMessageFor(Function(m) m.Code)
        </div>
    </div>
    @<div class="form-group">
        <label class="col-md-2 control-label">Nomor Material</label>
        <div class="col-md-3">
            @Html.TextBox("Symbol", Nothing, New With {.class = "form-control",.Style = "max-width:120px;"})
            @Html.ValidationMessageFor(Function(m) m.Symbol)
        </div>
    </div>
    @<div class="form-group">
        <label class="col-md-2 control-label">Nama Material</label>
        <div class="col-md-3">
            @Html.TextBox("Name", Nothing, New With {.class = "form-control"})
            @Html.ValidationMessageFor(Function(m) m.Name)
        </div>
    </div>
    @<div class="form-group">
        <label class="col-md-2 control-label">Jenis Material</label>
        <div class="col-md-3">
            @Html.DropDownList("IDMaterialType", DirectCast(ViewData("MaterialTypes"), IEnumerable(Of SelectListItem)), New With {.class = "form-control select2"})
            @Html.ValidationMessageFor(Function(m) m.IdMaterialType)
        </div>
    </div>
    @<div class="form-group">
        <label class="col-md-2 control-label">Satuan</label>
        <div class="col-md-3">
                @Html.DropDownList("IdMeasurementUnit", New SelectList(DirectCast(ViewData("unit"), IEnumerable), "ID", "Unit"), Nothing, New With {.class = "form-control select2"})
            @Html.ValidationMessageFor(Function(m) m.IdMeasurementUnit)
        </div>
    </div>
    @<div class="form-group">
        <label class="col-md-2 control-label">Stok Awal</label>
        <div class="col-md-3">
                 
                @Html.TextBoxFor(Function(m) m.InitialStock, New With {.class = "form-control text-right", .Style = "max-width:120px;", .Value = 0})
            @Html.ValidationMessageFor(Function(m) m.InitialStock)
        </div>
    </div>
    @*@<div class="form-group">
        <label class="col-md-2 control-label">Tanggal Stock</label>
        <div class="col-md-3">
                @Html.TextBox("DateInitialStock", Nothing, New With {.class = "form-control"})
            @Html.ValidationMessageFor(Function(m) m.DateInitialStock)
        </div>
    </div>*@
    @<div class="form-group">
        <label class="col-md-2 control-label"></label>
        <div class="col-md-3">
            <label class="checkbox-inline">
            @Html.CheckBox("isInventory", New With {.class = "form-control"})
            Termasuk Stok
            </label>
        </div>
    </div>
    @<div class="form-group">
        <label class="col-md-2 control-label">Keluaran dari Mesin</label>
        <div class="col-md-3">
                @Html.DropDownList("IDMachineType", New SelectList(DirectCast(ViewData("IDMachineTypes"), IEnumerable), "ID", "MachineType"), Nothing, New With {.class = "form-control select2"})
        </div>
    </div>
    @<div class="form-actions form-actions-padding-sm">
        <div class="row">
            <div class="col-md-5 col-md-offset-2">
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