using System;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.Routing;
using System.Net.Mail;
using System.Configuration;
using System.Net.Mime;

namespace Mailer.Controllers
{
    public class EmailController : Controller
    {
        private static readonly string SENDER_LOGIN = ConfigurationManager.AppSettings["AuthEmail"];
        private static readonly string SENDER_PASSWORD = ConfigurationManager.AppSettings["AuthPassword"];
        private static readonly string SENDER_HOST = ConfigurationManager.AppSettings["AuthHost"];
        private static readonly int SENDER_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["AuthPort"]);

        public static readonly string CONFIRMATION_EMAIL = "Confirmation";
        public static readonly string NEWS_EMAIL = "News";

        private static string RenderToString(RequestContext context, string view, Models.EmailViewModelBase model)
        {
            if (context == null)
            {
                throw new ArgumentNullException("A request context is required to render an email view to a string.");
            }
            if (string.IsNullOrEmpty(view))
            {
                throw new ArgumentException("A view to render is not specified.");
            }
            if (model == null)
            {
                throw new ArgumentNullException("You have not provided data to build an email.");
            }
            var controller = (ControllerBase)ControllerBuilder.Current.GetControllerFactory().CreateController(context, "Email");
            var controllerContext = new ControllerContext(context, controller);
            ViewEngines.Engines.Add(new EmailViewEngine());
            var renderedView = ViewEngines.Engines.FindPartialView(controllerContext, view).View;
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                using (var tw = new HtmlTextWriter(sw))
                {
                    var viewData = new ViewDataDictionary();
                    var tempData = new TempDataDictionary();
                    viewData.Model = model;
                    renderedView.Render(new ViewContext(controllerContext, renderedView, viewData, tempData, tw), tw);
                }
            }
            return sb.ToString();
        }

        public static void Send(RequestContext context, string destination,
            string subject, string view, Models.EmailViewModelBase model)
        {
            if (string.IsNullOrEmpty(SENDER_LOGIN) || string.IsNullOrEmpty(SENDER_PASSWORD) || string.IsNullOrEmpty(SENDER_HOST) || SENDER_PORT == 0)
            {
                throw new ConfigurationErrorsException("Please, provide a sender credentials in Web.config to use mailer (AuthEmail, AuthPassword, AuthHost, AuthPort).");
            }
            if (string.IsNullOrEmpty(destination))
            {
                throw new ArgumentNullException("Please, provide a mail destination.");
            }

            string body = RenderToString(context, view, model);

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(SENDER_LOGIN);
            msg.To.Add(new MailAddress(destination));
            msg.Subject = subject;
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html));

            SmtpClient smtpClient = new SmtpClient(SENDER_HOST, SENDER_PORT);
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(SENDER_LOGIN, SENDER_PASSWORD);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = credentials;
            if (SENDER_PORT == 25)
            {
                smtpClient.EnableSsl = false;
            }
            else
            {
                smtpClient.EnableSsl = true;
            }
            smtpClient.Send(msg);
        }

        // GET: Email/Confirmation
        public ActionResult Confirmation(Models.ConfirmationEmailViewModel model)
        {
            return View(model);
        }

        // GET: Email/News
        public ActionResult News(Models.NewsEmailViewModel model)
        {
            return View(model);
        }
    }

    public class EmailViewEngine : RazorViewEngine
    {
        public EmailViewEngine() : base()
        {
            PartialViewLocationFormats = new string[]
            {
                "~/Views/Email/{0}.cshtml"
            };
        }
    }
}