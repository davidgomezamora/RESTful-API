using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonWebAPI.Helpers;
using CommonWebAPI.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RESTful_API_Demo.Controllers
{
    [ApiController]
    public class APIController : ControllerBase
    {
        /*
         * Se hace override del método ValidationProblem, para que la respuesta no sea un 400 Bad Request sino un 422 Unprocessable Entity y
         * se tenga en cuenta lo configurado en el Startup de la apliacición.
         */
        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        /*
         * Se implementa método que genera el enlace de paginación
         */
        public string CreateResourceUri<T>(T resourceParameters, ResourceUriType resourceUriType, string methodName) where T : ParametersBase
        {
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    resourceParameters.PageNumber--;

                    return Url.Link(methodName, resourceParameters);

                case ResourceUriType.NextPage:
                    resourceParameters.PageNumber++;

                    return Url.Link(methodName, resourceParameters);

                default:
                    return Url.Link(methodName, resourceParameters);
            }
        }
    }
}
