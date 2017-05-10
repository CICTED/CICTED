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
        var drpDown = $(this);
        var subAreaId = dropSubAreas.val();

        dropDown.on('change', function () {
            var areaId = $(this).val();
            dropSubAreas.html('<option value="-1">Selecione a subarea</option>');

            $.ajax({
                url: '/trabalho/list/subarea',
                method: 'GET',
                sucess: function (data) {
                    $.each(data, function (i, item) {
                        if (item.id == subAreaId) {
                            dropSubAreas.append('<option value="'+item.id+'" selected>'+item.subArea+'</option>');
                        }else{
                            dropSubAreas.append('<option value="'+item.id+'">'+item.subArea+'"</option>');
                        }
                    });
                    dropSubAreas.removeAttr('disabled');
                },
                error: function(x,y,message){
                    dropSubAreas.attr('disabled','disabled');
                }
            });
        });
    };
                           
})(jQuery);