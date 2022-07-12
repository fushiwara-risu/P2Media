using P2Media.Core;
using System.Net;
using System.Net.Sockets;

namespace P2Media.Tests;

public class NodeTests {
	private readonly IPHostEntry _remoteServerEP = Dns.GetHostEntry("risu.tech");
	private readonly IPEndPoint _serverEP = new(IPAddress.Loopback, 8069);
	private readonly IPEndPoint _clientEP = new(IPAddress.Loopback, 8068);

	[Fact]
	public void ServerCreation() {
		Node server = new(_serverEP);
		Assert.NotNull(server);
	}

	[Fact]
	public void ClientCreation() {
		Node client = new(_clientEP);
		Assert.NotNull(client);
	}

	[Fact]
	public async Task LocalClientLocalServerConnect() {
		Node server = new(_serverEP);
		Node client = new(_clientEP);

		await client.ConnectAsync(_serverEP);
		await server.AcceptConnectionAsync();

		Assert.NotEmpty(server.ConnectedPeers);
		Assert.NotEmpty(client.ConnectedPeers);
	}

	[Fact]
	public async Task LocalClientObtainLocalServerConnections() {
		Node server = new(_serverEP);
		List<Node> Clients = new();
		for (int i = 0; i < 5; i++) {
			Clients.Add(new(new(IPAddress.Loopback, 8060 + i)));
		}
		foreach (Node client in Clients) {
			await client.ConnectAsync(_serverEP);
			await server.AcceptConnectionAsync();
		}

		Node c1 = new(_clientEP);
		await c1.ConnectAsync(_serverEP);
		await server.AcceptConnectionAsync();
		foreach (TcpClient x in server.ConnectedPeers) c1.ConnectedPeers.Add(x);
		
		Assert.Equal(7, c1.ConnectedPeers.Count);
	}

	[Fact]
	public void LocalClientLocalServerLocalClientConnect() {

	}
}