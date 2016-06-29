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
            <button class="btn btn-primary btn-lg" id="btnAddMenu">
                New Menu
            </button>
           
            <button class="btn btn-primary btn-lg" id="btnRefresh">
               Refresh
            </button>
            
           <button class="btn btn-primary btn-lg" id="btnDownloadModuleRole">
               Download Module Role
            </button>
        </div>
    </div>
    @<div class="row">
        <div class="col-sm-12 col-lg-12">
            <div class="dd" id="nestable3">
            </div>
        </div>
       
    </div>
   
End Using
<link href="@Url.Content("~/plugins/jquery-nestable/Jquery-nestable.css")" rel="stylesheet" type="text/css" />
<script src="@Url.Content("~/plugins/jquery-nestable/jquery.nestable.js")" type="text/javascript"></script>
<link href="@Url.Content("~/plugins/bootstrap-iconpicker/css/bootstrap-iconpicker.css")" rel="stylesheet" type="text/css" />
<style type="text/css">

#nestable3 
{
    min-height:300px;
    background-color:#6AA6D6;
    padding:20px 10px;
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

          
       });

    };


    var _buildMenu = function (data, _ol) {
        $(data).each(function (idx) {
            var _li = $("<li/>", {
                class: "dd-item dd3-item"
            }).data("id", this.ID);
            var _div1 = $("<div/>", {
                class: "dd-handle dd3-handle"
            }).appendTo(_li);
            var spWarning = "";
            if (this.ActionLink == "#" && this.Children.length == 0) {
                spWarning = '&nbsp;<span class="label label-warning" title="warning: action link is empty">!</span>';
            }
            var _div2 = $("<div/>", {
                html: this.Label + spWarning,
                class: "dd3-content"
            }).appendTo(_li);

            var _div3 = $("<div/>", {
                class: "btn-group",
                style: "position:absolute;top:2px;right:2px;"

            }).appendTo(_li);

            $("<button/>", {
                class: "btn btn-primary btnEdit",
                text: "Edit"

            }).data("id", this.ID)
            .appendTo(_div3);

            $("<button/>", {
                class: "btn btn-danger btnDelete",
                text: "Delete"
            }).data("id", this.ID)
            .appendTo(_div3);


            $("<button/>", {
                class: "btn btn-info btnRoles",
                text: "Roles"
            }).data("id", this.ID)
            .appendTo(_div3);

            $(_ol).append(_li);
            if (this.Children.length > 0) {
                var _nOl = $("<ol/>", { class: 'dd-list' }).appendTo(_li);
                _buildMenu(this.Children, _nOl);
            };
        });

    };






    $(function () {

        $('#nestable3').nestable().change(function (e) {
            var list = e.length ? e : $(e.target);
            var _datalist = list.nestable('serialize');
            var jsondata = JSON.stringify(_datalist);
            $.post("/SystemSetting/reorder", { obj: jsondata });
        }); ;

        loadMenu();

        $("#btnAddMenu").click(function () {
            window.location = "/SystemSetting/CreateMenu/?for=" + $("#ModuleNames").val();

        });

        $("#btnRefresh").click(function () {
            loadMenu();

        });

        $("#btnDownloadModuleRole").click(function () {
            var _modulename = $("#ModuleNames").val();
            window.location = "/SystemSetting/GetAssemblyLinks/?modulename=" + _modulename;

        });
        $("body").on("click", ".btnEdit", function (e) {
            window.location = "/SystemSetting/EditMenu/" + $(this).data("id");

        }).on("click", ".btnDelete", function (event) {
            event.stopPropagation();
            if (confirm("Remove this menu?")) {

                var _data = { id: $(this).data("id") };
                var _url = "/SystemSetting/DeleteMenu";
                $.post(
                    _url, _data
                ).done(function (data) {

                    loadMenu();
                });


            }
        }).on("click", ".btnRoles", function (e) {
            window.location = "/SystemSetting/RolesForMenu/" + $(this).data("id");
        });



    });
</script>
