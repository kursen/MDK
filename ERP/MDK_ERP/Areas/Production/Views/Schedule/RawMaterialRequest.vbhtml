@Code
    ViewData("Title") = "Permintaan Raw Material"
    ViewBag.headIcon = "icon-tasks"
End Code
@imports MDK_ERP.HtmlHelpers
<div class="row">
    <div class='form form-horizontal'>
        <div class="col-xs-6 col-sm-12 col-md-12 col-lg-7">
            <div class="form-group">
                <label class="col-md-2 col-sm-2 col-xs-4 text-left control-label">
                    Material</label>
                <div class="col-md-4 col-sm-4 col-xs-8">
                    <select style="display: none;" tabindex="-1" class="form-control select2" id="MaterialList">
                        <optgroup label="Raw">
                            <option value="121">GROGOL</option>
                            <option value="122">PASIR</option>
                            <option value="123">SIRTU</option>
                        </optgroup>
                        <optgroup label="dalam Proses">
                            <option value="111">BASE KLAS A</option>
                            <option value="112">Base Klas B</option>
                            <option value="113">ASPAL CURAH</option>
                            <option value="115">SPLIT 2/3</option>
                            <option value="116">Medium</option>
                            <option value="118">ABU BATU</option>
                            <option value="119">SPLIT 1-2</option>
                        </optgroup>
                        <optgroup label="Produk">
                            <option value="109">Aspal AC-BC</option>
                            <option value="128">AC-BC (RECOND)</option>
                        </optgroup>
                        <optgroup label="Pembantu">
                            <option value="114">Minyak Solar</option>
                            <option value="120">Minyak Hitam</option>
                            <option value="124">SEENDSEET</option>
                        </optgroup>
                    </select><span style="width: 189px;" dir="ltr" class="select2 select2-container select2-container--default"><span
                        class="selection"><span aria-labelledby="select2-MaterialList-container" aria-owns="select2-MaterialList-results"
                            tabindex="0" class="select2-selection select2-selection--single form-control"
                            role="combobox" aria-autocomplete="list" aria-haspopup="true" aria-expanded="false"><span
                                title="GROGOL" id="select2-MaterialList-container" class="select2-selection__rendered">GROGOL</span><span
                                    class="select2-selection__arrow" role="presentation"><b role="presentation"></b></span></span></span><span
                                        class="dropdown-wrapper" aria-hidden="true"></span></span>
                </div>
            </div>
            <div class="form-group">
                <label class="col-md-2 col-sm-2 col-xs-4 text-left control-label">
                    Periode</label>
                <div class="col-md-4 col-sm-4 col-xs-8">
                    <div class="input-group date" id="datetimepicker">
                        <input value="Juni 2015" class="form-control" placeholder="DD-MM-YYYY" type="button">
                        <span class="input-group-addon"><span class="icon-calendar"></span></span>
                    </div>
                </div>
                <div class="col-md-2 col-sm-2 col-xs-offset-9 col-xs-2 margin-left-min20 margin-top-plus20">
                    <a onclick="" id="filter" href="javascript:void(0)" class="btn btn-primary">Filter</a>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <table class="table table-bordered">
        <thead>
            <tr>
                <th rowspan="2">
                    #
                </th>
                <th colspan="3">
                    Perkiraan
                </th>
                <th colspan="4">
                    Rencana Pembelian
                </th>
               
            </tr>
            <tr>
                <th>
                    Tanggal
                </th>
                <th>
                    Stok
                </th>
                <th>
                    Kebutuhan
                </th>
                  <th>
                    Tanggal
                </th>
                <th>
                    Rencana
                </th>
                <th>
                    Realisasi
                </th>
                 <th >
                    Status
                </th>
            </tr>
        </thead>
    </table>
</div>
