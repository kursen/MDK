//*************************
//    View: /Dashboard
//*************************
$(document).ready(function () {
    FillTable();
});

function groupTable($rows, startIndex, total) {
    if (total === 0) {
        return;
    }
    var i, currentIndex = startIndex, count = 1, lst = [];
    var tds = $rows.find('td:eq(' + currentIndex + ')');
    var ctrl = $(tds[0]);
    lst.push($rows[0]);
    for (i = 1; i <= tds.length; i++) {
        if (ctrl.text() == $(tds[i]).text()) {
            count++;
            $(tds[i]).addClass('hide');
            lst.push($rows[i]);
        }
        else {
            if (count > 1) {
                ctrl.attr('rowspan', count);
                groupTable($(lst), startIndex + 1, total - 1)
            }
            count = 1;
            lst = [];
            ctrl = $(tds[i]);
            lst.push($rows[i]);
        }
    }
}

FillTable = function () {
    //AMP Data
    var url = parseUrl("Production/Dashboard/GetAMPData");
    $.post(url, function (data) {
        var StrHTML = "";
        var rowno;
        if (data.length == 0) {
            StrHTML += "<tr><td colspan='4' class='td-center'>Tidak ada data.</td></tr>";
        }
        else {
            for (var i = 0; i < data.length; i++) {
                StrHTML += "<tr><td>" + data[i].MaterialProduction + "</td>"
                StrHTML += "<td class='text-right'>" + data[i].AmountProduction + "</td>"
                StrHTML += "<td>" + data[i].MaterialUse + "</td>"
                StrHTML += "<td class='text-right'>" + data[i].AmountUse + "</td>"
                StrHTML += "</tr>"
            }
        }
        $("#tbAmp tbody").html(StrHTML);
        groupTable($('#tbAmp tr:has(td)'), 0, 2);
    });
    //InventoryData
    url = parseUrl("Production/Dashboard/GetInventoryData");
    $.post(url, function (data) {
        var StrHTML = "";
        var rowno;
        if (data.length == 0) {
            StrHTML += "<tr><td colspan='5' class='td-center'>Tidak ada data.</td></tr>";
        }
        else {
            for (var i = 0; i < data.length; i++) {
                StrHTML += "<tr>"
                StrHTML += "<td>" + data[i].NamaMaterial + "</td>"
                StrHTML += "<td style='text-align: right'>" + numberFormat(data[i].SisaKemarin) + "</td>"
                StrHTML += "<td style='text-align: right'>" + numberFormat(data[i].TambahSekarang) +"</td>"
                StrHTML += "<td style='text-align: right'>" + numberFormat(data[i].KurangSekarang) + "</td>"
                StrHTML += "<td style='text-align: right'>" + numberFormat(data[i].Sisa) + "</td>"
                StrHTML += "</tr>"
            }
        }
        $("#tbInventory tbody").html(StrHTML);
//        groupTable($('#tb_stock tr:has(td)'), 0, 3);
    });
}
