@ModelType ProjectManagement.ProjectTaskDivisionItem
@Code
    ViewData("Title") = "Ringkasan task"
    
    
    Dim rvalue = From m In Model.DailyReportTasks
                       Select New With {m.Volume, m.DailyReport.DayWork, m.ProjectTaskDivisionItem.UnitQuantity, m.ID,
                                        m.ProjectTaskDivisionItemId, m.Location,
                                        .WeekNumber = DatePart(DateInterval.WeekOfYear,
                                                               m.ProjectTaskDivisionItem.ProjectTaskDivision.ProjectInfo.DateStart.AddDays(m.DailyReport.DayWork - 1),
                                                                 Microsoft.VisualBasic.FirstDayOfWeek.Monday)}

    Dim result = From m In rvalue
                 Order By m.DayWork
                 Group By m.WeekNumber Into g = Group
                 Select New With {WeekNumber, g}
    
    
  
    Dim FirstWeek = 0
    If result.Count > 0 Then
        FirstWeek = result.First.WeekNumber
    End If
    Dim firstDay = Model.ProjectTaskDivision.ProjectInfo.DateStart
    
    Dim agg As Double = 0
    Dim nCounter As Integer = 0
    
    Dim pmEntities As New ProjectManagement.ProjectManagement_ERPEntities
    
    Dim Summary = pmEntities.ExecuteStoreQuery(Of ProjectManagement.ProjectTaskDoneSummary)("EXEC PMn.ProjectTaskDoneSummary @ProjectId ",
                                                                                 New System.Data.SqlClient.SqlParameter("@projectId",
                                                                                                                        Model.ProjectTaskDivision.ProjectInfo.Id)).ToList()
    Dim SummaryItem = Summary.Find(Function(m) m.TaskId = Model.Id)
    Dim targetvolume = SummaryItem.TargetQuantity
    
End Code
@Functions
    Function inputGroup(ByVal value As String, grouptext As String) As MvcHtmlString
        
    
        Dim control = "<div class='input-group'>" &
                            "<span class='form-control text-right'>" & value & "</span>" &
                            "<span class='input-group-addon'>" & grouptext & "</span>" &
                        "</div>"
	 
		
                          
        Return New MvcHtmlString(control)

    End Function
    
End Functions
@Html.Partial("ProjectPageMenu", Model.ProjectTaskDivision.ProjectInfo)
@Using Html.BeginJUIBox("Ringkasan")
    @Html.Hidden("TaskId", Model.ProjectTaskDivisionId)
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <h3>@Model.PaymentNumber @Model.TaskTitle</h3>
            <h4>@Model.ProjectTaskDivision.DivisionNumber . @Model.ProjectTaskDivision.TaskTitle</h4>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="pull-right">
                <a class="btn btn-primary btn-label-left" 
                href="@Url.Content("~/ProjectManagement/TaskSummary/Index")/@Model.ProjectTaskDivision.ProjectInfoId">
                    <span><i class="fa fa-arrow-left"></i></span>Kembali</a>
                @Html.ActionLink("Refresh", "DetailSummary", "TaskSummary", New With {.taskId = Model.Id}, New With {.class = "btn btn-primary"})
            </div>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-6 col-sm-6">
            <div class="form-horizontal">
                <div class="panel" style="height: 206px">
                    <div class="panel-heading">
                    </div>
                    <div class="panel-body">
                        @Html.WriteFormInput(inputGroup(SummaryItem.TargetQuantity.ToString("N"), SummaryItem.UnitQuantity), "Target Volume", lgLabelWidth:=4, lgControlWidth:=2)
                        @Html.WriteFormInput(inputGroup(SummaryItem.Volume.ToString("N"), SummaryItem.UnitQuantity), "Volume Dikerjakan", lgLabelWidth:=4, lgControlWidth:=2)
                        @Html.WriteFormInput(inputGroup(SummaryItem.Percentage.ToString("N"), "%"), "Persentase", lgLabelWidth:=4, lgControlWidth:=2)
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-6 col-sm-6">
            <div class="panel" style="height: 206px">
                <div class="panel-body">
                    <div id="chartdata" style="height: 180px">
                    </div>
                </div>
            </div>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <table class="table table-bordered">
                <colgroup>
                    <col style="width: 60px">
                </colgroup>
                <thead>
                    <tr>
                        <th>
                            #
                        </th>
                        <th colspan="2">
                            Hari Kerja Ke
                        </th>
                        <th>
                            Tanggal
                        </th>
                        <th colspan="2">
                            Volume
                        </th>
                        <th colspan="2">
                            Agregat
                        </th>
                    </tr>
                </thead>
                <tbody>
                    @If result.Count = 0 Then
                        @<tr>
                            <td colspan="7">
                                [TIDAK ADA DATA]
                            </td>
                        </tr>
    End If
                    @For Each item In result
                
                            
                          
               
                        @<tr style="background-color: #fff;">
                            <td>
                            </td>
                            <td colspan="3" class="text-center text-bold">
                                Minggu Ke @(item.WeekNumber - FirstWeek + 1)
                            </td>
                            <td class="text-right" colspan="2">
                                @item.g.Sum(Function(m) m.Volume).ToString("N")
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
        For Each task In item.g
            nCounter += 1
            agg += task.Volume
                        @<tr>
                            <td class="text-right">@nCounter.
                            </td>
                            <td class="text-right">@task.DayWork
                            </td>
                            <td>
                                @firstDay.AddDays(task.DayWork - 1).ToString("dddd")
                            </td>
                            <td class='text-center'>@firstDay.AddDays(task.DayWork - 1).ToString("dd-MM-yyyy")
                            </td>
                            <td class="text-right editable" data-location="@task.Location" data-daywork="@task.DayWork" data-id="@task.ID" 
                                    data-projecttaskdivisionitemid="@task.ProjectTaskDivisionItemId">@task.Volume.ToString("N")
                            </td>
                            <td class="text-center">@task.UnitQuantity
                            </td>
                            <td class="text-right">@agg.ToString("N")
                            </td>
                            <td class="text-right">@((agg / targetvolume * 100).ToString("N"))
                                %
                            </td>
                        </tr>
                        
        Next
    Next
                </tbody>
                <tfoot>
                    <tr style="background-color: #fff;">
                        <td colspan="4" class="text-center text-bold">
                            T O T A L
                        </td>
                        <td class="text-right text-bold" colspan="2">
                            @Model.DailyReportTasks.Sum(Function(m) m.Volume).ToString("N")
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
    
End Using
<div class="modal fade" id="dlgProgressForm" role="dialog">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="modalTitle">
                    Edit Data Laporan</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    @Using Html.BeginForm("SaveDetail_A", "Reports", New With {.ProjectInfoID = Model.ProjectTaskDivision.ProjectInfo.Id},
                                         FormMethod.Post, New With {.class = "form-horizontal", .id = "frmTask", .autocomplete = "off"})
                        @Html.Hidden("ID", 0, New With {.id = "editId"})
                        
                        @Html.Hidden("ProjectTaskDivisionItemId")
                        @Html.WriteFormInput(Html.IntegerInput("DayWork", 0, thousandSeparator:="."), "Hari Kerja", lgControlWidth:=2, smControlWidth:=3)
                        
                        @Html.WriteFormInput(Html.TextBox("Volume", 0, New With {.class = "form-control text-right"}), "Volume", lgControlWidth:=3)
                 
                        @Html.WriteFormInput(Html.TextBox("Location", Nothing, New With {.class = "form-control"}), "Lokasi", lgControlWidth:=6)
                        
                    End Using
                </div>
                <div class="modal-footer">
                    <button class="btn btn-primary" id="btnSave">
                        Simpan</button>
                    <button class="btn btn-primary" data-dismiss="modal">
                        Batal</button>
                </div>
            </div>
        </div>
    </div>
</div>
<style>
    .editable
    {
        color: #4124ff;
        font-weight: bold;
        cursor: pointer;
    }
</style>
<script src="../../../../plugins/flot/jquery.flot.js" type="text/javascript"></script>
<script src="../../../../plugins/flot/jquery.flot.resize.js" type="text/javascript"></script>
<script src="../../../../plugins/flot/jquery.flot.categories.js" type="text/javascript"></script>
<script type="text/javascript">
    var _loaddata = function () {

        var taskId = $("#TaskId").val();

        $.ajax({
            url: '/ProjectManagement/TaskSummary/GetProgressChartData',
            data: { taskId: taskId },
            type: 'POST',
            success: function (d) {
                var _gridOption = {
                    lines: {
                        show: true,
                        barWidth: 0.6,
                        align: "center"
                    },
                    points: {
                        show: false
                    },
                    xaxis: {
                        mode: "categories",
                        tickLength: 10


                    },
                    yaxis: {
                        min: 0,
                        max: 100
                    },
                    legend: {
                        show: true,
                        position: "se",
                        margin: 10,
                        backgroundColor: "#00ffff",
                        labelFormatter: function (label, series) {
                            //                    // series is the series object for the label
                            //                    return '<a href="#' + label + '">' + label + '</a>';
                            return label;
                        }
                    }

                };


                var placeholder = $("#chartdata");
                var plot = $.plot(placeholder, [d.data], _gridOption);
                plot.draw();

            },
            error: ajax_error_callback,
            datatype: 'json'
        });
    }



    $(function () { //init
        $('#Volume').number(false, 2, ".", ",");
        _loaddata()
        $("#btnSave").click(function (d) {

            var _data = $("#frmTask").serialize();
            var _url = $("#frmTask").attr("action");
            var isNew = $("#editId").val() == 0;

            $.ajax({
                url: _url,
                data: _data,
                type: 'POST',
                success: function (data) {
                    if (data.stat == 1) {
                        showNotification("Data tersimpan");

                        $("#dlgProgressForm").modal('hide');
                        window.location.reload(true);

                    } else {
                        showNotificationSaveError(data, "Menyimpan data unit kerja");
                    }

                },
                error: ajax_error_callback,
                datatype: 'json'
            });

        });

        $("body").delegate(".editable", "click", function () {
            $("#editId").val($(this).data("id"));
            $("#ProjectTaskDivisionItemId").val($(this).data("projecttaskdivisionitemid"));
            $("#DayWork").val($(this).data("daywork"));
            $("#Volume").val($(this).text().trim());
            $("#Location").val($(this).data("location"));
            if (parseInt($(this).data("id")) > 0) {
                $("#DayWork").attr("readonly", "readonly")
            }
            $("#dlgProgressForm").modal();
        });
    });             //end init
</script>
