using System.Net;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UmbralRealm.Core.Network;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Security;
using UmbralRealm.Core.Utilities;

namespace UmbralRealm.Login.Service
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var host = new HostBuilder()
                .ConfigureServices(services =>
                {
                    
                    var loginOpcodeMapping = OpcodeMapping.Create(new Packet.PacketOpcode());

                    // Setup server
                    var loginOptions = configuration.GetSection("LocalLoginEndpoint").Get<EndpointOptions>();
                    ArgumentNullException.ThrowIfNull(loginOptions);
                    var endpoint = Program.ParseEndpoint(loginOptions);
                    var socketFactory = new SocketWrapperFactory(endpoint);


                    var certificate = NetworkCertificate.CreatePrivateAsync().Result;

                    var packetFactory = new PacketConverter(loginOpcodeMapping);
                    var connectionFactory = new ConnectionFactory(packetFactory);

                    var mediator = new BufferBlockMediator<IWriteConnection>();

                    var server = new SocketServer(socketFactory, certificate, connectionFactory, mediator);

                    var handler = new Handler();
                    mediator.Subscribe(handler);

                    services.AddHostedService(provider => server);
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
    }
}
