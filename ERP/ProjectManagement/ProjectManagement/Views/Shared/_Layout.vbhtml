<!--Start Breadcrumb-->
<div class="row">
	<div id="breadcrumb" class="col-xs-12">
		<ol class="breadcrumb">
			<li>@ViewData("Title")</li>
		</ol>
	</div>
</div>
<!--End Breadcrumb-->

<h3 class='page-header'>
    <i class='fa @IIf(ViewData("HeaderIcon") is Nothing, "fa-list",ViewData("HeaderIcon"))'> </i> @ViewData("Title") </h3>

@RenderBody()
<script type="text/javascript">
    $(function () {
        updateUserActivity();

    });
</script>