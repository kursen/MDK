
    @Using Html.BeginForm("SaveUploadDocDetail", "ProjectInfo", FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmUploadDocDetail", .enctype = "multipart/form-data"})
        @<div class="form-group hidden">
            <div class="">@Html.Hidden("ID")</div>
        </div>
        
        @<div class="form-group hidden">
            <div class="">@Html.hidden("ProjectInfoID", Model.Id)</div>
        </div>
        
        @<div class="form-group">
            <label class="col-lg-3 col-sm-4 control-label">Dokumen</label>
            <div class="col-lg-5 col-sm-5">
                <span class="btn btn-success btn-large fileinput-button"><i class="icon-upload icon-white"></i>
                    <input type="file" multiple="false" id='Files' name="Files" /> Pilih Dokumen
                </span>
                @Html.TextBox("FileName", Nothing, New With {.readOnly = "readOnly", .class = "input-none col-xs-7"})
            </div>
        </div>
    
        @Html.WriteFormInput(Html.TextBox("DocTitle", Nothing, New With {.class = "form-control"}), "Judul Dokumen", smControlWidth:=5, lgControlWidth:=5)
   
        @Html.WriteFormInput(Html.TextArea("Description", New With {.class = "form-control"}), "Deskripsi", smControlWidth:=5, lgControlWidth:=5)
    
        @<div class="col-lg-offset-4 col-sm-offset-2">
            <button type="submit" class="btn btn-primary" id="btnSave"><i class="fa fa-save"></i> 
                Simpan</button>
            <button type="button" class="btn" id="btnBatalUpload" onclick="">
                Batal</button>
        </div>
    End Using
    
    
<link href="@Url.Content("~/plugins/jquery-fileupload/jquery.fileupload-ui.css")" rel="stylesheet"type="text/css" />
<script type="text/javascript">
    $("#btnUpload").click(function () {
        $("#divList").slideUp("slow");
        $("#divForm").slideDown("slow");
    });

    $("#btnBatalUpload").click(function () {
        $("#divList").slideDown("slow", function () {
            $("#frmUploadDocDetail").trigger("reset");
            $("#ID").val(0);
        });
        $("#divForm").slideUp("slow");

        $('.form-group', $('#frmUploadDocDetail')).removeClass("has-error");
    });
</script>