<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.groupWebService = New System.Windows.Forms.GroupBox()
        Me.txtWebServiceUrl = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.groupConnService = New System.Windows.Forms.GroupBox()
        Me.txtTimeOut = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.FileDialog2 = New System.Windows.Forms.Button()
        Me.txtConnectionString = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtFileDB = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.FileDialog = New System.Windows.Forms.Button()
        Me.txtServicePath = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LabelHint = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.groupWebService.SuspendLayout()
        Me.groupConnService.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(177, 322)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(84, 27)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "&Terapkan"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(264, 322)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(84, 27)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "&Batal"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'groupWebService
        '
        Me.groupWebService.Controls.Add(Me.txtWebServiceUrl)
        Me.groupWebService.Controls.Add(Me.Label1)
        Me.groupWebService.Location = New System.Drawing.Point(12, 81)
        Me.groupWebService.Name = "groupWebService"
        Me.groupWebService.Size = New System.Drawing.Size(347, 74)
        Me.groupWebService.TabIndex = 4
        Me.groupWebService.TabStop = False
        Me.groupWebService.Text = "Web Service"
        '
        'txtWebServiceUrl
        '
        Me.txtWebServiceUrl.Location = New System.Drawing.Point(101, 28)
        Me.txtWebServiceUrl.Name = "txtWebServiceUrl"
        Me.txtWebServiceUrl.Size = New System.Drawing.Size(217, 20)
        Me.txtWebServiceUrl.TabIndex = 5
        Me.txtWebServiceUrl.Text = "http://"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Alamat URL"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'groupConnService
        '
        Me.groupConnService.Controls.Add(Me.txtTimeOut)
        Me.groupConnService.Controls.Add(Me.Label5)
        Me.groupConnService.Controls.Add(Me.FileDialog2)
        Me.groupConnService.Controls.Add(Me.txtConnectionString)
        Me.groupConnService.Controls.Add(Me.Label3)
        Me.groupConnService.Controls.Add(Me.txtFileDB)
        Me.groupConnService.Controls.Add(Me.Label4)
        Me.groupConnService.Location = New System.Drawing.Point(12, 170)
        Me.groupConnService.Name = "groupConnService"
        Me.groupConnService.Size = New System.Drawing.Size(347, 142)
        Me.groupConnService.TabIndex = 5
        Me.groupConnService.TabStop = False
        Me.groupConnService.Text = "Koneksi Service"
        '
        'txtTimeOut
        '
        Me.txtTimeOut.Location = New System.Drawing.Point(141, 97)
        Me.txtTimeOut.Name = "txtTimeOut"
        Me.txtTimeOut.Size = New System.Drawing.Size(77, 20)
        Me.txtTimeOut.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(19, 100)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(112, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Waktu Tunda Service"
        '
        'FileDialog2
        '
        Me.FileDialog2.Location = New System.Drawing.Point(292, 26)
        Me.FileDialog2.Name = "FileDialog2"
        Me.FileDialog2.Size = New System.Drawing.Size(26, 23)
        Me.FileDialog2.TabIndex = 8
        Me.FileDialog2.Text = "..."
        Me.FileDialog2.UseVisualStyleBackColor = True
        '
        'txtConnectionString
        '
        Me.txtConnectionString.Location = New System.Drawing.Point(140, 64)
        Me.txtConnectionString.Name = "txtConnectionString"
        Me.txtConnectionString.Size = New System.Drawing.Size(178, 20)
        Me.txtConnectionString.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(18, 67)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "String Koneksi"
        '
        'txtFileDB
        '
        Me.txtFileDB.Location = New System.Drawing.Point(140, 28)
        Me.txtFileDB.Name = "txtFileDB"
        Me.txtFileDB.Size = New System.Drawing.Size(146, 20)
        Me.txtFileDB.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(18, 31)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(116, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Lokasi Database Lokal"
        '
        'FileDialog
        '
        Me.FileDialog.Location = New System.Drawing.Point(295, 32)
        Me.FileDialog.Name = "FileDialog"
        Me.FileDialog.Size = New System.Drawing.Size(63, 23)
        Me.FileDialog.TabIndex = 11
        Me.FileDialog.Text = "Cari"
        Me.FileDialog.UseVisualStyleBackColor = True
        '
        'txtServicePath
        '
        Me.txtServicePath.BackColor = System.Drawing.SystemColors.Control
        Me.txtServicePath.Enabled = False
        Me.txtServicePath.Location = New System.Drawing.Point(88, 34)
        Me.txtServicePath.Name = "txtServicePath"
        Me.txtServicePath.Size = New System.Drawing.Size(201, 20)
        Me.txtServicePath.TabIndex = 10
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Lokasi Service"
        '
        'LabelHint
        '
        Me.LabelHint.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelHint.ForeColor = System.Drawing.Color.Red
        Me.LabelHint.Location = New System.Drawing.Point(89, 54)
        Me.LabelHint.Name = "LabelHint"
        Me.LabelHint.Size = New System.Drawing.Size(200, 33)
        Me.LabelHint.TabIndex = 12
        Me.LabelHint.Text = "Label"
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(370, 361)
        Me.Controls.Add(Me.LabelHint)
        Me.Controls.Add(Me.FileDialog)
        Me.Controls.Add(Me.txtServicePath)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.groupConnService)
        Me.Controls.Add(Me.groupWebService)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSettings"
        Me.ShowInTaskbar = False
        Me.Text = "Konfigurasi"
        Me.groupWebService.ResumeLayout(False)
        Me.groupWebService.PerformLayout()
        Me.groupConnService.ResumeLayout(False)
        Me.groupConnService.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents groupWebService As System.Windows.Forms.GroupBox
    Friend WithEvents txtWebServiceUrl As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents groupConnService As System.Windows.Forms.GroupBox
    Friend WithEvents txtTimeOut As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents FileDialog2 As System.Windows.Forms.Button
    Friend WithEvents txtConnectionString As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtFileDB As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents FileDialog As System.Windows.Forms.Button
    Friend WithEvents txtServicePath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LabelHint As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
