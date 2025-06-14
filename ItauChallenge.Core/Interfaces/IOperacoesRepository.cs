﻿using ItauChallenge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItauChallenge.Core.Interfaces
{
    public interface IOperacoesRepository
    {
        public Task<IEnumerable<Operacao>> GetByIdUsuarioAsync(int userId);
        public Task<IEnumerable<Operacao>> GetComprasPorUsuarioEAtivoAsync(int usuarioId, int ativoId);

        public Task<IEnumerable<Operacao>> GetAllAsync();
        public Task<decimal> GetSomaTotalCorretagemAsync();

    }
}
