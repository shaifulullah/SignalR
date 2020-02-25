$(function () {
    jQuery.validator.setDefaults({
        submitHandler: function (form) { form.submit(); }
    });

    $(document).ajaxError(function (event, jqXHR, ajaxSettings, thrownError) {
        var json = $.parseJSON(jqXHR.responseText);
        window.location.pathname = json.url;
    });
})