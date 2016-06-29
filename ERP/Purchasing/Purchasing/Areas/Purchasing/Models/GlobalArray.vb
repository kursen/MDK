Public Class GlobalArray
    Public Shared ReadOnly Property PurchaseRequisitionDocState As String()
        Get
            Return {"Draft", "Pengajuan", "Disetujui", "Ditolak", "Proses Pembelian", "Pembelian Selesai"}
        End Get
    End Property

    Public Shared ReadOnly Property PurchaseOrderDocState As String()
        Get
            Return {"Draft", "Proses Pembelian", "Pembelian Selesai"}
        End Get
    End Property
End Class
