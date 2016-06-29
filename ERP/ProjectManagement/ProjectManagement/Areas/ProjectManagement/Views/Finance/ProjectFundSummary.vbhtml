@ModelType ProjectManagement.ProjectInfo
@Code
    ViewData("Title") = "Rekap Kebutuhan Dana"
End Code
@Html.Partial("ProjectPageMenu", Model)
@Html.Hidden("ProjectInfoId", Model.Id)
@Using Html.BeginJUIBox("Rekapitulasi Kebutuhan Dana Proyek")
    @<table class="table table-bordered dataTable" id="tblData" role="grid" width="100%">
        <colgroup>
            <col style="width: 40px" />
            <col style="width: auto" />
            <col style="width: 150px" />
            <col style="width: 200px" />
            <col style="width: 150px" />
            <col style="width: 200px" />
        </colgroup>
        <thead>
            <tr>
                <th rowspan="2">
                    #
                </th>
                <th rowspan="2">
                    Bulan
                </th>
                <th colspan="4">
                    Progress Kebutuhan Keuangan Bulanan
                </th>
            </tr>
            <tr>
                <th>
                    % / Bulan
                </th>
                <th>
                    Rp
                </th>
                <th>
                    % Kumulatif
                </th>
                <th>
                    Rp
                </th>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="2" class="text-center">
                    T O T A L
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td><td>
                </td>
            </tr>
        </tfoot>
    </table>
End Using
<link rel="stylesheet" type="text/css" href="@Url.Content("~/plugins/datatables/dataTables.bootstrap.css")" />
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/sum.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">

    var tblData = null;
    var _loadData = function () {
        var projectId = $("#ProjectInfoId").val();
        $.ajax({
            type: "POST",
            data: { id: projectId },
            url: "/Finance/GetFundSummaryList",
            success: _loadDataCallback
        });
    }
    var _loadDataCallback = function (d) {
        tblData.clear();
        if (d.data.length > 0) {
            tblData.rows.add(d.data).draw();

        }


    }
    $(function () {  //init
        var arrColumns = [
            { "data": null, "sClass": "text-right" }, //
              {"data": "MonthName" }, //
              {"data": "Weight", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //
              {"data": "MonthlyCost", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //
              {"data": "WeightAccumulation", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //
              {"data": "AccumulationMonthlyCost", "sClass": "text-right", "mRender": _fnRender2DigitDecimal}//
        ];
        var _coldefs = [];
        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = _coldefs;
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = false;
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            for (var i = 0, iLen = oSettings.aiDisplay.length; i < iLen; i++) {
                $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html((i + 1) + '.');
            }
            var api = this.api();
            var totalPercentage = api.column(2, { page: 'current' }).data().sum();
            var totalFund = api.column(3, { page: 'current' }).data().sum()
            console.log(totalPercentage);
            console.log(totalFund);
            $(api.table().footer()).children("tr:first").find("td:eq(1)").html($.number(totalPercentage, 2, ",", "."));
            $(api.table().footer()).children("tr:first").find("td:eq(2)").html($.number(totalFund, 2, ",", "."));
            $(api.table().footer()).children("tr:first").find("td:eq(3)").html($.number(totalPercentage, 2, ",", "."));
            $(api.table().footer()).children("tr:first").find("td:eq(4)").html($.number(totalFund, 2, ",", "."));

        };
        tblData = $("#tblData").DataTable(datatableDefaultOptions);
        _loadData();
    });              //end init
    
</script>
