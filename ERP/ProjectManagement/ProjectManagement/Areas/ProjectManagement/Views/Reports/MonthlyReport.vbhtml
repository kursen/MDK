@code
    ViewData("Title") = "Informasi Proyek"
    ViewData("ForMC0") = False
End Code
@Html.Partial("ProjectPageMenu", ViewData("ProjectInfo"))

<style type="text/css">
</style>

@Using Html.BeginJUIBox("Laporan Bulanan")
    @<div style="min-height: 300px;">
        <div id="layerRptForm" style="display:none;">
            <div class="row">
                <div class="col-sm-6">
                    <h3>Approval Form</h3>
                </div>
                <div class="col-sm-6">
                    <div class="pull-right">
                        <button type="button" class="btn btn-danger btn-label-left btnLayerList" id="">
                            <span><i class="fa fa-arrow-left"></i></span>Kembali</button>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-xs-12">
                    @Html.Partial("MonthlyReport_Form", ViewData("MonthlyReport"))
                </div>
            </div>
        </div>

        <div id="layerList">
            <div class="row">
                <div class="col-sm-12 col-lg-12">
                    <table id="tb_MonthList" class="table table-striped table-hover table-heading table-datatable dataTable responsive no-footer scrollTable" width="100%">
                        <thead class="fixedHeader" id="fixedHeader">
                            <tr>
                                <th>#</th>
                                <th>Bulan Ke</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody class="scrollContent">
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="@Url.Content("~/js/CRUDHelpers.js")" type="text/javascript"></script>
<script src="@Url.Content("~/js/shared-function.js")" type="text/javascript"></script>


<link href="@Url.Content("~/plugins/bootstrap-datepicker/bootstrap-datepicker3.css")" rel="stylesheet" />
<script src="@Url.Content("~/plugins/bootstrap-datepicker/bootstrap-datepicker.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/bootstrap-datepicker/bootstrap-datepicker.id.min.js")" type="text/javascript"></script>
<script type="text/javascript">
$(function () {
    $('#dtpk_DateApproval').datepicker({
        "format": "dd-mm-yyyy",
        "autoclose": "true",
        "orientation": "top left",
        "todayBtn": "linked",
        "language": "id",
        "daysOfWeekDisabled": [0]
    });
    $('#dtpk_DateApproval').val('');
});
</script>

<script type="text/javascript">
    var _renderCheckBox = function (data, type, row) {
        if (type == 'display' && data != null) {
            return "<input type='checkbox' value='" + data + "'  />";
        }
        return data;
    }
    var _renderDetail = function (data, type, row) {
        if (type == 'display') {
            if (data != null && data != 0)
                return "<div class='btn-group' role='group'>" + 
                    "<button class='btn btn-default btn-xs print'><i class='fa fa-print'></i></button>" +
                    "<button class='btn btn-default btn-xs edit'><i class='fa fa-edit'></i></button>" +
                    "<button class='btn btn-default btn-xs delete'><i class='fa fa-remove'></i></button>" + 
                 "</div>";
            else
                return "<a href='javascript:void(0)' onclick='' class='ajax-link btn btn-primary btn-xs btnLayerRptForm'>Detail</a>";
        }
        return data;
    }

    GetList = function () {
        var attr = _attrCRUD();
        attr.dataTable.autoWidth = false;
        attr.dataTable.bSort = false;
        attr.useRowNumber = true;
        attr.usingId.tableId = '#tb_MonthList';
        attr.url = {
            "Read": "ProjectManagement/Reports/GetReportListMonthItems/" + @model.Id,
            "Delete": "ProjectManagement/Reports/RemoveMonthlyDetail"
        };
        attr.dataTable.columns = [
            {
                "data": null,
                "className" : "text-right",
                "bSortable": false
            },
            {
                "data": "MonthNumber",
                "bSortable": false,
                "mRender":function(data, type, full){
                   return full.MonthNumber + " ("  + formatShortDate(full.BeginDate) + " s/d " + formatShortDate(full.EndDate) + ")";
                }
            },
            {
                "data": "ID",
                "className" : "text-center",
                "mRender": _renderDetail
            }
        ];
        attr.dataTable.bPaginate = false;

        fnGetList(attr);

        $(attr.usingId.tableId + ' tbody')
            .on('click', '.print', function () {
                var Data = GenTable.row($(this).parents('tr')).data();
                window.location.href = "/ProjectManagement/Reports/MonthlyReportPrint/" + Data.ID;
            })
            .on('click', '.btnLayerRptForm, .edit', function () {
                var Data = GenTable.row($(this).parents('tr')).data();
                    if (!isAnyError){
                        _openDetail(Data);
                    }
                    $("#layerList").fadeOut(400, function () {
                        $("#layerRptForm").show("slide", { direction: "right" }, 430);
                    });
            });
    }
</script>

<script type="text/javascript">
    _closeDetail = function () {
        $(".btnLayerList").click()
        $("#btnBatal").click()
    }

    _openDetail = function (Data) {
        _closeForm();
        $("#layerList").fadeOut(400, function () {
            $("#layerRptForm").show("slide", { direction: "right" }, 430);
            $("#MingguKe").val(Data.MonthNumber + " (" + formatShortDate(Data.BeginDate) + " s/d " + formatShortDate(Data.EndDate) + ")");
            $("#ProjectInfoId").val(@Model.Id);
            $("#MonthNumber").val(Data.MonthNumber);

            $("#ID").val(Data.ID);
            $("#ImplementingActivities").val(Data.ImplementingActivities);
            $("#Place").val(Data.Place);
            $("#DateApproval").val(Data.DateApproval == null ? '' : formatShortDate(Data.DateApproval));
        
            $("#Approval1Occupation").val(Data.Approval1Occupation);
            $("#Approval1Company").val(Data.Approval1Company);
            $("#Approval1Name").val(Data.Approval1Name);
            $("#Approval1Identity").val(Data.Approval1Identity);

            $("#Approval2Occupation").val(Data.Approval2Occupation);
            $("#Approval2Company").val(Data.Approval2Company);
            $("#Approval2Name").val(Data.Approval2Name);
            $("#Approval2Identity").val(Data.Approval2Identity);

            $("#Approval3Occupation").val(Data.Approval3Occupation);
            $("#Approval3Company").val(Data.Approval3Company);
            $("#Approval3Name").val(Data.Approval3Name);
            $("#Approval3Identity").val(Data.Approval3Identity);
        });
        isAnyError = false;
    }

    _closeForm = function () {
        if ($(this).is(":visible")) {
        }
    }
    $(function () {
    
        GetList();

        $("#btnBatal,.btnLayerList").click(function () {
            if ($("#layerDetail").is(":visible")) {
                $("#layerDetail").hide("slide", { direction: "right" }, 300, function () {
                    $("#layerList").fadeIn(500);
                });
            } else {
                $("#layerRptForm").hide("slide", { direction: "right" }, 300, function () {
                    $("#layerList").fadeIn(500);
                });
            }
        });

    });
</script>
