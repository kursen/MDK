@ModelType Purchasing.DocumentSearchResult()
@Code
    ViewData("Title") = "Pencarian PO"
    
    Dim term = Request.QueryString("term")
   
    Dim groupped = From m In Model
            Group By m.DocDate, m.DocNumber, m.DocumentId, m.Archive, m.Vendor_CompanyName Into g = Group

    
End Code
@Using Html.BeginJUIBox("Pencarian", iconClass:="fa fa-search")
    
    @<div class="row">
        <div class="col-lg-12-col-sm-12">
            @Using Html.BeginForm("Search", "PurchaseOrder", Nothing, FormMethod.Get, New With {.class = "form-inline", .autocomplete = "off"})
            
                @Html.WriteFormInput(Html.TextBox("term", term,
                                                  New With {.class = "form-control"}), "Cari No PO / Nama Item / Nama Vendor", lgControlWidth:=6, lgLabelWidth:=6)
                @Html.WriteFormInput(New MvcHtmlString("<button class='btn btn-primary'>Cari</button>"), "", lgControlWidth:=6, lgLabelWidth:=1)
            
    End Using
        </div>
    </div>
    
End Using
@If Model IsNot Nothing AndAlso term IsNot Nothing Then
     
    Using Html.BeginJUIBox("Hasil Pencarian", iconClass:="fa fa-list")
        
       
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            @If Model.Count > 0 Then
            Dim mItemname As String = ""
            Dim mDocNumber As String = ""
            For Each item In groupped
                Dim startpos, endpost As Integer
                startpos = item.DocNumber.ToUpper().IndexOf(term.ToUpper())
                endpost = startpos + term.Length
                mDocNumber = item.DocNumber
                If startpos >= 0 AndAlso endpost > 0 Then
                    mDocNumber = item.DocNumber.Insert(endpost, "</span>").Insert(startpos, "<span class='searchresult'>")
                End If
    
                            

                @<a href="@Url.Action("Detail", New With {.id = item.DocumentId})">@Html.Raw(mDocNumber)</a>
                @Html.Raw(String.Concat(", Tanggal: ", item.DocDate.ToString("dd-MM-yyyy"), ", Nama Vendor: ", item.Vendor_CompanyName, ", Status Dokumen: ", IIf(item.Archive, "Arsip", "Aktif")))
                Dim lsItem As New List(Of String)
                For Each gItem In item.g
                    If String.IsNullOrEmpty(gItem.ItemName) = False Then
                        startpos = gItem.ItemName.ToUpper().IndexOf(term.ToUpper())
                    
                    
                        If startpos >= 0 Then
                        
                            endpost = startpos + term.Length
                            lsItem.Add(gItem.ItemName.Insert(endpost, "</span>").Insert(startpos, "<span class='searchresult'>"))
                        Else
                            lsItem.Add(gItem.ItemName)
                        End If
    
               
                    Else
                @<br />
                    End If
        Next
        If lsItem.Count > 0 Then
            @<ul>
            @For Each itemname In lsItem
             @Html.Raw("<li>" & itemname & "</li>")
            Next
            </ul>
        End If
            
            
                @<br />
            Next
        Else
            If term IsNot Nothing Then
                @<p>
                    Tidak terdapat hasil pencarian.</p>
            End If
                
        End If
        </div>
    </div>
    End Using


End If
<style>
    .searchresult
    {
        background-color: Yellow;
    }
</style>
