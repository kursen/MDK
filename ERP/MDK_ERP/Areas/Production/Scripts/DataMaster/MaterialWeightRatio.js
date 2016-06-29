//*************************
//    View: /DataMaster/MaterialWeightRatio
//*************************


/*--------------------------------------
    WHEN WINDOWS READY TO OPEN
--------------------------------------*/
$(document).ready(function () {
    GetList();
    $('#addRatio').click(function () {
       var strHtml = '';
       var tBody = $('#tb_ratio tbody');
       strHtml+="<tr>";
       strHtml+="<td></td>";
       strHtml+="<td>J</td>";
       strHtml+="</tr>";
       tBody.append(strHtml);
    });
    //    $('#IDMaterial').change(function () {
    //        var strHtml = '';
    //        var tHead = $('#tb_ratio thead');
    //        var Body = $('#tb_ratio tbody');
    //        strHtml = '<tr>';
    //        if($(this).val()==52){
    //        strHtml='<td>Litre</td>';
    //        strHtml='<td><input type="text" name
    //        }
    //        
    //        strHtml='<td><
    //    });
});

GetList = function () {
    var attr = _attrCRUD()
    attr.url = {
        "Read": "Production/DataMaster/getListsRatio"
        //"Delete": "Production/DataMaster/Unit_Delete"
    };
    attr.dataTable.columns = [
        {
            "data": "Material",
        },
        {"data":"Satuan"},
        { "data":"Rasio"},
        {
            "className": "action",
            "data": null,
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
getListSatuan = function(){
    $.ajax({
        type:"POST",
        dataType:"json",
        url:"Production/DataMaster/listRatio",
        success:function(data){
            return false;
        },
        error:function(xhr,status,data){
         alert("Failed" + xhr.status + status + data)
        }
    });
}






