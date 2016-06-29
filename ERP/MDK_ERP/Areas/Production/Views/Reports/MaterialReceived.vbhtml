@Code
    ViewData("Title") = "Laporan"
    ViewBag.headIcon = "icon-file-text-alt"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Production", "/Production"}, _
                                             {"Laporan - Pengiriman Material", Nothing}
                                         }).ToString()
End Code

@imports MDK_ERP.HtmlHelpers

@Html.Partial("_DoubleDatePickerPartial")

<h4 class='inline'>
    Laporan Penerimaan Material</h4>
<a id="btn-print" class="btn btn-sm btn-success pull-right" href="@Url.Action("../../Reports/LaporanPenerimaanMaterial.xlsx")">
    <i class="icon-print"></i> Cetak Laporan</a>
<div class="clear"></div>
@Using Html.WriteSummaryBox("Penerimaan Material", "icon-file-text-alt")
    @<table id="tb_rptWeight" class="display table table-striped" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th>No</th>
                <th>Tanggal</th>
                <th>Kode Pengiriman</th>
                <th>Nama Material</th>
                <th>Berat</th>
                <th>Perusahaan</th>
                <th>Sumber</th>
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
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/Reports/MaterialReceived.js")"></script>
End Section
