@ModelType Purchasing.PurchaseOrder
@Code
    ViewData("Title") = "Detail PO"
    Dim _purchaseEntities As New Purchasing.PurchasingEntities
    
    Dim modelDO = (From mdo In _purchaseEntities.DeliveryOrders
                   Where mdo.PurchaseOrderId = Model.ID).ToList
    
    Dim detailNumber As Integer = 0

End Code
@Functions
    Function WriteSpanControl(ByVal value As String, ByVal id As String) As MvcHtmlString
        Return New MvcHtmlString("<p class='form-control-static'>" & value & "</p>")
    End Function
End Functions

@Using Html.BeginJUIBox("Detail PO")
    @<div class="row">
            <div class="col-lg-12 col-sm-12">
                <div class="pull-right">
                    <a href="@Url.Action("Index", "DeliveryOrder")" class="btn btn-danger btn-label-left">
                    <span><i class="fa fa-list"></i></span>Daftar DO</a>
                    <button class="btn btn-danger" id="btnClosePO">Tutup PO</button>

                </div>
            </div>
        </div>
    @<div id='documentView'>
    <input type="hidden" id="documentid" value ="@Model.ID" />
        
        <div class="row">
            <div class="col-sm-offset-6 col-lg-offset-8">
                <div class="form-horizontal POTop">
                    @Html.WriteFormInput(WriteSpanControl(Model.OrderDate.ToString("dd-MM-yyyy"), "spOrderDate"), "Tanggal", smLabelWidth:=5, lgLabelWidth:=5)
                    <hr />
                    @Html.WriteFormInput(WriteSpanControl(Model.OrderNumber, "spOrderNumber"), "No. PO", smLabelWidth:=5, lgLabelWidth:=5)
                    <hr />
                    @Html.WriteFormInput(WriteSpanControl(If(Model.DeliveryDate.HasValue,
                                                             Model.DeliveryDate.Value.ToString("dd-MM-yyyy"), ""), "spDeliveryDate"),
                                                     "Tgl Kebutuhan", smLabelWidth:=5,lgLabelWidth:=5)
                                                     <hr />
                    @Html.WriteFormInput(WriteSpanControl(Model.TermOfPayment, "spOrderNumber"), "Term Of Payment", smLabelWidth:=5, lgLabelWidth:=5)
                    <hr />
                </div>
            </div>
        </div>
        <hr />
        <div class="row">
            <div class="col-sm-6 col-lg-6">
                <div class="form-horizontal">
                <h4>Vendor</h4>
                @Html.WriteFormInput(WriteSpanControl(Model.Vendor_CompanyName, "spContactPerson_Phone"), "Perusahaan")
                @Html.WriteFormInput(WriteSpanControl(Model.Vendor_ContactName, "spContactPerson_Phone"), "Contact")
                @Html.WriteFormInput(WriteSpanControl(Model.Vendor_Phone, "spContactPerson_Phone"), "Tel/Mobile")
                @Html.WriteFormInput(WriteSpanControl(Model.Vendor_Address, "spContactPerson_Phone"), "Alamat")
                @Html.WriteFormInput(WriteSpanControl(Model.Vendor_City & "-" & Model.Vendor_Province, "spContactPerson_Phone"), "")
                </div>
                </div>
            <div class="col-sm-6 col-lg-6">
                <div class="form-horizontal">
                <h4>Dikirim Ke</h4>
                    @Html.WriteFormInput(WriteSpanControl(Model.ContactPerson_Name, "spContactPerson_Name"), "Contact")
                    @Html.WriteFormInput(WriteSpanControl(Model.ContactPerson_Phone, "spContactPerson_Phone"), "No. Tel/Mobile")
                    
                    
                    @Html.WriteFormInput(WriteSpanControl(Model.DeliveryTo, "spDeliveryTo"), "Dikirimkan Ke")
                    @Html.WriteFormInput(WriteSpanControl(Model.DeliveryAddress, "spDeliveryAddress"), "Alamat")
                    
                </div>
            </div>
        </div>
        <div style="margin-top: 30px">
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    DAFTAR ITEM PERMINTAAN</div>
                <div class="table-responsive">
                    <table class="table table-bordered table-striped table-hover table-heading table-datatable dataTable"
                        id="tblItems">
                        <colgroup>
                            <col style="width: 60px" />
                            <col style="width: auto" />
                            <col style="width: 60" />
                            <col style="width: 60" />
                            <col style="width: 60" />
                            <col style="width: 60" />
                            <col style="width: 70" />
                        </colgroup>
                        <thead>
                            <tr>
                                <th>
                                    No.
                                </th>
                                <th style="width: 300px">
                                    Item
                                </th>
                                <th colspan="2">
                                    Jumlah
                                </th>
                                <th colspan="2">
                                    Diterima
                                </th>
                                <th>Lengkap</th>
                            </tr>
                        </thead>
                       
                    </table>
                </div>
            </div>
        </div>
    </div>
    
    For Each item In modelDO
        detailNumber = 1
        Using Html.BeginJUIBox("Data Penerimaan Barang No: " & item.DocNo)
        @<div class="row">
            <div class="col-sm-12 col-lg-12">
                <div class="form-horizontal">
                    @Html.WriteFormInput(WriteSpanControl(item.ReceiptDate.ToString("dd MMM yyyy"), "Receipt_Date"), "Tanggal Terima", lgLabelWidth:=4)
                    @Html.WriteFormInput(WriteSpanControl(item.ReceiptBy_Name, "ReceiptBy_Name"), "Diterima Oleh", lgLabelWidth:=4)
                </div>
            </div>
        </div>
        
       @<div class="row">
            <div class="col-lg-12 col-sm-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        DAFTAR ITEM DITERIMA</div>
                    <div class="table-responsive">
                        <table class="table table-bordered table-striped table-hover table-heading table-datatable dataTable">
                            <colgroup>
                                <col style="width: 60px" />
                                <col style="width: auto" />
                                <col style="width: 60px" />
                                <col style="width: 60px" />
                            </colgroup>
                            <thead>
                                <tr>
                                    <th>
                                        No.
                                    </th>
                                    <th style="width: 300px">
                                        Item
                                    </th>
                                    <th colspan="2">
                                        Diterima
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                @For Each itemDetail In item.DeliveryOrderDetails
                                        If itemDetail.Quantity > 0 Then
                                        @<tr>
                                        <td align="right">@(detailNumber).</td>
                                        <td>@itemDetail.PurchaseOrderDetail.ItemName</td>
                                        <td align = "right">@itemDetail.Quantity</td>
                                        <td>@itemDetail.PurchaseOrderDetail.UnitQuantity</td>
                                        </tr>
                                            detailNumber += 1
                                End If
                            Next
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        
        @<div style="margin-top: 30px">
        </div>
End Using
Next
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
    #documentView hr
    {
        margin:0px;
        padding:0px;
    }
    .POTop .control-label
    {
        text-align:left;
    }
    .popover
    {
        min-width: 300px;
        min-height: 200px;
    }
    div.dataTables_processing
    {
        z-index: 1;
    }
</style>
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="../../../../plugins/datatables/sum.js" type="text/javascript"></script>
<script type="text/javascript">
    var tblItems = null;
    var _initTable = function () {

        var renderCheckComplete = function (data, type, row) {
            if (type == "display") {
                if (row.Quantity == row.QuantityReady) {
                    return '<i class="fa fa-check"></i>';
                } else {
                    return '<i class="fa fa-square"></i>';
                }


            }
            return data;
        }
        var arrColumns = [
              { "data": "PurchaseOrderId", "sClass": "text-right" }, //0
              {"data": "ItemName" }, //1
              {"data": "Quantity", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //2
              {"data": "UnitQuantity" }, //3
              {"data": "QuantityReady", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //4
              {"data": "UnitQuantity" }, //5
              {"data": "UnitQuantity", "sClass": "text-center", "mRender": renderCheckComplete }, //5
        ];

        var _localLoad = function (data, callback, setting) {
            var docId = $("#documentid").val();
            $.ajax({
                url: '/Purchasing/DeliveryOrder/GetPOItems',
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
            $('[data-toggle="popover"]').popover({ placement: "left", trigger: "manual", container: 'body' }).click(function (e) {
                $('.popover').not(this).popover('hide');
                $(this).popover('toggle');
            }); ;
        };
        datatableDefaultOptions.ajax = _localLoad;
        tblItems = $('#tblItems').DataTable(datatableDefaultOptions)
    }              //init table

    $(function () {
        _initTable();

        $("#btnClosePO").click(function () {


            if (confirm("Tandai bahwa semua penerimaan telah lengkap?") == false) {
                return;
            }
            var poid = $("#documentid").val();
            $.ajax({
                url: '/Purchasing/PurchaseOrder/ClosePO',
                type: 'POST',
                data: { id: poid },
                success: function (d) {
                    if (d.stat == 0) {
                        showNotificationSaveError(d, "Gagal Menutup PO");
                        return;
                    } else {
                        showNotification("PO telah ditutup");

                        setTimeout(function () { window.location = "/Purchasing/DeliveryOrder/Index"; }, 1000);

                    }
                },
                error: ajax_error_callback,
                datatype: 'json'
            });
        });

    });

</script>
