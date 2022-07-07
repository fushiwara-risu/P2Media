using System.Net;
using System.Net.Sockets;

namespace P2Media.Core;
public class Client: IDisposable {
	private readonly TcpClient _client;

	public List<TcpClient> ConnectedPeers { get; private set; }

	public Client(IPEndPoint ip) {
		_client = new() {
			Client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
		};

		_client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
		_client.Client.Bind(ip);

		ConnectedPeers = new();
	}

	~Client() => Dispose();
	
	public async Task ConnectAsync(string host, int port = 8069) => await ConnectAsync(IPEndPoint.Parse($"{host}:{port}"));
	public async Task ConnectAsync(IPEndPoint ip) {
		TcpClient tmpClient = new();
		tmpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
		await tmpClient.ConnectAsync(ip);
		ConnectedPeers.Add(tmpClient);
	}

	public void Dispose() {
		_client.Close();
		_client.Dispose();
	}
}
