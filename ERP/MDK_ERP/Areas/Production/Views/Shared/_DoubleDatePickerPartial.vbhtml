
<style type="text/css">
   .mrg-bottom {
       margin-bottom: 15px;
   }
</style>

<div class="row">
    <div class="form form-horizontal">
        <div class="col-xs-12 col-lg-8">
            <div class="form-group">
                <label class="col-md-2 col-xs-2 col-lg-2 control-label text-left">Periode</label>
                <div class="col-lg-4 col-md-3 col-sm-4 col-xs-12">
                    <div class='input-group date col-xs-5 col-md-12 mrg-bottom' id='datetimepicker1'>
                        <input id="startDate" type='button' class="form-control" placeholder="Tanggal Awal" name="startDate" /> <span class="input-group-addon"><span class="icon-calendar"></span></span>
                    </div>
                </div>
                <div class="col-lg-4 col-md-3 col-sm-4 col-xs-12 margin-left-min20">
                    <div class='input-group date col-xs-5 col-md-12 mrg-bottom' id='datetimepicker2'>
                        <input id="endDate" type='button' class="form-control" placeholder="Tanggal Akhir" name="endDate" /> <span class="input-group-addon"><span class="icon-calendar"></span></span>
                    </div>
                </div>
                <div class="col-md-2 col-sm-2 col-xs-5 col-lg-2 margin-left-min20">
                    <a onclick="" id="filterBtn" href="javascript:void(0)" class="btn btn-primary">Filter</a>
                    <span class="loader hidden"></span>
                </div>
            </div>
        </div>
    </div>
</div>