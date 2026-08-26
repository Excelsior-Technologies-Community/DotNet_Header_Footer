using DotNet_Header_Footer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DotNet_Header_Footer.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<string> _passwordHasher;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;

            _passwordHasher =
                new PasswordHasher<string>();
        }


        // =========================================================
        // REGISTER - GET
        // =========================================================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        // =========================================================
        // REGISTER - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            // =====================================================
            // NORMALIZE DATA
            // =====================================================

            model.FullName =
                model.FullName.Trim();

            model.Email =
                model.Email.Trim().ToLowerInvariant();

            model.Phone =
                model.Phone.Trim();


            string passwordHash =
                _passwordHasher.HashPassword(
                    model.Email,
                    model.Password);


            string? connectionString =
                _configuration.GetConnectionString(
                    "DBConnection");


            if (string.IsNullOrWhiteSpace(
                connectionString))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Database connection is not available.");

                return View(model);
            }


            using (SqlConnection con =
                   new SqlConnection(connectionString))
            {
                using (SqlCommand cmd =
                       new SqlCommand(
                           "SP_RegisterUser",
                           con))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;


                    cmd.Parameters.Add(
                        "@FullName",
                        SqlDbType.NVarChar,
                        100).Value =
                        model.FullName;


                    cmd.Parameters.Add(
                        "@Email",
                        SqlDbType.NVarChar,
                        150).Value =
                        model.Email;


                    cmd.Parameters.Add(
                        "@PasswordHash",
                        SqlDbType.NVarChar,
                        500).Value =
                        passwordHash;


                    cmd.Parameters.Add(
                        "@Phone",
                        SqlDbType.NVarChar,
                        20).Value =
                        model.Phone;


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
                                reader["Message"]
                                ?.ToString()
                                ?? string.Empty;


                            if (result == 1)
                            {
                                TempData["SuccessMessage"] =
                                    message;

                                return RedirectToAction(
                                    nameof(Login));
                            }


                            ModelState.AddModelError(
                                string.Empty,
                                message);
                        }
                    }
                }
            }


            return View(model);
        }


        // =========================================================
        // LOGIN - GET
        // =========================================================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        // =========================================================
        // LOGIN - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            model.Email =
                model.Email.Trim()
                    .ToLowerInvariant();


            string? connectionString =
                _configuration.GetConnectionString(
                    "DBConnection");


            if (string.IsNullOrWhiteSpace(
                connectionString))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Database connection is not available.");

                return View(model);
            }


            using (SqlConnection con =
                   new SqlConnection(connectionString))
            {
                using (SqlCommand cmd =
                       new SqlCommand(
                           "SP_LoginUser",
                           con))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;


                    cmd.Parameters.Add(
                        "@Email",
                        SqlDbType.NVarChar,
                        150).Value =
                        model.Email;


                    con.Open();


                    using (SqlDataReader reader =
                           cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            ModelState.AddModelError(
                                string.Empty,
                                "Invalid email or password.");

                            return View(model);
                        }


                        int userId =
                            Convert.ToInt32(
                                reader["UserId"]);


                        string fullName =
                            reader["FullName"]
                            ?.ToString()
                            ?? string.Empty;


                        string email =
                            reader["Email"]
                            ?.ToString()
                            ?? string.Empty;


                        string passwordHash =
                            reader["PasswordHash"]
                            ?.ToString()
                            ?? string.Empty;


                        PasswordVerificationResult
                            passwordResult =
                                _passwordHasher.VerifyHashedPassword(
                                    model.Email,
                                    passwordHash,
                                    model.Password);


                        if (passwordResult ==
                            PasswordVerificationResult.Failed)
                        {
                            ModelState.AddModelError(
                                string.Empty,
                                "Invalid email or password.");

                            return View(model);
                        }


                        // =================================================
                        // SESSION
                        // =================================================

                        HttpContext.Session.SetInt32(
                            "UserId",
                            userId);

                        HttpContext.Session.SetString(
                            "UserName",
                            fullName);

                        HttpContext.Session.SetString(
                            "UserEmail",
                            email);


                        return RedirectToAction(
                            "Index",
                            "Home");
                    }
                }
            }
        }


        // =========================================================
        // LOGOUT
        // =========================================================

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Index",
                "Home");
        }
    }
}