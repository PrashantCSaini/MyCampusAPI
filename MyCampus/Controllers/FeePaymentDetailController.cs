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
    public class FeePaymentDetailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        public FeePaymentDetailController(IConfiguration configuration)
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
                using (SqlCommand myCommand = new SqlCommand("select FeeId, Date, Amount, ModeOfPayment, StudentId from FeePaymentDetail where StudentId=@StudentId", myCon))
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
        public IActionResult Post([FromBody] FeePaymentDetail f)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("insert into FeePaymentDetail (Date, Amount, ModeOfPayment, StudentId) values (@Date, @Amount, @ModeOfPayment, @StudentId)", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@Date", f.Date);
                    myCommand.Parameters.AddWithValue("@Amount", f.Amount);
                    myCommand.Parameters.AddWithValue("@ModeOfPayment", f.ModeOfPayment);
                    myCommand.Parameters.AddWithValue("@StudentId", f.StudentId);
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Error while adding fee");
                }
            }
            return Ok("Fee Added Successfully");
        }
    }
}
