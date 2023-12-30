using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyCampus.Models;
using System.Data;
using System.Data.SqlClient;

namespace MyCampus.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SubjectController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        public SubjectController(IConfiguration configuration)
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
                using (SqlCommand myCommand = new SqlCommand("get_Subject", myCon))
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

        [HttpGet("GetSubjectByCourseID/{id}")]
        public IActionResult GetSubjectByCourseID(string id)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("select * from Subject where CourseId=@CourseId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@CourseId", id);
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
            Subject sub = new Subject();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("select SubjectId, SubjectName, InternalMarks, ExternalTheoryMarks, ExternalPracticalMarks, s.CourseId, c.CourseName from Subject s, CourseMaster c where s.CourseId = c.CourseId and SubjectId = @id", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        sub.SubjectId = (int)myReader["SubjectId"];
                        sub.SubjectName = (string)myReader["SubjectName"];
                        sub.InternalMarks = (int)myReader["InternalMarks"];
                        sub.ExternalTheoryMarks = (int)myReader["ExternalTheoryMarks"];
                        sub.ExternalPracticalMarks = (int)myReader["ExternalPracticalMarks"];
                        sub.CourseId = (int)myReader["CourseId"];
                        sub.CourseName = (string)myReader["CourseName"];
                    }
                    myReader.Close();
                    if (sub.SubjectId == 0)
                        return NotFound("Subject of id " + id + " could not be found.");
                }
            }
            return Ok(sub);
        }

        [HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post([FromBody] Subject sub)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("insert_Subject", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@SubjectName", sub.SubjectName);
                    myCommand.Parameters.AddWithValue("@InternalMarks", sub.InternalMarks);
                    myCommand.Parameters.AddWithValue("@ExternalTheoryMarks", sub.ExternalTheoryMarks);
                    myCommand.Parameters.AddWithValue("@ExternalPracticalMarks", sub.ExternalPracticalMarks);
                    myCommand.Parameters.AddWithValue("@CourseId", sub.CourseId);
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Error while adding subject");
                }
            }
            return Ok("Subject Added Successfully");
        }


        [HttpPut]
        public IActionResult Put([FromBody] Subject sub)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand("Update Subject set SubjectName=@SubjectName, InternalMarks=@InternalMarks, ExternalTheoryMarks=@ExternalTheoryMarks, ExternalPracticalMarks=@ExternalPracticalMarks, CourseId=@CourseId where SubjectId=@SubjectId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@SubjectId", sub.SubjectId);
                    myCommand.Parameters.AddWithValue("@SubjectName", sub.SubjectName);
                    myCommand.Parameters.AddWithValue("@InternalMarks", sub.InternalMarks);
                    myCommand.Parameters.AddWithValue("@ExternalTheoryMarks", sub.ExternalTheoryMarks);
                    myCommand.Parameters.AddWithValue("@ExternalPracticalMarks", sub.ExternalPracticalMarks);
                    myCommand.Parameters.AddWithValue("@CourseId", sub.CourseId);
                    myCon.Open();
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound($"Subject with id {sub.SubjectId} could not be found.");
                }
            }
            return Ok("Subject Updated Successfully");
        }


        // DELET /api/course/102
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand("Delete from Subject where SubjectId=@SubjectId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@SubjectId", id);
                    myCon.Open();
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Subject of id " + id + " could not be found.");
                }
            }
            return Ok("Subject Deleted Successfully");
        }
    }
}
