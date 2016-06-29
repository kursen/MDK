@Code
    ViewData("Title") = "Pemakaian Solar"
    ViewBag.headIcon = "icon-tint"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Production", "/Production"}, _
                                             {"Material", "/Production/Stock"}, _
                                             {"Pemakaian Solar", Nothing}
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers
<div class="row">
    <div class="form form-horizontal">
        <div class="col-xs-6 col-sm-12 col-md-12 col-lg-7">
        <div class="form-group">
            <label class="col-md-2 col-sm-2 col-xs-12 text-left control-label">Periode</label>
            <div class="col-md-4 col-sm-4 col-xs-8">
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

<a data-toggle="modal" id="addNew" data-target="#modal-Data" class="btn btn-sm btn-success pull-right" href="javascript:void(0)" style="margin-right: 9px;margin-top: -50px;">
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
                <th>
                    Mesin
                </th>
                <th>
                    Operator
                </th>
                <th class="action">
                </th>
            </tr>
        </thead>
    </table>
End Using
@Using Html.ModalForm("modal-Data", "Form Pemakaian", Url.Action("solarSave", "Process"), "", "POST", "form-Data", "form-horizontal")
    @<div class="form-group">
        <label class="col-md-3 control-label">
        @Html.Hidden("ID")
            @Html.Hidden("IDMaterial", ViewData("solar"))
            Tanggal
        </label>
        <div class="col-sm-4">
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
        <div class="col-sm-5">
            @Html.TextBox("OperatorName", Nothing, New With {.class = "form-control"})
        </div>
    </div>
  
    @<div class="form-group">
        <label class="col-md-3 control-label">
            Mesin
        </label>
        <div class="col-sm-4">
        @*@Html.Hidden("MachineName")*@
            @Html.DropDownList("IdMachine", New SelectList(DirectCast(ViewData("mesin"), IEnumerable), "ID", "MachineName"), "", New With {.class = "form-control select2"})
        </div>
    </div>
    @<div class="form-group">
        <label class="col-md-3 control-label">
            Jumlah
        </label>
        <div class="col-sm-2">
            @Html.TextBox("Amount", 0, New With {.class = "form-control", .placeholder = 0})
        </div>
       @* @Html.Hidden("IDMeasurementUnit",ViewData("Unit"))
        @Html.Hidden("IDInventory", ViewData("inventory"))*@
        <label class="col-sm-1 control-label">
            Liter</label>
    </div>
End Using
@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.css")" />
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/Select2/select2.css")" />
End Section
@Section jsScript
    @*<script type="text/javascript" src="@Url.Content("~/Content/Plugin/DataTables/js/jquery.dataTables.forHelper.js")"></script>*@
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/Select2/select2.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            datePicker_ByMonth('#datetimepicker');
            GetList();
            datePicker_ByDate('#dtDate');
            $('#form-data').submit(function () {
                fnInsert('#form-data', '#modal-Data');
            });

        });
        GetList = function () {
            var attr = _attrCRUD()
            attr.url = {
                "Read": "Production/Process/getListSolar",
                "Delete": "Production/Process/deleteSolar"
            };
            attr.dataTable.ajax.data = function (d) {
                var arrDt = splitMonYear($("#datetimepicker > input").val());
                d.MonthVal = arrDt[0];
                d.YearVal = arrDt[1];
            };
            attr.dataTable.columns = [
                {
                    "data": "DateUse",
                    "className":"td-center"
                },
                {
                    "data": "Amount",
                    "className": "td-right"
                },
                { 
                    "data": "MachineName" ,
                    "className":"td-center"
                },
                {
                    "data": "OperatorName",
                    "className":"td-center"
                },
                {
                    "bSortable": false,
                    "defaultContent": "" +
                      "<div align='center'><button id='Edit' class='icon-edit btn btn-primary btn-xs' data-toggle='tooltip' data-placement='bottom' title='Edit'></button>\
                      <button id='Delete' class='icon-remove btn btn-danger btn-xs' data-toggle='tooltip' data-placement='right' title='Hapus'></button></div>"
                }
            ];
            attr.dataTable.columnDefs = [
                {
                    "render": function (data, type, row) {
                        return formatShortDate(data);
                    },
                    "targets": 0
                }
            ];

            fnGetList(attr);
        }
        
    </script>
End Section
