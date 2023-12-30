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
    public class FacultyWiseSubjectController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        public FacultyWiseSubjectController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
        }

        [HttpGet]
        public IActionResult Get()
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("select fs.FacultyId, fs.SubjectId, f.FacultyName, s.SubjectName, c.CourseName from FacultyWiseSubject fs, FacultyMaster f, Subject s, CourseMaster c where fs.FacultyId = f.FacultyId and fs.SubjectId = s.SubjectId and c.CourseId = s.CourseId", myCon))
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
        //[ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post([FromBody] FacultyWiseSubject f)
        {
            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand("insert into FacultyWiseSubject values (@FacultyId, @SubjectId)", myCon))
                    {
                        myCommand.CommandType = CommandType.Text;
                        myCommand.Parameters.AddWithValue("@FacultyId", f.FacultyId);
                        myCommand.Parameters.AddWithValue("@SubjectId", f.SubjectId);
                        myCommand.ExecuteNonQuery();
                    }
                }
                return Ok("Subject Assigned to faculty");
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 2627)
                    return StatusCode(409, "Subject is already assign to faculty.");
                else
                    return StatusCode(500, "Error while adding. Please try Again.");
            }
        }
    }
}
