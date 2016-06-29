Namespace ProjectManagement.Areas.ProjectManagement.Controllers
    Public Class TaskSummaryController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /ProjectManagement/TaskSummary
        Dim pmEntities As ProjectManagement_ERPEntities
        Dim UTF8Encoding As Object

        Function Index(ByVal Id As Integer) As ActionResult


            Dim p As ProjectInfo = pmEntities.ProjectInfoes.Where(Function(m) m.Id = Id).SingleOrDefault()
            If p Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            ViewData("ProjectInfo") = p

            Dim Summary = pmEntities.ExecuteStoreQuery(Of ProjectTaskDoneSummary)("EXEC PMn.ProjectTaskDoneSummary @ProjectId ",
                                                                                  New SqlClient.SqlParameter("@projectId", Id)).ToList()

            Dim sm = From m In Summary
                     Order By m.ord
                     Group By m.DivisionNumber, m.DivisionTitle Into g = Group
                    Select New With {DivisionNumber, DivisionTitle, g}









            Return View(Summary)
        End Function
        Public Function DetailSummary(ByVal taskId As Integer) As ActionResult
            Dim model = pmEntities.ProjectTaskDivisionItems.Where(Function(m) m.Id = taskId).SingleOrDefault

            Dim result = From task In model.DailyReportTasks
                         Select New With {task, .Date = task.ProjectTaskDivisionItem.ProjectTaskDivision.ProjectInfo.DateStart.AddDays(task.DailyReport.DayWork)}






         
            Return View(model)
        End Function


        <HttpPost()>
        Public Function GetProgressChartData(ByVal taskId As Integer) As ActionResult

            Dim model = pmEntities.ProjectTaskDivisionItems.Where(Function(m) m.Id = taskId).SingleOrDefault
            Dim Summary = pmEntities.ExecuteStoreQuery(Of ProjectTaskDoneSummary)("EXEC PMn.ProjectTaskDoneSummary @ProjectId ",
                                                                               New System.Data.SqlClient.SqlParameter("@projectId",
                                                                                                                      model.ProjectTaskDivision.ProjectInfo.Id)).ToList()
            Dim progressArr = (From m In model.DailyReportTasks
                              Order By m.DailyReport.DayWork
                              Select New With {m.DailyReport.DayWork, m.Volume}).ToList

            Dim FirstDaywork = progressArr.Min(Function(m) m.DayWork)
            Dim LastDayWork = progressArr.Max(Function(m) m.DayWork)

            If Not FirstDaywork.HasValue Then
                Return Json(New With {.data = New ArrayList})
            End If

            Dim SummaryItem = Summary.Find(Function(m) m.TaskId = taskId)
            Dim targetVolume = SummaryItem.TargetQuantity
            Dim d As New ArrayList
            Dim aggVolume As Double = 0
            Dim theNumber As Integer = 0
            Dim counter As Integer = 0
            d.Add({0, 0})
            For i As Integer = FirstDaywork To LastDayWork
                counter += 1
                theNumber = i
                Dim item = progressArr.Find(Function(m) m.DayWork = theNumber)
                If item IsNot Nothing Then
                    aggVolume += Math.Round((item.Volume / targetVolume) * 100, 2)
                End If
                d.Add({counter, aggVolume})

            Next


            Return Json(New With {.data = d.ToArray()})
        End Function
        Public Sub New()
            pmEntities = New ProjectManagement_ERPEntities
        End Sub
    End Class
End Namespace
