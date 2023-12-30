using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyCampus.Models.Auth;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;


namespace MyCampus.Controllers.Authentication
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IJwtAuthenticationManager jwtAuthenticationManager;
        private readonly IConfiguration _configuration;
        private int id = 0;

        public LoginController(IJwtAuthenticationManager jwtAuthenticationManager, IConfiguration configuration)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this._configuration = configuration;
        }


        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "Prashant", "Saini" };
        //}

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserCred usercred)
        {
            int status = LoginCheck(usercred.username, usercred.password, usercred.type);
            if (status == 0)
            {
                return Unauthorized();
            }
            string token = jwtAuthenticationManager.Authenticate(usercred.username, id, usercred.type);
            // return Ok(token);
            return Json(token);

        }


        private int LoginCheck(string username, string password, char type)
        {
            int status = 0;
            SqlDataReader rdr;
            string sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("LoginCheck", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@LoginUsername", username);
                    myCommand.Parameters.AddWithValue("@LoginPassword", password);
                    myCommand.Parameters.AddWithValue("@UserType", type);
                    rdr = myCommand.ExecuteReader();
                    while (rdr.Read())
                    {
                        status = Convert.ToInt32(rdr["status"]);
                        id = Convert.ToInt32(rdr["id"]);
                    }
                    myCon.Close();
                }
            }
            return status;
        }
    }
}
