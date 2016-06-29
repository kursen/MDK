@Code
    ViewData("Title") = "Stock Awal"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code
@imports MDK_ERP.HtmlHelpers
@*
<h4 class='inline'><i class='icon-archive'></i> Stock Awal</h4>*@
<a data-toggle="modal" id="" data-target="#modal-Data" class="btn btn-sm btn-success pull-right" href="javascript:void(0)">
	<i class="icon-plus"></i> Tambah Baru
</a>
<div class="clear"></div>
    @Using Html.RowBox("box-table")
        @<table id="tb_Data" class="display" cellspacing="0" width="100%">
            <thead>
                <tr>
                    <th>Tanggal</th>
                    <th>Nama Material</th>
                    <th>Jumlah Input</th>
                    <th>Satuan</th>
                    <th>Keterangan</th>
                    <th class="action"></th>
                </tr>
            </thead>
        </table>
    End Using
<!-- Start Dummy Form -->
<section class="">
<!--** start modal **-->
     <!-- >> Material -->

    @Using Html.ModalForm("modal-Data", "Tambah Stok Awal", Url.Action("SaveInitialStock", "Stock"), "", "POST", "form-Data", "form-horizontal")
        @<div class="form-group">
            <label class="col-md-3 control-label">Tanggal</label>
            <div class="col-md-5">
                <div class='input-group date' id='dtpDateInput'>
                    @Html.TextBox("DateInput", Now, New With {.class = "form-control"})
                    <span class="input-group-addon"><span class="icon-calendar"></span></span>
                </div>
            </div>
        </div>
        @<div class="form-group">
            <label class="col-md-3 control-label">Nama Material</label>
            <div class="col-md-5">
                @Html.DropDownList("IDMaterial", Nothing, New With {.class = "form-control"})
            </div>
        </div>
        @<div class="form-group">
            <label class="col-md-3 control-label">Jumlah Input</label>
            <div class="col-md-8">
                @Html.TextBox("Amount", Nothing, New With {.class = "form-control"})
            </div>
            <div class="clear">
            </div>
        </div>
        @<div class="form-group">
            <label class="col-md-3 control-label">Satuan</label>
            <div class="col-md-5">
                @Html.DropDownList("IDMeasurementUnit", Nothing, New With {.class = "form-control"})
            </div>
        </div>
        @<div class="form-group">
            <label class="col-md-3 control-label">Keterangan</label>
            <div class="col-md-8">
                @Html.TextArea("Description", New With {.class = "form-control"})
            </div>
            <div class="clear">
            </div>
        </div>
    End Using
    <!-- end >> Material -->
<!-- end modal -->

</section>
@Section StyleSheet
End Section
@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/Inventory/InitialStock.js")"></script>
End Section