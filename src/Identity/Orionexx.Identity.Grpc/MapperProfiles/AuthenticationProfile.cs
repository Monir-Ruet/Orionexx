using AutoMapper;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Orionexx.Identity.Grpc.MapperProfiles;

public class AuthenticationProfile : Profile
{
    public AuthenticationProfile()
    {
        CreateMap<AccessTokenResponse, Orionexx.Proto.AccessTokenResponse>();
    }
}