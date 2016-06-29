@Code
    ViewData("Title") = "Dashboard"
    
    Dim _eqpEntities = New Equipment.EquipmentEntities
    
    Dim _mainEntites = New ERPBase.MainEntities
    
    Dim WorkUnit = _mainEntites.Offices.ToLookup(Of Integer)(Function(m) m.Id)
    
    
    
    Dim HeavyEquipments = From m In _eqpEntities.HeavyEqps
                   Group By m.IDArea Into G = Group
                   Select New With {G, IDArea}
                   
    
    Dim Vehicles = From m In _eqpEntities.Vehicles
                    Group By m.IDArea Into G = Group
                    Select New With {G, IDArea}
    
    
End Code
<style>
    .badge
    {
        font-size: larger;
        background: #6AA6D6 none repeat scroll 0% 0% !important;
        color: #F8F8F8 !important;
    }
</style>
<div class="row">
    <div class="col-lg-4 col-sm-5">
        @Using Html.BeginJUIBox("Armada Per Unit Kerja")
            @<div class="panel panel-info">
                <div class="panel-heading">
                    <h4>
                        Alat Berat</h4>
                </div>
                <ul class="list-group">
                    @For Each item In HeavyEquipments
                Dim w = WorkUnit(item.IDArea).First.Name
            
                        @<li class="list-group-item"><span class="badge bagde-info ">@item.G.Count</span> @w</li>
            Next
                </ul>
            </div>
      
        
            @<div class="panel panel-info">
                <div class="panel-heading">
                    <h4>
                        Kendaraan</h4>
                </div>
                <ul class="list-group">
                    @For Each item In Vehicles
                Dim w = WorkUnit(item.IDArea).First.Name
            
                        @<li class="list-group-item"><span class="badge ">@item.G.Count</span> @w</li>
            Next
                </ul>
            </div>
        End Using
    </div>
    <div class="col-lg-8 col-sm-7">
        @Using Html.BeginJUIBox("Perawatan")
            @<ul id='maintainlistproposal'>
                
            </ul>
        End Using
        @Using Html.BeginJUIBox("Kelengkapan Laporan ")
            Dim tabItems = {"Kendaraan", "Alat Berat", "Trado"}
    
            @<nav>
                <ul class="pager">
                    <li class="previous"><a id="prevWeek" href="#"><span aria-hidden="true">&larr;</span>
                        Minggu Lalu</a></li>
                    <li><span id="sp_firstDayOfWeek"></span>S/D <span id="sp_lastDayOfWeek"></span></li>
                    <li class="next"><a id="nextWeek" href="#">Minggu Berikutnya <span aria-hidden="true">
                        &rarr;</span></a></li>
                </ul>
            </nav>
            @<ul class="nav nav-tabs" role="tablist" id="tabsReport">
                @For Each item In tabItems
                    @<li role="presentation"><a  href="#@item.Replace(" "c, String.Empty)" aria-controls="@item.Replace(" "c, String.Empty)" role="tab"
                    data-toggle="tab">@item</a></li>
            Next
            </ul>
            @<div class="tab-content">
                @For Each item In tabItems
                    @<div role="tabpanel" class="tab-pane" id="@item.Replace(" "c, String.Empty)">
                        <div class="table-responsive">
                            <table class="table table-bordered"  id="tbl@(item.Replace(" "c, String.Empty))">
                                <colgroup>
                                    <col style="width: 60px" />
                                    <col style="width: 100px" />
                                    <col style="width: auto" />
                                    <col style="width: 50px" />
                                    <col style="width: 50px" />
                                    <col style="width: 50px" />
                                    <col style="width: 50px" />
                                    <col style="width: 50px" />
                                    <col style="width: 50px" />
                                    <col style="width: 50px" />
                                </colgroup>
                                <thead class="bg-primary">
                                    <tr>
                                        <th>
                                            #
                                        </th>
                                        <th>
                                            Kode
                                        </th>
                                        <th>
                                            Jenis
                                        </th>
                                        <th>
                                            Mg<br />
                                            <span class="spDay1"></span>
                                        </th>
                                        <th>
                                            Sn<br />
                                            <span class="spDay2"></span>
                                        </th>
                                        <th>
                                            Sl<br />
                                            <span class="spDay3"></span>
                                        </th>
                                        <th>
                                            Rb<br />
                                            <span class="spDay4"></span>
                                        </th>
                                        <th>
                                            Km<br />
                                            <span class="spDay5"></span>
                                        </th>
                                        <th>
                                            Jm<br />
                                            <span class="spDay6"></span>
                                        </th>
                                        <th>
                                            Sb<br />
                                            <span class="spDay7"></span>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
            Next
            </div>
         
        End Using
    </div>
</div>
<style>
    #sp_firstDayOfWeek, #sp_lastDayOfWeek
    {
        border: none;
        border-radius: 0;
        background: none;
        font-weight: bold;
    }
</style>
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">
    var tblKendaraan = null, tblAlatBerat = null, tblTrado = null;
    var _renderCheckedColumn = function (data, type, row) {
        if (type == "display") {
            if (data == 1)
                return '<span class="fa fa-check"></span>';
            else
                return "";
        }
        return data;
    };
    var _initTables = function () {


        var arrColumns = [
            { "data": "Code", "sClass": "text-right" }, //0
            {"data": "Code" }, //0
            {"data": "ItemName" }, //1
            {"data": "H1", "sClass": "text-center", "mRender": _renderCheckedColumn }, //2
            {"data": "H2", "sClass": "text-center", "mRender": _renderCheckedColumn }, //3
            {"data": "H3", "sClass": "text-center", "mRender": _renderCheckedColumn }, //4
            {"data": "H4", "sClass": "text-center", "mRender": _renderCheckedColumn }, //5
            {"data": "H5", "sClass": "text-center", "mRender": _renderCheckedColumn }, //6
            {"data": "H6", "sClass": "text-center", "mRender": _renderCheckedColumn }, //7
            {"data": "H7", "sClass": "text-center", "mRender": _renderCheckedColumn}//8
        ];

        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [{ "targets": [0], "orderable": false}];
        datatableDefaultOptions.order = [[1, "asc"]];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = true;

        tblKendaraan = $("#tblKendaraan").DataTable(datatableDefaultOptions)
        tblAlatBerat = $("#tblAlatBerat").DataTable(datatableDefaultOptions)
        tblTrado = $("#tblTrado").DataTable(datatableDefaultOptions)

    };  //_initTables


    var _getCompletionOfReports = function (datestring) {


        $.ajax({
            url: '/Equipment/Home/CompletionUsageReports',
            data: { sundayDate: datestring },
            type: 'POST',
            success: function (data) {
                $("#nextWeek").data("datedata", data.nextWeekDay);
                $("#prevWeek").data("datedata", data.prevWeekDay);

                $("#sp_firstDayOfWeek").text(data.thisWeekDayFirst);
                $("#sp_lastDayOfWeek").text(data.thisWeekDayLast);


                var m = moment(data.thisWeekDayFirst, "DD-MM-YYYY");

                for (var i = 0; i < 7; i++) {
                    $(".spDay" + (i + 1)).text(m.format('DD'));
                    m.add(1, 'days');
                }

                tblKendaraan.clear();
                tblKendaraan.rows.add(data.VehicleList).draw();
                tblAlatBerat.clear();
                tblAlatBerat.rows.add(data.HeavyEquipmentList).draw();
                tblTrado.clear();
                tblTrado.rows.add(data.TradoList).draw();

            },
            error: ajax_error_callback,
            datatype: 'json'
        });

    }                       //getcompletionreport

    var _getUnitVehicleToMaintain = function () {

        $.ajax({
            url: '/Equipment/Home/GetNumberOfVehicleNeedToMaintain',
            type: 'POST',
            success: function (data) {
                if (data.NumberVehicleToMaintain > 0) {
                    var li = $("<li></li>").appendTo($("#maintainlistproposal"));
                    $(li).append("<a href='/Equipment/VehicleMaintenance/ProposedMaintaenanceList' >" +
                data.NumberVehicleToMaintain + " kendaraan memerlukan perawatan</a>");
                }
            }
        });
        $.ajax({
            url: '/Equipment/Home/GetNumberOfHeavyEqpNeedToMaintain',
            type: 'POST',
            success: function (data) {
                if (data.NumberHeavyEqpToMaintain > 0) {
                    var li = $("<li></li>").appendTo($("#maintainlistproposal"));
                    $(li).append("<a href='/Equipment/HeavyEquipmentMaintenance/ProposedMaintenanceList' >" +
                data.NumberHeavyEqpToMaintain + " alat berat memerlukan perawatan</a>");
                }

            }
        });
        //mesin
        $.ajax({
            url: '/Equipment/Home/GetNumberOfMachineNeedToMaintain',
            type: 'POST',
            success: function (data) {
                if (data.NumberMachineToMaintain > 0) {
                    var li = $("<li></li>").appendTo($("#maintainlistproposal"));
                    $(li).append("<a href='/Equipment/MachineMaintenance/ProposedMaintenanceList' >" +
                data.NumberMachineToMaintain + " mesin memerlukan perawatan</a>");
                }

            }
        });
    }

    $(function () {

        _getUnitVehicleToMaintain();

        $('#tabsReport a:first').tab('show') // Select first tab
        _initTables();


        _getCompletionOfReports($("#sp_firstDayOfWeek").text());

        $("#prevWeek, #nextWeek").click(function () {

            _getCompletionOfReports($(this).data("datedata"));
            return false;

        });
    });
</script>
