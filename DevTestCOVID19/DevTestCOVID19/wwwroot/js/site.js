var DevTestApp = (function () {
    let tableContainer ='#table-container';
    let $ddlRegion = $('#ddlRegion');
    let $btnReport = $('#btnReport');
    let $btnReset = $('#btnReset');
    let $frmExportXML = $('#frmExportXML');
    let $frmExportJSON = $('#frmExportJSON');
    let $frmExportCSV = $('#frmExportCSV');

    let loading  = function(selector) {
        $(selector).html('<div class="d-flex align-items-center justify-content-center"> \
                        <div class= "spinner-border text-primary" style="width: 3rem; height: 3rem;" role="status">\
                            <span class="sr-only">Loading...</span>\
                        </div>\
                         <strong class="ml-3">Loading...</strong>\
                     </div>');
    }

    let loadGridCallback = function () {
        $frmExportXML.attr('action', '/Home/ExportXMLByRegion');
        $frmExportJSON.attr('action', '/Home/ExportJSONByRegion');
        $frmExportCSV.attr('action', '/Home/ExportCSVByRegion');
    }

    let loadGridCallbackByRegion = function () {
        var regionISO = $ddlRegion.val();
        $btnReset.attr('disabled', false);
        $frmExportXML.attr('action', `/Home/ExportXMLByProvince?region=${regionISO}`);
        $frmExportJSON.attr('action', `/Home/ExportJSONByProvince?region=${regionISO}`);
        $frmExportCSV.attr('action', `/Home/ExportCSVByProvince?region=${regionISO}`);
    }

    let getReportByRegion = function () {
        var regionISO = $ddlRegion.val();
        if (regionISO) {
            loading(tableContainer);
            $(tableContainer).load(`${loadGridByRegionUrl}?region=${regionISO}`, loadGridCallbackByRegion);
        }
    }

    let resetReportByRegion = function () {
        loading(tableContainer);
        $(tableContainer).load(loadGridUrl, loadGridCallback);
        $ddlRegion.val('');
        $btnReport.off('click').attr('disabled', true);
        $btnReset.attr('disabled', true);
    }

    let changeRegion = function (evt) {
        $btnReport.off('click');
        if (!$(evt.target).val()) {
            $btnReport.attr('disabled', true);
        } else {
            $btnReport.attr('disabled', false);
            $btnReport.on('click', getReportByRegion);
        }
    }

    let init = function () {
        loading(tableContainer);
        $(tableContainer).load(loadGridUrl, loadGridCallback);
        $btnReset.on('click', resetReportByRegion);
        $ddlRegion.on('change', changeRegion);
    }

    return {
        initialize: init
    }
}());

DevTestApp.initialize();