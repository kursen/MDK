@Code
    ViewData("Title") = "Perawatan Kendaraan"
    Dim arrMonth = {"Januari", "Februari", "Maret", "April", "Mei", "Juni",
                    "Juli", "Agustus", "September", "Oktober", "Nopember", "Desember"}
    Dim lsMonth As New List(Of OptionItem)
    For iMonth As Integer = 1 To 12
        lsMonth.Add(New OptionItem With {.Text = arrMonth(iMonth - 1), .Value = iMonth})
    Next
    
    Dim MonthSelector = New SelectList(lsMonth, "Value", "Text", Date.Today.Month - 1)
    
    
End Code
@Using Html.BeginJUIBox("Perawatan Kendaraan")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="btn-group pull-right">
            @Html.ActionLink("Tambah Data", "CreateMaintenanceRecord", "VehicleMaintenance", Nothing, New With {.class = "btn btn-primary"})
                
            </div>
        </div>
    </div>
    
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
        @Using Html.BeginForm("GetMaintenanceList", "VehicleMaintenance", Nothing, FormMethod.Post, New With {.class = "form-horizontal",
                                                                                                          .id = "frmFilter", .autocomplete = "off"})
            
                @Html.WriteFormInput(Html.DropDownList("MonthSelector", MonthSelector, New With {.class = "form-control"}), "Bulan")
                @Html.WriteFormInput(Html.YearInput("YearSelector", Date.Today.Year), "Tahun", lgControlWidth:=1, smControlWidth:=1)
                @Html.WriteFormInput(New MvcHtmlString("<button id='btnView' type='submit' class='btn btn-primary'>Lihat</buton>"), "")
            
            End Using
        
            
        </div>
    </div>
    
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
        <div class='table-responsive'>
       
            <table class="table table-bordered" id="tblServiceSchedule">
                <thead>
                    <tr>
                        <th style="width: 60px">
                            #
                        </th>
                        <th style="width: 100px">
                            Tanggal
                        </th>
                        <th style="width: 100px">
                            Kode
                        </th>
                        <th style="width: 150px">
                            No. Polisi
                        </th>
                        <th>
                            Merk/Type
                        </th>
                        <th style="width: 200px">
                            Jenis
                        </th>
                        <th style="width: 140px">
                            Status
                        </th>
                        <th style="width: 180px"></th>
                    </tr>
                </thead>
                
            </table>
 </div>
        </div>
    </div>
End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type='text/javascript'>
    var tblServiceSchedule = null;



    var _initTblData = function () {

        var _localLoad = function (data, callback, setting) {
            var _month = $("#MonthSelector").val();
            var _year = $("#YearSelector").val();
            var _url = $("#frmFilter").attr("action");
            var _data = { selectedMonth: _month, selectedYear: _year };
            $.ajax({
                url: _url,
                data: _data,
                type: 'POST',
                success: callback,
                error: ajax_error_callback,
                datatype: 'json'
            });
        }
        var _renderDetail = function (data, type, row) {
            if (type == 'display') {
                var htmls = new Array();
                htmls.push('<div class="btn-group">');
                htmls.push("<a role='button' data-target='#' href='#' title='Lihat Item' class='btn btn-primary lnkViewItem'>Detail</a>");
                htmls.push('<button type="button" class="btn btn-danger dropdown-toggle" ');
                htmls.push('data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">');
                htmls.push('<span class="caret"></span>');
                htmls.push('<span class="sr-only">Action</span>');
                htmls.push('</button>');
                htmls.push('<ul class="dropdown-menu text-left pull-right">');
                htmls.push('<li><a href="#" class="lnkEditItem">Edit</a></li>');
                htmls.push('<li><a href="#" class="lnkDeleteItem">Hapus</a></li>');
                htmls.push('</ul>');
                htmls.push('</div>');


                return htmls.join("\n");
            }
            return data;
        }
        var _renderStatus = function (data, type, row) {
            if (type == 'display') {
                var status = "";
                switch (data) {
                    case 0: status = "Menunggu"; break;
                    case 1: status = "Sedang dikerjakan"; break;
                    default:
                        status = "Selesai";
                }
                return status;
            }
            return data;
        }

        var _lnk_View = function (e) {
            e.preventDefault();
            var data = _getDataTableRow(this).data();
            window.location = "/Equipment/VehicleMaintenance/Detail/" + data.Id;

        }
        var _lnk_Edit = function (e) {
            e.preventDefault();
            var data = _getDataTableRow(this).data();
            window.location = "/Equipment/VehicleMaintenance/Edit/" + data.Id;
        }
        var _lnk_Delete = function (e) {
            e.preventDefault();
            var data = _getDataTableRow(this).data();
            var _url = "/Equipment/VehicleMaintenance/Delete";

            if (confirm("Hapus data ini?")) {
                $.ajax({
                    url: _url,
                    data: { id: data.Id },
                    type: 'POST',
                    success: function (get) {
                        if (get.stat == 1) {
                            console.log(get.stat);
                            tblServiceSchedule.ajax.reload();
                            showNotification("Data Telah terhapus!");
                        }
                    },
                    error: ajax_error_callback,
                    datatype: 'json'
                });
            }
        }
        var arrColumns = [
            { "data": "Id", "sClass": "text-right" }, //
            {"data": "MaintenanceDateStart", "sClass": "text-center", "mRender": _fnRenderNetDate }, //
              {"data": "Code", "sClass": "text-center" }, //
              {"data": "PoliceNumber" }, //
              {"data": "Merk" }, //
              {"data": "Species" }, //
              {"data": "MaintenanceState", "sClass": "text-center", "mRender": _renderStatus }, //
              {"data": "Id", "sClass": "text-center", "mRender": _renderDetail}//
        ];
        var _coldefs = [];
        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [];
        datatableDefaultOptions.ajax = _localLoad;
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = true;
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            _fnlocalDrawCallback(oSettings);
            $(".lnkViewItem").click(_lnk_View);
            $(".lnkEditItem").click(_lnk_Edit);
            $(".lnkDeleteItem").click(_lnk_Delete);
        }
        tblServiceSchedule = $("#tblServiceSchedule").DataTable(datatableDefaultOptions);


    }               //end _initTblData;


 
    var _getDataTableRow = function (obj) {
        return tblServiceSchedule.row($(obj).closest('tr'));
    }

    $(function () {

        _initTblData();
        $("#frmFilter").submit(function (e) {
            e.preventDefault();
            tblServiceSchedule.ajax.reload();

        });
    });    //end init


</script>