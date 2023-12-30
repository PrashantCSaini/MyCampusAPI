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
    public class AttendanceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        public AttendanceController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
        }

        [HttpPost]
        public IActionResult Post([FromBody]Attendances a)
        {
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("insert into Attendance (Date, CourseId, Semester, SubjectId, StudentId, Attendance) values (@Date, @CourseId, @Semester, @SubjectId, @StudentId, @Attendance)", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@Date", a.Date);
                    myCommand.Parameters.AddWithValue("@CourseId", a.CourseId);
                    myCommand.Parameters.AddWithValue("@Semester", a.Semester);
                    myCommand.Parameters.AddWithValue("@SubjectId", a.SubjectId);
                    myCommand.Parameters.AddWithValue("@StudentId", a.StudentId);
                    myCommand.Parameters.AddWithValue("@Attendance", a.Attendance);
                    myCommand.ExecuteNonQuery();
                    myCon.Close();
                }
            }
            return Ok("Added Successfully");
        }

        [HttpGet("GetStudentByCourse/{id}/{sem}")]
        public IActionResult GetStudentByCourse(int id, int sem)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("select StudentId, Name from StudentMaster where CourseId=@CourseId and CurrentSemester=@CurrentSemester", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@CourseId", id);
                    myCommand.Parameters.AddWithValue("@CurrentSemester", sem);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }

        [HttpGet("GetTotalLecture/{sid}")]
        public IActionResult GetTotalLecture(int sid)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("select s.SubjectId, max(s.SubjectName) as SubjectName, count(a.AttendanceId) as [TotalLecture] " 
                                                                +"from Attendance a, Subject as s "
                                                                +"where a.SubjectId = s.SubjectId and  a.StudentId = @StudentId "
                                                                +"group by s.SubjectId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@StudentId", sid);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }

        [HttpGet("GetPresentAttendence/{id}")]
        public IActionResult GetPresentAttendence(int id)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("select s.SubjectId, max(s.SubjectName) as SubjectName, count(a.AttendanceId) as [TotalLecture] " +
                                                                "from Attendance a, Subject as s "+
                                                                "where a.SubjectId = s.SubjectId and  a.StudentId = @StudentId and a.Attendance in ('P') "+
                                                                "group by s.SubjectId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@StudentId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }

        [HttpGet("GetAbsentAttendence/{sid}")]
        public IActionResult GetAbsentAttendence(int sid)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("select s.SubjectId, max(s.SubjectName) as SubjectName, count(a.AttendanceId) as [TotalLecture] " +
                                                                "from Attendance a, Subject as s " +
                                                                "where a.SubjectId = s.SubjectId and  a.StudentId = @StudentId and a.Attendance in ('A') " +
                                                                "group by s.SubjectId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@StudentId", sid);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }

    }
}
