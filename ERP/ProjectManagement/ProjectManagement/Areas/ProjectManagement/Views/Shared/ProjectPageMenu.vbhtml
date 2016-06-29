@modeltype ProjectManagement.ProjectInfo
@code
    ViewData("Title") = "Informasi Proyek"
End Code
<h4 class="">@Html.Raw(Model.ProjectTitle) ( @Html.Raw(Model.ProjectCode) )
</h4>
<div style="height: 10px">
</div>
<div class="row page-header">
    <div class="col-sm-12 col-lg-12" style="background-color: White; border-radius: 15px;
        font-weight: bold">
        <ul class="nav nav-pills">
            <li role="presentation" @Html.SetActiveClass("Detail", "ProjectInfo") >
                @Html.ActionLink("Detail", "Detail", "ProjectInfo", New With {.id = Model.Id}, Nothing)
            </li>
            <li role="presentation"  @Html.SetActiveClass("Index", "Budget") >
            <a class="dropdown-toggle" href="#" role="button" data-toggle="dropdown">
                Anggaran Proyek<span class="caret"></span></a>
                <ul class="dropdown-menu">
                    <li role="presentation" @Html.SetActiveClass("Index", "Budget") >
                        @Html.ActionLink("Renc.Anggaran Fisik", "Index", "Budget", New With {.id = Model.Id}, Nothing)
                    </li>
                    <li class="divider">
                    &nbsp;
                    </li>
                    <li role="presentation">
                         @Html.ActionLink("Mutual Check 0", "MutualCheck0", "Budget", New With {.id = Model.Id}, Nothing)
                    </li>
                    <li class="divider"></li>
                    <li role="presentation" @Html.SetActiveClass("Index", "ProjectFundSummary")>
                        @Html.ActionLink("Rekap Kebutuhan Dana", "ProjectFundSummary", "Finance", New With {.id = Model.Id}, Nothing)
                    </li>
                </ul>
                
            </li>
            <li role="presentation" @Html.SetActiveClass("Index", "Timesheet")  @Html.SetActiveClass("TimesheetMC0", "Timesheet") >
               

                 <a class="dropdown-toggle" href="#" role="button" data-toggle="dropdown">Timesheet <span
                    class="caret"></span></a>
                <ul class="dropdown-menu">
                    <li role="presentation" @Html.SetActiveClass("Index", "Timesheet") >
                         @Html.ActionLink("Timesheet", "Index", "Timesheet", New With {.id = Model.Id}, Nothing)
                    </li>
                    <li role="presentation" @Html.SetActiveClass("TimesheetMC0", "Timesheet") >
                        @Html.ActionLink("Timesheet MC0", "TimesheetMC0", "Timesheet", New With {.id = Model.Id}, Nothing)
                    </li>
                </ul>
            </li>
            <li role="presentation"  @Html.SetActiveClass("Index", "EquipmentSchedule") @Html.SetActiveClass("Index", "Material") @Html.SetActiveClass("Index", "Print") >
                <a class="dropdown-toggle" href="#" role="button" data-toggle="dropdown">Schedule <span
                    class="caret"></span></a>
                <ul class="dropdown-menu">
                    <li role="presentation" @Html.SetActiveClass("Index", "EquipmentSchedule") >
                        @Html.ActionLink("Peralatan", "Index", "EquipmentSchedule", New With {.id = Model.Id}, Nothing)
                    </li>
                    <li role="presentation" @Html.SetActiveClass("Index", "Material") >
                        @Html.ActionLink("Bahan", "Index", "Material", New With {.id = Model.Id}, Nothing)
                    </li>
                    <li role="separator" class="divider"></li>
                    <li role="presentation" @Html.SetActiveClass("Index", "Print") >
                        @Html.ActionLink("Print", "Index", "Print", New With {.id = Model.Id}, Nothing)
                    </li>
                </ul>
            </li>
            <li role="presentation" @Html.SetActiveClass("DailyReport", "Reports")
             @Html.SetActiveClass("WeeklyReport", "Reports") @Html.SetActiveClass("DetailSummary", "TaskSummary")
             @Html.SetActiveClass("Index", "TaskSummary") @Html.SetActiveClass("MonthlyReport", "Reports") @Html.SetActiveClass("WeeklyReportMC0", "Reports") @Html.SetActiveClass("MonthlyReportMC0", "Reports") @Html.SetActiveClass("ReportsTemplate", "Reports")><a class="dropdown-toggle"
                href="#" role="button" data-toggle="dropdown" aria-haspopup="true">Laporan <span
                    class="caret"></span></a>
                <ul class="dropdown-menu">
                    @*<li role="presentation"><a href="#">Input Laporan</a></li>
                    <li class="divider"></li>*@
                    <li role="presentation" @Html.SetActiveClass("DailyReport", "Reports")>@Html.ActionLink("Harian", "DailyReport", "Reports", New With {.id = Model.Id}, Nothing)</li>
                    <li role="presentation" @Html.SetActiveClass("WeeklyReport", "Reports")>@Html.ActionLink("Mingguan", "WeeklyReport", "Reports", New With {.id = Model.Id}, Nothing)</li>
                    <li role="presentation" @Html.SetActiveClass("MonthlyReport", "Reports")>@Html.ActionLink("Bulanan", "MonthlyReport", "Reports", New With {.id = Model.Id}, Nothing)</li>
                    <li role="separator" class="divider"></li>
                    <li role="presentation" @Html.SetActiveClass("WeeklyReportMC0", "Reports")>@Html.ActionLink("Mingguan MC0", "WeeklyReportMC0", "Reports", New With {.id = Model.Id}, Nothing)</li>
                    <li role="presentation" @Html.SetActiveClass("MonthlyReportMC0", "Reports")>@Html.ActionLink("Bulanan MC0", "MonthlyReportMC0", "Reports", New With {.id = Model.Id}, Nothing)</li>
                    <li role="separator" class="divider"></li>
                    <li role="presentation" @Html.SetActiveClass("Index", "TaskSummary") @Html.SetActiveClass("DetailSummary", "TaskSummary")>@Html.ActionLink("Ringkasan", "Index", "TaskSummary", New With {.id = Model.Id}, Nothing)</li>
                    <li role="separator" class="divider"></li>
                    <li role="presentation" @Html.SetActiveClass("ReportsTemplate", "Reports")>@Html.ActionLink("Template Laporan", "ReportsTemplate", "Reports", New With {.id = Model.Id}, Nothing)</li>
                </ul>
            </li>
            <li role="presentation"  @Html.SetActiveClass("Index", "Finance")><a class="dropdown-toggle"
                href="#" role="button" data-toggle="dropdown" aria-haspopup="true">Pengeluaran Proyek <span
                    class="caret"></span></a>
                <ul class="dropdown-menu">
                    <li role="presentation" @Html.SetActiveClass("Index", "Finance")>
                    @Html.ActionLink("Pengeluaran", "Index", "Finance", New With {.id = Model.Id}, Nothing)
                    
                </ul>
            </li>
        </ul>
    </div>
</div>
