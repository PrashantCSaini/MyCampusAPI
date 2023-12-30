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
    public class FacultyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        public FacultyController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
        }

        [HttpGet("id/{username}")]
        public IActionResult Get(string username)
        {
            int studentId;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("SELECT s.FacultyId  FROM ApplicationLogin l, FacultyMaster s  WHERE l.LoginId = s.LoginId and l.LoginUsername = @LoginUsername", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@LoginUsername", username);
                    studentId = (int)myCommand.ExecuteScalar();
                    myCon.Close();
                }
            }
            return Ok(studentId);
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
                using (SqlCommand myCommand = new SqlCommand("get_FacultyMaster", myCon))
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

        // GET: /api/course/100001
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get(int id)
        {
            FacultyMaster faculty = new FacultyMaster();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("get_FacultyMasterById", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@FacultyId", id);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        faculty.FacultyId = (int)myReader["FacultyId"];
                        faculty.FacultyName = (string)myReader["FacultyName"];
                        faculty.Education = (string)myReader["Education"];
                        faculty.DateOfBirth = (DateTime)myReader["DateOfBirth"];
                        faculty.Email = Convert.ToString(myReader["Email"]);
                        faculty.Address = (string)myReader["Address"];
                        faculty.CourseId = (int)myReader["CourseId"];
                        faculty.LoginId = (int)myReader["LoginId"];
                        faculty.MobileNo = Convert.ToString(myReader["MobileNo"]);
                    }
                    myReader.Close();
                    if (faculty.FacultyId == 0)
                        return NotFound("Faculty of id " + id + " could not be found.");
                }
            }
            return Ok(faculty);
        }

        [HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post([FromBody] FacultyMaster faculty)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("insert_FacultyMaster", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@FacultyName", faculty.FacultyName);
                    myCommand.Parameters.AddWithValue("@Education", faculty.Education);
                    myCommand.Parameters.AddWithValue("@DateOfBirth", faculty.DateOfBirth);
                    myCommand.Parameters.AddWithValue("@Email", faculty.Email);
                    myCommand.Parameters.AddWithValue("@Address", faculty.Address);
                    myCommand.Parameters.AddWithValue("@CourseId", faculty.CourseId);
                    myCommand.Parameters.AddWithValue("@LoginId", faculty.LoginId);
                    myCommand.Parameters.AddWithValue("@MobileNo", faculty.MobileNo);
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Error while adding faculty");
                }
            }
            return Ok("Faculty Added Successfully");

        }


        [HttpPut]
        public IActionResult Put([FromBody] FacultyMaster faculty)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand("update_FacultyMaster", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@FacultyId", faculty.FacultyId);
                    myCommand.Parameters.AddWithValue("@FacultyName", faculty.FacultyName);
                    myCommand.Parameters.AddWithValue("@Education", faculty.Education);
                    myCommand.Parameters.AddWithValue("@DateOfBirth", faculty.DateOfBirth);
                    myCommand.Parameters.AddWithValue("@Email", faculty.Email);
                    myCommand.Parameters.AddWithValue("@Address", faculty.Address);
                    myCommand.Parameters.AddWithValue("@CourseId", faculty.CourseId);
                    myCommand.Parameters.AddWithValue("@LoginId", faculty.LoginId);
                    myCommand.Parameters.AddWithValue("@MobileNo", faculty.MobileNo);
                    myCon.Open();
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound($"Faculty with id {faculty.FacultyId} could not be found.");
                }
            }
            return Ok("Faculty Updated Successfully");
        }


        // DELET /api/course/102
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand("Delete from FacultyMaster where FacultyId=@FacultyId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@FacultyId", id);
                    myCon.Open();
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Faculty of id " + id + " could not be found.");
                }
            }
            return Ok("Faculty Deleted Successfully");
        }
        
    }
}
