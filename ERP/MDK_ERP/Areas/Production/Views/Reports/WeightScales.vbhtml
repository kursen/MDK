@Code
    ViewData("Title") = "Laporan"
    ViewBag.headIcon = "icon-file-text-alt"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Production", "/Production"}, _
                                             {"Laporan - Berat Timbangan", Nothing}
                                         }).ToString()
End Code

@imports MDK_ERP.HtmlHelpers

@Html.Partial("_DoubleDatePickerPartial")

<h4 class='inline'>
    Berat Timbangan</h4>
<a id="btn-print" class="btn btn-sm btn-success pull-right" href="javascript:void(0)">
    <i class="icon-print"></i> Cetak Laporan</a>
<div class="clear"></div>

@Using Html.RowBox("box-table")
    @<table id="tb_Data" class="display" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th>No. Record</th>
                <th>Masuk</th>
                <th>Keluar</th>
                <th>NoPolisi</th>
                <th>Perusahaan</th>
                <th>Barang</th>
                <th>Keterangan</th>
            </tr>
        </thead>
    </table>
End Using
@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.css")" />
End Section

@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/Reports/WeightScales.js")"></script>
End Section
