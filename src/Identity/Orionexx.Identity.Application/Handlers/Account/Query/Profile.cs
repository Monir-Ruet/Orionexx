using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Orionexx.Core.Shared.Abstractions;
using Orionexx.Proto;
using Orionexx.Identity.Application.Interfaces.Repositories;
using Orionexx.Identity.Core.Entities.Account;

namespace Orionexx.Identity.Application.Handlers.Account.Query;

public class ProfileQuery : IRequest<Result<User>>
{
    public required string Email { get; set; }
}

public class ProfileHandler(
    IMapper mapper,
    UserManager<AppUser> userManager) : IRequestHandler<ProfileQuery, Result<User>>
{
    public async Task<Result<User>> Handle(ProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        return user == null ? Result.Failure<User>() : Result.Success(mapper.Map<User>(user));
    }
}