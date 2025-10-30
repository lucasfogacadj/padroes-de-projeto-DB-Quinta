using Application.Interfaces;

using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Produtos
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Produto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Produtos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task AddAsync(Produto produto, CancellationToken ct = default)
        {
            await _context.Produtos.AddAsync(produto, ct);
        }

        public Task RemoveAsync(Produto produto, CancellationToken ct = default)
        {
            _context.Produtos.Remove(produto);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
