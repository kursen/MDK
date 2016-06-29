@ModelType ProjectManagement.ProjectInfo
@Code
    ViewData("Title") = "Informasi Proyek"
    Dim TimeSheetHeader = ProjectManagement.ProjectTimeSheetViewer.CreateProjectTimesheetHeader(Model)
    
    Dim NumberOfColumns As Integer = TimeSheetHeader.NumberOfWeeks + 3
    Dim arrItemCounter As New List(Of Integer)
    Dim cntr = 0
End Code
@Html.Partial("ProjectPageMenu", Model)
@Html.Hidden("ProjectInfoId", Model.Id)
@Using Html.BeginJUIBox("Timesheet MC0 RESCHEDULE")
    
    @<div id='bottomNavBar' style="width: 50%; position: fixed; bottom: 0px; right: 30px;
        height: 50px; z-index: 1200;">
        <div style='padding: 13px; background-color: #6699FF; border-radius: 5px;'>
            <div class="scroll-bar-wrap ui-widget-content ui-corner-bottom">
            </div>
            <div class="scroll-bar">
            </div>
        </div>
    </div>
    
    
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="pull-right">
                <button type="button" class="btn btn-danger btn-label-left" id="btnPrint">
                    <span><i class="fa fa-print"></i></span>Print</button>
                <button type="button" class="btn btn-danger btn-label-left" id="btnRefresh">
                    <span><i class="fa fa-calculator"></i></span>Refresh
                </button>
                <button type="button" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#mdColor">
                    Colors
                </button>
            </div>
        </div>
    </div>
    
  
    @<div style="position: relative">
        <div style="" id='divWrapperTopTop'>
            <table class="table-bordered " id="tblTimesheetHeadTop" style="table-layout: fixed;
                margin-bottom: 0px; background-color: #fff; display: none;">
                <colgroup>
                    <col style="width: 100px; border-left-style: double;" />
                    <col style="width: 460px;" />
                </colgroup>
                <thead>
                    <tr class="double_top_border double_bottom_border">
                        <th style="height: 154px; padding: 8px; vertical-align: bottom" colspan="1" class="text-center">
                            No. Mata Pembayaran
                        </th>
                        <th style="height: 154px; padding: 8px; vertical-align: bottom" colspan="1" class="text-center">
                            Uraian
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div id="divWrapperTopRight">
            <table class="table table-bordered " id="tblTimesheetHeadTopRight" style="table-layout: fixed;
                margin-bottom: 0px; background-color: #fff; display: none;">
                <colgroup>
                    <col style="width: 80px" />
                    <col style="width: 140px" />

   <col style="width: 80px" />
                    <col style="width: 140px" />
                    <col style="width: 80px; border-right-style: double;" />
                    @code
    arrItemCounter.Clear()
    cntr = 0
    For Each item In TimeSheetHeader.MonthList
        cntr += item.Count
        arrItemCounter.Add(cntr)
    Next
                                         
    For i As Integer = 1 To TimeSheetHeader.NumberOfWeeks
        If arrItemCounter.Contains(i) Then
                        @<col style="width: 60px; border-right-style: double;" />
        Else
                            
                        @<col style="width: 60px" />        
        End If
                    
    Next
                    End Code
                </colgroup>
                <thead style="background-color: #fff">
                    <tr class="double_top_border double_bottom_border">
                        <th class="text-center" rowspan="4">
                            Satuan
                        </th>
                        <th class="text-center" rowspan="4">
                            Perkiraan Kuantitas
                        </th>
                        <th class="text-center" rowspan="4">
                            Bobot
                        </th>
                         <th class="text-center" rowspan="4">
                            Kuantitas MC0
                        </th>
                        <th class="text-center" rowspan="4">
                            Bobot MC0
                        </th>
                        <th class="text-center " colspan="@TimeSheetHeader.NumberOfWeeks" >
                            Jadwal Pelaksanaan
                        </th>
                    </tr>
                    <tr>
                        @Code
                       
                                  
    For Each item In TimeSheetHeader.MonthList
                            @<th colspan="@item.Count" class="text-center">
                                @item.MonthName
                            </th>
                            
    Next
                        
                        End Code
                    </tr>
                    <tr>
                        @Code
    For i As Integer = 1 To TimeSheetHeader.NumberOfWeeks
                            @<th class="text-center">
                                @i.ToString("\W#")
                            </th>
    Next
                        End Code
                    </tr>
                    <tr class="double_bottom_border">
                        @code
                        
    For Each item In TimeSheetHeader.WeekItems
                        
                            @<th class="text-center" data-weeknumber='@item.WeekNumber' data-monthname='@item.MonthName'>
                                @item.DayStartEnd
                            </th>
                        
    Next
                        End Code
                    </tr>
                </thead>
            </table>
        </div>
        <div id="divWrapperTopLeft">
            <table class="table table-bordered" id="tbTimesheetHeaderLeft" style="width: auto;
                table-layout: fixed; background-color: #fff;">
                <colgroup>
                    <col style="width: 100px; border-left-style: double;" />
                    <col style="width: 460px;" />
                </colgroup>
                <thead>
                    <tr role="row" class="double_top_border double_bottom_border">
                        <th style="height: 154px;" colspan="1" class="text-center">
                            No. Mata Pembayaran
                        </th>
                        <th style="height: 154px;" colspan="1" class="text-center">
                            Uraian
                        </th>
                    </tr>
                </thead>
                <tfoot>
                    <tr class="tr_kumulatif_mingguan double_top_border">
                        <td>
                        </td>
                        <td>
                            Kemajuan Pekerjaan Mingguan
                        </td>
                    </tr>
                    <tr class="tr_kumulatif_mingguan">
                        <td>
                        </td>
                        <td>
                            Kemajuan Pekerjaan Kumulatif Mingguan
                        </td>
                    </tr>
                    <tr class="tr_kumulatif_bulanan double_top_border">
                        <td>
                        </td>
                        <td>
                            Kemajuan Pekerjaan Bulanan
                        </td>
                    </tr>
                    <tr class="tr_kumulatif_bulanan double_bottom_border">
                        <td>
                        </td>
                        <td>
                            Kemajuan Pekerjaan Kumultatif Bulanan
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
        <div id="divWrapperTable">
            <table class="table table-bordered " id="tblTimesheet" style="table-layout: fixed;">
                <colgroup>
                    <col style="width: 80px" />
                    <col style="width: 140px" />
                     <col style="width: 80px" />
                    <col style="width: 140px" />
                    <col style="width: 80px; border-right-style: double;" />
                    @code
    arrItemCounter.Clear()
    cntr = 0
    For Each item In TimeSheetHeader.MonthList
        cntr += item.Count
        arrItemCounter.Add(cntr)
    Next
                                         
    For i As Integer = 1 To TimeSheetHeader.NumberOfWeeks
        If arrItemCounter.Contains(i) Then
                        @<col style="width: 60px; border-right-style: double;" />
        Else
                            
                        @<col style="width: 60px" />        
        End If
                    
    Next
                    End Code
                </colgroup>
                <thead style="background-color: #fff">
                    <tr class="double_top_border double_bottom_border">
                        <th class="text-center" rowspan="4">
                            Satuan
                        </th>
                        <th class="text-center" rowspan="4">
                             Kuantitas
                        </th>
                        <th class="text-center" rowspan="4">
                            Bobot
                        </th>
                        <th class="text-center" rowspan="4">
                            Kuantitas MC0
                        </th>
                        <th class="text-center" rowspan="4">
                            Bobot MC0
                        </th>
                        <th class="text-center " colspan="@TimeSheetHeader.NumberOfWeeks" >
                            Jadwal Pelaksanaan
                        </th>
                    </tr>
                    <tr>
                        @Code
                       
                                  
    For Each item In TimeSheetHeader.MonthList
                            @<th colspan="@item.Count" class="text-center">
                                @item.MonthName
                            </th>
                            
    Next
                        
                        End Code
                    </tr>
                    <tr>
                        @Code
    For i As Integer = 1 To TimeSheetHeader.NumberOfWeeks
                            @<th class="text-center">
                                @i.ToString("\W#")
                            </th>
    Next
                        End Code
                    </tr>
                    <tr class="double_bottom_border">
                        @code
                        
    For Each item In TimeSheetHeader.WeekItems
                        
                            @<th class="text-center" data-weeknumber='@item.WeekNumber' data-monthname='@item.MonthName'>
                                @item.DayStartEnd
                            </th>
                        
    Next
                        End Code
                    </tr>
                </thead>
                <tfoot>
                    <tr class="tr_kumulatif_mingguan double_top_border">
                        <td>
                            %
                        </td>
                        <td>
                        </td>
                        <td>
                            100,00
                        </td>
                         <td>
                        </td>
                        <td>
                            100,00
                        </td>
                        @code
    For i As Integer = 1 To TimeSheetHeader.NumberOfWeeks
        Dim itemId = "tweek_" & i.ToString()
                            @<td class="text-center" id='@itemId'>
                            </td>
    Next
                        End Code
                    </tr>
                    <tr class="tr_kumulatif_mingguan">
                        <td class="text-center">
                            %
                        </td>
                        <td>
                        </td>
                        <td>
                        </td><td></td><td></td>
                        @code
    For i As Integer = 1 To TimeSheetHeader.NumberOfWeeks
        Dim itemId = "tkweek_" & i.ToString()
                            @<td class="text-center" id='@itemId'>
                            </td>
    Next
                        End Code
                    </tr>
                    <tr class="tr_kumulatif_bulanan double_top_border">
                        <td class="text-center">
                            %
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td></td><td></td>
                        @Code
                       
    cntr = 1
    For Each item In TimeSheetHeader.MonthList
                            @<td colspan="@item.Count" id="@Html.Raw("tmonth_" & cntr)" class="text-center">
                            </td>
        cntr += 1
    Next
                        
                        End Code
                    </tr>
                    <tr class="tr_kumulatif_bulanan double_bottom_border">
                        <td class="text-center">
                            %
                        </td>
                        <td>
                        </td>
                        <td>
                        </td><td></td><td></td>
                        @Code
                       
    cntr = 1
    For Each item In TimeSheetHeader.MonthList
                            @<td colspan="@item.Count" id="@Html.Raw("tkmonth_" & cntr)" class="text-center">
                            </td>
        cntr += 1
    Next
                        
                        End Code
                    </tr>
                </tfoot>
            </table>
        </div>
        <div class="clearfix">
        </div>
    </div>
End Using
@Html.Partial("Colors")
<style>
    .double_top_border
    {
        border-top-style: double;
    }
    .double_bottom_border
    {
        border-bottom-style: double;
    }
    
    .tr_division
    {
        background-color: #f0f0f0;
        font-weight: bold;
    }
    
    .tr_kumulatif_mingguan
    {
        background-color: #f0f0f0;
        font-weight: bold;
    }
    .tr_kumulatif_bulanan
    {
        background-color: #99CCFF;
        font-weight: bold;
    }
    
    .editable-click, a.editable-click, a.editable-click:hover
    {
        text-decoration: none;
        border-bottom: 1px dashed #08C;
    }
    
    #divWrapperTopTop
    {
        position: fixed;
        overflow: hidden;
        z-index: 1002;
        background-color: white;
        top: 0px;
        padding: 0px;
        margin: 0px;
        top: 80px;
    }
    
    #divWrapperTopLeft
    {
        position: absolute;
        top: 0px;
        padding: 90px 0px 0px 0px;
        z-index: 0;
        width: 565px;
    }
    #divWrapperTopRight
    {
        position: fixed;
        overflow: hidden;
        z-index: 1002;
        background-color: white;
        top: 0px;
        padding: 0px;
        margin: 0px;
        top: 80px;
    }
    #divWrapperTable
    {
        overflow: hidden;
        padding-top: 90px;
        margin: 0px;
    }
    
    .scroll-bar-wrap
    {
        clear: left;
        padding: 0 4px 0 2px;
        margin: 0 -1px -1px -1px;
    }
    .scroll-bar-wrap .ui-slider
    {
        background: none;
        border: 0;
        height: 2em;
        margin: 0 auto;
    }
    .scroll-bar-wrap .ui-handle-helper-parent
    {
        position: relative;
        width: 100%;
        height: 100%;
        margin: 0 auto;
    }
    .scroll-bar-wrap .ui-slider-handle
    {
        top: .2em;
        height: 1.5em;
    }
    .scroll-bar-wrap .ui-slider-handle .ui-icon
    {
        margin: -8px auto 0;
        position: relative;
        top: 50%;
    }
    
    .cutofftext
    {
        width: 420px;
        white-space: nowrap;
        text-overflow: ellipsis;
        overflow: hidden;
    }
    .bgwhite
    {
        background-color: #fff;
        color: #000;
    }
    .highlighted
    {
        background-color: #f90;
    }
</style>
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<link href="../../../../plugins/bootstrap-editable/bootstrap-editable.css" rel="stylesheet"
    type="text/css" />
<script src="../../../../plugins/bootstrap-editable/bootstrap-editable.js" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<link href="../../../../plugins/color-picker/evol.colorpicker.min.css" rel="stylesheet"
    type="text/css" />
<script src="../../../../plugins/color-picker/evol.colorpicker.min.js" type="text/javascript"></script>
<script type="text/javascript">
    var _numberOfWeeks = @TimeSheetHeader.NumberOfWeeks;

</script>
<script type="text/javascript">
    var isMouseDown = false, isHighlighted;
    var trSelected = null;
    var initMouseSelector = function () {

        $("#tblTimesheet  td.taskweight")
    .mousedown(function () {
        
        clearSelection();
        //if ($(this).hasClass("taskweight") == false) return;
        isMouseDown = true;
        trSelected = $(this).parent()[0];
        $(this).toggleClass("highlighted");
        isHighlighted = $(this).hasClass("highlighted");
        return false; // prevent text selection
    })
    .mouseover(function () {
        if (isMouseDown) {
            if ($(this).parent()[0] == trSelected) {

                $(this).toggleClass("highlighted", isHighlighted);
            }

        }
    }).mouseup(function () {
        isMouseDown = false;
        if ($("#tblTimesheet td.highlighted").length > 1) {
            $(this).trigger("click");
        }


    })
    .bind("selectstart", function () {
        return false;
    })

    };

    var clearSelection = function () {
        $("#tblTimesheet td.highlighted").removeClass("highlighted");



    };
</script>
<script type="text/javascript">
    var tblTimesheet = null;
    tbTimesheetHeaderLeft = null;


    //overrides the MessageMenuWidth
    //to make sure the table header panel always 
    //fit to the table
    function MessagesMenuWidth() {
        var W = window.innerWidth;
        var W_menu = $('#sidebar-left').outerWidth();
        var w_messages = (W - W_menu) * 16.666666666666664 / 100;
        $('#messages-menu').width(w_messages);
        initWrapper();

    }

    var _loadContent = function () {
        var _ProjectId = $("#ProjectInfoId").val();
        $.ajax({
            url: '/Timesheet/GetTimeSheetContentMC0',
            data: { id: _ProjectId },
            type: 'POST',
            success: _loadContentCallback,
            error: ajax_error_callback,
            datatype: 'json'
        });


    };

    var _loadContentCallback = function (data) {

        tblTimesheet.clear();
        tbTimesheetHeaderLeft.clear();
        tblTimesheet.rows.add(data.data).draw();
        tbTimesheetHeaderLeft.rows.add(data.data).draw();
        var ddlItem = $("#ddTaskItemId");
        ddlItem.empty();
        $(data.data).each(function (e) {
            var d = this;
            ddlItem.append("<option value='" + d.id + "'>" + d.paymentnumber + " - " + d.tasktitle + "</option>");

        });

    }
    var _loadFooter = function () {
        var _ProjectId = $("#ProjectInfoId").val();
        $.ajax({
            url: '/Timesheet/GetTimesheetFooterMC0',
            data: { id: _ProjectId },
            type: 'POST',
            success: _loadFooterCallback,
            error: ajax_error_callback,
            datatype: 'json'
        });

    };
    var _loadFooterCallback = function (data) {
        $(data.WeeklyWeight).each(function (i, g) {


            $("#tweek_" + g.Weeknumber).html($.number(g.Weight, 2, ",", "."));
            $("#tkweek_" + g.Weeknumber).html($.number(g.WeightAccumulation, 2, ",", "."));

        });

        $(data.MonthlyWeight).each(function (i, g) {
            $("#tmonth_" + g.MonthNumber).html($.number(g.Weight, 2, ",", "."));
            $("#tkmonth_" + g.MonthNumber).html($.number(g.WeightAccumulation, 2, ",", "."));

        });

    }


    initScrollbar = function () {
        var scrollPane = $("#divWrapperTopRight"),
          tblTimesheetHeadTopRightContent = $("#tblTimesheetHeadTopRight");
        var tblTimesheetContent = $("#tblTimesheet");
        var scrollbar = $(".scroll-bar").slider({
            slide: function (event, ui) {
                if (tblTimesheetContent.width() > scrollPane.width()) {
                    tblTimesheetHeadTopRightContent.css("margin-left", Math.round(
                ui.value / 100 * (scrollPane.width() - tblTimesheetHeadTopRightContent.width())
              ) + "px");
                    tblTimesheetContent.css("margin-left", Math.round(
                ui.value / 100 * (scrollPane.width() - tblTimesheetContent.width())
              ) + "px");
                } else {
                    tblTimesheetHeadTopRightContent.css("margin-left", 0);
                    tblTimesheetContent.css("margin-left", 0);
                }
            }
        });

        var handleHelper = scrollbar.find(".ui-slider-handle");
        if (handleHelper.has("span").length == 0) {
            handleHelper.append("<span class='ui-icon ui-icon-grip-dotted-vertical'></span>");
        }


        scrollPane.css("overflow", "hidden");


    } //end initScrollbar;


    initWrapper = function () {


        $("#divWrapperTopTop").width($("#divWrapperTopLeft").width());
        $("#divWrapperTopRight").css("margin-left", $("#divWrapperTopTop").width());
        $("#divWrapperTable").css("margin-left", $("#divWrapperTopLeft").width());
        $("#divWrapperTopRight").width($("#divWrapperTable").width());
        ///end init thewrapper
    }


    $(function () { //init js

        $("#btnPrint").click(function () {

            var projectId = $("#ProjectInfoId").val();
            window.location = "/Timesheet/PrintTimeSheetMC0/?ProjectId=" + projectId;
        });

        _loadContent();
        _loadFooter();
        $("#btnRefresh").click(function () {
            _loadContent();
            _loadFooter();

        });


        $("#mdColor").appendTo("body");


        $(window).scroll(function (event) {
            var scroll = $(window).scrollTop();
            if (scroll > 392) {
                $("#tblTimesheetHeadTop").show()
                $("#tblTimesheetHeadTopRight").show()

                $("#tblTimesheetHeadTopRight").css("margin-left", $("#tblTimesheet").css("margin-left"));
            } else {
                $("#tblTimesheetHeadTop").hide()
                $("#tblTimesheetHeadTopRight").hide()

            }
        });



        ///tabledata area

        var _RenderBlankIfZero = function (data, type, row) {
            if (type == 'display') {
                if (parseFloat(data) == 0) {

                    return "";
                }

                return $.number(data, 2, ",", ".");
            }
            return data;
        }
        var _CellCreated = function (td, cellData, rowData, row, col) {
            if (cellData != 0) {
                $(td).css("background-color", "#" + rowData.CellBackgroundColor);
                $(td).css("color", "#" + rowData.CellTextColor);
            }
          
        }

        var arrColumns = [

            { "data": "unitquantity", "sClass": "text-center bgwhite" }, //2
            {"data": "quantity", "sClass": "text-right bgwhite", "mRender": _fnRender2DigitDecimal }, //3
            {"data": "taskweight", "sClass": "text-right bgwhite", "mRender": _fnRender2DigitDecimal }, //4
             {"data": "quantityMC0", "sClass": "text-right bgwhite", "mRender": _fnRender2DigitDecimal }, //3
            {"data": "taskWeightMC0", "sClass": "text-right bgwhite", "mRender": _fnRender2DigitDecimal} //4
        ];

        for (i = 1; i < _numberOfWeeks + 1; i++) {

            var pad = "000000";
            var result = (pad + i).slice(-pad.length);
            
            arrColumns.push({ 'data': result, 'fnCreatedCell': _CellCreated, 'mRender': _RenderBlankIfZero, 'sClass': 'text-right taskweight', 'name': result });
        }
        var _coldefs = [];
        var _localLoad = function (data, callback, setting) {

            var _ProjectId = $("#ProjectInfoId").val();
            $.ajax({
                url: '/Timesheet/GetTimeSheetContentMC0',
                data: { id: _ProjectId },
                type: 'POST',
                success: callback,
                datatype: 'json'
            });

        };


        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = _coldefs;
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = false;
        datatableDefaultOptions.fnCreatedRow = function (row, data, dataIndex) {
            var tbl = $(row).parent();
            $(row).addClass("tr_id_" + data.id);
            $(row).css("background-color", "#" + data.RowBackgroundColor)

        }

        datatableDefaultOptions.fnDrawCallback = function (settings) {
            var divisionId = 0;
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            api.column(0, { page: 'current' }).data().each(function (group, i) {

                var currentdivisionId = api.row(i).data().divisionid;


                if (currentdivisionId != divisionId) {

                    var _html = "<tr class='tr_division'>";
                    for (var nweek = 0; nweek < _numberOfWeeks + 5; nweek++) {
                        _html += "<td>&nbsp;</td>";

                    }

                    _html += "</tr>";

                    $(rows).eq(i).before(_html);


                    divisionId = currentdivisionId;
                }


            });
            //initMouseSelector();

        }


        tblTimesheet = $("#tblTimesheet").DataTable(datatableDefaultOptions)
	        .on("draw.dt", function () {
	            initWrapper();
	            initScrollbar();

	        });



        //left table header
        var _renderCutoff = function (data, type, row) {
            if (type == "display") {
                return "<div class='cutofftext'>" + data + "</div>";
            }
            return data;
        }


        arrColumns = [

            { "data": "paymentnumber", "sClass": "text-center clrpicker" }, //0
            {"data": "tasktitle", "mRender": _renderCutoff} //1
            ];

        datatableDefaultOptions.fnDrawCallback = function (settings) {


            var divisionId = 0;
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            api.column(0, { page: 'current' }).data().each(function (group, i) {

                var currentdivisionId = api.row(i).data().divisionid;


                if (currentdivisionId != divisionId) {
                    $(rows).eq(i).before('<tr class="tr_division"><td class="text-center">' +
                        api.rows(i).data()[0].divisionnumber + '</td><td>' + api.rows(i).data()[0].divisiontitle + '</td>' +
                        '</tr>')
                    divisionId = currentdivisionId;
                }
            })
            $('.taskweight').on('hidden.bs.popover', function (e) {
                clearSelection();
            });


        };
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = _coldefs;
        datatableDefaultOptions.fnCreatedRow = function (row, data, dataIndex) { };


        tbTimesheetHeaderLeft = $("#tbTimesheetHeaderLeft").DataTable(datatableDefaultOptions)
            .on("draw.dt", function () {
                $(".clrpicker").attr('role', 'button');

            });



        //create editable taskweight
        $('body').editable({
            selector: ".taskweight",
            type: "text",
            html: true,
            container: "body",
            pk: 0,
            url: "/Timesheet/SaveWeightMC0",
            emptytext: "",
            params: function (p) {
                var post = tblTimesheet.cell(this).index();
                var datarow = tblTimesheet.row(post.row).data();
                var weeknumberHeader = tblTimesheet.column(post.column).header();
                var weeknumber = $(weeknumberHeader).data("weeknumber");
                p.itemId = datarow.id;
                p.weeknumber = weeknumber;
                p.taskweight = $.number(datarow.taskWeightMC0, 2, ",", ".");
                //console.log($("td.highlighted").length);
                return p;

            },
            title: function (e) {
                var post = tblTimesheet.cell(this).index();
                var datarow = tblTimesheet.row(post.row).data();
                var weeknumberHeader = tblTimesheet.column(post.column).header();
                var weeknumber = $(weeknumberHeader).data("weeknumber");
                var monthname = $(weeknumberHeader).data("monthname");
                var _dtlist = "<dl class='dl-horizontal'>" +
			    "<dt>No.Pembayaran</dt><dd>" + datarow.paymentnumber + "</dd>" +
			    "<dt>Uraian</dt><dd>" + datarow.tasktitle + "</dd>" +
			    "<dt>Bulan</dt><dd>" + monthname + "</dd>" +
			    "<dt>Minggu ke </dt><dd>" + weeknumber + "</dd>" +
			    "<dt>Tanggal</dt><dd>" + $(weeknumberHeader).text() + "</dd>" +
                "<dt>Bobot</dt><dd>" + datarow.taskWeightMC0 + "</dd>" +
			    "</dt>";
                return _dtlist;
            },
            display: function (value) {

                var tmp = value.replace(",", ".");

                if (isNaN(parseInt(tmp))) {
                    $(this).empty();
                    $(this).css("background-color", "");
                    $(this).css("color", "");
                } else {
                    $(this).text($.number(value, 2, ",", "."));
                    var post = tblTimesheet.cell(this).index();
                    var datarow = tblTimesheet.row(post.row).data();
                    $(this).css("background-color", datarow.CellBackgroundColor);
                    $(this).css("color", datarow.CellTextColor);
                }

            },
            success: function (data, newvalue) {

                if ((data.stat.length) && (data.stat == 1)) {
                    _loadFooter();
                    if (newvalue == "") {

                    }
                } else if (data.stat == 1) {

                    _loadFooter();
                    if (newvalue == "") {
                        $(this).css("background-color", "");
                        $(this).css("color", "");
                    }
                } else {
                    return data.error;
                }

            }

        }).on("init", function (e, reason) {

        }); //end set the editable




    });                   //end init
</script>
