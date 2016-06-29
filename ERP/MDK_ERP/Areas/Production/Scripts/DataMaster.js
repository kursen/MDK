//*************************
//    View: /DataMaster/
//*************************


/*--------------------------------------
    WHEN WINDOWS READY TO OPEN
--------------------------------------*/
$(document).ready(function () {
   
    confMenu();
    GetList_Material();
    GetList_StockType();
    //GetList_Unit();
    GetList_Vehicle();
    GetList_Weigher();
    GetList_MaterialComposition();
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
    Gak Pake Ajax
    --------------------------------------*/
   

    //===============================================


    GetList_Material = function () {
        $('#tb_Material').dataTable({
            "ajax": parseUrl('DataSample/dataSample?isGroup=true'),
            "pageLength": 10,
            "sDom": '<"top"f<"dataTables_add1">>rt<"bottom"<"div-page"ip>><"clear">',
            "columns": [
//            { "data": "3" },
            { "data": "0" },
            { "data": "1" },
            { "data": "7" },
            {
                "className": 'action',
                "orderable": false,
                "data": null, //"3",
                "defaultContent": '' +
                '<div class="text-right" style="white-space:nowrap;">' +
                  '<a id="Edit" class="btn btn-primary btn-xs" href="javascript:void(0)" title="edit">' +
                  '    <i class="icon-edit"></i></a>' +
                  '<a id="Delete" class="btn btn-danger btn-xs" href="javascript:void(0)" title="remove">' +
                  '    <i class="icon-remove"></i></a>' +
                '</div>'
            },
        ]
        });

        var btnAddNew = '<a data-toggle="modal" id="" data-target="#form-material" class="btn btn-default" href="javascript:void(0)">' +
						    '<i class="icon-plus btn btn-success"></i> Komposisi Baru' +
			            '</a>'
        $("div.dataTables_add1").html(btnAddNew);
    }

    GetList_MaterialComposition = function () {
        $('#tb_MaterialComposition').DataTable({
            "ajax": parseUrl('DataSample/dataSampleMaterialComposition'),
            "pageLength": 5,
            "sDom": '<"top"<"dataTables_add6">>rt<"bottom"<"div-page">><"clear">',
            "columns": [
            { "data": "0" },
            { "data": "1" },
            { "data": "2" },
            {
                "className": 'action',
                "orderable": false,
                "data": null,//"3",
                "defaultContent": '' +
                '<div class="text-right" style="white-space:nowrap;">' +
                  '<a class="btn btn-primary btn-xs" href="javascript:void(0)" onclick="editRow(\'\',this);" title="edit">' +
                  '    <i class="icon-edit"></i></a>' +
                  '<a class="btn btn-danger btn-xs" href="javascript:void(0)" onclick="removeRow(\'\',this);" title="remove">' +
                  '    <i class="icon-remove"></i></a>' +
                '</div>'
            },
        ]
        });

        var btnAddNew = '<a data-toggle="modal" id="" data-target="#form-materialcomposition" class="btn btn-default" href="javascript:void(0)">' +
						    '<i class="icon-plus btn btn-success"></i> Tambah Baru' +
			            '</a>'
        $("div.dataTables_add6").html(btnAddNew);
    }

   

    GetList_StockType = function () {
        $('#tb_stockType').dataTable({
            "ajax": parseUrl('DataSample/dataSampleST'),
            "pageLength": 5,
            "sDom": '<"top"<"dataTables_add2">>rt<"bottom"<"div-page">><"clear">',
            "columns": [
            { "data": "1" }
        ]
        });

        var btnAddNew = '<a data-toggle="modal" id="" data-target="#form-stockType" class="btn btn-default" href="javascript:void(0)">' +
						    '<i class="icon-plus btn btn-success"></i> Tambah Baru' +
			            '</a>'
        $("div.dataTables_add2").html(btnAddNew);
    }

  

    GetList_Vehicle = function () {
        $('#tb_Vehicle').dataTable({
            "ajax": parseUrl('DataSample/dataSampleKK'),
            "pageLength": 5,
            "sDom": '<"top"<"dataTables_add4">>rt<"bottom"<"div-page">><"clear">',
            "columns": [
                { "data": "0" },
                { "data": "1" }
            ]
        });

        var btnAddNew = '<a data-toggle="modal" id="" data-target="#form-vehicle" class="btn btn-default" href="javascript:void(0)">' +
						    '<i class="icon-plus btn btn-success"></i> Tambah Baru' +
			            '</a>'
        $("div.dataTables_add4").html(btnAddNew);
    }

    GetList_Weigher = function () {
        $('#tb_Weigher').dataTable({
            "ajax": parseUrl('DataSample/dataSampleTT'),
            "pageLength": 5,
            "sDom": '<"top"<"dataTables_add5">>rt<"bottom"<"div-page">><"clear">',
            "columns": [
            { "data": "0" },
            { "data": "1" }
        ]
        });

        var btnAddNew = '<a data-toggle="modal" id="" data-target="#form-weigher" class="btn btn-default" href="javascript:void(0)">' +
						    '<i class="icon-plus btn btn-success"></i> Tambah Baru' +
			            '</a>'
        $("div.dataTables_add5").html(btnAddNew);
    }

  

    reload_table_unit = function () {
        var table=$('#tb_Unit').dataTable({
            "ajax": parseUrl('DataSample/dataSampleTT')
        });
        setInterval(function () {
            table.ajax.reload();
        }, 30000);
    }

   