using FluentValidation;
using UmbralRealm.Domain.ValueObjects;
using UmbralRealm.Login.Packet.Client;

namespace UmbralRealm.Login.Service.Validators
{
    public class LoginAuthenticatePacketValidator : AbstractValidator<RequestContext<LoginAuthenticatePacket>>
    {
        public LoginAuthenticatePacketValidator()
        {
            this.RuleFor(request => request.Request.Account.Text)
                .Must(text => Username.IsValid(text));

            this.RuleFor(request => request.Request.Password.Text)
                .Must(text => MD5Hash.IsValid(text));
        }
    }
}
