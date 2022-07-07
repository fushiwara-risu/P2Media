using System.Net;
using System.Net.Sockets;

namespace P2Media.Core;
public class Node {
	private readonly TcpListener _listener;
	public List<TcpClient> ConnectedPeers { get; private set; }
	public Node(IPEndPoint endPoint) {
		_listener = new(endPoint);
		ConnectedPeers = new();
	}

	~Node() => Stop();
	
	public void Start() => _listener.Start();
	public void Stop() => _listener.Stop();

	public async Task<TcpClient> AcceptConnectionAsync() {
		TcpClient client = await _listener.AcceptTcpClientAsync();
		if (!ConnectedPeers.Contains(client)) ConnectedPeers.Add(client);
		return client;
	}

	public static async Task ConnectAsync(string host, int port = 8069) => await ConnectAsync(IPEndPoint.Parse($"{host}:{port}"));
	public static async Task ConnectAsync(IPEndPoint ip) {
		TcpClient tmpClient = new();
		tmpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
		await tmpClient.ConnectAsync(ip);
	}
}
