@ModelType  ERPBase.LogOnModel
@Code
    ViewData("Title") = "Log On"
End Code
<p>
    Session anda telah berakhir. Silahkan login kembali.</p>
<p>@Html.ActionLink("log in", "logon", "account", Nothing, New With {.style = "font-size:20px;", .id = "atarget"})</p>
<p id="countmesg">
</p>
<script type="text/javascript">
    $(document).ready(function () {
        return;
        var delay = 10;
        var url = $("#atarget").attr("href");
        function countdown() {
            setTimeout(countdown, 1000);
            $('#countmesg').html("Membuka halaman login  dalam waktu " + delay + " detik.");
            delay--;
            if (delay < 0) {
                window.location = url;
                delay = 0;
            }
        }
        countdown();
    });
</script>
