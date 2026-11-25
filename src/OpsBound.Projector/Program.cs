using OpsBound.Projector;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ProjectorWorker>();

var host = builder.Build();
host.Run();
