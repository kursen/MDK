
                <div class="navbar-brand md-hide">
                    <ul class="nav navbar-nav">
                        @If Request.IsAuthenticated = True Then
                            @<li class="dropdown">
                              <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">
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
                            @<li>
                                @Html.RouteLink("Login", "Default", New With {.action = "LogOn", .controller = "Account"})
                            </li>
   
                        End If
                    </ul>
                </div>