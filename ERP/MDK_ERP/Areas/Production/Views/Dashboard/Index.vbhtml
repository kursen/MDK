@Code
    ViewData("Title") = "Dashboard"
    ViewBag.headIcon = "icon-dashboard"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Production", "/Production"}, _
                                             {"Dashboard", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers
@*@Using Html.WriteSummaryBox("Grafik", "icon-bar-chart", "")
    @<div class="row-fluid">
        <div class="col-sm-12 column" style="min-height: 200px">
        </div>
        <div class="clear">
        </div>
    </div>
End Using
<hr class="hr-normal" />*@
@Using Html.WriteSummaryBox("Hasil Mesin AMP Kemarin", "", "")
    @<table class="table table-striped table-bordered" id = "tbAmp" width="100%">
        <thead>
            <tr>
                <th style="width:35%" rowspan="2">
                    Item
                </th>
                <th style="width:15%" rowspan="2">Jumlah (ton)</th>
                <th colspan ="2">
                    Pemakaian Bahan
                </th>
            </tr>
                <tr>
                    <th style="width:35%">Bahan</th>
                    <th style="width:15%">Jumlah (bucket)</th>
                </tr>

        </thead>
        <tbody>
        </tbody>
    </table>
End Using
<hr class="hr-normal" />
@Using Html.WriteSummaryBox("Pemakaian Bahan", "", "")
@<table class="table table-striped table-bordered" id = "tbInventory" width="100%">
    <thead>
        <tr>
            <th rowspan="2">
                Item
            </th>
            <th rowspan="2">
                Kemarin
            </th>
            <th colspan="2">
                Hari Ini
            </th>
            <th rowspan="2">
                Sisa
            </th>
        </tr>
        <tr>
            <th>
                Bertambah
            </th>
            <th>
                Berkurang
            </th>
        </tr>
    </thead>
    <tbody>
    </tbody>
</table>
    End Using
@Section StyleSheet
End Section
@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/Dashboard.js")"></script>
End Section
