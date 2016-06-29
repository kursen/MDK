@Code
    ViewData("Title") = "Perawatan Kendaraan"
  
    
    
End Code
@Functions
    Function WriteSpanControl(ByVal value As String, ByVal id As String) As MvcHtmlString
        Return New MvcHtmlString("<span class='form-control-static' id='" & id & "'>" & value & "</span>")
    End Function
    
End Functions
@Using Html.BeginJUIBox("Perawatan Kendaraan")

    

    
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <table class="table table-bordered" id="tblServiceSchedule">
                <thead>
                    <tr>
                        <th style="width: 60px">
                            #
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
                        <th style="width: 180px">
                            Status
                        </th>
                        <th></th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
End Using
<div class="modal fade" tabindex="-1" role="dialog" id="dlgDetail">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header bg-primary">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title">
                    Item Perawatan</h4>
            </div>
            <div class="modal-body">
                <div class="form-horizontal">
                    @Html.WriteFormInput(WriteSpanControl("", "sp_Code"), "Kode Kendaraan", lgLabelWidth:=4, lgControlWidth:=8)
                    @Html.WriteFormInput(WriteSpanControl("", "sp_PoliceNumber"), "No Polisi", lgLabelWidth:=4, lgControlWidth:=8)
                    @Html.WriteFormInput(WriteSpanControl("", "sp_CategoryMerkType"), "Kategory/Merek/Tipe", lgLabelWidth:=4, lgControlWidth:=8)
                    <input type='hidden' id='hVehicleCode' value='0' />
                    <br />
                    <br />
                    <table class="table table-bordered" id='tblItems'>
                        <colgroup>
                            <col style="width: 60px" />
                            <col style="width: auto" />
                        </colgroup>
                        <thead>
                            <tr>
                                <th>
                                    #
                                </th>
                                <th>
                                    Poin
                                </th>
                                <th>
                                    Perawatan Terakhir
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div class="modal-footer bg-primary">
                <button type="button" class="btn btn-default" data-dismiss="modal">
                    Tutup</button>
                <button type="button" class="btn btn-primary" id='btnDoMaintenance'>
                    Lakukan Perawatan</button>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
<!-- /.modal -->
<style>
    #dlgDetail .form-group
    {
        margin-bottom: 0px;
    }
    #YearSelector
    {
        padding-right: 20px;
    }
</style>
<link href="../../../../plugins/datatables/dataTables.bootstrap.css" rel="stylesheet"
    type="text/css" />
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="../../../../plugins/datatables/dataTables.bootstrapPaging.js" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">

    var tblServiceSchedule = null;
    var tblItems = null;
    var _loadData = function () {

        var _url = "/Equipment/VehicleMaintenance/GetProposedVehicleToMaintainList";
        $.ajax({
            type: 'POST',
            url: _url,
            dataType: 'json',
            success: function (data) {

                tblServiceSchedule.clear();
                tblServiceSchedule.rows.add(data.data).draw();
            },
            error: ajax_error_callback
        });
    }
    var _initTblServiceSchedule = function () {
        var _renderDetail = function (data, type, row) {
            if (type == 'display') {
                var htmls = new Array();
                htmls.push('<div class="btn-group">');
                htmls.push("<a role='button' data-target='#' href='#' title='Lihat Item' class='btn btn-primary viewitem'>Item Perawatan</a>");
                htmls.push('<button type="button" class="btn btn-danger dropdown-toggle" ');
                htmls.push('data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">');
                htmls.push('<span class="caret"></span>');
                htmls.push('<span class="sr-only">Action</span>');
                htmls.push('</button>');
                htmls.push('<ul class="dropdown-menu text-left pull-right">');
                htmls.push('<li><a href="#" class="lnkPerawatan">Lakukan Perawatan</a></li>');
                htmls.push('<li><a href="#" class="lnkArchive">Riwayat Perawatan</a></li>');
                htmls.push('</ul>');
                htmls.push('</div>');


                return htmls.join("\n");
            }
            return data;
        }

        var arrColumns = [
              { "data": "VehicleId", "sClass": "text-right" },
              { "data": "Code" },
              { "data": "PoliceNumber" },
              { "data": "Merk" },
              { "data": "Species" },
              { "data": "VehicleId", "sClass": "text-center", "mRender": _renderDetail }
        ];

        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [{ "targets": [0, 5], "orderable": false}];
        datatableDefaultOptions.order = [[1, "asc"]];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = true;
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            _fnlocalDrawCallback(oSettings);

            //show / hide pagination
            var wrapper = this.parent();
            var rowsPerPage = this.fnSettings()._iDisplayLength;
            console.log(wrapper);
            var rowsToShow = this.fnSettings().fnRecordsDisplay();


            if (rowsToShow <= rowsPerPage || rowsPerPage == -1) {
                $('#tblItems_paginate').css('visibility', 'hidden');

            }
            else {
                $('#tblItems_paginate').css('visibility', 'visible');
            }



            $(".viewitem").click(_lnkViewItem_click);
            $(".lnkPerawatan").click(_lnkDoMaintenance_click);
        }
        tblServiceSchedule = $('#tblServiceSchedule').DataTable(datatableDefaultOptions)

    }

    var _initTblItems = function () {
        var arrc = [
              { "data": "VehicleId", "sClass": "text-right" },
              { "data": "ItemName" },
              { "data": "LastCheck_Date", "mRender": _fnRenderNetDate, "sClass": "text-center" }
        ];
        var datatableDefaultOptions1 = new Object();
        datatableDefaultOptions1.searching = false;
        datatableDefaultOptions1.bLengthChange = false;
        datatableDefaultOptions1.aoColumns = arrc;
        datatableDefaultOptions1.columnDefs = null; // [{ "targets": [0, 1], "orderable": false}];
        datatableDefaultOptions1.order = [[0, "asc"]];
        datatableDefaultOptions1.autoWidth = false;
        datatableDefaultOptions1.ordering = true;
        datatableDefaultOptions1.info = false;
        datatableDefaultOptions1.fnDrawCallback = _fnlocalDrawCallback;
        datatableDefaultOptions1.sDom = "<'row'<'col-lg-12 col-sm-12'<'pull-right'f>>><'row'<'col-sm-12'tr>><'row'<'col-sm-5'i><'col-sm-7'p>>";
        tblItems = $('#tblItems').DataTable(datatableDefaultOptions1)
    }

    var _lnkViewItem_click = function (e) {

        var row = _getDataTableRow(this);
        $("#sp_Code").text(row.data().Code);
        $("#sp_PoliceNumber").text(row.data().PoliceNumber);
        var arr = [row.data().Species, row.data().Merk, row.data().Type];
        $("#sp_CategoryMerkType").text(arr.join(" / "));
        $("#hVehicleCode").val(row.data().VehicleId);
        tblItems.clear();
        tblItems.rows.add(row.data().Vehicle).draw();

        $("#dlgDetail").modal();
        return false;
    }

    var _lnkDoMaintenance_click = function (e) {

        e.preventDefault();
        var row = _getDataTableRow(this);
        OpenMaintenanceForm(row.data().VehicleId);
    }

    var _getDataTableRow = function (obj) {
        return tblServiceSchedule.row($(obj).closest('tr'));
    }

    var OpenMaintenanceForm = function (vehicleCode) {
       
        window.location = "/Equipment/VehicleMaintenance/CreateMaintenanceRecord/" + vehicleCode;
    };

    $(function () {
        $("#dlgDetail").appendTo("body");
        $(".itemDetail").click(function () {


        });

        $("#btnView").click(function () {
            _loadData();
        });

        $("#btnDoMaintenance").click(function () {
            var vehicleCode = $("#hVehicleCode").val();
            OpenMaintenanceForm(vehicleCode);
        });



        _initTblServiceSchedule();
        _initTblItems();
        _loadData();
        $("#YearSelector").spinner();
    });

</script>
