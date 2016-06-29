@Code
    ViewData("Title") = "Laporan Lembur"
    Dim datedefaultOption = New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                            .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}}
    
    'Dim LastDayOfThisWeek = Date.Today
    'While LastDayOfThisWeek.DayOfWeek <> DayOfWeek.Friday
    '    LastDayOfThisWeek = LastDayOfThisWeek.AddDays(1)
    'End While
    Dim LastDayOfThisWeek = New Date(Date.Today.Year, Date.Today.Month, 25)
    
End Code
@Using Html.BeginJUIBox("Rekapitulasi Lembur")
    
    @<div>
        <div class="row">
            <div class="col-lg-12 col-sm-12">
                @Using Html.BeginForm("PrintOvertime", "OvertimeSummary", Nothing, FormMethod.Get, New With {.class = "form-horizontal", .id = "frmOvertimeView", .autocomplete = "off"})
                    @<div class="form-group">
                        <label class="col-lg-3 col-sm-4 control-label">
                            Tanggal</label>
                        <div class="col-lg-4 col-sm-6">
                            <div class="input-group date input-daterange">
                                <input type="text" class="form-control input-small" value="@LastDayOfThisWeek.AddMonths(-1).AddDays(1).ToString("dd-MM-yyyy")" id="dtStartDate" name="dtStartDate">
                                <span class="input-group-addon">s/d</span>
                                <input type="text" class="form-control input-small" value="@LastDayOfThisWeek.ToString("dd-MM-yyyy")" id="dtEndDate" name="dtEndDate">
                            </div>
                        </div>
                    </div>
                    @Html.WriteFormInput(Html.TextBox("EmployeeName", "", New With {.class = "form-control"}), "Nama")
                    @Html.WriteFormInput(Html.TextBox("OccupationName", "", New With {.class = "form-control read-only", .readonly = "readonly"}), "Jabatan")
                    @Html.Hidden("EmployeeId")
                    @Html.Hidden("OfficeId")

                    @<div class="well">
                        <div style="width: 100px;" class='center-block'>
                            <div class="btn-group">
                                <button class="btn btn-primary" type='button' id='btnView'>
                                    Lihat</button>
                                <button type="button" class="btn btn-danger dropdown-toggle" data-toggle="dropdown"
                                    aria-haspopup="true" aria-expanded="false">
                                    <span class="caret"></span><span class="sr-only">Toggle Dropdown</span>
                                </button>
                                <ul class="dropdown-menu">
                                    <li><a href="#" id="btnPrint">Print</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                
    End Using
            </div>
        </div>
    </div>
    
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="table-responsive">
                <table class="table table-bordered table-stripper table-hover" id="tblOvertime">
                    <colgroup>
                        <col style='width: 40px;'>
                        <col style='width: 120px;'>
                        <col style='width: auto;'>
                        <col style='width: 80px;'>
                        <col style='width: 80px;'>
                        <col style='width: 80px;'>
                        <col style='width: 120px;'>
                        <col style='width: 80px;'>
                        <col style='width: 80px;'>
                        <col style='width: 120px;'>
                    </colgroup>
                    <thead>
                        <tr>
                            <th rowspan='2'>
                                #
                            </th>
                            <th rowspan='2'>
                                Tanggal
                            </th>
                            <th rowspan='2'>
                                Kegiatan
                            </th>
                            <th colspan='4'>
                                Lembur I
                            </th>
                            <th colspan='3'>
                                Lembur II
                            </th>
                            <th rowspan='2'>
                                Total
                            </th>
                        </tr>
                        <tr>
                            <th>
                                05:00-08:00
                            </th>
                            <th>
                                17:00-24:00
                            </th>
                            <th>
                                Total Jam
                            </th>
                            <th>
                                Jumlah
                            </th>
                            <th>
                                00:00-05:00
                            </th>
                            <th>
                                Total Jam
                            </th>
                            <th>
                                Jumlah
                            </th>
                        </tr>
                    </thead>
                    <tfoot>
                        <tr>
                            <td colspan='5'>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td><td></td>
                        </tr>
                    </tfoot>
                </table>
            </div>
        </div>
    </div>
    
End Using
<style>
    .ui-autocomplete
    {
        height: 200px;
        overflow-y: scroll;
        overflow-x: hidden;
    }
</style>
<link href='@Url.Content("/plugins/bootstrap-datepicker/bootstrap-datepicker3.css")' rel='stylesheet'>
<script src="@Url.Content("/plugins/bootstrap-datepicker/bootstrap-datepicker.js")" type="text/javascript"></script>
<script src="@Url.Content("/plugins/bootstrap-datepicker/bootstrap-datepicker.id.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrapPaging.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/sum.js")" type="text/javascript"></script>
<script type="text/javascript">
    var tblOvertime = null;
    var dtformat = {
        "format": "dd-mm-yyyy",
        "autoclose": "true",
        "orientation": "top left",
        "todayBtn": "linked",
        "language": "id"

    }


    var _initAutocompletes = function () {


        //init autocomplete
        $('#EmployeeName').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/HRD/GlobalArray/AutoCompleteEmployeeForOffice',
                    data: {
                        term: $('#EmployeeName').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.OccupationName,
                                value: obj.Fullname,
                                id: obj.Id,
                                officeId: obj.OfficeId
                            }
                        }));
                    }
                });
            },
            change: function (event, ui) {
                tblOvertime.clear().draw();
                if (ui.item == null) {
                    $('#EmployeeId').val(0)
                    $('#EmployeeId').parent().addClass('has-error');
                    $("#OccupationName").val("");
                    $("#OfficeId").val("0");
                } else {
                    $('#EmployeeId').val(ui.item.id)
                    $('#EmployeeId').parent().removeClass('has-error');
                    $("#OccupationName").val(ui.item.label);
                    $("#OfficeId").val(ui.item.officeId);
                }
            }
        }).data('ui-autocomplete')._renderItem = function (ul, item) {
            //location
            return ($('<li>').append('<a><strong>' + item.value + '</strong>, <i>' +
            item.label + '</i></a>').appendTo(ul));
        };

        ; //end autocomplete


    }; //end initautoomplete

    var _initDTPicker = function () {
        $('.input-daterange').datepicker(dtformat);
    }


    var _loadData = function () {
        tblOvertime.clear().draw();
        var empId = parseInt($("#EmployeeId").val());
        if (isNaN(empId)) {
            return;
        }
        if (empId == 0) {
            return;
        }

        $.ajax({
            "url": "GetOvertimeListForEmploye",
            "type": "post",
            "data": { datestart: $("#dtStartDate").val(), dateend: $("#dtEndDate").val(), employeeId: $("#EmployeeId").val() },
            "success": function (d) {
                if (d.stat == 1) {
                    tblOvertime.rows.add(d.data).draw();
                    return;
                }
                showNotificationSaveError(d);
            }
        });
    }

    var _initTblOvertime = function () {

        var _isValidDate = function (obj) {
            var t = moment(obj);
            if (t == null || !t.isValid()) return "";
            return t.format("HH:mm");
        }
        var _render_Overtime1Morning = function (data, type, row) {
            if (type == 'display') {
                return (_isValidDate(row.BeginOvertime1Morning) + "-" + _isValidDate(row.EndOvertime1Morning));
            }
            return data;
        }
        var _render_Overtime1Afternoon = function (data, type, row) {
            if (type == 'display') {
                return (_isValidDate(row.BeginOvertime1Afternoon) + "-" + _isValidDate(row.EndOvertime1Afternoon));
            }
            return data;
        }
        var _render_Overtime2 = function (data, type, row) {
            if (type == 'display') {
                return (_isValidDate(row.BeginOvertime2) + "-" + _isValidDate(row.EndOvertime2));
            }
            return data;
        }
        var _renderDate = function (data, type, row) {
            if (type == 'display') {
                return moment(data).format("DD-MM-YYYY");
            }
            return data;
        }
        var _renderSubTotal = function (data, type, row) {
            data = row.Overtime1Total + row.Overtime2Total;
            if (type == 'display') {
                if (data > 0) {
                    return $.number(data, 2, ',', ' ');
                }
                return "";
            }
            return data;
        }
        var _fnRender1DigitDecimal = function (data, type, row) {
            if (type == 'display') {
                if (data > 0) {
                    return $.number(data, 1, ',', ' ');
                }
                return "";

            }
            return data;
        }
        var _fnRender2DigitDecimal = function (data, type, row) {
            if (type == 'display') {
                if (data > 0) {
                    return $.number(data, 2, ',', ' ');
                }
                return "";

            }
            return data;
        }
        var arrColumns = [
            { "data": "Id", "sClass": "text-right" },
            { "data": "ActivityDate", mRender: _renderDate, "sClass": "text-center" },
            { "data": "Activity" },
            { "data": "BeginOvertime1Morning", "mRender": _render_Overtime1Morning, "sClass": "text-center" },
            { "data": "BeginOvertime1Afternoon", "mRender": _render_Overtime1Afternoon, "sClass": "text-center" },
            { "data": "TotalOvertime1", "sClass": "text-right", "mRender": _fnRender1DigitDecimal },
            { "data": "Overtime1Total", "sClass": "text-right", "mRender": _fnRender2DigitDecimal },
            { "data": "BeginOvertime2", "mRender": _render_Overtime2, "sClass": "text-center" },
            { "data": "TotalOvertime2", "sClass": "text-right", "mRender": _fnRender1DigitDecimal },
            { "data": "Overtime2Total", "sClass": "text-right", "mRender": _fnRender2DigitDecimal },
            { "data": "Overtime2Total", "sClass": "text-right", "mRender": _renderSubTotal }

        ];
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.bAutoWidth = false;
        datatableDefaultOptions.fnDrawCallback = function (settings) {
            _fnlocalDrawCallback(settings);
            //calculate total
            var api = this.api();
            var grandtotal = 0;
            var total1 = api.column(5, { page: 'current' }).data().sum();
            
            total1 = $.number(total1, 1, ",", ".");

            var total11 = api.column(6, { page: 'current' }).data().sum();
            grandtotal += total11
            total11 = $.number(total11, 2, ",", ".");

            var total2 = api.column(8, { page: 'current' }).data().sum();
            total2 = $.number(total2, 1, ",", ".");

            var total22 = api.column(9, { page: 'current' }).data().sum();
            grandtotal += total22;
            total22 = $.number(total22, 2, ",", ".");
             

            grandtotal = $.number(grandtotal, 2, ",", ".");

            $(api.table().footer()).find("tr:first td").eq(1).html(total1);
            $(api.table().footer()).find("tr:first td").eq(2).html(total11);
            $(api.table().footer()).find("tr:first td").eq(4).html(total2);
            $(api.table().footer()).find("tr:first td").eq(5).html(total22);
            $(api.table().footer()).find("tr:first td").eq(6).html(grandtotal);
        }
        tblOvertime = $("#tblOvertime").DataTable(datatableDefaultOptions);
    };                      //end init tblOvertime

    $(function () {

        _initDTPicker();
        _initAutocompletes();
        _initTblOvertime();

        $("#btnView").click(function () {
            _loadData();

        }); //btnview click
        $("#btnPrint").click(function (e) {
            e.preventDefault();
            $("#frmOvertimeView").submit();

        }); //btnprintclick
    });
</script>
