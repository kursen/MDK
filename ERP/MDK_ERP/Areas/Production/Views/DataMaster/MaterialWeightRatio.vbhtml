@Code
    ViewData("Title") = "Konfigurasi"
    ViewBag.headIcon = "icon-wrench"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Konfigurasi", "/Production/DataMaster"}, _
                                             {"Daftar Rasio Material", Nothing}
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

<h4 class='inline'><i class='icon-retweet'></i> Daftar Rasio Material</h4>
<a data-toggle="modal" id="" data-target="#modal-Data" class="btn btn-sm btn-success pull-right" href="javascript:void(0)">
	<i class="icon-plus"></i> Tambah Baru
</a>
<div class="clear"></div>
@Using Html.RowBox("box-table")
    @<table id="tb_Data" class="display" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th>
                    Material
                </th>
                <th>
                    Satuan
                </th>
                <th>
                    Rasio
                </th>
                <th class="action">
                </th>
            </tr>
        </thead>
    </table>
End Using

@Using Html.ModalForm("modal-Data", "Tambah Rasio", Url.Action("MWR_Save", "DataMaster"), "", "POST", "form-Data", "form-horizontal")
    @<div class="form-group">
        <label class="col-md-3 control-label">
            Nama Material</label>
        <div class="col-sm-5">
            @Html.DropDownList("IDMaterial", New SelectList(DirectCast(ViewData("materials"), IEnumerable), "ID", "Name"), "Pilih Produk", New With {.class = "form-control"})
        </div>
    </div>
    Using Html.RowBox("box-table")
    @<a id="addRatio" class="btn" href="javascript:void(0)"> <i class="icon-plus btn btn-success">
    </i>Tambah Ratio </a>
    @<table id="tb_ratio" class="table" style="display: none;">
        <thead>
            <tr>
                <th>Satuan</th>
                <th>Nilai</th>
                <th class="action"></th>
            </tr>
        </thead>
        <tbody>
        </tbody>
    </table>
    End Using
End Using
<div style="display: none;" id="dropdown">
    @Html.DropDownList("IDMeasurementUnit", New SelectList(DirectCast(ViewData("listRatio"), IEnumerable), "ID", "Unit"), "Pilih Satuan", New With {.class = "form-control"})
</div>
@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/Select2/select2.css")" />
End Section
@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/Select2/select2.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript">
    $(document).ready(function () {
    GetList();
    $('#addRatio').attr('disabled',true);
    
    $('#addNew').click(function(){
        fnReset();
        $('#addRatio').attr('disabled',true);
    });

    $('#IDMaterial').change(function(){
           if($(this).val()!=''){
                $('#addRatio').removeAttr('disabled');
           }else{
                $('#addRatio').attr('disabled',true);
           }
     });
    
    $('#addRatio').click(function () {
            $('#tb_ratio').show();
           var strHtml = '';
           var tBody = $('#tb_ratio tbody');
           strHtml+="<tr>";
            strHtml +='<td id="satuan">'+'<input type="hidden" id="ratioVal" name="ratioVal"/>'+$('#dropdown').html()+'</td>';
           //strHtml+='<td>'+ $('#IDMeasurementUnit').css("width","80%").parent().html() +'</td>';
           strHtml+="<td id='weight'><input type=text name=Weight class='form-control weight' style='width:30%;margin-left:40%;'/></td>";
           strHtml +='<td>';
           strHtml+='<a href=javascript:void(0) class="icon-paste btn btn-success btn-xs" data-toggle="tooltip" data-placement="right" title="Simpan"></a>';
           strHtml+='<a style="display:none;" href=javascript:void(0) class="icon-edit btn btn-success btn-xs" data-toggle="tooltip" data-placement="right" title="Edit"></a>';
           strHtml+='&nbsp;<a href=javascript:void(0) class="icon-remove btn btn-danger btn-xs" data-toggle="tooltip" data-placement="right" title="Hapus"></a></td>';
           strHtml+="</tr>";
           tBody.append(strHtml);
           //
           addHandler(tBody);
           fnInsert('#form-data','#modal-Data');
    });
    function addHandler(tBody){
        $(tBody).find('.icon-paste').click(function(){
                 //
               var InputSatuan = $(this).parent().parent().find("select");
               var InputWeight=$(this).parent().parent().find('.weight');
               var satuan = InputSatuan.val();
              var weight = InputWeight.val();
              $(this).parent().parent().find('#ratioVal').val(satuan);
              InputSatuan.attr('disabled','disabled');
              InputWeight.attr('readonly',true);
              $(this).parent().parent().find(".icon-edit").show();
               $(this).hide();
         });
           //
         $('.icon-edit').click(function(){
                var obj=$(this).parent().parent();
                obj.find('#ratioVal').val('');
                obj.find('select').removeAttr('disabled');
                obj.find('.weight').removeAttr('readonly');
                $(this).hide();
                $('.icon-paste').show();
       
            });
        $('.icon-remove').click(function(){
            $(this).parent().parent().remove();
        });
    }
});
GetList = function () {
    var attr = _attrCRUD()
    attr.url = {
        "Read": "Production/DataMaster/getListsRatio",
        "Delete": "Production/DataMaster/MWR_Delete"
    };
    attr.dataTable.columns = [
        {"data": "Material"},
        {"data":"Satuan"},
        { "data": "Rasio" },
        {
            "bSortable": false,
            "defaultContent": "" +
                "<div align='center'>" +
                "  <button class='icon-edit btn btn-primary btn-xs' data-toggle='tooltip' data-placement='bottom' title='Edit'></button>" +
                "  <button class='icon-remove btn btn-danger btn-xs' data-toggle='tooltip' data-placement='right' title='Hapus'></button>" +
                "</div>"
        }
    ];

    fnGetList(attr);
}
    </script>
End Section
