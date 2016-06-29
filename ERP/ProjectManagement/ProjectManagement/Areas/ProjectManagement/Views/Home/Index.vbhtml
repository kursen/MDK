@ModelType List(Of ProjectManagement.ProjectInfo)
@Code
    ViewData("Title") = "Project Management Dashboard"
    ViewData("HeaderIcon") = "fa-dashboard"
End Code
@helper  WriteSummaryPanelItem(ByVal value As String, title As String, icon As String)
    @<div class="panel noborder">
        <div class="panel-heading noborder">
            <!-- panel-btns -->
            <div class="panel-icon icon-@icon">
                <i class="fa fa-@icon"></i>
            </div>
            <div class="media-body">
                <h1 class="mt5">
                    @value</h1>
                <h5 class="md-title nomargin">
                    @title</h5>
            </div>
            <!-- media-body -->
        </div>
        <!-- panel-body -->
    </div>
End Helper
@Functions 
    Public Function WriteContractValue(ByVal value As Decimal) As String
        Dim sValue As String = ""
        Select Case value
            Case Is > 1000000000
                sValue = (value / 1000000000).ToString("#.#0\M")
            Case Is > 1000000
                sValue = (value / 1000000).ToString("#.#0\J")
            Case Is > 1000
                sValue = (value / 1000).ToString("#.#0")
            Case Else
                sValue = value.ToString("#,###0.#0")
        End Select
        Return sValue
    End Function
    Public Function GetProgress(ByVal Id As Int32) As String
        Dim dc = CType(ViewData("itemProgress"), Dictionary(Of Int32, Double))
        If dc.Count = 0 Then
        Return "0,00%"    
        End If
        Dim value = dc.Item(Id)
        
        Return value.ToString("#0.#0\%")
    End Function
    Public Function GetMaxWeek(ByVal id As Int32) As String
        Dim dc = CType(ViewData("itemWeekMax"), Dictionary(Of Int32, int32))
        If dc.Count = 0 Then
            Return "0"
        End If
        Dim value = dc.Item(id)
        Return value.ToString()
    End Function
End Functions
@helper WriteTabContent(ByVal m As ProjectManagement.ProjectInfo, isActive As Boolean)
    Dim tabId = "tab_" & m.Id
    Dim chartId = "chart" & m.Id
    Dim active = IIf(isActive, "active", "")
   
    @<div class="tab-pane @active" id="@tabId">
        <div class="row">
            <div style="margin-top: 30px">
                <div class="col-md-3">
                    @WriteSummaryPanelItem(GetProgress(m.Id), "Penyelesaian", "dashboard")
                </div>
                <div class="col-md-3">
                    @WriteSummaryPanelItem(Math.Round(m.NumberOfDays / 7, 0).ToString(), "Minggu Target", "calendar-o")
                </div>
                <div class="col-md-3">
                    @WriteSummaryPanelItem(GetMaxWeek(m.Id), "Minggu Progres", "calendar")
                </div>
                <div class="col-md-3">
                    @WriteSummaryPanelItem(WriteContractValue(m.ContractValue), "Nilai Proyek", "money")
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12 col-sm-12">
                <a class="btn btn-danger pull-right" href="/projectmanagement/projectinfo/detail/@m.Id">Detail..</a>
            
            </div>
        
        </div>
        <div class="row">
            <div class="col-lg-12 col-sm-12">
                <div class="panel" style="padding: 30px;">
                    <div id="@chartId" style="height: 350px;width:auto">
                    </div>
                </div>
            </div>
        </div>
        
        <div class="clearfix">
        </div>
    </div>
End Helper
<script src="../../../../plugins/flot/jquery.flot.js" type="text/javascript"></script>
<script src="../../../../plugins/flot/jquery.flot.resize.js" type="text/javascript"></script>
<script src="../../../../plugins/flot/jquery.flot.categories.js" type="text/javascript"></script>
<style type="text/css">
    .row-stat .panel .panel-heading
    {
        -moz-border-radius: 3px;
        -webkit-border-radius: 3px;
        border-radius: 3px;
    }
    
    .panel-icon
    {
        background: rgba(255,255,255,0.9);
        -moz-border-radius: 3px;
        -webkit-border-radius: 3px;
        border-radius: 50%;
        width: 40px;
        height: 40px;
        float: left;
        margin-right: 15px;
        text-align: center;
    }
    
    .panel-icon.icon-calendar-o
    {
        background-color: #FA8564;
    }
    
    .panel-icon.icon-dashboard
    {
        background-color: #1FB5AD;
    }
    
    .panel-icon.icon-calendar
    {
        background-color: #A48AD4;
    }
    
    .panel-icon.icon-money
    {
        background-color: #AEC785;
    }
    .panel-icon .fa
    {
        color: #fff;
        font-size: 26px;
        padding: 7px 0 0 0;
    }
    
    .row-stat .md- title
    {
        opacity: 0.6;
        font-size: 12px;
        margin-bottom: 5px;
    }
    
    .row-stat h1
    {
        margin-bottom: 0;
    }
    
    .row-stat hr
    {
        opacity: 0.3;
        margin: 15px 0 0 0;
        border-width: 2px;
    }
    
    .sublabel
    {
        font-size: 11px;
        display: block;
        margin-bottom: 3px;
    }
    .mt5
    {
        font-weight: bold;
        text-align: right;
    }
    .md-title
    {
        text-align: right;
    }
</style>
<div class="row-fluid">
    <div style="background: #3575A0">
        <div id="dashboard_tabs" class="col-xs-12 col-sm-10 tab-content">
        @If Model.Count = 0 Then
            Dim m = New ProjectManagement.ProjectInfo
            m.Id = 0
            m.ContractValue = 0.0F
                @WriteTabContent(m, True)
        Else
            For Each item In Model
                @WriteTabContent(item, item.Equals(Model.First))
            Next
        End If
          
            <div class="clearfix">
            </div>
        </div>
        <div id="dashboard_links" class="col-xs-12 col-sm-2 " style="">
            <ul class="nav nav-pills nav-stacked">
                @For Each item In Model
                    Dim className = ""
                    Dim href = "#tab_" & item.Id
                    If item.Equals(Model.First) Then
                        className = "active"
                    End If
                    @<li class="@className"><a href="@href" class="tab-link" data-toggle="tab" data-projectid="@item.Id">@item.ProjectTitle.ToUpper()</a></li>    
                Next
                <li class=""><a href="/ProjectManagement/ProjectInfo" class="tab-link">Lainnya... </a>
                </li>
            </ul>
        </div>
        <div class="clearfix">
        </div>
    </div>
</div>
<script type="text/javascript">




    var _loaddata = function (_ProjectId) {



        $.ajax({
            url: '/ProjectManagement/Home/getProjectTimesheetSeriesData',
            data: { id: _ProjectId },
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
                        tickLength: 10,
                        ticks: []


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

                var lenData = d.data[0].data.length - 1;
                _gridOption.xaxis.ticks.push([1, "Week 1"]);
                for (var i = 5; i < lenData; i += 5) {
                    _gridOption.xaxis.ticks.push([i, i]);
                }
                _gridOption.xaxis.ticks.push([lenData, lenData]);
                var placeholder = $("#chart" + _ProjectId);
                var plot = $.plot(placeholder, d.data, _gridOption);
                plot.draw();

            },
            error: ajax_error_callback,
            datatype: 'json'
        });
    }



    $(function () {  //init

        $("#dashboard_links a").click(function () {
            if (!$(this).data("loaded")) {
                var _id = $(this).data("projectid");
                _loaddata(_id);
                $(this).data("loaded", true);
            }


        });



        var firstTab = $("#dashboard_links a:first");
        var _id = $(firstTab).data("projectid");
        if (_id != null) {
            _loaddata($(firstTab).data("projectid"));
        }
        else {
            $("#chart0").append("<span id='#myDialog'>TIDAK ADA DATA PROJECT</span>");
          
             
        }






    });
</script>
