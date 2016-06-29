Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Public Class PdfMerge
    Public Shared Function Combine(ByVal lsbyte As List(Of Byte())) As Byte()
        Dim doc As New Document()
        Using output As New MemoryStream
            Dim masterpdf = New PdfCopy(doc, output)

            doc.Open()
            Dim docPageCounter As Integer = 0
            For Each item In lsbyte
                Dim reader = New PdfReader(item)
                Dim nPages = reader.NumberOfPages
                For currentPageIndex = 1 To nPages
                    docPageCounter += 1

                    Dim importedPage = masterpdf.GetImportedPage(reader, currentPageIndex)
                    masterpdf.AddPage(importedPage)
                Next
                masterpdf.FreeReader(reader)
                reader.Close()

            Next
            doc.Close()
            Return output.GetBuffer()
        End Using
        Return Nothing
    End Function
End Class
