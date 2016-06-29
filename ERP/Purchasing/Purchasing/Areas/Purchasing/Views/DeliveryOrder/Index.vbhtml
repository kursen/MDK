@Code
    ViewData("Title") = "Daftar Penerimaan Barang"
End Code
<div class="row">
    <div class="col-xs-12">
        <div class="pull-right">
            <a href="@Url.Action("Create", "DeliveryOrder")" class="btn btn-sm btn-success  btn-add btn-label-left" >
                <span><i class="fa fa-plus"></i></span>Tambah Baru</a>
        </div>
    </div>
</div>
@Using Html.BeginJUIBox("Daftar Penerimaan Barang")
    @<div class="table-responsive">
        <table id="tblData" class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer">
            <colgroup>
                <col style="width: 60px" />
                <col style="width: 100px" />
                <col style="width: 160px" />
                <col style="width: auto" />
                <col style="width: 160px" />
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
                        No. DO
                    </th>
                    <th>
                        Vendor
                    </th>
                    <th>
                        Diterima Oleh
                    </th>
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
        var _renderDetail = function (data, type, row) {
            if (type == 'display') {
                var htmls = new Array();
                htmls.push('<div class="btn-group">');
                htmls.push("<a role='button' data-target='#' href='/Purchasing/DeliveryOrder/Detail/" + data +
                "' title='Detail DO' class='btn btn-primary'>Detail DO</a>");
                htmls.push('<button type="button" class="btn btn-danger dropdown-toggle" ');
                htmls.push('data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">');
                htmls.push('<span class="caret"></span>');
                htmls.push('<span class="sr-only">Action</span>');
                htmls.push('</button>');
                htmls.push('<ul class="dropdown-menu text-left pull-right">');
                htmls.push('<li><a href="/Purchasing/DeliveryOrder/Edit/' + data + '">Edit DO</a></li>');
                htmls.push('<li><a href="#" class="lnkDelete">Hapus DO</a></li>');
                htmls.push('</ul>');
                htmls.push('</div>');


                return htmls.join("\n");
            }
            return data;
        }
        var renderlnkDetailPO = function (data, type, row) {
            if (type == "display") {
                return '<a href="/Purchasing/DeliveryOrder/DetailPO/' + row.PurchaseOrderId + '">' + data + '</a>'
            }
            return data;

        }
        var arrColumns = [
              { "data": "PurchaseOrderId", "sClass": "text-right" },
              { "data": "ReceiptDate", "sClass": "text-center", "mRender": _fnRenderNetDate },
              { "data": "OrderNumber", "mRender": renderlnkDetailPO },
              { "data": "DocNo" },
              { "data": "Vendor_CompanyName" },
              { "data": "ReceiptBy_Name" },
              { "data": "ID", "sClass": "text-center", "mRender": _renderIconItem },
              { "data": "ID", "sClass": "text-center", "mRender": _renderDetail }

        ];
        var _localLoad = function (data, callback, setting) {
            var docId = $("#documentid").val();
            $.ajax({
                url: '/Purchasing/DeliveryOrder/GetDOList',
                type: 'POST',
                success: callback,
                error: ajax_error_callback,
                datatype: 'json'
            });
        };
        var _coldefs = [];
        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [{ "targets": [1, 2, 3, 4, 5, 6, 7], "orderable": false }, { "visible": false, "targets": 2}];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = true;
        datatableDefaultOptions.order = [[0, 'asc']];
        datatableDefaultOptions.ajax = _localLoad;
        datatableDefaultOptions.fnCreatedRow = function (row, data, index) {
            $(row).data("poid", data.PurchaseOrderId);

        };

        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            _fnlocalDrawCallback(oSettings);

            $('[data-toggle="popover"]').popover({
                title: "Detail",
                container: "body",
                html: true,
                placement: "left",
                content: function () {
                    var tr = $(this).closest('tr');
                    var row = tblData.row(tr);
                    var dataItem = row.data();
                    var _html = "<table class='table table-bordered table-heading table-responsive' style='width:100%'>"
                    + "<thead><tr><th>#</th><th>Item</th><th>Satuan</th><th>Kuantitas</th><th>Diterima</th><th>Sudah Diterima</th><th>Total</th><th>Lengkap</th></tr></thead><tbody>";
                    for (var i = 0; i < dataItem.Details.length; i++) {
                        _html += "<tr><td class='text-right'>" + (i + 1) + ".</td><td>" + dataItem.Details[i].ItemName + "</td>"
                        + "<td class='text-center'>" + dataItem.Details[i].UnitQuantity + "</td>"
                        + "<td class='text-right'>" + $.number(dataItem.Details[i].QuantityOrder, 2, ",", ".") + "</td>"
                        + "<td class='text-right'>" + $.number(dataItem.Details[i].QuantityReceipt, 2, ".", ",") + "</td>"
                        + "<td class='text-right'>" + $.number(dataItem.Details[i].QuantityReady, 2, ".", ",") + "</td>"
                        + "<td class='text-right'>" + $.number(dataItem.Details[i].QuantityReady + dataItem.Details[i].QuantityReceipt, 2, ".", ",") + "</td>"
                        + "<td class='text-center'>" + (dataItem.Details[i].QuantityReady + dataItem.Details[i].QuantityReceipt >= +dataItem.Details[i].QuantityOrder ? "Ya" : "Tidak") + "</td>"
                        + "</tr>";
                    }
                    _html += "</tbody>"
                    _html += "</table>";
                    ;
                    return _html;
                }
            });
            $('[data-toggle="popover"]').on("click", function (e) {
                e.preventDefault();
                $('[data-toggle="popover"]').not(this).popover("hide");
            });



            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;

            api.column(2, { page: 'current' }).data().each(function (group, i) {

                if (last !== group) {
                    var poid = $(rows[i]).data("poid");
                    
                    var htmls = new Array();
                    htmls.push('<div class="btn-group">');
                    htmls.push("<a role='button' data-target='#'  href='/Purchasing/DeliveryOrder/DetailPO/" + poid +
                     "' title='Detail PO' class='btn btn-primary'>Detail PO</a>");
                    htmls.push('<button type="button" class="btn btn-danger dropdown-toggle" ');
                    htmls.push('data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">');
                    htmls.push('<span class="caret"></span>');
                    htmls.push('<span class="sr-only">Action</span>');
                    htmls.push('</button>');
                    htmls.push('<ul class="dropdown-menu text-left pull-right">');
                    htmls.push('<li><a href="#" data-id="' + poid + '" class="lnkClosePO">Tutup PO</a></li>');
                    htmls.push('</ul>');
                    htmls.push('</div>');


                    $(rows).eq(i).before(
                        '<tr class="group"><td colspan="6"><a href="/Purchasing/DeliveryOrder/DetailPO/' + poid + '">' + group + '</a></td>' +
                        '<td class="text-center">' + htmls.join("\n") + '</td>' +
                        '</tr>'
                    );

                    last = group;
                }
                //console.log(rdata);
            });

            $(".lnkDelete").click(_lnkDelete_click);
            $(".lnkClosePO").click(_lnkClosePO_click);
        } //end fnDrawCallback
        tblData = $('#tblData').DataTable(datatableDefaultOptions)
    }
    var _lnkDelete_click = function (e) {
        e.preventDefault();
        if (confirm("Hapus dokumen ini?") == false) {
            return;
        }
        var row = _getDataTableRow(this)
        $.ajax({
            url: '/Purchasing/DeliveryOrder/Delete',
            type: 'POST',
            data: { id: row.data().ID },
            success: function () {
                tblData.ajax.reload();
            },
            error: ajax_error_callback,
            datatype: 'json'
        });

    }



    var _lnkClosePO_click = function (e) {
        e.preventDefault();
        if (confirm("Tandai bahwa semua penerimaan telah lengkap?") == false) {
            return;
        }
        var poid = $(this).data("id");
        $.ajax({
            url: '/Purchasing/PurchaseOrder/ClosePO',
            type: 'POST',
            data: { id: poid },
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
