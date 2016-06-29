@Code
    ViewData("Title") = "Daftar Permintaan Pembelian"
End Code
@Using Html.BeginJUIBox("Daftar Permintaan Pembelian")
    @<div class="table-responsive">
	<table id="tbRequisition" class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer">
        <colgroup>
            <col style='width: 60px' />
            <col style='width: 120px' />
            <col style='width: 180px' />
            <col style='width: 120' />
            <col style='width: auto' />
            <col style='width: 80px' />
            <col style='width: 60px' />
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
                <th>Tipe Pembelian</th>
             
                <th>
                    Dikirim ke
                </th>
                <th>
                    Status
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
    @html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(Purchasing.GlobalArray.PurchaseRequisitionDocState))
    @Html.Raw(";")
    @html.Raw("</script>")
    
End Code
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">
    var tbRequisition = null;
    var _initTblRequistion = function () {


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
           
                htmls.push("<a role='button' data-target='#' href='/Purchasing/DepartmentPurchasingRequestApproval/Detail/" + data +
                "' title='Detail view' class='btn btn-primary'><span class='fa fa-arrow-right'></span> Detail</a>");


                return htmls.join("\n");
            }
            return data;
        }
        var _localLoad = function (data, callback, setting) {
            $.ajax({
                url: '/Purchasing/DepartmentPurchasingRequestApproval/GetRequisitionList',
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
              {"data": "RequestTypeName" }, //3
              {"data": "RequestedBy_Name", "mRender": _renderRequestedBy }, //4
              {"data": "DocState", "sClass": "text-center", "mRender": _renderStatus }, //5
              {"data": "ID", "sClass": "text-center", "mRender": _renderDetail}//6

        ];

        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [{ "targets": [0, 6], "orderable": false}];
        datatableDefaultOptions.order = [[1, "asc"]];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = true;
        datatableDefaultOptions.ajax = _localLoad;
        tbRequisition = $('#tbRequisition').DataTable(datatableDefaultOptions)
    }         //end _initTblRequistion

    $(function () {

        _initTblRequistion();

    }); //end init;

</script>
