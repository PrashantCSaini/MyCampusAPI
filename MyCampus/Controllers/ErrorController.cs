using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MyCampus.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class ErrorController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public ErrorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            if (statusCode == 400)
            {
                return BadRequest("400 - You've sent a bad request.");
            }
            else if (statusCode == 401)
            {
                return Unauthorized("401 - Authorization required.");
            }
            else if (statusCode == 403)
            {
                return Forbid("403 - This is a forbidden area.");
            }
            else if (statusCode == 404)
            {
                return NotFound("404 - Sorry, the resource you requested could not be found.");
            }
            else if (statusCode == 405)
            {
                return StatusCode(StatusCodes.Status405MethodNotAllowed ,"Sorry, method not allowed.");
            }
            else if (statusCode == 415)
            {
                return StatusCode(StatusCodes.Status415UnsupportedMediaType, "Unsupported media type.");
            }
            else 
            {
                return new JsonResult($"{statusCode} - An error occured while processing the request");
            }
        }

        [Route("Error")]
        [HttpGet]
        //[AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            string path = exceptionDetails.Path;
            string errorMessage = exceptionDetails.Error.Message;
            string stacktrace = exceptionDetails.Error.StackTrace;
            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            string error = ErrorLogger(ipAddress, path, errorMessage, stacktrace);

            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occured while processing the request{error}");

        }
        string ErrorLogger(string ipAddress, string path, string errorMessage, string stacktrace)
        {
            try
            {
                string sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand("LogError", myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("@IPAddress", ipAddress);
                        myCommand.Parameters.AddWithValue("@Path", path);
                        myCommand.Parameters.AddWithValue("@Message", errorMessage);
                        myCommand.Parameters.AddWithValue("@Stacktrace", stacktrace);
                        myCommand.ExecuteReader();
                        myCon.Close();
                    }
                }
                return "";
            }
            catch
            {
                return ".";
            }

        }
    }
}
