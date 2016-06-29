@Code
    ViewData("Title") = "MonthReport"
End Code
@Using Html.BeginJUIBox("Laporan Bulanan")
@*  @<div id='bottomNavBar' style="width: 50%; position: fixed; bottom: 0px; right: 30px;
        height: 50px; z-index: 1200;">
        <div style='padding: 13px; background-color: #6699FF; border-radius: 5px;'>
            <div class="scroll-bar-wrap ui-widget-content ui-corner-bottom">
            </div>
            <div class="scroll-bar">
            </div>
        </div>
    </div>*@
     @<div style="position: relative">
   <table id="tb_Data" class="table table-bordered" style="width: 100%; table-layout: fixed;
        background-color: #fff;">
        <thead>
            <tr>
                <th colspan="2" class="text-center" style="width: 48%;">
                    <h4>
                        Pemerintah Provinsi Sumatera Barat</h4>
                    <h3>
                        Dinas Prasarana Jalan</h3>
                    <h3>
                        Tata Ruang Dan Pemukiman</h3>
                </th>
                <th class="text-center" colspan="6">
                    <h1 style="margin-top: -20%;">
                        Laporan Bulanan</h1>
                </th>
                <th rowspan="2" colspan="5">
                    <div style="margin-top: -55%;">
                        <h5>
                            KONTRAKTOR : PT. ANANDA PRATAMA</h5>
                        <h5>
                            NO. KONTRAK : 609/90/KTR/KPA-JJ/IV/2015
                        </h5>
                        <h5>
                            TANGGAL KONTRAK : 14 APRIL 2015</h5>
                        <h5>
                            NILAI KONTRAK : Rp. 13.310.157.000.-</h5>
                        <h5>
                            NO. KONTRAK ADD.I :
                        </h5>
                        <h5>
                            TANGGAL KONTRAK ADD.I :
                        </h5>
                        <h5>
                            NILAI KONTRAK ADD-01
                        </h5>
                        <h5>
                            MASA WAKTU : 180 HARI KALENDER</h5>
                    </div>
                </th>
            </tr>
            <tr>
                <td colspan="2">
                    <h5>
                        PELAKSANA KEGIATAN : PEMBANGUNAN JALAN PROVINSI KABUPATEN PASAMAN DAN KABUPATEN
                        PASAMAN BARAT</h5>
                    <h5>
                        NAMA PAKET : Pembangunan Jalan Lubuk Sikaping - Talu (SP.157)</h5>
                    <h5>
                        KONSULTAN SUPERVISI : PT Dhanesmantara Consultan Jo PT. Seecons</h5>
                </td>
                <td colspan="6">
                    <h5>
                        BULAN KE : 01(SATU)</h5>
                    <h5>
                        PERIODE : 14 APRIL S/D 3 MEI 2015
                    </h5>
                </td>
            </tr>
            <tr>
                <th rowspan="2">
                    NOMOR MATA PEMBAYARAN
                </th>
              <th rowspan="2">
                    URAIAN PEKERJAAN
                </th>
               <th rowspan="2">
                    Sat
                </th>
              <th rowspan="2">
                    HARGA SATUAN (Rp)
                </th>
                <th colspan="3">
                    KONTRAK AWAL
                </th>
                <th colspan="2">
                    S/D BULAN LALU
                </th>
              <th colspan="2">
                    BULAN INI
                </th>
              <th colspan="2">
                    S/D BULAN INI
                </th>
            </tr>
            <tr>
                <td>
                    KUANTITAS
                </td>
                <td>
                    JUMLAH HARGA (Rp)
                </td>
                <td>
                    BOBOT (%)
                </td>
                <td>
                    KUANTITAS
                </td>
                <td>
                    BOBOT (%)
                </td>
                  <td>
                    KUANTITAS
                </td>
                <td>
                    BOBOT (%)
                </td>
                  <td>
                    KUANTITAS
                </td>
                <td>
                    BOBOT (%)
                </td>
            </tr>
        </thead>
        <tbody>
         <tr>
            <td></td>
            <td> </td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                DIVISI 1 : UMUM
            </td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
          <tr>
            <td>1.2</td>
            <td>Mobilisasi</td>
            <td>Ls</td>
            <td>56,665,000.00</td>
            <td>1.00</td>
            <td>56,665,000.00</td>
            <td>0.47</td>
            <td>-</td>
            <td>-</td>
            <td>0.60</td>
            <td>0.28</td>
            <td>0.60</td>
            <td>0.28</td>
        </tr>
        </tbody>
    </table>
 </div>
End Using
<style>
    .double_top_border
    {
        border-top-style: double;
    }
    .double_bottom_border
    {
        border-bottom-style: double;
    }
    
    #divWrapperTopTop
    {
        position: fixed;
        overflow: hidden;
        z-index: 1002;
        background-color: white;
        top: 0px;
        padding: 0px;
        margin: 0px;
        top: 80px;
    }
    
    #divWrapperTopLeft
    {
        position: absolute;
        top: 0px;
        padding: 90px 0px 0px 0px;
        z-index: 0;
        width: 565px;
    }
    #divWrapperTopRight
    {
        position: fixed;
        overflow: hidden;
        z-index: 1002;
        background-color: white;
        top: 0px;
        padding: 0px;
        margin: 0px;
        top: 80px;
    }
    #divWrapperTable
    {
        overflow: hidden;
        padding-top: 90px;
        margin: 0px;
    }
    
    .scroll-bar-wrap
    {
        clear: left;
        padding: 0 4px 0 2px;
        margin: 0 -1px -1px -1px;
    }
    .scroll-bar-wrap .ui-slider
    {
        background: none;
        border: 0;
        height: 2em;
        margin: 0 auto;
    }
    .scroll-bar-wrap .ui-handle-helper-parent
    {
        position: relative;
        width: 100%;
        height: 100%;
        margin: 0 auto;
    }
    .scroll-bar-wrap .ui-slider-handle
    {
        top: .2em;
        height: 1.5em;
    }
    .scroll-bar-wrap .ui-slider-handle .ui-icon
    {
        margin: -8px auto 0;
        position: relative;
        top: 50%;
    }
    
    .cutofftext
    {
        width: 420px;
        white-space: nowrap;
        text-overflow: ellipsis;
        overflow: hidden;
    }
</style>