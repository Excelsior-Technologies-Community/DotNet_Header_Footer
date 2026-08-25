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
                _configuration.GetConnectionString("DBConnection");

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


        // ============================================
        // CATEGORY
        // ============================================

        public IActionResult Category(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return RedirectToAction(nameof(Index));
            }

            string? connectionString =
                _configuration.GetConnectionString("DBConnection");

            Category? category = null;

            using (SqlConnection con =
                   new SqlConnection(connectionString))
            {
                using (SqlCommand cmd =
                       new SqlCommand("SP_GetCategoryBySlug", con))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@Slug",
                        slug);

                    con.Open();

                    using (SqlDataReader reader =
                           cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            category = new Category
                            {
                                CategoryId =
                                    Convert.ToInt32(
                                        reader["CategoryId"]),

                                CategoryName =
                                    reader["CategoryName"]
                                    ?.ToString(),

                                Slug =
                                    reader["Slug"]
                                    ?.ToString()
                            };
                        }
                    }
                }
            }

            if (category == null)
            {
                return NotFound();
            }

            ViewBag.SiteSettings = GetSiteSettings();

            return View(category);
        }


        // ============================================
        // NEWSLETTER
        // ============================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubscribeNewsletter(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["NewsletterError"] =
                    "Please enter your email address.";

                return RedirectToAction(nameof(Index));
            }

            string? connectionString =
                _configuration.GetConnectionString("DBConnection");

            using (SqlConnection con =
                   new SqlConnection(connectionString))
            {
                using (SqlCommand cmd =
                       new SqlCommand(
                           "SP_SubscribeNewsletter",
                           con))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@Email",
                        email.Trim());

                    con.Open();

                    using (SqlDataReader reader =
                           cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int result =
                                Convert.ToInt32(
                                    reader["Result"]);

                            string message =
                                reader["Message"]?.ToString()
                                ?? "";

                            if (result == 1)
                            {
                                TempData["NewsletterSuccess"] =
                                    message;
                            }
                            else
                            {
                                TempData["NewsletterError"] =
                                    message;
                            }
                        }
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }


        // ============================================
        // SITE SETTINGS HELPER
        // ============================================

        private SiteSettings GetSiteSettings()
        {
            SiteSettings settings = new SiteSettings();

            string? connectionString =
                _configuration.GetConnectionString("DBConnection");

            if (string.IsNullOrEmpty(connectionString))
                return settings;

            using (SqlConnection con =
                   new SqlConnection(connectionString))
            {
                using (SqlCommand cmd =
                       new SqlCommand(
                           "SP_GetSiteSettings",
                           con))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    con.Open();

                    using (SqlDataReader reader =
                           cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            settings.SettingId =
                                Convert.ToInt32(
                                    reader["SettingId"]);

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

            return settings;
        }
    }
}