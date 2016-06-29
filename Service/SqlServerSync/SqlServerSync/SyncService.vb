Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Configuration
Imports System.ServiceModel

Public Class SyncService
    'Dim ctx As New MDKEntities1
    Dim wsv As MDKAccessSync.MDKAccessSyncSoapClient
    Dim errMessage As New Dictionary(Of Integer, String)

    'Declare service status function
    Private m_oSyncThread As New Thread(New System.Threading.ThreadStart(AddressOf SyncProcess))
    Declare Auto Function SetServiceStatus Lib "advapi32.dll" (ByVal handle As IntPtr, ByRef serviceStatus As ServiceStatus) As Boolean


    Public Sub New(ByVal cmdArgs() As String)
        MyBase.New()

        Dim eventSourceName As String = "SyncServiceSource"
        Dim logName As String = "SyncServiceLog"

        ' This call is required by the designer.
        InitializeComponent()

        'If (cmdArgs.Count() > 0) Then
        '    eventSourceName = cmdArgs(0)
        'End If

        'If (cmdArgs.Count() > 1) Then
        '    logName = cmdArgs(1)
        'End If

        log = New System.Diagnostics.EventLog()
        Me.log = New System.Diagnostics.EventLog

        ' Add any initialization after the InitializeComponent() call.
        If Not System.Diagnostics.EventLog.SourceExists(eventSourceName) Then
            System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName)
        End If

        log.Source = eventSourceName
        log.Log = logName
    End Sub

    <Conditional("DEBUG_SERVICE")> _
    Private Shared Sub DebugMode()
        Debugger.Break()
    End Sub


    Private Sub OnTimer(sender As Object, e As Timers.ElapsedEventArgs)
        Dim eventId As Integer
        log.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId)
        eventId += 1
    End Sub

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        log.WriteEntry("SyncService Starting.")
        DebugMode()
        m_oSyncThread.Start()
    End Sub

    Protected Overrides Sub OnContinue()
        log.WriteEntry("SyncService Continue")
        m_oSyncThread.Start()
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        log.WriteEntry("SyncService Stop")
        m_oSyncThread.Abort()
    End Sub

    Private Sub setWebService()
        'Set up the binding element to match the app.config settings
        Dim endpointStr = ConfigurationManager.AppSettings("WebServiceURL").ToString()
        wsv = New MDKAccessSync.MDKAccessSyncSoapClient
        wsv.Endpoint.Address = New ServiceModel.EndpointAddress(endpointStr)
    End Sub

    Private Sub SyncProcess()
        log.WriteEntry("SyncService Started")
        'Thread.Sleep(ConfigurationManager.AppSettings("DelayTimeOut").ToString())
        Try
            'Do Loops Until killed by Stop Process
            Do
                SyncPass()
                Thread.Sleep(ConfigurationManager.AppSettings("DelayTimeOut").ToString())
            Loop
        Catch ex As Exception
            Dim errTxt = "Get Error before enter the process."
            errTxt += vbCrLf & "Message: '" & ex.Message & "'"
            If Not IsNothing(ex.InnerException) Then errTxt += vbCrLf & "InnerMessage: '" & ex.InnerException.Message & "'"
            errTxt += vbCrLf & "StackTrace: '" & ex.StackTrace & "'"

            log.WriteEntry(errTxt, EventLogEntryType.Error)
        End Try
        m_oSyncThread.Abort()
        log.WriteEntry("SyncService stopped")
    End Sub

    Private Sub SyncPass()
        Dim hint As Integer = 0
        errMessage = New Dictionary(Of Integer, String)
        Try
            log.WriteEntry("Enter SyncProcess", EventLogEntryType.Information)
            Dim FileName As String = ConfigurationManager.AppSettings("FileDB").ToString()
            Dim accConnection As New OleDb.OleDbConnection(ConfigurationManager.AppSettings("ConnectionString").ToString() & ";Data Source=" & FileName) ' & "; Password=libraemas")
            
            'do webbservice settings
            setWebService()

            'Starting to Import the Access Data
            accConnection.Open()

            Thread.Sleep(15000)
            Try
                Dim command = accConnection.CreateCommand
                Dim result As MDKAccessSync.StatusMessage
                log.WriteEntry("WebService Address: " & wsv.Endpoint.Address.ToString, EventLogEntryType.Warning)

                'Table History
                log.WriteEntry("Sync History", EventLogEntryType.Information)
                Dim max_historyID As Integer = wsv.get_MaxIDHistory()

                'Dim accHistory = New OleDb.OleDbDataAdapter("SELECT * FROM tblMisc_History where historyID >" & max_historyID, accConnection)
                command.CommandText = "SELECT * FROM tblMisc_History where historyID > ?"
                command.Parameters.Clear()
                command.Parameters.AddWithValue("?HistoryID", max_historyID)
                Dim accHistory = New OleDb.OleDbDataAdapter(Command)

                Dim tableHistory As DataTable = New DataTable
                accHistory.Fill(tableHistory)

                Dim modelHistory As New MDKAccessSync.tblMisc_History
                For Each item As DataRow In tableHistory.Rows
                    modelHistory = New MDKAccessSync.tblMisc_History With {
                        .historyID = Trim(item("historyID")), _
                        .jenis = Trim(item("jenis")), _
                        .norecord = Trim(item("norecord")), _
                        .Operator = Trim(item("Operator")), _
                        .description = Trim(item("description")), _
                        .waktu = item("waktu") _
                    }
                    result = wsv.insert_History(modelHistory)
                    If result.Status = False Then
                        errMessage.Add(hint + 1, result.Message)
                        Exit For
                    End If
                Next

                'Tabel Perusahaan
                log.WriteEntry("Sync Perusahaan", EventLogEntryType.Information)
                Dim max_KodePeru = wsv.get_MaxKodePeru()
                
                'Dim accPerusahaan = New OleDb.OleDbDataAdapter("SELECT * FROM TblMst_Perusahaan where KodePeru >" & max_KodePeru, accConnection)
                command.CommandText = "SELECT * FROM TblMst_Perusahaan where KodePeru > ?"
                command.Parameters.Clear()
                command.Parameters.AddWithValue("?max_KodePeru", max_KodePeru)
                Dim accPerusahaan = New OleDb.OleDbDataAdapter(command)

                Dim tablePerusahaan As New DataTable
                accPerusahaan.Fill(tablePerusahaan)

                Dim modelPerusahaan As New MDKAccessSync.TblMst_Perusahaan
                For Each item As DataRow In tablePerusahaan.Rows
                    modelPerusahaan = New MDKAccessSync.TblMst_Perusahaan With {
                        .KodePeru = Trim(item("KodePeru")), _
                        .NamaPeru = item("NamaPeru"), _
                        .NoPeru = Trim(item("NoPeru")), _
                        .AlamatPeru1 = Trim(item("AlamatPeru1")), _
                        .AlamatPeru2 = Trim(item("AlamatPeru2")), _
                        .FaxPeru = Trim(item("FaxPeru")), _
                        .KotaPeru = item("KotaPeru"), _
                        .TelpPeru = Trim(item("TelpPeru")) _
                    }
                    result = wsv.insert_Perusahaan(modelPerusahaan)
                    If result.Status = False Then
                        errMessage.Add(hint + 1, result.Message)
                        Exit For
                    End If
                Next

                'Table barang
                log.WriteEntry("Sync Barang", EventLogEntryType.Information)
                Dim maxId_Barang As Integer = wsv.get_KodeBarang()

                'Dim accBarang = New OleDb.OleDbDataAdapter("SELECT * FROM TblMst_Barang where KodeBrg >" & maxId_Barang.ToString, accConnection)
                command.CommandText = "SELECT * FROM TblMst_Barang where KodeBrg > ?"
                command.Parameters.Clear()
                command.Parameters.AddWithValue("?max_KodePeru", maxId_Barang)
                Dim accBarang = New OleDb.OleDbDataAdapter(command)

                Dim tableBarang As DataTable = New DataTable
                accBarang.Fill(tableBarang)
                Dim modelBarang As New MDKAccessSync.TblMst_Barang
                For Each item As DataRow In tableBarang.Rows
                    modelBarang = New MDKAccessSync.TblMst_Barang With {
                       .KodeBrg = item("KodeBrg"), _
                       .NamaBrg = Trim(item("NamaBrg")), _
                       .NoBrg = Trim(item("NoBrg")), _
                       .Keterangan = Trim(item("Keterangan"))
                    }
                    result = wsv.insert_Barang(modelBarang)
                    If result.Status = False Then
                        errMessage.Add(hint + 1, result.Message)
                        Exit For
                    End If
                Next

                'Table NoRec
                log.WriteEntry("Sync NoRec", EventLogEntryType.Information)
                Dim max_NoUrut = wsv.get_maxNoRec()
                'Dim accNoRec = New OleDb.OleDbDataAdapter("SELECT * FROM TblMst_NoRec where NoUrut >" & max_NoUrut, accConnection)
                command.CommandText = "SELECT * FROM TblMst_NoRec where NoUrut > ?"
                command.Parameters.Clear()
                command.Parameters.AddWithValue("?max_KodePeru", maxId_Barang)
                Dim accNoRec = New OleDb.OleDbDataAdapter(command)

                Dim tableNorec As New DataTable
                accNoRec.Fill(tableNorec)
                Dim modelNorec As New MDKAccessSync.TblMst_NoRec
                For Each item As DataRow In tableNorec.Rows
                    modelNorec = New MDKAccessSync.TblMst_NoRec With {
                        .NoRecord = Trim(item("NoRecord")), _
                        .NoUrut = item("NoUrut")
                   }
                    result = wsv.insert_NoRec(modelNorec)
                    If result.Status = False Then
                        errMessage.Add(hint + 1, result.Message)
                        Exit For
                    End If
                Next

                'Table Penimbangan2
                log.WriteEntry("Sync Penimbangan2", EventLogEntryType.Information)
                Dim accPenimbangan2 As OleDbDataAdapter
                Dim tablePenimbangan2 As DataTable = New DataTable
                Dim cmd As OleDbCommand = New OleDbCommand("SELECT * FROM TblTrans_Penimbangan2 where TglMasuk > @dt", accConnection)
                Try
                    Dim maxDateIn As DateTime = wsv.get_TglMasukPenimbangan2()
                    'accPenimbangan2 = New OleDb.OleDbDataAdapter("SELECT * FROM TblTrans_Penimbangan2 where TglMasuk >#" & maxDateIn & "#", accConnection)

                    command.CommandText = "SELECT * FROM TblTrans_Penimbangan2 where TglMasuk > @dt"
                    command.Parameters.Clear()
                    command.Parameters.AddWithValue("@dt", maxDateIn)
                    accPenimbangan2 = New OleDb.OleDbDataAdapter(command)

                    log.WriteEntry("Continue to Sync Penimbangan2 with maxDateIn as reference. Date:" & maxDateIn.ToString("#dd/MM/yyyy HH:mm:ss#"), EventLogEntryType.Warning)
                Catch ex As Exception
                    log.WriteEntry("Sync Penimbangan2 Failed get the upper of TglMasuk because of the function not detected or program code not run correctly", EventLogEntryType.Warning)
                    Dim maxRecord_P2 As Integer = wsv.get_NoPenimbangan2()
                    'accPenimbangan2 = New OleDb.OleDbDataAdapter("SELECT * FROM TblTrans_Penimbangan2 where NoRecord >'" & maxRecord_P2 & "'", accConnection)

                    command.CommandText = "SELECT * FROM TblTrans_Penimbangan2 where NoRecord > ?"
                    command.Parameters.Clear()
                    Command.Parameters.AddWithValue("?NoRecord", maxRecord_P2)
                    accPenimbangan2 = New OleDb.OleDbDataAdapter(Command)

                    log.WriteEntry("Continue to Sync Penimbangan2 with maxRecord as reference", EventLogEntryType.Information)
                End Try
                'log.WriteEntry("Step 1", EventLogEntryType.Warning)
                accPenimbangan2.Fill(tablePenimbangan2)
                'log.WriteEntry("Step 2 and count data found is " & tablePenimbangan2.Rows.Count.ToString(), EventLogEntryType.Warning)
                Dim modelPenimbangan2 As New MDKAccessSync.TblTrans_Penimbangan2
                For Each item As DataRow In tablePenimbangan2.Rows
                    modelPenimbangan2 = New MDKAccessSync.TblTrans_Penimbangan2 With {
                        .NoRecord = Trim(item("NoRecord")), _
                        .Sopir = Trim(item("Sopir")), _
                        .NoPolisi = Trim(item("NoPolisi")), _
                        .TglKeluar = DateTime.Parse(item("TglKeluar")), _
                        .TglMasuk = DateTime.Parse(item("TglMasuk")), _
                        .Berat1 = item("Berat1"), _
                        .Berat2 = item("Berat2"), _
                        .Clerk1 = item("Clerk1"), _
                        .Clerk2 = item("Clerk2"), _
                        .Copy = item("Copy"), _
                        .DeliveryNote = Trim(item("DeliveryNote")), _
                        .KodeBrg = item("KodeBrg"), _
                        .KodePeru = item("KodePeru")
                    }
                    result = wsv.insert_Penimbangan2(modelPenimbangan2)
                    If result.Status = False Then
                        errMessage.Add(hint + 1, result.Message)
                        Exit For
                    End If
                Next

                If errMessage.Count > 0 Then
                    Dim stackErr As String = "Terdapat kesalahan terjadi pada aplikasi. Keterangan sebagai berikut :" & vbCrLf
                    For Each er In errMessage
                        stackErr += " - " & er.Value & vbCrLf
                    Next
                    log.WriteEntry(stackErr, EventLogEntryType.Warning)
                Else
                    log.WriteEntry("SyncService executed, No Activities")
                End If
            Catch ex As Exception
                Dim errTxt = "Get Error when do the process."
                errTxt += vbCrLf & "Message: '" & ex.Message & "'"
                If Not IsNothing(ex.InnerException) Then errTxt += vbCrLf & "InnerMessage: '" & ex.InnerException.Message & "'"
                errTxt += vbCrLf & "StackTrace: '" & ex.StackTrace & "'"

                log.WriteEntry(errTxt, EventLogEntryType.Error)
            End Try

            accConnection.Close()
        Catch ex As Exception
            Dim errTxt = "Get Error when enter the process."
            errTxt += vbCrLf & "Message: '" & ex.Message & "'"
            If Not IsNothing(ex.InnerException) Then errTxt += vbCrLf & "InnerMessage: '" & ex.InnerException.Message & "'"
            errTxt += vbCrLf & "StackTrace: '" & ex.StackTrace & "'"

            log.WriteEntry(errTxt, EventLogEntryType.Error)
        End Try
            log.WriteEntry("SyncProcess has done", EventLogEntryType.SuccessAudit)
    End Sub
End Class