using AutoMapper;
using Orionexx.Proto;
using Orionexx.Web.Contracts.Identity;

namespace Orionexx.Web.MapperProfiles.Identity;

public class AuthenticationProfiles : Profile
{
    public AuthenticationProfiles()
    {
        CreateMap<LoginContract, LoginRequest>();
        CreateMap<SignupContract, SignupRequest>();
    }
}