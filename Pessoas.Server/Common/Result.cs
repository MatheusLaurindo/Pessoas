namespace Pessoas.Server.Common
{
    public record Result<T>
    {
        public T Valor { get; init; }
        public bool FoiSucesso { get; init; }
        public string Mensagem { get; init; }

        public Result(bool foiSucesso, T valor, string mensagem)
        {
            this.FoiSucesso = foiSucesso;
            this.Valor = valor;
            this.Mensagem = mensagem;
        }

        public static Result<T> Sucesso(T value)
        {
            return new Result<T>(foiSucesso: true, value, null);
        }

        public static Result<T> Falha(string error)
        {
            return new Result<T>(foiSucesso: false, default(T), error);
        }
    }
}
