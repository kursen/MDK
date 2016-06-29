@code
    ViewData("Title") = "Informasi Proyek"
End Code
@Html.Partial("ProjectPageMenu", ViewData("ProjectInfo"))

<link href="@Url.Content("~/plugins/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css")" rel="stylesheet">
<style type="text/css">
/* Terence Ordona, portal[AT]imaputz[DOT]com         */
/* http://creativecommons.org/licenses/by-sa/2.0/    */

.border-none {
    border-left:medium none !important;
    border-right:medium none !important;
}
.border-left-none {
    border-left:medium none !important;
}
.border-right-none {
    border-right:medium none !important;
}


#layerRptForm table thead th 
{
    border:medium none !important;
}
</style>


@Using Html.BeginJUIBox("Laporan Harian")
    @<div style="min-height: 300px;">
    
        <div id="layerDetail" style="display:none;">
            <div class="row">
                <div class="col-md-12 col-xs-12">
                    <div class="pull-right">
                        <button type="button" class="btn btn-danger btn-label-left btnLayerList">
                            <span><i class="fa fa-arrow-left"></i></span>Kembali</button>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-xs-12">
				<div class="table-responsive">
                    <table id="tb_Detail" class="table table-striped table-hover table-heading table-datatable dataTable responsive no-footer" cellspacing="0" width="100%">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Name</th>
                                <th>Position</th>
                            </tr>
                        </thead>
                    </table>
					</div>
                </div>
            </div>
        </div>

        <div id="layerRptForm" style="display:none;">
            <div class="row">
                <div class="col-md-12 col-xs-12">
                    <div class="pull-right">
                        <button type="button" class="btn btn-danger btn-label-left btnLayerList" id="">
                            <span><i class="fa fa-arrow-left"></i></span>Kembali</button>
                        <button type="button" class="btn btn-danger btn-label-left" id="btnPrint">
                            <span><i class="fa fa-print"></i></span>Print</button>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-xs-12">
                    @Html.Partial("DailyReport_Form", ViewData("ProjectInfo"))
                </div>
            </div>
        </div>

        <div id="layerList">
            <div class="row">
                <div class="col-sm-6 col-xs-12">
                </div>
                <div class="col-sm-6 col-xs-12">
                    <div class="pull-right">

                        <button type="button" class="btn btn-danger btn-label-left" id="btnPrintRange" disabled="disabled">
                            <span><i class="fa fa-print"></i></span>Print</button>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 col-lg-12">
				<div class="table-responsive">
                    <table id="tb_DateList" class="table table-striped table-hover table-heading table-datatable dataTable responsive no-footer scrollTable" width="100%">
                        <colgroup>
                            <col style="width:70px" />
                            <col />
                            <col />
                            <col style="width:100px" />
                        </colgroup>
                        <thead class="fixedHeader" id="fixedHeader">
                            <tr>
                                <th>#</th>
                                <th><input type="checkbox" id="selectAll" value=""/></th>
                                <th>Tanggal</th>
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

    </div>
End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/jquery-number/jquery.numeric.js")" type="text/javascript"></script>

<script src="@Url.Content("~/js/CRUDHelpers.js")" type="text/javascript"></script>
<script src="@Url.Content("~/js/shared-function.js")" type="text/javascript"></script>

<script src="@Url.Content("~/plugins/bootstrap-datetimepicker/bootstrap-datetimepicker.js")" type="text/javascript"></script>

<script type="text/javascript">
    var modelID = @Model.Id
</script>

<script type="text/javascript" src="@Url.Content("~/Areas/ProjectManagement/Scripts/DailyReports/DailyReport_A.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Areas/ProjectManagement/Scripts/DailyReports/DailyReport_B.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Areas/ProjectManagement/Scripts/DailyReports/DailyReport_C.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Areas/ProjectManagement/Scripts/DailyReports/DailyReport_D.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Areas/ProjectManagement/Scripts/DailyReports/DailyReport_E.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Areas/ProjectManagement/Scripts/DailyReports/DailyReport_F.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Areas/ProjectManagement/Scripts/DailyReports/DailyReport_G.js")"></script>
<script type="text/javascript">
    var _renderCheckBox = function (data, type, row) {
        if (type == 'display' && data != null) {
            return "<input type='checkbox' value='" + data + "'  />";
        }
        return data;
    }
    var _renderDetail = function (data, type, row) {
        if (type == 'display')
            return "<a href='javascript:void(0)' onclick='' class='ajax-link btn btn-primary btn-xs btnLayerRptForm'>Detail</a>";
        return data;
    }

    GetList = function () {
        var attr = _attrCRUD();
        attr.dataTable.autoWidth = false;
        attr.useRowNumber = true;
        attr.dataTable.bSort = false;
        attr.usingId.tableId = '#tb_DateList';
        attr.url = {
            "Read": "ProjectManagement/Reports/GetReportListDateItems/" + @model.Id,
            "RefreshTable": function (id) {
                return "ProjectManagement/Reports/GetDailyReportData/" + id
            }
        };
        attr.dataTable.columns = [
            {
                "data": null,
                "className" : "text-right",
                "bSortable": false
            },
            {
                "data": "ID",
                "className" : "text-center",
                "bSortable": false,
                "mRender": _renderCheckBox
            },
            {
                "data": "DateItem",
                "mRender": _fnRenderNetDate
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
            .on('click', '.btnLayerRptForm', function () {
                var Data = GenTable.row($(this).parents('tr')).data();
                fnRefreshTable('#tb_DetailA', parseUrl(attr_DetA.url.RefreshTable(@Model.Id,Data.DayNum)), function () {});
                fnRefreshTable('#tb_DetailB', parseUrl(attr_DetB.url.RefreshTable(@Model.Id,Data.DayNum)), function () {});
                fnRefreshTable('#tb_DetailC', parseUrl(attr_DetC.url.RefreshTable(@Model.Id,Data.DayNum)), function () {});
                fnRefreshTable('#tb_DetailD', parseUrl(attr_DetD.url.RefreshTable(@Model.Id,Data.DayNum)), function () {});
                fnRefreshTable('#tb_DetailE', parseUrl(attr_DetE.url.RefreshTable(@Model.Id,Data.DayNum)), function () {});
                fnRefreshTable('#tb_DetailF', parseUrl(attr_DetF.url.RefreshTable(@Model.Id,Data.DayNum)), function () {});
                fnRefreshTable('#tb_DetailG', parseUrl(attr_DetG.url.RefreshTable(@Model.Id,Data.DayNum)), function () {});
                if (!isAnyError){
                    _openDetail(Data);
                }
            })
            .on('change', 'input[type="checkbox"]', function () {
                var oTbody = $('table#tb_DateList tbody');
                var lenChecked = $('input[type="checkbox"]:checked', oTbody).length;
                if(lenChecked > 0){
                    $('#btnPrintRange').prop('disabled', false);
                    if (lenChecked == $('input[type="checkbox"]', oTbody).length)
                        $('#selectAll').prop('checked', true);
                    else
                        $('#selectAll').prop('checked', false);
                } else {
                    $('#btnPrintRange').prop('disabled', true);
                }
                
            });
    }
</script>

<script type="text/javascript">
    _closeDetail = function(){
        $(".btnLayerList").click()
    }

    _openDetail = function(Data){
        _closeForm();
        $("#layerList").fadeOut(400, function () {
            $('#form_A,#form_B,#form_C,#form_D,#form_E,#form_F,#form_G').hide();
            $("#layerRptForm").show("slide", { direction: "right" }, 430);
            $("#DateItem, .DateItem").attr("value",formatShortDate(Data.DateItem));
            $(".DayWork").val(Data.DayNum);
        });
        isAnyError = false;
    }

    _openForm = function(type){
        switch(type){
            case 'A':
                $('#btnAdd_A').click();
            break;
        }
    }

    _closeForm = function(type){
        if ($(this).is(":visible")) {
            switch(type){
                case 'A':
                    $('#btnAdd_A').click();
                    $('#btnAdd_A').html('<span><i class="fa fa-plus"></i></span> Tambah');
                break;
            }
        }
    }

    $(function () {
        GetListDetail_A(@Model.Id, null);
        GetListDetail_B(@Model.Id, null);
        GetListDetail_C(@Model.Id, null);
        GetListDetail_D(@Model.Id, null);
        GetListDetail_E(@Model.Id, null);
        GetListDetail_F(@Model.Id, null);
        GetListDetail_G(@Model.Id, null);
        GetList();

        $("#btnPrint").click(function () {
            window.location.href = "/ProjectManagement/Reports/DailyOneReportPrint?ProjectDiv=" + @Model.Id + "&DayWork=" + $('.DayWork').val();
        });

        $("#btnPrintRange").click(function () {
            var checkedValues = $('table#tb_DateList td input[type="checkbox"]:checked').map(function () {
                return this.value;
            }).get();
            var temp = '';
            for(i=0;i<checkedValues.length;i++){
                temp = temp + checkedValues[i] + (i+1==checkedValues.length?'':',');
            }
            $("#btnPrintRange").prop('disabled',true);
            window.location.href = "/ProjectManagement/Reports/DailyReportsPrint?IdList=" + temp;
        });

        $(".btnLayerList").click(function () {
            if ($("#layerDetail").is(":visible")) {
                $("#layerDetail").hide("slide", { direction: "right" }, 300, function () {
                    $("#layerList").fadeIn(500);
                });
            } else {
                $("#layerRptForm").hide("slide", { direction: "right" }, 300, function () {
                    $("#layerList").fadeIn(500);
                });
                GenTable.ajax.reload();
            }
        });

        $('#selectAll').prop('checked', false);

        $('#selectAll').change(function () {
            if ($(this).prop('checked') == true)
                $('table#tb_DateList td input[type="checkbox"]').prop('checked', true).change();
            else
                $('table#tb_DateList td input[type="checkbox"]').prop('checked', false).change();
        });

    });
</script>
