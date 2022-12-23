using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UmbralRealm.Core.Network;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Core.Network.Packet.Model.Generic;
using UmbralRealm.Core.Security;
using UmbralRealm.Core.Utilities;
using UmbralRealm.Login.Packet.Server;

namespace UmbralRealm.Proxy
{
    internal class Program
    {
        /// <summary>
        /// Proxy class because .NET has an issue with starting multiple hosted instances with the same type name.
        /// https://github.com/dotnet/runtime/issues/38751
        /// </summary>
        private class LoginServer : SocketServer
        {
            public LoginServer(SocketWrapper listener, IPEndPoint endPoint, NetworkCertificate certificate, IConnectionFactory connectionFactory) 
                : base(listener, endPoint, certificate, connectionFactory) { }
        }

        /// <summary>
        /// Proxy class because .NET has an issue with starting multiple hosted instances with the same type name.
        /// https://github.com/dotnet/runtime/issues/38751
        /// </summary>
        private class WorldServer : SocketServer
        {
            public WorldServer(SocketWrapper listener, IPEndPoint endPoint, NetworkCertificate certificate, IConnectionFactory connectionFactory)
                : base(listener, endPoint, certificate, connectionFactory) { }
        }


        private static uint _realWorldIp;
        private static ushort _realWorldPort;


        static BufferBlock<IWriteConnection> worldRequestQueue = new();
        static BufferBlock<IWriteConnection> worldResponseQueue = new();

        static void Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices(services =>
                {
                    var loginOpcodeMapping = OpcodeMapping.Create(new Login.Packet.PacketOpcode());
                    

                    // Setup queues
                    var requestQueue = new BufferBlock<IWriteConnection>();
                    var responseQueue = new BufferBlock<IWriteConnection>();
                    var packetQueue = new BufferBlock<IPacket>();

                    packetQueue.LinkTo(new ActionBlock<IPacket>(packet => HandlePacket(packet, loginOpcodeMapping)));

                    // Setup server
                    var ipAddress = IPAddress.Parse("127.0.0.1");
                    var endpoint = new IPEndPoint(ipAddress, 20000);
                    var listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    var listenerAdapter = new SocketWrapper(listenerSocket);
                    var certificate = NetworkCertificate.CreatePrivateAsync().Result;

                    var activator = new ActivatorWrapper();
                    var packetFactory = new PacketConverter(loginOpcodeMapping, activator);
                    var connectionFactory = new ConnectionFactory(packetFactory);

                    var server = new LoginServer(listenerAdapter, endpoint, certificate, connectionFactory);

                    // Setup proxy
                    var remoteIp = IPAddress.Parse("0.0.0.0");
                    var remoteEndpoint = new IPEndPoint(remoteIp, 6543);
                    var socketFactory = new SocketConnectionFactory();

                    var proxy = new SocketClient(socketFactory, remoteEndpoint, requestQueue, connectionFactory);

                    proxy.Subscribe(packetQueue.AsObserver());
                    proxy.PacketReceivedHandler = StartWorldProxy;

                    server.Subscribe(requestQueue.AsObserver());
                    server.Subscribe(responseQueue.AsObserver());

                    services.AddHostedService(provider => server);




                    // World
                    var worldEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20001);
                    var worldServer = Program.CreateSocketServer(worldEndpoint, ServerType.World, worldRequestQueue, worldResponseQueue);

                    services.AddHostedService(provider => worldServer);
                })
                .UseConsoleLifetime()
                .Build();

            host.Run();

            Console.WriteLine("Hello, World!");
        }

        private static void HandlePacket(IPacket packet, OpcodeMapping mapping)
        {
            if (mapping.TryGetByModel(packet.GetType(), out var map))
            {
                Console.WriteLine($"ServerType: {map.ServerType}");
                Console.WriteLine($"Origin: {map.Origin}");

                if (map.ServerType == ServerType.Login)
                {
                    var opcode = (Login.Packet.PacketOpcode)map.Opcode;
                    Console.WriteLine($"Opcode: {opcode}");
                }

                if (map.ServerType == ServerType.World)
                {
                    var opcode = (World.Packet.PacketOpcode)map.Opcode;
                    Console.WriteLine($"Opcode: {opcode}");
                }
            }
            else if(packet is UnknownPacket unknownPacket)
            {
                //Console.WriteLine($"ServerType: {unknownPacket.ServerType}");
                Console.WriteLine($"Origin: {unknownPacket.Origin}");
                Console.WriteLine($"Opcode: {unknownPacket.Opcode}");
            }

            var data = packet.Serialize();

            Console.WriteLine($"{BitConverter.ToString(data)}");
            Console.WriteLine();
        }

        private static WorldServer CreateSocketServer(IPEndPoint endpoint, ServerType serverType, BufferBlock<IWriteConnection> requestQueue, BufferBlock<IWriteConnection> responseQueue)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var socketAdapter = new SocketWrapper(socket);

            var certificate = NetworkCertificate.CreatePrivateAsync().Result;

            var activator = new ActivatorWrapper();
            var worldOpcodeMapping = OpcodeMapping.Create(new World.Packet.PacketOpcode());

            var packetFactory = new PacketConverter(worldOpcodeMapping, activator);
            var connectionFactory = new ConnectionFactory(packetFactory);

            var server = new WorldServer(socketAdapter, endpoint, certificate, connectionFactory);

            server.Subscribe(requestQueue.AsObserver());
            server.Subscribe(responseQueue.AsObserver());

            return server;
        }

        private static void StartWorldProxy(IPacket packet, IWriteConnection client)
        {
            if (packet is WorldConnectionPacket worldConnectionPacket)
            {
                _realWorldIp = worldConnectionPacket.WorldIPAddress;
                _realWorldPort = worldConnectionPacket.WorldIPPort;

                var activator = new ActivatorWrapper();
                var worldOpcodeMapping = OpcodeMapping.Create(new World.Packet.PacketOpcode());
                var packetFactory = new PacketConverter(worldOpcodeMapping, activator);
                var connectionFactory = new ConnectionFactory(packetFactory);

                // proxy
                var remoteEndpoint = new IPEndPoint(_realWorldIp, _realWorldPort);
                var socketConnectionFactory = new SocketConnectionFactory();

                
                var packetQueue = new BufferBlock<IPacket>();

                packetQueue.LinkTo(new ActionBlock<IPacket>(packet => HandlePacket(packet, worldOpcodeMapping)));

                var proxy = new SocketClient(socketConnectionFactory, remoteEndpoint, worldRequestQueue, connectionFactory);
                proxy.PacketReceivedHandler = InjectRealWorldConnection;

                proxy.Subscribe(packetQueue.AsObserver());

                worldConnectionPacket.WorldIPAddress = BitConverter.ToUInt32(IPAddress.Parse("127.0.0.1").GetAddressBytes(), 0);
                worldConnectionPacket.WorldIPPort = 20001;
            }
        }

        private static void InjectRealWorldConnection(IPacket packet, IWriteConnection client)
        {
            if (packet is World.Packet.Client.WorldAuthenticatePacket worldAuthenticatePacket)
            {
                worldAuthenticatePacket.WorldIPAddress = _realWorldIp;
                worldAuthenticatePacket.WorldIPPort = _realWorldPort;
            }
        }
    }
}
