@Code
    ViewData("Title") = "Konfigurasi"
    ViewBag.headIcon = "icon-wrench"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Konfigurasi", "/Production/DataMaster"}, _
                                             {"Status Inventaris", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

<h4 class='inline'><i class='icon-check'></i> Status Inventaris</h4>
<a data-toggle="modal" id="" data-target="#modal-Data" class="btn btn-sm btn-success pull-right" href="javascript:void(0)">
	<i class="icon-plus"></i> Tambah Baru
</a>
<div class="clear"></div>
    @Using Html.RowBox("box-table")
        @<table id="tb_Data" class="display" cellspacing="0" width="100%">
            <thead>
                <tr>
                    <th>Keterangan</th>
                    <th class="action"></th>
                </tr>
            </thead>
        </table>
    End Using
<!-- Start Dummy Form -->
<section class="">

    @Using Html.ModalForm("modal-Data", "Tambah Jenis Mesin", Url.Action("IS_Save", "DataMaster"), "", "POST", "form-Data", "form-horizontal")
        @<div class="form-group">
            <label class="col-md-3 control-label">Nama Status</label>
            <div class="col-md-8">
                @Html.Hidden("ID")
                @Html.TextBox("StatusName", Nothing, New With {.class = "form-control"})
            </div>
        </div>
    End Using

</section>

@Section StyleSheet
End Section

@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/DataMaster/InventoryStatus.js")"></script>
End Section
