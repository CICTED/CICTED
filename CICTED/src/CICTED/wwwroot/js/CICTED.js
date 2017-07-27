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
            dropSubAreas.html('<option value="-1">Selecione a subarea</option>');

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

        dropDown.on('change', function () {
            var areaId = $(this).select2() || [];

           
                $.ajax({
                    url: '/trabalho/list/subarea/' + areaId,
                    method: 'GET',
                    success: function (data) {
                        console.log(data);
                        if (areaId == 1) {
                            dropSubAreas.append('<optgroup label="Ciencias Biologicas"></optgroup>')
                        } else {
                            if (areaId == 2) {
                                dropSubAreas.append('<optgroup label="Ciencias Exatas"></optgroup>')
                            } else {
                                if (areaId == 3) {
                                    dropSubAreas.append('<optgroup label="Ciencias Humanas"></optgroup>')
                                }
                            }
                        }
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