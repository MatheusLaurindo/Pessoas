namespace Pessoas.Server.Model.Base
{
    public class Entidade
    {
        public Guid Id { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; } = null;

        public Entidade()
        {
            Id = Guid.NewGuid();
            DataCadastro = DateTime.UtcNow;
        }

        public void SetDataAtualizacao()
        {
            DataAtualizacao = DateTime.UtcNow;
        }
    }
}
