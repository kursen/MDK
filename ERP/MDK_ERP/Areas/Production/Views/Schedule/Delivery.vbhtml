@Code
    ViewData("Title") = "Jadwal Delivery"
    ViewBag.headIcon = "icon-tasks"
End Code
@imports MDK_ERP.HtmlHelpers
<div class="row">
    <table class="table table-bordered">
        <thead>
            <tr>
                <th>
                    Tanggal
                </th>
                <th>
                    Produk
                </th>
                <th>
                    Kuantitas
                </th>
                <th>
                    Tujuan
                </th>
                <th>
                    Nr. Kontrak
                </th>
                <th>
                    Status
                </th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>
                    01-01-2015
                </td>
                <td>
                    ASPAL AC-BC
                </td>
                <td>
                    5 ton
                </td>
                <td>
                    Pasar 5 Marelan
                </td>
                <td>
                    <a href="">1233555-3344.</a>
                </td>
                <td>
                    Delivered
                </td>
            </tr>
            <tr>
                <td>
                    01-01-2015
                </td>
                <td>
                    ASPAL AC-BC
                </td>
                <td>
                    5 ton
                </td>
                <td>
                    Pasar 5 Marelan
                </td>
                <td>
                    <a href="">1233555-3344.</a>
                </td>
                <td>
                 <a href="#" data-toggle="tooltip" title="klik untuk mengubah jadwal produksi">Scheduled</a>
                </td>
            </tr>
            <tr>
                <td>
                    01-01-2015
                </td>
                <td>
                    ASPAL AC-BC
                </td>
                <td>
                    5 ton
                </td>
                <td>
                    Pasar 5 Marelan
                </td>
                <td>
                    <a href="">1233555-3344.</a>
                </td>
                <td>
                    <a href="#" data-toggle="tooltip" title="klik untuk membuat jadwal produksi">Unscheduled</a>
                </td>
            </tr>
        </tbody>
    </table>
</div>


@Section jsScript
<script type="text/javascript">
$(function () { $('[data-toggle="tooltip"]').tooltip() })
</script>
End Section
