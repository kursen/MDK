<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>ERP @ViewData("Title")</title>
    <meta name="description" content="description">
    <meta name="author" content="Evgeniya">
    <meta name="keyword" content="keywords">
    <link href="@Url.Content("~/plugins/bootstrap/bootstrap.css")" rel="stylesheet">
    <link href="@Url.Content("~/plugins/jquery-ui/jquery-ui.min.css")" rel="stylesheet">
    <link href="@Url.Content("~/css/font-awesome.css")" rel="stylesheet" type="text/css" />
    <link href='@Url.Content("~/css/righteous.css")' rel='stylesheet' type='text/css'>
    <link href="@Url.Content("~/css/style.css")" rel="stylesheet">
    <link href="@Url.Content("~/css/animate.css")" rel="stylesheet" type="text/css" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
				<script src="http://getbootstrap.com/docs-assets/js/html5shiv.js"></script>
				<script src="http://getbootstrap.com/docs-assets/js/respond.min.js"></script>
		<![endif]-->
    <script type="text/javascript" src="@Url.Content("~/plugins/jquery/jquery-2.1.0.min.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/plugins/jquery-ui/jquery-ui.min.js")"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script type="text/javascript" src="@Url.Content("~/plugins/bootstrap/bootstrap.min.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/plugins/jquery-number/jquery.number.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/plugins/bootstrap-notify/bootstrap-notify.js")" ></script>
    <!-- All functions for this theme + document.ready processing -->
    <script type="text/javascript" src="@Url.Content("~/js/devoops.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/js/common.js")" ></script>
</head>
<body>
    <div class="container-fluid">
        @RenderBody()
    </div>
</body>
</html>
