using OpsBound.Contracts.Kafka;
using OpsBound.Publisher;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton(KafkaOptions.Bind(builder.Configuration));
builder.Services.AddHostedService<PublisherWorker>();

var host = builder.Build();
host.Run();
