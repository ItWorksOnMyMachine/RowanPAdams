//http://www.codeproject.com/Articles/806029/Getting-started-with-AngularJS-and-ASP-NET-MVC-Par
//http://genericunitofworkandrepositories.codeplex.com/

using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RowanAdams.Models;

namespace RowanAdams.Controllers
{
	[System.Web.Mvc.Authorize]
	public class MvcAccountController : System.Web.Mvc.Controller
	{
		private ApplicationUserManager _userManager;
		private ApplicationUserManager.ApplicationSignInManager _signInManager;

		public MvcAccountController() { }

		public MvcAccountController(ApplicationUserManager userManager, ApplicationUserManager.ApplicationSignInManager signInManager)
		{
			UserManager = userManager;
			SignInManager = signInManager;
		}

		public ApplicationUserManager UserManager
		{
			get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
			private set { _userManager = value; }
		}

		public ApplicationUserManager.ApplicationSignInManager SignInManager
		{
			get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationUserManager.ApplicationSignInManager>(); }
			private set { _signInManager = value; }
		}

		[System.Web.Mvc.AllowAnonymous]
		public System.Web.Mvc.ActionResult Login()
		{
			return PartialView();
		}

		[System.Web.Mvc.AllowAnonymous]
		public System.Web.Mvc.ActionResult Register()
		{
			return PartialView();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<bool> Login(LoginViewModel model)
		{
			var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
			switch (result)
			{
				case SignInStatus.Success:
					return true;
				default:
					ModelState.AddModelError("", "Invalid login attempt.");
					return false;
			}
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<bool> Register(RegisterViewModel model)
		{
			var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
			var result = await UserManager.CreateAsync(user, model.Password);
			if (!result.Succeeded) return false;
			await SignInManager.SignInAsync(user, false, false);
			return true;
		}

	}
}
