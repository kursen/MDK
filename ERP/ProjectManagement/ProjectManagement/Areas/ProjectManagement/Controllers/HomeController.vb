Namespace ProjectManagement.Areas.ProjectManagement.Controllers
    Public Class HomeController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /ProjectManagement/Home
        Dim pmn As ProjectManagement_ERPEntities

        Function Index() As ActionResult


            Dim model = (From m In pmn.ProjectInfoes
                         Where m.Archive = False
                     Order By m.DateStart).ToList()

            Dim lsProgress As New Dictionary(Of Int32, Double)
            Dim lsWeekRuns As New Dictionary(Of Int32, Int32)
            For Each item In model
                Dim f = ProjectTimeSheetViewer.CreateTimesheetFooterProjectProgress(item, pmn)
                Dim maxValue = f.WeeklyWeight.Max(Function(m) m.WeightAccumulation)
                Dim maxWeek = f.WeeklyWeight.Where(Function(m) m.WeightAccumulation.Equals(maxValue)).First

                lsProgress.Add(item.Id, maxValue)
                lsWeekRuns.Add(item.Id, maxWeek.Weeknumber)
            Next
            ViewData("itemProgress") = lsProgress
            ViewData("itemWeekMax") = lsWeekRuns
            Return View(model)
        End Function
        Public Function getProjectTimesheetSeriesData(ByVal id As Integer) As ActionResult

            Dim model = (From m As ProjectInfo In pmn.ProjectInfoes
                         Where m.Id = id).SingleOrDefault()


            Dim planData = ProjectTimeSheetViewer.CreateTimesheetFooter(model, pmn)


            Dim planSeries As New ArrayList
            planSeries.Add({"W0", 0})

            For Each item In planData.WeeklyWeight
                Dim arrItem = New ArrayList
                arrItem.Add(item.Weeknumber.ToString("\W#"))
                arrItem.Add(item.WeightAccumulation)
                planSeries.Add(arrItem)
            Next

            Dim realisationSeries As New ArrayList
            Dim realisasationData = ProjectTimeSheetViewer.CreateTimesheetFooterProjectProgress(model, pmn)
            Dim wmax = realisasationData.WeeklyWeight.Max(Function(m) m.WeightAccumulation)

            realisationSeries.Add({"W0", 0})
            For Each item In realisasationData.WeeklyWeight
                
                Dim arrItem = New ArrayList
                arrItem.Add(item.Weeknumber.ToString("\W#"))
                arrItem.Add(item.WeightAccumulation)
                realisationSeries.Add(arrItem)
                If item.WeightAccumulation.Equals(wmax) Then
                    Exit For
                End If
            Next

            Dim dataseries As New ArrayList
            dataseries.Add(New With {.label = "Rencana", .data = planSeries})
            dataseries.Add(New With {.label = "Realisasi", .data = realisationSeries})

            Dim ProjectInfo = New With {model.ProjectTitle, model.Id, model.ContractValue, model.DateStart, .data = dataseries}
            Return Json(ProjectInfo, "text/json", JsonRequestBehavior.AllowGet)
        End Function
        Sub New()
            pmn = New ProjectManagement_ERPEntities
        End Sub
    End Class
End Namespace
