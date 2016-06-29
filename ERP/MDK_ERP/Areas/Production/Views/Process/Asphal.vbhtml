@Code
    ViewData("Title") = "Pemakaian Aspal"
    ViewBag.headIcon = "icon-road"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Production", "/Production"}, _
                                             {"Material", "/Production/Stock"}, _
                                             {"Pemakaian Aspal", Nothing}
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

<div class="row">
    <div class="form form-horizontal">
        <div class="col-xs-6 col-sm-12 col-md-12 col-lg-7">
        <div class="form-group">
            <label class="col-md-2 col-sm-2 col-xs-12 text-left control-label">Periode</label>
            <div class="col-md-3 col-sm-4">
                <div class='input-group date' id='datetimepicker'>
                    <input type='button' class="form-control" placeholder="DD-MM-YYYY" />
                    <span class="input-group-addon">
                        <span class="icon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-4 col-sm-4 col-xs-12 col-md-offset-2 col-sm-offset-2">
                <a onclick="" id="filterBtn" href="javascript:void(0)" class="btn btn-primary">Filter</a>
                <span class="loader hidden"></span>
            </div>
        </div>
        </div>
    </div>
</div>

<a data-toggle="modal" id="" data-target="#modal-Data" class="btn btn-sm btn-success pull-right" href="javascript:void(0)" style="margin-right: 9px;margin-top: -50px;">
	<i class="icon-plus"></i> Tambah Baru
</a>
<div class="clear"></div>

@Using Html.RowBox("box-table")
    @<table id="tb_Data" class="display" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th>
                    Tanggal
                </th>
                <th>
                    Jumlah
                </th>
                @*<th>
                    Produk
                </th>*@
                <th>
                    Operator
                </th>
                <th class="action">
                </th>
            </tr>
        </thead>
    </table>
End Using
@Using Html.ModalForm("modal-Data", "Form Pemakaian", Url.Action("asphalSave", "Process"), "", "POST", "form-Data", "form-horizontal")
    @<div class="form-group">
    @Html.Hidden("ID")
        <label class="col-md-3 control-label">
            @Html.Hidden("IDMaterial", ViewData("IDaspal"))
            Tanggal
        </label>
        <div class="col-md-3 col-sm-4">
            <div class='input-group date' id='dtDate'>
                @Html.TextBox("DateUse", Nothing, New With {.class = "form-control"})
                <span class="input-group-addon"><span class="icon-calendar"></span></span>
            </div>
        </div>
    </div>
    @<div class="form-group">
        <label class="col-md-3 control-label">
            Operator
        </label>
        <div class="col-md-3 col-sm-4">
            @Html.TextBox("OperatorName", Nothing, New With {.class = "form-control"})
            @Html.Hidden("IdMachine", ViewData("IDMesin"))
        </div>
    </div>
    @<div class="form-group">
        <label class="col-md-3 control-label">
            Shift
        </label>
        <div class="col-sm-4">
            @Html.DropDownList("IdShift", New SelectList(DirectCast(ViewData("sifht"), IEnumerable), "ID", "Shift"), "", New With {.class = "form-control select2"})
        </div>
    </div>
    @<div class="form-group">
        <label class="col-md-3 control-label">
            Produk
        </label>
        <div class="col-sm-4">
            @Html.DropDownList("IDProduk", New SelectList(DirectCast(ViewData("aspal"), IEnumerable), "ID", "Name"), "", New With {.class = "form-control select2"})
        </div>
    </div>
    @<div class="form-group">
        <label class="col-md-3 control-label">
            Jumlah
        </label>
        <div class="col-sm-2">
            @Html.TextBox("Amount", 0, New With {.class = "form-control", .placeholder = 0})
        </div>
        <label class="col-sm-1 control-label">
            Kg</label>
    </div>
End Using
@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.css")" />
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/Select2/select2.css")" />
End Section
@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/Select2/select2.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Scripts/shared-function.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            datePicker_ByMonth('#datetimepicker');
            GetList();
            datePicker_ByDate('#dtDate');
            $('#form-data').submit(function () {
                fnInsert('#form-data', '#modal-Data');
            });
            $('[data-toggle="modal"]').click(function () {
                fnReset();
                $('#ID').remove();
            });

        });
        GetList = function () {
            var attr = _attrCRUD()
            attr.url = {
                "Read": "Production/Process/getListAsphal",
                "Delete": "Production/Process/deleteSolar"
            };
            attr.dataTable.columns = [
                { "data": "DateUse" },
                { "data": "Amount" },
                { "data": "OperatorName" },
                {
                    "bSortable": false,
                    "defaultContent": "" +
                      "<div align='center'><button id='Edit' class='icon-edit btn btn-primary btn-xs' data-toggle='tooltip' data-placement='bottom' title='Edit'></button>" +
                      "<button id='Delete' class='icon-remove btn btn-danger btn-xs' data-toggle='tooltip' data-placement='right' title='Hapus'></button></div>"
                }
            ];
            attr.dataTable.ajax.data = function (d) {
                var arrDt = splitMonYear($("#datetimepicker > input").val());
                d.MonthVal = arrDt[0];
                d.YearVal = arrDt[1];
            };
            attr.dataTable.columnDefs = [
                {
                    "render": function (data, type, row) {
                        return formatShortDate(data);
                    },
                    "targets": 0
                }
            ]
            fnGetList(attr);
        }
        
    </script>
End Section

