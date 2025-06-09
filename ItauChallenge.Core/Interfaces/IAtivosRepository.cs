using ItauChallenge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Core.Interfaces
{
    public interface IAtivosRepository
    {
        public Task<Ativo>  GetByIdAsync(int ativoId);
        public Task<Ativo>  GetByCodigoAsync(string codigoDoAtivo);
    }
}
