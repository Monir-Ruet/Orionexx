using AutoMapper;
using MediatR;
using Orionexx.Core.Shared.Abstractions;
using Orionexx.Proto;
using Orionexx.Identity.Application.Infrastructure.Repositories;

namespace Orionexx.Identity.Application.Handlers.Account.Query;

public class ProfileQuery : IRequest<Result<User>>
{
    public required string Email { get; set; }
}

public class ProfileHandler(
    IMapper mapper,
    IAccountRepository accountRepository) : IRequestHandler<ProfileQuery, Result<User>>
{
    public async Task<Result<User>> Handle(ProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await accountRepository.FindByEmailAsync(request.Email);
        return user == null ? Result.Failure<User>() : Result.Success(mapper.Map<User>(user));
    }
}