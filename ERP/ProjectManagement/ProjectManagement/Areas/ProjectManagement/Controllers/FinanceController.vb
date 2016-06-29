Imports Microsoft.Reporting.WebForms
Namespace ProjectManagement.Areas.ProjectManagement.Controllers
    Public Class FinanceController
        Inherits System.Web.Mvc.Controller

        Dim prmEntities As ProjectManagement_ERPEntities
        '
        ' GET: /ProjectManagement/Finance

        Function Index(ByVal id As Integer) As ActionResult
            Dim p As ProjectInfo = prmEntities.ProjectInfoes.Where(Function(m) m.Id = id).SingleOrDefault()
            If p Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            Return View(p)
        End Function


        <HttpPost()>
        Function GetCostList(ByVal Id As Integer) As ActionResult
            Dim xmodel = (From m In prmEntities.ProjectCostRecords
                         Where m.ProjectInfoId.Equals(Id)
                         Join n In prmEntities.ProjectCostRecordItems On
                         m.Id Equals n.ProjectCostRecordId
                         Select New With {.ProjectCostRecordId = m.Id, m.PacketCode, m.ProjectInfoId,
                                          m.TaskGroupTitle, m.TransactionDate, m.RecordedBy, m.Cashier,
                                          n.Id, n.ItemDescription, n.PostCategory, n.Value, n.Notes}).ToList



            Dim model = (From m In xmodel
                        Select New With {m.ProjectCostRecordId, m.PacketCode, m.ProjectInfoId,
                                          m.TaskGroupTitle, m.TransactionDate, m.RecordedBy, m.Cashier,
                                          .WeekDateStart = m.TransactionDate.AddDays(Microsoft.VisualBasic.FirstDayOfWeek.Sunday -
                                                                                     m.TransactionDate.DayOfWeek),
                                          m.Id, m.ItemDescription, m.PostCategory, m.Value, m.Notes}).ToList

            Dim dateStart As Date?
            Dim dateEnd As Date?
            If model.Count > 0 Then

                Dim weekday = model.First.TransactionDate.DayOfWeek
                dateStart = model.First.TransactionDate.AddDays(Microsoft.VisualBasic.FirstDayOfWeek.Sunday - weekday)
                dateEnd = model.Last.TransactionDate.AddDays(Microsoft.VisualBasic.FirstDayOfWeek.Sunday - weekday).AddDays(6)

            End If


            Dim TotalAmount = model.Sum(Function(m) m.Value)
            Return Json(New With {.data = model.OrderBy(Function(m) m.TransactionDate), dateStart, dateEnd, TotalAmount})
        End Function




        Function ValidateCostItem(ByVal item As ProjectCostRecordItem) As ActionResult
            If ModelState.IsValid Then
                Return Json(New With {.stat = 1})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function
        <HttpPost()>
        Function DeleteCostItem(ByVal Id As Integer) As ActionResult
            Dim model = prmEntities.ProjectCostRecordItems.Where(Function(m) m.Id = Id).SingleOrDefault()
            If model Is Nothing Then
                Return Json(New With {.stat = 1})
            End If
            prmEntities.ProjectCostRecordItems.DeleteObject(model)
            prmEntities.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function
        Function SaveCashForm(ByVal header As ProjectCostRecord, ByVal Items As String) As ActionResult
            If ModelState.IsValid Then
                Dim CostItems As New List(Of ProjectCostRecordItem)
                Newtonsoft.Json.JsonConvert.PopulateObject(Items, CostItems)
                If CostItems.Count = 0 Then
                    ModelState.AddModelError("general", "Item belum ditambahkan")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If

                header.RecordedBy = User.Identity.Name
                header.Cashier = "-"
                prmEntities.ProjectCostRecords.AddObject(header)
                If header.Id > 0 Then
                    prmEntities.ObjectStateManager.ChangeObjectState(header, EntityState.Modified)
                End If


                For Each item In CostItems
                    header.ProjectCostRecordItems.Add(item)

                    If item.Id > 0 Then
                        prmEntities.ObjectStateManager.ChangeObjectState(item, EntityState.Modified)
                    End If
                Next
                prmEntities.SaveChanges()
                Return Json(New With {.stat = 1, .ProjectCostRecordId = header.Id})
            End If


            Return Json(New With {.stat = -0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function


        <HttpGet()>
        Function PostingTypes(ByVal query As String) As ActionResult
            Dim posts = (From m In prmEntities.ProjectCostRecordItems
                        Select m.PostCategory).Distinct()

            If posts.Count = 0 Then
                Return Nothing
            End If
            query = query.ToLower()
            Dim rvalue = posts.Where(Function(m) m.ToLower().Contains(query))
            Return Json(rvalue, JsonRequestBehavior.AllowGet)
        End Function

        <HttpGet()>
        Function TaskTypes(ByVal query As String) As ActionResult
            Dim posts = (From m In prmEntities.ProjectCostRecords
                        Select m.TaskGroupTitle).Distinct()

            If posts.Count = 0 Then
                Return Nothing
            End If
            query = query.ToLower()
            Dim rvalue = posts.Where(Function(m) m.ToLower().Contains(query))
            Return Json(rvalue, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()>
        Function DeleteProjectCost(ByVal id As Integer) As ActionResult
            Dim query = <query>
                            DELETE FROM PMn.ProjectCostRecordItem WHERE ProjectCostRecordId=@id;
                            DELETE FROM PMn.ProjectCostRecord WHERE ID=@id;
                        </query>.Value()
            Dim result = prmEntities.ExecuteStoreCommand(query, New SqlClient.SqlParameter("@Id", id))
            Return Json(New With {.stat = 1})
        End Function

        Function PrintProjectCost(ByVal Id As Integer) As ActionResult

            Dim p As ProjectInfo = prmEntities.ProjectInfoes.Where(Function(m) m.Id = Id).SingleOrDefault()
            If p Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            Dim model = prmEntities.ExecuteStoreQuery(Of ProjectCostListView)("EXEC  [PMn].[GetProjectCostList] @projectId",
                                                                    New SqlClient.SqlParameter("projectId", Id)).ToArray


            Dim mimeType As String = Nothing
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing
            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.ReportPath = Server.MapPath("~/Areas/ProjectManagement/Reports/ProjectCostSummary.rdl")


            Dim ds2 = prmEntities.ProjectMutualCheck0.Where(Function(m) m.ProjectInfoId = Id)
            r.SetParameters(New ReportParameter("ProjectInfoId", Id))
            r.SetParameters(New ReportParameter("NamaProyek", p.ProjectTitle))
            r.SetParameters(New ReportParameter("PeriodeStart", model.First.WeekDateStart))
            r.SetParameters(New ReportParameter("PeriodeEnd", model.Last.WeekDateStart.AddDays(7)))
            r.DataSources.Add(New ReportDataSource("CostList", model))


            r.Refresh()
            Dim reportType As String = "PDF"

            Dim devinfo As String = "<DeviceInfo>" +
            "<ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>8.27in</PageWidth>" +
            "  <PageHeight>11.69</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.25in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>"

            Dim output() As Byte = r.Render(reportType, devinfo, mimeType, Encoding, fileNameExtension, streams, warnings)
            Return File(output, mimeType, "ProjectCostList." & fileNameExtension)

        End Function

#Region "Rekap Kebutuhan Dana"
        ''' <summary>
        ''' Rekap kebutuhan dana adalah jumlah dana yang dibutuhkan untuk project dibagi atas periode tertentu,
        ''' berdasarkan timesheet project
        ''' </summary>
        ''' <remarks></remarks>
        ''' 

        Public Function ProjectFundSummary(ByVal Id As Integer) As ActionResult
            Dim p As ProjectInfo = prmEntities.ProjectInfoes.Where(Function(m) m.Id = Id).SingleOrDefault()
            If p Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            Return View(p)
        End Function


        <HttpPost()>
        Public Function GetFundSummaryList(ByVal Id As Integer) As ActionResult
            Dim p As ProjectInfo = prmEntities.ProjectInfoes.Where(Function(m) m.Id = Id).SingleOrDefault()
            If p Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            Dim TimeLineSummary = ProjectTimeSheetViewer.CreateTimesheetFooter(p, prmEntities)

            Return Json(New With {.data = TimeLineSummary.MonthlyWeight})

        End Function

#End Region
        Public Sub New()
            prmEntities = New ProjectManagement_ERPEntities
        End Sub
    End Class
End Namespace
