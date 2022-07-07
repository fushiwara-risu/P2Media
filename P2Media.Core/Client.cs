using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P2Media.Core;
internal class Client {
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

	public async Task ConnectAsync(string host, int port) {
		TcpClient tmpClient = new();
		tmpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
		await tmpClient.ConnectAsync(host, port);
		ConnectedPeers.Add(tmpClient);
	}
}
