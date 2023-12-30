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
    public class ParentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        public ParentController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
        }

        [HttpGet("id/{username}")]
        public IActionResult Get(string username)
        {
            int[] data = new int[2];
            int studentId;
            int parentId;
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("SELECT s.ParentId , s.StudentId  FROM ApplicationLogin l, ParentMaster s  WHERE l.LoginId = s.LoginId and l.LoginUsername = @LoginUsername", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@LoginUsername", username);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        parentId = (int)myReader["ParentId"];
                        studentId = (int)myReader["StudentId"];
                        data[0] = parentId;
                        data[1] = studentId;
                    }
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(data);
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
                using (SqlCommand myCommand = new SqlCommand("get_ParentMaster", myCon))
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
            ParentMaster parent = new ParentMaster();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("get_ParentMasterById", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@ParentId", id);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        parent.ParentId = (int)myReader["ParentId"];
                        parent.FatherName = (string)myReader["FatherName"];
                        parent.FatherEducation = (string)myReader["FatherEducation"];
                        parent.FatherOccupation = (string)myReader["FatherOccupation"];
                        parent.FatherOfficeAddress = Convert.ToString(myReader["FatherOfficeAddress"]);
                        parent.FatherMobileNo = (string)myReader["FatherMobileNo"];
                        parent.FatherEmail = (string)myReader["FatherEmail"];
                        parent.MotherName = (string)myReader["MotherName"];
                        parent.MotherEducation = Convert.ToString(myReader["MotherEducation"]);
                        parent.MotherOccupation = (string)myReader["MotherOccupation"];
                        parent.MotherOfficeAddress = (string)myReader["MotherOfficeAddress"];
                        parent.MotherMobileNo = (string)myReader["MotherMobileNo"];
                        parent.MotherEmail = (string)myReader["MotherEmail"];
                        parent.StudentId = (int)myReader["StudentId"];
                        parent.LoginId = (int)myReader["LoginId"];
                    }
                    myReader.Close();
                    if (parent.ParentId == 0)
                        return NotFound("Parent of id " + id + " could not be found.");
                }
            }
            return Ok(parent);
        }

        [HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post([FromBody] ParentMaster parent)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("insert_ParentMaster", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@FatherName", parent.FatherName);
                    myCommand.Parameters.AddWithValue("@FatherEducation", parent.FatherEducation);
                    myCommand.Parameters.AddWithValue("@FatherOccupation", parent.FatherOccupation);
                    myCommand.Parameters.AddWithValue("@FatherOfficeAddress", parent.FatherOfficeAddress);
                    myCommand.Parameters.AddWithValue("@FatherMobileNo", parent.FatherMobileNo);
                    myCommand.Parameters.AddWithValue("@FatherEmail", parent.FatherEmail);
                    myCommand.Parameters.AddWithValue("@MotherName", parent.MotherName);
                    myCommand.Parameters.AddWithValue("@MotherEducation", parent.MotherEducation);
                    myCommand.Parameters.AddWithValue("@MotherOccupation", parent.MotherOccupation);
                    myCommand.Parameters.AddWithValue("@MotherOfficeAddress", parent.MotherOfficeAddress);
                    myCommand.Parameters.AddWithValue("@MotherMobileNo", parent.MotherMobileNo);
                    myCommand.Parameters.AddWithValue("@MotherEmail", parent.MotherEmail);
                    myCommand.Parameters.AddWithValue("@StudentId", parent.StudentId);
                    myCommand.Parameters.AddWithValue("@LoginId", parent.LoginId);
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Error while adding parent");
                }
            }
            return Ok("Parent Added Successfully");
        }


        [HttpPut]
        public IActionResult Put([FromBody] ParentMaster parent)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand("update_ParentMaster", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@ParentId", parent.ParentId);
                    myCommand.Parameters.AddWithValue("@FatherName", parent.FatherName);
                    myCommand.Parameters.AddWithValue("@FatherEducation", parent.FatherEducation);
                    myCommand.Parameters.AddWithValue("@FatherOccupation", parent.FatherOccupation);
                    myCommand.Parameters.AddWithValue("@FatherOfficeAddress", parent.FatherOfficeAddress);
                    myCommand.Parameters.AddWithValue("@FatherMobileNo", parent.FatherMobileNo);
                    myCommand.Parameters.AddWithValue("@FatherEmail", parent.FatherEmail);
                    myCommand.Parameters.AddWithValue("@MotherName", parent.MotherName);
                    myCommand.Parameters.AddWithValue("@MotherEducation", parent.MotherEducation);
                    myCommand.Parameters.AddWithValue("@MotherOccupation", parent.MotherOccupation);
                    myCommand.Parameters.AddWithValue("@MotherOfficeAddress", parent.MotherOfficeAddress);
                    myCommand.Parameters.AddWithValue("@MotherMobileNo", parent.MotherMobileNo);
                    myCommand.Parameters.AddWithValue("@MotherEmail", parent.MotherEmail);
                    myCommand.Parameters.AddWithValue("@StudentId", parent.StudentId);
                    myCommand.Parameters.AddWithValue("@LoginId", parent.LoginId);
                    myCon.Open();
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound($"Parent with id {parent.ParentId} could not be found.");
                }
            }
            return Ok("Parent Updated Successfully");
        }


        // DELET /api/course/102
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand("Delete from ParentMaster where ParentId=@ParentId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@ParentId", id);
                    myCon.Open();
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Parent of id " + id + " could not be found.");
                }
            }
            return Ok("Parent Deleted Successfully");
        }
    }
}
