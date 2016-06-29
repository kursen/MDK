@Code
    PageData("ActiveLink") = "/SystemSetting/SecurityCache"
    ViewData("Title") = "SecurityCache"
End Code
<h2>
    SecurityCache</h2>
    <br />
    <br />
@Code
    Dim keys As New List(Of String)
    
    Dim CacheEnum As IDictionaryEnumerator = Cache.GetEnumerator()
    
    While CacheEnum.MoveNext()
        Dim cacheItem = Server.HtmlEncode(CacheEnum.Key.ToString())
        If cacheItem.StartsWith("haloErp") Then
            keys.Add(cacheItem)
        End If
    
   
       
              
    
        
    End While
    
    For Each item In keys
        @Html.Raw(item)
        @<br />
    Next
End Code


@Using Html.BeginForm()
    @<button class="btn btn-danger">Reset Cache</button>
End Using