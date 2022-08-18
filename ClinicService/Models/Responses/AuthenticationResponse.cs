using ClinicService.Enums;

namespace ClinicService.Models.Responses
{
    public class AuthenticationResponse
    {
        public AuthenticationStatus Status { get; set; }
        public SessionInfo SessionInfo { get; set; }
    }
}
