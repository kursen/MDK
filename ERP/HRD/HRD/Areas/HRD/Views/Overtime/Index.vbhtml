@Code
    ViewData("Title") = "Daftar Lembur"
    Dim datedefaultOption = New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}}
End Code
@Using Html.BeginJUIBox("Lembur")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="pull-right">
                <div class="btn-group">
                    <button class="btn btn-primary" id="btnNewOvertimeRecord">
                        Menambah Data Lembur</button>
                </div>
            </div>
        </div>
    </div>
    
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            @Using Html.BeginForm("", "Overtime", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "off"})
                @<div class="form-group">
                    <label class="col-lg-3 col-sm-4 control-label">
                        Tanggal</label>
                    <div class="col-lg-3 col-sm-4">
                        <div class="input-group">
                            @Html.DateInput("Overtimedate", Date.Today, datedefaultOption)
                            <div class="input-group-btn">
                                <button type="button" class="btn btn-primary" id="btnRefresh">
                                    <span class="fa fa-refresh"></span>
                                </button>
                                <button type="button" class="btn btn-primary" id="btnPrevDay">
                                    <span class="fa fa-arrow-left"></span>
                                </button>
                                <button type="button" class="btn btn-primary" id="btnNextDay">
                                    <span class="fa fa-arrow-right"></span>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
         
    End Using
        </div>
    </div>
    @<div class="row">
        <div class="col-sm-12 col-lg-12">
            <div class="table-responsive">
                <table class="table table-bordered" id="tblOvertime">
                    <colgroup>
                        <col style='width: 40px;'>
                        <col style='width: 220px;'>
                        <col style='width: auto;'>
                        <col style='width: 80px;'>
                        <col style='width: 80px;'>
                        <col style='width: 80px;'>
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
                                Nama
                            </th>
                            <th rowspan='2'>
                                Kegiatan
                            </th>
                            <th colspan='3'>
                                Lembur I
                            </th>
                            <th colspan='2'>
                                Lembur II
                            </th>
                            <th rowspan='2'></th>
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
                                00:00-05:00
                            </th>
                            <th>
                                Total Jam
                            </th>
                            
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>
    
End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrapPaging.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">

    var tblOvertime = null;
    var _getDataTableRow = function (obj) {
        return tblOvertime.row($(obj).closest('tr'));
    }
    var initTblOvertime = function () {
        var _lnkDelete_click = function (e) {
            e.preventDefault();
            if (confirm("Hapus dokumen ini?") == false) {
                return;
            }
            var row = _getDataTableRow(this)
            $.ajax({
                url: '/HRD/Overtime/Delete',
                type: 'POST',
                data: { id: row.data().Id },
                success: function () {
                    tblOvertime.ajax.reload();
                },
                error: ajax_error_callback,
                datatype: 'json'
            });

        }
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
        var _renderAction = function (data, type, row) {
            if (type == 'display') {
                var htmls = new Array();
                htmls.push('<div class="btn-group">');
                htmls.push("<a role='button' data-target='#' href='/HRD/Overtime/Edit/" + data +
                "' title='Edit' class='btn btn-primary'>Edit</a>");
                htmls.push('<button type="button" class="btn btn-danger dropdown-toggle" ');
                htmls.push('data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">');
                htmls.push('<span class="caret"></span>');
                htmls.push('<span class="sr-only">Action</span>');
                htmls.push('</button>');
                htmls.push('<ul class="dropdown-menu text-left pull-right">');
                htmls.push('<li><a href="#" class="lnkDelete">Hapus</a></li>');
                htmls.push('</ul>');
                htmls.push('</div>');
                return htmls.join("\n");
            }
            return data;
        }
        var arrColumns = [
            { "data": "Id" },
            { "data": "EmployeeName" },
            { "data": "Activity" },
            { "data": "BeginOvertime1Morning", "mRender": _render_Overtime1Morning, "sClass": "text-center" },
            { "data": "BeginOvertime1Afternoon", "mRender": _render_Overtime1Afternoon, "sClass": "text-center" },
            { "data": "TotalOvertime1", "sClass": "text-center" },
            { "data": "BeginOvertime2", "mRender": _render_Overtime2, "sClass": "text-center" },
            { "data": "TotalOvertime2", "sClass": "text-center" },
            { "data": "Id", "mRender": _renderAction, "sClass": "text-center" }

        ];
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.bAutoWidth = false
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            _fnlocalDrawCallback(oSettings);
            $(".lnkDelete").click(_lnkDelete_click);

        }
        datatableDefaultOptions.ajax = function (data, callback, settings) {
            $.ajax({
                "url": "GetOvertimeList",
                "type": "post",
                "data": { overtimedate: $("#Overtimedate").val() },
                "success": callback
            });
        };
        tblOvertime = $("#tblOvertime").DataTable(datatableDefaultOptions);
    };//end init tblOvertime

    $(function () {


        $("#btnNextDay").click(function () {
            var today = $("#dtpk_Overtimedate").datepicker("getDate");
            $("#dtpk_Overtimedate").datepicker("update", moment(today).add(1, "days").format("DD-MM-YYYY"));
            tblOvertime.ajax.reload();
        });

        $("#btnPrevDay").click(function () {
            var today = $("#dtpk_Overtimedate").datepicker("getDate");
            $("#dtpk_Overtimedate").datepicker("update", moment(today).subtract(1, "days").format("DD-MM-YYYY"));
            tblOvertime.ajax.reload();

        });

        $("#btnRefresh").click(function () {
            tblOvertime.ajax.reload();
        });

        $("#btnNewOvertimeRecord").click(function () {
            window.location = "/HRD/Overtime/Create";
        });
        initTblOvertime();
    });           ///init


</script>
