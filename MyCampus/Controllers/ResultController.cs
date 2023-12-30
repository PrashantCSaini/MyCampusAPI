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
    public class ResultController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        public ResultController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("select ResultId, ExamType, SubjectName, MarksScored, TotalMarks, StudentId from Result where StudentId=@StudentId order by ExamType desc", myCon))
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


        [HttpPost]
        public IActionResult Post([FromBody] Result r)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("insert into Result (ExamType, SubjectName, MarksScored, TotalMarks, StudentId) values (@ExamType, @SubjectName, @MarksScored, @TotalMarks, @StudentId)", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@ExamType", r.ExamType);
                    myCommand.Parameters.AddWithValue("@SubjectName", r.SubjectName);
                    myCommand.Parameters.AddWithValue("@MarksScored", r.MarksScored);
                    myCommand.Parameters.AddWithValue("@TotalMarks", r.TotalMarks);
                    myCommand.Parameters.AddWithValue("@StudentId", r.StudentId);
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Error while adding marks");
                }
            }
            return Ok("Marks Added Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("delete from Result where ResultId=@ResultId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@ResultId", id);
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Error while deleting marks");
                }
            }
            return Ok("Marks Deleted Successfully");
        }
    }
}
