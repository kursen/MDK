@Using Html.BeginForm("CropPhoto", "Account", Nothing, FormMethod.Post, New With {.id = "frmEditPhoto"})
    @<input type="hidden" name="userid" id="userid" value="@User.Identity.Name" />
    @<input type="hidden" id="x" name="x" />
    @<input type="hidden" id="y" name="y" />
    @<input type="hidden" id="w" name="w" />
    @<input type="hidden" id="h" name="h" />
End Using
<div class="well" style="margin: 0px auto; min-height: 200px">
    <img id="imgTarget" src="" alt="">
</div>
<div class="clear">
</div>
<div class="well">
    <span class="btn btn-success btn-large fileinput-button"><i class="icon-upload icon-white">
    </i><span>&nbsp;&nbsp;Ganti Foto</span>
        <input type="file" multiple="false" id='fileupload'>
    </span>
    <button type="button" id="btninit" class="btn btn-large btn-primary" onclick="initCropping()">
        <i class="icon-wrench icon-white"></i>&nbsp;&nbsp;Edit</button>
    <button type="button" id="btncrop" class="btn btn-large btn-primary  disabled" onclick="cropPhoto()"
        disabled="disabled">
        <i class="icon-upload icon-white"></i>&nbsp;&nbsp;Crop</button>
    <button type="button" class="btn btn-large btn-primary" onclick="navigateToDetailPage();">
        <i class="icon-arrow-left icon-white"></i>&nbsp;&nbsp;Kembali</button>
    <div id="placeholder_upload">
    </div>
</div>
<style>
    #imgTarget
    {
        max-width: none;
    }
</style>
<script src="@Url.Content("~/plugins/jquery/jquery-migrate-1.2.1.min.js")" type="text/javascript"></script>

<link href="@Url.Content("~/plugins/jquery-fileupload/jquery.fileupload-ui.css")" rel="stylesheet"
    type="text/css" />
<script src="@Url.Content("~/plugins/jquery-fileupload/jquery.iframe-transport.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/jquery-fileupload/jquery.fileupload.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/jquery-jcrop/jquery.Jcrop.js")" type="text/javascript"></script>
<link href="@Url.Content("~/plugins/jquery-jcrop/jquery.Jcrop.css")" rel="stylesheet" type="text/css" />
<script src="@Url.Content("~/js/uploadphoto.js")" type="text/javascript"></script>
<script type="text/javascript">
     


        
</script>
