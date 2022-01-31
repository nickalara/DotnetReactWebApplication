//API for the Departments table in the db


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Get method, display all db entries
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select DepartmentID, DepartmentName, IsActive,
                            convert(varchar(10),ByCreated,120) as ByCreated,
                            convert(varchar(10),ByUpdated,120) as ByUpdated,
                            convert(varchar(10),DateCreated,120) as DateCreated,
                            convert(varchar(10),DateUpdated,120) as DateUpdated
                            from dbo.Departments
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

        //Post method, aka add to the db table
        [HttpPost]
        public JsonResult Post(Departments dep)
        {
            string query = @"
                            insert into dbo.Departments
                            (DepartmentID,DepartmentName,IsActive,ByCreated,ByUpdated,DateCreated,DateUpdated)
                            values (@DepartmentID,@DepartmentName,@IsActive,@ByCreated,@ByUpdated,@DateCreated,@DateUpdated)
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentID", dep.DepartmentID);
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myCommand.Parameters.AddWithValue("@IsActive", dep.IsActive);
                    myCommand.Parameters.AddWithValue("@ByCreated", dep.ByCreated);
                    myCommand.Parameters.AddWithValue("@ByUpdated", dep.ByUpdated);
                    myCommand.Parameters.AddWithValue("@DateCreated", dep.DateCreated);
                    myCommand.Parameters.AddWithValue("@DateUpdated", dep.DateUpdated);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added successfully.");
        }

        //Put method aka modify a db table entry
        [HttpPut]
        public JsonResult Put(Departments dep)
        {
            string query = @"
                            update dbo.Departments
                             set DepartmentName=@DepartmentName,
                             IsActive=@IsActive,
                             ByUpdated=@ByUpdated,
                             DateUpdated=@DateUpdated
                            where DepartmentId=@DepartmentId
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentID", dep.DepartmentID);
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myCommand.Parameters.AddWithValue("@IsActive", dep.IsActive);
                    myCommand.Parameters.AddWithValue("@ByUpdated", dep.ByUpdated);
                    myCommand.Parameters.AddWithValue("@DateUpdated", dep.DateUpdated);
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
                            delete from dbo.Departments
                             where DepartmentID=@DepartmentID
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentID", id);
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
