//*************************
//    View: /DataMaster/
//*************************


/*--------------------------------------
    WHEN WINDOWS READY TO OPEN
--------------------------------------*/
$(document).ready(function () {

    confMenu();

    GetList_Material();
//    GetList_StockType();
//    GetList_Unit();
//    GetList_Vehicle();
//    GetList_Weigher();
//    GetList_MaterialComposition();
});


/*--------------------------------------
    function for Stack menu
--------------------------------------*/

    confMenu = function () {
        $(window).bind('scroll', function () {
            if ($(window).scrollTop() > 200) {
                //$('.nav.nav-stacked').addClass('nav-fixed');
                $('.nav.nav-stacked').css({ "margin-top": $(window).scrollTop() - 200 });
            }
            else {
                //$('.nav.nav-stacked').removeClass('nav-fixed');
                $('.nav.nav-stacked').removeAttr('style');
            }
        });
    }


/*--------------------------------------
    Call AJAX for list of data
--------------------------------------*/

    GetList_Material = function () {
        $('#tb_Material').dataTable({
            "ajax": parseUrl('DataMaster/GetList_Material'),
            "pageLength": 10,
            "sDom": '<"top"f<"dataTables_add1">>rt<"bottom"<"div-page"ip>><"clear">',
            "columns": [
            { "data": "ID"
              ,"visible" : false},
            { "data": "Kode" },
            { "data": "Nama" },
            { "data": "JenisStok" },
            {
                "className": 'action',
                "orderable": false,
                "data": null, //"3",
                "defaultContent": '' +
                '<div class="text-right" style="white-space:nowrap;">' +
                  '<a class="btn btn-primary btn-xs" href="javascript:GetInfo(this)" title="edit">' +
                  '    <i class="icon-edit"></i></a>' +
                  '<a class="btn btn-danger btn-xs" href="javascript:void(0)" title="remove">' +
                  '    <i class="icon-remove"></i></a>' +
                '</div>'
            },
        ]
        });

        var btnAddNew = '<a data-toggle="modal" id="0" data-target="#form-material" class="btn btn-white" href="javascript:void(0)">' +
						    '<i class="icon-plus btn btn-success"></i> Komposisi Baru' +
			            '</a>'
        $("div.dataTables_add1").html(btnAddNew);
    }

    function tesEdit() {
        alert("TES");
    }
    $('#form-material').on('show.bs.modal', function (e) {
        alert($(this).data('target'));
////        if (!data) return e.preventDefault() // stops modal from being shown
    })
    function GetInfo(row) {
        var string_val = $(row).children('td').eq(0).html();
        alert(string_val);
    }