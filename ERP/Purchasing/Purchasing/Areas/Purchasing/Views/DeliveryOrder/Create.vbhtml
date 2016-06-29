@ModelType Purchasing.DeliveryOrder
@Code
    ViewData("Title") = "Penerimaan Barang"
   
    Dim dateFormat = New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left",
                               .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}}
   
End Code

@Using Html.BeginForm("Save", "DeliveryOrder", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmDeliveryOrder"})
    @<input type='hidden' name='Id' id='DOId' value="@Model.ID" />
    @<input type='hidden' name='PurchaseOrderId' id='PurchaseOrderId' value="@Model.PurchaseOrderId" />
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            @Using Html.BeginJUIBox("Penerimaan Barang")
                @Html.WriteFormDateInputFor(Function(m) m.ReceiptDate, "Tanggal", dateFormat, smControlWidth:=4, lgControlWidth:=2)
                @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.DocNo, New With {.class = "form-control"}), "No. DO", smControlWidth:=8)
                @Html.WriteFormInput(Html.TextBox("OrderNumber", ViewData("OrderNumber"), New With {.class = "form-control"}), "No. PO", smControlWidth:=8)
    End Using
        </div>
    </div>
    @<div class = "row" visible ="false" ></div>
    Using Html.BeginJUIBox("Items")
        @<div class="table-responsive">
            <table class="table table-bordered table-striped table-hover table-heading table-datatable dataTable"
                id="tblDODetail">
                <colgroup>
                    <col style="width: 50px" />
                    <col style="width: auto" />
                    <col style="width: 60" />
                    <col style="width: 60" />
                    <col style="width: 60" />
                    <col style="width: 60" />
                    <col style="width: 60" />
                    <col style="width: 60" />
                    <col style="width: 60" />
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
                </tbody>
            </table>
        </div>
    End Using
    @<div class="row">
        <div class="col-lg-4 col-sm-4 pull-right">
            <div class="panel panel-primary">
                <div class="panel-heading">Diterima Oleh</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.ReceiptBy_Name, New With {.class = "form-control person"}), "Nama", smControlWidth:=8, lgControlWidth:=8)
                    </div>
                </div>
            </div>
        </div>
    </div>
End Using
<div class = "row" visible ="false" ></div>
<div class="well cell-center">
    <div class="center-block" style="width: 200px">
        <button type="button" class="btn btn-primary" id="btnSaveDeliveryOrder">
            Simpan</button>
        <a href="@Url.Action("Index", "DeliveryOrder")" class="btn btn-default" >Batal</a>
    </div>
</div>
<link rel="stylesheet" type="text/css" href="@Url.Content("~/plugins/datatables/dataTables.bootstrap.css")" />
<link href="../../../../plugins/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" type="text/css" />
<script src="@Url.Content("~/plugins/jquery-number/jquery.numeric.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/sum.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/bootstrap-typeahead/typeahead.bundle.min.js")" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="@Url.Content("~/plugins/bootstrap-typeahead/typeahead.css")" />

<script src="../../../../plugins/tinymce/tinymce.min.js" type="text/javascript"></script>

<script type="text/javascript">
    var _loadItems = function (POId) {
        $("#tblDODetail tbody").empty();
        $("#PurchaseOrderId").val(POId);
        $.ajax({
            type: "POST",
            url: '/Purchasing/DeliveryOrder/GetPurchaseOrderItems',
            data: { "POId": POId, "DOId": $("#DOId").val()},
            dataType: 'json',
            success: function (data) {
                var strHTML = "";
                for (var i = 0; i < data.length; i++) {
                    strHTML += "<tr>";
                    strHTML += "<td align='right'>" + (i + 1) + "</td>";
                    strHTML += "<td>" + data[i].NamaItem + "</td>";
                    strHTML += "<td align='right'>" + $.number(data[i].QuantityOrder, 2, ",", ".") + "</td>";
                    strHTML += "<td>" + data[i].UnitQuantity + "</td>";
                    strHTML += "<td align='right'>" + $.number(data[i].QuantityReady, 2, ",", ".") + "</td>";
                    strHTML += "<td>" + data[i].UnitQuantity + "</td>";
                    strHTML += "<td style='padding-right:30px'><input type='hidden' value=" + data[i].ID + " name='PurchaseOrderDetailId'>";
                    strHTML += "";
                    strHTML += "<input type='text' min='0' max='" + (data[i].QuantityOrder - data[i].QuantityReady) +
                                "' value='" + data[i].Quantity + 
                                "' name='Quantity' class='form-control numeric text-right'></td>";
                    strHTML += "<td>" + data[i].UnitQuantity + "</td>";
                    strHTML += "<tr>";
                }
                $("#tblDODetail tbody").append(strHTML);
                $('.numeric').numeric({ decimal: "," });
                $('.numeric').blur(function (e) {
                    var maxValue = $(this).attr("max");
                    var curValue = $(this).val();
                    if (curValue < 0) { $(this).val(0);}
                    if (curValue > maxValue) { $(this).val(maxValue);}
                });
            }
        });
    }

    $(document).ready(function () {
        //Submit Purchase
        $('#btnSaveDeliveryOrder').click(function (e) {
            showSavingNotification("Menyimpan data");
            //Operation
            var _data = $('#frmDeliveryOrder').serializeArray();
            var _url = $("#frmDeliveryOrder").attr('action');
            $.ajax({
                type: "POST",
                data: $.param(_data),
                url: _url,
                datatype: 'json',
                success: function (data) {
                    if (data.stat == 0) {
                        showNotificationSaveError(data);
                        return
                    } else {
                        window.location = "/Purchasing/DeliveryOrder/Index";
                    }
                }
            });
        });

        $("#OrderNumber").autocomplete({
            source: "/Purchasing/DeliveryOrder/GetPurchaseOrderNumber",
            data: { term: $("#OrderNumber").val() },
            minLength: 1,
            select: function (event, ui) {
                $("#OrderNumber").val(ui.item.value);
                _loadItems(ui.item.key);
                return false;
            }
        })
        .data("ui-autocomplete")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<a>" + item.value + "</a>")
                .appendTo(ul);
        };

        if ($("#PurchaseOrderId").val() > 0) { _loadItems($("#PurchaseOrderId").val()); }
    });   
                                             
</script>
