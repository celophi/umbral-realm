using FluentValidation;
using UmbralRealm.Domain.ValueObjects;
using UmbralRealm.Login.Packet.Client;

namespace UmbralRealm.Login.Service.Validators
{
    public class LoginAuthenticatePacketValidator : AbstractValidator<GenericRequest<LoginAuthenticatePacket>>
    {
        public LoginAuthenticatePacketValidator()
        {
            this.RuleFor(request => request.Packet.Account.Text)
                .Must(text => Username.IsValid(text));

            this.RuleFor(request => request.Packet.Password.Text)
                .Must(text => MD5Hash.IsValid(text));
        }
    }
}
