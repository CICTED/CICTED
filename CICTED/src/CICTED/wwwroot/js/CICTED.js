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

    $.fn.subArea = function (dropSubAreas) {
        var dropDown = $(this);
        var subAreaId = dropSubAreas.val();

        dropDown.on('change', function () {
            var areaId = $(this).val();           

                $.ajax({
                    url: '/trabalho/list/subarea/' + areaId,
                    method: 'GET',
                    success: function (data) {
                        console.log(data);
                        $.each(data, function (i, item) {
                            dropSubAreas.append('<option value="' + item.id + '">' + item.nome + '</option>');

                        });
                        dropSubAreas.removeAttr('disabled');
                    },
                    error: function (x, y, message) {
                        console.log(data);
                        dropSubAreas.attr('disabled', 'disabled');
                    }
                });
        });
    };
         
    $.fn.subAreas = function (dropSubAreas) {
        var dropDown = $(this);
        var subAreaId = dropSubAreas.val();

        dropDown.on('click', function () {
            var areaId = $(this).val();

           
                $.ajax({
                    url: '/administrador/list/subarea/' + areaId,
                    method: 'GET',
                    success: function (data) {
                        console.log(data);                     
                        $.each(data, function (i, item) {
                            dropSubAreas.append('<option value="' + item.id + '">' + item.nome + '</option>');

                        });
                        dropSubAreas.removeAttr('disabled');
                    },
                    error: function (x, y, message) {
                        console.log(data);
                        dropSubAreas.attr('disabled', 'disabled');
                    }
                });
        });
    };

})(jQuery);