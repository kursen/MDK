Imports System
Imports System.ServiceProcess
Imports System.Diagnostics
Imports System.Threading

Public Class MainForm
    Dim sync As ServiceController = New ServiceController("syncservice")
    Dim increment As Integer = 0

    Private Sub btnStart_Click(sender As System.Object, e As System.EventArgs) Handles btnStart.Click
        Try
            sync.Start()
            barrProgress.PerformStep()
            Timer1.Start()
            Label1.Text = "Running"
            btnStart.Enabled = False
            stopBtn.Enabled = True
        Catch ex As Exception
            MessageBox.Show("Error " + ex.Message, "Error", Nothing, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub stopBtn_Click(sender As System.Object, e As System.EventArgs) Handles stopBtn.Click
        Try
            sync.Stop()
            barrProgress.PerformStep()
            Timer1.Start()
            Label1.Text = "Stopped"
            btnStart.Enabled = True
            stopBtn.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message + "Service is " + sync.Status.ToString, "Error", Nothing, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        barrProgress.Increment(1)
        Timer1.Interval = (10)
        If barrProgress.Value = 100 Then
            Timer1.Stop()
            barrProgress.Value = 0
        End If
    End Sub

    Private Sub MainForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        barrProgress.Minimum = 0
        barrProgress.Maximum = 100
        barrProgress.Step = 1
        Label1.Text = ""
        'cek service is runnning
        If sync.Status = ServiceControllerStatus.Running Then
            stopBtn.Enabled = True
            btnStart.Enabled = False
            Label1.Text = "Running"
        Else
            stopBtn.Enabled = False
            btnStart.Enabled = True
            Label1.Text = "Stopped"
        End If
    End Sub
End Class
