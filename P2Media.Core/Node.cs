using System.Net;
using System.Net.Sockets;

namespace P2Media.Core;
public class Node: IDisposable {
	private readonly TcpListener _listener;
	public List<TcpClient> ConnectedPeers { get; private set; }
	public Node(IPEndPoint endPoint) {
		_listener = new(endPoint);
		ConnectedPeers = new();
		Start();
	}

	public void Dispose() {
		Stop();
		GC.SuppressFinalize(this);
	}

	public void Start() => _listener.Start();
	public void Stop() => _listener.Stop();
	
	public async Task<TcpClient> AcceptConnectionAsync() {
		TcpClient client = await _listener.AcceptTcpClientAsync();
		if (!ConnectedPeers.Contains(client)) ConnectedPeers.Add(client);
		return client;
	}

	public async Task ConnectAsync(string host, int port) => await ConnectAsync(IPEndPoint.Parse($"{host}:{port}"));
	public async Task ConnectAsync(IPEndPoint ip) {
		TcpClient client = new();
		client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
		await client.ConnectAsync(ip);
		if (!ConnectedPeers.Contains(client) && client.Connected) ConnectedPeers.Add(client);
	}
}
