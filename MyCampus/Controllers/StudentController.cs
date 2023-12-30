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
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlDataSource;
        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDataSource = _configuration.GetConnectionString("MyCampusDB");
        }
        // GET /api/student/id/
        [HttpGet("id/{username}")]
        public IActionResult Get(string username)
        {
            int[] data = new int[3];
            int studentId;
            int courseId;
            int semester;
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("SELECT s.StudentId, s.CourseId, s.CurrentSemester  FROM ApplicationLogin l, StudentMaster s  WHERE l.LoginId = s.LoginId and l.LoginUsername = @LoginUsername", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@LoginUsername", username);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        studentId = (int)myReader["StudentId"];
                        courseId = (int)myReader["CourseId"];
                        semester = (int)myReader["CurrentSemester"];
                        data[0] = studentId;
                        data[1] = courseId;
                        data[2] = semester;
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
                using (SqlCommand myCommand = new SqlCommand("get_StudentMaster", myCon))
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
            StudentMaster student = new StudentMaster();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("get_StudentMasterById", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@StudentId", id);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        student.StudentId = (int)myReader["StudentId"];
                        student.AdmissionYear = (int)myReader["AdmissionYear"];
                        student.Branch = (string)myReader["Branch"];
                        student.CurrentSemester = (int)myReader["CurrentSemester"];
                        student.Division = Convert.ToChar(myReader["Division"]);
                        student.RollNo = (int)myReader["RollNo"];
                        student.Name = (string)myReader["Name"];
                        student.DateOfBirth = (DateTime)myReader["DateOfBirth"];
                        student.Gender = Convert.ToChar(myReader["Gender"]);
                        student.ResidentialAddress = (string)myReader["ResidentialAddress"];
                        student.NativeAddress = (string)myReader["NativeAddress"];
                        student.MobileNo1 = (string)myReader["MobileNo1"];
                        student.MobileNo2 = (string)myReader["MobileNo2"];
                        student.Email = (string)myReader["Email"];
                        student.Nationality = (string)myReader["Nationality"];
                        student.MotherTongue = (string)myReader["MotherTongue"];
                        student.Discipline = (string)myReader["Discipline"];
                        student.JoiningDate = (DateTime)myReader["JoiningDate"];
                        student.PhysicallyHandicapped = (string)myReader["PhysicallyHandicapped"];
                        student.MentorName = (string)myReader["MentorName"];
                        student.LoginId = (int)myReader["LoginId"];
                        student.CourseId = (int)myReader["CourseId"];
                        student.CourseName = (string)myReader["CourseName"];
                    }
                    myReader.Close();
                    if (student.StudentId == 0)
                        return NotFound("Student of id " + id + " could not be found.");
                }
            }
            return Ok(student);
        }

        [HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post([FromBody] StudentMaster student)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("insert_StudentMaster", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@AdmissionYear", student.AdmissionYear);
                    myCommand.Parameters.AddWithValue("@Branch", student.Branch);
                    myCommand.Parameters.AddWithValue("@CurrentSemester", student.CurrentSemester);
                    myCommand.Parameters.AddWithValue("@Division", student.Division);
                    myCommand.Parameters.AddWithValue("@RollNo", student.RollNo);
                    myCommand.Parameters.AddWithValue("@Name", student.Name);
                    myCommand.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                    myCommand.Parameters.AddWithValue("@Gender", student.Gender);
                    myCommand.Parameters.AddWithValue("@ResidentialAddress", student.ResidentialAddress);
                    myCommand.Parameters.AddWithValue("@NativeAddress", student.NativeAddress);
                    myCommand.Parameters.AddWithValue("@MobileNo1", student.MobileNo1);
                    myCommand.Parameters.AddWithValue("@MobileNo2", student.MobileNo2);
                    myCommand.Parameters.AddWithValue("@Email", student.Email);
                    myCommand.Parameters.AddWithValue("@Nationality", student.Nationality);
                    myCommand.Parameters.AddWithValue("@MotherTongue", student.MotherTongue);
                    myCommand.Parameters.AddWithValue("@Discipline", student.Discipline);
                    myCommand.Parameters.AddWithValue("@JoiningDate", student.JoiningDate);
                    myCommand.Parameters.AddWithValue("@PhysicallyHandicapped", student.PhysicallyHandicapped);
                    myCommand.Parameters.AddWithValue("@MentorName", student.MentorName);
                    myCommand.Parameters.AddWithValue("@LoginId", student.LoginId);
                    myCommand.Parameters.AddWithValue("@CourseId", student.CourseId);
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Error while adding student");
                }
            }
            return Ok("Student Added Successfully");
        }


        [HttpPut]
        public IActionResult Put([FromBody] StudentMaster student)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand("update_StudentMaster", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@StudentId", student.StudentId);
                    myCommand.Parameters.AddWithValue("@AdmissionYear", student.AdmissionYear);
                    myCommand.Parameters.AddWithValue("@Branch", student.Branch);
                    myCommand.Parameters.AddWithValue("@CurrentSemester", student.CurrentSemester);
                    myCommand.Parameters.AddWithValue("@Division", student.Division);
                    myCommand.Parameters.AddWithValue("@RollNo", student.RollNo);
                    myCommand.Parameters.AddWithValue("@Name", student.Name);
                    myCommand.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                    myCommand.Parameters.AddWithValue("@Gender", student.Gender);
                    myCommand.Parameters.AddWithValue("@ResidentialAddress", student.ResidentialAddress);
                    myCommand.Parameters.AddWithValue("@NativeAddress", student.NativeAddress);
                    myCommand.Parameters.AddWithValue("@MobileNo1", student.MobileNo1);
                    myCommand.Parameters.AddWithValue("@MobileNo2", student.MobileNo2);
                    myCommand.Parameters.AddWithValue("@Email", student.Email);
                    myCommand.Parameters.AddWithValue("@Nationality", student.Nationality);
                    myCommand.Parameters.AddWithValue("@MotherTongue", student.MotherTongue);
                    myCommand.Parameters.AddWithValue("@Discipline", student.Discipline);
                    myCommand.Parameters.AddWithValue("@JoiningDate", student.JoiningDate);
                    myCommand.Parameters.AddWithValue("@PhysicallyHandicapped", student.PhysicallyHandicapped);
                    myCommand.Parameters.AddWithValue("@MentorName", student.MentorName);
                    myCommand.Parameters.AddWithValue("@LoginId", student.LoginId);
                    myCommand.Parameters.AddWithValue("@CourseId", student.CourseId);
                    myCon.Open();
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound($"Student with id {student.StudentId} could not be found.");
                }
            }
            return Ok("Student Updated Successfully");
        }


        // DELET /api/course/102
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int rowAffected;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand("Delete from StudentMaster where StudentId=@StudentId", myCon))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@StudentId", id);
                    myCon.Open();
                    rowAffected = myCommand.ExecuteNonQuery();
                    if (rowAffected == 0)
                        return NotFound("Student of id " + id + " could not be found.");
                }
            }
            return Ok("Student Deleted Successfully");
        }
    }
}
