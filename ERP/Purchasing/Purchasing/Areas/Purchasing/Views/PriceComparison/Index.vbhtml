@ModelType Purchasing.PriceComparison
@Code
    ViewData("Title") = "Perbandingan Harga"
    Dim Months = {"Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "Nopember", "Desember"}
    Dim opt = New List(Of ERPBase.OptionItem)
    
    For i As Integer = 0 To Months.Count-1
        opt.Add(New ERPBase.OptionItem With {.Text = Months(i), .Value = i + 1})
    Next
    Dim monthFilter = New SelectList(opt, "Value", "Text", Date.Today.Month)
End Code
@Using Html.BeginJUIBox("Perbandingan Harga")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="pull-right">
                <a href="/Purchasing/PriceComparison/Create" class="btn btn-primary" id="btnAdd">Tambah
                    Data</a>
            </div>
        </div>
    </div>
    @<div class="row">
    <div class="col-lg-12 col-sm-12">
        @Using Html.BeginForm("GetComparisonList", "PriceComparison", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .id = "frmfilter", .autocomplete = "off"})
        @Html.WriteFormInput(Html.DropDownList("fMonth",monthFilter ,Nothing,New With{.class="form-control"}),"Bulan")
            @Html.WriteFormInput(Html.YearInput("fYear", Date.Today.Year), "Tahun", lgControlWidth:=1)
        @Html.WriteFormInput(New MvcHtmlString("<button class='btn btn-primary' id='btnView' type='button'>Lihat</button>"),"")
            
    End Using
    </div>
    
    </div>
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="table-responsive">
                <table class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer"
                    id="tblData">
                    <colgroup>
                    <col style='width:40px' />
                    <col style='width:120px' />
                    <col style='width:auto' />
                    <col style='width:120px' />
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
                                No Permintaan
                            </th>
                            <th>
                                Detail
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>

End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">
    var tblData = null;
    var _renderDetail = function (data, type, row) {
        if (type == 'display') {
            var htmls = new Array();
            htmls.push('<div class="btn-group">');
            htmls.push("<a role='button' data-target='#' href='/Purchasing/PriceComparison/Detail/" + data +
                "' title='Detail view' class='btn btn-primary'><span class='fa fa-arrow-right'></span> Detail</a>");
            htmls.push('<button type="button" class="btn btn-danger dropdown-toggle" ');
            htmls.push('data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">');
            htmls.push('<span class="caret"></span>');
            htmls.push('<span class="sr-only">Action</span>');
            htmls.push('</button>');
            htmls.push('<ul class="dropdown-menu text-left pull-right">');
            htmls.push('<li><a href="/Purchasing/PriceComparison/Edit/' + data + '">Edit</a></li>');
            htmls.push('<li><a href="#" class="lnkDelete">Hapus</a></li>');
            htmls.push('</ul>');
            htmls.push('</div>');


            return htmls.join("\n");
        }
        return data;
    }

    var _lnkDelete_click = function (e) {
        e.preventDefault();
        if (confirm("Hapus dokumen ini?") == false) {
            return;
        }
        var row = _getDataTableRow(this)
        $.ajax({
            url: '/Purchasing/PriceComparison/Delete',
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

    var _localLoad = function (data, callback, setting) {
        var _data = { "m": $("#fMonth").val(), "y": $("#fYear").val() }
        var _url = $("#frmfilter").attr("action");
        $.ajax({
            url: _url,
            data: _data,
            type: 'POST',
            success: callback,
            error: ajax_error_callback,
            datatype: 'json'
        });
    };

    var _initTableData = function () {

        var arrColumns = [
         { "data": "ID" },
         { "data": "CreateDate", "sClass": "text-center", "mRender": _fnRenderNetDate },
         { "data": "RecordNo" },
         { "data": "ID", "sClass": "text-center", "mRender": _renderDetail }
        ];

        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [{ "targets": [0,3], "orderable": false}];
        datatableDefaultOptions.order = [[1, "asc"]];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = true;
        datatableDefaultOptions.ajax = _localLoad;
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            _fnlocalDrawCallback(oSettings);
            $(".lnkDelete").click(_lnkDelete_click);
        }

        tblData = $("#tblData").DataTable(datatableDefaultOptions);
    };

    $(function () { //init document
        _initTableData();

        $("#btnView").click(function () {
            tblData.ajax.reload();
        }); //btnView.click
    });   //end init document;

</script>
