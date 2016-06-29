Public Class EmployeeOvertimeView
    Inherits EmployeeOvertime
    Property EmployeeName As String

    Public ReadOnly Property TotalOvertime1 As Double
        Get
            Dim nOvertime As Double = 0.0
            If Me.BeginOvertime1Afternoon.HasValue AndAlso Me.EndOvertime1Afternoon.HasValue Then
                nOvertime += DateDiff(DateInterval.Minute, BeginOvertime1Afternoon.Value, EndOvertime1Afternoon.Value)
            End If
            If Me.BeginOvertime1Morning.HasValue AndAlso Me.EndOvertime1Morning.HasValue Then
                nOvertime += DateDiff(DateInterval.Minute, BeginOvertime1Morning.Value, EndOvertime1Morning.Value)
            End If
            If nOvertime > 0 Then
                nOvertime = nOvertime / 60
            End If
            Return nOvertime
        End Get
    End Property
    Public ReadOnly Property TotalOvertime2 As Double
        Get
            Dim nOvertime As Double = 0.0
            If Me.BeginOvertime2.HasValue AndAlso Me.EndOvertime2.HasValue Then
                nOvertime += DateDiff(DateInterval.Minute, BeginOvertime2.Value, EndOvertime2.Value)
            End If
            If nOvertime > 0 Then
                nOvertime = nOvertime / 60
            End If
            Return nOvertime
        End Get

    End Property
End Class
