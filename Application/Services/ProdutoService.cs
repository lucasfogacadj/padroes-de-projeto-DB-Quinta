using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

// TODO (Grupo Service): Implementar regras de negócio aqui.
// NÃO colocar detalhes de EF Core. Usar apenas a abstração IProdutoRepository.
// Integrar posteriormente com validações (FluentValidation) e Factory.
// Sugerido: lançar exceções de domínio específicas ou retornar Result Pattern (opcional, comentar no PR).
public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _repo;

    public ProdutoService(IProdutoRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Produto>> ListarAsync(CancellationToken ct = default)
    {
        // TODO: Adicionar possibilidade de filtros futuros (Specification Pattern em fases posteriores)
        return await _repo.GetAllAsync(ct);
    }

    public async Task<Produto?> ObterAsync(int id, CancellationToken ct = default)
    {
        // TODO: Validar id > 0 e talvez normalizar algum aspecto.
        if (id <= 0)
            return null;

        return await _repo.GetByIdAsync(id, ct);        
    }

    public async Task<ProdutoReadDto> CriarAsync(string nome, string descricao, decimal preco, int estoque, CancellationToken ct = default)
    {
        // TODO: Integrar com ProdutoFactory.Criar e depois persistir via repository.
        // TODO: Tratar regras: nome não vazio, preço > 0, estoque >= 0, trimming.

        if (nome == "" || preco <= 0 || estoque >= 0)
            throw new NotImplementedException();

        var produto = ProdutoFactory.Criar(nome, descricao, preco, estoque);

        await _repo.AddAsync(produto);
        var produtoDTO = MappingExtensions.ToReadDto(produto);
        return produtoDTO;
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        // TODO: Buscar, validar existência e remover.
        if (id <= 0)
        {
            return false;
        }

        var produtoParaRemover = await _repo.GetByIdAsync(id, ct);

        if (produtoParaRemover == null)
        {
            return false; 
        }

        await _repo.RemoveAsync(produtoParaRemover, ct);

        return true;

        throw new NotImplementedException();
    }
    
}
