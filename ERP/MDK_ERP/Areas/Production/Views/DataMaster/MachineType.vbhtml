@Code
    ViewData("Title") = "Konfigurasi"
    ViewBag.headIcon = "icon-wrench"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Konfigurasi", "/Production/DataMaster"}, _
                                             {"Tipe Mesin", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

<h4 class='inline'><i class='icon-cogs'></i> Jenis Mesin</h4>
<a data-toggle="modal" id="" data-target="#modal-Data" class="btn btn-sm btn-success pull-right" href="javascript:void(0)">
	<i class="icon-plus"></i> Tambah Baru
</a>
<div class="clear"></div>
    @Using Html.RowBox("box-table")
        @<table id="tb_Data" class="display" cellspacing="0" width="100%">
            <thead>
                <tr>
                    <th>Jenis Mesin</th>
                    <th>Keterangan</th>
                    <th class="action"></th>
                </tr>
            </thead>
        </table>
    End Using
<!-- Start Dummy Form -->
<section class="">

    @Using Html.ModalForm("modal-Data", "Tambah Jenis Mesin", Url.Action("SaveMachineType", "DataMaster"), "", "POST", "form-Data", "form-horizontal")
        @<div class="form-group">
            <label class="col-md-3 control-label">Jenis Mesin</label>
            <div class="col-md-8">
                @Html.Hidden("ID", Nothing)
                @Html.TextBox("MachineType", Nothing, New With {.class = "form-control"})
            </div>
            <div class="clear">
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

</section>
@Section StyleSheet
End Section
@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/DataMaster/MachineTypes.js")"></script>
End Section