using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace VTS.API.Controllers.Core;

[ApiController, Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public abstract class BaseController : ControllerBase
{

}
