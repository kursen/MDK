@ModelType Purchasing.DocumentSearchResult()
@Code
    ViewData("Title") = "Pencarian Permintaan Pembelian"
    
    Dim term = Request.QueryString("term")
    Dim searchOpt = CType(ViewData("SearchOpt"), Boolean)
    
End Code
@Using Html.BeginJUIBox("Pencarian", iconClass:="fa fa-search")
    Using Html.BeginForm("Index", "SearchPurchaseRequisition", Nothing, FormMethod.Get, New With {.class = "form-inline", .autocomplete = "off"})
    @Html.Hidden("SearchOpt",1)
        @<div class="row">
            <div class="col-lg-12-col-sm-12">
                @Html.WriteFormInput(Html.TextBox("term", term,
                                                    New With {.class = "form-control"}), "Cari No Dokumen / Nama Item / Nama Pemohon", lgControlWidth:=5, lgLabelWidth:=7)
                @Html.WriteFormInput(New MvcHtmlString("<button class='btn btn-primary'>Cari</button>"), "", lgControlWidth:=6, lgLabelWidth:=1)
            
            </div>
        </div>
    End Using
End Using
@If Model IsNot Nothing Then
     
    Using Html.BeginJUIBox("Hasil Pencarian", iconClass:="fa fa-list")
        
       
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            @If Model.Count > 0 Then
            For Each item In Model
                Dim startpos, endpost As Integer
                startpos = item.DocNumber.ToUpper().IndexOf(term.ToUpper())
                endpost = startpos + term.Length

                        If startpos >= 0 AndAlso endpost > 0 Then
                            item.DocNumber = item.DocNumber.Insert(endpost, "</span>").Insert(startpos, "<span class='searchresult'>")
                        End If
    
                            

                @<a href="@Url.Action("Detail", IIf(searchOpt, "DepartmentPurchaseRequisition", "ProjectPurchasingRequest"), New With {.id = item.DocumentId})">@Html.Raw(item.DocNumber)</a>
                @Html.Raw(String.Concat(", Tanggal: ", item.DocDate.ToString("dd-MM-yyyy"), ", Permintaan Oleh: ", item.ResBy_Name, ", Status Dokumen: ", IIf(item.Archive, "Arsip", "Aktif")))
                                                                                                                 
                If String.IsNullOrEmpty(item.ItemName) = False Then
                    startpos = item.ItemName.ToUpper().IndexOf(term.ToUpper())
                    
                    
                If startpos >= 0 Then
                        
                    endpost = startpos + term.Length
                    item.ItemName = item.ItemName.Insert(endpost, "</span>").Insert(startpos, "<span class='searchresult'>")
                End If
    
                @<ul>
                    <li>@Html.Raw(item.ItemName)</li>
                </ul>
                Else
                @<br />
                End If
            
                @<br />
            Next
        Else
                @<p>
                    Tidak terdapat hasil pencarian.</p>
        End If
        </div>
    </div>
    End Using
     
End If

<style type="text/css">
.searchresult
{
    background-color:Yellow;
}

</style>