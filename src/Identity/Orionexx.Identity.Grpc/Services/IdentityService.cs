using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Orionexx.Proto;

namespace Orionexx.Identity.Grpc.Services;

public class IdentityService : Proto.Identity.IdentityBase
{
    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        return await Task.FromResult(new LoginResponse()
        {
            Message = "Hello Orionexx",
        });
    }

    public override async Task<Empty> SignUp(SignUpRequest request, ServerCallContext context)
    {
        return await Task.FromResult(new Empty());
    }
}