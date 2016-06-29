@ModelType Equipment.MachineEqp
@Code
    ViewData("Title") = "Detail Alat Peralatan Mesin"
    Html.SetEditableDefaultSettings(DisableOnload:=True)
    
    Dim roles = "Equipment.Supervisor, Equipment.Manager, Equipment.DataOperator".Split(",")
    Dim isProperUser = ERPBase.ErpAuthorization.UserInAnyRoles(roles, User)
End Code
@Using Html.BeginJUIBox("Detail Alat Peralatan Mesin", False, False, False, False, False, "fa fa-table")

    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <br />
            <div class="col-lg-offset-8 col-sm-offset-8">
                @If isProperUser Then
        @<button type="button" class="btn btn-danger btn-label-left" onclick="$('#dProjectInfo .editable') .editable('toggleDisabled')">
                    <span><i class="fa fa-edit"></i></span>Edit</button>
    End If
                
                <a href="@Url.Action("Index", "EquipmentList")" class="btn btn-default btn-label-left">
                    Kembali</a>
            </div>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-5 col-sm-5">
            <dl class="dl-horizontal" id='dProjectInfo'>
                <dt>Kode </dt>
                <dd>@Html.EditableInputTextBox("Name", Model.Name, "text", "Nama", datapk:=Model.ID, dataurl:="/EquipmentList/SavePartial")
                    &nbsp;</dd>
                <dt>Merk</dt>
                <dd>@Html.EditableInputTextBox("Merk", Model.Merk, "text", "Merk", datapk:=Model.ID, dataurl:="/EquipmentList/SavePartial")
                    &nbsp;</dd>
                <dt>Tipe</dt>
                <dd>@Html.EditableInputTextBox("Type", Model.Type, "text", "Tipe", datapk:=Model.ID, dataurl:="/EquipmentList/SavePartial")
                    &nbsp;</dd>
                <dt>Harga</dt>
                <dd>@Html.EditableInputTextBox("Cost", Model.Cost.ToString("#,###,##0"), "text", "Harga", datapk:=Model.ID, dataurl:="/EquipmentList/SavePartial")
                    &nbsp;</dd>
                <dt>Nomor Seri</dt>
                <dd>@Html.EditableInputTextBox("SerialNumber", Model.SerialNumber, "text", "Nomor Seri", datapk:=Model.ID, dataurl:="/EquipmentList/SavePartial")
                    &nbsp;</dd>
                <dt>Kapasitas</dt>
                <dd>@Html.EditableInputTextBox("Capacity", Model.Capacity, "text", "Kapasitas", datapk:=Model.ID, dataurl:="/EquipmentList/SavePartial")
                    &nbsp;</dd>
                <dt>Tahun Pembelian</dt>
                <dd>@Html.EditableInputTextBox("Year", Model.BuyYear, "text", "Tahun Pembelian", datapk:=Model.ID, dataurl:="/EquipmentList/SavePartial")
                    &nbsp;</dd>
                <dt>Tahun Pembuatan</dt>
                <dd>@Html.EditableInputTextBox("BuiltYear", Model.MadeYear, "text", "Tahun Pembuatan", datapk:=Model.ID, dataurl:="/EquipmentList/SavePartial")
                    &nbsp;</dd>
                <dt>Wilayah</dt>
                <dd>
                    @*@Html.EditableInputTextBox("IDArea", ViewData("area"), "select", "Pilih Unit", datapk:=Model.ID, dataurl:="/EquipmentList/SavePartial")*@
                    <a href="#" id="IDArea" name="IDArea" class="officename" data-type="select" data-pk='@Model.ID' data-url="/EquipmentList/SavePartial" data-title="Pilih Unit Kerja">@ViewData("area")</a>
                </dd>
                <dt>Keterangan</dt>
                <dd>@Html.EditableInputTextBox("Remarks", Model.Remark, "textarea", "Keterangan", datapk:=Model.ID, dataurl:="/EquipmentList/SavePartial")
                    &nbsp;</dd>
            </dl>
        </div>
    </div>
    @<br />
End Using
<script type="text/javascript">

    var initEditable = function (arrsource) {
        $(".officename").editable({
            source: arrsource,
            success: function () {
                showNotification("Data tersimpan.");

            }
        });
    }

    var _savepartialresponse = function (data) {

        if ((data) && (data.stat == 1)) {

        } else {
            showNotification(data);
        };

    }
    $(function () {

        $.post('/Equipment/EquipmentList/getOfficeId', function (data, status) {
            initEditable(data.officelist);
        });
    });
    
</script>
