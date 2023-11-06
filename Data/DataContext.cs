using icarus.projetoWorker.Entity;
using Microsoft.EntityFrameworkCore;

namespace icarus.projetoWorker.Data;

public class DataContext : DbContext
{
    public DataContext()
    { }

    public DataContext(DbContextOptions options) : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {   
            var DbConnection = Environment.GetEnvironmentVariable("DB_CONNECTION");
            var ServerVersion = new MySqlServerVersion(new Version(8,0,31));
            optionsBuilder.UseMySql(DbConnection, ServerVersion);            
        }
    }
  
    public DbSet<ProdutosDisponiveis> ProdutosEmEstoque { get; set; }
}
