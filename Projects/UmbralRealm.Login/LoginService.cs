using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbralRealm.Login.Data;
using UmbralRealm.Login.Interfaces;
using UmbralRealm.Login.Packet.Server;

namespace UmbralRealm.Login
{
    public class LoginService : ILoginService
    {
        private readonly IAccountRepository _accountRepository;

        public LoginService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public LoginRejectedPacket BuildLoginRejectedPacket()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DoesAccountExist(string name)
        {
            return true;
        }

        public bool IsLoggedIn(string name)
        {
            // TODO: implement
            return false;
        }

        public bool IsPasswordValid(string name, string password)
        {
            throw new NotImplementedException();
        }

        public void Login(string name)
        {
            throw new NotImplementedException();
        }

    }
}
