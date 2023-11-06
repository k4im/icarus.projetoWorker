using icarus.projetoWorker.Entity;

namespace icarus.projetoWorker.Repository;

public interface IRepoProdutos
{
    Task<bool> RemoverProdutos(int id);
    Task<bool> AdicionarProdutos(ProdutosDisponiveis model);
    Task<bool> AtualizarProdutos(int id, ProdutosDisponiveis model);
}
