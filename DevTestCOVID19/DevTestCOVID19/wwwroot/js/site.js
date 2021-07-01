// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code. 
$(function () {
    $('#table-container').load(loadGridUrl);

    $('#btnReset').on('click', function () {
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
                    $('#table-container').html('');
                    $('#table-container').load(`${loadGridByRegionUrl}?region=${regionISO}`, function () {
                        $('#btnReset').attr('disabled', false);
                    });                    
                }
            });
        }
    })
}())