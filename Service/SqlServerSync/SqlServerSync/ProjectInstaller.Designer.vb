<System.ComponentModel.RunInstaller(True)> Partial Class ProjectInstaller
    Inherits System.Configuration.Install.Installer

    'Installer overrides dispose to clean up the component list.
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

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.SyncServiceProcessInstaller = New System.ServiceProcess.ServiceProcessInstaller()
        Me.SyncServiceInstaller = New System.ServiceProcess.ServiceInstaller()
        '
        'SyncServiceProcessInstaller
        '
        Me.SyncServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem
        Me.SyncServiceProcessInstaller.Password = Nothing
        Me.SyncServiceProcessInstaller.Username = Nothing
        '
        'SyncServiceInstaller
        '
        Me.SyncServiceInstaller.Description = "MDK Service"
        Me.SyncServiceInstaller.ServiceName = "SyncService"
        Me.SyncServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic
        '
        'ProjectInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.SyncServiceProcessInstaller, Me.SyncServiceInstaller})

    End Sub
    Friend WithEvents SyncServiceProcessInstaller As System.ServiceProcess.ServiceProcessInstaller
    Friend WithEvents SyncServiceInstaller As System.ServiceProcess.ServiceInstaller

End Class
