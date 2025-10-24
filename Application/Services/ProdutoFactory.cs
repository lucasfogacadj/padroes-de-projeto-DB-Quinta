namespace Application.Services;

// TODO (Grupo Factory): Centralizar criação de Produto garantindo invariantes.
// Sugestões de validação: nome não vazio, descricao não vazia, preco > 0, estoque >= 0.
// Discutir se deve lançar ArgumentException, DomainException custom ou retornar Result.
// Explicar no PR por que uma Factory faz sentido (ou se seria overkill neste tamanho) — reflexão.
public static class ProdutoFactory
{
    public static Produto Criar(string nome, string descricao, decimal preco, int estoque)
    {

        // Validações de entrada
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome não pode ser vazio.", nameof(nome));
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição não pode ser vazia.", nameof(descricao));
        if (preco <= 0)
            throw new ArgumentException("Preço deve ser maior que zero.", nameof(preco));
        if (estoque < 0)
            throw new ArgumentException("Estoque não pode ser negativo.", nameof(estoque));

        var nomeLimpo = nome.Trim();
        var descricaoLimpa = descricao.Trim();
        var precoArredondado = Math.Round(preco, 2);
        var estoqueFinal = Math.Max(estoque, 0);
        var produto = new Produto();
        produto.Nome = nomeLimpo;
        produto.Descricao = descricaoLimpa;
        produto.Preco = precoArredondado;
        produto.Estoque = estoqueFinal;
        produto.DataCriacao = DateTime.Now;

        return produto;
    }
}

/*
Uma Factory pode ser considerada overengineering nas seguintes situações:

1. Quando a criação do objeto é simples e não requer validações complexas
2. Quando há poucos objetos sendo criados no sistema
3. Quando não há variações na lógica de criação
4. Quando o objeto não tem invariantes importantes para manter
5. Em projetos pequenos onde a complexidade adicional não traz benefícios claros

Neste caso específico, a Factory se justifica pois:
- Centraliza validações importantes
- Garante invariantes do domínio (preço > 0, estoque >= 0)
- Padroniza a criação de produtos em todo sistema
- Facilita manutenção futura das regras de criação
*/