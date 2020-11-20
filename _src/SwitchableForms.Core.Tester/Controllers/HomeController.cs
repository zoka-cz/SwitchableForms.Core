using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SwitchableForms.Core.Tester.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchableForms.Core.Tester.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index(string message = null)
		{
			return View((object)message);
		}


		public IActionResult Contact()
		{
			var contact_model = new ContactModel();
			return View(contact_model);
		}


		[HttpPost]
		public IActionResult Contact(ContactModel model)
		{
			if (ModelState.IsValid)
			{
				string msg;
				if (model.Contact.Switcher == 1)
					msg = $"Contacting user on Phone number {model.Contact.GetSelectedModel<ContactModel.ContactTelephone>().PhoneNumber}";
				else
				{
					msg = $"Contacting user on Email {model.Contact.GetSelectedModel<ContactModel.ContactEmail>().Email}";
				}
				return RedirectToAction("Index", new { message = msg });
			}
			else
			{
				ModelState.AddModelError("", "Please fill in all data correctly");
			}

			return View(model);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
