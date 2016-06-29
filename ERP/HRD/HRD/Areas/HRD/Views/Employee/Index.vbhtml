@Code
    ViewData("Title") = "Data Karyawan"
End Code
@Using Html.BeginJUIBox("Daftar Karyawan")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
        <div class="pull-right">
        <div class="btn-group">
            <button class="btn btn-primary" id="btnAddEmployee">Menambah Data Karyawan</button>
        </div>
        </div>
        </div>
        
    </div>
    @<div class="row">
        <div class="col-lg-4 col-sm-6">
            KANTOR</div>
        <div class="col-lg-4 col-sm-6">
            @Html.DropDownList("OfficeList", Nothing, New With {.class = "form-control"})
        </div>
        <br />
        <br />
    </div>
    
    @<div class="row">
    <div class="col-lg-12 col-sm-12">
	<div class="table-responsive">
    <table class="table table-bordered" id="tblEmployee">
            <colgroup>
                <col style="width: 60px">
                <col style="width: 100px">
                <col >
                <col style="width: 200px">
                <col style="width: 10px">
            </colgroup>
            <thead>
                <tr>
                    <th>
                        #
                    </th>
                    <th>
                        NIK
                    </th>
                    <th>
                        Nama
                    </th>
                    <th>
                        Jabatan
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
    var tblEmployee = null;
    var _renderAlias = function (data, type, row) {
        if (type == 'display') {
            if (row.Alias != null) {
                return data + " (" + row.Alias + ")";
            }
            return data;
        }
        return data;
    }
    var _renderDetail = function (data, type, row) {
        if (type == 'display') {
            if (data == null) {
                return "";
            }

            return "<a href='/HRD/Employee/detail/" + data + "' class='btn btn-primary btn-xs'>Detail</a>";
        }
        return data;
    }
    var _initDatatable = function () {
        var arrColumns = [
            { "data": "OfficeId", "sClass": "text-right" }, //0
            {"data": "EmployeeId" }, //1
            {"data": "Fullname", "mRender": _renderAlias }, //2
            {"data": "OccupationName" }, //3
            {"data": "Id", "sClass": "text-center", "mRender": _renderDetail }
        ];
        var _localLoad = function (data, callback, setting) {

            $.ajax({
                url: '/HRD/Employee/GetEmployeeList',
                data: { officeid: $("#OfficeList").val() },
                type: 'POST',
                success: callback,
                datatype: 'json'
            });

        };
        _coldefs = [];
        datatableDefaultOptions.searching = true;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = _coldefs;
        datatableDefaultOptions.ajax = _localLoad;
        datatableDefaultOptions.info = false;
        datatableDefaultOptions.bAutoWidth = false;
        datatableDefaultOptions.ordering = false;
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            var counter = 1;

            var api = this.api();
            if (parseInt(api.data().length, 0) > 0) {

                var rows = api.rows({ page: 'current' }).nodes();
                var _currentOfficeId = -1;
                $(rows).each(function (tr, idx) {
                    var datarow = api.row(idx).data();
                    if (datarow.EmployeeId != null) {
                        $(this).find("td:eq(0)").text(counter);
                        counter++;

                    } else {
                        $(this).find("td:eq(0)").html("&nbsp;");
                    }



                    if (_currentOfficeId != datarow.OfficeId) {
                        var _html = "<tr class='text-bold'><td colspan='5'>" + datarow.OfficeName + "</td></tr>";
                        $(this).before(_html);
                        _currentOfficeId = datarow.OfficeId;
                    }

                });
            }

        };
        datatableDefaultOptions.sDom = "<'row'<'col-lg-12 col-sm-12'<'pull-right'f>>><'row'<'col-sm-12'tr>><'row'<'col-sm-5'i><'col-sm-7'p>>";

        tblEmployee = $("#tblEmployee").DataTable(datatableDefaultOptions);
    }




    $(function () {
        _initDatatable();
        $("#btnAddEmployee").click(function () {
            window.location = "/HRD/Employee/EmployeeForm";
        });
        $('div.dataTables_filter input').removeClass('input-sm');
        $("#OfficeList").change(function () {
            tblEmployee.ajax.reload();
        });
    });
</script>