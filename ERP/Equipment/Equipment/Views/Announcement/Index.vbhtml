@Code
    ViewData("Title") = "Index"
    
    Dim lsmonth As New List(Of String)
    For i As Int16 = 1 To 12
        lsmonth.Add(MonthName(i))
    Next
    Dim ms = lsmonth.Select(Function(c, i) New With {.idx = i, .name = c})
    ViewData("MonthList") = New SelectList(ms, "idx", "name", Date.Today.Month)
    
End Code
@Using Html.BeginJUIBox("Pengumuman")
    If User.IsInRole("Home.Administrator") Then
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="pull-right">
                <div class="btn-group">
                    <button class="btn btn-primary" id="btnNewAnnouncement">
                        Membuat Pengumuman</button>
                </div>
            </div>
        </div>
    </div>
    End If
       
   @Html.Hidden("AllowToManage", IIf( User.IsInRole("Home.Administrator"),1,0))
    
    @<div class="row">
        @Using Html.BeginForm("GetAnnouncementList", "Announcement", Nothing, FormMethod.Post, New With {.id = "frmFilter", .autocomplete = "off", .class = "form-horizontal"})
                
@* @Html.WriteFormInput(Html.DropDownList("MonthList", Nothing, New With {.class = "form-control"}), "Bulan")*@
            @Html.WriteFormInput(Html.TextBox("yy", Date.Today.Year, New With {.class = "spinner"}), "Tahun")
            @Html.WriteFormInput(New MvcHtmlString("<button class='btn btn-primary' type='button' id='btnView'>Lihat</button>"), "")
                
    End Using
    </div>
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="table-responsive">
                <table class="table table-bordered table-hover" id="tblAnnouncement">
                    <colgroup>
                        <col style="width: 40px" />
                        <col style="width: 120px" />
                        <col style="width: auto;" />
                        <col style="width: 120" />
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
                                Judul
                            </th>
                            <th>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>
    
End Using
<style>
    .ui-spinner
    {
        position: relative;
        display: inline-block;
        overflow: hidden;
        padding: 0;
        vertical-align: middle;
    }
    
    .ui-spinner-input
    {
        border: none;
        background: none;
        padding: 0;
        margin: .2em 0;
        vertical-align: middle;
        margin-left: .4em;
        margin-right: 22px;
    }
    
    .ui-spinner-button
    {
        width: 16px;
        height: 50%;
        font-size: .5em;
        padding: 0;
        margin: 0;
        text-align: center;
        position: absolute;
        cursor: default;
        display: block;
        overflow: hidden;
        right: 0;
    }
    
    /* more specificity required here to overide default borders */
    .ui-spinner a.ui-spinner-button
    {
        border-top: none;
        border-bottom: none;
        border-right: none;
    }
    
    /* vertical centre icon */
    .ui-spinner .ui-icon
    {
        position: absolute;
        margin-top: -8px;
        top: 50%;
        left: 0;
    }
    
    .ui-spinner-up
    {
        top: 0;
    }
    
    .ui-spinner-down
    {
        bottom: 0;
    }
    
    /* need to fix icons sprite */
    .ui-spinner .ui-icon-triangle-1-s
    {
        background-position: -65px -16px;
    }
</style>
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">


    var tblAnnouncement = null;

    var loadData = function () {
        var _url = $("#frmFilter").attr("action");
        var _data = $("#frmFilter").serialize();
        tblAnnouncement.clear().draw();
        $.ajax({
            url: _url,
            data: _data,
            type: 'POST',
            success: function (d) {
                tblAnnouncement.rows.add(d.data).draw();
            },
            error: ajax_error_callback,
            datatype: 'json'
        });

    };
    var _initTblAnnouncement = function () {

        var _renderDetail = function (data, type, row) {
            if (type == "display") {
                var AllowToManage = $("#AllowToManage").val() == 1 ? true : false;
                var htmls = new Array();
                htmls.push('<div class="btn-group">');
                htmls.push("<a role='button' data-target='#' href='/Announcement/Detail/" + data +
                "' title='Detail' class='btn btn-primary'>Detail</a>");

                if (AllowToManage) {
                    htmls.push('<button type="button" class="btn btn-danger dropdown-toggle" ');

                    htmls.push('data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">');
                    htmls.push('<span class="caret"></span>');
                    htmls.push('<span class="sr-only">Action</span>');
                    htmls.push('</button>');
                    htmls.push('<ul class="dropdown-menu text-left pull-right">');
                    htmls.push('<li><a href="/Announcement/Edit/' + data + '">Edit</a></li>');

                    htmls.push('<li><a href="#" class="lnkDelete">Hapus</a></li>');


                    htmls.push('</ul>');
                }
                htmls.push('</div>');


                return htmls.join("\n");
            }
            return data;
        }
        var cols = [
        { 'data': 'ID', "sClass": "text-right" },
        { 'data': 'PublishDate', "sClass": "text-center", "mRender": _fnRenderNetDate },
        { 'data': 'Title' },
        { 'data': 'ID', "sClass": "text-center", "mRender": _renderDetail }

    ];
        datatableDefaultOptions.searching = true;
        datatableDefaultOptions.aoColumns = cols;
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            _fnlocalDrawCallback(oSettings);
            $(".lnkDelete").click(_lnkDelete_click);
        }
        tblAnnouncement = $("#tblAnnouncement").DataTable(datatableDefaultOptions)

    };                 //init tblannouncement;


    var _lnkDelete_click = function (e) {
        e.preventDefault();
        if (confirm("Hapus dokumen ini?") == false) {
            return;
        }
        var row = _getDataTableRow(this)
        $.ajax({
            url: '/Announcement/Delete',
            type: 'POST',
            data: { id: row.data().ID },
            success: function () {
                loadData();
            },
            error: ajax_error_callback,
            datatype: 'json'
        });

    }//lnkdelete click
    var _getDataTableRow = function (obj) {
        return tblAnnouncement.row($(obj).closest('tr'));
    }
    $(function () {

        var spinner = $(".spinner").spinner().css({ "width": "80px", "margin": "0px", "border": "0px solid #cccccc" });
        $(".ui-spinner-button").css("background-color", "#cccccc");

        _initTblAnnouncement();
        $("#btnView").click(function () {
            loadData();
        });

        $("#btnView").trigger("click");
        $("#btnNewAnnouncement").click(function () {
            window.location = "/Announcement/Create";
        });

    });       //end init;




</script>
