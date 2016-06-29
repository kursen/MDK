Imports Microsoft.Reporting.WebForms

Namespace ProjectManagement.Areas.ProjectManagement.Controllers
    Public Class BudgetController
        Inherits System.Web.Mvc.Controller
        Dim prmEntities As ProjectManagement_ERPEntities
        '
        ' GET: /ProjectManagement/Budget

        Function Index(ByVal Id As Integer) As ActionResult

            Dim p As ProjectInfo = prmEntities.ProjectInfoes.Where(Function(m) m.Id = Id).SingleOrDefault()
            If p Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            Return View(p)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Id">Project Info Id</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpPost()>
        Function GetRAPItems(ByVal Id As Integer) As ActionResult
            Dim thedivision = From m As ProjectTaskDivision In prmEntities.ProjectTaskDivisions
                              Order By m.Ordinal
                              Where m.ProjectInfoId = Id
                              Select New With {.DivisionId = m.Id, m.DivisionNumber, .DivisionTitle = m.TaskTitle,
                                               .DivisionOrdinal = m.Ordinal}

            Dim theItems = From n As ProjectTaskDivisionItem In prmEntities.ProjectTaskDivisionItems
                           Order By n.Ordinal
                           Where n.ProjectTaskDivision.ProjectInfo.Id = Id
                           Select New With {.ItemId = n.Id, n.TaskTitle, n.Quantity, n.UnitQuantity, n.Ordinal, n.Value,
                                            .Total = n.Value * n.Quantity,
                                            n.PaymentNumber, n.WorkDays, n.ProjectTaskDivisionId}

            'Math.Round((n.Value * n.Quantity) / 1000, 0) * 1000,
            Dim x = From Division In thedivision Order By Division.DivisionOrdinal
                Group Join n In theItems On Division.DivisionId Equals n.ProjectTaskDivisionId Into Group

            Return Json(New With {.data = x.ToArray()})
        End Function

        <HttpPost()>
        Public Function SaveDivision(ByVal model As ProjectTaskDivision) As ActionResult

            If ModelState.IsValid Then
                If model.Ordinal = 0 Then

                    Dim sqlMax = "SELECT MAX(ordinal) FROM PMn.ProjectTaskDivision " &
                        "WHERE ProjectInfoId = @projectinfoId"
                    Dim maxOrdinal = prmEntities.ExecuteStoreQuery(Of Integer?)(sqlMax, New SqlClient.SqlParameter("projectInfoId", model.ProjectInfoId)).Single()
                    If maxOrdinal.HasValue Then
                        model.Ordinal = maxOrdinal + 1
                    Else
                        model.Ordinal = 1
                    End If

                End If
                prmEntities.ProjectTaskDivisions.AddObject(model)
                If model.Id > 0 Then
                    prmEntities.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                End If
                prmEntities.SaveChanges()
                Return Json(New With {.stat = 1})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        <HttpPost()>
        Public Function DeleteDivision(ByVal id As Integer) As ActionResult
            Dim sqlDelete = "DELETE FROM PMn.ProjectTaskDivision WHERE ID=@id"
            Dim result = prmEntities.ExecuteStoreCommand(sqlDelete, New SqlClient.SqlParameter("@Id", id))
            If result > 0 Then

            End If

            Return Json(New With {.stat = 1})
        End Function

        <HttpPost()>
        Public Function SaveDivisionOrder(ByVal ids As String, projectInfoId As Integer) As ActionResult
            Dim arrDivision = From m In prmEntities.ProjectTaskDivisions
                              Where m.ProjectInfoId = projectInfoId

            Dim arrIds = ids.Split(",")
            Dim ordinalcounter As Integer = 1
            For Each item In arrIds
                Dim theId As Integer
                If Integer.TryParse(item, theId) Then
                    Dim division = arrDivision.Where(Function(m) m.Id = theId).SingleOrDefault
                    If division IsNot Nothing Then
                        division.Ordinal = ordinalcounter
                        ordinalcounter += 1
                    End If
                End If
            Next
            prmEntities.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function
        <HttpPost()>
        Public Function SaveDivisionItem(model As ProjectTaskDivisionItem) As ActionResult

            If (ModelState.IsValid) Then

                prmEntities.ProjectTaskDivisionItems.AddObject(model)
                If model.Id > 0 Then
                    prmEntities.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                Else
                    If model.Ordinal = 0 Then
                        Dim arrItems = (From m In prmEntities.ProjectTaskDivisionItems
                                        Where m.ProjectTaskDivisionId = model.ProjectTaskDivisionId)
                        If arrItems IsNot Nothing AndAlso arrItems.Count > 0 Then
                            Dim maxOrdinal = arrItems.Max(Function(m) m.Ordinal)
                            maxOrdinal += 1
                            model.Ordinal = maxOrdinal
                        End If

                    End If
                End If

                prmEntities.SaveChanges()
                Return Json(New With {.stat = 1})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        <HttpPost()>
        Public Function SaveDivisionItemOrder(ByVal ids As String, ByVal DivisionId As Integer) As ActionResult
            Dim arrIds = ids.Split(",")
            Dim arrDivision = prmEntities.ProjectTaskDivisionItems
            Dim ordinalcounter As Integer = 1
            For Each item In arrIds
                Dim theId As Integer
                If Integer.TryParse(item, theId) Then
                    Dim division = arrDivision.Where(Function(m) m.Id = theId).SingleOrDefault
                    If division IsNot Nothing Then
                        division.Ordinal = ordinalcounter
                        division.ProjectTaskDivisionId = DivisionId
                        ordinalcounter += 1
                    End If
                End If
            Next
            prmEntities.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function

        <HttpPost()>
        Public Function DeleteDivisionItem(ByVal id As Integer) As ActionResult
            Dim sqlDelete = "DELETE FROM PMn.ProjectTaskDivisionItem WHERE ID=@id"
            Dim result = prmEntities.ExecuteStoreCommand(sqlDelete, New SqlClient.SqlParameter("@Id", id))
            If result > 0 Then

            End If

            Return Json(New With {.stat = 1})
        End Function

        Public Function BudgetReportPrint(ByVal Id As Integer) As ActionResult

            Dim model = prmEntities.GetTimesheetItems(Id)

            Try

                Dim mimeType As String = "application/pdf"
                Dim Encoding As String = Nothing
                Dim fileNameExtension As String = Nothing
                Dim streams As String() = Nothing
                Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

                Dim r As New Microsoft.Reporting.WebForms.LocalReport
                r.EnableExternalImages = True
                r.ReportPath = Server.MapPath("~/Areas/ProjectManagement/Reports/RAP.rdlc")
                'r.ReportPath = System.AppDomain.CurrentDomain.BaseDirectory() & "Reports\ReportBeritaAcara.rdlc"

                If Not IsNothing(model) Then
                    'r.SetParameters(New ReportParameter("projectInfoId", Id))
                    r.DataSources.Add(New ReportDataSource("DSTimeSheetItems", model))
                End If

                r.Refresh()
                Dim reportType As String = "PDF"

                Dim devinfo As String = "<DeviceInfo>" +
                "<ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>0.5in</MarginLeft>" +
                "  <MarginRight>0in</MarginRight>" +
                "  <MarginBottom>0in</MarginBottom>" +
                "</DeviceInfo>"

                Dim output() As Byte = r.Render(reportType, devinfo, mimeType, Encoding, fileNameExtension, streams, warnings)
                Return File(output, mimeType)

            Catch ex As Exception
                ModelState.AddModelError("General", ex.Message)
            End Try

            Return View()
        End Function


#Region "Mutual Check 0"
        Public Function MutualCheck0(ByVal Id As Integer) As ActionResult
            Dim model As ProjectInfo = prmEntities.ProjectInfoes.Where(Function(m) m.Id = Id).SingleOrDefault()
            If model Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            If model.ProjectTaskDivisions.Count = 0 Then
                ViewData("errorReason") = "Rencana Anggaran Fisik belum dibuat."
                Return View("MutualCheck0NotAvailable", model)
            End If
            For Each item In model.ProjectTaskDivisions
                If item.ProjectTaskDivisionItems.Count = 0 Then
                    ViewData("errorReason") = String.Format("Item untuk Divisi {0} tidak ada", item.DivisionNumber)
                    Return View("MutualCheck0NotAvailable", model)
                End If
            Next
            'check if mutualchec0 already exist
            Dim mCheck0 = prmEntities.ProjectMutualCheck0.Where(Function(m) m.ProjectInfoId = Id).SingleOrDefault()
            If mCheck0 Is Nothing Then
                mCheck0 = New ProjectMutualCheck0
                With mCheck0
                    .ProjectInfoId = Id
                    .DocumentDate = Date.Today
                    .DocumentNumber = "-"
                    .Description = "-"
                    .ApprovedByOccupation = "Pejabat Pelaksana Kegiatan Teknis (PPTK)"
                    .ProposedByCompany = model.CompanyInfo.Name
                    .ProposedByOccupation = "General Superintendent"
                    .CheckedByCompany = model.ConsultanName


                End With
                prmEntities.ProjectMutualCheck0.AddObject(mCheck0)
                prmEntities.SaveChanges()
            End If
            ViewData("MutualCheck0") = mCheck0
            Return View(model)
        End Function

        Public Function GetMutualCheck0View(ByVal Id As Integer) As ActionResult
            Dim model = prmEntities.ExecuteStoreQuery(Of MutualCheckView)("EXEC  [PMn].[GetMutualCheck0View] @projectId",
                                                                          New SqlClient.SqlParameter("projectId", Id))
            Return Json(New With {.data = model.OrderBy(Function(m) m.OrdinalSort)}, JsonRequestBehavior.AllowGet)
        End Function

       
        Public Function SaveQuantityMutualCheck0(ByVal name As String, value As String, pk As Integer,
                                                 ByVal ProjectMutualCheck0Id As Integer) As ActionResult
            Dim model = prmEntities.ProjectMutualCheck0Item.Where(Function(m) m.ProjectTaskDivisionItemId = pk).SingleOrDefault()
            If model Is Nothing Then
                model = New ProjectMutualCheck0Item
                model.ProjectMutualCheck0Id = ProjectMutualCheck0Id
                model.ProjectTaskDivisionItemId = pk

                prmEntities.ProjectMutualCheck0Item.AddObject(model)
            End If

            If Double.TryParse(value, model.Quantity) = False Then
                ModelState.AddModelError("MC0Quantity", "Nilai Kuantitas MC0 harus berupa angka")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            prmEntities.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function


        <HttpPost()>
        Public Function SaveMutualCheck0Header(ByVal model As ProjectMutualCheck0) As ActionResult
            If ModelState.IsValid Then
                model.Description = "-"
                prmEntities.ProjectMutualCheck0.AddObject(model)
                prmEntities.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                prmEntities.SaveChanges()
                Return Json(New With {.stat = 1})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function


        Public Function PrintMutualCheck0(ByVal id As Integer) As ActionResult

            Dim p As ProjectInfo = prmEntities.ProjectInfoes.Where(Function(m) m.Id = id).SingleOrDefault()
            If p Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            Dim model = prmEntities.ExecuteStoreQuery(Of MutualCheckView)("EXEC  [PMn].[GetMutualCheck0View] @projectId",
                                                                    New SqlClient.SqlParameter("projectId", id)).ToArray
            'Try

            Dim mimeType As String = "application/pdf"
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing
            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.ReportPath = Server.MapPath("~/Areas/ProjectManagement/Reports/BoQMC0.rdlc")


            Dim ds2 = prmEntities.ProjectMutualCheck0.Where(Function(m) m.ProjectInfoId = id).ToList


            r.SetParameters(New ReportParameter("projectInfoId", id))
            r.SetParameters(New ReportParameter("NamaPaket", p.ProjectTitle))
            r.SetParameters(New ReportParameter("NamaKontraktor", p.CompanyInfo.Name))
            r.SetParameters(New ReportParameter("NomorKontrak", p.ContractNumber))
            r.SetParameters(New ReportParameter("TanggalKontrak", p.DateStart.ToString("dd-MMM-yyyy")))
            r.SetParameters(New ReportParameter("Konsultan", p.ConsultanName))
            r.SetParameters(New ReportParameter("projectInfoId", id))
            r.DataSources.Add(New ReportDataSource("ProjectTaskList", model))
            r.DataSources.Add(New ReportDataSource("FileHeader", ds2))

            r.Refresh()
            Dim reportType As String = "PDF"

            Dim devinfo As String = "<DeviceInfo>" &
            "<ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" &
            "  <PageWidth>20.5in</PageWidth>" &
            "  <PageHeight>15in</PageHeight>" &
            "  <MarginTop>0.5in</MarginTop>" &
            "  <MarginLeft>0.5in</MarginLeft>" &
            "  <MarginRight>0in</MarginRight>" &
            "  <MarginBottom>0in</MarginBottom>" &
            "</DeviceInfo>"


            Dim output() As Byte = r.Render(reportType, devinfo, mimeType, Encoding, fileNameExtension, streams, warnings)
            Return File(output, mimeType, "MC0.PDF")

            'Catch ex As Exception
            '    Throw New HttpException(500, "SERVER ERROR")
            'End Try


            Return View()
        End Function
#End Region

        Public Sub New()
            prmEntities = New ProjectManagement_ERPEntities
        End Sub
    End Class
End Namespace












