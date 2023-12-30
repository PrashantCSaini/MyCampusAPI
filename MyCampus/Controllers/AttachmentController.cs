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
    public class AttachmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        public AttachmentController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
        }

        [HttpPost]
        public IActionResult Post([FromBody] Attachment a)
        {
            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand("insert into Attachment (UploadDate, AttachmentUrl, MarksScored, StudentId, AssignmentId) values (@UploadDate, @AttachmentUrl, @MarksScored, @StudentId, @AssignmentId)", myCon))
                    {
                        myCommand.CommandType = CommandType.Text;
                        myCommand.Parameters.AddWithValue("@UploadDate", a.UploadDate);
                        myCommand.Parameters.AddWithValue("@AttachmentUrl", a.AttachmentUrl);
                        myCommand.Parameters.AddWithValue("@MarksScored", a.MarksScored);
                        myCommand.Parameters.AddWithValue("@StudentId", a.StudentId);
                        myCommand.Parameters.AddWithValue("@AssignmentId", a.AssignmentId);
                        myCommand.ExecuteNonQuery();
                        return Ok("Assignment Submitted Successfully");
                    }
                }
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 2627)
                    return StatusCode(409, "Assignment already submitted");
                else
                    return StatusCode(500, "Error while submitting assignment");
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Marks m)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand("update Attachment set MarksScored=@MarksScored where AttachmentId=@AttachmentId and StudentId=@StudentId and AssignmentId=@AssignmentId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@AttachmentId", m.AttachmentId);
                    myCommand.Parameters.AddWithValue("@StudentId", m.StudentId);
                    myCommand.Parameters.AddWithValue("@AssignmentId", m.AssignmentId);
                    myCommand.Parameters.AddWithValue("@MarksScored", m.MarksScored);
                    myCon.Open();
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Error updating Assignment");
                }
            }
            return Ok("Assignment Marked Successfully");
        }
    }
}
