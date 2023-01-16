using MediatR;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Domain.Enumerations;
using UmbralRealm.Domain.Models;
using UmbralRealm.Domain.ValueObjects;
using UmbralRealm.Login.Data;
using UmbralRealm.Login.Packet.Client;
using UmbralRealm.Login.Packet.Server;
using UmbralRealm.Login.Service.Interfaces;

namespace UmbralRealm.Login.Service.Requests
{
    /// <summary>
    /// Request handler for <see cref="LoginAuthenticatePacket"/>
    /// </summary>
    public class LoginAuthenticateRequestHandler : IRequestHandler<RequestContext<LoginAuthenticatePacket>, RequestResult>
    {
        /// <summary>
        /// Used for accessing account data from persistence.
        /// </summary>
        private readonly IAccountRepository _accountRepository;

        /// <summary>
        /// Used for retrieving metadata about available servers.
        /// </summary>
        private readonly IServerInfoService _serverInfoService;

        /// <summary>
        /// Creates an instance of this handler.
        /// </summary>
        /// <param name="accountRepository"></param>
        /// <param name="serverInfoService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public LoginAuthenticateRequestHandler(IAccountRepository accountRepository, IServerInfoService serverInfoService)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _serverInfoService = serverInfoService ?? throw new ArgumentNullException(nameof(serverInfoService));
        }

        /// <summary>
        /// Handles a request of the <see cref="LoginAuthenticatePacket"/> packet type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<RequestResult> Handle(RequestContext<LoginAuthenticatePacket> context, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(context);

            var result = new RequestResult();
            var request = context.Request;
            var connection = context.Connection;

            var username = new Username(request.Account.Text);
            var accountEntity = await _accountRepository.GetByUsername(username);

            if (accountEntity == null)
            {
                await connection.SendAsync(new LoginRejectedPacket { Reason = LoginFailureResult.InvalidCredentials1 });
                return result;
            }

            var account = new Account(accountEntity);
            var password = new MD5Hash(request.Password.Text);

            if (account.Password != password)
            {
                await connection.SendAsync(new LoginRejectedPacket { Reason = LoginFailureResult.InvalidCredentials1 });
                return result;
            }

            if (account.Standing == AccountStanding.Suspended)
            {
                await connection.SendAsync(new LoginRejectedPacket { Reason = LoginFailureResult.AccountSuspended });
                return result;
            }

            if (account.Standing == AccountStanding.Locked)
            {
                await connection.SendAsync(new LoginRejectedPacket { Reason = LoginFailureResult.AccountLocked });
                return result;
            }

            var response = _serverInfoService.BuildWorldSelectionPacket();
            await connection.SendAsync(response);

            return result;
        }
    }
}
