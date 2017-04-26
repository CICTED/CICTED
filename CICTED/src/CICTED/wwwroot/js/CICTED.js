(function ($) {

    $.fn.states = function (dropCidades) {
        var dropDown = $(this);

        dropDown.on('change', function () {
            var estadoID = $(this).val();
            dropCidades.html('<option value="-1">Selecione sua cidade</option>');

            $.ajax({
                url: '/localizacao/lista/cidades/' + estadoID,
                method: 'GET',
                success: function (data) {
                    $.each(data, function (i, item) {
                        dropCidades.append('<option value="' + item.id + '">' + item.cidadeNome + '</option>');
                    });

                    dropCidades.removeAttr('disabled');
                },
                error: function (x, y, message) {
                    dropCidades.attr('disabled', 'disabled');
                }
            });
        });
    };
})(jQuery);