@Code
    ViewData("Title") = "Jurnal Crusher"
    ViewBag.headIcon = "icon-tasks"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Production", "/Production"}, _
                                             {"Jurnal Crusher", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers
    <div class="form form-horizontal">
        <div class="col-xs-6 col-sm-12 col-md-12 col-lg-7">
        <div class="form-group">
            <label class="col-md-2 col-sm-2 col-xs-4 text-left control-label">Periode</label>
            <div class="col-md-4 col-sm-4 col-xs-8">
                <div class='input-group date' id='datetimepicker'>
                    <input type='button' class="form-control" placeholder="MMMM YYYY" />
                    <span class="input-group-addon">
                        <span class="icon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 col-sm-2 col-xs-4 text-left control-label"></label>
            <div class="col-md-4 col-sm-4 col-xs-8">
                <a onclick="" id="filter" href="javascript:void(0)" class="btn btn-primary">Filter</a>
                <span class="loader hidden"></span>
            </div>
        </div>

        </div>
    </div>

<div class="pull-right">
<a class="btn btn-sm btn-success" href="@Url.Action("CreateCrusherJournal", "CrusherJournals")" style="margin-top: 50px;">
    <i class="icon-plus"></i> Tambah Baru</a>
<a class="btn btn-sm btn-primary" id="print" href="javascript:void(0)"  style="margin-top: 50px;">
    <i class="icon-print"></i> Cetak Laporan</a>
</div>
<div class="clear">
</div>
@Using Html.RowBox("box-table")
    @<table id="tb_Data" class="display table table-bordered table-striped" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th rowspan="6">
                    No
                </th>
                <th rowspan="6">
                    Tanggal
                </th>
                <th colspan="9">
                    Keterangan
                </th>
                <th rowspan="2">
                    Satuan
                </th>
                <th rowspan="6">
                    M3
                </th>
                <th rowspan="2">
                    Jumlah
                </th>
                <th rowspan="6">
                    Keterangan
                </th>
                <th rowspan="6">
                    Anggota
                </th>
                <th rowspan="6" class="action">
                </th>
            </tr>
            <tr>
                <th>
                    Grogol
                </th>
                <th>
                    Gresley
                </th>
                <th>
                    Medium
                </th>
                <th>
                    Abu Batu
                </th>
                <th>
                    Base A
                </th>
                <th>
                    Base B
                </th>
                <th>
                    Split 1-2
                </th>
                <th>
                    Split 2-3
                </th>
                <th>
                    Gresley
                </th>
            </tr>
            <tr>
                <th colspan="9">
                    Jumlah material per (m3)
                </th>
                <th rowspan="3">
                    Total Material (bucket)
                </th>
                <th rowspan="3">
                    Total Material (m3)
                </th>
            </tr>
            <tr>
                <th id="totalM3Grogol"></th>
                <th id="totalM3GresleyIn"></th>
                <th id="totalM3Medium"></th>
                <th id="totalM3AbuBatu"></th>
                <th id="totalM3BaseA"></th>
                <th id="totalM3BaseB"></th>
                <th id="totalM3Split12"></th>
                <th id="totalM3Split23"></th>
                <th id="totalM3GresleyOut"></th>
            </tr>
            <tr>
                <th colspan="9">
                    Jumlah material per (bucket)
                </th>
            </tr>
            <tr>
                <th id="totalGrogol"></th>
                <th id="totalGresleyIn"></th>
                <th id="totalMedium"></th>
                <th id="totalAbuBatu"></th>
                <th id="totalBaseA"></th>
                <th id="totalBaseB"></th>
                <th id="totalSplit12"></th>
                <th id="totalSplit23"></th>
                <th id="totalGresleyOut"></th>
                <th id="totalSatuan"></th>
                <th id="totalJumlah"></th>
            </tr>
        </thead>
        <tbody>
        </tbody>
    </table>
End Using

@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.css")" />
End Section
@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/CrusherJournals/CrusherJournals.js")"></script>
End Section
