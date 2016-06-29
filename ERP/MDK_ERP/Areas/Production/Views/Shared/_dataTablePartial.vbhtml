<div class="hidden">
<table>
    <tr id="form-komp-material">
        <!-- nama hanya misal, ganti sesuai dengan yang diperlukan -->
        <!-- setelah dibaca, komen ini akan terhapus dalam waktu 5 detik. agar terpenuhi, mohon hapus komen ini 5 detik setelah dibaca. Trimakasih -->
        <td>
            @Html.TextBox("ID", "1", New With {.class = "form-control"})
        </td>
        <td>
            @Html.TextBox("MtrType", "ASPAL AC-WC", New With {.class = "form-control"})
        </td>
        <td>
            @Html.TextBox("BucketCount", "3", New With {.class = "form-control"})
        </td>
        <td style="vertical-align: middle;white-space: nowrap ! important;">
            @Html.Hidden("ID","0")
            <div class="text-right" style="white-space:nowrap;">
                <a class="btn btn-success btn-xs" href="javascript:void(0)" onclick="cancelEdit(this);" title="save">
                    <i class="icon-save"></i></a>
                <a class="btn btn-primary btn-xs" href="javascript:void(0)" onclick="cancelEdit(this);" title="cancel">
                    <i class="icon-share"></i></a>
            </div>
        </td>
    </tr>
</table>
</div>