using Microsoft.AspNetCore.Mvc;
using Product.Microservice.Core.Domain.Result;
using Product.Microservice.Core.Enums;

namespace Product.Microservice.Extensions
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Extensions to convert the result to action result 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="controllerBase"></param>
        /// <returns></returns>
        public static ActionResult ConvertToActionResult<T>(this Result<T> result, ControllerBase controllerBase)
        {

            switch (result.StatusCode)
            {
                case StatusCode.Success: return controllerBase.Ok(result);
                case StatusCode.UnAuthorized: return controllerBase.Unauthorized();
                case StatusCode.BadRequest: return BadRequest(controllerBase, result);
                case StatusCode.UnAuthenticated: return controllerBase.Unauthorized();
                case StatusCode.NotFound: return controllerBase.NotFound();
                case StatusCode.AlreadyExist: return controllerBase.Conflict(result);
                case StatusCode.InternalError: return controllerBase.StatusCode(StatusCodes.Status500InternalServerError, result);
                default: return new ObjectResult((StatusCodes.Status500InternalServerError));
            }

        }
        public static ActionResult BadRequest<T>(ControllerBase controllerBase, Result<T> result)
        {
            return controllerBase.BadRequest(result);
        }
    }
}
