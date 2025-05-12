using Pessoas.Server.Common;
using Pessoas.Server.Enuns;
using Pessoas.Server.Exceptions;

namespace Pessoas.Server.Model
{
    public class UsuarioPermissao
    {
        public Guid UsuarioId { get; protected set; }
        public Usuario Usuario { get; protected set; }
        public Permissao Permissao { get; protected set; }

        protected UsuarioPermissao()
        {
        }

        protected UsuarioPermissao(Guid usuarioId, Permissao permissao)
        {
            SetUsuario(usuarioId);
            SetPermissao(permissao);
        }

        public void SetUsuario(Guid usuarioId)
        {
            if (usuarioId == Guid.Empty)
                throw new DominioInvalidoException(nameof(usuarioId));

            UsuarioId = usuarioId;
        }

        public void SetPermissao(Permissao permissao)
        {
            Permissao = permissao;
        }

        public static Result<UsuarioPermissao> Create(Guid usuarioId, Permissao permissao)
        {
            return Result<UsuarioPermissao>.Sucesso(new UsuarioPermissao(usuarioId, permissao));
        }
    }
}
