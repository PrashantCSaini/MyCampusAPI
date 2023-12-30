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
    public class AssignmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        public AssignmentController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
        }

        [HttpGet("{FacultyId}/{CourseId}/{SubjectId}/{Semester}")]
        public IActionResult Get(int FacultyId, int CourseId, int SubjectId, int Semester)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("Select AssignmentId, AssignmentDate, Name, Description, DueDate, TotalMarks, CourseId, Semester, FacultyId, SubjectId from Assignment where FacultyId=@FacultyId and CourseId=@CourseId and Semester=@Semester and SubjectId=@SubjectId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@FacultyId", FacultyId);
                    myCommand.Parameters.AddWithValue("@CourseId", CourseId);
                    myCommand.Parameters.AddWithValue("@SubjectId", SubjectId);
                    myCommand.Parameters.AddWithValue("@Semester", Semester);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }


        
        [HttpGet("{AssignmentId}")]
        public IActionResult Get(int AssignmentId)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("Select a.AssignmentId, a.AssignmentDate, a.Name, a.Description, a.DueDate, " +
                                                            "a.TotalMarks, a.CourseId, a.Semester, a.FacultyId, a.SubjectId, at.StudentId, " +
                                                            "s.Name as StudentName, s.RollNo, at.AttachmentId, at.MarksScored, datediff(day, a.DueDate, at.UploadDate) as datedif, " +
                                                            "at.UploadDate, at.AttachmentUrl " +
                                                            "from Assignment a , Attachment at, StudentMaster s " +
                                                            "where a.AssignmentId = at.AssignmentId and at.StudentId = s.StudentId and a.AssignmentId = @AssignmentId " +
                                                            "order by at.UploadDate", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@AssignmentId", AssignmentId);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }

        [HttpGet("GetPendingAssignment/{CourseId}/{Semester}/{StudentId}")]
        public IActionResult GetPendingAssignment(int CourseId, int Semester, int StudentId)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("get_PendingAssignment", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@CourseId", CourseId);
                    myCommand.Parameters.AddWithValue("@Semester", Semester);
                    myCommand.Parameters.AddWithValue("@StudentId", StudentId);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }


        [HttpGet("GetSubmittedAssignment/{CourseId}/{Semester}/{StudentId}")]
        public IActionResult GetSubmittedAssignment(int CourseId, int Semester, int StudentId)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("get_SubmittedAssignment", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@CourseId", CourseId);
                    myCommand.Parameters.AddWithValue("@Semester", Semester);
                    myCommand.Parameters.AddWithValue("@StudentId", StudentId);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }


        [HttpPost]
        public IActionResult Post([FromBody] Assignment a)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("insert into Assignment (AssignmentDate, Name, Description, DueDate, TotalMarks, CourseId, Semester, FacultyId, SubjectId) values (@AssignmentDate, @Name, @Description, @DueDate, @TotalMarks, @CourseId, @Semester, @FacultyId, @SubjectId)", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@AssignmentDate", a.AssignmentDate);
                    myCommand.Parameters.AddWithValue("@Name", a.Name);
                    myCommand.Parameters.AddWithValue("@Description", a.Description);
                    myCommand.Parameters.AddWithValue("@DueDate", a.DueDate);
                    myCommand.Parameters.AddWithValue("@TotalMarks", a.TotalMarks);
                    myCommand.Parameters.AddWithValue("@CourseId", a.CourseId);
                    myCommand.Parameters.AddWithValue("@Semester", a.Semester);
                    myCommand.Parameters.AddWithValue("@FacultyId", a.FacultyId);
                    myCommand.Parameters.AddWithValue("@SubjectId", a.SubjectId);
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Error creating new Assignment");
                }
            }
            return Ok("New Assignment created successfully");

        }
    }
}
