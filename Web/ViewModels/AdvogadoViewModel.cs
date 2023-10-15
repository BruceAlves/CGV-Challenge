using Dominio;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    [Serializable]
    public class AdvogadoViewModel : EnderecoViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public SenioridadeEnum Senioridade { get; set; }

        public EnderecoViewModel Endereco { get; set; }
    }
}
