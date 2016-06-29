<div class="modal fade" id="mdColor">
    <div class="modal-dialog">
        <div class="modal-content">
            <form class="form-horizontal" autocomplete="off" method="post" action="/Timesheet/SaveColor" id="frmColors">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title">
                    Setting Warna</h4>
            </div>
            <div class="modal-body">
                @Code
                    Dim ddTaskItem = New Mvc.MvcHtmlString("<select id='ddTaskItemId' name='TaskItemId' class='form-control'></select>")
                End Code
                @Html.WriteFormInput(ddTaskItem,
                                   "Uraian", smLabelWidth:=2, smControlWidth:=4, lgLabelWidth:=4, lgControlWidth:=6)
                @Html.WriteFormInput(Html.Hidden("RowBackgroundColor", Nothing, New With {.class = "form-control"}),
                                     "Warna Latar Baris", smLabelWidth:=2, smControlWidth:=1, lgLabelWidth:=4, lgControlWidth:=1)
                @Html.WriteFormInput(Html.Hidden("CellBackgroundColor", Nothing, New With {.class = "form-control"}),
                                     "Warna Latar Cell", smLabelWidth:=2, smControlWidth:=1, lgLabelWidth:=4, lgControlWidth:=1)
                @Html.WriteFormInput(Html.Hidden("CellColor", Nothing, New With {.class = "form-control"}),
                                     "Warna Text", smLabelWidth:=2, smControlWidth:=1, lgLabelWidth:=4, lgControlWidth:=1)
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">
                    Tutup</button>
                <button type="submit" class="btn btn-primary" id="dlgModalSaveColors">
                    Simpan</button>
            </div>
            </form>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
<!-- /.modal -->
<script type="text/javascript">

    var submitFormColor_Callback = function (data) {
        _loadContent();
        _loadFooter();

        $("#mdColor").modal('hide');
    }
    $(function () {
        //colorpicker
        $("#RowBackgroundColor").colorpicker();
        $("#CellBackgroundColor").colorpicker();
        $("#CellColor").colorpicker();

        $("#frmColors").submit(function () {
            var _url = $("#frmColors").attr("action");
            var _data = $("#frmColors").serialize();
            $.ajax({
                url: _url,
                data: _data,
                type: 'POST',
                success: submitFormColor_Callback,
                error: ajax_error_callback,
                datatype: 'json'
            });
            return false;
        });


        $("body").on("click", ".clrpicker", function (e) {
            $("#mdColor").modal('show');

            var post = tbTimesheetHeaderLeft.cell(e.target).index();
            var datarow = tbTimesheetHeaderLeft.row(post.row).data();
            $("#ddTaskItemId").val(datarow.id);
            $("#RowBackgroundColor").colorpicker("val", "#"+datarow.RowBackgroundColor);
            $("#CellBackgroundColor").colorpicker("val", "#" + datarow.CellBackgroundColor);
            $("#CellColor").colorpicker("val", "#" + datarow.CellTextColor);
        });
    })
</script>