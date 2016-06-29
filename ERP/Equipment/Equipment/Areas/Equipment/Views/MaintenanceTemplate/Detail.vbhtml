@ModelType Equipment.MaintenanceTemplateItem
@Code
    ViewData("Title") = "Maintenance Template"
    Html.SetEditableDefaultSettings(DisableOnload:=True)
End Code
@Using Html.BeginJUIBox("Detail Alat Peralatan Mesin", False, False, False, False, False, "fa fa-table")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <br />
           <div class="col-lg-offset-8 col-sm-offset-8">
                <button type="button" class="btn btn-danger btn-label-left" onclick="$('#dProjectInfo .editable') .editable('toggleDisabled')">
                <span><i class="fa fa-edit"></i></span>Edit</button>
                <a href="@Url.Action("Index", "MaintenanceTemplate")" class="btn btn-default btn-label-left">
                    Kembali</a>
            </div>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-5 col-sm-5">
            <dl class="dl-horizontal" id='dProjectInfo'>
                <dt>Item</dt>
                <dd>@Html.EditableInputTextBox("Item", Model.Item, "text", "Item", datapk:=Model.ID, dataurl:="/MaterialTemplate/SavePartial")
                    &nbsp;</dd>
                <dt>Nilai</dt>
                <dd>@Html.EditableInputTextBox("Value", Model.Value, "text", "Value", datapk:=Model.ID, dataurl:="/MaterialTemplate/SavePartial")
                    &nbsp;</dd>
                <dt>Satuan</dt>
                <dd>@Html.EditableInputTextBox("Unit", Model.Unit, "text", "Satuan", datapk:=Model.ID, dataurl:="/MaterialTemplate/SavePartial")
                    &nbsp;</dd>
                <dt>Tipe Mesin</dt>
                <dd>@Html.EditableInputTextBox("MachineTypeID", Model.MachineTypeID, "text", "Tipe Mesin", datapk:=Model.ID, dataurl:="/MaterialTemplate/SavePartial")
                    &nbsp;</dd>
            </dl>
        </div>
    </div>
    @<br />
End Using
<script type="text/javascript">

    var _savepartialresponse = function (data) {

        if ((data) && (data.stat == 1)) {

        } else {
            showNotification(data);
        };

    }
    </script>

