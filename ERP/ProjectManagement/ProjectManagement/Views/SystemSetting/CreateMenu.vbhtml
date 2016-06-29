@ModelType SideBarMenu
@Code
    ViewData("Title") = "CreateMenu"
   
End Code
<h2>
    @Html.Raw(Model.Module) @Html.Raw("Menu")</h2>
<br />
<br />
@Using Html.BeginJUIBox("Menu builder")
    
    @<div class="row">
        <div class="col-lg-4 col-sm-4">
            <div style="padding: 10px; background-color: #6AA6D6; height: 300px;">
                <ul class="dropdown-menu" style="display: block; top: 40px; left: 30px;">
                    <li id='sampleLink'><a href="#" target="_blank"><i class="fa fa-dashboard"></i></a>
                    </li>
                </ul>
            </div>
        </div>
        <div class="col-lg-8 col-sm-8">
            @Using Html.BeginForm("SaveMenu", "SystemSetting", FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmMenu"})
                @Html.HiddenFor(Function(m) m.ID)
                @Html.HiddenFor(Function(m) m.Icon)
                @Html.HiddenFor(Function(m) m.Module)
                @Html.HiddenFor(Function(m) m.Ordinal)
                @Html.HiddenFor(Function(m) m.ActionLink)
                @Html.HiddenFor(Function(m) m.ParentId)
                @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Label, New With {.class = "form-control"}), "Text Menu")

                @<div class="form-group">
                    <label class="col-lg-3 col-sm-4 control-label">
                        Jenis Menu</label>
                    <div class="col-lg-8 col-sm-6">
                        <div class="radio-inline">
                            <label>
                            
                                    @Html.RadioButton("menutype", "submenu", Model.ActionLink = "#", New With {.id = "menutype_submenu"})
                                    @Html.Raw("SubMenu<i class='fa fa-circle-o'></i> ")
                                    
                            
                              @*  <input name="menutype" checked="checked" type="radio" value="submenu" id="menutype_submenu">
                                Sub Menu <i class="fa fa-circle-o"></i>    *@
                                
                                
                            </label>
                        </div>
                        <div class="radio-inline">
                            <label>
                            
                                    @Html.RadioButton("menutype", "actionlink", Model.ActionLink <> "#", New With {.id = "menutype_actionlink"})
                                    @Html.Raw("Link Menu<i class='fa fa-circle-o'></i> ")
                               @* <input name="menutype" type="radio" value="actionlink" id="menutype_actionlink">
                                Link Menu <i class="fa fa-circle-o"></i>*@
                            </label>
                        </div>
                    </div>
                </div>
                        
                        
        Dim linkTextbox As New MvcHtmlString("<div class='input-group'>" &
                                            "<span class='input-group-addon'>~</span>" &
                                            "<input  type='text' id='ActionLinkInput' class='form-control' value='#' disabled='disabled'/>" &
                                            "</div>")
                @Html.WriteFormInput(linkTextbox, "Link", smControlWidth:=6, lgControlWidth:=8)
                @Html.WriteFormInput(New MvcHtmlString("<div  id='iconpicker'></div>"), "Icon")
                        
                @<div class="well">
                    <div class="row">
                        <div class="col-sm-3 col-sm-offset-3 col-lg-4 col-lg-offset-4">
                            <button type="button" class="btn btn-primary" id="btnSaveMenu">
                                Simpan</button>
                                 <button type="button" class="btn btn-primary" id="btnCancel">
                                Kembali</button>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
    End Using
        </div>
    </div>

End Using
<style>
  .ui-autocomplete {
    max-height: 100px;
    overflow-y: auto;
    /* prevent horizontal scrollbar */
    overflow-x: hidden;
  }
</style>
<script src="@Url.Content("~/plugins/bootstrap-iconpicker/js/iconset/iconset-fontawesome-4.2.0.js")"  type="text/javascript"></script>
<script src="@Url.Content("~/plugins/bootstrap-iconpicker/js/bootstrap-iconpicker.js")" type="text/javascript"></script>
<script type="text/javascript">




    var saveMenu = function () {
        var _data = $("#frmMenu").serialize();
        var _url = $("#frmMenu").attr("action");
        $.ajax({
            type: 'POST',
            url: _url,
            data: _data,
            success: saveMenuCallback,
            error: ajax_error_callback,
            dataType: 'json'
        });
    }

    saveMenuCallback = function (data) {
        if (data.stat == 1) {
            window.location = "/SystemSetting/?modulename=" + $("#Module").val();
        }

    }

    var initAutocompleteLink = function () {
        var _url = "/SystemSetting/GetAvailabelLinks";
        
        $.ajax({
            type: 'POST',
            url: _url,
            success: function (data) {
                $("#ActionLinkInput").autocomplete({
                    source: data

                });

            },
            error: ajax_error_callback,
            dataType: 'json'
        });
    }
</script>
<script type="text/javascript">
    var availableLink = [];

    $(function () {
        initAutocompleteLink();
        $("#btnSaveMenu").click(function () {

            saveMenu();

        });
        $("#btnCancel").click(function () {

            window.location = "/SystemSetting";
        });



        $("#iconpicker").iconpicker({
            arrowPrevIconClass: "fa fa-arrow-left",
            arrowNextIconClass: "fa fa-arrow-right",
            iconset: 'fontawesome',
            cols: 8,
            "selectedClass": "btn-danger",
            "unselectedClass": "btn-info",
            "searchText": "Cari.."
        });


        $("input[name='menutype']:radio").change(function () {

            var _value = $(this).val();
            switch (_value) {
                case "submenu":
                    $("#ActionLink").val("#");
                    $("#ActionLinkInput").val("#");
                    $("#ActionLinkInput").attr("disabled", "disabled");
                    break;
                case "actionlink":
                    $("#ActionLink").val("/");
                    $("#ActionLinkInput").val("/");
                    $("#ActionLinkInput").removeAttr("disabled");
                    break;

            }
            
        });



        $('#Label').on('focusout', function (e) {
            var icon = $('#sampleLink a > i');
            var text = $("#Label").val();

            $('#sampleLink a').empty();
            $('#sampleLink a').append(icon).append(" " + text);

        });
        $("#ActionLinkInput").on('focusout', function (e) {
            var url = $("#ActionLinkInput").val();
            $('#sampleLink a').attr("href", url);
            $("#ActionLink").val(url);


        });

        $('#iconpicker').on('change', function (e) {
            $('#sampleLink a > i').attr('class', '').addClass('fa ' + e.icon);
            $("#Icon").val(e.icon);
        });

        $('#Label').trigger("focusout");

        if ($("#ActionLink").val() != "#") {
            $("#ActionLinkInput").removeAttr("disabled");
            $("#ActionLinkInput").val($("#ActionLink").val());
        }

    });
    
</script>
