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
    public class PlacementController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        public PlacementController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
        }

        [HttpGet("{courseId}/{semester}")]
        public IActionResult Get(int courseId, int semester)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("Select PlacementId, date, Semester, Title, Description, RegistrationLink, CourseId, FacultyId from Placement where CourseId=@CourseId and Semester=@Semester", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@CourseId", courseId);
                    myCommand.Parameters.AddWithValue("@Semester", semester);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }


        [HttpPost]
        public IActionResult Post([FromBody] Placement p)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("insert into Placement (date, Semester, Title, Description, RegistrationLink, CourseId, FacultyId) values (@date, @Semester, @Title, @Description, @RegistrationLink, @CourseId, @FacultyId)", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@date", p.date);
                    myCommand.Parameters.AddWithValue("@Semester", p.Semester);
                    myCommand.Parameters.AddWithValue("@Title", p.Title);
                    myCommand.Parameters.AddWithValue("@Description", p.Description);
                    myCommand.Parameters.AddWithValue("@RegistrationLink", p.RegistrationLink);
                    myCommand.Parameters.AddWithValue("@CourseId", p.CourseId);
                    myCommand.Parameters.AddWithValue("@FacultyId", p.FacultyId);
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Error while adding placement");
                }
            }
            return Ok("Placement Added Successfully");
        }
    }
}
