@Code
    ViewData("Title") = "Purchase Order List"
    
    Dim arrDocState = Purchasing.GlobalArray.PurchaseOrderDocState()
    Dim serializer = New Script.Serialization.JavaScriptSerializer
    Dim strArrJson = serializer.Serialize(arrDocState)
    
    
End Code
<div class="row">
    <div class="col-xs-12">
        <div class="pull-right">
            <a href="@Url.Action("Create", "PurchaseOrder")" class="btn btn-sm btn-success  btn-add btn-label-left" >
                <span><i class="fa fa-plus"></i></span>Tambah Baru</a>
        </div>
    </div>
</div>
@Using Html.BeginJUIBox("Purchase Order List")
    @<div class="table-responsive">
        <table id="tblData" class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer">
            <colgroup>
                <col style="width: 60px" />
                <col style="width: 80px" />
                <col style="width: 160px" />
                <col style="width: auto" />
                <col style="width: 60px" />
                <col style="width: 120px" />
            </colgroup>
            <thead>
                <tr>
                    <th>
                        #
                    </th>
                    <th>
                        Tanggal
                    </th>
                    <th>
                        No. PO
                    </th>
                    <th>
                        Vendor
                    </th>
                    <th>Status</th>
                    <th>
                        Items
                    </th>
                    <th class="action">
                        Action
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    
End Using
<style>
    body .popover
    {
        width: 700px;
        max-width: 100%;
    }
</style>
@code
@Html.Raw("<script type='text/javascript'>" & vbCrLf)    
    @Html.Raw("var PurchaseOrderState =" & strArrJson & ";"& vbCrLf)
@Html.Raw("</script>" & vbCrLf)    
    
End Code

    


<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">
    var tblData = null;

    var _initDatatable = function () {
        var _renderIconItem = function (data, type, row) {
            if (type == 'display') {
                return "<a href='#' data-toggle='popover' class='btn btn-info'><span class='fa fa-list'></span></a>";
            }
            return data;
        }
        var _renderOrderState = function (data, type, row) {
            if (type == 'display') {
                return PurchaseOrderState[data];
            }
            return data;
        }
        var _renderDetail = function (data, type, row) {
            if (type == "display") {
                //return "<a href='/Purchasing/PurchaseOrder/Detail/" + row.ID + "' class='btn btn-primary'><span class='fa fa-list'></span></a>";
                var htmls = new Array();
                htmls.push('<div class="btn-group">');
                htmls.push("<a role='button' data-target='#' href='/Purchasing/PurchaseOrder/Detail/" + data +
                "' title='Detail DO' class='btn btn-primary'>Detail</a>");


                htmls.push('<button type="button" class="btn btn-danger dropdown-toggle" ');

                htmls.push('data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">');
                htmls.push('<span class="caret"></span>');
                htmls.push('<span class="sr-only">Action</span>');
                htmls.push('</button>');
                htmls.push('<ul class="dropdown-menu text-left pull-right">');
                htmls.push('<li><a href="/Purchasing/PurchaseOrder/Edit/' + data + '">Edit</a></li>');
                if (row.DocState < 1) {
                    htmls.push('<li><a href="#" class="lnkDelete">Hapus</a></li>');
                }
                if (row.DocState > 1) {
                    htmls.push('<li><a href="#" class="lnkArchive">Arsip</a></li>');
                }
                
                htmls.push('</ul>');

                htmls.push('</div>');


                return htmls.join("\n");
            }
            return data;
        }
        var arrColumns = [
              { "data": "ID", "sClass": "text-right" }, //0
              {"data": "OrderDate", "sClass": "text-center", "mRender": _fnRenderNetDate }, //1
               {"data": "OrderNumber" }, //2
              {"data": "Vendor_CompanyName" },
              { "data": "DocState", "sClass": "text-center", "mRender": _renderOrderState },
               { "data": "ID", "sClass": "text-center", "mRender": _renderIconItem },
                { "data": "ID", "sClass": "text-center", "mRender": _renderDetail} //3

        ];
        var _localLoad = function (data, callback, setting) {
            var docId = $("#documentid").val();
            $.ajax({
                url: '/Purchasing/PurchaseOrder/GetPOList',
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
        datatableDefaultOptions.ajax = _localLoad;
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            $(".lnkDelete").click(_lnkDelete_click);
            $(".lnkArchive").click(_lnkArchive_click);
            $('[data-toggle="popover"]').popover({
                title: "Detail",
                container: "body",
                html: true,
                placement: "left",
                content: function () {

                    var tr = $(this).closest('tr');
                    var row = tblData.row(tr);
                    var dataItem = row.data();
                    var total = 0;
                    var _html = "<table class='table table-bordered table-heading table-responsive' style='width:100%'>"
                    + "<thead><tr><th>#</th><th>Item</th><th colspan='2'>Kuantitas</th><th>Harga</th><th>Total</th></tr></thead><tbody>";
                    for (var i = 0; i < dataItem.Details.length; i++) {
                        _html += "<tr><td class='text-right'>" + (i + 1) + ".</td><td>" + dataItem.Details[i].ItemName + "</td>"
                        + "<td class='text-right'>" + $.number(dataItem.Details[i].Quantity, 2, ",", ".") + "</td>"
                        + "<td>" + dataItem.Details[i].UnitQuantity + "</td>"
                        + "<td class='text-right'>" + $.number(dataItem.Details[i].UnitPrice, 2, ".", ",") + "</td>"
                        + "<td class='text-right'>" + $.number(dataItem.Details[i].TotalPrice, 2, ".", ",") + "</td>"
                        + "</tr>";
                        total += dataItem.Details[i].TotalPrice;
                    }
                    _html += "</tbody>"
                    _html += "<tfoot><tr><td colspan='5'>Total</td><td class='text-right'>" + $.number(total, 2, ",", ".") + "</td></tr></tfoot>";
                    _html += "</table>";
                    ;
                    return _html;

                }

            });
            $('[data-toggle="popover"]').on("click", function (e) {
                $('[data-toggle="popover"]').not(this).popover("hide");
            });

        }
        tblData = $('#tblData').DataTable(datatableDefaultOptions)

    }
    var _lnkDelete_click = function (e) {
        e.preventDefault();
        if (confirm("Hapus dokumen ini?") == false) {
            return;
        }
        var row = _getDataTableRow(this)
        $.ajax({
            url: '/Purchasing/PurchaseOrder/Delete',
            type: 'POST',
            data: { id: row.data().ID },
            success: function () {
                tblData.ajax.reload();
            },
            error: ajax_error_callback,
            datatype: 'json'
        });

    }

    var _lnkArchive_click = function (e) {
        e.preventDefault();
        if (confirm("Arsipkan dokumen ini?") == false) {
            return;
        }
        var row = _getDataTableRow(this)
        $.ajax({
            url: '/Purchasing/PurchaseOrder/Archive',
            type: 'POST',
            data: { id: row.data().ID },
            success: function () {
                tblData.ajax.reload();
            },
            error: ajax_error_callback,
            datatype: 'json'
        });

    }
    var _getDataTableRow = function (obj) {
        return tblData.row($(obj).closest('tr'));
    }
    $(function () {
        _initDatatable();
      
    }); //init

</script>
