namespace Pessoas.Server.Exceptions
{
    public class DominioInvalidoException : Exception
    {
        public DominioInvalidoException() { }
        public DominioInvalidoException(string paramName) : base($"O parâmetro '{paramName} é inválido.'") { }
        public DominioInvalidoException(string paramName, Exception inner) : base($"O parâmetro '{paramName} é inválido.'", inner) { }
    }
}
