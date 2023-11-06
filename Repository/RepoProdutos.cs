using icarus.projetoWorker.Data;
using icarus.projetoWorker.Entity;
using Microsoft.EntityFrameworkCore;

namespace icarus.projetoWorker.Repository;
public class RepoProdutos : IRepoProdutos
{
    public async Task<bool> AdicionarProdutos(ProdutosDisponiveis model)
    {
          try
        {
            using var db = new DataContext();
            db.ProdutosEmEstoque.AddRange(model);
            await db.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            Console.WriteLine("Não foi possivel realizar a operação, a mesma já foi realizado por um outro usuario!");
            return false;
        }
        catch (ArgumentException)
        {
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Não foi possivel realizar a operação: {e.Message}");
            return false;
        }
    }

    public async Task<bool> AtualizarProdutos(int id, ProdutosDisponiveis model)
    {
        try
        {
            using var db = new DataContext();
            await db.ProdutosEmEstoque.Where(x => x.Id == id)
            .ExecuteUpdateAsync(setter =>
                setter.SetProperty(p => p.Nome, model.Nome)
                .SetProperty(p => p.Quantidade, model.Quantidade));
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            Console.WriteLine("Não foi possivel estar realizando a operação, a mesma já foi realizada por um outro usuario!");
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Não foi possivel estar realizando a operação: {e.Message}");
            return false;
        }
    }

    public async Task<bool> RemoverProdutos(int id)
    {
        try
        {
            using var db = new DataContext();
            await db.ProdutosEmEstoque.Where(x => x.Id == id)
            .ExecuteDeleteAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            Console.WriteLine("Não foi possivel realizar a operação, a mesma já foi realizado por um outro usuario!");
            return false;
        }
        catch (Exception e)
        {
            Console.Write($"Não foi possivel realizar a operação: {e.Message}");
            return false;
        }
    }
}
