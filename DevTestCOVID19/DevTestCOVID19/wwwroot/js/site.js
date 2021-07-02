// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code. 
$(function () {
    loading('#table-container');
    $('#table-container').load(loadGridUrl);

    $('#btnReset').on('click', function () {
        loading('#table-container');
        $('#table-container').load(loadGridUrl);
        $('#ddlRegion').val('');
        $('#btnReport').off('click');
        $('#btnReport').attr('disabled', true);
        $('#btnReset').attr('disabled', true);
    });

    $('#ddlRegion').on('change', function (e) {
        $('#btnReport').off('click');
        if (!$(e.target).val()) {
            $('#btnReport').attr('disabled', true);
        } else {
            $('#btnReport').attr('disabled', false);
            $('#btnReport').on('click', function () {
                var regionISO = $('#ddlRegion').val();
                if (regionISO) {
                    loading('#table-container');
                    $('#table-container').load(`${loadGridByRegionUrl}?region=${regionISO}`, function () {
                        $('#btnReset').attr('disabled', false);
                    });
                }
            });
        }
    })
}());

function loading(selector) {
    $(selector).html('<div class="d-flex align-items-center justify-content-center"> \
                        <div class= "spinner-border text-primary" style="width: 3rem; height: 3rem;" role="status">\
                            <span class="sr-only">Loading...</span>\
                        </div>\
                         <strong class="ml-3">Loading...</strong>\
                     </div>');
}
