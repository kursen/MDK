Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

<MetadataType(GetType(ProjectInfoDataValidation))>
Public Class ProjectInfo

   
End Class

Class ProjectInfoDataValidation
    <Required(ErrorMessage:="Kode Proyek harus diisi.")> _
    Public ProjectCode As String

    <Required(ErrorMessage:="Nama proyek harus diisi.")>
    Public ProjectTitle As String

    <Required(ErrorMessage:="Tanggal Mulai harus diisi.")>
    Public DateStart As Date

    <Required(ErrorMessage:="No. Kontrak harus diisi.")>
    Public ContractNumber As String

    <Required(ErrorMessage:="Nama Perusahaan Konsultan harus diisi")>
    Public ConsultanName As String


    <Required()>
    <Range(1, 365, ErrorMessage:="Jumlah hari kerja tidak boleh lebih dari 365 hari")>
    Public NumberOfDays As Integer

    <Required()>
    <Range(0, Decimal.MaxValue)>
    Public ContractValue As Decimal
End Class

Public Class ProjectTimeSheetViewer
    Property WeekList As List(Of String)
    Property NumberOfWeeks As Integer
    Property MonthList As List(Of MonthCounter)
    Property WeekItems As List(Of WeekCounter)

    Shared Function CreateProjectTimesheetHeader(ByVal model As ProjectInfo) As ProjectTimeSheetViewer
        Dim rValue As New ProjectTimeSheetViewer

        Dim dateEnd = model.DateStart.AddDays(model.NumberOfDays)
        Dim weekItems = New List(Of WeekCounter)
        Dim arrWeeks As New List(Of String)
        Dim dayStart As Date = model.DateStart
        Dim dayNumberInWeek = CInt(dayStart.DayOfWeek)

      

        Dim arrMonth As New List(Of String)

        arrMonth.Add(dayStart.ToString("MMMM"))

        Dim nMonth As Integer = 0
        Dim nweek As Integer = 1
        Dim dayEnd As Date
        While dayStart < dateEnd

            If dayStart.DayOfWeek <> DayOfWeek.Monday Then
                dayEnd = dayStart.AddDays(7 - CInt(dayStart.DayOfWeek))
            Else
                dayEnd = dayStart.AddDays(6)
            End If
            If dayEnd > dateEnd Then
                dayEnd = dateEnd
            End If
            Dim w As New WeekCounter
            w.DayStartEnd = dayStart.ToString("dd") & "-" & dayEnd.ToString("dd")
            w.MonthName = arrMonth.Last
            w.WeekNumber = nweek
            weekItems.Add(w)

            arrWeeks.Add(w.DayStartEnd)


            dayStart = dayEnd.AddDays(1)
            arrMonth.Add(dayStart.ToString("MMMM"))

            nweek += 1


        End While
        


        rValue.WeekList = arrWeeks

        rValue.NumberOfWeeks = arrWeeks.Count
        Dim ms = From i In weekItems Select i.MonthName
        Dim arrM = From m In ms
                                   Group m By m Into g = Group

        rValue.MonthList = New List(Of MonthCounter)
        For Each item In arrM
            Dim mcounter As New MonthCounter
            mcounter.MonthName = item.m
            mcounter.Count = item.g.Count
            rValue.MonthList.Add(mcounter)
        Next
        rValue.WeekItems = weekItems
        Return rValue

    End Function

    Private Shared Function CreateTimeSheetFooterBase(model As ProjectInfo,
                                                     prm As ProjectManagement_ERPEntities,
                                                     dbWeekValue As List(Of ProjectTimeSheetFooterViewer.WeekValue)) As ProjectTimeSheetFooterViewer

        Dim viewer As New ProjectTimeSheetFooterViewer
        Dim dayStart As Date = model.DateStart
        Dim dateEnd = model.DateStart.AddDays(model.NumberOfDays)
        Dim _weekValues As New List(Of ProjectTimeSheetFooterViewer.WeekValue)
        Dim _weekAccumulation As Double

        Dim weekCounter As Integer = 1
        Dim weightCounter As Decimal = 0


        While dayStart < dateEnd
            Dim dbValue = dbWeekValue.Where(Function(m) m.Weeknumber = weekCounter).SingleOrDefault()
            Dim theWeekValue As New ProjectTimeSheetFooterViewer.WeekValue

            theWeekValue.Weeknumber = weekCounter
            theWeekValue.Datestart = dayStart
            If theWeekValue.Datestart.DayOfWeek <> DayOfWeek.Monday Then
                theWeekValue.DateEnd = dayStart.AddDays(7 - CInt(theWeekValue.Datestart.DayOfWeek))
            Else
                theWeekValue.DateEnd = dayStart.AddDays(6)
            End If

            theWeekValue.Weight = If(dbValue IsNot Nothing, dbValue.Weight, 0)
            _weekAccumulation += theWeekValue.Weight
            theWeekValue.WeightAccumulation = _weekAccumulation
            _weekValues.Add(theWeekValue)

            dayStart = theWeekValue.DateEnd.AddDays(1)
            weekCounter += 1
        End While



        Dim wGroup = From m In _weekValues
                     Group By m.Datestart.Month, m.Datestart.Year Into Group

        Dim _monthValues = New List(Of ProjectTimeSheetFooterViewer.MonthValue)
        weightCounter = 0
        Dim monthCounter As Integer = 1
        For Each item In wGroup
            Dim mValue As New ProjectTimeSheetFooterViewer.MonthValue

            With mValue
                .Month = item.Month
                .Year = item.Year
                .MonthName = (New Date(item.Year, item.Month, 1)).ToString("MMMM")
                .MonthNumber = monthCounter
                .Weight = item.Group.Sum(Function(n) n.Weight)
                weightCounter += mValue.Weight
                .WeightAccumulation = weightCounter
                .MonthlyCost = .Weight * model.ContractValue
                .AccumulationMonthlyCost = .WeightAccumulation * model.ContractValue
            End With

            _monthValues.Add(mValue)
            monthCounter += 1

        Next
        viewer.WeeklyWeight = _weekValues
        viewer.MonthlyWeight = _monthValues
        Return viewer
    End Function


    Public Shared Function CreateTimesheetFooter(ByVal model As ProjectInfo, prm As ProjectManagement_ERPEntities) As ProjectTimeSheetFooterViewer
        Dim spQuery = "Exec [PMn].[GetTaskWeightAccumulation] @projectId"
        Dim dbWeekValue = prm.ExecuteStoreQuery(Of ProjectTimeSheetFooterViewer.WeekValue)(spQuery, New SqlClient.SqlParameter("@projectId", model.Id)).ToList()
        Return CreateTimeSheetFooterBase(model, prm, dbWeekValue)
    End Function
    Public Shared Function GetTimesheetTable(ByVal ProjectId As Integer) As DataTable
        Dim prm As New ProjectManagement_ERPEntities
        Dim dbconnection = DirectCast(CType(prm.Connection, EntityClient.EntityConnection).StoreConnection, SqlClient.SqlConnection)

        Dim cmd = dbconnection.CreateCommand
        cmd.CommandText = "EXEC PMn.GetTimesheetItems @projectId;"
        cmd.Parameters.AddWithValue("@projectId", ProjectId)

        Dim da As New System.Data.SqlClient.SqlDataAdapter(cmd)

        Dim tblTimesheet As New DataTable("data")
        da.Fill(tblTimesheet)
        tblTimesheet.PrimaryKey = {tblTimesheet.Columns("id")}
        cmd.CommandText = "EXEC  PMn.GetProjectTaskWeight @projectId;"
        Dim tblWeight As New DataTable("weight")
        da.Fill(tblWeight)
        If tblWeight.Columns.Count > 0 Then
            For Each c As DataColumn In tblWeight.Columns
                If c.ColumnName = "taskId" Then Continue For
                tblTimesheet.Columns.Add(New DataColumn With {.ColumnName = c.ColumnName, .DataType = c.DataType})
            Next
        Else

            'we need to add column manually
            Dim model = prm.ProjectInfoes.Where(Function(m) m.Id = ProjectId).SingleOrDefault()
            Dim dayend = model.DateStart.AddDays(model.NumberOfDays)
            Dim nWeek = DateDiff(DateInterval.Weekday, model.DateStart, dayend) + 1
            For i = 0 To nWeek
                tblTimesheet.Columns.Add(New DataColumn With {.ColumnName = i.ToString("00000#"), .DataType = GetType(Int32), .DefaultValue = 0})
            Next
        End If
        
        For Each r As DataRow In tblWeight.Rows
            Dim taskId = r("taskId").ToString()
            Dim rowTimesheet = tblTimesheet.Rows.Find(taskId)
            For Each c As DataColumn In tblWeight.Columns
                Dim columname = c.ColumnName
                If columname.Equals("taskId") Then Continue For
                rowTimesheet(columname) = r(columname)
            Next
        Next
        Return tblTimesheet
    End Function

    Public Shared Function GetTimesheetTableMC0(ByVal ProjectId As Integer) As DataTable
        Dim prm As New ProjectManagement_ERPEntities
        Dim dbconnection = DirectCast(CType(prm.Connection, EntityClient.EntityConnection).StoreConnection, SqlClient.SqlConnection)

        Dim cmd = dbconnection.CreateCommand
        cmd.CommandText = "EXEC pmn.GetMutualCheck0View @projectId;"
        cmd.Parameters.AddWithValue("@projectId", ProjectId)

        Dim da As New System.Data.SqlClient.SqlDataAdapter(cmd)


        Dim tblTimesheet As New DataTable("data")
        da.Fill(tblTimesheet)
        tblTimesheet.PrimaryKey = {tblTimesheet.Columns("id")}
        cmd.CommandText = "EXEC  PMn.GetProjectTaskWeightMC0 @projectId;"
        Dim tblWeight As New DataTable("weight")
        da.Fill(tblWeight)
        If tblWeight.Columns.Count > 0 Then
            For Each c As DataColumn In tblWeight.Columns
                If c.ColumnName = "taskId" Then Continue For
                tblTimesheet.Columns.Add(New DataColumn With {.ColumnName = c.ColumnName, .DataType = c.DataType})
            Next
        Else

            'we need to add column manually
            Dim model = prm.ProjectInfoes.Where(Function(m) m.Id = ProjectId).SingleOrDefault()
            Dim dayend = model.DateStart.AddDays(model.NumberOfDays)
            Dim nWeek = DateDiff(DateInterval.Weekday, model.DateStart, dayend) + 1
            For i = 0 To nWeek
                tblTimesheet.Columns.Add(New DataColumn With {.ColumnName = i.ToString("00000#"), .DataType = GetType(Int32), .DefaultValue = 0})
            Next
        End If

        For Each r As DataRow In tblWeight.Rows
            Dim taskId = r("taskId").ToString()
            Dim rowTimesheet = tblTimesheet.Rows.Find(taskId)
            For Each c As DataColumn In tblWeight.Columns
                Dim columname = c.ColumnName
                If columname.Equals("taskId") Then Continue For
                rowTimesheet(columname) = r(columname)
            Next
        Next
        Return tblTimesheet
    End Function

    Public Shared Function CreateTimesheetFooterMC0(ByVal model As ProjectInfo, prm As ProjectManagement_ERPEntities) As ProjectTimeSheetFooterViewer


        Dim spQuery = "Exec [PMn].[GetTaskWeightAccumulationMC0] @projectId"
        Dim dbWeekValue = prm.ExecuteStoreQuery(Of ProjectTimeSheetFooterViewer.WeekValue)(spQuery, New SqlClient.SqlParameter("@projectId", model.Id)).ToList()
        Return CreateTimeSheetFooterBase(model, prm, dbWeekValue)

     
    End Function

    Public Shared Function CreateTimesheetFooterProjectProgress(ByVal model As ProjectInfo, prm As ProjectManagement_ERPEntities) As ProjectTimeSheetFooterViewer


        Dim spQuery = "Exec [PMn].[GetProjectTaskProgressAccumulation] @projectId"
        Dim dbWeekValue = prm.ExecuteStoreQuery(Of ProjectTimeSheetFooterViewer.WeekValue)(spQuery, New SqlClient.SqlParameter("@projectId", model.Id)).ToList()
        Return CreateTimeSheetFooterBase(model, prm, dbWeekValue)


    End Function
    Public Class WeekCounter
        Property MonthName As String
        Property WeekNumber As Integer
        Property DayStartEnd As String
    End Class
    Public Class MonthCounter
        Property MonthName As String
        Property Count As Integer
    End Class

    Public Class ProjectTimeSheetFooterViewer
        Public Property WeeklyWeight As List(Of WeekValue)
        Public Property MonthlyWeight As List(Of MonthValue)
        Class WeekValue
            Property Weeknumber As Integer
            Property Datestart As Date
            Property DateEnd As Date
            Property Weight As Double
            Property WeightAccumulation As Double
        End Class
        Class MonthValue
            Property MonthNumber As Integer
            Property Month As Integer
            Property MonthName As String
            Property Year As Integer
            Property Weight As Double
            Property WeightAccumulation As Double
            Property MonthlyCost As Decimal
            Property AccumulationMonthlyCost As Decimal

        End Class

    End Class
End Class