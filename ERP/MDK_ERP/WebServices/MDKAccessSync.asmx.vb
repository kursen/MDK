Imports System.Web.Services
Imports System.ComponentModel
Imports System.Xml.Serialization

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://halotec-indonesia.com/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class MDKAccessSync
    Inherits System.Web.Services.WebService

    Dim ctx As New ERPEntities
    Dim statMsg As New StatusMessage
    Dim max_ID As Integer

    <WebMethod()> _
    Public Function TestConnection() As String
        Return "Koneksi sukses : " & Server.MachineName
    End Function

    'TblMisc_History
    <WebMethod()> _
    Public Function get_MaxIDHistory() As Integer
        max_ID = (From h In ctx.tblMisc_History Select h.historyID Order By historyID Descending).FirstOrDefault()
        Return max_ID
    End Function
    <WebMethod()> _
    Public Function insert_History(ByVal data As tblMisc_History) As StatusMessage
        Try
            ctx.tblMisc_History.AddObject(data)
            ctx.SaveChanges()
        Catch ex As Exception
            statMsg.Status = False
            statMsg.Message = "Import get Error. Table: tblMisc_History; Message: " + ex.Message
        End Try
        Return New StatusMessage With {.Message = statMsg.Message, .Status = statMsg.Status}
    End Function

    'TblMst_Perusahaan
    <WebMethod()> _
    Public Function get_MaxKodePeru() As Integer
        max_ID = (From peru In ctx.TblMst_Perusahaan Select peru.KodePeru Order By KodePeru Descending).FirstOrDefault()
        Return max_ID
    End Function
    <WebMethod()> _
    Public Function insert_Perusahaan(ByVal data As TblMst_Perusahaan) As StatusMessage
        Try
            ctx.TblMst_Perusahaan.AddObject(data)
            ctx.SaveChanges()
        Catch ex As Exception
            statMsg.Status = False
            statMsg.Message = "Import get Error. Table: TblMst_Perusahaan; Message: " + ex.Message
        End Try
        Return New StatusMessage With {.Message = statMsg.Message, .Status = statMsg.Status}
    End Function

    'TblMst_NoRec
    <WebMethod()> _
    Public Function get_maxNoRec() As Integer
        max_ID = (From r In ctx.TblMst_NoRec Select r.NoUrut Order By NoUrut Descending).FirstOrDefault()
        Return max_ID
    End Function
    <WebMethod()> _
    Public Function insert_NoRec(ByVal data As TblMst_NoRec) As StatusMessage
        Try
            ctx.TblMst_NoRec.AddObject(data)
            ctx.SaveChanges()
        Catch ex As Exception
            statMsg.Status = False
            statMsg.Message = "Import get Error. Table: TblMst_NoRec; Message: " + ex.Message
        End Try
        Return New StatusMessage With {.Message = statMsg.Message, .Status = statMsg.Status}
    End Function

    'TblMst_Barang
    <WebMethod()> _
    Public Function get_KodeBarang() As Integer
        max_ID = (From r In ctx.TblMst_Barang Select r.KodeBrg Order By KodeBrg Descending).FirstOrDefault()
        Return max_ID
    End Function
    <WebMethod()> _
    Public Function insert_Barang(ByVal data As TblMst_Barang) As StatusMessage
        Try
            ctx.TblMst_Barang.AddObject(data)
            ctx.SaveChanges()
        Catch ex As Exception
            statMsg.Status = False
            statMsg.Message = "Import get Error. Table: TblMst_Barang; Message: " + ex.Message
        End Try
        Return New StatusMessage With {.Message = statMsg.Message, .Status = statMsg.Status}
    End Function

    'TblTrans_Penimbangan2
    <WebMethod()> _
    Public Function get_NoPenimbangan2() As Integer
        max_ID = (From r In ctx.TblTrans_Penimbangan2 Order By r.TglMasuk Descending Select r.NoRecord).FirstOrDefault()
        Return max_ID
    End Function
    <WebMethod()> _
    Public Function get_TglMasukPenimbangan2() As DateTime
        Dim max_TglMasuk = (From r In ctx.TblTrans_Penimbangan2 Order By r.TglMasuk Descending Select r.TglMasuk).FirstOrDefault()
        If max_TglMasuk Is Nothing Then
            max_TglMasuk = DateTime.MinValue
        End If
        Return max_TglMasuk
    End Function
    <WebMethod()> _
    Public Function insert_Penimbangan2(ByVal data As TblTrans_Penimbangan2) As StatusMessage
        Try
            ctx.TblTrans_Penimbangan2.AddObject(data)
            ctx.SaveChanges()
        Catch ex As Exception
            statMsg.Status = False
            statMsg.Message = "Import get Error. Table: TblTrans_Penimbangan2; Message: " & ex.Message & IIf(IsNothing(ex.InnerException), "", " InnerException Message:" & ex.InnerException.Message)
        End Try
        Return New StatusMessage With {.Message = statMsg.Message, .Status = statMsg.Status}
    End Function

End Class

<Serializable()> _
Public Class StatusMessage
    Property Status As Boolean = True
    Property Message As String
End Class

