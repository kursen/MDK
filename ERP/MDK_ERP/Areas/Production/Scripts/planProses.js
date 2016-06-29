//*************************
//    View: /AMPProcess/Index
//*************************


/*--------------------------------------
    WHEN WINDOWS READY TO OPEN
--------------------------------------*/
$(document).ready(function () {

    datePicker_ByDate($('#tgl'));
    var nilai_produk, jlh_produk; //variable untuk menentukan produk dan jumlahnya
    //Datatable

    $('#tbl_plan').dataTable({
        "ajax": parseUrl('AMPProcess/get_listPlan'),
        //"pageLength": 30,
        //"searching": false,
        //"bPaginate": false,
        "bSort": true,
        "sDom": '<"top">rt<"bottom"ip><"clear">',
        "sDom": '<"top"<"dataTables_add1">>rt<"bottom"<"div-page"ip>><"clear">',
        "columns": [
            {
                "data": "ID"
            },
            {
                "data": "IDMaterial"
            },
            {
                "data": "Keterangan"
            },
            {
                "data": "TglPerencanaan"
            },
            {
                "data": "JumlahPerencanaan"
            }
           /* {
                "className": "td-center"
            },*/
            
        ]
        //,"order": [[ 3, 'asc' ]],
    });

    var btnAddNew = '<a data-toggle="modal" id="" data-target="#modal-Produk" class="btn btn-default" href="javascript:void(0)">' +
    '<i class="icon-plus btn btn-success"></i> Perencanaan Baru' +
    '</a>'
    $("div.dataTables_add1").html(btnAddNew);

    reset_form();
    tampil_komposisi('', '');
    datetimePicker_ByDate('#tgl');
    //lakukan jika produk telah dipilih dan jumlahnya telah ditetapkan
    $('#jenis-produk').change(function () {
        nilai_produk = $('#jenis-produk').val();
        $('#jlh').val('');
        kondisi_tbl(0);
        if (nilai_produk == 0) {
            reset_form();
        }
        else if (nilai_produk > 3) {
            kondisi_mesin(1);
        } else {
            kondisi_mesin(0);
        }
    });
    $('#jlh').change(function () {
        jlh_produk = $('#jlh').val();
        if (jlh_produk == '') {//jika jumlah belum di tetapkan
            kondisi_tbl(0);
        }
        else if (isNaN(jlh_produk)) {
            validasi(1);
            $('#jlh').val('');
            kondisi_tbl(0);
        } else {
            tampil_komposisi(nilai_produk, jlh_produk);
            validasi(0);
        }
    });

    $('#modal-Produk').on('hide.bs.modal', function (e) {
        reset_form();
        kondisi_tbl(0);
    });

    //tooltip
    $('[data-toggle="tooltip"]').tooltip();

    //modal pelaksanaan
    $('#modal-pelaksanaan').on('show.bs.modal', function (e) {
        strModal_Html = '';
        strModal_Html += '<div class="form-group">';
        strModal_Html += '<label class="col-sm-3 control-label">Jenis Produk</label>';
        strModal_Html += '<label class="col-sm-2 control-label">AC-WC</label>';
        strModal_Html += '</div>';
        strModal_Html += '<div class="form-group">';
        strModal_Html += '<label class="col-sm-3 control-label">Mesin Produksi</label>';
        strModal_Html += '<label class="col-sm-2 control-label">AMP</label>';
        strModal_Html += '</div>';
        strModal_Html += '<div class="form-group">';
        strModal_Html += '<label class="col-sm-3 control-label">Tanggal Rencana</label>';
        strModal_Html += '<label class="col-sm-2 control-label">3-3-2015 </label>';
        strModal_Html += '</div>';
        strModal_Html += '<div class="form-group">';
        strModal_Html += '<label class="col-sm-3 control-label">Jumlah Rencana</label>';
        strModal_Html += '<label class="col-sm-2 control-label">3 Bucket</label>';
        strModal_Html += '</div>';
        strModal_Html += '<div class="form-group">';
        strModal_Html += '<label class="col-sm-3 control-label">Tanggal Pelaksanaan</label>';
        strModal_Html += '<div class="col-sm-5">';
        strModal_Html += '<input type="text" id="dt_do" class="form-control" /></div>';
        strModal_Html += '</div>';
        strModal_Html += '<div class="form-group">';
        strModal_Html += '<label class="col-sm-3 control-label">Jumlah Pelaksanaan</label>';
        strModal_Html += '<div class="col-sm-3">';
        strModal_Html += '<input type="text" id="jlh_do" class="form-control" /></div>';
        strModal_Html += '<div class="col-sm-2">';
        strModal_Html += '<label class="control-label">Bucket</label>';
        strModal_Html += '</div>';
        strModal_Html += '</div>';
        //table
        strModal_Html += '<table class="table table-striped">';
        strModal_Html += '<thead>';
        strModal_Html += '<tr>';
        strModal_Html += '<th>Nama Material</th>';
        strModal_Html += '<th>Jumlah Perencanaan(Bucket)</th><th>Jumlah Pelaksanaan(Bucket)</th>';
        strModal_Html += '</tr>';
        strModal_Html += '</thead>';
        strModal_Html += '<tbody>';
        strModal_Html += '<tr>';
        strModal_Html += '<td>Abu Batu</td>';
        strModal_Html += '<td align="center">1</td>';
        strModal_Html += '<td><input style="width:50%;" type="text" class="form-control" /></td>';
        strModal_Html += '</tr>';
        strModal_Html += '<tr>';
        strModal_Html += '<td>Split 1,2</td>';
        strModal_Html += '<td align="center">1</td>';
        strModal_Html += '<td><input style="width:50%;" type="text" class="form-control" /></td>';
        strModal_Html += '</tr>';
        strModal_Html += '<tr>';
        strModal_Html += '<td>Split 2,3</td>';
        strModal_Html += '<td align="center">0</td>';
        strModal_Html += '<td></td>';
        strModal_Html += '</tr>';
        strModal_Html += '<tr>';
        strModal_Html += '<td>Pasir</td>';
        strModal_Html += '<td align="center">1</td>';
        strModal_Html += '<td><input style="width:50%;" type="text" class="form-control" /></td>';
        strModal_Html += '</tr>';
        /*strModal_Html+='<tr>';
        strModal_Html+='<td></td>';
        strModal_Html+='<td></td>';
        strModal_Html+='<td></td>';
        strModal_Html+='</tr>';*/
        strModal_Html += '</tbody>';
        strModal_Html += '</table>';
        //end of table
        $(this).find('#form-pelaksanaan').html(strModal_Html);
        //   datetimePicker_ByDate('#dt_do');
    });

    //insert data form
    $('#form-perencanaan').submit(function (e) {
        e.preventDefault();
        var url = $(this).attr('action');
        var data = $(this).serialize();
        Insert_Planning(url, data);
    });
    //==================================
});
/*--------------------------------------
Call AJAX for list of data
--------------------------------------*/
//fungsi insert data
Insert_Planning = function (url, data) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: url,
        data: data,
        success: function (get) {
            if (get.stat == 1) {
                alert(get.msg);
                location.reload(true);
            } else {
                alert(get.msg);
            }
            return false;
        },
        error: function (xhr, status, data) {
            alert("Failed ", xhr.status);
        }
    });
}
//========================================

/*------------------------------------------
---------------------------------------------*/
//fungsi reset Form
reset_form=function(){
	document.getElementById("form-perencanaan").reset();//resetform
}

//========================
//fungsi saat modal tampil
tampil_komposisi=function(nilai,jlh){
		//cek nilai produk
		var tbl_komposisi_body = $('#tbl_komposisi tbody');
		var strHtml='';
		
		var total,jlh_abu=0,jlh_split12=0,jlh_split23=0,jlh_pasir=0;
		total=jlh/3;
		if(nilai==1){
			/*$('#lblsplit12,#split12').hide();//sembunyikan field yg tidak termasuk
			$('#lblsplit1,#split1,#lblabu,#abu,#lblpasir,#pasir').show();*/
			jlh_abu=(total+1).toFixed(2);
			jlh_split12=(total+0.2).toFixed(2);
			jlh_pasir=(total+1.5).toFixed(2);
			kondisi_tbl(1);
			kondisi_mesin(0);
		}else if(nilai==2){
			/*$('#lblsplit1,#split1').hide();//sembunyikan field yg tidak termasuk
			$('#lblsplit12,#split12,#lblabu,#abu,#lblpasir,#pasir').show();
			total=jlh/3;*/
			jlh_split23=(total+1).toFixed(2);
			jlh_abu =(total+0.7).toFixed(2);
			jlh_pasir=(total+2).toFixed(2);
			kondisi_tbl(1);
			kondisi_mesin(0);
		}else if(nilai==3){
			/*$('#lblpasir,#pasir').hide();//sembunyikan field yg tidak termasuk
			$('#lblsplit1,#split1,#lblsplit12,#split12,#lblabu,#abu').show();
			total=jlh/3;*/
			jlh_abu=(total+1).toFixed(2);
			jlh_split12=(total-1).toFixed(2);
			jlh_split23=(total+0.5).toFixed(2);
			jlh_pasir=(total+0.4).toFixed(2);
			kondisi_tbl(1);
			kondisi_mesin(0);
		}else if(nilai>3){
			kondisi_tbl(0);
		}else{
			$('#jlh').val('');
			kondisi_tbl(0);
			kondisi_mesin(2);
		}
		strHtml+='<tr class="even">';
		strHtml+='<td>Abu</td>';
		strHtml+='<td>'+jlh_abu+'</td>';
		strHtml+='</tr>';
		strHtml+='<tr class="odd">';
		strHtml+='<td>Split12</td>';
		strHtml+='<td>'+jlh_split12+'</td>';
		strHtml+='</tr>';
		strHtml+='<tr class="even">';
		strHtml+='<td>Split23</td>';
		strHtml+='<td>'+jlh_split23+'</td>';
		strHtml+='</tr>';
		strHtml+='<tr class="odd">';
		strHtml+='<td>Pasir</td>';
		strHtml+='<td>'+jlh_pasir+'</td>';
		strHtml+='</tr>';
		tbl_komposisi_body.html(strHtml);
}

//=====================================
//fungsi kondisi mesin
kondisi_mesin=function(val){
	if (val==0){
		$('#mesin_operasi').val('AMP');
	}else if(val==1){
		$('#mesin_operasi').val('Crusher');
	}else{
		$('#mesin_operasi').val('');
	}
}


//=================================
//fungsi sembunyikan tabel komposisi
kondisi_tbl=function(val){
	//sembunyikan komposisi produk
	if (val ==0){
	$('#tbl_komposisi').css('display','none');
	}else{
		$('#tbl_komposisi').removeAttr('style');
	}
}

//fungsi switch produk
/*get_produk = function (val) {
    var jenis_produk = document.getElementById('jenis-produk');
    var val_jenisProduk = "";
    switch (val) {
        case 1:
            val_jenisProduk="AC-WC"
            break;
        case 2:
            val_jenisProduk = "AC-WC"
            break;
        case 3:
            val_jenisProduk = "AC-WC"
            break;
        case 4:
            day = "Thursday";
            break;
        case 5:
            day = "Friday";
            break;
        case 6:
            day = "Saturday";
            break;
    }
}*/

//===========================================

//Fungsi Validasi 
validasi=function(val){
	if (val==0){
		$('.text-muted').css('display','none');
	}else{
		$('.text-muted').css('display','block');
	}
}
//=====================================
