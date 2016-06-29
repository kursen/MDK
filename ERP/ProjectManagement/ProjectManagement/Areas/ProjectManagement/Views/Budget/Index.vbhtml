@ModelType ProjectManagement.ProjectInfo
@code
    ViewData("Title") = "Informasi Proyek"
End Code
@Html.Partial("ProjectPageMenu", Model)
@Using Html.BeginJUIBox("Rencana Angggaran Fisik Proyek")
    @<div style="min-height: 400px;">
        <div id='divList'>
            <div class="row">
                <div class="col-lg-12 col-sm-12">
                    <div class="pull-right">
                        <button type="button" class="btn btn-danger btn-label-left" id="btnPrint">
                            <span><i class="fa fa-print"></i></span>Print</button>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 col-lg-12">
				<div class="table-responsive">
                    <table class="table table-bordered table-heading" width="100%" style="table-layout: fixed"
                        id="tblBudget">
                        <colgroup>
                            <col style="width: 80px" />
                            <col style="width: auto" />
                            <col style="width: 80px" />
                            <col style="width: 80px" />
                            <col style="width: 120px" />
                            <col style="width: 150px" />
                            <col style="width: 100px" />
                            <col style="width: 120px" />
                        </colgroup>
                        <thead>
                            <tr>
                                <th class="text-center">
                                    No. Mata Pembayaran
                                </th>
                                <th class="text-center">
                                    Uraian
                                </th>
                                <th class="text-center">
                                    Satuan
                                </th>
                                <th class="text-center">
                                    Perkiraan Kuantitas Kontrak
                                </th>
                                <th class="text-center">
                                    Harga Dasar (Rupiah)
                                </th>
                                <th class="text-center">
                                    Harga Dasar Pekerjaan (Rupiah)
                                </th>
                                <th class="text-center">
                                    Waktu Pelaksanaan (hari)
                                </th>
                                <th class='editColumn'>
                                    &nbsp;
                                </th>
                            </tr>
                            <tr class="text-center">
                                <th class="text-center">
                                    a
                                </th>
                                <th class="text-center">
                                    b
                                </th>
                                <th class="text-center">
                                    c
                                </th>
                                <th class="text-center">
                                    d
                                </th>
                                <th class="text-center">
                                    e
                                </th>
                                <th class="text-center">
                                    f=(dxe)
                                </th>
                                <th class="text-center">
                                    g
                                </th>
                                <th class="editColumn">
                                    @If Model.Archive = False Then

                                        @<button type="button" class="btn btn-danger btn-label-left btnAdd" id='btnAdd'>
                                            <span><i class="fa fa-plus"></i></span>Tambah</button>
    End If
                                </th>
                            </tr>
                        </thead>
                        <tfoot style='font-weight: bold'>
                            <tr>
                                <td colspan="5" class="text-right">
                                    Total Rencana Anggaran Proyek Rp.
                                </td>
                                <td class="text-right" id="totalrap">
                                </td>
                                <td class="text-right">
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" class="text-right">
                                    PPn 10% Rp.
                                </td>
                                <td class="text-right" id="ppn">
                                </td>
                                <td class="text-right">
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" class="text-right">
                                    Anggaran Proyek Termasuk PPn Rp.
                                </td>
                                <td class="text-right" id="rapppn">
                                </td>
                                <td class="text-right">
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7"></td>
                          <td>                                    
                          @If Model.Archive = False Then

                                        @<button type="button" class="btn btn-danger btn-label-left btnAdd" >
                                            <span><i class="fa fa-plus"></i></span>Tambah</button>
                          End If
                          </td>
                            </tr>
                        </tfoot>
                    </table>
					</div>
                </div>
            </div>
        </div>
        <div id='divForm' style='display: none'>
            @Using Html.BeginForm("SaveDivision", "Budget", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmDivision"})
    
                @Html.Hidden("ProjectInfoId", Model.Id)
                @Html.Hidden("Ordinal", 0)
                @<input type="hidden" id="DivisionId" name="id" value="0" />
                @Html.WriteFormInput(Html.TextBox("DivisionNumber", String.Empty,
                                                  New With {.class = "form-control", .maxlength = 10}), "Nomor Divisi", lgControlWidth:=1, smControlWidth:=1)
                
        Dim taskTitleInput = New MvcHtmlString("<input type='text' name='TaskTitle' id='DivisionTaskTitle' value='' class='form-control'")
                
                @Html.WriteFormInput(taskTitleInput, "Uraian")
            
            
                @<div class="col-lg-offset-4 col-sm-offset-2">
                    <button type="submit" class="btn btn-primary" id="btnSimpanDivision">
                        Simpan</button>
                    <button type="reset" class="btn btn-primary" id="btnBatalDivision">
                        Batal</button>
                </div>
    End Using
        </div>
        <div id="divItemPlaceHolder" style="display: none">
            <div id="divItem">
                @Using Html.BeginForm("SaveDivisionItem", "Budget", Nothing, FormMethod.Post,
                         New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmDivisionItem"})
                       
                    @<input type="hidden" id="ItemId" name="Id" value="0" />
    
                    @Html.Hidden("ProjectTaskDivisionId", 0)
                    @<input type="hidden" id="ItemOrdinal" name="Ordinal" value="0" />
                    
                    @Html.WriteFormInput(Html.TextBox("PaymentNumber", "", New With {.class = "form-control",
                                                                                     .maxlength = 10}), "No. Mata Pembayaran",
                                                                             smControlWidth:=2, lgControlWidth:=2)
        
                    @Html.WriteFormInput(Html.TextBox("TaskTitle", String.Empty, New With {.class = "form-control"}), "Uraian")
                    @Html.WriteFormInput(Html.TextBox("UnitQuantity", String.Empty,
                                                      New With {.class = "form-control typeahead", .maxlength = 10}), "Satuan", smControlWidth:=1, lgControlWidth:=1)
                    @Html.WriteFormInput(Html.DecimalInput("QuantityMask", 0), "Perkiraan Kuantitas Kontrak")
                    @Html.Hidden("Quantity")

                    @Html.WriteFormInput(Html.DecimalInput("ValueMask", 0), "Harga Dasar")
                    @Html.Hidden("Value")
                    @Html.WriteFormInput(Html.IntegerInput("WorkDays", 0), "Waktu Pelaksanaan")
           
            
                    @<div class="col-lg-offset-4 col-sm-offset-2">
                        <button type="submit" class="btn btn-primary" id="btnSimpanDivisionItem">
                            Simpan</button>
                        <button type="reset" class="btn btn-primary" id="btnBatalDivisionItem">
                            Batal</button>
                    </div>
    End Using
            </div>
        </div>
    </div>
    
End Using
@If Model.Archive = False Then
    @<script type="text/javascript">
         function WriteItemRow(itemdiv) {
             var htm = "<tr class='tr_itemDivision' data-rowid='" + itemdiv.ItemId + "'>" +
                            "<td class='text-center handle'>" + itemdiv.PaymentNumber + "</td>" +
                            "<td>" + itemdiv.TaskTitle + "</td>" +
                            "<td class='text-center'>" + itemdiv.UnitQuantity + "</td>" +
                            "<td class='text-right'>" + $.number(itemdiv.Quantity, 2, ",", ".") + "</td>" +
                            "<td class='text-right'>" + $.number(itemdiv.Value, 2, ",", ".") + "</td>" +
                            "<td class='text-right'>" + $.number(itemdiv.Total, 2, ",", ".") + "</td>" +
                            "<td class='text-right'>" + (itemdiv.WorkDays == 0 ? "" : itemdiv.WorkDays) + "</td>" +
                            "<td class='text-center editColumn'>" +
                            "<div class='btn-group' role='group'>" +
                            "<button type='button' class='btn btn-default btn-xs' onClick='editDivisionItem(this)'><i class='fa fa-edit'></i></button>" +
                            "<button type='button' class='btn btn-default btn-xs' onClick='removeDivisionItem(this)'><i class='fa fa-remove'></i></button>" +
                            "</div>" +
                            "</td>" +
                            "</tr>";
             return htm
         }
         var _RenderAddDivisionItem = function (data, type, row) {
             if (type == 'display') {
                 return "<div class='btn-group' role='group'>" +
                "<button type='button' class='btn btn-default btn-xs' onclick='addDivisionItem(this)' id='btnAddDivisionItem_"+data+"'><i class='fa fa-plus'></i></button>" +
                "<button type='button' class='btn btn-default btn-xs' onclick='editDivision(this)'><i class='fa fa-edit'></i></button>" +
                "<button type='button' class='btn btn-default btn-xs' onclick='removeDivision(this)'><i class='fa fa-remove'></i></button>" +
                "</div>"; ;
             }
             return data;
         }
    </script>
Else
    @<script type="text/ecmascript">
         function WriteItemRow(itemdiv) {
             var htm = "<tr class='tr_itemDivision' data-rowid='" + itemdiv.ItemId + "'>" +
                            "<td class='text-center handle'>" + itemdiv.PaymentNumber + "</td>" +
                            "<td>" + itemdiv.TaskTitle + "</td>" +
                            "<td class='text-center'>" + itemdiv.UnitQuantity + "</td>" +
                            "<td class='text-right'>" + $.number(itemdiv.Quantity, 2, ",", ".") + "</td>" +
                            "<td class='text-right'>" + $.number(itemdiv.Value, 2, ",", ".") + "</td>" +
                            "<td class='text-right'>" + $.number(itemdiv.Total, 2, ",", ".") + "</td>" +
                            "<td class='text-right'>" + (itemdiv.WorkDays == 0 ? "" : itemdiv.WorkDays) + "</td>" +
                            "<td class='text-center editColumn'>&nbsp;" +
                            "</td>" +
                            "</tr>";
             return htm;
         }
         var _RenderAddDivisionItem = function (data, type, row) {
             if (type == 'display') {
                 return "<div class='btn-group' role='group'>" +

                "</div>"; ;
             }
             return data;
         }
    </script>
End If
<script type="text/javascript" src="../../../../plugins/bootstrap/typeahead.bundle.js"></script>
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<style type="text/css">
    .tr_divisi
    {
        background-color: #f0f0f0;
        vertical-align: middle;
    }
    #tblBudget tr.tr_subtotal td
    {
        border-top-width: 2px;
        border-bottom-width: 2px;
        font-weight: bold;
        vertical-align: middle;
        padding-top: 5px;
        padding-bottom: 5px;
    }
    
    .sortable(-PlaceHolder)
    {
        background-color: #ff0000;
    }
</style>
<script src="../../../../plugins/jquery-hotkeys/jquery.hotkeys.js" type="text/javascript"></script>
<script type="text/javascript">

    var tblBudget = null;
    var isEditMode = false;
    //button handler on division row
    var addDivisionItem = function (obj) {
        $("#frmDivisionItem").trigger("reset");
          $("#ItemId").val(0);
        $("#frmDivisionItem .form-group").removeClass("has-error");
        var _trParent = $(obj).parents("tr");
        var _tr = $(_trParent).nextAll("tr.tr_subtotal:first");
        _tr.before($("#form_placeholder"));
        $("#form_placeholder").show("slow");
        var _data = tblBudget.row(_trParent).data();
        $("#ProjectTaskDivisionId").val(_data.Division.DivisionId);
        $("#PaymentNumber").focus();

        $('html, body').animate({
            scrollTop: $("#form_placeholder").offset().top - 150
        }, 0);

    }
    var editDivision = function (obj) {

        $("#btnAdd").trigger("click");
        var _trParent = $(obj).parents("tr");
        var _data = tblBudget.row(_trParent).data();
        $("#DivisionId").val(_data.Division.DivisionId);
        $("#DivisionNumber").val(_data.Division.DivisionNumber);
        $("#DivisionTaskTitle").val(_data.Division.DivisionTitle);
        $("#Ordinal").val(_data.Division.DivisionOrdinal);
        $("#DivisionNumber").focus();

    }
    var removeDivision = function (obj) {
        if (confirm("Hapus divisi ini?")) {
            var _trParent = $(obj).parents("tr");
            var _data = tblBudget.row(_trParent).data();
            var _id = _data.Division.DivisionId;
            var _url = "/Budget/DeleteDivision";
            $.ajax({
                type: "POST",
                data: { id: _id },
                url: _url,
                success: function (data) {
                    if (data.stat == 1) {
                        showNotification("Data terhapus.");
                        tblBudget.ajax.reload();
                        return;
                    };
                    showNotificationSaveError(data, "Penghapusan gagal.");
                }


            });
        }


    }
    //end
    //button handler on divisionitem row
    var editDivisionItem = function (obj) {
        $("#frmDivisionItem").trigger("reset");
        $("#frmDivisionItem .form-group").removeClass("has-error");
        var _trParent = $(obj).parents("tr");
        var rowid = _trParent.data("rowid");
        var divisionRow = $(_trParent).prevAll(".tr_divisi:first");
        var _data = tblBudget.row(divisionRow).data();
        $(_data.Group).each(function (g, item) {
            if (item.ItemId == rowid) {
                $("#ItemId").val(rowid);
                $("#ProjectTaskDivisionId").val(item.ProjectTaskDivisionId);

                $("#ItemOrdinal").val(item.Ordinal);
                $("#PaymentNumber").val(item.PaymentNumber);
                $("#TaskTitle").val(item.TaskTitle);
                $("#UnitQuantity").val(item.UnitQuantity);
                $("#QuantityMask").val(item.Quantity);
                $("#ValueMask").val(item.Value);
                $("#WorkDays").val(item.WorkDays);
                _trParent.before($("#form_placeholder"));
                _trParent.hide();
                $("#form_placeholder").show("slow");
                $("#PaymentNumber").focus();
                return;
            }

        });


    }
    var removeDivisionItem = function (obj) {
        if (confirm("Hapus item ini ?")) {
            var _trParent = $(obj).parents("tr");
            var rowid = _trParent.data("rowid");

            var _url = "/Budget/DeleteDivisionitem";
            $.ajax({
                type: "POST",
                data: { id: rowid },
                url: _url,
                success: function (data) {
                    if (data.stat == 1) {
                        showNotification("Data terhapus.");
                        tblBudget.ajax.reload();
                        return;
                    };
                    showNotificationSaveError(data, "Penghapusan gagal.");
                }


            });
            return false;
        }
    }
    $(function () {//init script

        $("#UnitQuantity").on("blur", function (e)
        {
            val = $(this).val();
            val = val.toLowerCase();
            switch(val)
            {
                case "m1":
                    $(this).val("M¹");
                    break;
               case "m2":
                    $(this).val("M²");
                    break;
                    case "m3":
                    $(this).val("M³")
                    break;
                    case "ls":
                    $(this).val("Ls");
                    break;
            }
        });

        $(".btnAdd").click(function () {
            $("#frmDivision").trigger("reset");
            $("#DivisionId").val(0);
            $("#divList").slideUp("slow");
            $("#divForm").slideDown("slow");
            $("#TaskTitle").focus();
        });
        $(document).bind('keydown', 'ctrl+f2', function () {
            $("#btnAdd").trigger("click");
        });

        $("#btnEdit").click(function () {
            $(".editColumn").toggle();
            isEditMode = true;
        });

        $("#btnBatalDivision").click(function () {
            $("#divList").slideDown("slow");
            $("#divForm").slideUp("slow");
        });

        $("#btnBatalDivisionItem").click(function () {
            $("#form_placeholder").slideUp("slow");
            var _trNext = $("#form_placeholder").next();
            if ((_trNext.length > 0) && (_trNext.hasClass("tr_itemDivision"))) {
                _trNext.show();
            };

        });

        $("#frmDivision").submit(function () {
            showSavingNotification();
            var _url = $("#frmDivision").attr("action");

            var _data = $("#frmDivision").serialize();
            $.ajax({
                type: "POST",
                data: _data,
                url: _url,
                success: function (data) {
                    if (data.stat == 1) {
                        showNotification("Data tersimpan.");
                        $("#btnBatalDivision").trigger("click");
                        tblBudget.ajax.reload();
                        return;
                    };
                    showNotificationSaveError(data, "Penyimpan gagal.");
                }


            });
            return false;
        });

        var currentDivisionId =null;

        $("#frmDivisionItem").submit(function () {
            showSavingNotification();
            var _url = $("#frmDivisionItem").attr("action");
            $("#Value").val($.number($("#ValueMask").val(), 2, ",", "."));
            $("#Quantity").val($.number($("#QuantityMask").val(), 2, ",", "."));
            var _data = $("#frmDivisionItem").serialize();
            currentDivisionId=$("#ProjectTaskDivisionId").val();
            $.ajax({
                type: "POST",
                data: _data,
                url: _url,
                success: function (data) {
                    if (data.stat == 1) {
                        showNotification("Data tersimpan.");
                        $("#btnBatalDivisionItem").trigger("click");
                        tblBudget.ajax.reload();
                        return;
                    };
                    showNotificationSaveError(data, "Penyimpan gagal.");

                }


            });
            return false;

        });


        //---------------- fill the table --------

        var _RenderBlank = function (data, type, row) {
            if (type == 'display') {
                return "";
            }
            return data;
        }
      
        var _RenderRemoveDivisionItem = function (data, type, row) {
            if (type == 'display') {
                return "<button type='button' class='btn btn-default btn-xs'><i class='fa fa-remove'></i></button>";
            }
            return data;
        }
        var _RenderDivisionTitle = function (data, type, row) {

            if (type == 'display') {
                return "<strong>DIVISI " + row.Division.DivisionNumber + ". " + data + "</strong>";
            }
            return data;

        }

        var arrColumns = [
            { "data": "Division.DivisionId", "mRender": _RenderBlank, "sClass": "handle" }, //0
            {"data": "Division.DivisionTitle", "mRender": _RenderDivisionTitle, "sClass": "" }, //1
            {"data": "Division.DivisionId", "mRender": _RenderBlank }, //2
            {"data": "Division.DivisionId", "mRender": _RenderBlank }, //3
            {"data": "Division.DivisionId", "mRender": _RenderBlank }, //4
            {"data": "Division.DivisionId", "mRender": _RenderBlank }, //5
            {"data": "Division.DivisionId", "mRender": _RenderBlank }, //6
            {"data": "Division.DivisionId", "mRender": _RenderAddDivisionItem, "sClass": "text-center editColumn"}//, //7
        //{"data": "Division.DivisionId", "mRender": _RenderRemoveDivisionItem}//8

        ];
        var _coldefs = [
                   { "bSortable": false, "targets": [0], "mData": null },
                   { "bSortable": false, "targets": [5] }

            ];
        var _localLoad = function (data, callback, setting) {
            $("#divItem").appendTo($("#divItemPlaceHolder"));
            var _ProjectId = $("#ProjectInfoId").val();
            $.ajax({
                    url:                            '/Budget/GetRAPItems',
                data: { id: _ProjectId },
                type: 'POST',
                success:                        callback,
                 DataType :'json'
            });

        };
        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = _coldefs;
        datatableDefaultOptions.ajax = _localLoad;
        datatableDefaultOptions.fnDrawCallback = function (settings) {

            var totalAll = 0;

            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;
            api.column(0, { page: 'current' }).data().each(function (group, index) {
                var subTotal = 0;
                var divisiondata = tblBudget.rows(index).data();


                tblBudget.rows(index).data().each(function (value, idx) {
                    $(value.Group).each(function (gidx, itemdiv) {
                        subTotal += itemdiv.Total;
                    });
                });
                totalAll += subTotal;

                var htmlTotalTr = "<tr class='tr_subtotal'>" +
                    "<td colspan='5' style='padding-left:130px;'>Jumlah Harga Pekerjaan Divisi " + divisiondata[0].Division.DivisionNumber +
                    " (masuk pada Rekapitulasi Perkiraan Harga Pekerjaan)</td>" +
                "<td class='text-right'>" + $.number(subTotal, 2, ",", ".") + "</td><td></td><td class='editColumn'>&nbsp;</td></tr>";
                $(rows).eq(index).after(htmlTotalTr);
                var nextRow = $(rows).eq(index).next();
                tblBudget.rows(index).data().each(function (value, idx) {
                    $(value.Group).each(function (gidx, itemdiv) {
                        var trItem = WriteItemRow(itemdiv);
                        $(nextRow).before(trItem);
                    });
                });

                $("#totalrap").text($.number(totalAll, 2, ",", "."));
                $("#ppn").text($.number((totalAll * 0.1), 2, ",", "."));
                $("#rapppn").text($.number(totalAll + (totalAll * 0.1), 2, ",", "."));

            });
            if (currentDivisionId!==null)
            {
                $("#btnAddDivisionItem_"+currentDivisionId).focus();
            }
        };
                                datatableDefaultOptions.ordering = false;
        datatableDefaultOptions.createdRow = function (row, data, index) {
            $(row).addClass("tr_divisi");
            $(row).data("rowid", data.Division.DivisionId);
        };

        tblBudget = $("#tblBudget").DataTable(datatableDefaultOptions).on("draw.dt", function () {
            $("#tblBudget").append("<tr id='form_placeholder' style='display:none'><td colspan='8'></td></tr>");
            $("#divItem").appendTo($("#form_placeholder td:first"));

            $("#tblBudget tr").removeClass("odd");
            $("#tblBudget tr").removeClass("even");



            $("#tblBudget tbody").sortable({
                items: "tr",

                cursor: "move",

                helper: fixHelperModified,
                handle: ".handle",
                forcePlaceholderSize: true,
                forceHelperSize: true,
                revert: true,
                options: {
                    placeholder: "sortable-placeholder",
                    cursorAt: { left: 1, top: 50 },
                tolerance:                      'pointer'
                },
                update: function (event, ui) {

                    if (ui.item.hasClass("tr_divisi")) {

                        var ids = [];
                        var counter = 0;
                        $("#tblBudget tbody tr").each(function (e) {
                            if ($(this).css("display") != "none") {
                                ids[counter] = $(this).data("rowid")
                                counter++;
                            }
                        })


                        var arrdata = ids.join(",");
                        var ProjectInfoId = $("#ProjectInfoId").val();
                        $.ajax({
                            type: "POST",
                            url: "/Budget/SaveDivisionOrder",
                            data: { ids: arrdata, ProjectInfoId: ProjectInfoId },
                            success: function (msg) {
                                tblBudget.ajax.reload();
                            }
                        });

                    } else if (ui.item.hasClass("tr_itemDivision")) {
                        //console.log(ui.item);
                        //get the division id;
                        var divisionRow = $(ui.item).prevAll(".tr_divisi:first");
                        var itemDivisions = $(divisionRow).nextUntil(".tr_subtotal");
                        var DivisionId = $(divisionRow).data("rowid");
                        var ids = [];
                        var counter = 0;
                        $(itemDivisions).each(function (idx) {
                            ids[counter] = $(this).data("rowid");
                            counter++;
                        });

                        var arrdata = ids.join(",");

                        $.ajax({
                            type: "POST",
                            url: "/Budget/SaveDivisionItemOrder",
                            data: { ids: arrdata, DivisionId: DivisionId },
                            success: function (msg) {
                                tblBudget.ajax.reload();
                            }
                        });

                    };



                },
                start: function (event, ui) {
                    if ($(ui.item).hasClass("tr_divisi")) {
                        $(".tr_subtotal").hide();
                        $(".tr_itemDivision").hide();
                    } else {
                        $(".tr_subtotal").hide();
                    }

                }
            })//.disableSelection();
        });


        $("#btnPrint").click(function () {
            window.location.href = "/Budget/BudgetReportPrint/" + @( Model.Id)
        });

    });
    //end init script
    var fixHelperModified = function (e, tr) {

        tr.children().each(function () {
            $(this).width($(this).width());
        });

        return tr;
    };  //Make diagnosis table sortable
   
</script>
