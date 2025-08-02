using LoginsignupPge.Models;
using System.Linq;
using System.Web.Mvc;

namespace LoginsignupPge.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Default Route
        public ActionResult Index() => RedirectToAction("Login");

        // ------------------------- LOGIN -------------------------
        public ActionResult Login() => View();

        [HttpPost]
        public ActionResult Login(string Username, string Password)
        {
            var user = db.Users.FirstOrDefault(u => u.Username == Username);
            if (user != null && user.Password == Password)
            {
                Session["UserEmail"] = user.Email; // ✅ Set session
                TempData["Message"] = "Login successful!";
                return View("LoginSucces"); // ✅ Make sure spelling matches the view file
            }

            ViewBag.Error = user == null ? "Account not found." : "Incorrect password.";
            return View();
        }

        // ------------------------- SUBMIT COMPLAINT -------------------------
        [HttpPost]
        public ActionResult SubmitComplaint(string FullName, string Email, string Phone, string Subject, string Message)
        {
            // ✅ Optional: Save to database (future step if you create a Complaint model)

            TempData["Success"] = "Your complaint has been submitted successfully.";

            // ✅ Corrected spelling for redirect view
            return View("LoginSucces");
        }

        // ------------------------- SIGNUP -------------------------
        public ActionResult Signup() => View();

        [HttpPost]
        public ActionResult Signup(string Username, string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                bool userExists = db.Users.Any(u => u.Username == Username || u.Email == Email);
                if (userExists)
                {
                    ViewBag.Error = "User already exists.";
                    return View();
                }

                var user = new User
                {
                    Username = Username,
                    Email = Email,
                    Password = Password
                };
                db.Users.Add(user);
                db.SaveChanges();

                Session["UserEmail"] = Email; // ✅ Set session after signup
                TempData["Message"] = "Signup successful!";
                return View("SignUpSucceful"); // ✅ This view includes the complaint form via partial
            }

            ViewBag.Error = "Please correct the form.";
            return View();
        }

        // ------------------------- FORGOT PASSWORD -------------------------
        public ActionResult ForgotPassword() => View();

        [HttpPost]
        public ActionResult ForgotPassword(string Email, string NewPassword)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == Email);
            if (user != null)
            {
                user.Password = NewPassword;
                db.SaveChanges();

                TempData["Message"] = "Password reset successful!";
                return RedirectToAction("Login");
            }

            ViewBag.Error = "Email not found.";
            return View();
        }

        // ------------------------- LOGOUT -------------------------
        [HttpPost]
        public ActionResult Logout()
        {
            Session.Clear(); // ✅ Clear session
            return RedirectToAction("Login");
        }
    }
}
