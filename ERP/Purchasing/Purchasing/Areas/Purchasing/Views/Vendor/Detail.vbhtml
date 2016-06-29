@ModelType Purchasing.Vendor
@Code
    ViewData("Title") = "Vendor"
    Html.SetEditableDefaultSettings(DisableOnload:=True)
End Code

@functions
    Function WriteEditableSelect(ByVal value As String, ByVal datapk As Integer) As MvcHtmlString
        'Dim strHtml = String.Format("<a id='CategoryId' href='#' data-pk='{0}' " &
        '                            "data-type='select' data-title='Kategori' data-url='/Purchasing/Vendor/SavePartial'>{1}</a>", datapk, value)
        
        
        Dim strHtml = String.Format("<a tabindex='-1' class='editable editable-click editable-disabled' data-pk='{0}' data-title='Kategori' data-type='select' data-url='/Vendor/SavePartial' id='CategoryId'>{1}</a>", datapk, value)
        Return New MvcHtmlString(strHtml)
        
    End Function
End Functions

@Using Html.BeginJUIBox("Detail Vendor", True, False, False, False, False, "fa fa-table")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
           <div class="pull-right">
                <button type="button" class="btn btn-danger btn-label-left" onclick="$('.detailData .editable') .editable('toggleDisabled')">
                <span><i class="fa fa-edit"></i></span>Edit</button>
                <a href="@Url.Action("Index", "Vendor")" class="btn btn-default btn-label-left">
                    Kembali</a>
            </div>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-6 col-sm-6">
            <dl class="dl-horizontal detailData">
                <dt>Nomor</dt>
                <dd>@Html.EditableInputTextBox("Number", Model.Number, "text", "Nomor", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt>Nama</dt>
                <dd>@Html.EditableInputTextBox("Name", Model.Name, "text", "Nama", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt>Alamat</dt>
                <dd>@Html.EditableInputTextBox("Address", Model.Address, "text", "Alamat", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt>Kode Pos</dt>
                <dd>@Html.EditableInputTextBox("Zip", Model.Zip, "text", "title", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt>Kota</dt>
                <dd>@Html.EditableInputTextBox("City", Model.City, "text", "Kota", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt>Provinsi</dt>
                <dd>@Html.EditableInputTextBox("Province", Model.Province, "text", "Provinsi", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt>Negara</dt>
                <dd>@Html.EditableInputTextBox("Country", Model.Country, "text", "Negara", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt></dt><dd>&nbsp;</dd>
                <dt>Serial NPWP</dt>
                <dd>@Html.EditableInputTextBox("NPWPNumber", Model.NPWPNumber, "text", "Serial NPWP", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt>Alamat NPWP</dt>
                <dd>@Html.EditableInputTextBox("NPWPAddress", Model.NPWPAddress, "text", "Alamat NPWP", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
            </dl>
        </div>
        <div class="col-lg-6 col-sm-6">
            <dl class="dl-horizontal detailData">
                <dt>Nama Kontak</dt>
                <dd>@Html.EditableInputTextBox("ContactName", Model.ContactName, "text", "Nama Kontak", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt>No. Telp</dt>
                <dd>@Html.EditableInputTextBox("Phone", Model.Phone, "text", "No. Telp", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt></dt><dd>&nbsp;</dd>
                <dt>Fax</dt>
                <dd>@Html.EditableInputTextBox("Fax", Model.Fax, "text", "Fax", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt>Email</dt>
                <dd>@Html.EditableInputTextBox("Email", Model.Email, "text", "Email", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt>Web</dt>
                <dd>@Html.EditableInputTextBox("Web", Model.Web, "text", "Web", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt>Currency</dt>
                <dd>@Html.EditableInputTextBox("Currency", Model.Currency, "text", "Currency", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt>Suspended</dt>
                <dd>@Html.EditableInputTextBox("Suspended", Model.Suspended, "text", "Suspended", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                    &nbsp;</dd>
                <dt>Category</dt>
                <dd>@WriteEditableSelect(ViewData("CategoryId"), Model.Id)
                
                
                @*@Html.EditableInputTextBox("Category", Model.CategoryId, "text", "Category", datapk:=Model.Id, dataurl:="/Vendor/SavePartial")
                *@    &nbsp;</dd>
            </dl>
        </div>
        <div class="col-lg-12 col-sm-12">
            <dl class="dl-horizontal detailData">
            </dl>
        </div>
    </div>
    @<br />
End Using
<script type="text/javascript">

    var _savepartialresponse = function (data) {

        if ((data) && (data.stat == 1)) {

        } else {
            showNotification(data);
        };

    }

    $(function () {
        $.ajax({
            url: '/Purchasing/Vendor/GetVendorCategoryList',
            type: 'POST',
            success: function (data) {
                $('#CategoryId').editable({
                    source: data,
                    success: function () {
                        showNotification("Data tersimpan.");
                    }
                });
            },
            error: ajax_error_callback,
            datatype: 'json'
        });
    });
    </script>

