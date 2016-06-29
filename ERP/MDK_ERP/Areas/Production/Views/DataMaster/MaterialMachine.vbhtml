@Code
    ViewData("Title") = "Konfigurasi"
    ViewBag.headIcon = "icon-wrench"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Konfigurasi", "/Production/DataMaster"}, _
                                             {"Daftar Mesin", Nothing}
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

<h4 class='inline'><i class='icon-cogs'></i> Daftar Mesin</h4>
<a data-toggle="modal" id="" data-target="#modal-Data" class="btn btn-sm btn-success pull-right" href="javascript:void(0)">
	<i class="icon-plus"></i> Tambah Baru
</a>
<div class="clear"></div>
    @Using Html.RowBox("box-table")
        @<table id="tb_Data" class="display" cellspacing="0" width="100%">
            <thead>
                <tr>
                    <th>Nama Mesin</th>
                    <th>Jenis Mesin</th>
                    <th>Nomor Seri</th>
                    <th class="action"></th>
                </tr>
            </thead>
        </table>
    End Using
<!-- Start Dummy Form -->
<section class="">
<!--** start modal **-->
     <!-- >> Material -->

    @Using Html.ModalForm("modal-Data", "Tambah Mesin", Url.Action("SaveMachine", "DataMaster"), "", "POST", "form-Data", "form-horizontal")
        @<div class="form-group">
            <label class="col-md-3 control-label">Nama Mesin</label>
            <div class="col-md-4">
                @Html.Hidden("ID", 0)
                @Html.TextBox("MachineName", Nothing, New With {.class = "form-control"})
            </div>
            <div class="clear">
            </div>
        </div>
        @<div class="form-group">
            <label class="col-md-3 control-label">Jenis Mesin</label>
            <div class="col-md-4">
                 @Html.DropDownList("IDMachineType", New SelectList(DirectCast(ViewData("MachineTypes"), IEnumerable), "ID", "MachineType"), Nothing, New With {.class = "form-control select2"})
            </div>
            <div class="clear">
            </div>
        </div>
        @<div class="form-group">
            <label class="col-md-3 control-label">Nomor Seri</label>
            <div class="col-md-3">
                @Html.TextBox("SeriesNumber", Nothing, New With {.class = "form-control"})
            </div>
            <div class="clear">
            </div>
        </div>
    End Using
    <!-- end >> Material -->
<!-- end modal -->

</section>
@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/Select2/select2.css")" />
End Section
@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/DataMaster/Machines.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/Select2/select2.js")"></script>
End Section