<nav class="navbar navbar-inverse" role="navigation">
    <div class="navbar-inner">
        <!-- Brand and toggle get grouped for better mobile display -->
        <div class="navbar-header">
            <button aria-expanded="false" type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            @Html.Partial("_LogOnPartial")
        </div>
        <!-- Collect the nav links, forms, and other content for toggling -->
        <div style="height: 1px;" aria-expanded="false" class="navbar-collapse collapse" id="bs-example-navbar-collapse-1">
            <ul class="nav navbar-nav">
                @*
                <li>
                    <a href="@Url.RouteUrl("Default", New With {.Action = "Index", .Controller = "Home"})" style="font-size: 18px;"><i class="icon-home"></i></a>
                </li>
                <li class="divider"></li>
                *@
                @Html.Raw(MDK_ERP.TopNavigationBuilder.Render(Request.Path, ViewContext.Controller.ViewData("ModuleName"), User))
                @If Request.IsAuthenticated = True Then
                    @<li class="dropdown lg-hide">
                        <a class="dropdown-toggle" href="#" data-toggle="dropdown">
                            <i class="icon-user icon-white"></i>&nbsp; @User.Identity.Name <span class="caret"></span>
                        </a>
                        <ul class="dropdown-menu" role="menu">
                            <li>
                                @Html.RouteLink("Ubah Password", "Default", New With {.action = "Changepassword", .controller = "Account"})
                            </li>
                            <li class="divider"></li>
                            <li>
                                @Html.RouteLink("Logout", "Default", New With {.action = "LogOff", .controller = "Account"})
                            </li>
                        </ul>
                    </li>
                Else
                    @<li class="lg-hide">
                        @Html.RouteLink("Login", "Default", New With {.action = "LogOn", .controller = "Account"})
                    </li>
                End If
            </ul>
        </div>
        <!-- /.navbar-collapse -->
    </div>
    <!-- /.container -->
</nav>
