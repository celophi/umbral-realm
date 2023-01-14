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
using UmbralRealm.Login.Data;
using UmbralRealm.Login.Interfaces;

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
                    services.AddScoped<IDbConnectionFactory, DbConnectionFactory>(provider =>
                    {
                        var connectionString = configuration.GetConnectionString("Default");
                        return new DbConnectionFactory(connectionString!);
                    });

                    var loginOpcodeMapping = OpcodeMapping.Create(new Packet.PacketOpcode());

                    // Setup server
                    var loginOptions = configuration.GetSection("LocalLoginEndpoint").Get<EndpointOptions>();
                    ArgumentNullException.ThrowIfNull(loginOptions);
                    var endpoint = Program.ParseEndpoint(loginOptions);
                    var socketFactory = new SocketFactory(endpoint);


                    var certificate = NetworkCertificate.CreatePrivateAsync().Result;

                    var packetFactory = new PacketConverter(loginOpcodeMapping);
                    var connectionFactory = new ConnectionFactory(packetFactory);

                    var mediator = new BufferBlockMediator<IWriteConnection>();
                    var mediator2 = new BufferBlockMediator<ISocketConnection>();

                    var listener = new SocketListener(socketFactory, mediator2);
                    var server = new SocketServer(certificate, connectionFactory, mediator);

                    mediator2.Subscribe(server);


                    services.AddScoped<IAccountRepository, AccountRepository>();
                    services.AddScoped<ILoginService, LoginService>();
                    services.AddScoped<IServerInfoService, ServerInfoService>();
                    services.AddScoped<LoginController>();
                    
                    services.AddHostedService(provider =>
                    {
                        var controller = provider.GetService<LoginController>();

                        var handler = new Handler(controller!);
                        mediator.Subscribe(handler);

                        return listener;
                    });
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
