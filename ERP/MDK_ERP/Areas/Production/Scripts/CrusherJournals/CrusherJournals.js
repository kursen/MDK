//*************************
//    View: Production/Project/Index
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
    $('#print').click(function () {
        var dateSplit = splitMonYear($('#datetimepicker input').val());
        var url = parseUrl("Production/CrusherJournals/PrintReportXls?dataMonth=" + dateSplit[0] + "&dataYear=" + dateSplit[1]);
        window.location.href = url;
    });
});

function groupTable($rows, startIndex, total) {
    if (total === 0) {
        return;
    }
    var i, currentIndex = startIndex, count = 1, lst = [];
    var tds = $rows.find('td:eq(' + currentIndex + ')');
    var tdAction = $rows.find('td:last-child'); //custom code
    var ctrl = $(tds[0]);
    var ctrlAction = $(tdAction[0]); //custom code
    lst.push($rows[0]);
    for (i = 1; i <= tds.length; i++) {
        if (ctrl.text() == $(tds[i]).text()) {
            count++;
            $(tds[i]).addClass('hide');
            $(tdAction[i]).addClass('hide'); // custom code

            lst.push($rows[i]);
        }
        else {
            if (count > 1) {
                ctrl.attr('rowspan', count);
                ctrlAction.attr('rowspan', count); //custom code
                groupTable($(lst), startIndex + 1, total - 1)
            }
            count = 1;
            lst = [];
            ctrl = $(tds[i]);
            ctrlAction = $(tdAction[i]); //custom code
            lst.push($rows[i]);
        }
    }
    $(".action[rowspan]").removeClass('hide'); // custom code
}

FillTable = function () {
    var dateSplit = splitMonYear($('#datetimepicker input').val());
    var url = "/CrusherJournals/GetList?dataMonth=" + dateSplit[0] + "&dataYear=" + dateSplit[1];
    $('.loader').removeClass("hidden");
    $.post(url, function (data) {
        var StrHTML = "";
        var rowno;
        var totalGrogol = 0;
        var totalGresleyIn = 0;
        var totalMedium = 0;
        var totalAbuBatu = 0;
        var totalBaseA = 0;
        var totalBaseB = 0;
        var totalSplit12 = 0;
        var totalSplit23 = 0;
        var totalGresleyOut = 0;
        if (data.data.length == 0) {
            StrHTML += "<tr><td colspan='17' class='td-center'>Tidak ada data.</td></tr>";
        }
        else {
            var rowNum = 1;
            var rowID = data.data.length > 0 ? data.data[0].IDJournal : 0;
            for (var i = 0; i < data.data.length; i++) {
                if (rowID != data.data[i].IDJournal) {
                    rowNum++;
                    rowID = data.data[i].IDJournal;
                }
                totalGrogol += data.data[i].GROGOL;
                totalGresleyIn += data.data[i].GRESLEYIn;
                totalMedium += data.data[i].MEDIUM;
                totalAbuBatu += data.data[i].ABUBATU;
                totalBaseA += data.data[i].BASEA;
                totalBaseB += data.data[i].BASEB;
                totalSplit12 += data.data[i].SPLIT12;
                totalSplit23 += data.data[i].SPLIT23;
                totalGresleyOut += data.data[i].GRESLEYOut;
                StrHTML += "<tr><td class='td-center'>" + rowNum + "</td>";
                StrHTML += "<td class='td-center'>" + formatCustomDate(data.data[i].Tanggal, "DD-MMM-YY") + "</td>";

                if (data.data[i].GROGOL > 0) {
                    StrHTML += "<td class='td-center' style='background-color: #00FF00'>" + (data.data[i].GROGOL > 0 ? numberFormat(data.data[i].GROGOL) : "") + "</td>"
                } else {
                    StrHTML += "<td class='td-center'></td>"
                }
                if (data.data[i].GRESLEYIn > 0) {
                    StrHTML += "<td class='td-center' style='background-color: #00FF00'>" + (data.data[i].GRESLEYIn > 0 ? numberFormat(data.data[i].GRESLEYIn) : "") + "</td>"
                } else {
                    StrHTML += "<td class='td-center'></td>"
                }
                if (data.data[i].MEDIUM > 0) {
                    StrHTML += "<td class='td-center' style='background-color: #00FF00'>" + numberFormat(data.data[i].MEDIUM) + "</td>"
                } else {
                    StrHTML += "<td class='td-center'></td>"
                }
                if (data.data[i].ABUBATU > 0) {
                    StrHTML += "<td class='td-center' style='background-color: #00FF00'>" + numberFormat(data.data[i].ABUBATU) + "</td>"
                } else {
                    StrHTML += "<td class='td-center'></td>"
                }
                if (data.data[i].BASEA > 0) {
                    StrHTML += "<td class='td-center' style='background-color: #00FF00'>" + numberFormat(data.data[i].BASEA) + "</td>"
                } else {
                    StrHTML += "<td class='td-center'></td>"
                }
                if (data.data[i].BASEB > 0) {
                    StrHTML += "<td class='td-center' style='background-color: #00FF00'>" + numberFormat(data.data[i].BASEB) + "</td>"
                } else {
                    StrHTML += "<td class='td-center'></td>"
                }
                if (data.data[i].SPLIT12 > 0) {
                    StrHTML += "<td class='td-center' style='background-color: #00FF00'>" + numberFormat(data.data[i].SPLIT12) + "</td>"
                } else {
                    StrHTML += "<td class='td-center'></td>"
                }
                if (data.data[i].SPLIT23 > 0) {
                    StrHTML += "<td class='td-center' style='background-color: #00FF00'>" + numberFormat(data.data[i].SPLIT23) + "</td>"
                } else {
                    StrHTML += "<td class='td-center'></td>"
                }
                if (data.data[i].GRESLEYOut > 0) {
                    StrHTML += "<td class='td-center' style='background-color: #00FF00'>" + (data.data[i].GRESLEYOut > 0 ? numberFormat(data.data[i].GRESLEYOut) : "") + "</td>"
                } else {
                    StrHTML += "<td class='td-center'></td>"
                }
                StrHTML += "<td class='td-center'>" + data.data[i].SatuanBucket + "</td>";
                StrHTML += "<td class='td-center'>" + data.data[i].M3 + "</td>";
                StrHTML += "<td class='td-center'>" + numberFormat(data.data[i].JumlahKubik) + "</td>";
                StrHTML += "<td class='td-center'>" + data.data[i].Keterangan + "</td>";
                StrHTML += "<td class='td-center'></td>"
                StrHTML += "<td class='action' style='min-width:70px;'>" +
                    "<div align='left'>" +
                    "  <button class='icon-edit btn btn-primary btn-xs' data-toggle='tooltip' data-placement='bottom' title='Edit'></button>" +
                    "<input type='hidden' class='idjournal' name='IDJournal' id='IDJournal' value='" + data.data[i].IDJournal + "'/>" +
                    "  <button class='icon-remove btn btn-danger btn-xs' data-toggle='tooltip' data-placement='right' title='Hapus'></button>" +
                    "</div>" +
                "</td>";
                StrHTML += "</tr>"
            }
        }
        $("#tb_Data tbody").html(StrHTML);
        $('.icon-edit').click(function () {
            var IDJournal = $(this).siblings('.idjournal').val();
            var urlEdit = parseUrl('Production/CrusherJournals/EditCrusher');
            window.location.href = urlEdit + "?ID=" + IDJournal;
        });
        $('.icon-remove').click(function () {
            var IDJournal = $(this).siblings('.idjournal').val();
            swal({
                title: "Apakah Anda yakin ingin menghapus?",
                text: "Anda tidak akan dapat memulihkan data yang telah terhapus!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Ya",
                cancelButtonText: "Tidak",
                closeOnConfirm: false
            },
            function () {
                var url = '/Production/CrusherJournals/Delete'
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: url,
                    data: { "id": IDJournal },
                    success: function (get) {
                        if (get.stat == 1) {
                            window.location.reload(true);
                        } else {
                            swal("Failed!", "Message : " + get.msg, "error");
                        }
                        return false;
                    },
                    error: function (xhr, status, data) {
                        swal("Failed!", "Message : " + xhr.status + xhr.statusText + data, "error");
                    }
                });
            });
        });
        var totalSatuan = totalGrogol + totalMedium + totalAbuBatu + totalBaseA + totalBaseB + totalSplit12 + totalSplit23;
        $("#totalGrogol").html(isNull(totalGrogol, ''));
        $("#totalGresleyIn").html(isNull(totalGresleyIn, ''));
        $("#totalMedium").html(isNull(totalMedium, ''));
        $("#totalAbuBatu").html(isNull(totalAbuBatu, ''));
        $("#totalBaseA").html(isNull(totalBaseA, ''));
        $("#totalBaseB").html(isNull(totalBaseB, ''));
        $("#totalSplit12").html(isNull(totalSplit12, ''));
        $("#totalSplit23").html(isNull(totalSplit23, ''));
        $("#totalGresleyOut").html(isNull(totalGresleyOut, ''));
        $("#totalSatuan").html(isNull(totalSatuan, ''));
        $("#totalJumlah").html(isNull(totalSatuan * 3, ''));

        $("#totalM3Grogol").html(isNull(totalGrogol * 3, ''));
        $("#totalM3GresleyIn").html(isNull(totalGresleyIn * 3, ''));
        $("#totalM3Medium").html(isNull(totalMedium * 3, ''));
        $("#totalM3AbuBatu").html(isNull(totalAbuBatu * 3, ''));
        $("#totalM3BaseA").html(isNull(totalBaseA * 3, ''));
        $("#totalM3BaseB").html(isNull(totalBaseB * 3, ''));
        $("#totalM3Split12").html(isNull(totalSplit12 * 3, ''));
        $("#totalM3Split23").html(isNull(totalSplit23 * 3, ''));
        $("#totalM3GresleyOut").html(isNull(totalGresleyOut * 3, ''));
        $('.loader').addClass("hidden");
        groupTable($('#tb_Data tr:has(td)'), 0, 2);
    });
}

