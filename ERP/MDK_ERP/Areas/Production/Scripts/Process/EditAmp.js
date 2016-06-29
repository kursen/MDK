$(document).ready(function () {

    datePicker_ByDate('#dtpDateUse', true);
    datetimePickerLinked_ByTime("#BeginTime", "#EndTime");



    var valIDMaterial = $('#IDMaterial').val();
    var idMachine = $("#IdMachine").val();
    var op = $('#Operator').val();
    var IdProject = $('#IDProject').val();
});