@ModelType List(Of Purchasing.DepartmentPurchaseRequisition)
@Code
    ViewData("Title") = "Daftar Permintaan Pembelian " & ViewData("officeName")
End Code
<div class="row">
    <div class="col-xs-12">
        <div class="pull-right">
            <a href="@Url.Action("Create", "DepartmentPurchaseRequisition")" class="btn btn-sm btn-success  btn-add btn-label-left" >
                <span><i class="fa fa-plus"></i></span>Tambah Baru</a>
        </div>
    </div>
</div>
@Using Html.BeginJUIBox("Daftar Permintaan Pembelian " & ViewData("officeName"))
    @<div class="table-responsive">
	<table id="tbRequisition" class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer">
        <colgroup>
            <col style='width: 60px' />
            <col style='width: 120px' />
            <col style='width: 180px' />
            <col style='width: auto' />
            <col style='width: 80px' />
            <col style='width: 60px' />
            <col style='width: 120px' />
        </colgroup>
        <thead>
            <tr>
                <th>
                    No.
                </th>
                <th>
                    Tanggal
                </th>
                <th>
                    No Dokumen
                </th>
                <th class='hidden-xs'>
                    Diminta Oleh
                </th>
                <th>Jenis</th>
                <th>
                    Status
                </th>
                  <th class='hidden-xs'>
                    Items
                </th>
                <th class="action">
                    Detail
                </th>
            </tr>
        </thead>
        <tbody>
    </table>
	</div>
End Using
@Code
    @Html.Raw("<script type='text/javascript'>")
    
    @Html.Raw("var arrDocState=")
    @Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(Purchasing.GlobalArray.PurchaseRequisitionDocState))
    @Html.Raw(";")
    @Html.Raw("</script>")
    
End Code
<style>
    body .popover
    {
        width: 700px;
        max-width: 100%;
    }
</style>
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrapPaging.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">
    var tbRequisition = null;

    var _initDetailPopover = function () {
        $('[data-toggle="popover"]').popover({
            title: "Detail",
            container: "#ajax-content",
            html: true,
            placement: "left",
            content: function () {

                var tr = $(this).closest('tr');
                var row = tbRequisition.row(tr);
                var dataItem = row.data();
                var total = 0;
                var _html = "<table class='table table-bordered table-heading table-responsive' style='width:100%'>"
                    + "<thead><tr><th>#</th><th>Item</th><th>Alokasi</th><th colspan='2'>Kuantitas</th><th>Harga</th><th>Total</th></tr></thead><tbody>";
                for (var i = 0; i < dataItem.Details.length; i++) {
                    _html += "<tr><td class='text-right'>" + (i + 1) + ".</td><td>" + dataItem.Details[i].ItemName + "</td>"
                        + "<td>" +  (dataItem.Details[i].Allocation==null?"": dataItem.Details[i].Allocation) + "</td>"
                        + "<td class='text-right'>" + $.number(dataItem.Details[i].Quantity, 2, ",", ".") + "</td>"
                        + "<td>" + dataItem.Details[i].UnitQuantity + "</td>"
                        + "<td class='text-right'>" + $.number(dataItem.Details[i].UnitPrice, 2, ".", ",") + "</td>"
                        + "<td class='text-right'>" + $.number(dataItem.Details[i].TotalPrice, 2, ".", ",") + "</td>"
                        + "</tr>";
                    total += dataItem.Details[i].TotalPrice;
                }
                _html += "</tbody>"
                _html += "<tfoot><tr><td colspan='6'>Total</td><td class='text-right'>" + $.number(total, 2, ",", ".") + "</td></tr></tfoot>";
                _html += "</table>";
                ;
                return _html;

            }

        });
        $('[data-toggle="popover"]').on("click", function (e) {
            $('[data-toggle="popover"]').not(this).popover("hide");
        });
    }

    var _initTblRequistion = function () {

        var _renderIconItem = function (data, type, row) {
            if (type == 'display') {
                return "<a href='#' data-toggle='popover' class='btn btn-info'><span class='fa fa-list'></span></a>";
            }
            return data;
        }
        var _renderRequestedBy = function (data, type, row) {
            if (type == 'display') {
                return (row.RequestedBy_Name + ' (' + row.RequestedBy_Occupation + ')');
            }
            return data;
        }

        var _renderStatus = function (data, type, row) {
            if (type == 'display') {

                var arrValue = arrDocState;

                return arrValue[data];
            }
            return data;
        };

        var _renderDetail = function (data, type, row) {
            if (type == 'display') {
                var htmls = new Array();
                htmls.push('<div class="btn-group">');
                htmls.push("<a role='button' data-target='#' href='/Purchasing/DepartmentPurchaseRequisition/Detail/" + data +
                "' title='Detail view' class='btn btn-primary'><span class='fa fa-arrow-right'></span> Detail</a>");
                htmls.push('<button type="button" class="btn btn-danger dropdown-toggle" ');
                htmls.push('data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">');
                htmls.push('<span class="caret"></span>');
                htmls.push('<span class="sr-only">Action</span>');
                htmls.push('</button>');
                htmls.push('<ul class="dropdown-menu text-left pull-right">');
                if (row.DocState <= 1) {
                    htmls.push('<li><a href="/Purchasing/DepartmentPurchaseRequisition/Edit/' + data + '">Edit</a></li>');
                }
                htmls.push('<li><a href="#" class="lnkDelete">Hapus</a></li>');
                htmls.push('<li role="separator" class="divider"></li>');
                htmls.push('<li><a href="#" class="lnkArchive">Simpan ke Arsip</a></li>');
                htmls.push('</ul>');
                htmls.push('</div>');


                return htmls.join("\n");
            }
            return data;
        }
        var _localLoad = function (data, callback, setting) {
            $.ajax({
                url: '/Purchasing/DepartmentPurchaseRequisition/GetRequisitionList',
                type: 'POST',
                success: callback,
                error: ajax_error_callback,
                datatype: 'json'
            });
        };
        var arrColumns = [
              { "data": "ID", "sClass": "text-right" }, //0
              {"data": "RequestDate", "sClass": "text-center", "mRender": _fnRenderNetDate }, //1
              {"data": "RecordNo" }, //2
              {"data": "RequestedBy_Name", "sClass": "hidden-xs", "mRender": _renderRequestedBy }, //3
              {"data": "RequestTypeName", "sClass": "hidden-xs" }, //3
              {"data": "DocState", "sClass": "text-center", "mRender": _renderStatus }, //4
              {"data": "ID", "sClass": "text-center hidden-xs", "mRender": _renderIconItem }, //5
              {"data": "ID", "sClass": "text-center", "mRender": _renderDetail}//6

        ];

        //datatableDefaultOptions.serverSide = true;
        datatableDefaultOptions.searching = false;
        //datatableDefaultOptions.paging = true;
        //datatableDefaultOptions.pageLength = 50;
        //datatableDefaultOptions.pagingType = "bootstrap";
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [{ "targets": [0, 5], "orderable": false}];
        datatableDefaultOptions.order = [[1, "asc"]];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = true;
        datatableDefaultOptions.ajax = _localLoad;
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            _fnlocalDrawCallback(oSettings);
            _initDetailPopover();
            $(".lnkDelete").click(_lnkDelete_click);
            $(".lnkArchive").click(_lnkArchive_click);
        }
        tbRequisition = $('#tbRequisition').DataTable(datatableDefaultOptions);

    } //end _initTblRequistion

    var _lnkDelete_click = function (e) {
        e.preventDefault();
        if (confirm("Hapus dokumen ini?") == false) {
            return;
        }
        var row = _getDataTableRow(this)
        $.ajax({
            url: '/Purchasing/DepartmentPurchaseRequisition/DeleteDocument',
            type: 'POST',
            data: { id: row.data().ID },
            success: function () {
                tbRequisition.ajax.reload();
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
            url: '/Purchasing/DepartmentPurchaseRequisition/Archive',
            type: 'POST',
            data: { id: row.data().ID },
            success: function () {
                tbRequisition.ajax.reload();
            },
            error: ajax_error_callback,
            datatype: 'json'
        });

    }


    var _getDataTableRow = function (obj) {
        return tbRequisition.row($(obj).closest('tr'));
    }

    $(function () {

        _initTblRequistion();

    }); //end init;

</script>
