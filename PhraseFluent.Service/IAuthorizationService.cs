using PhraseFluent.Service.DTO.Responses;

namespace PhraseFluent.Service;

public interface IAuthorizationService
{
    TokenResponse GetToken();
}