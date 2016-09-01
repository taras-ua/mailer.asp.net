using System.IO;
using System.Web.Mvc;

namespace Mailer.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendConfirmation(string destination)
        {
            var model = new Models.ConfirmationEmailViewModel
            {
                ConfirmationLink = Request.Url.Scheme + "://" + @Request.Url.Authority
                    + Url.Action("FakeConfirmation", new { ConfirmationToken = Path.GetRandomFileName() + Path.GetRandomFileName() + Path.GetRandomFileName() + Path.GetRandomFileName() })
            };
            model.WebVersion = Request.Url.Scheme + "://" + @Request.Url.Authority + Url.Action(EmailController.CONFIRMATION_EMAIL, "Email", model);
            EmailController.Send(HttpContext.Request.RequestContext, destination, "Confirmation Email Test", EmailController.CONFIRMATION_EMAIL, model);
            return RedirectToAction("Ok", new { destination = destination, email = model.WebVersion });
        }

        [HttpPost]
        public ActionResult SendNews(string destination, string title, string text)
        {
            var model = new Models.NewsEmailViewModel
            {
                Title = title,
                Description = text,
                ImageLink = Request.Url.Scheme + "://" + @Request.Url.Authority + Url.Content("~/Images/ipsum.jpg"),
                ReadMoreLink = Request.Url.Scheme + "://" + @Request.Url.Authority + Url.Action("FakeNews")
            };
            model.WebVersion = Request.Url.Scheme + "://" + @Request.Url.Authority + Url.Action(EmailController.NEWS_EMAIL, "Email", model);
            EmailController.Send(HttpContext.Request.RequestContext, destination, "News Email Test", EmailController.NEWS_EMAIL, model);
            return RedirectToAction("Ok", new { destination = destination, email = model.WebVersion });
        }

        [HttpGet]
        public ActionResult Ok(string destination, string email)
        {
            ViewBag.Dest = destination;
            ViewBag.Email = email;
            return View();
        }

        [HttpGet]
        public ActionResult FakeConfirmation()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FakeNews()
        {
            return View();
        }
    }
}