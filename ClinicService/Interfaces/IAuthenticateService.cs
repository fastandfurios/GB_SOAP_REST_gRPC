using ClinicService.Models;
using ClinicService.Models.Requests.Auth;
using ClinicService.Models.Responses;

namespace ClinicService.Interfaces
{
    public interface IAuthenticateService
    {
        AuthenticationResponse Login(AuthenticationRequest authenticationRequest);

        public SessionInfo GetSessionInfo(string sessionToken);
    }
}
