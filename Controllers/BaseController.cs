using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManagement.DTOs;

namespace TeamTaskManagement.Controllers;

public class BaseController : ControllerBase
{
    internal int GetStatusCode (int statusCode) 
    {
        return statusCode switch
        {
            200 => StatusCodes.Status200OK,
            400 => StatusCodes.Status400BadRequest,
            401 => StatusCodes.Status401Unauthorized,
            409 => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };
    }
}