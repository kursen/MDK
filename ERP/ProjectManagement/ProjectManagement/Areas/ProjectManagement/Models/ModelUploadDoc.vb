Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization


<MetadataType(GetType(ModelUploadDoc))>
Public Class ProjectDocDetail
End Class

Public Class ModelUploadDoc
    <Required(ErrorMessage:="Judul Dokumen wajib diisi.")> _
    Public Property DocTitle As String

    <Required(ErrorMessage:="Dokumen wajib dipilih.")> _
    Public Property FileName As String
End Class
