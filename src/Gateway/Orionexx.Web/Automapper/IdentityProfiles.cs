using AutoMapper;
using Orionexx.Proto;
using Orionexx.Web.DTOs.Account;
using Orionexx.Web.DTOs.Identity;

namespace Orionexx.Web.Automapper;

public class IdentityProfiles : Profile
{
    public IdentityProfiles()
    {
        CreateMap<LoginRequestDto, LoginRequest>();
        CreateMap<SignupRequestDto, SignupRequest>();
        CreateMap<ResetPasswordRequestDto, ResetPasswordRequest>();
        CreateMap<User, UserResponseDto>();
    }
}