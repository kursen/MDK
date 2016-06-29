//*************************
//  Helper: CRUD
//  Created Date: 28 Jumadil Ula 1436 H || Thursday, 19 March, 2015 C
//*************************


var GenTable; // Declare public variable for temporary DataTable


/*================================================================*/

/* set default cofiguration of DataTable
------------------------------------------*/
_attrCRUD = function () {
    var o = {
        "usingId": {
            "tableId": "#tb_Data",
            "formId": "#form-Data",
            "modalId": "#modal-Data",
            "filterBtnId": "#filterBtn"
        },
        "url": {},
        "dataTable": {
            "ajax": {
                "url": "", //(url == null ? "" : parseUrl(url)),
                "data": {}
            },
            //"sPaginationType": "bootstrap",
            "oLanguage": {
                "sLengthMenu": "_MENU_ data per halaman"
            },
            "bSort": true,
            "bPaginate": true,
            "pageLength": 25,
            "columns": [],
            "columnDefs": [],
            "order": [],
            "dom": '<"top">rt<"bottom"p><"clear">'
        }
    }
    return o;
}


/* function for get list of data
and set the action of
default button (Edit,Delete)
------------------------------------------*/
fnGetList = function (attrCRUD, _isExpandable, _strFor) {
    if (attrCRUD.url.Read != undefined) {
        attrCRUD.dataTable.ajax.url = parseUrl(attrCRUD.url.Read);
    } else {
        delete attrCRUD.dataTable.ajax;
        delete attrCRUD.dataTable.columns;
    }
    GenTable = $(attrCRUD.usingId.tableId).DataTable(attrCRUD.dataTable);
    $(attrCRUD.usingId.tableId + ' tbody')
    //for edit button
    .on('click', '.edit2.icon-edit', function () {
        var Data = GenTable.row($(this).parents('tr')).data();
        window.location.href = parseUrl(attrCRUD.url.Update + "?ID=" + Data.ID);
    })
    .on('click', '.icon-edit', function () {
        var Data = GenTable.row($(this).parents('tr')).data();
        if ($(attrCRUD.usingId.formId + " #ID").length == 0) {
            var hiddenID = '<input type="hidden" name="ID" id="ID" />';
            $(attrCRUD.usingId.formId).append(hiddenID);
        }
        fnGetData(attrCRUD.usingId.modalId, Data);
    })
    //for detail button
    .on('click', '.icon-list-alt', function () {
        var Data = GenTable.row($(this).parents('tr')).data();
        //alert(attrCRUD.usingId.tableId);
        fnGetData(attrCRUD.usingId.modalId2, Data);
    })
    //for delete button
    .on('click', '.icon-remove', function () {
        var Data = GenTable.row($(this).parents('tr')).data();
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
            fnDelete(Data.ID, attrCRUD.url.Delete);
        });
    })
    //for lock button in user management
    .on('click', '.icon-lock', function () {
        var Data = GenTable.row($(this).parents('tr')).data();
        //alert(attrCRUD.usingId.tableId);
        fnActivateUser(Data.ID, attrCRUD.url.ActivateUser);
    })
    .on('click', '.icon-unlock', function () {
        var Data = GenTable.row($(this).parents('tr')).data();
        //alert(attrCRUD.usingId.tableId);
        fnActivateUser(Data.ID, attrCRUD.url.ActivateUser);
    });

    //set trigger for refresh table
    /*$('#filter').click(function () {
        $('.loader').removeClass("hidden");
        GenTable.ajax.reload(function () {
            $('.loader').addClass("hidden");
        });
    });*/

    //set trigger for submit
    fnInsert(attrCRUD.usingId.formId, attrCRUD.usingId.modalId);

    //set trigger to reset form when modal opened
    $('[data-toggle="modal"]').click(function () {
        fnReset();
    });

    //insert dataTable object to temporary variable
    var tempTable = [];
    tempTable.push(GenTable);

    //set trigger of #idFilter button to refresh
    $('body').on('click', attrCRUD.usingId.filterBtnId, function () {
        $('.loader').removeClass("hidden");
        tempTable[tempTable.length - 1].ajax.reload(function () {
            $('.loader').addClass("hidden");
        });
    });

    //set dataTable in temporary variable has expandability
    if (_isExpandable) {
        detailRow_add(attrCRUD.usingId.tableId, tempTable[tempTable.length - 1], _strFor);
    }
}

/*================================================================*/

/* function for Insert data
------------------------------------------*/
fnInsert = function (idForm, idModal) {
    var objForm = $(idForm);
    objForm.submit(function (event) {
        event.preventDefault();
        //event.stopPropagation();
        var url = $(this).attr('action');
        data = $(this).serialize();
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: url,
            data: data,
            success: function (get) {
                var err;
                var errmsg = '';
                for (var i = 0; i < get.msg.length; i++) {
                    for (var j = 0; j < get.msg[i].Value.length; j++) {
                        errmsg += '* ' + get.msg[i].Value[j] + '\n';
                    }
                }
                if (get.stat == 0) {
                    swal("", errmsg, "error");
                } else {
                    GenTable.ajax.reload();
                    swal("Tersimpan!", "", "success");
                    if (idModal != null) {
                        $(idModal).modal('hide');
                    }
                }
                return false;
            },
            Error: function (xhr, status, data) {
                swal("Failed!", "Message : " + xhr.status + xhr.statusText + data, "warning");
            }
        });
    });
}

/* function for call data and throw it
into the form
------------------------------------------*/
fnGetData = function (idModal, rowData) {
    $(idModal).modal('show');
    var str = "", obj;
    $.each(rowData, function (index, value) {
        str = "" + value;
        if (str.toLowerCase().indexOf("/date") >= 0) {
            value = formatShortDate(str);
        }
        obj = $("[name='" + index + "']");
        obj.val(value);
        if (obj.hasClass("select2")) {
            var tmp = obj.parent().find(".select2-selection__rendered");
            tmp.text(obj.find("option:selected").text());
            tmp.attr("title", obj.find("option:selected").text());
        }
    });
}

/* function for Delete data
------------------------------------------*/
fnDelete = function (ID, url) {
    $.ajax({
        type: 'POST',
        url: parseUrl(url),
        data: { id: ID },
        dataType: 'json',
        success: function (get) {
            if (get.stat == 1) {
                //alert(get.msg);
                swal("Data Telah terhapus!", "", "success");
                GenTable.ajax.reload();
            } else {
                swal(get.msg, get.msgDesc, "error");
            }
            return false;
        },
        error: function (xhr, status, data) {
            swal("Failed!", "Message : " + xhr.status + xhr.statusText + data, "warning");
        }
    });
}

/* function toggle to activate, lock or unlock user
------------------------------------------*/
fnActivateUser = function (ID, url) {
    $.ajax({
        type: 'POST',
        url: parseUrl(url),
        data: { id: ID },
        dataType: 'json',
        success: function (get) {
            if (get.stat == 1) {
                GenTable.ajax.reload();
            } else {
                swal(get.msg, get.msgDesc, "error");
            }
            return false;
        },
        error: function (xhr, status, data) {
            swal("Failed!", "Message : " + xhr.status + xhr.statusText + data, "warning");
        }
    });
}

//==Untuk Reset Form=====
fnReset = function (formId) {
    //var o = $(_attrCRUD().usingId.formId);
    $("[name='ID']").val("");
    if (formId == undefined)
        var o = document.getElementById("form-Data");
    else
        var o = document.getElementById(formId);
    $.each(o, function (i, v) {
        if ($(v).attr('name') != undefined) {
            //if ($(v).attr('name').toLowerCase().indexOf("date") == 0) {
            $(v).val($(v).prop("defaultValue"));
            //}
            if ($(v).hasClass("select2")) {
                var tmp = $(v).parent().find(".select2-selection__rendered");
                tmp.text('');
                tmp.attr('');
            }
        }
    });
}