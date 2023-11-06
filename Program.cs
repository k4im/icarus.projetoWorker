using icarus.projetoWorker;
using icarus.projetoWorker.RabbitConsumer;
using icarus.projetoWorker.Repository;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScoped<IRepoProdutos, RepoProdutos>();
        services.AddScoped<IQueueConsumer, QueueConsumer>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
