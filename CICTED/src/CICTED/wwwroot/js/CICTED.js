(function ($) {

    $.fn.states = function (dropCities) {
        var dropDown = $(this);

        dropDown.on('change', function () {
            var stateID = $(this).val();
            dropCities.html('<option value="-1">Selecione sua cidade</option>');

            $.ajax({
                url: '/localization/list/cities/' + stateID,
                method: 'GET',
                success: function (data) {
                    $.each(data, function (i, item) {
                        dropCities.append('<option value="' + item.id + '">' + item.name + '</option>');
                    });

                    dropCities.removeAttr('disabled');
                },
                error: function (x, y, message) {
                    dropCities.attr('disabled', 'disabled');
                }
            });
        });
    };
})(jQuery);