Imports System
Imports System.ServiceProcess
Imports System.Diagnostics
Imports System.Threading
Imports System.Windows.Forms
Imports System.Security.Principal

Public Class frmMain

    Public svcStatus As String
    Public serviceName As String
    Public myService As ServiceController

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
    End Sub

    Private Sub syncServiceGUI_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            serviceName = Configuration.ConfigurationManager.AppSettings("ServiceName")
            myService = New ServiceController(serviceName)

            myService.ServiceName = serviceName
            svcStatus = myService.Status.ToString()

            Label1.Text = IIf(svcStatus = "Running", "Sedang Berjalan", "Nonaktif")
            barrProgress.Hide()

            'cek service is runnning
            If myService.Status = ServiceControllerStatus.Stopped Then
                'btnStart_Click(sender, e)
                btnStart.Text = "Start"
            Else
                btnStart.Text = "Stop"
            End If
        Catch ex As Exception
            MessageBox.Show("Aplikasi tidak dapat berjalan dengan baik." +
                            " Mohon periksa kembali kelengkapan atau kesalahan yang mungkin terjadi." +
                            " Hubungi Administrator untuk info lebih lanjut" + vbCrLf + vbCrLf +
                            "Error Message : " + ex.Message, _
                            "Pesan Error", Nothing, MessageBoxIcon.Warning)
            Me.Close()
        End Try
    End Sub


    Private Sub btnStart_Click(sender As System.Object, e As System.EventArgs) Handles btnStart.Click
        barrProgress.Minimum = 0
        barrProgress.Maximum = 100
        barrProgress.Step = 1

        barrProgress.Show()
        barrProgress.PerformStep()
        Timer1.Start()
        btnStart.Enabled = False
        Label1.Text = "Sistem mencoba untuk memulai aplikasi service di komputer ini ..."
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        barrProgress.Increment(1)
        Timer1.Interval = (10)

        If barrProgress.Value = 90 Then
            Timer1.Stop()

            Dim svcStatusWas As String = ""
            Dim countHit As Byte = 0

            Try
                Dim identity = WindowsIdentity.GetCurrent()
                Dim principal = New WindowsPrincipal(identity)
                Dim isElevated As Boolean = principal.IsInRole(WindowsBuiltInRole.Administrator)
                If (isElevated) Then
                    If (svcStatus = "Stopped") Then
                        myService.Start()   ' START the service if it is already Stopped

                        While Not svcStatus = "Running" AndAlso countHit <= 7
                            myService.Refresh()
                            svcStatus = myService.Status.ToString()
                            Thread.Sleep(2000)
                            countHit += 1
                        End While
                    ElseIf (svcStatus = "Running") Then
                        myService.Stop()   ' STOP the service if it is already Started

                        While Not svcStatus = "Stopped" AndAlso countHit <= 7
                            myService.Refresh()
                            svcStatus = myService.Status.ToString()
                            Thread.Sleep(2000)
                            countHit += 1
                        End While
                    End If

                    If countHit >= 6 Then
                        MsgBox("Proses berjalan terlalu lama, mohon tunggu beberapa saat lalu coba kembali." + vbCrLf + vbCrLf + "Bila terus berlanjut, mohon hubungi Administrator", MsgBoxStyle.Exclamation)
                    End If
                Else
                    MsgBox("Aplikasi tidak berjalan dengan Hak Akses Administrator. Maaf aplikasi harus dihentikan", MsgBoxStyle.Information)
                    Me.Close()
                End If

            Catch ex As Exception
                MessageBox.Show("Aplikasi belum berjalan dengan baik. " +
                                "Mohon tunggu beberapa saat lalu coba ulangi kembali." + vbCrLf +
                                "jika terus berlanjut, mohon hubungi Administrator" + vbCrLf + vbCrLf +
                                "Error Message : " + ex.Message, _
                                "Pesan Error", Nothing, MessageBoxIcon.Warning)
            End Try

            'If (svcStatus = svcStatusWas AndAlso svcStatus = "Running") Then
            If svcStatus = "Running" Then
                'MsgBox("Status: " + svcStatus)
                barrProgress.Value = 100
                barrProgress.Value = 0
                btnStart.Text = "Stop"
            Else
                btnStart.Text = "Start"
            End If
            'End If

            barrProgress.Value = 0
            barrProgress.Hide()
            btnStart.Enabled = True
            Label1.Text = IIf(svcStatus = "Running", "Sedang Berjalan", "Tidak Aktif")
        End If
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SettingsToolStripMenuItem.Click
        frmSettings.ShowDialog()
    End Sub
End Class