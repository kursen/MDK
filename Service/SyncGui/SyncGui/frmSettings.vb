Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.ServiceModel

Public Class frmSettings

    Private _ToolTip As New ToolTip()

    Private Function SetConfig(ByVal configFile As String) As System.Configuration.Configuration
        Dim configFileMap As New ExeConfigurationFileMap
        configFileMap.ExeConfigFilename = configFile

        Dim config As System.Configuration.Configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None)

        Return config
    End Function

    Private Sub LoadSetting()
        txtServicePath.Text = My.Settings.ServicePath
        GetConfigOfService()
    End Sub

    Private Sub GetConfigOfService()
        Dim settings = My.Settings
        LabelHint.Text = ""
        disableAll()
        Try
            If txtServicePath.Text <> "" Then
                Dim configFile As String = txtServicePath.Text
                If File.Exists(txtServicePath.Text) Then
                    Dim configFileMap As New ExeConfigurationFileMap
                    configFileMap.ExeConfigFilename = configFile

                    Dim config As System.Configuration.Configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None)
                    If Not IsNothing(config.AppSettings.Settings("FileDB")) Then
                        disableAll(False)
                        If SaveServicePathConf().Status Then
                            LabelHint.Text = ""

                            'connection Service Settings
                            txtConnectionString.Text = config.AppSettings.Settings("ConnectionString").Value()
                            txtFileDB.Text = config.AppSettings.Settings("FileDB").Value()
                            txtTimeOut.Text = config.AppSettings.Settings("DelayTimeOut").Value()

                            'web Service settings
                            txtWebServiceUrl.Text = config.AppSettings.Settings("WebServiceURL").Value()
                        Else
                            LabelHint.Text = "Aplikasi mendapat Error"
                            MsgBox("Terjadi Kesalahan, mohon tunggu beberapa saat lalu kemudian ulangi kembali. Jika terus berlanjut, silahkan hubungi Administrator" + vbCrLf + vbCrLf + "Error Message : " + SaveServicePathConf.Message, MsgBoxStyle.Exclamation)
                        End If
                    Else
                        LabelHint.Text = "File Konfigurasi terpilih tidak sesuai"
                    End If
                Else
                    LabelHint.Text = "File Path yang diminta tidak ada."
                End If
            Else
                LabelHint.Text = "Mohon pilih lokasi File Konfigurasi Service"
            End If
        Catch ex As Exception
            LabelHint.Text = "Mohon pilih lokasi File Konfigurasi Service"
            txtServicePath.Text = settings.ServicePath
        End Try
    End Sub

    Private Function SaveSetting() As Boolean
        Dim StatMsg As New StatusMessage
        If SaveWebServiceConf.Status AndAlso SaveConnServiceConf.Status Then
            Return True
        Else
            If SaveWebServiceConf.Status = False Then
                MsgBox(SaveWebServiceConf.Message, MsgBoxStyle.Exclamation)
            ElseIf SaveConnServiceConf.Status = False Then
                MsgBox(SaveConnServiceConf.Message, MsgBoxStyle.Exclamation)
            End If
        End If
        Return False
    End Function

    Private Function SaveServicePathConf() As StatusMessage
        Dim StatMsg As New StatusMessage
        Try
            My.Settings.ServicePath = txtServicePath.Text
            My.Settings.Save()
            StatMsg.Status = True
        Catch ex As Exception
            StatMsg.Message = "terjadi kesalahan pada aplikasi. mohon hubungi Administrator." + vbCrLf + vbCrLf + "Error Message: " + ex.Message
        End Try
        Return StatMsg
    End Function

    Private Function SaveWebServiceConf() As StatusMessage
        Dim StatMsg As New StatusMessage
        If txtServicePath.Text = "" OrElse txtWebServiceUrl.Text = "" Then
            StatMsg.Message = "Isian tidak boleh kosong!"
        Else
            Try
                My.Settings.WebServiceURL = txtWebServiceUrl.Text
                My.Settings.Save()
                Dim config As Configuration = SetConfig(My.Settings.ServicePath)
                config.AppSettings.Settings("WebServiceURL").Value() = txtWebServiceUrl.Text()
                config.Save()
                StatMsg.Status = True
                Return StatMsg
            Catch ex As Exception
                StatMsg.Message = "terjadi kesalahan pada aplikasi. mohon hubungi Administrator." + vbCrLf + vbCrLf + "Error Message: " + ex.Message
            End Try
        End If
        Return StatMsg
    End Function

    Private Function SaveConnServiceConf() As StatusMessage
        Dim StatMsg As New StatusMessage
        If txtConnectionString.Text = "" OrElse txtFileDB.Text = "" OrElse txtTimeOut.Text = "" Then
            StatMsg.Message = "Isian tidak boleh kosong!"
        Else
            Try
                Dim config = SetConfig(My.Settings.ServicePath)
                config.AppSettings.Settings("ConnectionString").Value = txtConnectionString.Text
                config.AppSettings.Settings("FileDB").Value = txtFileDB.Text
                config.AppSettings.Settings("DelayTimeOut").Value = txtTimeOut.Text
                config.Save()
                StatMsg.Status = True
                Return StatMsg
            Catch ex As Exception
                StatMsg.Message = "terjadi kesalahan pada aplikasi. mohon hubungi Administrator." + vbCrLf + vbCrLf + "Error Message: " + ex.Message
            End Try
        End If
        Return StatMsg
    End Function

    Private Function OpenFileDialogConf() As OpenFileDialog
        Dim fd As OpenFileDialog = New OpenFileDialog()
        fd.Title = "File Dialog"
        fd.InitialDirectory = Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly().Location)
        'fd.Filter = "All File (*.*)|*.*|XML Configuration File (*.config)|*.config"
        fd.Filter = "XML Configuration File (*.config)|*.config"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True
        fd.CheckFileExists = True
        Return fd
    End Function

    Private Sub disableAll(Optional ByVal isDisabled As Boolean = True)
        If isDisabled Then
            groupWebService.Enabled = False
            groupConnService.Enabled = False
            btnSave.Enabled = False
        Else
            groupWebService.Enabled = True
            groupConnService.Enabled = True
            btnSave.Enabled = True
        End If
    End Sub


#Region "Common Form"
    Private Sub frmSettings_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        LoadSetting()
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        If SaveSetting() Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Dispose()
    End Sub

    Private Sub txtServicePath_MouseHover(sender As Object, e As System.EventArgs) Handles txtServicePath.MouseHover
        _ToolTip.Show(txtServicePath.Text, txtServicePath)
    End Sub
#End Region


#Region "Web Service Settings"
    Private Sub FileDialog_Click(sender As System.Object, e As System.EventArgs) Handles FileDialog.Click
        Dim fd As OpenFileDialog = OpenFileDialogConf()
        fd.InitialDirectory = Path.GetDirectoryName(IIf(txtServicePath.Text = "", Reflection.Assembly.GetExecutingAssembly().Location, txtServicePath.Text))
        If fd.ShowDialog() = DialogResult.OK Then
            txtServicePath.Text = fd.FileName
            GetConfigOfService()
        End If
    End Sub
#End Region


#Region "Connection Service Settings"
    Private Sub FileDialog2_Click(sender As System.Object, e As System.EventArgs) Handles FileDialog2.Click
        Dim fd As OpenFileDialog = OpenFileDialogConf()
        fd.InitialDirectory = Path.GetDirectoryName(IIf(txtFileDB.Text = "", Reflection.Assembly.GetExecutingAssembly().Location, txtFileDB.Text))
        fd.Filter = "Ms. Access 2000-2003 or 2007 (*.mdb;*accdb)|*.mdb;*accdb"
        If fd.ShowDialog() = DialogResult.OK Then
            txtFileDB.Text = fd.FileName
        End If
    End Sub

    Private Sub txtTimeOut_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtTimeOut.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled = True
    End Sub
#End Region
End Class

Public Class StatusMessage
    Property Status As Boolean = False
    Property Message As String
End Class
