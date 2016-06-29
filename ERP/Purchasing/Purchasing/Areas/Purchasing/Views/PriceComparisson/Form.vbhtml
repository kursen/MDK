@ModelType Purchasing.PriceComparison
@Code
    ViewData("Title") = "Price Comparison"
    Dim defaultDateFormat = New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}}
End Code
@Using Html.BeginJUIBox("Daftar Permintaan Pembelian")
    @<form name="formcomparison" id="frmcomparison" class="form-horizontal" method="post"
    autocomplete="off">
    @*@Html.Hidden("PurchaseRequisitionID", 0, New With {.id = "spIDNoRecord"})*@
    @Html.HiddenFor(Function(m) m.PurchaseRequisitionID, New With {.id = "spIDNoRecord"})
    @Html.Hidden("VendorID", 0)
    @Html.Hidden("Ordinal", 0)
    <!--list arr send to controller -->
    <input type="hidden" id="ItemVendor" name="ItemVendorID" value="" />
    <input type="hidden" name="listItemComparison" id="ItemComparisonSend" value="" />
    @Html.WriteFormDateInputFor(Function(m) m.CreateDate, "Tanggal", defaultDateFormat,
                                   smControlWidth:=2, lgControlWidth:=2)
    @Html.WriteFormInput(Html.TextBox("NoRecord", Nothing, New With {.class = "form-control"}), "No Permintaan")
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
                        <input type="hidden" class="vendorsId" value="" />
                    </th>
                    <th colspan="2">
                        <a class="btn btn-default vendorpopover" data-toggle="popover">Vendor II</a>
                        <input type="hidden" class="vendorsId" value="" />
                    </th>
                    <th colspan="2">
                        <a class="btn btn-default vendorpopover" data-toggle="popover">Vendor III</a>
                        <input type="hidden" class="vendorsId" value="" />
                    </th>
                </tr>
                <tr>
                    <th>
                        Harga Dasar
                    </th>
                    <th>
                        Total
                    </th>
                    <th>
                        Harga Dasar
                    </th>
                    <th>
                        Total
                    </th>
                    <th>
                        Harga Dasar
                    </th>
                    <th>
                        Total
                    </th>
                </tr>
            </thead>
            <tfoot>
                <tr>
                    <th colspan="4" style="text-align: right">
                        Total:
                    </th>
                    <th style="text-align: right">
                    </th>
                    <th colspan="2" style="text-align: right">
                    </th>
                    <th colspan="2" style="text-align: right">
                    </th>
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
            <a href="@Url.Action("Index", "DepartmentPurchaseRequisition")" class="btn btn-default" >
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

    //popover harga
    var _initcostpopover = function () {
        $('.EstUnitPricePopover').popover({
            title: 'Harga Item',
            placement: 'left',
            html: true,
            container: "#ajax-content",
            content: '<input type="text" name="EstUnitPrice" \
            id="id_EstUnitPrice" class="form-control" value="" /><small style="color:red;"></small>'
        }).on('shown.bs.popover', function () {
            $('#id_EstUnitPrice').val($(this).text());
        }).
        on('hide.bs.popover', function () {
            beginEstUnitPrice = parseInt($('.popover #id_EstUnitPrice').val());
            if (isNaN(beginEstUnitPrice)) {
                $('.popover').find('#id_EstUnitPrice').next('small').text('harga tidak boleh kosong !').hide().show('slow').fadeout();
                return false;
            }
        }).
        on('hidden.bs.popover', function () {
            var tr = $(this).closest('tr');
            var row = tablecomparison.row(tr);
            var rowindex = row.index();
            var rowdata = row.data();
            //
            var td = $(this).closest('td');
            var cell = tablecomparison.cell(td);
            var dataItem = cell.data();
            dataItem = beginEstUnitPrice;



            var totalestprice = beginEstUnitPrice * rowdata.Quantity;

            $(this).text(beginEstUnitPrice);
            td.next('td').text(totalestprice);

            var classtd1 = td.hasClass('Price1');
            var classtd2 = td.hasClass('Price2');
            if (classtd1) {
                rowdata.Price1 = beginEstUnitPrice;
                rowdata.TotalEstPrice1 = totalestprice;
            } else if (classtd2) {
                rowdata.Price2 = beginEstUnitPrice;
                rowdata.TotalEstPrice2 = totalestprice;
            } else {
                rowdata.Price3 = beginEstUnitPrice;
                rowdata.TotalEstPrice3 = totalestprice;
            }
            tablecomparison.draw();

        });

    }

    var _initDatatableDefaultOptions = function () {
        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.columnDefs = [];
        // datatableDefaultOptions.order = [[1, "asc"]];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = false;
        //datatableDefaultOptions.destroy = false;


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
            { 'data': 'Price1', 'sClass': 'text-right Price1', 'mRender': _renderpopovercost },
            { 'data': 'TotalEstPrice1', 'sClass': 'text-right' },
             { 'data': 'Price2', 'sClass': 'text-right Price2', 'mRender': _renderpopovercost },
            { 'data': 'TotalEstPrice2', 'sClass': 'text-right' },
             { 'data': 'Price3', 'sClass': 'text-right Price3', 'mRender': _renderpopovercost },
            { 'data': 'TotalEstPrice3', 'sClass': 'text-right'}//,
        //   { 'data': 'ID', 'mRender': _renderEditColumnOp}
        ];
        datatableDefaultOptions.aoColumns = arrColumnsOp;
        datatableDefaultOptions.ajax = function (data, callback, settings) {
            var _id = parseInt($('#spIDNoRecord').val());

            if (_id == 0 || isNaN(_id))
            { return; }

            $.ajax({
                url: '/Purchasing/PriceComparisson/GetItemToPriceComparison',
                data: { id: _id },
                type: 'POST',
                success: callback,
                datatype: 'json'
            });

        };

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
    id="vendorinput" class="form-control vendorautocom" /><small style="font-size: 10px;color:red;"></small>';
    var tempvendor, idvendor, nextidvendor;
    //
    var _initautocompletenoRecord = function () {
        $('#NoRecord').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/Purchasing/PriceComparisson/autocompleteRequestNumber',
                    type: 'POST',
                    data: {
                        term: $('#NoRecord').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.Norecord,
                                value: obj.Norecord,
                                ID: obj.ID


                            }
                        }));
                    }
                });
            },
            change: function (event, ui) {
                $('#spIDNoRecord').val(ui.item.ID);
                tablecomparison.ajax.reload();

            }
        }).data('ui-autocomplete')._renderItem = function (ul, item) {
            //location
            return ($('<li>').append('<a><strong>' + item.label + '</strong>').appendTo(ul));
        };
    }
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
                if (ui.item != null) {
                    tempvendor = ui.item.label;
                    idvendor = ui.item.id;
                }

            }
        });
        //});
    }

    var submitFormActivityCallback = function (data) {
        if (data.stat == 0) {
            showNotificationSaveError(data);
            return;
        }
        else if (data.stat == 2) {
            //data already there
            showNotificationSaveError(data);
        } else {
            window.location.href = '/Purchasing/PriceComparisson/Detail/'+data.id;
        }
    };

    //validation estimation price
    $(document).on('keypress', '#id_EstUnitPrice', function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            //display error message
            $(this).next('small').text("Hanya Angka!").show().fadeOut("slow");
            return false;
        }
    });
    var arridvendor = [];
    $(document).ready(function () {
        //_initTblItem();
        if ($('#NoRecord').val() == null || $('#NoRecord').val() == '') {
            //initialize table item
            _initTblItem();
            _initautocompletenoRecord();
        }
        else {
            $('#NoRecord').attr('readonly', true);
            _initTblItem();

        }

        _initcostpopover();
        $('.vendorpopover').popover({
            title: 'Tambah Vendor',
            placement: 'left',
            html: true,
            content: contentpopover
        }).
        on('show.bs.popover', function () {
            //$(this).closest('td').next('button').attr('disabled', true);
        }).
        on('shown.bs.popover', function () {
            _initautocompleteVendor();
            $('.vendorautocom').val($(this).text());
            tempvendor = $('.vendorautocom').val();
        }).
        on('hide.bs.popover', function (e) {
            for (var i = 0; i < arridvendor.length; i++) {
                if (idvendor == arridvendor[i]) {
                    $('.popover #vendorinput').next('small').text('vendor sudah ada!!').hide().show('slow');
                    return false;
                }
            }
        }).
        on('hidden.bs.popover', function () {
            $(this).text(tempvendor);
            $(this).next('.vendorsId').val(idvendor);
            arridvendor.push(idvendor);
        });


//        $(document).on('click', '.vendorpopover', function (e) {
//            var td = $(this).closest('td');
//            td.find('.vendorpopover').attr('disabled', true);
//        });
        //end popover




        //hide error sedang memproses datatable
        $('.dataTables_processing').hide();

        $('#btnsave').click(function (e) {
            var _arrvendoridsend = [];
            var _dataItemPriceComparison = tablecomparison.data();
            var _dataItemPriceComparisonSend = [];

            $('.vendorsId').each(function (d, e) {
                var _objvendorid = {};
                var vendorIdVal = parseInt($(this).val());
                if (isNaN(vendorIdVal)) {
                    return;
                } else {
                    _objvendorid = { VendorID: vendorIdVal };

                }
                _arrvendoridsend.push(_objvendorid);
            });

            // for (var i = 0; i < _arrvendoridsend.length; i++) {
            $(_dataItemPriceComparison).each(function (d, e) {
                var _objdataItemComparison = {
                    PRDetailID: e.PRDetailID,
                    Price: [e.Price1, e.Price2, e.Price3]
                };
                _dataItemPriceComparisonSend.push(_objdataItemComparison);
            });
            // }



            $('#ItemVendor').val(JSON.stringify(_arrvendoridsend));
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
