using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoredProc.Data;
using StoredProc.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoredProc.Controllers
{
    public class EmployeeController : Controller
    {
        public StoredProcDbContext _context;
        public IConfiguration _config { get; }

        public EmployeeController
            (
            StoredProcDbContext context,
            IConfiguration config
            )
        {
            _context = context;
            _config = config;

        }


        public IActionResult Index()
        {
            return View();
        }

        public IEnumerable<Employee> SearchResult()
        {
            var result = _context.Employee
                .FromSqlRaw<Employee>("spSearchEmployees")
                .ToList();

            return result;
        }

        [HttpGet]
        public IActionResult DynamicSQL()
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "dbo.spSearchEmployees";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                List<Employee> model = new List<Employee>();
                while (sdr.Read())
                {
                    var details = new Employee();
                    details.FirstName = sdr["FirstName"].ToString();
                    details.LastName = sdr["LastName"].ToString();
                    details.Gender = sdr["Gender"].ToString();
                    details.Salary = Convert.ToInt32(sdr["Salary"]);
                    model.Add(details);
                }
                return View(model);
            }
        }

        /// <summary>
        /// SearchPageWithoutDynamicSQL
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DynamicSQL(string firstName, string lastName, string gender, int salary)
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "dbo.spSearchEmployees";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (firstName != null)
                {
                    SqlParameter param_fn = new SqlParameter("@FirstName", firstName);
                    cmd.Parameters.Add(param_fn);
                }
                if (lastName != null)
                {
                    SqlParameter param_ln = new SqlParameter("@LastName", lastName);
                    cmd.Parameters.Add(param_ln);
                }
                if (gender != null)
                {
                    SqlParameter param_g = new SqlParameter("@Gender", gender);
                    cmd.Parameters.Add(param_g);
                }
                if (salary != 0)
                {
                    SqlParameter param_s = new SqlParameter("@Salary", salary);
                    cmd.Parameters.Add(param_s);
                }
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                List<Employee> model = new List<Employee>();
                while (sdr.Read())
                {
                    var details = new Employee();
                    details.FirstName = sdr["FirstName"].ToString();
                    details.LastName = sdr["LastName"].ToString();
                    details.Gender = sdr["Gender"].ToString();
                    details.Salary = Convert.ToInt32(sdr["Salary"]);
                    model.Add(details);
                }
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult SearchWithDynamic()
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                StringBuilder sbCommand = new

                StringBuilder("Select * from Employees where 1 = 1");
                cmd.CommandText = sbCommand.ToString();
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                List<Employee> model = new List<Employee>();
                while (rdr.Read())
                {
                    var details = new Employee();
                    details.FirstName = rdr["FirstName"].ToString();
                    details.LastName = rdr["LastName"].ToString();
                    details.Gender = rdr["Gender"].ToString();
                    details.Salary = Convert.ToInt32(rdr["Salary"]);
                    model.Add(details);
                }
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult SearchWithDynamic(string inputFirstname, string inputLastname, string inputGender, int inputSalary)
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                StringBuilder sbCommand = new

                StringBuilder("Select * from Employees where 1 = 1");
                if (inputFirstname != null)
                {
                    sbCommand.Append(" AND FirstName=@FirstName");
                    SqlParameter param = new
                    SqlParameter("@FirstName", inputFirstname);
                    cmd.Parameters.Add(param);
                }
                if (inputLastname != null)
                {
                    sbCommand.Append(" AND LastName=@LastName");
                    SqlParameter param = new
                    SqlParameter("@LastName", inputLastname);
                    cmd.Parameters.Add(param);
                }
                if (inputGender != null)
                {
                    sbCommand.Append(" AND Gender=@Gender");
                    SqlParameter param = new
                    SqlParameter("@Gender", inputGender);
                    cmd.Parameters.Add(param);
                }
                if (inputSalary != 0)
                {
                    sbCommand.Append(" AND Salary=@Salary");
                    SqlParameter param = new
                    SqlParameter("@Salary", inputSalary);
                    cmd.Parameters.Add(param);
                }
                cmd.CommandText = sbCommand.ToString();
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                List<Employee> model = new List<Employee>();
                while (rdr.Read())
                {
                    var details = new Employee();
                    details.FirstName = rdr["FirstName"].ToString();
                    details.LastName = rdr["LastName"].ToString();
                    details.Gender = rdr["Gender"].ToString();
                    details.Salary = Convert.ToInt32(rdr["Salary"]);
                    model.Add(details);
                }
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult DynamicSQLInStoredProcedure()
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "spSearchEmployeesGoodDynamicSQL";
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                List<Employee> model = new List<Employee>();
                while (rdr.Read())
                {
                    var details = new Employee();
                    details.FirstName = rdr["FirstName"].ToString();
                    details.LastName = rdr["LastName"].ToString();
                    details.Gender = rdr["Gender"].ToString();
                    details.Salary = Convert.ToInt32(rdr["Salary"]);
                    model.Add(details);
                }
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult DynamicSQLInStoredProcedure(string inputFirstname, string inputLastname, string inputGender, int inputSalary)
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "spSearchEmployeesGoodDynamicSQL";
                cmd.CommandType = CommandType.StoredProcedure;

                if (inputFirstname != null)
                {
                    SqlParameter param = new SqlParameter("@FirstName",
                        inputFirstname);
                    cmd.Parameters.Add(param);
                }

                if (inputLastname != null)
                {
                    SqlParameter param = new SqlParameter("@LastName",
                        inputLastname);
                    cmd.Parameters.Add(param);
                }

                if (inputGender != null)
                {
                    SqlParameter param = new SqlParameter("@Gender",
                        inputGender);
                    cmd.Parameters.Add(param);
                }

                if (inputSalary != 0)
                {
                    SqlParameter param = new SqlParameter("@Salary",
                        inputSalary);
                    cmd.Parameters.Add(param);
                }

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                List<Employee> model = new List<Employee>();
                while (rdr.Read())
                {
                    var details = new Employee();
                    details.FirstName = rdr["FirstName"].ToString();
                    details.LastName = rdr["LastName"].ToString();
                    details.Gender = rdr["Gender"].ToString();
                    details.Salary = Convert.ToInt32(rdr["Salary"]);
                    model.Add(details);
                }
                return View(model);


            }
        }






    }
}