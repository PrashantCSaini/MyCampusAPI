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
    public class StudentDashboardController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        public StudentDashboardController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
        }


        [HttpGet("GetAttendancePercent/{sid}")]
        public IActionResult GetAttendancePercent(int sid)
        {
            decimal attendancepercent;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("get_AttendancePercent ", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@StudentId", sid);
                    attendancepercent = Convert.ToDecimal(myCommand.ExecuteScalar());
                    myCon.Close();
                }
            }
            return Ok(attendancepercent);
        }

        [HttpGet("GetPendingAssignmentNumber/{CourseId}/{Semester}/{StudentId}")]
        public IActionResult GetPendingAssignmentNumber(int CourseId, int Semester, int StudentId)
        {
            int pendingassignnment;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("get_PendingAssignmentNumber ", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@CourseId", CourseId);
                    myCommand.Parameters.AddWithValue("@Semester", Semester);
                    myCommand.Parameters.AddWithValue("@StudentId", StudentId);
                    pendingassignnment = (int)myCommand.ExecuteScalar();
                    myCon.Close();
                }
            }
            return Ok(pendingassignnment);
        }

        [HttpGet("GetCourseName/{StudentId}")]
        public IActionResult GetCourseName(int StudentId)
        {
            string coursename;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("select c.CourseName from StudentMaster s, CourseMaster c where s.CourseId = c.CourseId and s.StudentId = @StudentId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@StudentId", StudentId);
                    coursename = (string)myCommand.ExecuteScalar();
                    myCon.Close();
                }
            }
            return Ok(coursename);
        }
    }
}
