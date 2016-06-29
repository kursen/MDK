Public Class EmployeeSearchResult
    Property Id As Integer
    Property EmployeeId As String
    Property Fullname As String
    Property OccupationName As String
    Property OfficePath As String
    Property PathId As String
    ReadOnly Property OfficeId As Integer
        Get
            If String.IsNullOrWhiteSpace(Me.PathId) = False Then
                Return CInt(PathId.Split("-"c).First())
            End If
            Return 0
        End Get
    End Property
End Class
