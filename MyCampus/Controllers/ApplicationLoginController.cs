using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyCampus.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MyCampus.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ApplicationLoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        public ApplicationLoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
        }


        // GET /api/course
        [HttpGet]
        public IActionResult Get()
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("get_ApplicationLogin", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }



        [HttpPost]
        public IActionResult Post([FromBody] ApplicationLogin login)
        {
            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand("insert_ApplicationLogin", myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("@LoginUsername", login.LoginUsername);
                        myCommand.Parameters.AddWithValue("@LoginPassword", login.LoginPassword);
                        myCommand.Parameters.AddWithValue("@UserType", login.UserType);
                        myCommand.ExecuteNonQuery();
                    }
                }
                return Ok("Login Added Successfully");
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 2627)
                    return StatusCode(409, "Username already exist.");
                else
                    return StatusCode(500, "Error while adding user login");
            }
        }

    }
}
