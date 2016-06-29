@Code
    Dim MenuDisplaystate As Boolean = False
    Dim DisplayName As String = User.Identity.Name
    Dim Officename = ""
    If User.Identity.IsAuthenticated Then

        Dim p = ERPBase.ErpUserProfile.GetUserProfile()
        Dim pcommon = ProfileCommon.Create(User.Identity.Name)
        MenuDisplaystate = pcommon.GetPropertyValue("MenuDisplayState")

        DisplayName = p.Fullname
        If String.IsNullOrEmpty(DisplayName) Then
            DisplayName = User.Identity.Name
        End If
        Officename = p.WorkUnitName
    End If
    Dim displayClass = IIf(MenuDisplaystate, "", "sidebar-show")
End Code
@helper WriteTopMenu()

    Dim listMenus As ERPBase.RegisteredModule()
    If Context.Cache.Item("haloErp.TopMenu") Is Nothing Then
        Dim _mainEntities = New ERPBase.MainEntities
        Dim menus = (From m In _mainEntities.RegisteredModules
                     Order By m.TopMenuOrdinal).ToArray()
        Context.Cache.Insert("haloErp.TopMenu", menus)
    End If
    
    listMenus = CType(Context.Cache.Item("haloErp.TopMenu"), ERPBase.RegisteredModule())
    For Each item In listMenus
        Dim theLink = Url.Content("~/" & item.ModuleName)
    @Html.Raw("<a href='" & theLink & "' >" & item.TopMenuCaption & "</a>")
        If Not item.Equals(listMenus.Last) Then
    @Html.Raw("&nbsp;|&nbsp;")
        End If
    Next
    

End Helper
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>MDK-ERP</title>
    <meta name="description" content="ERP">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="@Url.Content("~/plugins/bootstrap/bootstrap.css")" rel="stylesheet">
    <link href="@Url.Content("~/plugins/jquery-ui/jquery-ui.min.css")" rel="stylesheet">
    <link href="@Url.Content("~/css/font-awesome.css")" rel="stylesheet" type="text/css" />
    <link href='@Url.Content("~/css/righteous.css")' rel='stylesheet' type='text/css'>
    <link href="@Url.Content("~/plugins/xcharts/xcharts.min.css")" rel="stylesheet">
    <link href="@Url.Content("~/plugins/select2/select2.css")" rel="stylesheet">
    <link href="@Url.Content("~/css/style.css")" rel="stylesheet">
    <link href="@Url.Content("~/css/custom.css")" rel="stylesheet">
    <link href="@Url.Content("~/css/animate.css")" rel="stylesheet" type="text/css" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
				<script src="http://getbootstrap.com/docs-assets/js/html5shiv.js"></script>
				<script src="http://getbootstrap.com/docs-assets/js/respond.min.js"></script>
		<![endif]-->
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <!--<script src="http://code.jquery.com/jquery.js"></script>-->
    <script type="text/javascript" src="@Url.Content("~/plugins/jquery/jquery-2.1.0.min.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/plugins/jquery-ui/jquery-ui.min.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/plugins/jquery-ui/jquery.ui.touch-punch.min.js")"></script>
    <script src="@Url.Content("~/plugins/bootstrap/modernizr-custom.js")" type="text/javascript"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script type="text/javascript" src="@Url.Content("~/plugins/bootstrap/bootstrap.min.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/plugins/jquery-number/jquery.number.js")" ></script>
    <script type="text/javascript" src="@Url.Content("~/plugins/bootstrap-notify/bootstrap-notify.js")" ></script>
    <script type="text/javascript" src="@Url.Content("~/plugins/bootbox/bootbox.min.js")" ></script>
    <script type="text/javascript" src="@Url.Content("~/plugins/moment/moment.min.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/plugins/moment/moment-with-locales.js")"></script>
    <!-- All functions for this theme + document.ready processing -->
    <script type="text/javascript" src="@Url.Content("~/js/devoops.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/js/common.js")" ></script>
</head>
<body>
    <!--Start Header-->
    <div id="screensaver">
        <canvas id="canvas">
        </canvas>
        <i class="fa fa-lock" id="screen_unlock"></i>
    </div>
    <header class="navbar">
        <div class="container-fluid expanded-panel">
            <div class="row">
                <div id="logo" class="col-xs-12 col-sm-5">
                    <a href="#" class="show-sidebar"><i class="fa fa-bars"></i></a><a href="/">MDK-<span>@Officename</span></a>
                </div>
                <div id="top-panel" class="col-xs-12 col-sm-7">
                    <div class="row">
                        <div class="col-xs-2 col-sm-1">
                        </div>
                        <div class="col-xs-10 col-sm-11 top-panel-right">
                            <ul class="nav navbar-nav pull-right panel-menu">
                                <li class="dropdown"><a href="#" class="dropdown-toggle account" data-toggle="dropdown">
                                    <div class="avatar">
                                        <img src="@Url.Content("~/account/UserPhoto?userid=" & User.Identity.Name)" class="img-rounded" alt="avatar" />
                                    </div>
                                    <i class="fa fa-angle-down pull-right"></i>
                                    <div class="user-mini pull-right">
                                        <span class="welcome">Welcome,</span> <span>
                                            @Html.Raw(DisplayName)
                                        </span>
                                    </div>
                                </a>
                                    <ul class="dropdown-menu">
                                        <li><a href="/Account/Profile"><i class="fa fa-user"></i><span>Profile</span> </a>
                                        </li>
                                        <li><a href="@Url.Content("~/Account/logoff")"><i class="fa fa-power-off"></i><span>
                                            Logout</span> </a></li>
                                    </ul>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12" id="__moduletitle">
                    @WriteTopMenu()
                </div>
            </div>
        </div>
    </header>
    <!--End Header-->
    <!--Start Container-->
    <div id="main" class="container-fluid @displayClass ">
        <div class="row">
            <div id="sidebar-left" class="col-xs-2 col-sm-2">
                <ul class="nav main-menu">
                    @Code
                        
                       
                        Dim _mainEntities = New ERPBase.MainEntities
                        Dim modulesname = (From m In _mainEntities.RegisteredModules
                                          Select m.ModuleName).ToList
                        Dim areaName = HttpContext.Current.Request.RequestContext.RouteData.DataTokens("area")
                        If String.IsNullOrEmpty(areaName) Then
                            Dim arrPath = Request.CurrentExecutionFilePath.Split("/"c)
                            areaName = arrPath(1)
                            If Not modulesname.IndexOf(areaName) > -1 Then
                                areaName = String.Empty
                            End If
                            If String.IsNullOrEmpty(areaName) Then
                                areaName = "HOME"
                            End If
                            
                        End If
                        Dim thePath = PageData("ActiveLink")
                        
                     
                    
                        @Html.Raw(SideMenuBuilder.RenderMenu(thePath, areaName.ToString(), User))

                
                
                    
                    End Code
                </ul>
            </div>
            <!--Start Content-->
            <div id="content" class="col-xs-12 col-sm-10">
                <div class="preloader">
                    <img src="@Url.Content("~/img/devoops_getdata.gif")" class="devoops-getdata" alt="preloader" />
                </div>
                <div id="ajax-content">
                    <!--Start Breadcrumb-->
                    <div class="row">
                        <div id="breadcrumb" class="col-xs-12">
                            <ol class="breadcrumb">
                                <li> @PageData("Breadcrumb")</li>
                            </ol>
                        </div>
                    </div>
                
                    <!--End Breadcrumb-->
                    <h3 class='page-header'>
                        <i class='fa @IIf(ViewData("HeaderIcon") Is Nothing, "fa-list", ViewData("HeaderIcon"))'>
                        </i>&nbsp;@ViewData("Title")
                    </h3>
                    @RenderBody()
                    <ul>
                    </ul>
                </div>
            </div>
            <!--End Content-->
        </div>
    </div>
    <!--End Container-->
    @Html.WriteInitScripts()
    <script type="text/javascript">

        parseUrl = function (url) {
            var url = '@Url.Content("~")' + url;
            return url;
        }

        updateUserActivity = function () {

            $.ajax({
                type: 'POST',
                url: "/Home/UpdateLastActivity",
                success: function (data) {

                },
                error: ajax_error_callback,
                dataType: 'json'
            });


        }

        $(function () {
            updateUserActivity();

        });
    </script>
    @RenderSection("endscript", False)
</body>
</html>
