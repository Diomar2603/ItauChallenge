using ItauChallenge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Core.Interfaces
{
    public interface IPosicoesRepository
    {
        Task<IEnumerable<Posicao>> GetByIdUsuarioAsync(int userId);
    }
}
