@modeltype Announcement
@Code
    
    Dim dateFormat = New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                             .orientation = "top left",
                              .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}}
   
End Code
@Using Html.BeginJUIBox("Isi Pengumuman")
    Using Html.BeginForm("Save", "Announcement", Nothing, FormMethod.Post, New With {.class = "form-horizontal",
                                                                                           .autocomplete = "off", .id = "frmAnnouncement"})
    
    
    
    @Html.HiddenFor(Function(m) m.ID)
 
    @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Title, New With {.class = "form-control"}), "Judul")    
    @Html.WriteFormDateInputFor(Function(m) m.PublishDate, "Tanggal", dateFormat, smControlWidth:=4, lgControlWidth:=2)

    @<div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            Simpan Sebagai</label>
        <div class="col-lg-6col-sm-6">
            <div class="radio-inline">
                <label>
                    @Html.RadioButton("IsPublished", False, Model.IsPublished = False, New With {.autocomplete = "off"})
                    Draft<i class="fa fa-circle-o"></i>
                </label>
            </div>
            <div class="radio-inline">
                <label>
                    @Html.RadioButton("IsPublished", True, Model.IsPublished = True, New With {.autocomplete = "off"})
                    Umumkan<i class="fa fa-circle-o"></i>
                </label>
            </div>
        </div>
    </div>
    

    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="title">
                Isi Pengumuman</div>
            @Html.TextAreaFor(Function(m) m.TextContent, New With {.autocomplete = "off"})
        </div>
    </div>

    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="well">
                <div class="center-block" style="width: 200px">
                    <button class="btn btn-primary" type="button" id="btnSave">
                        Simpan</button>
                </div>
            </div>
        </div>
    </div>
           
    End Using
    
End Using
<script src="@Url.Content("~/plugins/tinymce/tinymce.min.js")" type="text/javascript"></script>
<script type="text/javascript">
    var _initTextEditor = function () {
        tinymce.init({
            selector: '#TextContent',
            height: 200,
            plugins: ["table link contextmenu code  autolink lists link image "],
            toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
            image_advtab: true
            //,content_css: "/Areas/Purchasing/css/contentTinyMCE.css"
        });
    } //init text editor

    var btnSubmit = null;
    $(function () {


        _initTextEditor();

        $("#btnSave").click(function () {
            tinyMCE.triggerSave();
            var _url = $("#frmAnnouncement").attr("action");
            var _data = $("#frmAnnouncement").serialize();
            btnSubmit = $(this).button("menyimpan data...");
            showSavingNotification();
            $.ajax({
                url: _url,
                data: _data,
                type: 'POST',
                success: function (d) {
                    if (d.stat == 1) {
                        window.location = "/Announcement/Index/";
                        return;
                    }
                    btnSubmit.button("Submit");
                    showNotificationSaveError(d);
                },
                error: ajax_error_callback,
                datatype: 'json'
            });

        });

    });

</script>
