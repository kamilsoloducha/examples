using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Blueprints.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Service1.Controllers
{
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ILogger<TestController> logger;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TestController(IPublishEndpoint publishEndpoint,
         ILogger<TestController> logger,
         IHttpContextAccessor httpContextAccessor)
        {
            this.publishEndpoint = publishEndpoint;
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Send message to Service 2
        /// </summary>
        [HttpGet("service2/{value}")]
        public async Task<ActionResult> SendToService2([FromRoute] string value, CancellationToken cancellationToken)
        {
            var currentActivity = Activity.Current;
            await publishEndpoint.Publish(new Service2Event { Value = value }, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Send message to Service 2 and Service 3
        /// </summary>
        [HttpGet("service2and3/{value}")]
        public async Task<ActionResult> SendToService2AndService3([FromRoute] string value, CancellationToken cancellationToken)
        {
            await publishEndpoint.Publish(new Service2and3Event { Value = value }, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Send message to Service 3 throught Service 2
        /// </summary>
        [HttpGet("service3th2/{value}")]
        public async Task<ActionResult> SendToService3([FromRoute] string value, CancellationToken cancellationToken)
        {
            await publishEndpoint.Publish(new Service3th2Event { Value = value }, cancellationToken);
            return Ok();
        }
    }
}