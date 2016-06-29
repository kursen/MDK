<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>@ViewData("Title")</title>

    <style type="text/css">
        .login .middle-wrapper > [class^='col-md'] {
            margin:auto;
            float:none;
        }
        .field-validation-error{
            color:Red;
        }
    </style>
    
    <link href="@Url.Content("~/Content/css/bootstrap.min.css")" rel="stylesheet" type="text/css" />
    <link href="@Url.Content("~/Content/css/Site.css")" rel="stylesheet" type="text/css" />
    <link href="@Url.Content("~/Content/css/light-theme.css")" rel="stylesheet" type="text/css" />
</head>
<body class="login">
<!-- Page Content -->
    <div class="middle-container">
        <div class="middle-row">
            @RenderBody()
        </div>
    </div>
</body>
</html>

@*

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>PDAM Tirtanadi - Human Resource System</title>
    <link href="@Url.Content("~/Content/css/images/shared/icon_idlogo.png")" rel="shortcut icon">
    <link href="@Url.Content("~/Content/css/bootstrap.css")" rel="stylesheet" type="text/css" media="screen" />
    <link href="@Url.Content("~/Content/css/Site.css")" rel="stylesheet" type="text/css" media="screen" title="default" />
    <link href="@Url.Content("~/Content/css/validation-summary.css")" rel="stylesheet"type="text/css" media="screen" />
</head>
<body id="login-bg">
    @RenderBody()
    <script src="@Url.Content("~/Content/js/jquery-1.8.2.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Content/js/bootstrap.js")" type="text/javascript"></script>
</body>
</html>*@