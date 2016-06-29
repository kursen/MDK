
Imports System.IO

Namespace ProjectManagement.Areas.ProjectManagement.Controllers
    Public Class ProjectInfoController
        Inherits System.Web.Mvc.Controller

        Dim prmEntities As ProjectManagement_ERPEntities

        Public Sub New()
            prmEntities = New ProjectManagement_ERPEntities
        End Sub

        '
        ' GET: /ProjectManagement/ProjectInfo

        Function Index() As ActionResult
            Return View()
        End Function

        Public Function Detail(ByVal Id As Integer) As ActionResult
            Dim p As ProjectInfo = prmEntities.ProjectInfoes.Where(Function(m) m.Id = Id).SingleOrDefault()
            If p Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            Return View(p)
        End Function

#Region "Detail Project"

        Private Function _GetProjectList(ByVal isActive As Boolean) As Object

            Dim projectList = (From p In prmEntities.ProjectInfoes
                             Where p.Archive = Not isActive
                             Select New With {p.Id, p.ProjectCode, p.ContractValue,
                                                           p.ProjectTitle, p.DateStart, p.NumberOfDays}).ToList

            Dim dc As New Dictionary(Of Integer, Double)
            For Each item In projectList
                Dim p As New ProjectInfo With {.Id = item.Id, .DateStart = item.DateStart, .ContractValue = item.ContractValue, .NumberOfDays = item.NumberOfDays}
                Dim f = ProjectTimeSheetViewer.CreateTimesheetFooterProjectProgress(p, prmEntities)
                Dim maxValue = f.WeeklyWeight.Max(Function(m) m.WeightAccumulation)
                dc.Add(item.Id, maxValue)
            Next

            Dim model = From m In projectList
                        Join item In dc On item.Key Equals m.Id
                        Select New With {m.Id, m.ProjectTitle, m.NumberOfDays, m.DateStart, m.ProjectCode, .Progress = item.Value}

            Return model
        End Function

        <HttpPost()>
        Public Function GetActiveProjectList() As ActionResult
           
            Dim model = _GetProjectList(True)
            Return Json(New With {.data = model})
        End Function
        <HttpPost()>
        Public Function GetInActiveProjectList() As ActionResult
            Dim model = _GetProjectList(False)
            Return Json(New With {.data = model})
        End Function


        <HttpGet()>
        Public Function GetCompanyList() As ActionResult
            Dim model = (From p In prmEntities.CompanyInfoes
                        Select New With {.value = p.ID, .text = p.Name})

            Return Json(model, "text/json", JsonRequestBehavior.AllowGet)
        End Function

        <HttpGet()>
        Public Function Create() As ActionResult
            Dim model As New ProjectInfo
            ' model.DateStart = Today
            model.DayUnit = "Hari Kerja"

            Dim Companies = prmEntities.CompanyInfoes.ToArray()
            Dim selectListCompanies = New SelectList(Companies, "ID", "Name")
            ViewData("CompanyInfoId") = selectListCompanies

            Return View(model)
        End Function

        <HttpPost()>
        Public Function SaveProjectInfo(ByVal model As ProjectInfo) As ActionResult

            If ModelState.IsValid Then
                prmEntities.ProjectInfoes.AddObject(model)
                If model.Id > 0 Then
                    prmEntities.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                End If
                prmEntities.SaveChanges()

                Return Json(New With {.stat = 1, .projectId = model.Id})
            End If

            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        <HttpPost()>
        Public Function SavePartial(ByVal pk As Integer, name As String, value As String) As ActionResult
            Dim model = prmEntities.ProjectInfoes.Where(Function(m) m.Id = pk).SingleOrDefault()
            If model Is Nothing Then
                ModelState.AddModelError("General", "Data project tidak ditemukan. Muat ulang halaman ini")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            Select Case name
                Case "DateStart"
                    Dim d As Date
                    Date.TryParse(value, d)
                    model.GetType.GetProperty(name).SetValue(model, d, Nothing)
                Case "CompanyInfoId", "NumberOfDays"
                    Dim i As Integer
                    Integer.TryParse(value, i)
                    model.GetType.GetProperty(name).SetValue(model, i, Nothing)
                Case "ContractValue"
                    Dim d As Decimal
                    Decimal.TryParse(value, d)
                    model.GetType.GetProperty(name).SetValue(model, d, Nothing)
                Case "Archive"
                    Dim b As Boolean
                    Boolean.TryParse(value, b)
                    model.GetType.GetProperty(name).SetValue(model, b, Nothing)
                Case Else
                    model.GetType.GetProperty(name).SetValue(model, value, Nothing)
            End Select

            prmEntities.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function


#End Region


#Region "Upload Doc Detail"
        Public Function GetDocumentList(ByVal ProjectInfoID As Integer) As JsonResult
            Dim documentList = (From p In prmEntities.ProjectDocDetails
                                Where p.ProjectInfoID = ProjectInfoID
                              Select p.ID, p.DocTitle, p.Description, p.FileName, p.DocType).ToList()
            Return Json(New With {.data = documentList}, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()> _
        Public Function SaveUploadDocDetail(ByVal model As ProjectDocDetail, ByVal Files As HttpPostedFileBase) As ActionResult

            '''' This Validation is not use
            'Dim validImageTypes = New String() { _
            '    "application/vnd.ms-excel", _
            '    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _
            '    "application/vnd.openxmlformats-officedocument.presentationml.presentation", _
            '    "application/vnd.ms-powerpointtd", _
            '    "application/vnd.ms-powerpoint", _
            '    "application/vnd.openxmlformats-officedocument.wordprocessingml.document", _
            '    "application/msword", _
            '    "application/pdf", _
            '    "application/zip", _
            '    "application/x-compressed-zip", _
            '    "text/plain"
            '}

            Dim dataExist = (From x In prmEntities.ProjectDocDetails Where x.ID = model.ID Select x).FirstOrDefault()

            If IsNothing(Files) Then
                'ModelState.AddModelError("noFile", "Dokumen wajib dipilih")
            Else
                '''' This Validation is not use
                'Dim strContentType As String = (From c In validImageTypes Where c = Files.ContentType Select c).FirstOrDefault()
                'If IsNothing(strContentType) Then
                '    ModelState.AddModelError("notSameMimeType", "Dokumen yang dipilih tidak valid")
                'Else
                '    Using binaryReader = New BinaryReader(Files.InputStream)
                '        model.DocFile = binaryReader.ReadBytes(Files.ContentLength)
                '    End Using
                '    model.DocType = strContentType
                'End If

                Using binaryReader = New BinaryReader(Files.InputStream)
                    model.DocFile = binaryReader.ReadBytes(Files.ContentLength)
                End Using
                model.DocType = Files.ContentType
            End If

            If ModelState.IsValid Then
                Try
                    'cek is edit or add new
                    If Not IsNothing(dataExist) Then
                        With dataExist
                            .DocTitle = model.DocTitle
                            .Description = model.Description
                            If Not IsNothing(Files) Then
                                .DocFile = model.DocFile
                                .DocType = model.DocType
                            End If
                            .FileName = model.FileName
                        End With
                    Else
                        prmEntities.ProjectDocDetails.AddObject(model)
                    End If
                    prmEntities.SaveChanges()
                    Return Json(New With {.stat = 1})
                Catch ex As Exception
                    ModelState.AddModelError("General", ex.Message)
                End Try
            End If

            'if we got this far so return back the view
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        Public Function DownloadDocDetail(ByVal Id As Integer) As FileResult

            Dim model = (From x In prmEntities.ProjectDocDetails
                        Where x.ID = Id
                        Select x).FirstOrDefault()
            'Save
            Dim stream As New MemoryStream
            Return File(model.DocFile, model.DocType, model.DocTitle & Path.GetExtension(model.FileName))

        End Function

        Function DeleteUploadDocDetail(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In prmEntities.ProjectDocDetails Where pl.ID = id).FirstOrDefault()
                If data IsNot Nothing Then prmEntities.ProjectDocDetails.DeleteObject(data)
                prmEntities.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Got Exception"
                msgDesc = ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function
#End Region


    End Class
End Namespace
