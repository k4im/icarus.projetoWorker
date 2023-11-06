using icarus.projetoWorker.Entity;
using icarus.projetoWorker.Repository;
using RabbitMQ.Client;

namespace icarus.projetoWorker.RabbitConsumer;

public class QueueConsumer : ConsumerBase, IQueueConsumer
{
    private readonly IConfiguration _config;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IRepoProdutos _repo;

    public QueueConsumer(IConfiguration config,  IRepoProdutos repo)
    {
        _config = config;

        var factory = new ConnectionFactory()
        {
            HostName = _config["RabbitMQ"],
            Port = int.Parse(_config["RabbitPort"]),
            UserName = Environment.GetEnvironmentVariable("RABBIT_MQ_USER"),
            Password = Environment.GetEnvironmentVariable("RABBIT_MQ_PWD"),
        };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            CriarFilas(_channel);
            _connection.ConnectionShutdown += RabbitMQFailed;
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Não foi possivel se conectar ao Message Bus: {e.Message}");
        }
        _repo = repo;
    }
    

    public string VerificarFilaComMensagens()
    {
        if (_channel.MessageCount(filaConsumerDisponiveis) != 0) return filaConsumerDisponiveis;
        if (_channel.MessageCount(filaConsumerAtualizados) != 0) return filaConsumerAtualizados;
        if (_channel.MessageCount(filaConsumerDeletados) != 0) return filaConsumerDeletados;
        return "vazia";
    }
    async Task LogicaDeFilas(ProdutosDisponiveis model, string fila)
    {
        switch (fila)
        {
            case "produtos.disponiveis":
                await _repo.AdicionarProdutos(model);
                break;
            case "produtos.disponiveis.atualizados":
                await _repo.AtualizarProdutos(model.Id, model);
                break;
            case "produtos.disponiveis.deletados":
                await _repo.RemoverProdutos(model.Id);
                break;
            default:
                break;
        }
    }

    void RabbitMQFailed(object sender, ShutdownEventArgs e)
        => Console.WriteLine($"--> Não foi possivel se conectar ao Message Bus: {e}");
}


