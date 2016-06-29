Imports Microsoft.Reporting.WebForms
Namespace Equipment.Areas.Equipment.Controllers
    Public Class ReportController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /Equipment/Report
        Dim ctx As New EquipmentEntities
        Function Index(ByVal id As Integer) As ActionResult
            Dim modelActivities = ctx.ReportHeavyEqpActivities(id)
            Dim detailOp = ctx.DetailOperation(id)
            Dim detailNonOp =
                 (From a In ctx.HeavyEqpNonOperations Select a
                  Where a.IDActivity = id).AsEnumerable().
 Select(Function(p, row) New With {.Begin = p.Begin.Hours & ":" & p.Begin.Minutes, _
                                     .End = p.End.Hours & ":" & p.End.Minutes, p.NonOperationType,
                                     p.Reason, .No = row + 1}).ToList()
            Dim detailOil = ctx.DetailOil(id, 0)
            Dim detailFuel = ctx.DetailFuel(id, 0)
                Dim mimeType As String = "application/pdf"
                Dim Encoding As String = Nothing
                Dim fileNameExtension As String = Nothing
                Dim streams As String() = Nothing
                Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

                Dim r As New Microsoft.Reporting.WebForms.LocalReport
                r.EnableExternalImages = True
                r.ReportPath = Server.MapPath("~/Areas/Equipment/Reports/RptHeavyActivities.rdlc")
                'r.ReportPath = System.AppDomain.CurrentDomain.BaseDirectory() & "Reports\ReportBeritaAcara.rdlc"

                If Not IsNothing(modelActivities) Then
                    'r.SetParameters(New ReportParameter("projectInfoId", id))
                    r.DataSources.Add(New ReportDataSource("DataSet1", modelActivities))
                r.DataSources.Add(New ReportDataSource("DataSetOp", detailOp))
                r.DataSources.Add(New ReportDataSource("DataSetNonOp", detailNonOp))
                r.DataSources.Add(New ReportDataSource("DataSetOil", detailOil))
                r.DataSources.Add(New ReportDataSource("DataSetFuel", detailFuel))
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


        End Function
        Function ReportDTActivities(ByVal id As Integer) As ActionResult
            Dim modelActivities = ctx.ReportDumpTruckActivities(id)
            Dim detailOp = ctx.DetailDumpTruckOperation(id)
            Dim detailNonOp =
                (From a In ctx.DumpTruckNonOperations Select a
                 Where a.IDActivity = id).AsEnumerable().
Select(Function(p, index) New With {.Begin = p.Begin.Hours & ":" & p.Begin.Minutes, _
                                    .End = p.End.Hours & ":" & p.End.Minutes, p.NonOperationType,
                                    p.Reason, .No = index + 1}).ToList()
            Dim detailOil = ctx.DetailOil(0, id)
            Dim detailFuel = ctx.DetailFuel(0, id)
            Dim mimeType As String = "application/pdf"
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing
            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.EnableExternalImages = True
            r.ReportPath = Server.MapPath("~/Areas/Equipment/Reports/RptDumpTruckActivities.rdlc")
            'r.ReportPath = System.AppDomain.CurrentDomain.BaseDirectory() & "Reports\ReportBeritaAcara.rdlc"

            If Not IsNothing(modelActivities) Then
                'r.SetParameters(New ReportParameter("projectInfoId", id))
                r.DataSources.Add(New ReportDataSource("DataSetDT", modelActivities))
                r.DataSources.Add(New ReportDataSource("DataSetDTOp", detailOp))
                r.DataSources.Add(New ReportDataSource("DataSetNonOp", detailNonOp))
                r.DataSources.Add(New ReportDataSource("DataSetOil", detailOil))
                r.DataSources.Add(New ReportDataSource("DataSetFuel", detailFuel))
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

        End Function

        Function ReportTradoActivities(ByVal id As Integer) As ActionResult
            Dim modelActivities = ctx.ReportTradoActivities(id)
            Dim detailOp = ctx.ProcTradoOperations(id)
            Dim detailNonOp = ctx.ProcTradoNonOp(id)
            '                (From a In ctx.DumpTruckNonOperations Select a
            '                 Where a.IDActivity = id).AsEnumerable().
            'Select(Function(p, index) New With {.Begin = p.Begin.Hours & ":" & p.Begin.Minutes, _
            '                                    .End = p.End.Hours & ":" & p.End.Minutes, p.NonOperationType,
            '                                    p.Reason, .No = index + 1}).ToList()
            Dim detailOil = ctx.ProcOil_SparePart(id)
            Dim detailFuel = ctx.ProcBBMTrado(id)
            Dim detailFuelOut = ctx.ProcBBMTradoOut(id)
            Dim mimeType As String = "application/pdf"
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing
            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.EnableExternalImages = True
            r.ReportPath = Server.MapPath("~/Areas/Equipment/Reports/RptTradoActivities.rdlc")
            'r.ReportPath = System.AppDomain.CurrentDomain.BaseDirectory() & "Reports\ReportBeritaAcara.rdlc"

            If Not IsNothing(modelActivities) Then
                'r.SetParameters(New ReportParameter("projectInfoId", id))
                r.DataSources.Add(New ReportDataSource("DataSetActivity", modelActivities))
                r.DataSources.Add(New ReportDataSource("DataSetOperation", detailOp))
                r.DataSources.Add(New ReportDataSource("DataSetNonOp", detailNonOp))
                r.DataSources.Add(New ReportDataSource("DataSetOilSparePart", detailOil))
                r.DataSources.Add(New ReportDataSource("DataSetBBM", detailFuel))
                r.DataSources.Add(New ReportDataSource("DataSetBBMOut", detailFuelOut))
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
        End Function

        Function ReportVehicleMaintenance(ByVal id As Integer) As ActionResult
            Dim vehiclerecord = ctx.ReportVehicleMaintenance(id).ToList
            Dim vehiclerecorditem = ctx.ReportVehicleMaintenanceItemWork(id).ToList
            Dim vehiclerecordmaterial = ctx.ReportVehicleMaintenanceMaterialUsed(id).ToList
           
            Dim vehiclerecordother = ctx.ReportVehicleMaintenanceOther(id).ToList

            If vehiclerecorditem.Count < 5 Then
                For counter As Integer = vehiclerecorditem.Count To 4
                    Dim x As New ReportVehicleMaintenanceItemWork_Result
                    x.ItemPekerjaan = ""
                    vehiclerecorditem.Add(x)
                Next
            End If

            If vehiclerecordmaterial.Count < 5 Then
                For count As Integer = vehiclerecordmaterial.Count To 4
                    Dim m As New ReportVehicleMaintenanceMaterialUsed_Result
                    m.MaterialUsed = ""
                    m.Quantity = 0.0
                    m.UnitQuantity = ""
                    vehiclerecordmaterial.Add(m)
                Next
            End If

            If vehiclerecordother.Count <= 4 Then
                For counters As Integer = vehiclerecordother.Count To 4
                    Dim x As New ReportVehicleMaintenanceOther_Result
                    x.Cost = 0
                    x.Item = ""
                    x.Remarks = ""
                    vehiclerecordother.Add(x)
                Next
            End If

            Dim mimeType As String = "application/pdf"
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing
            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.EnableExternalImages = True
            r.ReportPath = Server.MapPath("~/Areas/Equipment/Reports/VehicleMaintenanceReport.rdlc")

            If Not IsNothing(vehiclerecord) Then
                
                r.DataSources.Add(New ReportDataSource("DataSetKendaraan", vehiclerecord))
                r.DataSources.Add(New ReportDataSource("DataSetItemPekerjaan", vehiclerecorditem))
                r.DataSources.Add(New ReportDataSource("DataSetMaterial", vehiclerecordmaterial))
                r.DataSources.Add(New ReportDataSource("DataSetOther", vehiclerecordother))
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
        End Function

        Function ReportHeavyEquipmentMaintenance(ByVal id As Integer) As ActionResult
            Dim heavyeqprecord = ctx.HeavyEqpMaintenanceRecords.Where(Function(m) m.Id = id).SingleOrDefault()
            Dim heavyeqprecorditem = (From m In heavyeqprecord.HeavyEqpMaintenanceRecordItems
                                   Select New With {.no = 1, .ItemPekerjaan = m.Item}).ToList

            Dim heavyeqprecordmaterial = (From m In heavyeqprecord.HeavyEqpMaintenanceRecordMaterialUseds
                                         Select New With {.no = 1, .MaterialUsed = m.MaterialUsed, .Quantity = m.Quantity,
                                                          .UnitQuantity = m.UnitQuantity}).ToList

            Dim heavyeqprecordother = ctx.HeavyEqpMaintenanceRecordOthers.ToList()
            For i As Integer = heavyeqprecorditem.Count To 4
                heavyeqprecorditem.Add(New With {.no = 1, .ItemPekerjaan = ""})
            Next
            For i As Integer = heavyeqprecordmaterial.Count To 4
                heavyeqprecordmaterial.Add(New With {.no = 1, .MaterialUsed = "", .Quantity = 0.0,
                                                          .UnitQuantity = ""})
            Next
            For i As Integer = heavyeqprecordother.Count To 4
                Dim x As New HeavyEqpMaintenanceRecordOther
                heavyeqprecordother.Add(x)
            Next


            Dim mimeType As String = "application/pdf"
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing
            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.EnableExternalImages = True
            r.ReportPath = Server.MapPath("~/Areas/Equipment/Reports/HeavyEquipmentMaintenanceReport.rdlc")

            If Not IsNothing(heavyeqprecord) Then
                r.SetParameters(New ReportParameter("Code", heavyeqprecord.HeavyEqp.Code))
                r.SetParameters(New ReportParameter("MerkType", heavyeqprecord.HeavyEqp.Merk & "-" & heavyeqprecord.HeavyEqp.Type))
                r.SetParameters(New ReportParameter("Status", heavyeqprecord.MaintenanceState.ToString()))
                r.SetParameters(New ReportParameter("Species", heavyeqprecord.HeavyEqp.Species))
                r.SetParameters(New ReportParameter("Startdate", heavyeqprecord.MaintenanceDateStart))
                r.SetParameters(New ReportParameter("EndDate", heavyeqprecord.MaintenanceDateEnd))
                r.SetParameters(New ReportParameter("HourMeter", heavyeqprecord.HourmeterValue))
                r.DataSources.Add(New ReportDataSource("DataSetPekerjaan", heavyeqprecorditem))
                r.DataSources.Add(New ReportDataSource("DataSetMaterial", heavyeqprecordmaterial))
                r.DataSources.Add(New ReportDataSource("DataSetOther", heavyeqprecordother))
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
        End Function

        Function ReportMachineMaintenance(ByVal id As Integer) As ActionResult
            Dim machinerecord = ctx.ReportMachineMaintenance(id).ToList
            Dim machinerecorditem = ctx.ReportMachineMaintenanceItemWork(id).ToList
            Dim machinerecordmaterial = ctx.ReportMachineMaintenanceMaterialUsed(id).ToList
            Dim machinerecordother = ctx.ReportMachineMaintenanceOther(id).ToList

            If machinerecorditem.Count <= 4 Then
                For count As Integer = machinerecorditem.Count To 4
                   
                    Dim m As New ReportMachineMaintenanceItemWork_Result
                    m.ItemPekerjaan = ""
                    m.No = count
                    machinerecorditem.Add(m)
                Next
            End If

            If machinerecordmaterial.Count <= 4 Then
                For counter As Integer = machinerecordmaterial.Count To 4
                    Dim n As New ReportMachineMaintenanceMaterialUsed_Result
                    n.MaterialUsed = ""
                    n.No = counter
                    n.Quantity = 0.0
                    n.UnitQuantity = ""
                    machinerecordmaterial.Add(n)
                Next
            End If

            If machinerecordother.Count <= 4 Then
                For counters As Integer = machinerecordother.Count To 4
                    Dim o As New ReportMachineMaintenanceOther_Result
                    o.Cost = 0
                    o.Item = ""
                    o.Remarks = ""
                    o.No = counters
                    machinerecordother.Add(o)
                Next
            End If

            Dim mimeType As String = "application/pdf"
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing
            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.EnableExternalImages = True
            r.ReportPath = Server.MapPath("~/Areas/Equipment/Reports/MachineMaintenanceReport.rdlc")
            'r.ReportPath = System.AppDomain.CurrentDomain.BaseDirectory() & "Reports\ReportBeritaAcara.rdlc"

            If Not IsNothing(machinerecord) Then
                'r.SetParameters(New ReportParameter("projectInfoId", id))
                r.DataSources.Add(New ReportDataSource("DataSetMesin", machinerecord))
                r.DataSources.Add(New ReportDataSource("DataSetItemPekerjaan", machinerecorditem))
                r.DataSources.Add(New ReportDataSource("DataSetMaterialUsed", machinerecordmaterial))
                r.DataSources.Add(New ReportDataSource("DataSetOther", machinerecordother))
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
        End Function


        Function RptEmptyDT() As ActionResult
            Dim modelActivities = ctx.ReportDumpTruckActivities(0)
            Dim mimeType As String = "application/pdf"
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing

            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.EnableExternalImages = True
            r.ReportPath = Server.MapPath("~/Areas/Equipment/Reports/EmptyReportDT.rdlc")
            'r.ReportPath = System.AppDomain.CurrentDomain.BaseDirectory() & "Reports\ReportBeritaAcara.rdlc"
            r.DataSources.Add(New ReportDataSource("DataSetDT", modelActivities))


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
        End Function

        Function RptEmptyHeavyEqp() As ActionResult
            Dim modelActivities = ctx.ReportHeavyEqpActivities(0)
            Dim mimeType As String = "application/pdf"
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing

            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.EnableExternalImages = True
            r.ReportPath = Server.MapPath("~/Areas/Equipment/Reports/EmptyReportHEP.rdlc")
            'r.ReportPath = System.AppDomain.CurrentDomain.BaseDirectory() & "Reports\ReportBeritaAcara.rdlc"
            r.DataSources.Add(New ReportDataSource("DataSet1", modelActivities))


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
        End Function

        Function RptEmptyTrado() As ActionResult
            Dim modelActivities = ctx.ReportTradoActivities(0)
            Dim mimeType As String = "application/pdf"
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing

            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.EnableExternalImages = True
            r.ReportPath = Server.MapPath("~/Areas/Equipment/Reports/EmptyReportTrado.rdlc")
            'r.ReportPath = System.AppDomain.CurrentDomain.BaseDirectory() & "Reports\ReportBeritaAcara.rdlc"
            r.DataSources.Add(New ReportDataSource("DataSet1", modelActivities))


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
        End Function
        
    End Class
End Namespace