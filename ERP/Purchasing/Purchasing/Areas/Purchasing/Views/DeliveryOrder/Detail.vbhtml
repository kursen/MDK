@ModelType Purchasing.DeliveryOrder 
@Code
    ViewData("Title") = "Detail Penerimaan Barang"
    Html.SetEditableDefaultSettings(DisableOnload:=True)

    Dim _purchaseEntities As New Purchasing.PurchasingEntities 
    Dim loopNumber As Integer = 0
    Dim dataDetail = (From dod In _purchaseEntities.DeliveryOrderDetails Where
                 dod.DeliveryOrderId = Model.ID
                 Order By dod.ID
                 Select New With {
                 .ID = dod.ID,
                 .NamaItem = dod.PurchaseOrderDetail.ItemName,
                 .UnitQuantity = dod.PurchaseOrderDetail.UnitQuantity,
                 .QuantityOrder = dod.PurchaseOrderDetail.Quantity,
                 .QuantityReady = (From sq In _purchaseEntities.DeliveryOrderDetails Where
                    sq.DeliveryOrder.PurchaseOrderId = Model.PurchaseOrderId AndAlso
                    sq.PurchaseOrderDetailId = dod.PurchaseOrderDetailId AndAlso
                    sq.ID < dod.ID
                    Select sq.Quantity).DefaultIfEmpty(0).Sum(),
                 .Quantity = dod.Quantity
             }).ToList
End Code

@Using Html.BeginJUIBox("Detail Penerimaan Barang")
        @<div class="row">
            <div class="col-lg-12 col-sm-12">
                <div class="pull-right">
                    @*<button type="button" class="btn btn-danger btn-label-left" onclick="$('#panelMaster .editable') .editable('toggleDisabled')">
                        <span><i class="fa fa-edit"></i></span>Edit</button>*@
                    <a href="@Url.Action("Edit", "DeliveryOrder", New With {.Id = Model.ID})" class="btn btn-danger btn-label-left">
                    <span><i class="fa fa-list"></i></span>Edit</a>
                    <a href="@Url.Action("Index", "DeliveryOrder")" class="btn btn-danger btn-label-left">
                    <span><i class="fa fa-list"></i></span>Daftar DO</a>
                    <a href="@Url.Action("PrintDO", "DeliveryOrder", New With {.id = Model.ID})" class="btn btn-danger btn-label-left">
                            <span><i class="fa fa-print"></i></span>Print</a>
                    @*<button type="button" class="btn btn-danger btn-label-left" id="btnDelete">
                    <span><i class="fa fa-trash"></i></span>Hapus</button>*@
                </div>
            </div>
        </div>
    @<input type='hidden' name='Id' id='DOId' value="@Model.ID" />
    @<dl class="dl-horizontal" id='panelMaster'>
        <dt>Tanggal</dt>
        <dd>@Html.EditableInputTextBox("ReceiptDate", Model.ReceiptDate.ToString("dd-MM-yyyy"), "date", "Tanggal Terima", datapk:=Model.ID, dataurl:="/DeliveryOrder/SavePartial")</dd>
        <dt>No. DO</dt>
        <dd>@Html.EditableInputTextBox("DocNo", Model.DocNo, "text", "No DO", datapk:=Model.ID, dataurl:="/DeliveryOrder/SavePartial")</dd>
        <dt>No. PO</dt>
        <dd>@Html.ActionLink(Model.DeliveryOrderDetails.First.PurchaseOrderDetail.PurchaseOrder.OrderNumber,"DetailPO",New With {.id=Model.DeliveryOrderDetails.First.PurchaseOrderDetail.PurchaseOrderId},Nothing)</dd>
        <dt>Diterima Oleh</dt>
        <dd>@Html.EditableInputTextBox("PreparedBy_Name", Model.receiptBy_Name, "text", "Diketahui Oleh", datapk:=Model.ID, dataurl:="/DeliveryOrder/SavePartial")</dd>
    </dl>
End Using

    <div class = "row" visible ="false" ></div>
  
    <div id="panelDetail">
    @Using Html.BeginJUIBox("Items")
        @<div class="table-responsive">
            <table class="table table-bordered table-striped table-hover table-heading table-datatable dataTable"
                id="tblDODetail">
                <colgroup>
                    <col style="width: 30px" />
                    <col style="width: auto" />
                    <col style="width: 100px" />
                    <col style="width: 50px" />
                    <col style="width: 100px" />
                    <col style="width: 50px" />
                    <col style="width: 100px" />
                    <col style="width: 50px" />
                </colgroup>
                <thead>
                    <tr>
                        <th>
                            No.
                        </th>
                        <th style="width: 300px">
                            Nama Item
                        </th>
                        <th colspan="2">
                            Jumlah Permintaan
                        </th>
                        <th colspan="2">
                            Sudah Diterima
                        </th>
                        <th colspan="2">
                            Diterima
                        </th>
                    </tr>
                </thead>
                <tbody>
                    @For Each item In dataDetail 
                            loopNumber += 1
                        @<tr>
                            <td align='right'>@(loopNumber) </td>
                            <td>@item.NamaItem </td>
                            <td align='right'>@item.QuantityOrder</td>
                            <td> @item.UnitQuantity </td>
                            <td align='right'>@item.QuantityReady</td>
                            <td>@item.UnitQuantity</td> 
                            <td align='right'>@Html.EditableInputTextBox("Quantity" & item.ID, item.Quantity, "number", "Diterima", datapk:=item.ID, dataurl:="/DeliveryOrder/SavePartialDetail")</td>
                            <td>@item.UnitQuantity</td> 
                         </tr>
                     Next
                </tbody>    
                    
            </table>
        </div>
    End Using
    </div>

    <style type="text/css">
        .popover
        {
            z-index: 40000;
        }
    </style>
    <script src="@Url.Content("~/plugins/jquery/jquery-migrate-1.2.1.min.js")" type="text/javascript"></script>
    <script src="../../../../plugins/datatables/jquery.dataTables.js" type="text/javascript"></script>
    <script src="../../../../plugins/datatables/dataTables.overrides_indo.js" type="text/javascript"></script>
    <script type="text/javascript" src="@Url.Content("~/js/CRUDHelpers.js")"></script>
@Section endScript
    <script type="text/javascript">
        $.fn.editabletypes.date.defaults.datepicker.language = "id";
        $.fn.editabletypes.date.defaults.viewformat = "dd-mm-yyyy";
        $.fn.editabletypes.date.defaults.format = "dd-mm-yyyy";

        var _savepartialresponse = function (data) {
            if ((data) && (data.stat == 1)) {

            } else {
                showNotification(data);
            };

        }
        $(function () {
            $("#btnDelete").click(function () {

                if (confirm("Hapus dokumen ini?")) {
                    $.ajax({
                        type: "POST",
                        data: { id: $("#DOId").val() },
                        url: "/Purchasing/DeliveryOrder/Delete",
                        datatype: 'json',
                        success: function (data) {
                            if (data.stat == 1) {
                                window.location = "/Purchasing/DeliveryOrder/Index";
                            }
                        }
                    });
                }
            });

        });
    </script>
End Section
