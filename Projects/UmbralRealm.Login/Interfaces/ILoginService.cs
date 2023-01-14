using UmbralRealm.Login.Packet.Server;

namespace UmbralRealm.Login.Interfaces
{
    public interface ILoginService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<bool> DoesAccountExist(string username);
        bool IsLoggedIn(string username);
        bool IsPasswordValid(string username, string password);
        void Login(string username);
        LoginRejectedPacket BuildLoginRejectedPacket();
    }
}
