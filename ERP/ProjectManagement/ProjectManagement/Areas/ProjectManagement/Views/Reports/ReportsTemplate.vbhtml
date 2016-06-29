@ModelType ProjectManagement.ReportLayout
@Code
    ViewData("Title") = "Informasi Proyek"
    Html.SetEditableDefaultSettings(DisableOnload:=True)
    
    
    Dim prmEntities As New ProjectManagement.ProjectManagement_ERPEntities
    ViewData("ProjectInfo") = prmEntities.ProjectInfoes.Where(Function(m) m.Id = Model.IdProjectInfo).SingleOrDefault()
End Code
@Html.Partial("ProjectPageMenu", ViewData("ProjectInfo"))

<style>
    img.reportLogo
    {
        height: 100px;
        box-shadow:0px 0px 10px rgba(210, 210, 210, 0.5) inset;
        -webkit-box-shadow:0px 0px 10px rgba(210, 210, 210, 0.5) inset;
        -moz-box-shadow:0px 0px 10px rgba(210, 210, 210, 0.5) inset;
        display: inline-block;
        margin: 10px 0px;
        min-width: 100px;
        text-align: left !important;
        /*padding: 30px;*/
    }
    img.reportLogo::before
    {
        text-align: left !important;
    }
    .editable-container.popover
    {
        z-index:999;
    }
    .editable-empty
    {
        color:#428BCA !important;
        font-style:normal;
    }
    a.editable-empty:hover, a.editable-empty[class$="Logo"]:focus {
    color: #2A6496 !important;
}
</style>

@Using Html.BeginJUIBox("Desain Layout Laporan")
        @<div class="row">
            <div class="col-lg-12 col-sm-12">
                <div class="pull-right">
                    <button type="button" class="btn btn-danger btn-label-left" onclick="$('#dProjectInfo .editable') .editable('toggleDisabled')">
                        <span><i class="fa fa-edit"></i></span>Edit</button>
                </div>
            </div>
        </div>
     @<div style="position: relative;overflow:auto;">
   <table id="dProjectInfo" class="table table-bordered" style="width: 100%;min-width:1000px; table-layout: fixed;
        background-color: #fff;">
        <thead>
            <tr>
                <th rowspan="2" class="text-center" style="width: 65%;">
                    <div class="row">
                        <div class="col-xs-3">
                            <a tabindex="-1" class="editable editable-click editable-disabled" data-pk="1" data-title="Logo Kiri" data-type="text" data-url="/Reports/SavePartial_Image" id="LeftLogo">
                                <img class="reportLogo" src="@Url.Action("getLogo", "Reports", New With {.id = Model.ID, .logo = "left"})" alt="Edit" />
                            </a>
                        </div>
                        <div class="col-xs-6">
                            <h5>
                                @Html.EditableInputTextBox("Title1", Model.Title1, "text", "Judul Atas", datapk:=Model.Id, dataurl:="/Reports/SavePartial")
                            </h5>
                            <h4>
                                @Html.EditableInputTextBox("Title2", Model.Title2, "text", "Judul Bawah", datapk:=Model.Id, dataurl:="/Reports/SavePartial")
                            </h4>
                            <br />
                            <h5 class="text-center">Deskripsi Proyek</h5>
                        </div>
                        <div class="col-xs-3">
                            <a tabindex="-1" class="editable editable-click editable-disabled" data-pk="1" data-title="Logo Kanan" data-type="text" data-url="/Reports/SavePartial_Image" id="RightLogo">
                                <img class="reportLogo" src="@Url.Action("getLogo", "Reports", New With {.id = Model.ID, .logo = "right"})" alt="Edit" />
                            </a>
                        </div>
                    
                    </div>
                </th>
                <th class="text-center" style="vertical-align: middle;">
                    <h4>Jenis Laporan</h4>
                </th>
                <th rowspan="2" style="vertical-align: middle;">
                    <p class="text-center">
                        Deskripsi
                    </p>
                </th>
            </tr>
            <tr>
                <td>
                    <p class="text-center">
                        Keterangan Waktu dan Periode
                    </p>
                </td>
            </tr>
            <tr>
                <th colspan="3" class="text-left" style="text-align:left !important;"> Header</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td colspan="3"> Content</td>
            </tr>
        </tbody>
    </table>
 </div>
End Using
@Section endScript
    <script type="text/javascript">

        var _savepartialresponse = function (data) {
            if ((data) && (data.stat == 1)) {

            } else {
                showNotification(data);
            };

        }

        $(function () {
            $('#LeftLogo, #RightLogo').click(function () {
                var obj = $(this);
                var img = obj.find('img.reportLogo').clone();
                setTimeout(function () {
                    var objForm = obj.siblings('.popover.editable-container').find('form');

                    objForm.find('.editable-input input').parent().before('' +
                        '<div class="editable-buttons">' +
                            '<button type="button" class="btn btn-danger btn-sm editable-remove" style="margin-right: 10px;"><i class="fa fa-trash"></i></button> ' +
                        '</div>');

                    objForm.find('.editable-input input').attr({ "type": "file", "name": "Logo" }).parent()
                        .append("<input type='hidden' value='@Model.ID' name='pk' />" +
                                "<input type='hidden' value='" + obj.attr('ID') + "' name='name' />");

                    objForm.submit(function (e) {
                        $(this).unbind('submit');
                        e.stopPropagation();
                        e.preventDefault();
                        var formData = new FormData(this);

                        if (objForm.find('.editable-input input[type="file"]').val() == "") {
                            showNotification("Masukan Logo tidak dapat kosong");
                            setTimeout(function () {
                                obj.html(img);
                            }, 200);
                            return false;
                        }

                        $.ajax({
                            type: 'POST',
                            url: parseUrl("Reports/SavePartial_Image"),
                            data: formData,
                            success: function (data) {
                                if (data.stat == 1) {
                                    dt = new Date();
                                    var url = img.attr("src") + "&LastMod=" + dt.getDate();
                                    img.attr("src",url)
                                    //i = $('<img/>', { src: url, class: "reportLogo", alt: "Edit" });

                                    setTimeout(function () {
                                        obj.html(img);
                                    }, 200);

                                    return false;
                                }
                                setTimeout(function () {
                                    obj.html(img);
                                }, 200);
                                showNotificationSaveError(data);
                            },
                            error: ajax_error_callback,
                            async: false,
                            contentType: false,
                            processData: false,
                            dataType: 'json'
                        });
                    });
                    objForm.find(".editable-remove").click(function () {
                        var _data = objForm.serialize();

                        $.ajax({
                            type: 'POST',
                            url: parseUrl("Reports/RemovePartial_Image"),
                            data: _data,
                            success: function (data) {
                                if (data.stat == 1) {
                                    dt = new Date();
                                    var url = img.attr("src") + "&LastMod=" + dt.getDate();
                                    img.attr("src", url)
                                    //i = $('<img/>', { src: url, class: "reportLogo", alt: "Edit" });

                                    setTimeout(function () {
                                        obj.html(img);
                                    }, 200);

                                    return false;
                                }
                                setTimeout(function () {
                                    obj.html(img);
                                }, 200);
                                showNotificationSaveError(data);
                            },
                            error: ajax_error_callback,
                            dataType: 'json'
                        });

                        $(this).parent().parent().find(".editable-cancel").click();
                    });
                }, 200);
            });
        })
    </script>
    <script src="@Url.Content("~/plugins/jquery/jquery-migrate-1.2.1.min.js")" type="text/javascript"></script>
End Section
