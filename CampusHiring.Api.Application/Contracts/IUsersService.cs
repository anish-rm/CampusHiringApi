using CampusHiring.Api.Application.DTOs.Auth;
using CampusHiring.Api.Common.Results;

namespace CampusHiring.Api.Application.Contracts
{
    public interface IUsersService
    {
        Task<Result<string>> LoginAsync(LoginUserDto userDto);
        Task<Result<RegisteredUserDto>> RegisterUserAsync(RegisterUserDto userDto);
    }
}