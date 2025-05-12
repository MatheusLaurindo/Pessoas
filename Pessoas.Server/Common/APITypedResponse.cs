namespace Pessoas.Server.Common
{
    public class APITypedResponse<T>
    {
        public APITypedResponse()
        {
            Successo = true;
            Mensagem = string.Empty;
        }
        public bool Successo { get; set; }
        public string Mensagem { get; set; }
        public T Data { get; set; }
        public static APITypedResponse<T> Create(T data, bool sucesso, string mensagem)
        {
            return new APITypedResponse<T>
            {
                Data = data,
                Successo = sucesso,
                Mensagem = mensagem
            };
        }
    }
}
