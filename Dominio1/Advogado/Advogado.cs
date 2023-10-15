using System;

namespace Dominio
{
    [Serializable]
    public class Advogado : Endereco
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public SenioridadeEnum Senioridade { get; set; }
        public Endereco endereco { get; set; }
    }
}
