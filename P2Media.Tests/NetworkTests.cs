using P2Media.Core;
using System.Net;

namespace P2Media.Tests;

public class NetworkTests {
	private readonly IPHostEntry _remoteServerEP = Dns.GetHostEntry("risu.tech");
	private readonly IPEndPoint _serverEP = new(IPAddress.Loopback, 8069);
	private readonly IPEndPoint _clientEP = new(IPAddress.Loopback, 8068);

	[Fact]
	public void ServerCreation() {
		Server server = new(_serverEP);
		Assert.NotNull(server);
	}

	[Fact]
	public void ClientCreation() {
		Client client = new(_clientEP);
		Assert.NotNull(client);
	}

	[Fact]
	public async Task LocalClientLocalServerConnect() {
		Server server = new(_serverEP);
		Client client = new(_clientEP);

		await client.ConnectAsync(_serverEP);
		await server.TraverseAsync();

		Assert.NotEmpty(server.ConnectedClients);
		Assert.NotEmpty(client.ConnectedPeers);
	}

	[Fact]
	public async Task LocalClientObtainLocalServerConnections() {
		Server server = new(_serverEP);
		List<Client> Clients = new();
		for (int i = 0; i < 5; i++) {
			Clients.Add(new(new(IPAddress.Loopback, 8060 + i)));
		}
		foreach (Client client in Clients) {
			await client.ConnectAsync(_serverEP);
			await server.TraverseAsync();
		}

		Client c1 = new(_clientEP);

	}

	[Fact]
	public void LocalClientLocalServerLocalClientConnect() {

	}
}