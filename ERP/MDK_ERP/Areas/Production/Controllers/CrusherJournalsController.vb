Imports Microsoft.Reporting.WebForms
Imports DocumentFormat.OpenXml.Spreadsheet
Imports ClosedXML.Excel
Imports System.IO

Namespace MDK_ERP.Areas.Production.Controllers

    <Authorize()> _
    Public Class CrusherJournalsController
        Inherits BaseController

        '
        ' GET: /Production/CrusherJournals

        Function Index() As ActionResult
            Return View()
        End Function

        Function GetList(dataMonth As Byte?, dataYear As Integer?) As JsonResult
            Dim model = (From cj In ctx.GetCrusherJournalData(dataMonth, dataYear)
                         Select cj).ToList()
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Sub CrusherJournalViewData()
            Dim modelWorkSchedule = (From ws In ctx.MstWorkSchedule Select New With {.ID = ws.ID, .Shift = ws.Shift}).ToList()
            Dim modelMaterial = (From mm In ctx.MstMaterials Where
                                 (mm.IdMaterialType > MaterialType.Raw AndAlso mm.IdMaterialType < MaterialType.Subsidiary) AndAlso
                                 mm.IDMachineType = 1
                                 Select New With {.ID = mm.ID, .Name = mm.Name}).ToList()
            Dim modelMeasurement = (From ms In ctx.MstMeasurementUnits Select New With {.ID = ms.ID, .Symbol = ms.Symbol}).ToList()
            Dim modelMachine = (From mc In ctx.MstMachines
                                Where mc.IdMachineType = 1
                                Select New With {.ID = mc.ID, .MachineName = mc.MachineName}).ToList()

            ViewData("lstWorkSchedule") = modelWorkSchedule
            ViewData("lstMaterial") = modelMaterial
            ViewData("lstMeasurement") = modelMeasurement
            ViewData("lstMachine") = modelMachine
        End Sub

        Function CreateCrusherJournal() As ActionResult
            Call CrusherJournalViewData()
            Return View()
        End Function

        <HttpPost()> _
        Function CreateCrusherJournal(ByVal model As ModelCrusherJournal, ByVal WorkDate As String) As ActionResult
            'If ModelState.IsValid Then
            Try
                model.WorkDate = StrtoDatetime(WorkDate)
                'Simpan data inventory baru
                Dim modelInventory As New MaterialInventories With {
                    .IdInventoryStatus = 4,
                    .IsPlus = False
                }
                ctx.MaterialInventories.AddObject(modelInventory)
                ctx.SaveChanges()

                'Simpan material use baru untuk input data GROGOL
                Dim newMaterialUse As MaterialUseJournal
                newMaterialUse = New MaterialUseJournal With {
                    .IDMaterial = GetIDMaterialByName("GROGOL"),
                    .DateUse = model.WorkDate,
                    .IdMachine = model.IDMachine,
                    .OperatorName = model.OperatorName,
                    .Amount = model.InputGrogol,
                    .Description = model.Description,
                    .IDInventory = modelInventory.ID
                }
                ctx.MaterialUseJournal.AddObject(newMaterialUse)
                ctx.SaveChanges()

                Dim NewMaterialUseID As Integer = newMaterialUse.ID

                If Not model.InputGresley = 0 OrElse IsNothing(model.InputGresley) Then
                    newMaterialUse = New MaterialUseJournal With {
                        .IDMaterial = GetIDMaterialByName("GRESLEY"),
                        .DateUse = model.WorkDate,
                        .IdMachine = model.IDMachine,
                        .OperatorName = model.OperatorName,
                        .Amount = model.InputGresley,
                        .Description = model.Description,
                        .IDInventory = modelInventory.ID
                    }
                    ctx.MaterialUseJournal.AddObject(newMaterialUse)
                    ctx.SaveChanges()
                End If

                'Simpan data Crusher baru
                Dim newCrusher As New CrusherJournals With {
                    .IdWorkSchedule = model.IDWorkSchedule,
                    .IDMaterialUse = NewMaterialUseID
                }
                ctx.CrusherJournals.AddObject(newCrusher)
                ctx.SaveChanges()

                'Simpan detail crusher
                For i = 1 To 7
                    '1. To prod.MaterialInvetories
                    Dim newInventory As New MaterialInventories With {
                        .IdInventoryStatus = 5,
                        .IsPlus = True
                    }
                    ctx.MaterialInventories.AddObject(newInventory)
                    ctx.SaveChanges()
                    Dim idDetail As Integer
                    Dim amountDetail As Integer
                    Select Case i
                        Case 1
                            idDetail = GetIDMaterialByName("MEDIUM")
                            amountDetail = model.MediumAmount
                        Case 2
                            idDetail = GetIDMaterialByName("ABU BATU")
                            amountDetail = model.AbuBatuAmount
                        Case 3
                            idDetail = GetIDMaterialByName("BASE A")
                            amountDetail = model.BaseAAmount
                        Case 4
                            idDetail = GetIDMaterialByName("BASE B")
                            amountDetail = model.BaseBAmount
                        Case 5
                            idDetail = GetIDMaterialByName("SPLIT 1-2")
                            amountDetail = model.Split12Amount
                        Case 6
                            idDetail = GetIDMaterialByName("SPLIT 2-3")
                            amountDetail = model.Split23Amount
                        Case 7
                            idDetail = GetIDMaterialByName("GRESLEY")
                            amountDetail = model.GresleyOutAmount
                    End Select
                    '2. To prod.CrusherJournalDetails
                    Dim modelDetail As New CrusherJournalDetails With {
                            .IDCrusherJournal = newCrusher.ID,
                            .IDInventory = newInventory.ID,
                            .IDMaterial = idDetail,
                            .OrderProcess = i,
                        .Amount = amountDetail
                    }
                    ctx.CrusherJournalDetails.AddObject(modelDetail)
                    ctx.SaveChanges()
                Next
                Return RedirectToAction("Index")
            Catch ex As Exception
                ModelState.AddModelError("", ex.Message)
            End Try
            'End If

            Call CrusherJournalViewData()
            Return View(model)
        End Function

        Private Function GetIDMaterialByCode(Code As Integer) As Integer
            Dim result = (From mm In ctx.MstMaterials Where mm.Code = Code Select mm.ID).FirstOrDefault()
            If IsNothing(result) Then result = 0
            Return result
        End Function

        Private Function GetIDMaterialByName(ByVal Name As String) As Integer
            Dim result = (From mm In ctx.MstMaterials Where mm.Name.Contains(Name) Select mm.ID).FirstOrDefault()
            If IsNothing(result) Then result = 0
            Return result
        End Function

        Private Function GetNameMaterialByID(ByVal ID As Integer) As String
            Dim result = (From mm In ctx.MstMaterials Where mm.ID = ID Select mm.Name).FirstOrDefault()
            If IsNothing(result) Then result = ""
            Return result
        End Function

        Function Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Try
                Dim delDetail = (From cd In ctx.CrusherJournalDetails Where cd.IDCrusherJournal = id).FirstOrDefault()
                If delDetail IsNot Nothing Then ctx.DeleteObject(delDetail)
                Dim delCrusher = ctx.CrusherJournals.Where(Function(m) m.ID = id).FirstOrDefault()
                If Not IsNothing(delCrusher) Then ctx.DeleteObject(delCrusher)
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Error :" + ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg}, JsonRequestBehavior.AllowGet)
        End Function

        Sub EditCrusherViewData(ByRef model As ModelEditCrusher, ByVal id As Integer)
            Try
                Dim modelCrusher = (From cd In ctx.CrusherJournals
                                    Join muj In ctx.MaterialUseJournal On muj.ID Equals cd.IDMaterialUse
                                    Where cd.ID = id
                                    Select idCrusher = cd.ID,
                                        idMaterialUserJournal = cd.IDMaterialUse,
                                        DateUse = muj.DateUse,
                                        OperatorName = muj.OperatorName,
                                        IdMachine = muj.IdMachine,
                                        IdWorkSchedule = cd.IdWorkSchedule,
                                        Description = muj.Description,
                                        IDMaterialUse = muj.ID
                     ).FirstOrDefault()

                'cd.MaterialUseJournal.Amount
                Dim dataAmount = (From x In ctx.MaterialUseJournal Where x.ID = modelCrusher.IDMaterialUse OrElse x.ID = modelCrusher.IDMaterialUse - 1 Select x.Amount, x.IDMaterial, MaterialUseID = x.ID).ToList()

                Dim i As Byte = 0
                With model
                    For Each o In dataAmount
                        Array.Resize(.Amount, i + 1)
                        Array.Resize(.IDMaterial, i + 1)
                        Array.Resize(.MaterialName, i + 1)
                        Array.Resize(.idMaterialUserJournal, i + 1)
                        .idMaterialUserJournal(i) = o.MaterialUseID
                        .Amount(i) = o.Amount
                        .IDMaterial(i) = o.IDMaterial
                        .MaterialName(i) = GetNameMaterialByID(o.IDMaterial)
                        i += 1
                    Next
                    .DateUse = modelCrusher.DateUse
                    .IdMachine = modelCrusher.IdMachine
                    .IdWorkSchedule = modelCrusher.IdWorkSchedule
                    .OperatorName = modelCrusher.OperatorName
                    .idCrusher = modelCrusher.idCrusher
                    .Description = modelCrusher.Description
                End With
            Catch ex As Exception
                MsgBox(ex.Message)
                Throw New HttpException(404, "NOT FOUND")
            End Try
            ViewData("lstWorkSchedule") = (From ws In ctx.MstWorkSchedule Select New With {.ID = ws.ID, .Shift = ws.Shift}).ToList
            ViewData("lstMachine") = (From mc In ctx.MstMachines
                                Where mc.IdMachineType = 1
                                Select New With {.ID = mc.ID, .MachineName = mc.MachineName}).ToList

            ViewBag.listAmount = (From cs In ctx.CrusherJournalDetails Join m In ctx.MstMaterials On
                                  cs.IDMaterial Equals m.ID Where
                                  cs.IDCrusherJournal = id Select cs.Amount, IDCs = cs.ID, NamaMaterial = m.Name).ToList()
            If Nothing Is model Then
                Throw New HttpException(404, "NOT FOUND")
            End If
        End Sub

        Function EditCrusher(ByVal id As Integer) As ActionResult
            Dim model As New ModelEditCrusher
            EditCrusherViewData(model, id)
            Return View(model)
        End Function

        <HttpPost()> _
        Function EditCrusher(ByVal model As ModelEditCrusher, ByVal IDcs As List(Of Integer), ByVal AMounts As List(Of Double)) As ActionResult
            If ModelState.IsValid Then
                Try
                    Dim i As Integer = 0
                    For i = 0 To model.Amount.Count() - 1
                        'update data material use
                        Dim MaterialID As Double = model.idMaterialUserJournal(i)
                        Dim dataUseJournal = (From mu In ctx.MaterialUseJournal Where mu.ID = MaterialID).FirstOrDefault()
                        If Not IsNothing(dataUseJournal) Then
                            With dataUseJournal
                                .Amount = model.Amount(i)
                                .IDMaterial = model.IDMaterial(i)
                                .DateUse = model.DateUse
                                .IdMachine = model.IdMachine
                                .OperatorName = model.OperatorName
                                .Description = model.Description
                            End With
                        Else
                            'Throw New HttpException(404, "NOT FOUND")
                            ModelState.AddModelError("", "Code: MaterialUse-NOTFOUND<br />Message : Terjadi kesalahan, mohon ulang kembali untuk beberapa waktu atau hubungi Administrator")
                        End If
                    Next

                    'update data crusher
                    Dim dataCrusher = (From cu In ctx.CrusherJournals Where cu.ID = model.idCrusher Select cu).FirstOrDefault()
                    If Not IsNothing(dataCrusher) Then
                        With dataCrusher
                            .IdWorkSchedule = model.IdWorkSchedule
                        End With
                    Else
                        'Throw New HttpException(404, "NOT FOUND")
                        ModelState.AddModelError("", "Code: Journals-NOTFOUND<br />Message : Terjadi kesalahan, mohon ulang kembali untuk beberapa waktu atau hubungi Administrator")
                    End If

                    'update crusherdetails
                    For counter As Integer = 0 To IDcs.Count - 1
                        Dim Id As Integer = IDcs(counter)
                        Dim dtCrusherDetails = (From cds In ctx.CrusherJournalDetails Where cds.ID = Id Select cds).FirstOrDefault()
                        If AMounts(counter) = 0 OrElse IsNothing(AMounts(counter)) Then
                            ctx.CrusherJournalDetails.DeleteObject(dtCrusherDetails)
                        Else
                            With dtCrusherDetails
                                .Amount = AMounts(counter)
                            End With
                        End If
                    Next
                    ctx.SaveChanges()
                    Return RedirectToAction("Index")
                Catch ex As Exception
                    'Throw New HttpException(404, "NOT FOUND")
                    ModelState.AddModelError("", "Message : " & ex.Message)
                End Try

            EditCrusherViewData(model, model.idCrusher)
            End If
            Return View(model)
        End Function

        Function PrintReportPdf(ByVal dataMonth As Byte, dataYear As Integer) As ActionResult
            Dim model = ctx.GetCrusherJournalData(dataMonth, dataYear).ToList
            Try
                'set path of rdlc
                Dim mimeType As String = "application/pdf"
                Dim Encoding As String = Nothing
                Dim fileNameExtension As String = Nothing
                Dim streams As String() = Nothing
                Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

                Dim r As New Microsoft.Reporting.WebForms.LocalReport
                r.EnableExternalImages = True
                r.ReportPath = Server.MapPath("~/Reports/HasilCrusher.rdlc")

                r.SetParameters(New ReportParameter("Month", dataMonth))
                r.SetParameters(New ReportParameter("Year", dataYear))
                r.DataSources.Add(New ReportDataSource("DsCrusher", model))
                r.Refresh()
                Dim reportType As String = "PDF"
                'Dim deviceInfo As String = "<DeviceInfo>" &
                '"  <OutputFormat>PDF</OutputFormat>" &
                '"  <PageWidth>11.69in</PageWidth>" &
                '"  <PageHeight>13in</PageHeight>" &
                '"  <MarginTop>0.0in</MarginTop>" &
                '"  <MarginLeft>0.0in</MarginLeft>" &
                '"  <MarginRight>0.0in</MarginRight>" &
                '"  <MarginBottom>0.0in</MarginBottom>" &
                '"</DeviceInfo>"

                Dim output As Byte()
                output = r.Render(reportType, "", mimeType, Encoding, fileNameExtension, streams, warnings)

                Return File(output, mimeType, "LaporanCrusher.pdf")
            Catch ex As Exception
                TempData("exeptionMsg") = ex.Message
                Return RedirectToRoute("Default", New With {.Action = "Index", .Controller = "Error"})
            End Try
        End Function

        Public Function PrintReportXls(ByVal dataMonth As Byte, dataYear As Integer) As FileResult
            Dim model = ctx.GetCrusherJournalData(dataMonth, dataYear).ToList

            Dim wb As XLWorkbook = New XLWorkbook()
            Dim ws As IXLWorksheet = wb.Worksheets.Add("Dok1")
            'Title
            With ws.Range("B1", "O1")
                .Merge()
                .SetValue("Hasil Crusher Periode Bulan " & _
                    Choose(dataMonth, "Januari", "Februari", "Maret", "April", "May", _
                        "Juni", "Juli", "Agustus", "September", "Oktober", "Nopember", "Desember") & _
                        " " & dataYear)
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 14
                End With
            End With
            With ws.Range("B3", "O3")
                .Merge()
                .SetValue("Hasil Crusher dari tanggal 1 - " & _
                    Day(DateAdd(DateInterval.Day, -1, DateSerial(dataYear, dataMonth + 1, 1))) & " " & _
                    Choose(dataMonth, "Januari", "Februari", "Maret", "April", "May", _
                        "Juni", "Juli", "Agustus", "September", "Oktober", "Nopember", "Desember") & _
                        " " & dataYear)
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                    .Font.Bold = True
                    .Font.Italic = True
                    .Font.FontName = "Times New Roman"
                    .Font.FontSize = 12
                End With
            End With
            'Header column
            With ws.Range("B4", "B10")
                .Merge()
                .SetValue("No")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thick
                    .Border.RightBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Range("C4", "C10")
                .Merge()
                .SetValue("Tgl")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thick
                    .Border.RightBorder = XLBorderStyleValues.Thin
                    .Border.LeftBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Range("D4", "J4")
                .Merge()
                .SetValue("Keterangan")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                    .Border.TopBorder = XLBorderStyleValues.Thick
                End With
            End With
            With ws.Range("D5", "D6")
                .Merge()
                .SetValue("Grogol")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Algerian"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Range("E5", "E6")
                .Merge()
                .SetValue("Medium")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Algerian"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Range("F5", "F6")
                .Merge()
                .SetValue("Abu Batu")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Algerian"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Range("G5", "G6")
                .Merge()
                .SetValue("Base A")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Algerian"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Range("H5", "H6")
                .Merge()
                .SetValue("Base B")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Algerian"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Range("I5", "I6")
                .Merge()
                .SetValue("Split 1, 2")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Algerian"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Range("J5", "J6")
                .Merge()
                .SetValue("Split 2, 3")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Algerian"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Range("D7", "J7")
                .Merge()
                .SetValue("Jumlah material per (m3)")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Range("D8", "J8")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                    .Border.InsideBorder = XLBorderStyleValues.Thin
                    .NumberFormat.SetNumberFormatId(3)
                End With
            End With
            With ws.Range("D9", "J9")
                .Merge()
                .SetValue("Jumlah material per (bucket)")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGray)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Range("D10", "J10")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGray)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                    .Border.InsideBorder = XLBorderStyleValues.Thin
                    .Border.BottomBorder = XLBorderStyleValues.Thick
                    .NumberFormat.SetNumberFormatId(3)
                End With
            End With
            With ws.Range("K4", "K6")
                .Merge()
                .SetValue("Satuan")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                    .Border.TopBorder = XLBorderStyleValues.Thick
                End With
            End With
            With ws.Range("K7", "K9")
                .Merge()
                .SetValue("Total Material (bucket)")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Cell("K10")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGray)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                    .Border.BottomBorder = XLBorderStyleValues.Thick
                    .NumberFormat.SetNumberFormatId(3)
                End With
            End With
            With ws.Range("L4", "L10")
                .Merge()
                .SetValue("M3")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thick
                    .Border.RightBorder = XLBorderStyleValues.Thin
                    .Border.LeftBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Range("M4", "M6")
                .Merge()
                .SetValue("Jumlah")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                    .Border.TopBorder = XLBorderStyleValues.Thick
                End With
            End With
            With ws.Range("M7", "M9")
                .Merge()
                .SetValue("Total Material (m3)")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Cell("M10")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGray)
                    .Border.OutsideBorder = XLBorderStyleValues.Thin
                    .Border.BottomBorder = XLBorderStyleValues.Thick
                    .NumberFormat.SetNumberFormatId(3)
                End With
            End With
            With ws.Range("N4", "N10")
                .Merge()
                .SetValue("Keterangan")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thick
                    .Border.RightBorder = XLBorderStyleValues.Thin
                    .Border.LeftBorder = XLBorderStyleValues.Thin
                End With
            End With
            With ws.Range("O4", "O10")
                .Merge()
                .SetValue("Anggota")
                With .Style
                    .Alignment.Vertical = XLAlignmentVerticalValues.Center
                    .Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    .Font.Bold = True
                    .Font.FontName = "Tahoma"
                    .Font.FontSize = 10
                    .Fill.SetBackgroundColor(XLColor.LightGreen)
                    .Border.OutsideBorder = XLBorderStyleValues.Thick
                    .Border.LeftBorder = XLBorderStyleValues.Thin
                End With
            End With
            ws.Range("B4", "O10").Style.Alignment.WrapText = True
            ws.Column("B").Width = 4.75
            ws.Column("C").Width = 12
            ws.Column("D").Width = 11
            ws.Column("E").Width = 11
            ws.Column("F").Width = 11
            ws.Column("G").Width = 11
            ws.Column("H").Width = 11
            ws.Column("I").Width = 11
            ws.Column("J").Width = 11
            ws.Column("K").Width = 9.5
            ws.Column("L").Width = 6
            ws.Column("M").Width = 8.38
            ws.Column("N").Width = 13.13
            ws.Column("O").Width = 8

            'Insert data
            Dim rowCount = 11
            Dim cekTanggal As Date
            Dim noBaris = 1
            Dim startRowMergeNo = 11
            Dim startRowMergeTgl = 11

            For Each Item In model
                With ws.Range("B" & rowCount, "O" & rowCount).Style.Border
                    .OutsideBorder = XLBorderStyleValues.Thin
                    .InsideBorder = XLBorderStyleValues.Thin
                End With

                If rowCount = 11 Then
                    cekTanggal = Item.Tanggal
                    ws.Cells("B" & rowCount).Value = noBaris
                Else
                    If cekTanggal <> Item.Tanggal Then
                        noBaris += 1
                        ws.Cells("B" & rowCount).Value = noBaris
                        If rowCount > 11 Then
                            ws.Range("B" & startRowMergeNo, "B" & rowCount - 1).Merge()
                            startRowMergeNo = rowCount
                        End If
                    End If
                End If
                ws.Cells("C" & rowCount).Style.DateFormat.Format = "dd-mmm-yy"
                ws.Cells("B" & rowCount).Style.Border.LeftBorder = XLBorderStyleValues.Double
                If Item.GROGOL > 0 Then
                    If rowCount > 11 Then
                        ws.Range("C" & startRowMergeTgl, "C" & rowCount - 1).Merge()
                        startRowMergeTgl = rowCount
                    End If
                    ws.Cells("C" & rowCount).Value = Item.Tanggal
                    ws.Cells("D" & rowCount).Value = Item.GROGOL
                    ws.Cells("D" & rowCount).Style.Fill.SetBackgroundColor(XLColor.LightGreen)
                End If
                If Item.MEDIUM > 0 Then
                    ws.Cells("E" & rowCount).Value = Item.MEDIUM
                    ws.Cells("E" & rowCount).Style.Fill.SetBackgroundColor(XLColor.LightGreen)
                End If
                If Item.ABUBATU > 0 Then
                    ws.Cells("F" & rowCount).Value = Item.ABUBATU
                    ws.Cells("F" & rowCount).Style.Fill.SetBackgroundColor(XLColor.LightGreen)
                End If
                If Item.BASEA > 0 Then
                    ws.Cells("G" & rowCount).Value = Item.BASEA
                    ws.Cells("G" & rowCount).Style.Fill.SetBackgroundColor(XLColor.LightGreen)
                End If
                If Item.BASEB > 0 Then
                    ws.Cells("H" & rowCount).Value = Item.BASEB
                    ws.Cells("H" & rowCount).Style.Fill.SetBackgroundColor(XLColor.LightGreen)
                End If
                If Item.SPLIT12 > 0 Then
                    ws.Cells("I" & rowCount).Value = Item.SPLIT12
                    ws.Cells("I" & rowCount).Style.Fill.SetBackgroundColor(XLColor.LightGreen)
                End If
                If Item.SPLIT23 > 0 Then
                    ws.Cells("J" & rowCount).Value = Item.SPLIT23
                    ws.Cells("J" & rowCount).Style.Fill.SetBackgroundColor(XLColor.LightGreen)
                End If

                ws.Cells("K" & rowCount).Value = Item.SatuanBucket
                ws.Cells("L" & rowCount).Value = Item.M3
                ws.Cells("M" & rowCount).Value = Item.JumlahKubik
                ws.Cells("N" & rowCount).Value = Item.Keterangan
                ws.Cells("O" & rowCount).Style.Border.RightBorder = XLBorderStyleValues.Double

                rowCount += 1
            Next
            ws.Range("B" & startRowMergeNo, "B" & rowCount - 1).Merge()
            ws.Range("C" & startRowMergeTgl, "C" & rowCount - 1).Merge()
            With ws.Range("B11", "O" & rowCount - 1).Style.Alignment
                .Horizontal = XLAlignmentHorizontalValues.Center
                .Vertical = XLAlignmentVerticalValues.Center
            End With
            ws.Range("B11", "B" & rowCount - 1).Style.Font.Bold = True
            With ws.Range("D11", "J" & rowCount - 1).Style
                .NumberFormat.Format = "#,##0"
            End With
            With ws.Range("L11", "M" & rowCount - 1).Style
                .NumberFormat.Format = "#,##0"
            End With
            With ws.Range("M11", "M" & rowCount - 1).Style
                .Font.SetFontColor(XLColor.Purple)
                .Font.Bold = True
            End With
            ws.Cell("D10").SetFormulaR1C1("=SUM(" & ws.Range("D11", "D" & rowCount - 1).ToString() & ")")
            ws.Cell("D8").SetFormulaR1C1("=D10 * 3")
            ws.Cell("E10").SetFormulaR1C1("=SUM(" & ws.Range("E11", "E" & rowCount - 1).ToString() & ")")
            ws.Cell("E8").SetFormulaR1C1("=E10 * 3")
            ws.Cell("F10").SetFormulaR1C1("=SUM(" & ws.Range("F11", "F" & rowCount - 1).ToString() & ")")
            ws.Cell("F8").SetFormulaR1C1("=F10 * 3")
            ws.Cell("G10").SetFormulaR1C1("=SUM(" & ws.Range("G11", "G" & rowCount - 1).ToString() & ")")
            ws.Cell("G8").SetFormulaR1C1("=G10 * 3")
            ws.Cell("H10").SetFormulaR1C1("=SUM(" & ws.Range("H11", "H" & rowCount - 1).ToString() & ")")
            ws.Cell("H8").SetFormulaR1C1("=H10 * 3")
            ws.Cell("I10").SetFormulaR1C1("=SUM(" & ws.Range("I11", "I" & rowCount - 1).ToString() & ")")
            ws.Cell("I8").SetFormulaR1C1("=I10 * 3")
            ws.Cell("J10").SetFormulaR1C1("=SUM(" & ws.Range("J11", "J" & rowCount - 1).ToString() & ")")
            ws.Cell("J8").SetFormulaR1C1("=J10 * 3")

            ws.Cell("K10").SetFormulaR1C1("=SUM(" & ws.Range("D10", "J10").ToString() & ")")
            ws.Cell("M10").SetFormulaR1C1("=K10 * 3")

            'Save
            Dim stream As New MemoryStream
            wb.SaveAs(stream)

            Return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LAPORANCRUSHER-" & dataMonth & "-" & dataYear & ".xlsx")
            'End Using
        End Function

    End Class
End Namespace
