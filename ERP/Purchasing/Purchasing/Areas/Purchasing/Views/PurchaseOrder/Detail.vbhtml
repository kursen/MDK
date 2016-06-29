@ModelType Purchasing.PurchaseOrder
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
                    @<a href="@Url.Action("Edit", "PurchaseOrder", New With {.id = Model.ID})" class="btn btn-danger btn-label-left">
                        <span><i class="fa fa-edit"></i></span>Edit</a> 
                End If
                <a href="@Url.Action("Index", "PurchaseOrder")" class="btn btn-danger btn-label-left">
                    <span><i class="fa fa-list"></i></span>Daftar PO</a> <a href="@Url.Action("PrintPO", "PurchaseOrder", New With {.id = Model.ID})" class="btn btn-danger btn-label-left">
                        <span><i class="fa fa-print"></i></span>Print</a>
                <button type="button" class="btn btn-danger btn-label-left" id="btnDeleteDocument">
                    <span><i class="fa fa-trash"></i></span>Hapus</button>
@If Model.Archive Then
                @<button type="button" class="btn btn-danger btn-label-left" id="btnUnArchive">
                    <span><i class="fa fa-down"></i></span>Buka Arsip</button>
Else
    @<button type="button" class="btn btn-danger btn-label-left" id="btnArchive">
                    <span><i class="fa fa-up"></i></span>Arsipkan</button>
End If
            </div>
        </div>
    </div>
</div>
@Using Html.BeginJUIBox("Detail PO")
    
    @<div id='documentView'>
        <input type="hidden" id="documentid" value ="@Model.ID" />
        <input type='hidden' id='PPnAdded' value='@Model.PPnAdded' />
        <div class="row">
            <div class="col-sm-offset-6 col-lg-offset-8">
                <div class="form-horizontal POTop">
                    @Html.WriteFormInput(WriteSpanControl(Model.OrderDate.ToString("dd-MM-yyyy"), "spOrderDate"), "Tanggal", smLabelWidth:=5, lgLabelWidth:=5)
                    <hr />
                    @Html.WriteFormInput(WriteSpanControl(Model.OrderNumber, "spOrderNumber"), "No. PO", smLabelWidth:=5, lgLabelWidth:=5)
                    <hr />
                    @Html.WriteFormInput(WriteSpanControl(If(Model.DeliveryDate.HasValue,
                                                             Model.DeliveryDate.Value.ToString("dd-MM-yyyy"), ""), "spDeliveryDate"),
                                                     "Tgl Kebutuhan", smLabelWidth:=5, lgLabelWidth:=5)
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
                    <h4>
                        Vendor</h4>
                    @Html.WriteFormInput(WriteSpanControl(Model.Vendor_CompanyName, "spContactPerson_Phone"), "Perusahaan")
                    @Html.WriteFormInput(WriteSpanControl(Model.Vendor_ContactName, "spContactPerson_Phone"), "Contact")
                    @Html.WriteFormInput(WriteSpanControl(Model.Vendor_Phone, "spContactPerson_Phone"), "Tel/Mobile")
                    @Html.WriteFormInput(WriteSpanControl(Model.Vendor_Address, "spContactPerson_Phone"), "Alamat")
                    @Html.WriteFormInput(WriteSpanControl(Model.Vendor_City & "-" & Model.Vendor_Province, "spContactPerson_Phone"), "")
                </div>
            </div>
            <div class="col-sm-6 col-lg-6">
                <div class="form-horizontal">
                    <h4>
                        Dikirim Ke</h4>
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
                    DAFTAR ITEM PERMINTAAAN</div>
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
                                    Item
                                </th>
                                <th colspan="2">
                                    Jumlah
                                </th>
                                <th colspan="2">
                                    Harga
                                </th>
                                <th colspan="2">
                                    Total Harga
                                </th>
                            </tr>
                        </thead>
                        <tfoot>
                            <tr>
                                <td>
                                </td>
                                <td colspan="6">
                                    Total
                                </td>
                                <td id='ppnbefore'>
                                </td>
                            </tr>
                             <tr class='ppn-row'>
                                <td>
                                </td>
                                <td colspan="6" class='text-right'>
                                    PPn 10%
                                </td>
                                <td id='ppn10' class='text-right'>
                                </td>
                            </tr>
                             <tr class='ppn-row'>
                                <td>
                                </td>
                                <td colspan="6" class='text-right'>
                                    Total
                                </td>
                                <td id='ppnafter' class='text-right'>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Catatan/Remarks</div>
                <div class="panel-body">
                    @Html.Raw(Model.Remarks)
                </div>
            </div>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
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
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    Disetujui oleh
                                </td>
                                <td>@Model.ApprovedBy_Name
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Dibuat Oleh
                                </td>
                                <td>@Model.PreparedBy_Name
                                </td>
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
    #documentView hr
    {
        margin: 0px;
        padding: 0px;
    }
    .POTop .control-label
    {
        text-align: left;
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




        var arrColumns = [
              { "data": "PurchaseOrderId", "sClass": "text-right" }, //0
              {"data": "ItemName" }, //1
              {"data": "Quantity", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //2
              {"data": "UnitQuantity" }, //3
              {"data": "Currency", "sClass": "text-right" }, //4
              {"data": "UnitPrice", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //5
               {"data": "Currency", "sClass": "text-right" }, //6
              {"data": "TotalPrice", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //7

        ];


        var _localLoad = function (data, callback, setting) {
            var docId = $("#documentid").val();
            $.ajax({
                url: '/Purchasing/PurchaseOrder/GetPOItems',
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
            var total = api.column(7, { page: 'current' }).data().sum();
            var stotal = $.number(total, 2, ",", ".");



            $(api.table().footer()).find("tr:first td").eq(2).html(stotal);
            $("#ppn10").html($.number(total * 0.1, 2, ",", "."));
            $("#ppnafter").html($.number(total + (total * 0.1), 2, ",", "."));
            $('[data-toggle="popover"]').popover({ placement: "left", trigger: "manual", container: 'body' }).click(function (e) {
                $('.popover').not(this).popover('hide');
                $(this).popover('toggle');
            }); ;
        };
        datatableDefaultOptions.ajax = _localLoad;
        tblItems = $('#tblItems').DataTable(datatableDefaultOptions)
    }          //init table

    $(function () {
        _initTable();

        $("#btnDeleteDocument").click(function () {

            if (confirm("Hapus dokumen ini ?")) {
                $.ajax({
                    type: "POST",
                    data: { id: $("#documentid").val() },
                    url: "/Purchasing/PurchaseOrder/Delete",
                    datatype: 'json',
                    success: function (data) {
                        if (data.stat == 1) {
                            window.location = "/Purchasing/PurchaseOrder/Index";
                        }
                    }
                });
            }
        }); //

        $("#btnArchive").click(function () {
            $.ajax({
                type: "POST",
                data: { id: $("#documentid").val() },
                url: "/Purchasing/PurchaseOrder/ClosePO",
                datatype: 'json',
                success: function (d) {
                    if (d.stat == 0) {
                        showNotificationSaveError(d, "Gagal Menutup PO");
                        return;
                    } else {
                        showNotification("PO telah ditutup");

                        setTimeout(function () { window.location = "/Purchasing/DeliveryOrder/Index"; }, 1000);

                    }
                }
            });

        }); //btnArchive;

        $("#btnUnArchive").click(function () {
            $.ajax({
                type: "POST",
                data: { id: $("#documentid").val() },
                url: "/Purchasing/PurchaseOrder/UnArchive",
                datatype: 'json',
                success: function (data) {
                    showNotification("Data dikembalikan dari arsip");
                }
            });

        }); //btnArchive;

        if ($("#PPnAdded").val() == "True") {
            $(".ppn-row").show();
        } else {
            $(".ppn-row").hide();
        }

    });         //init document

</script>
