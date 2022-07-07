using System.Net;
using System.Net.Sockets;

namespace P2Media.Core;

/// <summary>
/// Class to represent a server configuration
/// </summary>
public class Server : IDisposable {
	private readonly TcpListener _listener;

	public List<TcpClient> ConnectedClients { get; private set; }

	public Server(IPEndPoint ip) {
		_listener = new(ip);
		_listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
		ConnectedClients = new();
		Start();
	}

	~Server() => Dispose();

	public void Dispose() {
		Stop();
		GC.SuppressFinalize(this);
	}

	public void Start() => _listener.Start();
	public void Stop() => _listener.Stop();

	public async Task TraverseAsync() {
		TcpClient client = await _listener.AcceptTcpClientAsync();
		if (!ConnectedClients.Contains(client)) ConnectedClients.Add(client);
	}

	
}