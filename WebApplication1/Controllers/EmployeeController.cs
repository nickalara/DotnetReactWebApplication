//API for the Employee table in the db

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration,IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        //Get method, display all db entries
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select EmployeeID,FirstName,LastName,DepartmentID,
                            convert(varchar(10),HireDate,120) as HireDate,
                            Picture,IsActive,
                            convert(varchar(10),ByCreated,120) as ByCreated,
                            convert(varchar(10),ByUpdated,120) as ByUpdated,
                            convert(varchar(10),DateCreated,120) as DateCreated,
                            convert(varchar(10),DateUpdated,120) as DateUpdated
                            from dbo.Employee
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        //Post method, aka add entry to the db table
        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            string query = @"
                           insert into dbo.Employee
                           (EmployeeID,FirstName,LastName,DepartmentID,HireDate,Picture,IsActive,ByCreated,ByUpdated,DateCreated,DateUpdated)
                           values (@EmployeeID,@FirstName,@LastName,@DepartmentID,@HireDate,@Picture,@IsActive,@ByCreated,@ByUpdated,@DateCreated,@DateUpdated)
                           ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeID", emp.EmployeeID);
                    myCommand.Parameters.AddWithValue("@FirstName", emp.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", emp.LastName);
                    myCommand.Parameters.AddWithValue("@DepartmentID", emp.DepartmentID);
                    myCommand.Parameters.AddWithValue("@HireDate", emp.HireDate);
                    myCommand.Parameters.AddWithValue("@Picture", emp.Picture);
                    myCommand.Parameters.AddWithValue("@IsActive", emp.IsActive);
                    myCommand.Parameters.AddWithValue("@ByCreated", emp.ByCreated);
                    myCommand.Parameters.AddWithValue("@ByUpdated", emp.ByUpdated);
                    myCommand.Parameters.AddWithValue("@DateCreated", emp.DateCreated);
                    myCommand.Parameters.AddWithValue("@DateUpdated", emp.DateUpdated);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added successfully.");
        }
        //To add employee photo to Photos folder
        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using(var stream=new FileStream(physicalPath,FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

        //Put method aka modify a db table entry
        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string query = @"
                            update dbo.Employee
                            set FirstName=@FirstName,
                             LastName=@LastName,
                             DepartmentID=@DepartmentID,
                             Picture=@Picture,
                             IsActive=@IsActive,
                             ByUpdated=@ByUpdated,
                             DateUpdated=@DateUpdated
                             where EmployeeID=@EmployeeID
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeID", emp.EmployeeID);
                    myCommand.Parameters.AddWithValue("@FirstName", emp.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", emp.LastName);
                    myCommand.Parameters.AddWithValue("@DepartmentID", emp.DepartmentID);
                    myCommand.Parameters.AddWithValue("@Picture", emp.Picture);
                    myCommand.Parameters.AddWithValue("@IsActive", emp.IsActive);
                    myCommand.Parameters.AddWithValue("@ByUpdated", emp.ByUpdated);
                    myCommand.Parameters.AddWithValue("@DateUpdated", emp.DateUpdated);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Update successful.");
        }

        //Delete method aka delete a db table entry 
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                            delete from dbo.Employee
                            where EmployeeID=@EmployeeID
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeID", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Delete successful.");
        }
    }
}
