Imports System.Web.Helpers
Imports System.Drawing
Namespace ProjectManagement.Areas.ProjectManagement.Controllers
    Public Class TimesheetController
        Inherits System.Web.Mvc.Controller
        Dim prm As ProjectManagement_ERPEntities
        '
        ' GET: /ProjectManagement/Timesheet

        Function Index(ByVal id As Integer) As ActionResult

            Dim model = prm.ProjectInfoes.Where(Function(m) m.Id = id).SingleOrDefault()

            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            If model.ProjectTaskDivisions.Count = 0 Then
                ViewData("errorReason") = "Rencana Anggaran Fisik belum dibuat."
                Return View("TimesheetNotAvailable", model)
            End If
            For Each item In model.ProjectTaskDivisions
                If item.ProjectTaskDivisionItems.Count = 0 Then
                    ViewData("errorReason") = String.Format("Item untuk Divisi {0} tidak ada", item.DivisionNumber)
                    Return View("TimesheetNotAvailable", model)
                End If
            Next

            Return View(model)
        End Function


        Public Function GetTimeSheetContent(ByVal Id As Integer) As ActionResult
            Dim tblTimesheet = ProjectTimeSheetViewer.GetTimesheetTable(Id)
            Dim ds As New DataSet
            tblTimesheet.TableName = "data"
            ds.Tables.Add(tblTimesheet)

            Return Content(Newtonsoft.Json.JsonConvert.SerializeObject(ds), "text/json")
        End Function
        Public Function GetTimesheetFooter(ByVal id As Integer) As ActionResult
            Dim model = prm.ProjectInfoes.Where(Function(m) m.Id = id).SingleOrDefault()
            Dim rvalue = ProjectTimeSheetViewer.CreateTimesheetFooter(model, prm)
            Return Json(rvalue, "text/json", JsonRequestBehavior.AllowGet)
        End Function


        Public Function SaveWeight(ByVal Itemid As Integer, weekNumber As Integer, value As String, taskweight As String) As ActionResult
            If String.IsNullOrEmpty(value) Then
                value = "0"
            End If
            Dim weekvalue As Double
            If Double.TryParse(value, weekvalue) = False Then
                Return Json(New With {.stat = 0, .error = "Masukkan angka"})
            End If
            If weekvalue < 0 Then
                Return Json(New With {.stat = 0, .error = "Angka tidak boleh lebih kecil dari 0."})
            End If
            Dim model = prm.ProjectTaskWeights.Where(Function(m) m.ProjectTaskDivisionItemId = Itemid AndAlso m.WeekNumber = weekNumber).SingleOrDefault


            If weekvalue > 0 Then

                Dim sqlQuery As New System.Text.StringBuilder
                sqlQuery.Append("SELECT Isnull(Sum(P.weight), 0) " & vbCrLf)
                sqlQuery.Append("FROM   pmn.projecttaskweight P " & vbCrLf)
                sqlQuery.Append("WHERE  P.projecttaskdivisionitemid = @itemid")
                Dim TaskMaxWeight As Double
                Double.TryParse(taskweight, TaskMaxWeight)
                Dim sumOfWeight = prm.ExecuteStoreQuery(Of Double)(sqlQuery.ToString(), New SqlClient.SqlParameter("@itemid", Itemid)).SingleOrDefault
                If model IsNot Nothing Then
                    'we need to reduce the sum of weight with model.weight
                    sumOfWeight = sumOfWeight - model.Weight
                End If

                If TaskMaxWeight < sumOfWeight + weekvalue Then
                    Dim maxWeight = TaskMaxWeight - sumOfWeight
                    Return Json(New With {.stat = 0,
                                          .error = String.Format("Nilai bobot minggu terlalu besar ({0:N2}). " &
                                                                 " Tidak boleh lebih dari {1:N2}", weekvalue, maxWeight)})
                End If
            End If


            If model Is Nothing Then
                model = New ProjectTaskWeight
                prm.ProjectTaskWeights.AddObject(model)
            End If
            If weekvalue = 0 Then
                prm.ProjectTaskWeights.DeleteObject(model)
            Else
                model.WeekNumber = weekNumber
                model.ProjectTaskDivisionItemId = Itemid
                model.Weight = value
            End If

            prm.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function

        Public Function SaveColor(ByVal TaskItemId As Integer, RowBackgroundColor As String, CellBackgroundColor As String, CellColor As String) As ActionResult
            Dim model = prm.ProjectTaskDivisionItemVisualColors.Where(Function(m) m.ProjectTaskDivisionItemId = TaskItemId).SingleOrDefault()
            If model Is Nothing Then
                model = New ProjectTaskDivisionItemVisualColor
                model.ProjectTaskDivisionItemId = TaskItemId
                prm.ProjectTaskDivisionItemVisualColors.AddObject(model)
            End If
            model.CellBackgroundColor = CellBackgroundColor.Replace("#", String.Empty)
            model.RowBackgroundColor = RowBackgroundColor.Replace("#", String.Empty)
            model.CellTextColor = CellColor.Replace("#", String.Empty)
            Dim C1 = System.Drawing.ColorTranslator.FromHtml(CellBackgroundColor)

            prm.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function

        Function PrintTimeSheet(ByVal projectid As Integer) As ActionResult

            Dim Printer = New ProjectTimesheetPrinter(prm, projectid)
            Dim ioMem = New IO.MemoryStream
            Printer.PrintTimeSheet.Save(ioMem, Drawing.Imaging.ImageFormat.Png)
            Return File(ioMem.GetBuffer(), "image/png", "ProjectTimeSheetSchedule.png")



        End Function


#Region "Timesheet MC0"

        Public Function TimesheetMC0(ByVal id As Integer) As ActionResult



            Dim model = prm.ProjectInfoes.Where(Function(m) m.Id = id).SingleOrDefault()

            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            If model.ProjectTaskDivisions.Count = 0 Then
                ViewData("errorReason") = "Rencana Anggaran Fisik belum dibuat."
                Return View("TimesheetNotAvailable", model)
            End If
            For Each item In model.ProjectTaskDivisions
                If item.ProjectTaskDivisionItems.Count = 0 Then
                    ViewData("errorReason") = String.Format("Item untuk Divisi {0} tidak ada", item.DivisionNumber)
                    Return View("TimesheetNotAvailable", model)
                End If
            Next

            Return View(model)
        End Function

        Public Function GetTimeSheetContentMC0(ByVal Id As Integer) As ActionResult
            Dim tblTimesheet = ProjectTimeSheetViewer.GetTimesheetTableMC0(Id)
            Dim ds As New DataSet
            tblTimesheet.TableName = "data"
            ds.Tables.Add(tblTimesheet)
            Dim c1 = tblTimesheet.Columns.Add("RowBackgroundColor", GetType(String))
            c1.DefaultValue = "#ffffff"
            Dim c2 = tblTimesheet.Columns.Add("CellTextColor", GetType(String))
            c2.DefaultValue = "#000000"

            Dim c3 = tblTimesheet.Columns.Add("CellBackgroundColor", GetType(String))
            c3.DefaultValue = "#ffffff"
            Dim sql = <sql>
                          SELECT c.ID,
                                    [ProjectTaskDivisionItemId]
                                    ,[CellBackgroundColor]
                                    ,[CellTextColor]
                                    ,[RowBackgroundColor]
                           FROM [PMn].[ProjectTaskDivisionItemVisualColor] c
                                    inner join [PMn].ProjectTaskDivisionItem i
                                    on c.ProjectTaskDivisionItemId = i.Id
                                    inner join [PMn].ProjectTaskDivision t
                                    on i.ProjectTaskDivisionId = t.Id
                                    where t.ProjectInfoId=@projectInfoId

                      </sql>.Value
            Dim colorItems = prm.ExecuteStoreQuery(Of ProjectTaskDivisionItemVisualColor)(sql, New SqlClient.SqlParameter("@projectInfoId", Id)).ToArray

            For Each item In colorItems
                Dim drow = tblTimesheet.Rows.Find(item.ProjectTaskDivisionItemId)
                If drow IsNot Nothing Then
                    drow("RowBackgroundColor") = item.RowBackgroundColor
                    drow("CellBackgroundColor") = item.CellBackgroundColor
                    drow("CellTextColor") = item.CellTextColor
                    drow.AcceptChanges()
                End If
            Next
            tblTimesheet.AcceptChanges()

            Return Content(Newtonsoft.Json.JsonConvert.SerializeObject(ds), "text/json")
        End Function

        Public Function GetTimesheetFooterMC0(ByVal id As Integer) As ActionResult
            Dim model = prm.ProjectInfoes.Where(Function(m) m.Id = id).SingleOrDefault()
            Dim rvalue = ProjectTimeSheetViewer.CreateTimesheetFooterMC0(model, prm)
            Return Json(rvalue, "text/json", JsonRequestBehavior.AllowGet)
        End Function
        Function PrintTimeSheetMC0(ByVal projectid As Integer) As ActionResult

            Dim Printer = New ProjectTimesheetPrinter(prm, projectid)
            Dim ioMem = New IO.MemoryStream
            Printer.PrintTimeSheetRevision(0).Save(ioMem, Drawing.Imaging.ImageFormat.Png)
            Return File(ioMem.GetBuffer(), "image/png", "ProjectTimeSheetSchedule.png")

        End Function

        Public Function SaveWeightMC0(ByVal Itemid As Integer, weekNumber As Integer, value As String, taskweight As String) As ActionResult
            If String.IsNullOrEmpty(value) Then
                value = "0"
            End If
            Dim weekvalue As Double
            If Double.TryParse(value, weekvalue) = False Then
                Return Json(New With {.stat = 0, .error = "Masukkan angka"})
            End If
            If weekvalue < 0 Then
                Return Json(New With {.stat = 0, .error = "Angka tidak boleh lebih kecil dari 0."})
            End If
            Dim model = prm.ProjectTaskWeight_MutualCheck0.Where(Function(m) m.ProjectTaskDivisionItemId = Itemid AndAlso m.WeekNumber = weekNumber).SingleOrDefault


            If weekvalue > 0 Then

                Dim sqlQuery As New System.Text.StringBuilder
                sqlQuery.Append("SELECT Isnull(Sum(P.weight), 0) " & vbCrLf)
                sqlQuery.Append("FROM   pmn.ProjectTaskWeight_MutualCheck0 P " & vbCrLf)
                sqlQuery.Append("WHERE  P.projecttaskdivisionitemid = @itemid")
                Dim TaskMaxWeight As Double
                Double.TryParse(taskweight, TaskMaxWeight)
                Dim sumOfWeight = prm.ExecuteStoreQuery(Of Double)(sqlQuery.ToString(), New SqlClient.SqlParameter("@itemid", Itemid)).SingleOrDefault
                If model IsNot Nothing Then
                    'we need to reduce the sum of weight with model.weight
                    sumOfWeight = sumOfWeight - model.Weight
                End If

                If Math.Round(TaskMaxWeight, 2) < Math.Round(sumOfWeight + weekvalue, 2) Then
                    Dim maxWeight = TaskMaxWeight - sumOfWeight
                    Return Json(New With {.stat = 0,
                                          .error = String.Format("Nilai bobot minggu terlalu besar ({0:N2}). " &
                                                                 " Tidak boleh lebih dari {1:N2}", weekvalue, maxWeight)})
                End If
            End If


            If model Is Nothing Then
                model = New ProjectTaskWeight_MutualCheck0
                prm.ProjectTaskWeight_MutualCheck0.AddObject(model)
            End If
            If weekvalue = 0 Then
                prm.ProjectTaskWeight_MutualCheck0.DeleteObject(model)
            Else
                model.WeekNumber = weekNumber
                model.ProjectTaskDivisionItemId = Itemid
                model.Weight = value
            End If

            prm.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function
#End Region




        Public Sub New()
            prm = New ProjectManagement_ERPEntities
        End Sub
    End Class
End Namespace
