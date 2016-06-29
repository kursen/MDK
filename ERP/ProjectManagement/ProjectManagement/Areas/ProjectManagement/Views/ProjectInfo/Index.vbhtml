@Code
    ViewData("Title") = "Informasi Proyek"
End Code
@Using Html.BeginJUIBox("Proyek Berjalan")
    @<div class="well">
        <a href="/ProjectManagement/ProjectInfo/Create" type="button" class="btn btn-success ajax-link">
            Menambah Project</a>
    </div>
    
    @<div class="dataTables_wrapper form-inline" role="grid">
		<div class="table-responsive">
        <table class="table responsive no-wrap table-bordered table-striped table-hover table-heading table-datatable dataTable no-footer"
            id="tblRunningProjects" width="100%">
            <thead>
                <tr>
                    <th>
                        #
                    </th>
                    <th>
                        Paket
                    </th>
                    <th>
                        Mulai
                    </th>
                    <th>
                        Lama
                    </th>
                    <th>
                        Progres
                    </th>
                    <th>
                    </th>
                    <th>
                    </th>
                </tr>
            </thead>
        </table>
		</div>
    </div>
End Using
@Using Html.BeginJUIBox("Arsip Proyek")
    @<div class="dataTables_wrapper form-inline" role="grid">
		<div class="table-responsive">
        <table class="table responsive no-wrap table-bordered table-striped table-hover table-heading table-datatable dataTable no-footer"
            id="tblArchiveProjects" width="100%">
            <thead>
                <tr>
                    <th>
                        #
                    </th>
                    <th>
                        Paket
                    </th>
                    <th>
                        Mulai
                    </th>
                    <th>
                        Lama
                    </th>
                    <th>
                        Progres
                    </th>
                    <th>
                    </th>
                    <th>
                    </th>
                </tr>
            </thead>
        </table>
		</div>
    </div>

End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">
    //** Detail DataTable **
    var tblRunningProjects = null;
    var tblArchiveProjects = null;
    var _renderDetail = function (data, type, row) {
        if (type == 'display') {
            return "<a href='/ProjectManagement/ProjectInfo/detail/" + data + "' class='ajax-link btn btn-primary btn-xs'>Detail</a>";
        }
        return data;
    }
    var _renderClose = function (data, type, row) {
        if (type == 'display') {
            return "<a href='#' class='ajax-link btn btn-danger btn-xs btnArchive'>Tutup</a>";
        }
        return data;
    }

    var _renderReopen = function (data, type, row) {
        if (type == 'display') {
            return "<a href='#' class='ajax-link btn btn-danger btn-xs btnReopen'>Buka</a>";
        }
        return data;
    }
    var _renderProgres = function (data, type, row) {
        if (type == 'display') {
              return $.number(data, 2, ',', ' ') + "%";
        }
        return data;
    }
    $(function () {



        var arrColumns = [
            { "data": "Id", "sClass": "text-right", "sWidth": "40px" }, //0
            {"data": "ProjectTitle" }, //1
            {"data": "DateStart", "sClass": "text-center", "mRender": _fnRenderNetDate, "sWidth": "80px" }, //2
            {"data": "NumberOfDays", "sClass": "text-right", "sWidth": "50px" }, //3
            {"data": "Progress", "sClass": "text-right", "mRender":_renderProgres, "sWidth": "80px" }, //4
            {"data": "Id", "sClass": "text-center", "mRender": _renderDetail, "sWidth": "60px" }, //5
            {"data": "Id", "sClass": "text-center", "mRender": _renderClose, "sWidth": "60px"}//6

        ];
        var _coldefs = [
                    { "bSortable": false, "targets": [0], "mData": null },
                    { "bSortable": false, "targets": [6] },
                    { "bSortable": false, "targets": [5] }

            ];
        var _localLoad = function (data, callback, setting) {

            $.ajax({
                url: '/ProjectManagement/ProjectInfo/GetActiveProjectList',
                type: 'POST',
                success: callback,
                datatype: 'json'
            })

        };
        datatableDefaultOptions.searching = true;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = _coldefs;
        datatableDefaultOptions.ajax = _localLoad;
        tblRunningProjects = $("#tblRunningProjects").DataTable(datatableDefaultOptions)
            .on("click", ".btnArchive", function (e, event) {

                var tr = $(this).closest('tr');
                var row = tblRunningProjects.row(tr);

                $.ajax({
                    url: '/ProjectManagement/ProjectInfo/SavePartial',
                    data: { "pk": row.data().Id, "name": "Archive", "value": true },
                    type: 'POST',
                    success: function (data) {
                        if (data.stat == 1) {
                            tblArchiveProjects.ajax.reload();
                            tblRunningProjects.ajax.reload();
                        }
                    },
                    datatype: 'json'
                });
                return false;
            });


        var arrArchiveColumns = [
            { "data": "Id", "sClass": "text-right", "sWidth": "40px" }, //0
            {"data": "ProjectTitle" }, //1
            {"data": "DateStart", "sClass": "text-center", "mRender": _fnRenderNetDate, "sWidth": "80px" }, //2
            {"data": "NumberOfDays", "sClass": "text-right", "sWidth": "50px" }, //3
           {"data": "Progress", "sClass": "text-right", "mRender": _renderProgres, "sWidth": "80px" }, //4
            {"data": "Id", "sClass": "text-center", "mRender": _renderDetail, "sWidth": "60px" }, //5
            {"data": "Id", "sClass": "text-center", "mRender": _renderReopen, "sWidth": "60px"}//6
        ];

        var _ArchivelocalLoad = function (data, callback, setting) {

            $.ajax({
                url: '/ProjectManagement/ProjectInfo/GetInActiveProjectList',
                type: 'POST',
                success: callback,
                datatype: 'json'
            })

        };
        datatableDefaultOptions.aoColumns = arrArchiveColumns;
        datatableDefaultOptions.ajax = _ArchivelocalLoad;
        tblArchiveProjects = $("#tblArchiveProjects").DataTable(datatableDefaultOptions)
            .on("click", ".btnReopen", function (e) {
                var tr = $(this).closest('tr');
                var row = tblArchiveProjects.row(tr);

                $.ajax({
                    url: '/ProjectManagement/ProjectInfo/SavePartial',
                    data: { "pk": row.data().Id, "name": "Archive", "value": false },
                    type: 'POST',
                    success: function (data) {
                        if (data.stat == 1) {
                            tblArchiveProjects.ajax.reload();
                            tblRunningProjects.ajax.reload();
                        }
                    },
                    datatype: 'json'
                });
                return false;
            });
    });

</script>
