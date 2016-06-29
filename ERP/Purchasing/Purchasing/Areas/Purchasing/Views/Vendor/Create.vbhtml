@ModelType Purchasing.Vendor
@Code
    ViewData("Title") = "Vendor"

End Code

@Functions
    Function CreateItemInput() As MvcHtmlString
        Dim openDiv = New MvcHtmlString("<div class='row'><div class='col-lg-4 col-sm-6 col-xs-6' style='padding-right: 0px;'>")
        Dim input1 = Html.TextBoxFor(Function(model) model.City, New With {.class = "form-control", .placeholder = "Kota"})
        Dim sp1 = New MvcHtmlString("</div><div class='col-lg-4 col-sm-6 col-xs-6'>")
        Dim input2 = Html.TextBoxFor(Function(model) model.Province, New With {.class = "form-control", .placeholder = "Provinsi"})
        Dim sp2 = New MvcHtmlString("</div><div class='col-lg-8 col-sm-12 col-xs-12'>")
        Dim input3 = Html.TextBoxFor(Function(model) model.Country, New With {.class = "form-control", .placeholder = "Negara", .Value = "Indonesia"})
        Dim closeDiv = New MvcHtmlString("</div></div>")
        Return HaloUIHelpers.Helpers.Helpers.Concat(openDiv, input1, sp1, input2, sp2, input3, closeDiv)
    End Function
    
    
    Function CreateCategorySelect() As MvcHtmlString
        Dim openDiv = New MvcHtmlString("<div class='row'><div class='col-lg-9 col-sm-9'>")
        Dim input1 = Html.DropDownList("CategoryId", Nothing, New With {.class = "form-control"})
        Dim sp1 = New MvcHtmlString("</div><label class='col-lg-1 col-sm-1'>")
        Dim button = New MvcHtmlString("<a href='#' class='btn btn-default' id='addCategory'><span class='fa fa-edit'></span> Edit</a>")
        Dim closeDiv = New MvcHtmlString("</label></div>")
        Return HaloUIHelpers.Helpers.Helpers.Concat(openDiv, input1, sp1, button, closeDiv)
    End Function
End Functions

@Html.Partial("~/Areas/Purchasing/Views/VendorCategory/Index.vbhtml")

@Using Html.BeginForm("Create", "Vendor", Nothing, FormMethod.Post, New With {.enctype = "multipart/form-data", .class = "form form-horizontal", .id = "form-data", .autocomplete = "off"})
    Using Html.BeginJUIBox("Data Form Vendor")
    @<div class="row">
        
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Number, New With {.class = "form-control", .Value = ViewData("QuoNumber")}), "Nomor", lgControlWidth:=2, smControlWidth:=2)
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Name, New With {.class = "form-control"}), "Nama")
        @Html.WriteFormInput(CreateCategorySelect(), "Kategori")
        @Html.WriteFormInput(Html.TextAreaFor(Function(model) model.Address, New With {.class = "form-control"}), "Alamat")

        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.NPWPNumber, New With {.class = "form-control"}), "Serial NPWP")
        @Html.WriteFormInput(Html.TextAreaFor(Function(model) model.NPWPAddress, New With {.class = "form-control"}), "Alamat NPWP")
        <br />
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Phone, New With {.class = "form-control"}), "No. Telp")
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.ContactName, New With {.class = "form-control"}), "Nama Kontak")
        <hr />
        @Html.WriteFormInput(CreateItemInput, "Domisili", lgControlWidth:=6)
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Zip, New With {.class = "form-control numeric2"}), "Kode Pos", smControlWidth:=2, lgControlWidth:=2)
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Fax, New With {.class = "form-control numeric2"}), "Fax", smControlWidth:=2, lgControlWidth:=2)
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Email, New With {.class = "form-control", .type = "email"}), "E-Mail")
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Web, New With {.class = "form-control"}), "Website")
        @Html.HiddenFor(Function(model) model.Currency, New With {.class = "form-control", .style = "text-transform: uppercase", .Value = "IDR"})
        <hr />
        @Html.WriteFormInput(Html.DropDownList("Suspended",nothing, New With {.class = "form-control"}), "Suspended", smControlWidth:=2, lgControlWidth:=2)
        @Html.HiddenFor(Function(model) model.Suspended, New With {.Value = "Active"})
    </div>
    @<div class="row">
        <div class="col-lg-offset-5 col-sm-offset-5">
            <button class="btn btn-primary btn-label-left" type="submit" id="btnSave">
                <span><i class="fa fa-save"></i></span>Simpan</button>
            @Html.ActionLink("Batal", "Index", "Vendor", Nothing, New With {.class = "btn btn-default"})
        </div>
    </div>
    End Using
End Using
<!-- end form-->

<script src="@Url.Content("~/plugins/jquery-number/jquery.numeric.js")" type="text/javascript"></script>
<script type="text/javascript">
    submitFormCallback = function (data) {
        if (data.stat == 0) {
            showNotificationSaveError(data);
            return
        } else if (data.stat == 1) {
            window.location = "/Purchasing/Vendor/Detail/" + data.id;
        } else {
            window.location = "/Purchasing/Vendor/Index";
        }
    }

    $(document).ready(function () {
        $('.numeric').numeric({ decimal: ","});
        $('.numeric2').numeric({ decimal: false, negative: false });

        //form submit
        $("#form-data").submit(function (e) {
            e.preventDefault();
            showSavingNotification();
            var _data = $(this).serialize();
            var _url = $(this).attr('action');
            $.ajax({
                type: 'POST',
                url: _url,
                data: _data,
                success: submitFormCallback
            });
        });
    });

    //show form category
    $('#addCategory').click(function () {
        $('#CategoryForm').slideDown();
        $('#form-data').find('input, select, textarea, button, a').attr("disabled", "disabled");
    });

    $('#btnCancelCategory').click(function () {
        $('#CategoryForm').slideUp();
        $('#form-data').find('input, select, textarea, button, a').removeAttr("disabled");
    });
   
</script>
