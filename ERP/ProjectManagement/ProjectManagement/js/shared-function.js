//*************************
//    Shared Function
//*************************


///*--------------------------------------
//WHEN WINDOWS READY TO OPEN
//--------------------------------------*/
$(document).ready(function () {
    $(".select2-selection").addClass("form-control");

    $(".validation-summary-errors").addClass("alert alert-danger");
    moment.locale('id');
});

/** Datetimepicker **/
//Doc: http://eonasdan.github.io/bootstrap-datetimepicker/

//format Date 1
formatCompleteDate = function (val) {
    return moment(val).format("DD-MM-YYYY HH:mm");
}

//format Date 2
formatShortDate = function (val) {
    if (val != "")
        return moment(val).format("DD-MM-YYYY");
    var dt = new Date();
    return moment(dt).format("DD-MM-YYYY");
}

//format Date 3
formatCustomDate = function (val, strFormat) {
    return moment(val).format(strFormat);
}

//for filter datepicker by date
datePicker_ByDate = function (obj, currentDate) {
    var value = "";
    var objIn = $(obj).find("input");
    value = formatShortDate($(objIn).val());
    //$(objIn).val(value);
    //document.getElementById($(objIn).attr('id')).defaultValue = value;
    $(obj).datetimepicker({
        format: 'DD-MM-YYYY',
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down",
            previous: 'fa fa-chevron-left',
            next: 'fa fa-chevron-right',
            today: 'fa fa-screenshot',
            clear: 'fa fa-trash'
        },
        locale: 'id'
    });
}

//for filter dateTimepicker by date
datetimePicker_ByDate = function (obj, currentDate) {
    $(obj).datetimepicker({
        format: 'DD-MM-YYYY HH:mm',
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down",
            previous: 'fa fa-chevron-left',
            next: 'fa fa-chevron-right',
            today: 'fa fa-screenshot',
            clear: 'fa fa-trash'
        },
        locale: 'id'
    });
    if (currentDate) {
        var dt = new Date();
        var c = formatCompleteDate(dt);
        $(obj).find("input").val(c);
    }
}

//for filter monthpicker
datePicker_ByMonth = function (obj) {
    $(obj).datetimepicker({
        viewMode: 'months',
        format: "MMMM YYYY",
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down",
            previous: 'fa fa-chevron-left',
            next: 'fa fa-chevron-right',
            today: 'fa fa-screenshot',
            clear: 'fa fa-trash'
        },
        locale: 'id'
    });

    var dt = new Date();
    var c = moment(dt).format("MMMM YYYY");
    $(obj).find("input").val(c);
}

//for filter linked dtpicker by date
datePickerLinked_ByDate = function (obj1, obj2) {
    datePicker_ByDate(obj1);
    datePicker_ByDate(obj2);
    $(obj1).on("dp.change", function (e) {
        $(obj2).data("DateTimePicker").minDate(e.date);
    });
    $(obj2).on("dp.change", function (e) {
        $(obj1).data("DateTimePicker").maxDate(e.date);
    });

    var dt = new Date();
    var c = formatShortDate(dt);
    if ($(obj1).find("input").val() == "")
        $(obj1).find("input").val(c);
    if ($(obj2).find("input").val() == "")
        $(obj2).find("input").val(c);
}

//for filter linked dtpicker by date
datetimePickerLinked_ByDate = function (obj1, obj2) {
    datetimePicker_ByDate(obj1);
    datetimePicker_ByDate(obj2);
    $(obj1).on("dp.change", function (e) {
        $(obj2).data("DateTimePicker").minDate(e.date);
    });
    $(obj2).on("dp.change", function (e) {
        $(obj1).data("DateTimePicker").maxDate(e.date);
    });

    var dt = new Date();
    var c = formatCompleteDate(dt);
    if ($(obj1).find("input").val() == "")
        $(obj1).find("input").val(c);
    if ($(obj2).find("input").val() == "")
        $(obj2).find("input").val(c);
}

//for filter monthpicker
timePicker = function (obj) {
    $(obj).datetimepicker({
        format: "HH:mm",
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down",
            previous: 'fa fa-chevron-left',
            next: 'fa fa-chevron-right',
            today: 'fa fa-screenshot',
            clear: 'fa fa-trash'
        },
        locale: 'id'
    });

    var dt = new Date();
    var c = moment(dt).format("HH:mm");
    $(obj).find("input").val(c);
}

//for split date data
splitMonYear = function (str) {
    var arr = str.split(" ");
    arrMon = { "Januari": 0, "Februari": 1, "Maret": 2, "April": 3, "Mei": 4, "Juni": 5, "Juli": 6, "Agustus": 7, "September": 8, "Oktober": 9, "November": 10, "Desember": 11 };
    arr[0] = arrMon[arr[0]] + 1;
    arr[1] = parseInt(arr[1]);
    return arr
}

//number format
numberFormat = function (nr) {
    var str = nr + '';
    x = str.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? ',' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + '.' + '$2');
    }
    return (x1 + x2);
}

/** Other **/

//set value if null
function isNull(val, m) {
    if (m != undefined || m != null)
        return val == null || val == 0 ? m : val;
    return val == null ? '-' : val;
}

