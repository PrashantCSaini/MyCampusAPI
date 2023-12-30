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
    public class NotificationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        public NotificationController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
        }

        [HttpGet("{id}/{sem}")]
        public IActionResult Get(int id, int sem)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("select n.NotificationId, n.Date, n.Description, n.CourseId, n.Semester, n.FacultyId, f.FacultyName from Notification n, FacultyMaster f where n.FacultyId = f.FacultyId and n.CourseId in (0, @CourseId) and  n.Semester in (0, @Semester) order by n.Date desc", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@CourseId", id);
                    myCommand.Parameters.AddWithValue("@Semester", sem);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }

        [HttpGet("GetNotificationForParent")]
        public IActionResult GetNotificationForParent()
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("select n.NotificationId, n.Date, n.Description, n.CourseId, n.Semester, n.FacultyId, f.FacultyName from Notification n, FacultyMaster f where n.FacultyId = f.FacultyId and n.CourseId in (-1) and  n.Semester in (-1) order by n.Date desc", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(table);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Notification a)
        {
            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand("insert into Notification (Date, Description, CourseId, Semester, FacultyId) values(@Date, @Description, @CourseId, @Semester, @FacultyId)", myCon))
                    {
                        myCommand.CommandType = CommandType.Text;
                        myCommand.Parameters.AddWithValue("@Date", a.Date);
                        myCommand.Parameters.AddWithValue("@Description", a.Description);
                        myCommand.Parameters.AddWithValue("@CourseId", a.CourseId);
                        myCommand.Parameters.AddWithValue("@Semester", a.Semester);
                        myCommand.Parameters.AddWithValue("@FacultyId", a.FacultyId);
                        myCommand.ExecuteNonQuery();
                        return Ok("Notification send successfully");
                    }
                }
            }
            catch
            {
                return StatusCode(500, "Error while sending Notification");
            }
        }
    }
}
