using DotNet_Header_Footer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DotNet_Header_Footer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            LoadSiteSettings();

            return View();
        }

        private void LoadSiteSettings()
        {
            SiteSettings settings = new SiteSettings();

            string? connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                ViewBag.SiteSettings = settings;
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd =
                       new SqlCommand("SP_GetSiteSettings", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            settings.SettingId =
                                Convert.ToInt32(reader["SettingId"]);

                            settings.BrandName =
                                reader["BrandName"]?.ToString();

                            settings.Logo =
                                reader["Logo"]?.ToString();

                            settings.Address =
                                reader["Address"]?.ToString();

                            settings.Phone =
                                reader["Phone"]?.ToString();

                            settings.Email =
                                reader["Email"]?.ToString();

                            settings.FacebookUrl =
                                reader["FacebookUrl"]?.ToString();

                            settings.InstagramUrl =
                                reader["InstagramUrl"]?.ToString();

                            settings.TwitterUrl =
                                reader["TwitterUrl"]?.ToString();

                            settings.MapEmbedUrl =
                                reader["MapEmbedUrl"]?.ToString();

                            settings.CopyrightText =
                                reader["CopyrightText"]?.ToString();
                        }
                    }
                }
            }

            ViewBag.SiteSettings = settings;
        }
    }
}