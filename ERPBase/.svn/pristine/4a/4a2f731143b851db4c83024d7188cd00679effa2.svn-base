@ModelType SideBarMenu
@Code
    ViewData("Title") = "System Menu"
  
End Code
@Using Html.BeginJUIBox("Menu Settings")
    @<div class="row">
        <div class="col-sm-4 col-lg-2">
            Module
        </div>
        <div class="col-sm-4 col-lg-2">
            @Using Html.BeginForm("GetSideMenus", "SystemSetting", Nothing, FormMethod.Post, New With {.autocomplete = "off"})
                @Html.DropDownList("ModuleNames", Nothing, Nothing, New With {.class = "form-control", .onchange = "loadMenu();"})    
    End Using
        </div>
    </div>
    @<div class="row">
        <div class="col-sm-8">
            <h2>
                Menu</h2>
        </div>
        <div class="col-sm-4">
            <button type="button" class="btn btn-primary btn-lg" onclick="showModalMenu();">
                add Menu
            </button>
        </div>
    </div>
    @<div class="row">
        <div class="col-sm-6 col-lg-6">
            <div class="dd" id="nestable3">
            </div>
        </div>
        <div class="col-sm-6 col-lg-6">
            <div class="panel">
                <div class="panel-heading bg-primary">
                    Menu Properties</div>
                <div class="panel-body">
                  
                </div>
            </div>
        </div>
    </div>
    @<div class="modal fade" id="modalMenu">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">
                        Menu</h4>
                </div>
                <div class="modal-body">
                  @Using Html.BeginForm("SaveMenu", "SystemSetting", FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmMenu"})
                        @Html.HiddenFor(Function(m) m.ID)
                        @Html.HiddenFor(Function(m) m.Icon)
                        @Html.HiddenFor(Function(m) m.Module)
                        @Html.HiddenFor(Function(m) m.Ordinal)
                        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Label, New With {.class = "form-control"}), "Text Menu")

                          @<div class="form-group">
                          <label class="col-lg-3 col-sm-4 control-label">Jenis Menu</label>  
                          <div class="col-lg-8 col-sm-6">
                            <div class="radio-inline">
                                <label>
                                    <input name="menutype" checked="checked" type="radio" value="submenu" id="menutype_submenu">
                                    Sub Menu <i class="fa fa-circle-o"></i>
                                </label>
                            </div>
                             <div class="radio-inline">
                                <label>
                                    <input name="menutype" type="radio" value="actionlink" id="menutype_actionlink">
                                    Link Menu <i class="fa fa-circle-o"></i>
                                </label>
                            </div>
                        </div>
                          </div>
                        
                        
        Dim linkTextbox As New MvcHtmlString("<div class='input-group'>" &
                                            "<span class='input-group-addon'>~</span>" &
                                            "<input  type='text' id='ActionLink' name='ActionLink' class='form-control' value='#' disabled='disabled'/>" &
                                            "</div>")
                        @Html.WriteFormInput(linkTextbox, "Link", smControlWidth:=6, lgControlWidth:=8)
                        @Html.WriteFormInput(New MvcHtmlString("<div  id='iconpicker'></div>"), "Icon")
                        
                        @<div class="well">
                            <div class="row">
                                <div class="col-sm-3 col-lg-4">
                                    Contoh:</div>
                                <div class="col-sm-6 col-lg-6">
                                    <ul class="dropdown-menu" style="display: block">
                                        <li id='sampleLink'><a href="#" target="_blank"><i class="fa fa-dashboard"></i>New Menu</a></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
    End Using
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                    <button type="button" class="btn btn-primary" onclick="saveMenu();">
                        Save changes</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
End Using

<link href="@Url.Content("~/plugins/jquery-nestable/Jquery-nestable.css")" rel="stylesheet" type="text/css" />
<script src="@Url.Content("~/plugins/jquery-nestable/jquery.nestable.js")" type="text/javascript"></script>
<link href="@Url.Content("~/plugins/bootstrap-iconpicker/css/bootstrap-iconpicker.css")" rel="stylesheet" type="text/css" />

<style type="text/css">
#nestable3 span
{
    position:absolute;top:4px;right:3px;display:block;width:30px;cursor:pointer;
}

@@media only screen and (min-width: 700px) {

    .dd { float: left; width: 100%; }
    .dd + .dd { margin-left: 2%; }

}

</style>
<script src="@Url.Content("~/plugins/bootstrap-iconpicker/js/iconset/iconset-fontawesome-4.2.0.js")"  type="text/javascript"></script>
<script src="@Url.Content("~/plugins/bootstrap-iconpicker/js/bootstrap-iconpicker.js")" type="text/javascript"></script>
<script type="text/javascript">


    loadMenu = function () {
        var _modulename = $("#ModuleNames").val();
        var _url = "/SystemSetting/GetSideMenus";
        $.post(
            _url, { modulename: _modulename }
       ).done(function (data) {
           $('#nestable3').empty();
           var _ol = "<ol class='dd-list'>";
           $("#nestable3").append(_ol);
           _ol = $("#nestable3 ol")

           _buildMenu(data, _ol);

           $(".dd3-content").click(function (e) {
               e.stopPropagation();
               getMenu(this);
           });
       });

    }
    var showModalMenu = function () {
        $("#frmMenu").trigger("reset");
        $("#samplelink").empty();
        $("#samplelink").append("<a href='#' target='_blank'><i class='fa fa-dashboard'></i>&Nbsp;New Menu</a>");
        $("#modalMenu").modal();
    }
    var _buildMenu = function (data, _ol) {
        $(data).each(function (m) {
            var _li = '<li class="dd-item dd3-item"  data-id="' + this.ID + '"/>';
            var _div1 = '<div class="dd-handle dd3-handle" >';
            var _div2 = '<div class="dd3-content" data-actionlink="' + this.ActionLink + '" data-icon="' + this.Icon +
            '" data-ordinal="' + this.Ordinal +
            '">' + this.Label + '</div><span onclick="showRoles(this);" title="roles"  class="bg-danger text-center"><i class="fa fa-users"></i></span>';
            var button1 = '<button style="display: block;" data-action="collapse" type="button">Collapse</button>';
            var button2 = '<button style="display: none;" data-action="expand" type="button">Expand</button>';
            $(_ol).append(_li);
            _li = $(_ol).find("li:last-child");



            jQuery.data(_li, "items", "test");
            if (this.Children.length > 0) {

                $(_li).append(button1, button2, _div1, _div2);
                var _ol2 = "<ol class='dd-list'>";
                $(_li).append(_ol2);
                _ol2 = $(_li).find("ol:last-child");
                var _data = this.Children;
                _buildMenu(_data, _ol2);
            } else {
                $(_li).append(_div1, _div2);

            }


        });
    }
    var getMenu = function (obj) {
        $("#ID").val($(obj).parent().data("id"));

        $("#Label").val($(obj).text());
        if ($(obj).data("actionlink") == "#") {
            $("input[name = 'menutype'][value='submenu']").prop("checked", true);
            $("#ActionLink").attr("disabled", "disabled");
        } else {
            $("input[name = 'menutype'][value='actionlink']").prop("checked", true);
            $("#ActionLink").removeAttr("disabled");
        }
        $("#ActionLink").val($(obj).data("actionlink"));
        $('#sampleLink a > i').attr('class', '').addClass();
        $("#Icon").val($(obj).data("icon"));
        $("#Ordinal").val($(obj).data("ordinal"));

        var icon = "<i class='fa " + $(obj).data("icon") + "'></i>";
        var text = $("#Label").val();

        $('#sampleLink a').empty();
        $('#sampleLink a').append(icon).append(" " + text);
        $("#iconpicker").iconpicker('setIcon', $(obj).data("icon"));
        $("#Module").val($("#ModuleNames").val());
        $("#modalMenu").modal();
    }


    //--modal dialog--

    saveMenu = function () {
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
        
    }

    showRoles = function (obj) {
        //alert($(obj).parent().data("id"));
        window.location = "/SystemSetting/rolesformenu/" + $(obj).parent().data("id");
    }
    $(function () {

    //move the #modalMenu to body to make sure z-index works well;
        $("#modalMenu").detach().appendTo($("body"));


        $('#nestable3').nestable().change(function (e) {
            var list = e.length ? e : $(e.target);
            var _datalist = list.nestable('serialize');
            var jsondata = JSON.stringify(_datalist);
            $.post("/SystemSetting/reorder", { obj: jsondata });
        }); ;
        loadMenu();


        //MODAL DIALOG
        $('#Label').on('focusout', function (e) {
            var icon = $('#sampleLink a > i');
            var text = $("#Label").val();

            $('#sampleLink a').empty();
            $('#sampleLink a').append(icon).append(" " + text);

        });
        $("#ActionLink").on('focusout', function (e) {
            var url = $("#ActionLink").val();
            $('#sampleLink a').attr("href", url);

        });

        $('#iconpicker').on('change', function (e) {
            $('#sampleLink a > i').attr('class', '').addClass('fa ' + e.icon);
            $("#Icon").val( e.icon);
        });


        $("#iconpicker").iconpicker({
            arrowPrevIconClass: "fa fa-arrow-left",
            arrowNextIconClass: "fa fa-arrow-right",
            iconset: 'fontawesome',
            cols: 8,
            "selectedClass": "btn-danger",
            "unselectedClass": "btn-info",
            "searchText":"Cari.."
        });


        $("input[name='menutype']:radio").change(function () {

            var _value = $(this).val();
            switch (_value) {
                case "submenu":
                    $("#ActionLink").val("#");
                    $("#ActionLink").attr("disabled", "disabled");
                    break;
                case "actionlink":
                    $("#ActionLink").val("/");
                    $("#ActionLink").removeAttr("disabled");
                    break;

            }

        });

    });
</script>
