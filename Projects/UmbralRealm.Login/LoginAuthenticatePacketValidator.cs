using FluentValidation;
using UmbralRealm.Login.Packet.Client;

namespace UmbralRealm.Domain.ValueObjects
{
    public class LoginAuthenticatePacketValidator : AbstractValidator<LoginAuthenticatePacket>
    {
        public LoginAuthenticatePacketValidator()
        {
            this.RuleFor(packet => packet.Account.Text)
                .Must(text => Username.IsValid(text));

            this.RuleFor(packet => packet.Password.Text)
                .Must(text => MD5Hash.IsValid(text));
        }
    }
}
