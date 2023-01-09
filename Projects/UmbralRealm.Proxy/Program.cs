using System.Net;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UmbralRealm.Core.Network;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Core.Security;
using UmbralRealm.Core.Utilities;
using UmbralRealm.Core.Utilities.Interfaces;
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
            public LoginServer(SocketFactory socketFactory, NetworkCertificate certificate, IConnectionFactory connectionFactory, IDataMediator<IWriteConnection> connectionMediator)
                : base(socketFactory, certificate, connectionFactory, connectionMediator) { }
        }

        /// <summary>
        /// Proxy class because .NET has an issue with starting multiple hosted instances with the same type name.
        /// https://github.com/dotnet/runtime/issues/38751
        /// </summary>
        private class WorldServer : SocketServer
        {
            public WorldServer(SocketFactory socketFactory, NetworkCertificate certificate, IConnectionFactory connectionFactory, IDataMediator<IWriteConnection> connectionMediator)
                : base(socketFactory, certificate, connectionFactory, connectionMediator) { }
        }

        private static uint _realWorldIp;
        private static ushort _realWorldPort;

        private static IPEndPoint _worldEndpoint;
        private static BufferBlockMediator<IWriteConnection> _worldConnectionMediator;

        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var host = new HostBuilder()
                .ConfigureServices(services =>
                {
                    var loginOpcodeMapping = OpcodeMapping.Create(new Login.Packet.PacketOpcode());

                    var packetMediator = new BufferBlockMediator<IPacket>();
                    packetMediator.Subscribe(new PacketLogger(loginOpcodeMapping));

                    // Setup server
                    var loginOptions = configuration.GetSection(EndpointOptions.LocalLoginEndpoint).Get<EndpointOptions>();
                    ArgumentNullException.ThrowIfNull(loginOptions);
                    var endpoint = Program.ParseEndpoint(loginOptions);

                    var certificate = NetworkCertificate.CreatePrivateAsync().Result;

                    var packetFactory = new PacketConverter(loginOpcodeMapping);
                    var connectionFactory = new ConnectionFactory(packetFactory);

                    var connectionMediator = new BufferBlockMediator<IWriteConnection>();

                    var server = new LoginServer(new SocketFactory(endpoint), certificate, connectionFactory, connectionMediator);

                    // Setup proxy

                    var remoteOptions = configuration.GetSection(EndpointOptions.RemoteLoginEndpoint).Get<EndpointOptions>();
                    ArgumentNullException.ThrowIfNull(remoteOptions);
                    var remoteEndpoint = Program.ParseEndpoint(remoteOptions);

                    var proxy = new SocketClient(new SocketFactory(remoteEndpoint), connectionFactory, packetMediator);
                    proxy.PacketReceivedHandler = StartWorldProxy;

                    connectionMediator.Subscribe(proxy);

                    services.AddHostedService(provider => server);

                    // World
                    var worldOptions = configuration.GetSection(EndpointOptions.LocalWorldEndpoint).Get<EndpointOptions>();
                    ArgumentNullException.ThrowIfNull(worldOptions);
                    _worldEndpoint = Program.ParseEndpoint(worldOptions);

                    _worldConnectionMediator = new BufferBlockMediator<IWriteConnection>();

                    var worldServer = Program.CreateSocketServer(_worldEndpoint, _worldConnectionMediator);

                    services.AddHostedService(provider => worldServer);
                })
                .UseConsoleLifetime()
                .Build();

            host.Run();

            Console.WriteLine("Hello, World!");
        }



        /// <summary>
        /// Parse the options to return a useable endpoint.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static IPEndPoint ParseEndpoint(EndpointOptions options)
        {
            var ipAddress = IPAddress.Parse(options.IPAddress);
            return new IPEndPoint(ipAddress, options.Port);
        }

        private static WorldServer CreateSocketServer(IPEndPoint endpoint, IDataMediator<IWriteConnection> connectionMediator)
        {
            var socketFactory = new SocketFactory(endpoint);

            var certificate = NetworkCertificate.CreatePrivateAsync().Result;

            var worldOpcodeMapping = OpcodeMapping.Create(new World.Packet.PacketOpcode());

            var packetFactory = new PacketConverter(worldOpcodeMapping);
            var connectionFactory = new ConnectionFactory(packetFactory);

            return new WorldServer(socketFactory, certificate, connectionFactory, connectionMediator);
        }

        private static void StartWorldProxy(IPacket packet, IWriteConnection client)
        {
            if (packet is WorldConnectionPacket worldConnectionPacket)
            {
                _realWorldIp = worldConnectionPacket.WorldIPAddress;
                _realWorldPort = worldConnectionPacket.WorldIPPort;

                var worldOpcodeMapping = OpcodeMapping.Create(new World.Packet.PacketOpcode());
                var packetFactory = new PacketConverter(worldOpcodeMapping);
                var connectionFactory = new ConnectionFactory(packetFactory);

                var packetMediator = new BufferBlockMediator<IPacket>();
                packetMediator.Subscribe(new PacketLogger(worldOpcodeMapping));

                // proxy
                var remoteEndpoint = new IPEndPoint(_realWorldIp, _realWorldPort);
                var socketConnectionFactory = new SocketFactory(remoteEndpoint);


                var packetQueue = new BufferBlock<IPacket>();


                var proxy = new SocketClient(socketConnectionFactory, connectionFactory, packetMediator);
                proxy.PacketReceivedHandler = InjectRealWorldConnection;


                _worldConnectionMediator.Subscribe(proxy);

                worldConnectionPacket.WorldIPAddress = BitConverter.ToUInt32(_worldEndpoint.Address.GetAddressBytes(), 0);
                worldConnectionPacket.WorldIPPort = (ushort)_worldEndpoint.Port;
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
