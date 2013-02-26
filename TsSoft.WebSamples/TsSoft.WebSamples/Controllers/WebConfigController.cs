namespace TsSoft.WebSamples.Controllers
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;
    using TsSoft.WebSamples.ConfigSections;
    using TsSoft.WebSamples.Helpers;
    using TsSoft.WebSamples.Models;

    public class WebConfigController : Controller
    {
        private static readonly EncryptHelper encryptHelper = new EncryptHelper();

        public ActionResult Index()
        {
            var section = ((SocialConfigSection)ConfigurationManager.GetSection("socialSettings"));
            var b = (ConnectionStringsSection)ConfigurationManager.GetSection("connectionStrings");
            var c = b.ConnectionStrings.Cast<ConnectionStringSettings>().ToList();
            var a = section.GetValue("Vk.appId");
            return View();
        }

        [HttpPost]
        public JsonResult Execute(WebConfigActions action, string sectionName, string providerName)
        {
            switch (action)
            {
                case WebConfigActions.Encrypt:
                    return EncryptSection(sectionName, providerName);
                case WebConfigActions.Decrypt:
                    return DecryptSection(sectionName);
                default:
                    return Json(new { status = "error", message = "Action not found" });
            }
        }

        private JsonResult EncryptSection(string sectionName, string providerName)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(providerName))
                {
                    encryptHelper.Encrypt(sectionName);
                }
                else
                {
                    encryptHelper.Encrypt(sectionName, providerName);
                }
                return Json(new { status = "success" });
            }
            catch (Exception e)
            {
                return Json(new { status = "error", message = e.Message });
            }
        }

        private JsonResult DecryptSection(string sectionName)
        {
            try
            {
                encryptHelper.Decrypt(sectionName);
                return Json(new { status = "success" });
            }
            catch (Exception e)
            {
                return Json(new { status = "error", message = e.Message });
            }
        }
    }
}