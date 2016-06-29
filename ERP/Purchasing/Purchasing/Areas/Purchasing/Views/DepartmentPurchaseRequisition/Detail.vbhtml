@ModelType Purchasing.DepartmentPurchaseRequisition
@Code
    ViewData("Title") = "Detail"

    
End Code
@Functions
    Function WriteSpanControl(ByVal value As String, ByVal id As String) As MvcHtmlString
        Return New MvcHtmlString("<p class='form-control-static'>" & value & "</p>")
    End Function
End Functions
<div class="row">
    <div class="col-lg-12 col-sm-12">
        <div class="pull-right">
            <div class="button-group">
            @If Model.DocState <= 1 Then
                @<a href="@Url.Action("Edit", "DepartmentPurchaseRequisition", New With {.id = Model.ID})" class="btn btn-danger btn-label-left">
                    <span><i class="fa fa-edit"></i></span>Edit</a> 
            End If
                
                     <a href="@Url.Action("Index", "DepartmentPurchaseRequisition")" class="btn btn-danger btn-label-left">
                    <span><i class="fa fa-arrow-left"></i></span>Daftar Permintaan</a>
                     <a href="@Url.Action("PrintPR", "DepartmentPurchaseRequisition", New With {.id = Model.ID})" class="btn btn-danger btn-label-left">
                            <span><i class="fa fa-print"></i></span>Print</a>
                <button type="button" class="btn btn-danger btn-label-left" id="btnDeleteDocument">
                    <span><i class="fa fa-trash"></i></span>Hapus</button>
            </div>
        </div>
    </div>
</div>
@Using Html.BeginJUIBox("Detail Permintaan")
    
    @<input type="hidden" id="documentId" value="@Model.Id" />
    @<div class="row">
        <div class="col-sm-12 col-lg-12">
            <div class="form-horizontal" id="documentView">
                <input type="hidden" id="documentid" value ="@Model.Id" />
                @Html.WriteFormInput(WriteSpanControl(Model.RequestDate.ToString("dd-MM-yyyy"), "spRequestDate"), "Tanggal")
                @Html.WriteFormInput(WriteSpanControl(Model.RecordNo, "spRecordNo"), "No. Dokumen")
                @Html.WriteFormInput(WriteSpanControl(Model.RequestType.Name, "spRecordNo"), "Tipe Permintaan")
                @Html.WriteFormInput(WriteSpanControl(Model.DeliveryDate.ToString("dd-MM-yyyy"), "spDeliveryDate"), "Tanggal Kebutuhan")
                @Html.WriteFormInput(WriteSpanControl(Model.DeliveryTo, "spDeliveryTo"), "Dikirimkan Ke")
                @Html.WriteFormInput(WriteSpanControl(Model.DeliveryAddress, "spDeliveryAddress"), "Alamat")
                @Html.WriteFormInput(WriteSpanControl(Model.RequestedBy_Name, "spRequestedBy_Name"), "Diajukan Oleh")
                @Html.WriteFormInput(WriteSpanControl(Model.RequestedBy_Occupation, "spRequestedBy_Occupation"), "Jabatan")
            </div>
        </div>
    </div>
    @<div style="margin-top: 30px">
    </div>
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    DAFTAR ITEM PERMINTAAAN</div>
                <div class="table-responsive">
                    <table class="table table-bordered table-striped table-hover table-heading table-datatable dataTable"
                        id="tblItems">
                        <colgroup>
                            <col style="width: 60px" />
                            <col style="width: auto" />
                            <col style="width: 400" />
                            <col style="width: 60" />
                            <col style="width: 60" />
                            <col style="width: 60" />
                            <col style="width: 60" />
                            <col style="width: 60" />
                            <col style="width: 60" />
                            <col style="width: 120" />
                        </colgroup>
                        <thead>
                            <tr>
                                <th>
                                    No.
                                </th>
                                <th style="width: 300px">
                                    Nama Item
                                </th>
                                <th>
                                   Alokasi
                                </th>
                                <th colspan="2">
                                    Jumlah
                                </th>
                                <th colspan="2">
                                    Perk. Harga
                                </th>
                                <th colspan="2">
                                    Total Harga
                                </th>
                                <th>
                                    Catatan
                                </th>
                            </tr>
                        </thead>
                        <tfoot>
                            <tr style="border-top: 1px solid #000;">
                                <td>
                                </td>
                                <td colspan="7">
                                    TOTAL
                                </td>
                                <td class="text-right">
                                    0,00
                                </td>
                                <td>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-9 col-sm-8">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Persetujuan
                </div>
                <div class="table-responsive">
                    <table class="table table-bordered table-stripped table-heading dataTable">
                        <thead>
                            <tr>
                                <th style="width: 30%">
                                    
                                </th>
                                <th style="width: 30%">
                                    Nama
                                </th>
                                <th style="width: 30%">
                                    Jabatan
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Disetujui oleh</td>
                                <td>@Model.ApprovedBy_Name</td>
                                <td>@Model.ApprovedBy_Occupation</td>
                            </tr>
                            <tr>
                                <td>Diketahui oleh</td>
                                <td>@Model.KnownBy_Name</td>
                                <td>@Model.KnownBy_Occupation</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
     
    </div>
End Using
<style>
    #documentView .form-group
    {
        margin-top: 0px;
        margin-bottom: 0px;
    }
    #documentView .form-control-static
    {
        padding-top: 4px;
    }
    .popover
    {
        min-width:300px;
        min-height:200px;
    }
    div.dataTables_processing { z-index: 1; }
</style>
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="../../../../plugins/datatables/sum.js" type="text/javascript"></script>
<script type="text/javascript">
    var tblItems = null;
    var _initTable = function () {

        var _renderRemarks = function (data, type, row) {
            if (type == 'display') {
                data = data == null ? "" : data;
                var content = "<a role='button'  data-trigger='focus' tabindex='0'  class='btn btn-lg btn-danger btn-xs'  data-toggle='popover' title='Catatan' " +
                        "data-content='" + data + "' ><span class='fa fa-align-left'></span> </a>";
                return content;
            }
            return data;
        }

        var arrColumns = [
              { "data": "DepartmentPurchaseRequisitionId", "sClass": "text-right" }, //0
              {"data": "ItemName" }, //1
               {"data": "Allocation" }, //2
              {"data": "Quantity", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //3
              {"data": "UnitQuantity" }, //4
              {"data": "Currency", "sClass": "text-right" }, //5
              {"data": "EstUnitPrice", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //6
               {"data": "Currency", "sClass": "text-right" }, //7
              {"data": "TotalEstPrice", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //8
              {"data": "Remarks", "sClass": "text-center", "mRender": _renderRemarks} //9

        ];
        var _localLoad = function (data, callback, setting) {
            var docId = $("#documentid").val();
            $.ajax({
                url: '/Purchasing/DepartmentPurchaseRequisition/GetRequestItems',
                data: { id: docId },
                type: 'POST',
                success: callback,
                error: ajax_error_callback,
                datatype: 'json'
            });
        };
        var _coldefs = [];

        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = false;
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            //show row number
            for (var i = 0, iLen = oSettings.aiDisplay.length; i < iLen; i++) {
                $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html((i + 1) + '.');
            }

            //calculate total
            var api = this.api();
            var total = api.column(8, { page: 'current' }).data().sum();
            total = $.number(total, 2, ",", ".");

            $(api.table().footer()).find("tr:first td").eq(2).html(total);
            $('[data-toggle="popover"]').popover({ placement: "left", trigger: "manual", container: 'body' }).click(function (e) {
                $('.popover').not(this).popover('hide');
                $(this).popover('toggle');
            }); ;
        };
        datatableDefaultOptions.ajax = _localLoad;
        tblItems = $('#tblItems').DataTable(datatableDefaultOptions)
    }         //init table

    $(function () {
        _initTable();

        $("#btnDeleteDocument").click(function () {

            if (confirm("Hapus dokumen ini ?")) {
                $.ajax({
                    type: "POST",
                    data: { id: $("#documentid").val() },
                    url: "/Purchasing/DepartmentPurchaseRequisition/DeleteDocument",
                    datatype: 'json',
                    success: function (data) {
                        if (data.stat == 1) {
                            window.location = "/Purchasing/DepartmentPurchaseRequisition/Index";
                        }
                    }
                });
            }
        });

    });    //init document

</script>
