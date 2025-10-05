using Dapr;
using Microsoft.AspNetCore.Mvc;
using Orionexx.Messaging.Dtos;

namespace Orionexx.Messaging.Controllers
{
    [ApiController]
    public class AccountEventController : ControllerBase
    {
        [Topic("pubsub", "AccountCreated")]
        [HttpPost("AccountCreated")]
        public async Task<IActionResult> HandleAccountCreated([FromBody] AccountCreated accountCreated)
        {
            // Handle the account created event
            return await Task.FromResult(Ok());
        }
    }
}
