using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyCampus.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace MyCampus.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CourseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        private readonly IEmailSender emailSender;
        public CourseController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
            //this.emailSender = emailSender;
        }


        // GET /api/course
        [HttpGet]
        public IActionResult Get()
        {
            //await emailSender.SendEmailAsync(email, subject, message);
            string email = "sainipras.xyz@gmail.com";
            string subject = "Test mail";
            string message = "Test mail from .net core!!";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("sainipras123@gmail.com", "gsjrufuspwdwznxw")
            };

            client.SendMailAsync(
                new MailMessage(from: "sainipras123@gmail.com",
                                to: email,
                                subject,
                                message
                                ));

            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("get_CourseMaster", myCon))
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
            CourseMaster cm = new CourseMaster();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("Select CourseId, CourseName, CourseFee from CourseMaster where CourseId = @id", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        cm.CourseId = (int)myReader["CourseId"];
                        cm.CourseName = (string)myReader["CourseName"];
                        cm.CourseFee = (int)myReader["CourseFee"];
                    }
                    myReader.Close();
                    if (cm.CourseId == 0)
                        return NotFound("Course of id " + id + " could not be found.");
                }
            }
            return Ok(cm);
        }

        [HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(CourseMaster courseMaster)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("insert_CourseMaster", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@CourseName", courseMaster.CourseName);
                    myCommand.Parameters.AddWithValue("@CourseFee", courseMaster.CourseFee);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            //return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
            ///return Created("sdad", 100);
            return Ok("Added Successfully");
        }


        [HttpPut]
        public IActionResult Put(CourseMaster courseMaster)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand("Update CourseMaster set CourseName=@CourseName , CourseFee=@CourseFee where CourseId=@CourseId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@CourseId", courseMaster.CourseId);
                    myCommand.Parameters.AddWithValue("@CourseName", courseMaster.CourseName);
                    myCommand.Parameters.AddWithValue("@CourseFee", courseMaster.CourseFee);
                    myCon.Open();
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound($"Course with id {courseMaster.CourseId} could not be found.");
                }
            }
            return Ok("Course Updated Successfully");
        }


        // DELET /api/course/102
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand("Delete from CourseMaster where CourseId=@CourseId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@CourseId", id);
                    myCon.Open();
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Course of id " + id + " could not be found.");
                }
            }
            return Ok("Course Deleted Successfully");
        }
    }
}
