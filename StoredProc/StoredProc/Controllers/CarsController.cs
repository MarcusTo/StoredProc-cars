using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StoredProc.Data;
using StoredProc.Models;

namespace StoredProc.Controllers
{
    public class CarsController : Controller
    {
        public StoredProcDbContext _context;
        public IConfiguration _config { get; }

        public CarsController
            (
            StoredProcDbContext context,
            IConfiguration config
            )
        {
            _context = context;
            _config = config;

        }

        [HttpGet]
        public IActionResult Index()
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "dbo.spSearchCars";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                List<Car> model = new List<Car>();
                while (sdr.Read())
                {
                    var details = new Car();
                    details.horsepower = Convert.ToInt32(sdr["horsepower"]);
                    details.manufacturer = sdr["manufacturer"].ToString();
                    details.model = sdr["model"].ToString();
                    details.year = Convert.ToInt32(sdr["year"]);
                    details.factory_color = sdr["factory_color"].ToString();

                    model.Add(details);
                }
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Index(int motor, string manufacturer, string model, int year, string factory_color)
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "dbo.spSearchCars";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (motor != 0)
                {
                    SqlParameter param_fn = new SqlParameter("motor", motor);
                    cmd.Parameters.Add(param_fn);
                }
                if (manufacturer != null)
                {
                    SqlParameter param_ln = new SqlParameter("manufacturer",manufacturer);
                    cmd.Parameters.Add(param_ln);
                }
                if (model != null)
                {
                    SqlParameter param_g = new SqlParameter("model", model);
                    cmd.Parameters.Add(param_g);
                }
                if (year != 0)
                {
                    SqlParameter param_s = new SqlParameter("year", year);
                    cmd.Parameters.Add(param_s);
                }
                if (factory_color != null)
                {
                    SqlParameter param_s = new SqlParameter("factory_color", factory_color);
                    cmd.Parameters.Add(param_s);
                }
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                List<Car> model_list = new List<Car>();
                while (sdr.Read())
                {
                    var details = new Car();
                    details.horsepower = Convert.ToInt32(sdr["horsepower"]);
                    details.manufacturer = sdr["manufacturer"].ToString();
                    details.model = sdr["model"].ToString();
                    details.year = Convert.ToInt32(sdr["year"]);
                    details.factory_color = sdr["factory_color"].ToString();

                    model_list.Add(details);
                }
                return View(model_list);
            }
        }
    }
}