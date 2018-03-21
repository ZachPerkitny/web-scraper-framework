using System;
using RestFul.Attributes;
using RestFul.Enum;
using RestFul.Http;
using RestFul.Result;

namespace ScraperFramework.Controllers
{
    [RestController(BasePath = "controller/")]
    public class CoordinatorController
    {
        private readonly ICoordinator _coordinator;

        public CoordinatorController(ICoordinator coordinator)
        {
            _coordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
        }

        [RestRoute(HttpMethod = HttpMethod.POST, Path = "pause")]
        public IResult PauseCoordinator(HttpContext httpContext)
        {
            _coordinator.Pause();
            return new EmptyResult();
        }
    }
}
