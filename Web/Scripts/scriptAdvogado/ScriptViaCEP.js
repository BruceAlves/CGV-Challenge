$(document).ready(function () {
    $("#cep").on("change", function () {
        var cep = $(this).val().replace(/\D/g, '');
        if (cep.length === 8) { 
           
            $.getJSON("https://viacep.com.br/ws/" + cep + "/json/", function (data) {
                if (!data.erro) {
                    $("#logradouro").val(data.logradouro);
                    $("#bairro").val(data.bairro);
                    $("#estado").val(data.uf);
                } else {
                    alert("CEP não encontrado.");
                }
            });
        } else {
            alert("CEP inválido. Certifique-se de que ele tenha 8 dígitos.");
        }
    });
});

$(document).ready(function () {
    var cepInput = $("#cep");

    cepInput.keyup(function () {
        var cep = cepInput.val().replace(/\D/g, ''); 
        if (cep.length == 8) {
            cepInput.val(cep.substr(0, 5) + '-' + cep.substr(5, 3));
        }
    });
});
