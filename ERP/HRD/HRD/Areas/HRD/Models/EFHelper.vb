Imports System.Data.Objects
Imports System.Data.SqlClient
Public Class EFHelper
    Public Shared Function ExecuteDataTable(ByVal ef As ObjectContext, ByVal Query As String, ByVal parameters As IEnumerable(Of SqlParameter)) As DataTable
        Dim dt As New DataTable
        Using connection = CType(ef.Connection, EntityClient.EntityConnection).StoreConnection
            connection.Open()
            Using cmd = connection.CreateCommand()
                cmd.CommandText = Query
                cmd.Parameters.AddRange(parameters)

                Dim reader = cmd.ExecuteReader()
                dt.Load(reader)
                reader.Close()
            End Using
        End Using
        
        Return dt
    End Function
End Class
