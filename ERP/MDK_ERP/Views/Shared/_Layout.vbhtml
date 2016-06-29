<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="PT. Halotec Indonesia">
    <title>@ViewData("Title") | PT. Mega Duta Konstruksi</title>
    
    <link href="@Url.Content("~/Content/Images/shared/icon_idlogo.png")" rel="shortcut icon" />
    
    <link href="@Url.Content("~/Content/css/bootstrap.min.css")" rel="stylesheet" type="text/css" media="screen" />
    <link href="@Url.Content("~/Content/css/font-awesome.css")" rel="stylesheet" type="text/css" />
    <link href="@Url.Content("~/Content/css/jquery-ui.css")" rel="stylesheet" />
    <link href="@Url.Content("~/Content/css/responsive.css")" rel="stylesheet" type="text/css" />
    <link href="@Url.Content("~/Content/Plugin/SweetAlert/sweet-alert.css")" rel="stylesheet" type="text/css" />
    <link href="@Url.Content("~/Content/Plugin/DataTables/css/jquery.dataTables.css")" type="text/css" rel="Stylesheet" />

    @RenderSection("styleSheet", False)

    <link href="@Url.Content("~/Content/css/Site.css")" rel="stylesheet" type="text/css" media="screen" />

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body class="">
    <!-- Start:: page-top-outer -->
    <div id="page-top-outer">
        <!-- Start:: page-top -->
        <div id="page-top" class="container">
        <div class="row">
            <!-- Start:: logo -->
            
            <div id="logo">
                <a href="">
                    @*<img src="@Url.Content("~/Content/images/shared/mdk.jpg")" width="320" height="70" alt="logo" />*@
                    <h1>PT. MEGA DUTA KONSTRUKSI</h1>
                </a>

            </div>
            <!-- End:: logo
            <div class='pull-right' style="color:White;font-weight:bold">
                @ViewData("ModuleName")
            </div> -->
            </div>
        </div>
    <!--Start:: Navbar -->
    <div class="container">
        @Html.Partial("_MenuPartial")
    </div>
    <!--  End:: Navbar-->
        <!-- End:: page-top -->
    </div>
    <!-- End:: page-top-outer -->
    <!-- Start: content-outer -->
    <div id="content-outer">
        <!-- Start: content -->
        <div class="container">
            <div id="page-content">
                
                <!--  Start: page-heading -->
                @If IsNothing(ViewBag.showHeader) OrElse ViewBag.showHeader = True Then
                    @<div id="page-heading">
                        <h1>
                            <i class="@ViewBag.headIcon"></i>
                            @ViewData("Title")
                        </h1>
                    </div>
                End If
                <!-- End: page-heading -->

                <!--  Start: BreadCrumb -->
                <div id="breadcrumbs">
                    @Html.Raw(ViewData("BreadCrumb"))

                </div>
                <!--  End: BreadCrumb -->

                <!--  Start: page-body -->
                <div class="row">
                    <div class="col-sm-12">
                        @*<div id="indicator">
                            <img src="@Url.Content("~/Content/images/shared/indicator.gif")" alt="" /></div>*@
                        @RenderBody()
                    </div>
                </div>
                <!-- End: page-body -->
            </div>
            <div class="clear">&nbsp;</div>

            <!-- Start: footer -->
            <div id="footer">
                <!--  Start: footer-left -->
                <div id="footer-left">
                    System &copy; Copyright - 2015
                </div>
                <!--  End: footer-left -->
                <div class="clear">&nbsp;</div>
            </div>
            <!-- End: footer -->

        </div>
        <!--  End: content -->
        <div class="clear">&nbsp;</div>
    </div>
    <!--  End: content-outer-->
    <div class="clear">&nbsp;</div>
    
    <!-- Script Reference -->
    <script src="@Url.Content("~/Content/js/jquery.js")" type="text/javascript"></script>@*
    <script src="@Url.Content("~/Content/js/jquery-2.1.1.min.js")" type="text/javascript"></script>*@
    <script src="@Url.Content("~/Content/js/jquery-migrate.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Content/js/jquery-ui-1.9.0.custom.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Content/js/bootstrap.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Content/js/theme.js")" type="text/javascript"></script>

    <!-- common for time -->
    <script type="text/javascript" src="@Url.Content("~/Content/js/moment.min.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Content/js/moment-with-locales.js")"></script>

    <!-- common for alert -->
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/SweetAlert/sweet-alert.min.js")"></script>

    <!-- common for dataTable -->
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/DataTables/js/jquery.dataTables.min.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/DataTables/js/dataTables.overrides_indo.js")"></script>

    <!-- shared customade function -->
    <script type="text/javascript" src="@Url.Content("~/Scripts/shared-function.js")"></script>

    <!-- Another Script -->
    <script type="text/javascript">
        parseUrl = function (url) {
            var url = '@Url.Content("~")' + url;
            return url;
        }

        breadcrumbs = function (arr) {
            if (arr.length != 0) {
                var o = $('#breadcrumbs');
                $(o).removeClass('hidden');
                var li = o.find('ul');
                $.each(arr, function (k, v) {
                    if (arr[k][1]!=null || arr[k][1]!=undefined) {
                        $(li).append('<li><a href="' + parseUrl(arr[k][1]) + '">' + arr[k][0] + '</a></li>');
                    } else {
                        $(li).append('<li class="active">' + arr[k][0] + '</li>');
                    }
                });
            }
        }
    </script>
    @RenderSection("jsScript", False)
</body>
</html>
