using System.Net;
using EduPortal.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace EduPortal.API.Controllers;

public class CustomBaseController : ControllerBase
{
    public IActionResult CreateResult<T>(Response<T> response)
    {
        if (response.StatusCode == HttpStatusCode.NoContent)
            return new ObjectResult(null) { StatusCode = (int)response.StatusCode };

        return new ObjectResult(response)
        {
            StatusCode = (int)response.StatusCode
        };
    }
}