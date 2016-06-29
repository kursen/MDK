//*************************
//    View: /Stock/
//*************************


/*--------------------------------------
WHEN WINDOWS READY TO OPEN
--------------------------------------*/
$(document).ready(function () {
    datePicker_ByMonth($('#datetimepicker'));

    var d = new Date();
    var curDate = formatCustomDate(d, "MMMM YYYY");

    $('#datetimepicker input').val(curDate);

    FillTable();
    $('#filter').click(function () {
        FillTable();
    });
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

/*--------------------------------------
Call AJAX for list of data
--------------------------------------*/

FillTable = function () {
    var dateSplit = splitMonYear($('#datetimepicker input').val());
    var url = "/Stock/GetInventoryData?IdMaterial=" + $("#MaterialList").val() + "&monthData=" + dateSplit[0] + "&yearData=" + dateSplit[1];
    $('.loader').removeClass("hidden");
    $.post(url, function (data) {
        var StrHTML = "";
        var rowno;
        if (data.length == 0) {
            StrHTML += "<tr><td colspan='6' class='td-center'>Tidak ada data.</td></tr>";
        }
        else {
            for (var i = 0; i < data.length; i++) {
                switch (data[i].UrutBaris) {
                    case 0:
                        StrHTML += "<tr><td colspan='5'><b>Saldo sebelumnya</b></td>"
                        StrHTML += "<td class='text-right'><b>" + numberFormat(data[i].Sisa) + "</b></td></tr>"
                        break;
                    case 1:
                        StrHTML += "<tr><td class='text-center'>" + formatCustomDate(data[i].Tanggal, "DD") + "</td>"
                        StrHTML += "<td class='text-center'>" + data[i].Status + "</td>"
                        StrHTML += "<td class='text-left'>" + data[i].Keterangan + "</td>"
                        StrHTML += "<td class='text-right'>" + numberFormat(data[i].Tambah) + "</td>"
                        StrHTML += "<td class='text-right'>" + numberFormat(data[i].Kurang) + "</td>"
                        StrHTML += "<td class='text-right'>" + numberFormat(data[i].Sisa) + "</td>"
                        //                    StrHTML += "<td class='action'>"
                        //                    StrHTML += "<div class='text-right' style='white-space:nowrap;'>"
                        //                    StrHTML += "<a class='btn btn-warning btn-xs' href='javascript:void(0)' data-toggle='modal' data-target='#modal-detail' title='detail'><i class='icon-list-alt'></i></a>"
                        //                    StrHTML += "</div>"
                        //                    StrHTML += "</td>"
                        StrHTML += "</tr>"
                        break;
                    case 2:
                        StrHTML += "<tr><td colspan='5'><b>Saldo terakhir</b></td>"
                        StrHTML += "<td class='text-right'><b>" + numberFormat(data[i].Sisa) + "</b></td></tr>"
                        break;
                }
            }
        }
        $("#tb_stock tbody").html(StrHTML);
        $('.loader').addClass("hidden");
        groupTable($('#tb_stock tr:has(td)'), 0, 3);
    });
}