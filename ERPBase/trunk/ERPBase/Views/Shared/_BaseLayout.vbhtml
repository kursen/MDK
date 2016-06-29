@Code
    Dim MenuDisplaystate As Boolean = False
    If User.Identity.IsAuthenticated Then

        Dim p = ProfileCommon.Create(User.Identity.Name)
        MenuDisplaystate = p.GetPropertyValue("MenuDisplayState")
    End If
    Dim displayClass = IIf(MenuDisplaystate, "", "sidebar-show")
End Code
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>ERP</title>
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
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script type="text/javascript" src="@Url.Content("~/plugins/bootstrap/bootstrap.min.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/plugins/jquery-number/jquery.number.js")" ></script>
    <script type="text/javascript" src="@Url.Content("~/plugins/bootstrap-notify/bootstrap-notify.js")" ></script>
    <script type="text/javascript" src="@Url.Content("~/plugins/bootbox/bootbox.min.js")" ></script>
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
                    <a href="#" class="show-sidebar"><i class="fa fa-bars"></i></a><a href="/">MEGA DUTA
                        KONSTRUKSI</a>
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
                                            @Html.Raw(User.Identity.Name)
                                        </span>
                                    </div>
                                </a>
                                    <ul class="dropdown-menu">
                                        <li><a href="/Account/Profile" class='ajax-link'><i class="fa fa-user"></i><span>Profile</span>
                                        </a></li>
                                        <!-- <li><a href="ajax/page_messages.html" class="ajax-link"><i class="fa fa-envelope"></i>
                                            <span>Messages</span> </a></li>
                                        <li><a href="ajax/gallery_simple.html" class="ajax-link"><i class="fa fa-picture-o">
                                        </i><span>Albums</span> </a></li>
                                        <li><a href="ajax/calendar.html" class="ajax-link"><i class="fa fa-tasks"></i><span>
                                            Tasks</span> </a></li>
                                        <li><a href="#"><i class="fa fa-cog"></i><span>Settings</span> </a></li>
                                        <li><a href="#" id="locked-screen"><i icon="fa fa-lock"></i><span>Lock</span> </a>
                                        </li>-->
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
                    <a href="/">HOME</a> | <a href="/ProjectManagement">Project Management</a>
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
                    Dim areaName = HttpContext.Current.Request.RequestContext.RouteData.DataTokens("area")
                    If String.IsNullOrEmpty(areaName) Then
                        areaName = "HOME"
                    End If
                    
                    @Html.Raw(SideMenuBuilder.RenderMenu(Request.Path, areaName.ToString(), User))

                    
                    
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
                                <li>@ViewData("Title")</li>
                            </ol>
                        </div>
                    </div>
                    <!--End Breadcrumb-->
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
    </script>
    @RenderSection("endscript",False)
</body>
</html>
