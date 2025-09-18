using AutoMapper;
using Orionexx.Identity.Core.Entities.Account;
using Orionexx.Proto;

namespace Orionexx.Identity.Service.Automappers;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<AppUser, User>();
    }
}