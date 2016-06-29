
<style type="text/css">
    #layerRptForm tr {
        background-image:none;
    }
    tr.tbHead {
        background-image: linear-gradient(to bottom, #F0F0F0, #DFDFDF) !important;
    }
    tr.tbHead th {
        height:30px;
        text-align:left !important;
    }
    
    .PositionRow {
        margin-top:0 !important;
    }
    
</style>

<div class="row">
    <div class="col-sm-12">
        @Html.WriteFormInput(Html.TextBox("DateItem", Nothing, New With {.class = "form-control", .disabled = "disabled"}), "Tanggal", smControlWidth:=3, lgControlWidth:=2, lgLabelWidth:=2, smLabelWidth:=2)
    </div>
</div>
<div class="row">
    <div class="col-sm-12">
        @Html.WriteFormInput(Html.TextBox("DayWork", Nothing, New With {.class = "form-control text-right DayWork", .disabled = "disabled"}), "Hari kerja ke-", smControlWidth:=2, lgControlWidth:=1, lgLabelWidth:=2, smLabelWidth:=2)
    </div>
</div>

<table id="tb_DetailA" class="table table-striped table-bordered table-hover table-heading table-datatable dataTable responsive no-footer" cellspacing="0" width="100%">
    <colgroup>
        <col style="width:120px" />
        <col />
        <col style="width:100px" />
        <col style="width:15px" />
        <col />
        <col style="width:70px" />
    </colgroup>
    <thead>
        <tr class="tbHead">
            <th colspan="6">A. PEKERJAAN</th>
        </tr>
        <tr>
            <th>No. Item Pembayaran</th>
            <th>Jenis Pekerjaan</th>
            <th colspan="2">Volume</th>
            <th>Lokasi</th>
            <th>
                <button type="button" class="btn btn-danger btn-label-left pull-right" id="btnAdd_A" style="margin:0px;">
                    <span><i class="fa fa-plus"></i></span>Tambah</button>
            </th>
        </tr>
    </thead>
</table>
<div class="row" id="form_A" style="display:none;">
    <div class="col-sm-12">
    @Using Html.BeginForm("SaveDetail_A", "Reports", New With {.ProjectInfoID = Model.id}, FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmDetail_A"})

        @<div class="form-group hidden">
            <div class=""><input type="hidden" id="ID_A" name="ID" value="0" /></div>
        </div>
        
        @<input type="hidden" class="DayWork" name="DayWork" />
        
        @Html.WriteFormInput(Html.DropDownList("ProjectTaskDivisionItemId", Nothing, " - Pilih -", htmlAttributes:=New With {.class = "form-control"}), "Jenis Pekerjaan", smControlWidth:=5, lgControlWidth:=5)

        
        @<div class="form-group"><label class="col-lg-3 col-sm-4 control-label">Volume</label>
            <div class="col-lg-1 col-sm-2 col-xs-11">@Html.TextBox("Volume", 0, New With {.class = "form-control text-right numeric"})</div>
            <div class="col-lg-2 col-sm-5" style="padding-top: 4px;"><span id="VolumeUnit"></span></div>
          </div>
        @Html.WriteFormInput(Html.TextBox("Location_A", Nothing, New With {.class = "form-control", .Name="Location"}), "Lokasi", smControlWidth:=5, lgControlWidth:=5)
    
        @<div class="col-lg-offset-4 col-sm-offset-2">
            <button type="submit" class="btn btn-primary"><i class="fa fa-save"></i> 
                Simpan</button>
            <button type="button" class="btn" id="btnBatal_A" onclick="">
                Batal</button>
        </div>
    End Using
    </div>
</div>


<br />



<table id="tb_DetailB" class="table table-striped table-bordered table-hover table-heading table-datatable dataTable responsive no-footer" cellspacing="0" width="100%">
    <colgroup>
        <col style="width:70px" />
        <col />
        <col style="width:180px" />
        <col style="width:15px" />
        <col style="width:180px" />
        <col style="width:15px" />
        <col style="width:70px" />
    </colgroup>
    <thead>
        <tr class="tbHead">
            <th colspan="8">B. PEMAKAIAN BAHAN KONSTRUKSI POKOK</th>
        </tr>
        <tr>
            <th>No.</th>
            <th>Jenis Material</th>
            <th colspan="2">Kuantitas Terpakai</th>
            <th colspan="2">Kuantitas Didatangkan</th>
            <th>
                <button type="button" class="btn btn-danger btn-label-left pull-right" id="btnAdd_B" style="margin:0px;">
                    <span><i class="fa fa-plus"></i></span>Tambah</button>
            </th>
        </tr>
    </thead>
</table>
<div class="row" id="form_B" style="display:none;">
    <div class="col-sm-12">
    @Using Html.BeginForm("SaveDetail_B", "Reports", New With {.ProjectInfoID = Model.id}, FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmDetail_B"})

        @<div class="form-group hidden">
            <div class="">@Html.Hidden("ID_B", 0, New With {.Name = "ID"})</div>
        </div>
        
        @<input type="hidden" class="DayWork" name="DayWork" />
        
        @Html.WriteFormInput(Html.TextBox("MaterialName", Nothing, New With {.class = "form-control"}), "Jenis Material", smControlWidth:=5, lgControlWidth:=5)

        @Html.WriteFormInput(Html.TextBox("QuantityUnit", Nothing, New With {.class = "form-control QuantityUnit"}), "Satuan", smControlWidth:=4, lgControlWidth:=3)

        @Html.WriteFormInput(Html.TextBox("QuantityUse", 0, New With {.class = "form-control text-right numeric"}), "Kuantitas terpakai", smControlWidth:=4, lgControlWidth:=3)

        @Html.WriteFormInput(Html.TextBox("QuantityImported", 0, New With {.class = "form-control text-right numeric"}), "Kuantitas Didatangkan", smControlWidth:=4, lgControlWidth:=3)
    
        @<div class="col-lg-offset-4 col-sm-offset-2">
            <button type="submit" class="btn btn-primary" ><i class="fa fa-save"></i> 
                Simpan</button>
            <button type="button" class="btn" id="btnBatal_B" onclick="">
                Batal</button>
        </div>
    End Using
    </div>
</div>


<br />


<table id="tb_DetailC" class="table table-striped table-bordered table-hover table-heading table-datatable dataTable responsive no-footer" cellspacing="0" width="100%">
    <colgroup>
        <col style="width:70px" />
        <col />
        <col style="width:150px" />
        <col />
        <col />
        <col style="width:70px" />
    </colgroup>
    <thead>
        <tr class="tbHead">
            <th colspan="6">C. PEMAKAIAN PERALATAN</th>
        </tr>
        <tr>
            <th>No.</th>
            <th>Nama Alat</th>
            <th>Jumlah</th>
            <th>Satuan</th>
            <th>Kondisi</th>
            <th>
                <button type="button" class="btn btn-danger btn-label-left pull-right" id="btnAdd_C" style="margin:0px;">
                    <span><i class="fa fa-plus"></i></span>Tambah</button>
            </th>
        </tr>
    </thead>
</table>
<div class="row" id="form_C" style="display:none;">
    <div class="col-sm-12">
    @Using Html.BeginForm("SaveDetail_C", "Reports", New With {.ProjectInfoID = Model.id}, FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmDetail_C"})

        @<div class="form-group hidden">
            <div class="">@Html.Hidden("ID_C", 0, New With {.Name = "ID"})</div>
        </div>
        
        @<input type="hidden" class="DayWork" name="DayWork" />

        @Html.WriteFormInput(Html.TextBox("EquipmentName", Nothing, New With {.class = "form-control"}), "Nama Alat", smControlWidth:=5, lgControlWidth:=5)

        @Html.WriteFormInput(Html.TextBox("Amount_C", 0, New With {.class = "form-control text-right numeric", .Name = "Amount"}), "Jumlah", smControlWidth:=5, lgControlWidth:=2)

        @Html.WriteFormInput(Html.TextBox("MeasurementUnit", "Unit", New With {.class = "form-control"}), "Satuan", smControlWidth:=5, lgControlWidth:=2)

        @Html.WriteFormInput(Html.TextArea("Condition", New With {.class = "form-control"}), "Kondisi", smControlWidth:=5, lgControlWidth:=5)
    
        @<div class="col-lg-offset-4 col-sm-offset-2">
            <button type="submit" class="btn btn-primary" ><i class="fa fa-save"></i> 
                Simpan</button>
            <button type="button" class="btn" id="btnBatal_C" onclick="">
                Batal</button>
        </div>
    End Using
    </div>
</div>


<br />


<table id="tb_DetailD" class="table table-striped table-bordered table-hover table-heading table-datatable dataTable responsive no-footer" cellspacing="0" width="100%">
    <colgroup>
        <col style="width:70px" />
        <col />
        <col style="width:150px" />
        <col/>
        <col style="width:70px" />
    </colgroup>
    <thead>
        <tr class="tbHead">
            <th colspan="6">D. TENAGA KERJA</th>
        </tr>
        <tr>
            <th>No.</th>
            <th>Jabatan</th>
            <th>Jumlah</th>
            <th>Satuan</th>
            <th>
                <button type="button" class="btn btn-danger btn-label-left pull-right" id="btnAdd_D" style="margin:0px;">
                    <span><i class="fa fa-plus"></i></span>Tambah</button>
            </th>
        </tr>
    </thead>
</table>
<div class="row" id="form_D" style="display:none;">
    <div class="col-sm-12">
    @Using Html.BeginForm("SaveDetail_D", "Reports", New With {.ProjectInfoID = Model.id}, FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmDetail_D"})

        @<div class="form-group hidden">
            <div class="">@Html.Hidden("ID_D", 0, New With {.Name = "ID"})</div>
        </div>
        
        @<input type="hidden" class="DayWork" name="DayWork" />

        @Html.WriteFormInput(Html.TextBox("Position", Nothing, New With {.class = "form-control"}), "Nama Jabatan", smControlWidth:=5, lgControlWidth:=5)

        @Html.WriteFormInput(Html.TextBox("Amount_D", 0, New With {.class = "form-control numeric", .Name = "Amount"}), "Jumlah", smControlWidth:=5, lgControlWidth:=2)

        @Html.WriteFormInput(Html.TextBox("Unit", "O/H", New With {.class = "form-control"}), "Satuan", smControlWidth:=5, lgControlWidth:=2)

        @<div class="col-lg-offset-4 col-sm-offset-2">
            <button type="submit" class="btn btn-primary" ><i class="fa fa-save"></i> 
                Simpan</button>
            <button type="button" class="btn" id="btnBatal_D" onclick="">
                Batal</button>
        </div>
    End Using
    </div>
</div>

<br />

<table id="tb_DetailE" class="table table-hover table-bordered table-heading table-datatable dataTable responsive no-footer" cellspacing="0" width="100%">
    <colgroup>
        <col style="width:15px" />
        <col />
        <col />
        <col style="width:70px" />
    </colgroup>
    <thead>
        <tr class="tbHead">
            <th colspan="6">E. PERSONIL LAPANGAN</th>
        </tr>
        <tr>
            <th>PositionType</th>
            <th colspan="2">Jabatan</th>
            <th>Jumlah</th>
            <th>
                <button type="button" class="btn btn-danger btn-label-left pull-right" id="btnAdd_E" style="margin:0px;">
                    <span><i class="fa fa-plus"></i></span>Tambah</button>
            </th>
        </tr>
    </thead>
</table>
<div class="row" id="form_E" style="display:none;">
    <div class="col-sm-12">
    @Using Html.BeginForm("SaveDetail_E", "Reports", New With {.ProjectInfoID = Model.id}, FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmDetail_E"})
        
        @<input type="hidden" class="DayWork" name="DayWork" />

        @Html.WriteFormInput(Html.TextBox("PositionTypeName", Nothing, New With {.class = "form-control"}), "Instansi", smControlWidth:=5, lgControlWidth:=5)

        @<div class="form-group" style="margin-bottom:0px;">
            <label class="col-lg-3 col-sm-4 control-label">Jabatan</label>
            <div class="btn-group col-sm-1 control-label" style="width: 40px;">
                <button id="addPositionRow" type="button" class='btn btn-default btn-xs' style="width: 40px;"><i class='fa fa-plus'></i></button>
            </div>
        </div>
        @<div class="form-group PositionRow">
            <div class="col-lg-5 col-sm-5 col-lg-offset-3 col-sm-offset-4 ">
                <table width="100%">
                    <tr>
                        <td>@Html.Hidden("ID_E", 0, New With {.Name = "ID"})</td>
                        <td width="65%">@Html.TextBox("Position", Nothing, New With {.class = "form-control", .placeholder = "Posisi"})</td>
                        <td>&nbsp;</td>
                        <td>@Html.TextBox("Amount", 0, New With {.class = "form-control text-right numeric"})</td>
                        <td style="display: inline-block; padding: 8px 4px 0px;">Org</td>
                        <td>&nbsp;</td>
                        <td>
                            <div class="btn-group">
                                <button type="button" class='btn btn-default btn-xs deletePositionRow' style="margin-top: 5px;"><i class='fa fa-remove'></i></button>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        @<div class="clear"><br /></div>

        @<div class="col-lg-offset-4 col-sm-offset-2">
            <button type="submit" class="btn btn-primary" ><i class="fa fa-save"></i> 
                Simpan</button>
            <button type="button" class="btn" id="btnBatal_E" onclick="">
                Batal</button>
        </div>
    End Using
    </div>
</div>
<div class="row" id="subform_E" style="display:none;">
    <div class="col-sm-12">
    @Using Html.BeginForm("SaveSubDetail_E", "Reports", New With {.ProjectInfoID = Model.id}, FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmsubDetail_E"})
        
        @<input type="hidden" class="DayWork" name="DayWork" />
        @Html.Hidden("ID_E", 0, New With {.class = "ID_E_", .Name = "ID"})
        @Html.Hidden("PositionType", "", New With {.class = "PositionType_"})
        @Html.Hidden("PositionTypeName", "", New With {.class = "PositionTypeName_"})

        @Html.WriteFormInput(Html.TextBox("PositionTypeConcat", Nothing, New With {.class = "form-control", .disabled = "disabled"}), "Petugas", smControlWidth:=5, lgControlWidth:=5)
        
        @Html.WriteFormInput(Html.TextBox("Position", Nothing, New With {.class = "form-control Position_"}), "Jabatan", smControlWidth:=5, lgControlWidth:=5)
        
        @Html.WriteFormInput(Html.TextBox("Amount", 0, New With {.class = "form-control text-right Amount_E_ numeric"}), "Jumlah", smControlWidth:=5, lgControlWidth:=2)

        @<div class="col-lg-offset-4 col-sm-offset-2">
            <button type="submit" class="btn btn-primary" ><i class="fa fa-save"></i> 
                Simpan</button>
            <button type="button" class="btn" id="btnSubBatal_E" onclick="">
                Batal</button>
        </div>
    End Using
    </div>
</div>


<br />


<table id="tb_DetailF" class="table table-hover table-bordered table-striped table-heading table-datatable dataTable responsive no-footer" cellspacing="0" width="100%">
    <colgroup>
        <col style="width:15px" />
        <col />
        <col />
        <col />
        <col style="width:140px"  />
        <col style="width:70px" />
    </colgroup>
    <thead>
        <tr class="tbHead">
            <th colspan="6">F. CUACA / BENCANA ALAM / KEJADIAN PENGHAMBAT KEGIATAN</th>
        </tr>
        <tr>
            <th>No.</th>
            <th>Jenis</th>
            <th>Jam</th>
            <th>Lokasi</th>
            <th>Tanggung Jawab<br />Kontraktor<br />( ya / tidak )</th>
            <th>
                <button type="button" class="btn btn-danger btn-label-left pull-right" id="btnAdd_F" style="margin:0px;">
                    <span><i class="fa fa-plus"></i></span>Tambah</button>
            </th>
        </tr>
    </thead>
</table>
<div class="row" id="form_F" style="display:none;">
    <div class="col-sm-12">
    @Using Html.BeginForm("SaveDetail_F", "Reports", New With {.ProjectInfoID = Model.id}, FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmDetail_F"})
        
        @Html.Hidden("ID_F", 0, New With {.Name = "ID"})
        @<input type="hidden" class="DayWork" name="DayWork" />

        @Html.WriteFormInput(Html.TextBox("Type", Nothing, New With {.class = "form-control"}), "Jenis", smControlWidth:=5, lgControlWidth:=5)

        @<div class="form-group"><label class="col-lg-3 col-sm-4 control-label">Waktu</label>
            <div class="col-lg-2 col-sm-3 col-xs-4">
                    @Html.TextBox("DateItem", Nothing, New With {.class = "form-control DateItem", .disabled = "disabled"})
            </div>
            <div class="col-lg-1 col-sm-2" style="width: 120px;">
                <div class="input-group date" data-date-container="#content" id="dtpk_Time">
                    @Html.TextBox("Time", Nothing, New With {.class = "form-control"})
                    <span class="input-group-addon"><i class="fa fa-clock-o"></i></span>
                </div>
            </div>
        </div>
        
        @Html.WriteFormInput(Html.TextBox("Location_F", Nothing, New With {.class = "form-control", .Name = "Location"}), "Lokasi", smControlWidth:=5, lgControlWidth:=5)

        @Html.WriteFormInput(Html.CheckBox("IsResponsibilityOfContractor", Nothing, New With {.class = ""}), "Tanggung Jawab Kontraktor", smControlWidth:=5, lgControlWidth:=5)

        @<div class="clear"><br /></div>

        @<div class="col-lg-offset-4 col-sm-offset-2">
            <button type="submit" class="btn btn-primary" ><i class="fa fa-save"></i> 
                Simpan</button>
            <button type="button" class="btn" id="btnBatal_F" onclick="">
                Batal</button>
        </div>
    End Using
    </div>
</div>




<br />



<table id="tb_DetailG" class="table table-striped table-bordered table-hover table-heading table-datatable dataTable responsive no-footer" cellspacing="0" width="100%">
    <colgroup>
        <col style="width:20%" style="min-width:160px;" />
        <col />
        <col style="width:100px" />
    </colgroup>
    <thead>
        <tr class="tbHead">
            <th colspan="3">G. SARAN / PENGAJUAN / PERSETUJUAN / PELAPORAN</th>
        </tr>
        <tr>
            <th></th>
            <th>Saran / Instruksi / Tanggapan</th>
            <th></th>
        </tr>
    </thead>
</table>
<div class="row" id="form_G" style="display:none;">
    <div class="col-sm-12">
    @Using Html.BeginForm("SaveDetail_G", "Reports", New With {.ProjectInfoID = Model.id}, FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmDetail_G"})

        @<div class="form-group hidden">
            <div class="">@Html.Hidden("ID_G", 0, New With {.Name = "ID"})</div>
        </div>
        
        @<input type="hidden" class="DayWork" name="DayWork" />
        
        @Html.Hidden("Title", "", New With {.class = "Title_"})
        
        @Html.WriteFormInput(Html.TextBox("Alias", Nothing, New With {.class = "form-control", .disabled = "disabled"}), "Perihal", smControlWidth:=5, lgControlWidth:=5)
        
        @Html.WriteFormInput(Html.TextArea("Value", Nothing, New With {.class = "form-control"}), "Saran / Instruksi / Tanggapan", smControlWidth:=5, lgControlWidth:=5)
    
        @<div class="col-lg-offset-4 col-sm-offset-2">
            <button type="submit" class="btn btn-primary"><i class="fa fa-save"></i> 
                Simpan</button>
            <button type="button" class="btn" id="btnBatal_G" onclick="">
                Batal</button>
        </div>
    End Using
    </div>
</div>