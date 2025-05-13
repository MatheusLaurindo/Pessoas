namespace Pessoas.Server.Common
{
    public class APIPaginatedResponse<T>
    {
        public int Total { get; set; }
        public T Data { get; set; }

        public APIPaginatedResponse()
        {
            Total = 0;
            Data = default!;
        }

        public static APIPaginatedResponse<T> Create(T data, int total)
        {
            return new APIPaginatedResponse<T>
            {
                Data = data,
                Total = total
            };
        }
    }
}
