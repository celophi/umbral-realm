using UmbralRealm.Login.Packet.Server;

namespace UmbralRealm.Login.Interfaces
{
    public interface ILoginService
    {
        public bool DoesAccountExist(string name);
        public bool IsLoggedIn(string name);
        public bool IsPasswordValid(string name, string password);
        public void Login(string name);
        public LoginRejectedPacket BuildLoginRejectedPacket();
    }
}
