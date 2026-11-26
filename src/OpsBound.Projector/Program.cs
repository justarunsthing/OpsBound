using OpsBound.Persistence;
using OpsBound.Projector;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddOpsBoundPersistence(builder.Configuration);
builder.Services.AddHostedService<ProjectorWorker>();

var host = builder.Build();
host.Run();
