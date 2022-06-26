using P2Media.Service;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices(services =>
	{
		services.AddHostedService<Worker>();
	})
	.Build();

await host.RunAsync();
