@ModelType Purchasing.PriceComparison
@Code
    ViewData("Title") = "Price Comparison"
    Dim defaultDateFormat = New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}}
End Code
@Using Html.BeginJUIBox("Daftar Permintaan Pembelian")
    @<form name="formcomparison" id="frmcomparison" class="form-horizontal" method="post"
    autocomplete="off">
    <input type="hidden" name="listItemComparison" id="ItemComparisonSend" value="" />
    <input type="hidden" name="listVendorID" id="ItemVendorID" value="" />
    <input type="hidden" name="idOffice" id="spOfficeId" value="" />
    <input type="hidden" name="norecordId" id="spIDNoRecord" value="" />
    @Html.HiddenFor(Function(m) m.PurchaseRequisitionID)
    @Html.WriteFormDateInputFor(Function(m) m.CreateDate, "Tanggal", defaultDateFormat,
                                   smControlWidth:=2, lgControlWidth:=2)
    @Html.WriteFormInput(Html.TextBox("RequestNumber", Nothing, New With {.class = "form-control"}), "No Permintaan")
    <div class="panel panel-primary">
        <div class="panel-heading">
            Price Comparison
        </div>
        <table id="tblcomparison" class="table table-bordered">
            <colgroup>
          <col style='width: 1px' />
            <col style='width: 200px' />
            <col style='width: 1px' />
            <col style='width: 100px' />
            <col style='width: auto' />
            <col style='width: 100px' />
            <col style='width: auto' />
            <col style='width: 100px' />
            <col style='width: auto' />
        </colgroup>
            <thead class="bg-default">
                <tr>
                    <th rowspan="2">
                        No.
                    </th>
                    <th rowspan="2">
                        Item
                    </th>
                    <th rowspan="2">
                        Kuantitas
                    </th>
                    <th colspan="2">
                        <a class="btn btn-default vendorpopover" data-toggle="popover">Vendor I</a>
                        <input type="hidden" class="vendorId" value="" />
                    </th>
                    <th colspan="2">
                        <a class="btn btn-default vendorpopover" data-toggle="popover">Vendor II</a>
                        <input type="hidden" class="vendorId" value="" />
                    </th>
                    <th colspan="2">
                        <a class="btn btn-default vendorpopover" data-toggle="popover">Vendor III</a>
                        <input type="hidden" class="vendorId" value="" />
                    </th>
                </tr>
                <tr>
                    <th>
                        Harga Dasar
                    </th>
                    <th>
                        Harga
                    </th>
                    <th>
                        Harga Dasar
                    </th>
                    <th>
                        Harga
                    </th>
                    <th>
                        Harga Dasar
                    </th>
                    <th>
                        Harga
                    </th>
                </tr>
            </thead>
            <tfoot>
            <tr>
                <th colspan="4" style="text-align:right">Total:</th>
                <th style="text-align:right"></th>
                <th colspan="2" style="text-align:right"></th>
                <th colspan="2" style="text-align:right"></th>
            </tr>
        </tfoot>
            @* <tfoot>
                <tr>
                    <th>Total</th>
                    <th class="totalest1"></th>
                    <th class="totalest2"></th>
                    <th class="totalest3"></th>
                </tr>
            </tfoot>*@
        </table>
       @* <table id="tbltotalestprice">
            <colgroup>
                <col style='width: 480px' />
                <col style='width: 200px' />
                <col style='width: 220px' />
            </colgroup>
            <tr>
                <td>
                    <strong>Total</strong>
                </td>
                <td id="totalprice1">
                    0
                </td>
                <td>
                    0
                </td>
                <td>
                    0   
                </td>
            </tr>
            <tfoot>
                
            </tfoot>
        </table>*@
    </div>
    <div class="well cell-center">
        <div class="center-block" style="width: 200px">
            <button type="button" class="btn btn-primary" id="btnsave">
                Simpan</button>
            <a href="@Url.Action("#", "#")" class="btn btn-default" >
                Batal</a>
        </div>
    </div>
    </form>
End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">
    var tablecomparison = null;
    var beginEstUnitPrice = 0;
    var _initcostpopover = function () {
        $('.EstUnitPricePopover').popover({
            title: 'Harga Item',
            placement: 'left',
            html: true,
            container: "#ajax-content",
            content: '<input type="text" name="EstUnitPrice" \
            id="id_EstUnitPrice" class="form-control" valye="" />'
        }).on('shown.bs.popover', function () {
            $('#id_EstUnitPrice').val($(this).text());
        }).
        on('hide.bs.popover', function () {
            beginEstUnitPrice = $('.popover #id_EstUnitPrice').val();
        }).
        on('hidden.bs.popover', function () {
            var tr = $(this).closest('tr');
            var row = tablecomparison.row(tr);

            var dataItem = row.data();
            dataItem.EstUnitPrice = beginEstUnitPrice;
            dataItem.TotalEstPrice = dataItem.EstUnitPrice * dataItem.Quantity;
            $(this).text(dataItem.EstUnitPrice);
            $(this).closest('td').next('td').text(dataItem.TotalEstPrice);
            var nexttdclass = $(this).closest('td').next('td').attr('class');

            //console.log(tablecomparison.columns(nexttdclass).data().eq(0).indexOf('Yes'));
        });

    }

    var _initDatatableDefaultOptions = function () {
        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.columnDefs = [];
        datatableDefaultOptions.order = [[1, "asc"]];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = false;
        //datatableDefaultOptions.destroy = false;


    }

    var _rendertotalEstprice = function (data, type, row) {

        if (type == 'display') {
            return data;
        }
        return data;

    }

    var _initTblItem = function () {
        var _renderEditColumnOp = function (data, type, row) {
            if (type == 'display') {
                return ('<div class=\'btn-group\' role=\'group\'>' +
                      '<button type=\'button\' class=\'btn btn-default btn-xs btnEditItemOperation\' ><i class=\'fa fa-edit\'></i></button>' +
                      '<button type=\'button\' class=\'btn btn-default btn-xs btRemoveItemOperation\'><i class=\'fa fa-remove\'></i></button>' +
                      '</div>');
            }
            return data;
        };


        var _rendertotalEstprice = function (data, type, row) {

            if (type == 'display') {
                return data;
            }
            return data;

        }


        var _renderpopovercost = function (data, type, row) {

            if (type == 'display') {
                return "<a href='javascript:void(0)' data-toggle='popover' class='EstUnitPricePopover'>" + data + "</a>";
            }
            return data;
        }
        var arrColumnsOp = [
            { 'data': 'PRDetailID', 'sClass': 'text-right' }, //
            {'data': 'ItemName' }, //
            {'data': 'Quantity', 'sClass': 'text-right' },
            { 'data': 'EstUnitPrice', 'sClass': 'text-right', 'mRender': _renderpopovercost },
            { 'data': 'TotalEstPrice', 'sClass': 'text-right' },
             { 'data': 'EstUnitPrice', 'sClass': 'text-right', 'mRender': _renderpopovercost },
            { 'data': 'TotalEstPrice', 'sClass': 'text-right' },
             { 'data': 'EstUnitPrice', 'sClass': 'text-right', 'mRender': _renderpopovercost },
            { 'data': 'TotalEstPrice', 'sClass': 'text-right'}//,
        //   { 'data': 'ID', 'mRender': _renderEditColumnOp}
        ];
        datatableDefaultOptions.aoColumns = arrColumnsOp;
        datatableDefaultOptions.ajax = function (data, callback, settings) {
            var _id = parseInt($('#spIDNoRecord').val());
            var _officeId = $('#spOfficeId').val();

            if (_id == 0 || isNaN(_id))
            { return; }

            $.ajax({
                url: '/Purchasing/PriceComparisson/GetItemToPriceComparison',
                data: { id: _id, officeid: _officeId },
                type: 'POST',
                success: callback,
                datatype: 'json'
            });

        };
//        datatableDefaultOptions.initComplete = function (Settings, json) {
//            this.api().columns(4).every(function () {
//                var column = this;
//                var sum = column
//                .data()
//                .reduce(function (a, b) {
//                    return parseInt(a, 10) + parseInt(b, 10);
//                });

//                $(column.footer()).html(sum);
//                console.log(sum);
//            });
//        };
        datatableDefaultOptions.fnDrawCallback = function (oSettings, data) {
            _fnlocalDrawCallback(oSettings);
            _initcostpopover();
            for (var i = 0, iLen = oSettings.aiDisplay.length; i < iLen; i++) {
                $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html((i + 1) + '.');
            }
        };
        
                datatableDefaultOptions.footerCallback = function (tfoot, data, start, end, display) {
                    
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    var api = this.api();

                    $(api.column(4).footer()).html(api.column(4).data().reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0)); //Total Price Vendor I


                    $(api.column(6).footer()).html(api.column(6).data().reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0)); //Total Price Vendor 2


                    $(api.column(8).footer()).html(api.column(8).data().reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0)); //Total Price Vendor 3
                };

        tablecomparison = $('#tblcomparison').DataTable(datatableDefaultOptions);

    }

    var contentpopover = '<input type="text" name="VendorVal" \
    id="vendorinput" class="form-control vendorautocom" />';
    var tempvendor, idvendor;

    //autocomplete vendor
    var _initautocompleteVendor = function () {
        $('.vendorautocom').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/Purchasing/PriceComparisson/autocompleteVendor',
                    type: 'POST',
                    data: {
                        term: $('.vendorautocom').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.name,
                                value: obj.name,
                                id: obj.id
                            }
                        }));
                    }
                });
            },
            change: function (event, ui) {
                tempvendor = ui.item.label;
                idvendor = ui.item.id;
            }
        });
        //});
    }
    $(document).ready(function () {

        _initcostpopover();
        $('.vendorpopover').popover({
            title: 'Tambah Vendor',
            placement: 'left',
            html: true,
            content: contentpopover
        }).
        on('shown.bs.popover', function () {
            _initautocompleteVendor();
            $('.vendorautocom').val($(this).text());
            tempvendor = $('.vendorautocom').val();
        }).
        on('hidden.bs.popover', function () {
            $(this).text(tempvendor);
            $(this).next('.vendorId').val(idvendor);
        });
        //end popover
        _initTblItem(); //initialize table item

        $('#RequestNumber').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/Purchasing/PriceComparisson/autocompleteRequestNumber',
                    type: 'POST',
                    data: {
                        term: $('#RequestNumber').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.Norecord,
                                value: obj.Norecord,
                                ID: obj.ID,
                                OfficeId: obj.officeId

                            }
                        }));
                    }
                });
            },
            change: function (event, ui) {
                if (ui.item.OfficeId == null) {
                    $('#spOfficeId').val(0);
                    $('#spIDNoRecord').val(ui.item.ID);

                } else {
                    $('#spOfficeId').val(ui.item.OfficeId);
                    $('#spIDNoRecord').val(ui.item.ID);
                }
                tablecomparison.ajax.reload();

            }
        }).data('ui-autocomplete')._renderItem = function (ul, item) {
            //location
            return ($('<li>').append('<a><strong>' + item.label + '</strong>').appendTo(ul));
        };


        //hide error sedang memproses datatable
        $('.dataTables_processing').hide();

        $('#btnsave').click(function (e) {
            var _dataItemPriceComparison = tablecomparison.data();
            var _arrvendorid = []
            $('.vendorId').each(function () {
                _arrvendorid.push($(this).val());
            });
            var _dataItemPriceComparisonSend = []
            $(_dataItemPriceComparison).each(function (d, e) {
                _dataItemPriceComparisonSend.push(e);
            });
            $('#ItemVendorID').val(JSON.stringify(_arrvendorid));
            $('#ItemComparisonSend').val(JSON.stringify(_dataItemPriceComparisonSend));
            var _data = $('#frmcomparison').serializeArray();
            $.ajax({
                type: 'POST',
                data: _data,
                url: '/Purchasing/PriceComparisson/SaveItemPriceComparison',
                dataType: 'json',
                success: submitFormActivityCallback
            });
        });

    });

</script>
