Imports Microsoft.Reporting.WebForms
Imports System.Web.Helpers
Imports System.IO

Namespace ProjectManagement.Areas.ProjectManagement.Controllers
    <Authorize()> _
    Public Class ReportsController
        Inherits System.Web.Mvc.Controller
        Dim prmEntities As New ProjectManagement_ERPEntities

        Function GetProjectTaskDivList(ByVal ProjectId As Integer) As SelectList

            Dim query = <sql>SELECT CAST( I.Id  AS VARCHAR(4) ) AS Value, I.PaymentNumber + ' - '+ i.TaskTitle as text FROM PMN.ProjectTaskDivision D
                                INNER JOIN PMN.ProjectTaskDivisionItem I
                                ON D.Id = I.ProjectTaskDivisionId
                                WHERE D.ProjectInfoId = @Id
                                ORDER BY PaymentNumber
                        </sql>.Value

            Dim items = prmEntities.ExecuteStoreQuery(Of ERPBase.OptionItem)(query, New SqlClient.SqlParameter("@id", ProjectId)).ToArray()

            Dim dataList As New SelectList(items, "Value", "Text")

            Return dataList
        End Function

#Region "Daily"
        Function Index() As ActionResult
            Return RedirectToActionPermanent("DailyReport", "Reports")
        End Function

        Function DailyReport(ByVal Id As Integer) As ActionResult
            Dim p As ProjectInfo = prmEntities.ProjectInfoes.Where(Function(m) m.Id = Id).SingleOrDefault()

            ViewData("ProjectTaskDivisionItemId") = GetProjectTaskDivList(Id)

            If p Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            Return View(p)
        End Function

        Function GetDailyReportData(Optional ByVal Id As String = "") As JsonResult
            Dim taskList = (From t In prmEntities.GetDailyReportData(Id)
                            Where t.GroupData <> 0
                            Order By t.GroupData
                            Select t).ToList()

            Return Json(New With {.data = taskList}, JsonRequestBehavior.AllowGet)
        End Function

        Function GetReportListDateItems(ByVal Id As Integer) As JsonResult
            Dim model = prmEntities.GetDailyReportDateList(Id).ToList()

            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function GetMaterialUnit(ByVal Id As Integer) As JsonResult
            Dim model = (From m In prmEntities.Materials Where m.ID = Id Select m.Unit).FirstOrDefault

            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function

        Function GetProjectTaskDivisionUnit(ByVal Id As Integer) As JsonResult
            Dim model = (From ptdi In prmEntities.ProjectTaskDivisionItems Where ptdi.Id = Id Select ptdi.UnitQuantity).FirstOrDefault

            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function

        Public Function DailyOneReportPrint(ByVal ProjectDiv As Integer, ByVal DayWork As Integer) As ActionResult
            Dim DailyRptID = (From x In prmEntities.DailyReports
                        Where x.DayWork = DayWork AndAlso x.ProjectInfoID = ProjectDiv
                        Select x.ID).FirstOrDefault()
            Return RedirectToAction("DailyReportsPrint", "Reports", New With {.IdList = DailyRptID})
        End Function

        Public Function DailyReportsPrint(ByVal IdList As String) As ActionResult

            Dim model = prmEntities.GetDailyReportData(IdList)

            Dim mimeType As String = "application/pdf"
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing
            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            Dim rs As New Microsoft.Reporting.WebForms.ServerReport
            r.EnableExternalImages = True

            r.ReportPath = Server.MapPath("~/Areas/ProjectManagement/Reports/DailyReport.rdlc")
            r.SetParameters(New ReportParameter("IdList", IdList))

            If Not IsNothing(model) Then
                r.DataSources.Add(New ReportDataSource("DsDailyReport", model))
            End If

            r.Refresh()

            Dim reportType As String = "PDF"

            Dim devinfo As String = "<DeviceInfo>" +
            "<ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>8.27in</PageWidth>" +
            "  <PageHeight>11.69in</PageHeight>" +
            "  <MarginTop>0.2in</MarginTop>" +
            "  <MarginLeft>0.2in</MarginLeft>" +
            "  <MarginRight>0.2in</MarginRight>" +
            "  <MarginBottom>0.2in</MarginBottom>" +
            "</DeviceInfo>"

            Dim output() As Byte = r.Render(reportType, devinfo, mimeType, Encoding, fileNameExtension, streams, warnings)
            Return File(output, mimeType)
        End Function
#End Region

#Region "CRUD Daily"

        Function GetDailyReportID(ProjectInfoID As Integer, DayWork As Integer) As Long
            Dim data = (From m In prmEntities.DailyReports
                        Where m.ProjectInfoID = ProjectInfoID AndAlso m.DayWork = DayWork
                        Select m).FirstOrDefault()
            If IsNothing(data) Then
                data = New DailyReport With {.ProjectInfoID = ProjectInfoID, .DayWork = DayWork}
                prmEntities.DailyReports.AddObject(data)
                prmEntities.SaveChanges()
            End If
            Return data.ID
        End Function

        Function GetMaxNum(ByVal Region As Char, ByVal projectInfoID As Integer, ByVal DayWork As Integer) As Integer
            Dim MaxNum As Nullable(Of Integer)
            Select Case (Region)
                Case "B"
                    MaxNum = (From x In prmEntities.DailyReportBaseConstructionMaterialUses
                            Join y In prmEntities.DailyReports On x.DailyReportId Equals y.ID
                            Where y.ProjectInfoID = projectInfoID AndAlso y.DayWork = DayWork
                            Order By x.Number Descending
                            Select x.Number).FirstOrDefault()
                Case "C"
                    MaxNum = (From x In prmEntities.DailyReportEquipmentUses
                            Join y In prmEntities.DailyReports On x.DailyReportId Equals y.ID
                            Where y.ProjectInfoID = projectInfoID AndAlso y.DayWork = DayWork
                            Order By x.Number Descending
                            Select x.Number).FirstOrDefault()
                Case "D"
                    MaxNum = (From x In prmEntities.DailyReportLaborUses
                            Join y In prmEntities.DailyReports On x.DailyReportId Equals y.ID
                            Where y.ProjectInfoID = projectInfoID AndAlso y.DayWork = DayWork
                            Order By x.Number Descending
                            Select x.Number).FirstOrDefault()
                Case "E"
                    MaxNum = (From x In prmEntities.DailyReportFieldPersonnelUses
                            Join y In prmEntities.DailyReports On x.DailyReportId Equals y.ID
                            Where y.ProjectInfoID = projectInfoID AndAlso y.DayWork = DayWork
                            Order By x.Ordinal Descending
                            Select x.Ordinal).FirstOrDefault()
                Case "F"
                    MaxNum = (From x In prmEntities.DailyReportIncidenceOfInhibitorActivities
                            Join y In prmEntities.DailyReports On x.DailyReportId Equals y.ID
                            Where y.ProjectInfoID = projectInfoID AndAlso y.DayWork = DayWork
                            Order By x.Number Descending
                            Select x.Number).FirstOrDefault()
                Case Else
                    MaxNum = Nothing
            End Select


            If IsNothing(MaxNum) Then
                MaxNum = 1
            Else
                MaxNum += 1
            End If

            Return MaxNum
        End Function

#Region "A"
        Function GetDailyDetail_A(ByVal ProjectDiv As Integer, Optional ByVal DayWork As Integer = 0) As JsonResult
            If DayWork = 0 Then
                Return Json(New With {.data = New List(Of Object)}, JsonRequestBehavior.AllowGet)
            End If

            Dim model = (From x In prmEntities.DailyReportTasks
                            Join y In prmEntities.DailyReports On x.DailyReportId Equals y.ID
                            Join z In prmEntities.ProjectTaskDivisionItems On x.ProjectTaskDivisionItemId Equals z.Id
                        Where y.DayWork = DayWork AndAlso y.ProjectInfoID = ProjectDiv
                        Order By z.ProjectTaskDivision.Ordinal, z.Ordinal
                        Select z.PaymentNumber, _
                               z.TaskTitle, _
                               z.UnitQuantity, _
                               ProjectTaskDivisionItemId = z.Id, _
                               x.Volume, _
                               x.Location, _
                               x.ID
                        ).ToList()

            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()> _
        Function SaveDetail_A(ByVal model As DailyReportTask, ProjectInfoID As Integer) As ActionResult
            'check: is data exist before?
            Dim dataExist As DailyReportTask = (From x In prmEntities.DailyReportTasks Where x.ID = model.ID AndAlso
                                                x.ProjectTaskDivisionItem.ProjectTaskDivision.ProjectInfoId = ProjectInfoID
                                                Select x).FirstOrDefault()

            If ModelState.IsValid Then
                Try
                    'check: volume must valid
                    Dim maxVolume = (From ptdi In prmEntities.ProjectTaskDivisionItems Where
                                      ptdi.Id = model.ProjectTaskDivisionItemId
                                     Select New With {.Quantity = ptdi.Quantity, .UnitQuantity = ptdi.UnitQuantity}).FirstOrDefault()

                    Dim obj = (From drt In prmEntities.DailyReportTasks
                               Where drt.ProjectTaskDivisionItemId = model.ProjectTaskDivisionItemId AndAlso
                               drt.ProjectTaskDivisionItem.ProjectTaskDivision.ProjectInfoId = ProjectInfoID)

                    Dim VolumeReported As Double = 0,
                        tmpVolume As Double = 0
                    If Not IsNothing(dataExist) Then
                        tmpVolume = dataExist.Volume
                    End If
                    If obj IsNot Nothing AndAlso obj.Count > 0 Then
                        VolumeReported = Math.Round(obj.Sum(Function(d) d.Volume) - tmpVolume, 4)
                    End If

                    If (model.Volume > (maxVolume.Quantity - VolumeReported)) Then
                        If (maxVolume.Quantity - VolumeReported) <= 0 Then
                            ModelState.AddModelError("General",
                                                     "Volume pekerjaan ini sudah penuh, tidak bisa ditambah lagi")
                        Else
                            ModelState.AddModelError("General",
                                                     "Nilai volume terlalu besar, nilai volume harus lebih kecil atau sama dengan : " & (maxVolume.Quantity - VolumeReported) & " " & maxVolume.UnitQuantity &
                                                     "<br /> Jumlah maksimum bobot : " & (maxVolume.Quantity) & " " & maxVolume.UnitQuantity)
                        End If
                    Else
                        'check: is edit or add new
                        If Not IsNothing(dataExist) Then ' result is edit
                            With dataExist
                                .DailyReportId = GetDailyReportID(ProjectInfoID, model.DayWork) 'check: throw the DailyReport ID if already exist, otherwise Create one
                                .ProjectTaskDivisionItemId = model.ProjectTaskDivisionItemId
                                .Volume = model.Volume
                                .Location = model.Location
                            End With
                            prmEntities.SaveChanges()
                            Return Json(New With {.stat = 1})
                        Else                            ' result is add new
                            model.DailyReportId = GetDailyReportID(ProjectInfoID, model.DayWork) 'check: throw the DailyReport ID if already exist, otherwise Create one
                            model.Location = model.Location
                            'check: duplicate data is exist
                            Dim duplicateData = (From x In prmEntities.DailyReportTasks Where
                                                 x.DailyReportId = model.DailyReportId AndAlso
                                                 x.ProjectTaskDivisionItemId = model.ProjectTaskDivisionItemId
                                                 Select x.ProjectTaskDivisionItem.TaskTitle).FirstOrDefault()

                            If duplicateData IsNot Nothing Then
                                ModelState.AddModelError("General", duplicateData & " sudah ada")
                            Else
                                prmEntities.DailyReportTasks.AddObject(model)
                                prmEntities.SaveChanges()
                                Return Json(New With {.stat = 1})
                            End If
                        End If
                    End If
                Catch ex As Exception
                    ModelState.AddModelError("General", ex.Message)
                End Try
            End If

            'if we got this far so return back the view
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        Function RemoveDailyDetail_A(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""

            Dim data = (From pl In prmEntities.DailyReportTasks Where pl.ID = id).FirstOrDefault()
            If data IsNot Nothing Then prmEntities.DailyReportTasks.DeleteObject(data)
            prmEntities.SaveChanges()
            stat = 1
            msg = "Success Deleted"
            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function
#End Region

#Region "B"
        Function GetDailyDetail_B(ByVal ProjectDiv As Integer, Optional ByVal DayWork As Integer = 0) As JsonResult
            If DayWork = 0 Then
                Return Json(New With {.data = New List(Of Object)}, JsonRequestBehavior.AllowGet)
            End If
            Dim model = (From x In prmEntities.DailyReportBaseConstructionMaterialUses
                            Join y In prmEntities.DailyReports On x.DailyReportId Equals y.ID
                        Where y.DayWork = DayWork AndAlso y.ProjectInfoID = ProjectDiv
                        Select x.Number, x.MaterialName, x.QuantityUse, x.QuantityImported, x.QuantityUnit, x.ID
                        ).ToList()

            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()> _
        Function SaveDetail_B(ByVal model As DailyReportBaseConstructionMaterialUse, ProjectInfoID As Integer) As ActionResult

            'check: is data exist before?
            Dim dataExist = (From x In prmEntities.DailyReportBaseConstructionMaterialUses Where x.ID = model.ID Select x).FirstOrDefault()

            If ModelState.IsValid Then
                Try
                    'check: is edit or add new
                    If Not IsNothing(dataExist) Then ' result is edit
                        With dataExist
                            .DailyReportId = GetDailyReportID(ProjectInfoID, model.DayWork) 'check: throw the DailyReport ID if already exist, otherwise Create one
                            '.Number = model.Number
                            .MaterialName = model.MaterialName
                            .QuantityUnit = model.QuantityUnit
                            .QuantityUse = model.QuantityUse
                            .QuantityImported = model.QuantityImported
                        End With
                    Else                            ' result is add new
                        model.Number = GetMaxNum("B", ProjectInfoID, model.DayWork)
                        model.DailyReportId = GetDailyReportID(ProjectInfoID, model.DayWork) 'check: throw the DailyReport ID if already exist, otherwise Create one
                        prmEntities.DailyReportBaseConstructionMaterialUses.AddObject(model)
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

        Function RemoveDailyDetail_B(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""

            Dim tmpNumber, tmpDailyRptID As Integer

            Dim data = (From pl In prmEntities.DailyReportBaseConstructionMaterialUses Where pl.ID = id).FirstOrDefault()

            tmpNumber = data.Number
            tmpDailyRptID = data.DailyReportId

            'reOrder Number
            Dim dtListdata = (From x In prmEntities.DailyReportBaseConstructionMaterialUses
                                Where x.DailyReportId = tmpDailyRptID AndAlso x.Number > tmpNumber).ToList()

            For Each dt In dtListdata
                dt.Number -= 1
            Next
            If data IsNot Nothing Then prmEntities.DailyReportBaseConstructionMaterialUses.DeleteObject(data)

            prmEntities.SaveChanges()
            stat = 1
            msg = "Success Deleted"

            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function
#End Region

#Region "C"
        Function GetDailyDetail_C(ByVal ProjectDiv As Integer, Optional ByVal DayWork As Integer = 0) As JsonResult
            If DayWork = 0 Then
                Return Json(New With {.data = New List(Of Object)}, JsonRequestBehavior.AllowGet)
            End If
            Dim model = (From x In prmEntities.DailyReportEquipmentUses
                                Join y In prmEntities.DailyReports On x.DailyReportId Equals y.ID
                            Where y.DayWork = DayWork AndAlso y.ProjectInfoID = ProjectDiv
                            Select x.Number, x.EquipmentName, x.Amount, x.Condition, x.MeasurementUnit, x.ID
                            ).ToList()

            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()> _
        Function SaveDetail_C(ByVal model As DailyReportEquipmentUse, ProjectInfoID As Integer) As ActionResult

            'check: is data exist before?
            Dim dataExist = (From x In prmEntities.DailyReportEquipmentUses Where x.ID = model.ID Select x).FirstOrDefault()

            If ModelState.IsValid Then
                Try
                    'check: is edit or add new
                    If Not IsNothing(dataExist) Then ' result is edit
                        With dataExist
                            .DailyReportId = GetDailyReportID(ProjectInfoID, model.DayWork) 'check: throw the DailyReport ID if already exist, otherwise Create one
                            '.Number = model.Number
                            .EquipmentName = model.EquipmentName
                            .Amount = model.Amount
                            .MeasurementUnit = model.MeasurementUnit
                            .Condition = model.Condition
                        End With
                    Else                            ' result is add new
                        model.DailyReportId = GetDailyReportID(ProjectInfoID, model.DayWork) 'check: throw the DailyReport ID if already exist, otherwise Create one
                        model.Number = GetMaxNum("C", ProjectInfoID, model.DayWork)
                        prmEntities.DailyReportEquipmentUses.AddObject(model)
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

        Function RemoveDailyDetail_C(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""

            Dim tmpNumber, tmpDailyRptID As Integer

            Dim data = (From pl In prmEntities.DailyReportEquipmentUses Where pl.ID = id).FirstOrDefault()

            tmpNumber = data.Number
            tmpDailyRptID = data.DailyReportId

            'reOrder Number
            Dim dtListdata = (From x In prmEntities.DailyReportEquipmentUses
                                Where x.DailyReportId = tmpDailyRptID AndAlso x.Number > tmpNumber).ToList()

            For Each dt In dtListdata
                dt.Number -= 1
            Next
            If data IsNot Nothing Then prmEntities.DailyReportEquipmentUses.DeleteObject(data)
            prmEntities.SaveChanges()
            stat = 1
            msg = "Success Deleted"

            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function
#End Region

#Region "D"
        Function GetDailyDetail_D(ByVal ProjectDiv As Integer, Optional ByVal DayWork As Integer = 0) As JsonResult
            If DayWork = 0 Then
                Return Json(New With {.data = New List(Of Object)}, JsonRequestBehavior.AllowGet)
            End If
            Dim model = (From x In prmEntities.DailyReportLaborUses
                                Join y In prmEntities.DailyReports On x.DailyReportId Equals y.ID
                            Where y.DayWork = DayWork AndAlso y.ProjectInfoID = ProjectDiv
                            Select x.Number, x.Position, x.Amount, x.Unit, x.ID
                            ).ToList()

            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()> _
        Function SaveDetail_D(ByVal model As DailyReportLaborUse, ProjectInfoID As Integer) As ActionResult

            'check: is data exist before?
            Dim dataExist = (From x In prmEntities.DailyReportLaborUses Where x.ID = model.ID Select x).FirstOrDefault()

            If ModelState.IsValid Then
                Try
                    'check: is edit or add new
                    If Not IsNothing(dataExist) Then ' result is edit
                        With dataExist
                            .DailyReportId = GetDailyReportID(ProjectInfoID, model.DayWork) 'check: throw the DailyReport ID if already exist, otherwise Create one
                            '.Number = model.Number
                            .Position = model.Position
                            .Amount = model.Amount
                            .Unit = model.Unit
                        End With
                    Else                            ' result is add new
                        model.DailyReportId = GetDailyReportID(ProjectInfoID, model.DayWork) 'check: throw the DailyReport ID if already exist, otherwise Create one
                        model.Number = GetMaxNum("D", ProjectInfoID, model.DayWork)
                        prmEntities.DailyReportLaborUses.AddObject(model)
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

        Function RemoveDailyDetail_D(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""

            Dim tmpNumber, tmpDailyRptID As Integer

            Dim data = (From pl In prmEntities.DailyReportLaborUses Where pl.ID = id).FirstOrDefault()

            tmpNumber = data.Number
            tmpDailyRptID = data.DailyReportId

            'reOrder Number
            Dim dtListdata = (From x In prmEntities.DailyReportLaborUses
                                Where x.DailyReportId = tmpDailyRptID AndAlso x.Number > tmpNumber).ToList()

            For Each dt In dtListdata
                dt.Number -= 1
            Next
            If data IsNot Nothing Then prmEntities.DailyReportLaborUses.DeleteObject(data)
            prmEntities.SaveChanges()
            stat = 1
            msg = "Success Deleted"

            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function
#End Region

#Region "E"
        Function GetDailyDetail_E(ByVal ProjectDiv As Integer, Optional ByVal DayWork As Integer = 0) As JsonResult
            If DayWork = 0 Then
                Return Json(New With {.data = New List(Of Object)}, JsonRequestBehavior.AllowGet)
            End If

            Dim model = (From x In prmEntities.DailyReportFieldPersonnelUses
                            Join y In prmEntities.DailyReports On x.DailyReportId Equals y.ID
                        Where y.DayWork = DayWork AndAlso y.ProjectInfoID = ProjectDiv
                        Order By x.PositionType
                        Select x.ID, x.PositionType, x.PositionTypeName, PositionTypeConcat = x.PositionType.TrimEnd() + ". " + x.PositionTypeName, x.Ordinal, x.Position, x.Amount
                        ).ToList()

            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()> _
        Function SaveDetail_E(ByVal model As DailyReportFieldPersonnelUseModel, ProjectInfoID As Integer) As ActionResult

            Dim modelData As New DailyReportFieldPersonnelUse, _
                modelID As Integer

            Dim MaxPositionType = (From x In prmEntities.DailyReportFieldPersonnelUses
                                   Join y In prmEntities.DailyReports On x.DailyReportId Equals y.ID
                                    Where y.ProjectInfoID = ProjectInfoID _
                                        AndAlso y.DayWork = model.DayWork
                                    Select x.PositionType).Max()

            If IsNothing(MaxPositionType) Then
                MaxPositionType = "A"
            Else
                MaxPositionType = Chr(Asc(MaxPositionType.Trim()) + 1)
            End If

            Dim i As Integer = 0
            If Not IsNothing(model.ID) Then

                Dim dataExist = (From x In prmEntities.DailyReportFieldPersonnelUses
                                Join y In model.ID On x.ID Equals y Select x).ToList()

                For Each itemID In model.ID
                    Try
                        modelID = itemID
                        If String.IsNullOrWhiteSpace(model.Position(i)) AndAlso model.Amount(i) < 0 Then
                            ModelState.AddModelError("General" & i, "Posisi Jabatan ke-" & (i + 1) & " dan Jumlah orang tidak boleh kosong")
                        ElseIf String.IsNullOrWhiteSpace(model.Position(i)) Then
                            ModelState.AddModelError("General1" & i, "Posisi Jabatan ke-" & (i + 1) & " tidak boleh kosong")
                        ElseIf model.Amount(i) < 0 Then
                            ModelState.AddModelError("General2" & i, "Jumlah orang ke-" & (i + 1) & " tidak boleh kosong")
                        End If

                        If ModelState.IsValid Then

                            Dim itemDataExist = (From x In dataExist Where x.ID = modelID Select x).FirstOrDefault()

                            'check: is edit or add new
                            If Not IsNothing(itemDataExist) Then ' result is edit
                                With itemDataExist
                                    .DailyReportId = GetDailyReportID(ProjectInfoID, model.DayWork) 'check: throw the DailyReport ID if already exist, otherwise Create one
                                    '.PositionType = "A" ' temporary hardcode
                                    .PositionTypeName = model.PositionTypeName
                                    '.Ordinal = i + 1
                                    .Position = model.Position(i)
                                    .Amount = model.Amount(i)
                                End With
                                prmEntities.SaveChanges()
                            Else                            ' result is add new
                                modelData = New DailyReportFieldPersonnelUse
                                modelData.DailyReportId = GetDailyReportID(ProjectInfoID, model.DayWork) 'check: throw the DailyReport ID if already exist, otherwise Create one
                                modelData.PositionType = MaxPositionType ' temporary hardcode
                                modelData.PositionTypeName = model.PositionTypeName
                                modelData.Ordinal = i + 1
                                modelData.Position = model.Position(i)
                                modelData.Amount = model.Amount(i)
                                prmEntities.DailyReportFieldPersonnelUses.AddObject(modelData)
                            End If

                        End If
                    Catch ex As Exception
                        ModelState.AddModelError("General", ex.Message)
                    End Try

                    i += 1
                Next
            Else
                ModelState.AddModelError("GeneralErr", "Posisi Jabatan tidak boleh kosong")
            End If
            If ModelState.IsValid Then
                prmEntities.SaveChanges()
                Return Json(New With {.stat = 1})
            End If

            'if we got this far so return back the view
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        Function RemoveDailyDetail_E(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""

            Dim tmpDailyRptID As Integer, tmpPositionType As Char

            Dim data = (From pl In prmEntities.DailyReportFieldPersonnelUses Where pl.ID = id Select pl)
            Dim dataSub = (From x In prmEntities.DailyReportFieldPersonnelUses
                           Join y In data On x.PositionType Equals y.PositionType _
                            And x.PositionTypeName Equals y.PositionTypeName Select x).ToList()

            If IsNothing(dataSub) Then
                tmpPositionType = ""
            Else
                tmpPositionType = dataSub.Item(0).PositionType
                tmpDailyRptID = dataSub.Item(0).DailyReportId
            End If

            For Each item In dataSub
                If data IsNot Nothing Then prmEntities.DailyReportFieldPersonnelUses.DeleteObject(item)
            Next

            'reOrder PositionType
            If Not String.IsNullOrEmpty(tmpPositionType) Then
                Dim dtListdata = (From x In prmEntities.DailyReportFieldPersonnelUses
                                    Where x.DailyReportId = tmpDailyRptID AndAlso x.PositionType > tmpPositionType).ToList()
                For Each dt In dtListdata
                    dt.PositionType = Chr(Asc(dt.PositionType.Trim()) - 1)
                Next
            End If

            prmEntities.SaveChanges()
            stat = 1
            msg = "Success Deleted"

            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()> _
        Function SaveSubDetail_E(ByVal model As DailyReportFieldPersonnelUse, ProjectInfoID As Integer) As ActionResult

            'check: is data exist before?
            Dim dataExist = (From x In prmEntities.DailyReportFieldPersonnelUses Where x.ID = model.ID Select x).FirstOrDefault()

            If ModelState.IsValid Then
                Try
                    'check: is edit or add new
                    If Not IsNothing(dataExist) Then ' result is edit
                        With dataExist
                            '.PositionType = model.PositionType
                            '.PositionTypeName = model.PositionTypeName
                            '.Ordinal = model.Ordinal
                            .Position = model.Position
                            .Amount = model.Amount
                        End With
                    Else                            ' result is add new
                        model.DailyReportId = GetDailyReportID(ProjectInfoID, model.DayWork) 'check: throw the DailyReport ID if already exist, otherwise Create one
                        model.Ordinal = GetMaxNum("E", ProjectInfoID, model.DayWork)

                        prmEntities.DailyReportFieldPersonnelUses.AddObject(model)
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

        Function RemoveDailySubDetail_E(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""

            Dim tmpOrdinal, tmpDailyRptID As Integer

            Dim data = (From pl In prmEntities.DailyReportFieldPersonnelUses Where pl.ID = id).FirstOrDefault()
            tmpOrdinal = data.Ordinal
            tmpDailyRptID = data.DailyReportId

            'reOrder Ordinal
            Dim dtListdata = (From x In prmEntities.DailyReportFieldPersonnelUses
                                Where x.DailyReportId = tmpDailyRptID AndAlso x.Ordinal > tmpOrdinal).ToList()

            For Each dt In dtListdata
                dt.Ordinal -= 1
            Next

            'delete object
            If data IsNot Nothing Then prmEntities.DailyReportFieldPersonnelUses.DeleteObject(data)
            prmEntities.SaveChanges()
            stat = 1
            msg = "Success Deleted"
            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function
#End Region

#Region "F"
        Function GetDailyDetail_F(ByVal ProjectDiv As Integer, Optional ByVal DayWork As Integer = 0) As JsonResult
            If DayWork = 0 Then
                Return Json(New With {.data = New List(Of Object)}, JsonRequestBehavior.AllowGet)
            End If

            Dim model = (From x In prmEntities.DailyReportIncidenceOfInhibitorActivities
                            Join y In prmEntities.DailyReports On x.DailyReportId Equals y.ID
                        Where y.DayWork = DayWork AndAlso y.ProjectInfoID = ProjectDiv
                        Select x.ID, x.Number, x.Type, x.Time, x.Location, x.IsResponsibilityOfContractor).ToList()

            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()> _
        Function SaveDetail_F(ByVal model As DailyReportIncidenceOfInhibitorActivity, ProjectInfoID As Integer) As ActionResult
            'check: is data exist before?
            Dim dataExist = (From x In prmEntities.DailyReportIncidenceOfInhibitorActivities Where x.ID = model.ID Select x).FirstOrDefault()

            If ModelState.IsValid Then
                Try
                    'check: is edit or add new
                    If Not IsNothing(dataExist) Then ' result is edit
                        With dataExist
                            .DailyReportId = GetDailyReportID(ProjectInfoID, model.DayWork) 'check: throw the DailyReport ID if already exist, otherwise Create one
                            '.Number = model.Number
                            .Type = model.Type
                            .Time = model.Time
                            .Location = model.Location
                            .IsResponsibilityOfContractor = model.IsResponsibilityOfContractor
                        End With
                    Else                            ' result is add new
                        model.DailyReportId = GetDailyReportID(ProjectInfoID, model.DayWork) 'check: throw the DailyReport ID if already exist, otherwise Create one
                        model.Number = GetMaxNum("F", ProjectInfoID, model.DayWork)
                        model.Location = model.Location
                        prmEntities.DailyReportIncidenceOfInhibitorActivities.AddObject(model)
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

        Function RemoveDailyDetail_F(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""

            Dim data = (From pl In prmEntities.DailyReportIncidenceOfInhibitorActivities Where pl.ID = id).FirstOrDefault()

            Dim tmpNumber, tmpDailyRptID As Integer

            tmpNumber = data.Number
            tmpDailyRptID = data.DailyReportId

            'reOrder Number
            Dim dtListdata = (From x In prmEntities.DailyReportIncidenceOfInhibitorActivities
                                Where x.DailyReportId = tmpDailyRptID AndAlso x.Number > tmpNumber).ToList()

            For Each dt In dtListdata
                dt.Number -= 1
            Next
            If data IsNot Nothing Then prmEntities.DailyReportIncidenceOfInhibitorActivities.DeleteObject(data)
            prmEntities.SaveChanges()
            stat = 1
            msg = "Success Deleted"
            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function
#End Region

#Region "G"
        Function GetDailyDetail_G(ByVal ProjectDiv As Integer, Optional ByVal DayWork As Integer = 0) As JsonResult
            If DayWork = 0 Then
                Return Json(New With {.data = New List(Of Object)}, JsonRequestBehavior.AllowGet)
            End If

            Dim model = prmEntities.GetDailyReportFor_G(ProjectDiv, DayWork).ToList()

            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()> _
        Function SaveDetail_G(ByVal model As GetDailyReportFor_G_Result, ProjectInfoID As Integer) As ActionResult
            Dim ModelID As Integer
            If ModelState.IsValid Then
                Try
                    ModelID = GetDailyReportID(ProjectInfoID, model.DayWork) 'check: throw the DailyReport ID if already exist, otherwise Create one

                    Dim query = "UPDATE PMn.DailyReport SET " & model.Title & " = @Value " & _
                                "WHERE ProjectInfoID = @ProjectInfoId AND DayWork = @DayWork"

                    Dim Param1 = New SqlClient.SqlParameter("@ProjectInfoId", ProjectInfoID)
                    Dim Param2 = New SqlClient.SqlParameter("@DayWork", model.DayWork)
                    Dim Param3 = New SqlClient.SqlParameter("@Value", model.Value)

                    Dim items = prmEntities.ExecuteStoreQuery(Of ERPBase.OptionItem)(query, Param1, Param2, Param3).ToArray()

                    prmEntities.SaveChanges()
                    Return Json(New With {.stat = 1})
                Catch ex As Exception
                    ModelState.AddModelError("General", ex.Message)
                End Try
            End If

            'if we got this far so return back the view
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        Function RemoveDailyDetail_G(ByVal id As Integer, ByVal title As String) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""

            Dim query = "UPDATE PMn.DailyReport SET " & title & " = NULL WHERE ID = @id"

            Dim Param1 = New SqlClient.SqlParameter("@id", id)
            Dim items = prmEntities.ExecuteStoreQuery(Of ERPBase.OptionItem)(query, Param1).ToArray()

            prmEntities.SaveChanges()
            stat = 1
            msg = "Success Deleted"
            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function
#End Region

#End Region

#Region "Weekly & WeeklyReportMC0"
        Function WeeklyReport(ByVal Id As Integer) As ActionResult
            Dim p As ProjectInfo = prmEntities.ProjectInfoes.Where(Function(m) m.Id = Id).SingleOrDefault()

            ViewData("ProjectTaskDivisionItemId") = GetProjectTaskDivList(Id)
            ViewData("WeeklyReport") = New WeeklyReport
            If p Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            Return View(p)
        End Function

        Function WeeklyReportMC0(ByVal Id As Integer) As ActionResult
            Dim p As ProjectInfo = prmEntities.ProjectInfoes.Where(Function(m) m.Id = Id).SingleOrDefault()

            ViewData("ProjectTaskDivisionItemId") = GetProjectTaskDivList(Id)
            ViewData("WeeklyReport") = New WeeklyReport
            If p Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            Return View(p)
        End Function

        Function GetReportListWeekItems(ByVal Id As Integer, Optional forMC0 As Boolean = False) As JsonResult
            Dim model = prmEntities.GetWeeklyReportList(Id).ToList()

            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function WeeklyReportPrint(ByVal Id As Integer, Optional forMC0 As Boolean = False) As ActionResult
            Dim modelHeaderFooter = prmEntities.GetWeeklyReportHeaderFooter(Id)
            Dim modelDetail = IIf(forMC0, prmEntities.GetWeeklyReportDetail_withADD(Id), prmEntities.GetWeeklyReportDetail(Id))
            Dim modelStats = prmEntities.GetWeeklyReportStats(Id)

            Dim mimeType As String = "application/pdf"
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing
            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.EnableExternalImages = True
            r.ReportPath = Server.MapPath("~/Areas/ProjectManagement/Reports/" & IIf(forMC0, "WeeklyReportMC0.rdlc", "WeeklyReport.rdlc"))
            r.SetParameters(New ReportParameter("WeekReportID", Id))

            If Not IsNothing(modelHeaderFooter) Then
                r.DataSources.Add(New ReportDataSource("DsWeeklyReportHeaderFooter", modelHeaderFooter))
            End If

            If Not IsNothing(modelDetail) Then
                r.DataSources.Add(New ReportDataSource("DsWeeklyReportDetail", modelDetail))
            End If

            If Not IsNothing(modelStats) Then
                r.DataSources.Add(New ReportDataSource("DsWeeklyReportStats", modelStats))
            End If

            r.Refresh()
            Dim reportType As String = "PDF"

            Dim devinfo As String = "<DeviceInfo>" &
            "<ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" &
            "  <PageWidth>" & IIf(forMC0, "13.5in", "11.5in") & "</PageWidth>" &
            "  <PageHeight>13.77in</PageHeight>" &
            "  <MarginTop>0.0in</MarginTop>" &
            "  <MarginLeft>0.0in</MarginLeft>" &
            "  <MarginRight>0.0in</MarginRight>" &
            "  <MarginBottom>0.0in</MarginBottom>" &
            "</DeviceInfo>"

            Dim output() As Byte = r.Render(reportType, devinfo, mimeType, Encoding, fileNameExtension, streams, warnings)
            Return File(output, mimeType)
        End Function


#End Region

#Region "CRUD Weekly"
        <HttpPost()> _
        Function SaveWeeklyReport(ByVal model As WeeklyReport) As ActionResult
            'check: is data exist before?
            Dim dataExist = (From x In prmEntities.WeeklyReports Where x.Id = model.Id Select x).FirstOrDefault()

            If ModelState.IsValid Then
                Try
                    'check: is edit or add new
                    If Not IsNothing(dataExist) Then ' result is edit
                        With dataExist
                            .Id = model.Id
                            .ProjectInfoId = model.ProjectInfoId
                            .ImplementingActivities = model.ImplementingActivities
                            .WeekNumber = model.WeekNumber
                            .Place = model.Place
                            .DateApproval = model.DateApproval
                            .Approval1Company = model.Approval1Company
                            .Approval1Identity = model.Approval1Identity
                            .Approval1Name = model.Approval1Name
                            .Approval1Occupation = model.Approval1Occupation
                            .Approval2Company = model.Approval2Company
                            .Approval2Identity = model.Approval2Identity
                            .Approval2Name = model.Approval2Name
                            .Approval2Occupation = model.Approval2Occupation
                            .Approval3Company = model.Approval3Company
                            .Approval3Identity = model.Approval3Identity
                            .Approval3Name = model.Approval3Name
                            .Approval3Occupation = model.Approval3Occupation
                        End With
                    Else                            ' result is add new
                        prmEntities.WeeklyReports.AddObject(model)
                    End If
                    prmEntities.SaveChanges()
                    Return Json(New With {.stat = 1, .WkID = model.Id})
                Catch ex As Exception
                    ModelState.AddModelError("General", ex.Message)
                End Try
            End If

            'if we got this far so return back the view
            Return Json(New With {.stat = 0, .WkID = model.Id, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        Function RemoveWeeklyDetail(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From wr In prmEntities.WeeklyReports Where wr.Id = id).FirstOrDefault()
                If data IsNot Nothing Then prmEntities.WeeklyReports.DeleteObject(data)
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

#Region "Monthly & MonthlyMC0"
        Function MonthlyReport(ByVal Id As Integer) As ActionResult
            Dim p As ProjectInfo = prmEntities.ProjectInfoes.Where(Function(m) m.Id = Id).SingleOrDefault()

            ViewData("ProjectTaskDivisionItemId") = GetProjectTaskDivList(Id)
            ViewData("MonthlyReport") = New MonthlyReport
            If p Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            Return View(p)
        End Function

        Function MonthlyReportMC0(ByVal Id As Integer) As ActionResult
            Dim p As ProjectInfo = prmEntities.ProjectInfoes.Where(Function(m) m.Id = Id).SingleOrDefault()

            ViewData("ProjectTaskDivisionItemId") = GetProjectTaskDivList(Id)
            ViewData("MonthlyReport") = New MonthlyReport
            If p Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            Return View(p)
        End Function

        Function GetReportListMonthItems(ByVal Id As Integer, Optional forMC0 As Boolean = False) As JsonResult
            Dim model = prmEntities.GetMonthlyReportList(Id).ToList()

            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function MonthlyReportPrint(ByVal Id As Integer, Optional forMC0 As Boolean = False) As ActionResult
            Dim modelHeaderFooter = prmEntities.GetMonthlyReportHeaderFooter(Id)
            Dim modelDetail = IIf(forMC0, prmEntities.GetMonthlyReportDetail_withADD(Id), prmEntities.GetMonthlyReportDetail(Id))
            Dim modelStats = prmEntities.GetMonthlyReportStats(Id)

            Dim mimeType As String = "application/pdf"
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing
            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.EnableExternalImages = True
            r.ReportPath = Server.MapPath("~/Areas/ProjectManagement/Reports/" & IIf(forMC0, "MonthlyReportMC0.rdlc", "MonthlyReport.rdlc"))
            r.SetParameters(New ReportParameter("MonthReportID", Id))

            If Not IsNothing(modelHeaderFooter) Then
                r.DataSources.Add(New ReportDataSource("DsMonthlyReportHeaderFooter", modelHeaderFooter))
            End If

            If Not IsNothing(modelDetail) Then
                r.DataSources.Add(New ReportDataSource("DsMonthlyReportDetail", modelDetail))
            End If

            If Not IsNothing(modelStats) Then
                r.DataSources.Add(New ReportDataSource("DsMonthlyReportStats", modelStats))
            End If

            r.Refresh()
            Dim reportType As String = "PDF"

            Dim devinfo As String = "<DeviceInfo>" +
            "<ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" &
            "  <PageWidth>" & IIf(forMC0, "13.5in", "11.6in") & "</PageWidth>" &
            "  <PageHeight>12.07in</PageHeight>" &
            "  <MarginTop>0.0in</MarginTop>" &
            "  <MarginLeft>0.0in</MarginLeft>" &
            "  <MarginRight>0.0in</MarginRight>" &
            "  <MarginBottom>0.0in</MarginBottom>" &
            "</DeviceInfo>"

            Dim output() As Byte = r.Render(reportType, devinfo, mimeType, Encoding, fileNameExtension, streams, warnings)
            Return File(output, mimeType)
        End Function
#End Region

#Region "CRUD Monthly"
        <HttpPost()> _
        Function SaveMonthlyReport(ByVal model As MonthlyReport) As ActionResult
            'check: is data exist before?
            Dim dataExist = (From x In prmEntities.MonthlyReports Where x.Id = model.Id Select x).FirstOrDefault()

            If ModelState.IsValid Then
                Try
                    'check: is edit or add new
                    If Not IsNothing(dataExist) Then ' result is edit
                        With dataExist
                            .Id = model.Id
                            .ProjectInfoId = model.ProjectInfoId
                            .ImplementingActivities = model.ImplementingActivities
                            .MonthNumber = model.MonthNumber
                            .Place = model.Place
                            .DateApproval = model.DateApproval
                            .Approval1Company = model.Approval1Company
                            .Approval1Identity = model.Approval1Identity
                            .Approval1Name = model.Approval1Name
                            .Approval1Occupation = model.Approval1Occupation
                            .Approval2Company = model.Approval2Company
                            .Approval2Identity = model.Approval2Identity
                            .Approval2Name = model.Approval2Name
                            .Approval2Occupation = model.Approval2Occupation
                            .Approval3Company = model.Approval3Company
                            .Approval3Identity = model.Approval3Identity
                            .Approval3Name = model.Approval3Name
                            .Approval3Occupation = model.Approval3Occupation
                        End With
                    Else                            ' result is add new
                        prmEntities.MonthlyReports.AddObject(model)
                    End If
                    prmEntities.SaveChanges()
                    Return Json(New With {.stat = 1, .MnID = model.Id})
                Catch ex As Exception
                    ModelState.AddModelError("General", ex.Message)
                End Try
            End If

            'if we got this far so return back the view
            Return Json(New With {.stat = 0, .MnID = model.Id, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        Function RemoveMonthlyDetail(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From wr In prmEntities.MonthlyReports Where wr.Id = id).FirstOrDefault()
                If data IsNot Nothing Then prmEntities.MonthlyReports.DeleteObject(data)
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

#Region "Report Template"

        Public Function ReportsTemplate(ByVal Id As Integer) As ActionResult
            Try
                Dim rl As ReportLayout = prmEntities.ReportLayouts.Where(Function(r) r.IdProjectInfo = Id).SingleOrDefault()
                If rl Is Nothing Then
                    rl = New ReportLayout With {.IdProjectInfo = Id, .Title1 = "Judul Atas", .Title2 = "Judul Bawah"}
                    prmEntities.ReportLayouts.AddObject(rl)
                    prmEntities.SaveChanges()
                End If
                Return View(rl)
            Catch ex As Exception
                Throw New HttpException(ex.Message)
            End Try
        End Function

        <HttpPost()> _
        Public Function SavePartial(ByVal pk As Integer, name As String, value As String) As ActionResult
            Dim model = prmEntities.ReportLayouts.Where(Function(m) m.ID = pk).SingleOrDefault()
            If model Is Nothing Then
                ModelState.AddModelError("General", "Data layout tidak ditemukan. Muat ulang halaman ini")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            model.GetType.GetProperty(name).SetValue(model, value, Nothing)

            prmEntities.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function

        <HttpPost()> _
        Public Function SavePartial_temp(ByVal pk As Integer, name As String, value As String) As ActionResult
            Return Json(New With {.stat = 1})
        End Function

        <HttpPost()> _
        Public Function SavePartial_Image(ByVal pk As Integer, ByVal name As String, Optional ByVal Logo As HttpPostedFileBase = Nothing) As ActionResult

            If IsNothing(Logo) Then Return Json(New With {.stat = 1})

            Dim model = prmEntities.ReportLayouts.Where(Function(m) m.ID = pk).SingleOrDefault()
            If model Is Nothing Then
                ModelState.AddModelError("General", "Data layout tidak ditemukan. Muat ulang halaman ini")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            '''' Acceptance of mimeType
            Dim validImageTypes = New String() { _
                "image/png"
            }
            Dim d As Byte()

            Dim strContentType As String = (From c In validImageTypes Where c = Logo.ContentType Select c).FirstOrDefault()
            If IsNothing(strContentType) Then
                ModelState.AddModelError("notSameMimeType", "Logo yang dipilih tidak valid. Logo harus bertipe .png")
                Return Json(New With {.stat = 0, .MnID = model.ID, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            Else
                Using binaryReader = New BinaryReader(Logo.InputStream)
                    d = binaryReader.ReadBytes(Logo.ContentLength)
                End Using
            End If

            model.GetType.GetProperty(name).SetValue(model, d, Nothing)

            prmEntities.SaveChanges()
            Return Json(New With {.stat = 1, .logo = d})
        End Function

        <HttpPost()> _
        Public Function RemovePartial_Image(ByVal pk As Integer, ByVal name As String) As ActionResult
            Dim model = prmEntities.ReportLayouts.Where(Function(m) m.ID = pk).SingleOrDefault()
            If model Is Nothing Then
                ModelState.AddModelError("General", "Data layout tidak ditemukan. Muat ulang halaman ini")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If

            model.GetType.GetProperty(name).SetValue(model, Nothing, Nothing)

            prmEntities.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function

        Public Function getLogo(ByVal id As Integer, ByVal logo As String) As ActionResult
            Dim model = prmEntities.ReportLayouts.Where(Function(m) m.ID = id).SingleOrDefault()
            'MsgBox(IsNothing(model.RightLogo))
            If Not IsNothing(model) Then
                Select Case logo
                    Case "right"
                        If IsNothing(model.RightLogo) Then
                            Return Nothing
                        Else
                            Return File(model.RightLogo, "image/png")
                        End If
                    Case "left"
                        If IsNothing(model.LeftLogo) Then
                            Return Nothing
                        Else
                            Return File(model.LeftLogo, "image/png")
                        End If
                End Select
            Else
                Return Nothing
            End If
            Return View("~/Views/Error/NotFound.vbhtml")
        End Function

#End Region

    End Class
End Namespace
