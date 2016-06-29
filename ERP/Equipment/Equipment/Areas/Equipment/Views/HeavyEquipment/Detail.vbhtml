﻿@ModelType Equipment.HeavyEqpView
@Code
    ViewData("Title") = "Detail Kendaraan"
    Html.SetEditableDefaultSettings(DisableOnload:=True)
    Dim roles = "Equipment.Supervisor, Equipment.Manager, Equipment.DataOperator".Split(",")
    Dim isProperUser = ERPBase.ErpAuthorization.UserInAnyRoles(roles, User)
  
        
           
    
    
End Code
@functions
    Function WriteDateValue(ByVal d As DateTime?) As String
        If d.HasValue Then
            Return d.Value.ToString("dd-MM-yyyy")
        Else
            Return ""
        End If
        
    End Function
    Function WriteEditableSelect(ByVal value As String, ByVal datapk As Integer) As MvcHtmlString
        Dim strHtml = String.Format("<a id='IDArea' href='#' id='IDArea' data-pk='{0}' " &
                                    "data-type='select' data-title='Kantor' data-url='/HeavyEquipment/SavePartial'>{1}</a>", datapk, value)
        Return New MvcHtmlString(strHtml)
        
    End Function
  
        
    
End Functions
@Using Html.BeginJUIBox("Detail Kendaraan", True, False, False, False, False, "fa fa-table")
    
    @<div class="row">
        <div class="col-sm-12 col-lg-12">
            <div class="well text-right">
            @If isProperUser Then
                @<button type="button" class="btn btn-info btn-label-left" id="btnChangePicture" data-toggle="modal"
                    data-target="#dlgChangePicture">
                    <span><i class="fa fa-plus-square"></i></span>Ganti Gambar</button>
                @<button type="button" class="btn btn-danger btn-label-left" onclick="$('#dProjectInfo .editable') .editable('toggleDisabled')">
                    <span><i class="fa fa-edit"></i></span>Edit</button>    
                End If
                
                <a href="@Url.Action("Index", "HeavyEquipment")" class="btn btn-default btn-label-left">Kembali</a>
            </div>
        </div>
    </div>
  
    @<div class="row">
        <div class="col-lg-5 col-sm-5">
            <div style="width: 80%" class="center-block">
                <a class="fancybox" href="@Url.Action("HeavyEqpPicture", New With {.id = Model.ID})#" title="Gambar Alat Berat" >
                    <img src="@Url.Action("HeavyEqpPicture", New With {.id = Model.ID})"  class="img-responsive img-thumbnail " id="showimg" alt="img" />
                </a>
            </div>
        </div>
        <div class="col-lg-7 col-sm-7">
            <div class="form-horizontal" id='dProjectInfo'>
            
                @Html.WriteFormInput(Html.EditableInputTextBox("Code", Model.Code, "text", "Kode", datapk:=Model.ID, dataurl:="/HeavyEquipment/SavePartial"),
                                     "Kode Kendaraan")
                @Html.WriteFormInput(Html.EditableInputTextBox("Species", Model.Species, "text", "Jenis", datapk:=Model.ID,
                                                               dataurl:="/HeavyEquipment/SavePartial"), "Jenis")

                @Html.WriteFormInput(WriteEditableSelect(Model.OfficeName, Model.ID), "Wilayah Kerja")
                <hr />
                @Html.WriteFormInput(Html.EditableInputTextBox("Merk", Model.Merk, "text", "Merk", datapk:=Model.ID,
                                                               dataurl:="/HeavyEquipment/SavePartial"), "Merk")
                @Html.WriteFormInput(Html.EditableInputTextBox("Type", Model.Type, "text", "Tipe", datapk:=Model.ID,
                                                               dataurl:="/HeavyEquipment/SavePartial"), "Tipe")
                @Html.WriteFormInput(Html.EditableInputTextBox("Cost", Model.Cost, "text", "Harga", datapk:=Model.ID,
                                                               dataurl:="/HeavyEquipment/SavePartial"), "Harga Perolehan")
                @Html.WriteFormInput(Html.EditableInputTextBox("Year", Model.Year, "text", "Tahun Beli", datapk:=Model.ID,
                                                               dataurl:="/HeavyEquipment/SavePartial"), "Tahun Pembelian")
                @Html.WriteFormInput(Html.EditableInputTextBox("BuiltYear", Model.BuiltYear, "text", "Tahun Pembuatan", datapk:=Model.ID,
                                                               dataurl:="/HeavyEquipment/SavePartial"), "Tahun Pembuatan")
                @Html.WriteFormInput(Html.EditableInputTextBox("Capacity", Model.Capacity, "text", "Kapasitas", datapk:=Model.ID,
                                                               dataurl:="/HeavyEquipment/SavePartial"), "Kapasitas")

                @Html.WriteFormInput(Html.EditableInputTextBox("SerialNumber", Model.SerialNumber, "text", "Nomor Seri",
                                                               datapk:=Model.ID, dataurl:="/HeavyEquipment/SavePartial"), "No. Seri")
                <hr />
                       @Html.WriteFormInput(Html.EditableInputTextBox("TaxNumber", Model.TaxNumber, "text", "No Register Pajak", datapk:=Model.ID,
                                                               dataurl:="/HeavyEquipment/SavePartial"), "No Register Pajak")
                @Html.WriteFormInput(Html.EditableInputTextBox("DueDate", WriteDateValue(Model.DueDate), "date", "Jatuh Tempo", datapk:=Model.ID,
                                                               dataurl:="/HeavyEquipment/SavePartial"), "Tgl Jatuh Tempo Pajak")

                                                              
                @Html.WriteFormInput(Html.EditableInputTextBox("Remarks", Model.Remarks, "text", "Keterangan", datapk:=Model.ID,
                                                               dataurl:="/HeavyEquipment/SavePartial"), "Keterangan")
            </div>
        </div>
    </div>
       
End Using
<div class="modal fade" tabindex="-1" role="dialog" id="dlgChangePicture">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title">
                    Ganti gambar</h4>
            </div>
            <div class="modal-body">
                @Using Html.BeginForm("SaveImage", "HeavyEquipment", Nothing, FormMethod.Post,
                                     New With {.enctype = "multipart/form-data", .class = "form form-horizontal", .id = "form-img", .autocomplete = "off"})
              
                    @<div class="form-group">
                        @Html.HiddenFor(Function(m) m.ID)
                        <label class="col-lg-12 col-sm-12">
                            Pilih File gambar (*.jpg, *.gif, *.png)</label>
                        <div class="col-lg-12 col-sm-12">
                            <input type="file" name="Img" style="visibility: hidden;" id="vehicleImage" /><br />
                            <div class="input-group">
                                <input type="text" name="subfile" id="subfile" class="form-control">
                                <span class="input-group-btn"><a class="btn btn-primary " onclick="$('#vehicleImage').click();"
                                    id="btnBrowse">Browse</a> </span>
                            </div>
                        </div>
                    </div>
                    @<div class="checkbox">
                        <label>
                            <input type="checkbox" name="deleteimage" id="deleteimage">
                            Hapus gambar <i class="fa fa-square-o"></i>
                        </label>
                    </div>
          
                End Using
            </div>
            <div class="modal-footer">
                <button type="reset" class="btn btn-default btn-label-left" data-dismiss="modal"
                    id="btnCancel">
                    <span><i class="fa fa-remove"></i></span>Batal</button>
                <button type="button" class="btn btn-primary" id="btnUploadImage">
                    Upload</button>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
<!-- /.modal -->
<style>
    #dProjectInfo .form-group
    {
        margin-top: 0px;
        margin-bottom: 0px;
    }
</style>
<link href="../../../../plugins/fancybox/jquery.fancybox.css" rel="stylesheet" type="text/css" />
<script src="../../../../plugins/fancybox/jquery.fancybox.pack.js" type="text/javascript"></script>
<script type="text/javascript">
    submitFormCallback = function (data) {
        if (data.stat == 0) {
            showNotificationSaveError(data);
            return
        } else {
            $("#subfile").removeAttr("disabled");
            $("#btnBrowse").removeAttr("disabled");
            $("#form-img")[0].reset();
            $('#dlgChangePicture').modal('hide')
            $('#showimg').attr('src', $('#showimg').attr('src') + '?' + Math.random());
        }
    }

    var _savepartialresponse = function (data) {

        if ((data) && (data.stat == 1)) {

        } else {

            showNotificationSaveError(data, "Penyimpanan data gagal");
            return false;
        };

    }



    $(document).ready(function () {
       

        $.ajax({
            url: '/Equipment/HeavyEquipment/GetOfficeList',
            type: 'POST',
            success: function (data) {

                $('#IDArea').editable({
                    source: data,
                    success: function () {
                        showNotification("Data tersimpan.");
                    }
                });
            },
            error: ajax_error_callback,
            datatype: 'json'
        });


        $("#dlgChangePicture").appendTo("body");

        $('#vehicleImage').change(function () {
            $('#subfile').val($(this).val());
        });
        $("#btnUploadImage").click(function () {
            $("#form-img").submit();

        });

        $("#btnCancel").click(function () {

        });

        $('#dlgChangePicture').on('show.bs.modal', function () {

            $("#deleteimage").prop("checked", false);
            $("#deleteimage").trigger("change");
        })
        $('a.fancybox').fancybox({ type: 'image' });
        //

        $("#deleteimage").change(function () {

            var check = $(this).prop('checked');
            if (check) {

                $("#subfile").attr("disabled", "disabled");
                $("#btnBrowse").attr("disabled", "disabled");
            } else {
                $("#subfile").removeAttr("disabled");
                $("#btnBrowse").removeAttr("disabled");
            }
        });

        $('#form-img').submit(function (e) {
            e.preventDefault();
            var _data = new FormData($(this)[0]);
            var _url = $(this).attr("action");

            $.ajax({
                type: 'POST',
                url: _url,
                data: _data,
                async: false,
                cache: false,
                contentType: false,
                processData: false,
                success: submitFormCallback
            });
        });

        $("#Cost").number(true, 2, ",", ".");




    });
</script>

