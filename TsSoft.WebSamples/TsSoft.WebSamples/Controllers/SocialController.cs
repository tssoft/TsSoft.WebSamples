namespace TsSoft.WebSamples.Controllers
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;
    using TsSoft.Social.Facebook;
    using TsSoft.Social.OAuth;
    using TsSoft.Social.Twitter;
    using TsSoft.Social.Vk;
    using TsSoft.WebSamples.ConfigSections;
    using TsSoft.WebSamples.Models;

    /// <author>Pavel Kurdikov</author>
    public class SocialController : Controller
    {
        private SocialContext context = new SocialContext();
        private const string VkName = "vk";
        private const string FacebookName = "facebook";
        private const string TwitterName = "twitter";
        private static readonly SocialConfigSection socialSection = ((SocialConfigSection)ConfigurationManager.GetSection("socialSettings"));
        private static readonly UrlHelper urlHelper = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);

        private static VkAuthorization vk;

        private static FacebookAuthorization facebook;

        private static AccessCredentials twitterCredentials = new AccessCredentials();

        static SocialController()
        {
            vk = new VkAuthorization(
                socialSection.GetValue("Vk.appId"),
                socialSection.GetValue("Vk.appSecret"),
                ConfigurationManager.AppSettings["AppBaseUrl"] + urlHelper.Content(urlHelper.Action("VkAuthResponse"))
            );

            facebook = new FacebookAuthorization(
                socialSection.GetValue("Facebook.appId"),
                socialSection.GetValue("Facebook.appSecret"),
                ConfigurationManager.AppSettings["AppBaseUrl"] + urlHelper.Content(urlHelper.Action("FacebookAuthResponse"))
            );

            twitterCredentials.ConsumerKey = socialSection.GetValue("Twitter.consumerKey");
            twitterCredentials.ConsumerSecret = socialSection.GetValue("Twitter.consumerSecret");
        }

        public ActionResult Index(string message = null)
        {
            ViewBag.Message = message;

            var model = new SocialViewModel
            {
                FacebookAuthorized = context.FacebookUser.Any(x => x.ExpireTime > DateTime.Now),
                VkAuthorized = context.VkUser.Any(),
                TwitterAuthorized = context.TwitterUser.Any()
            };

            return View(model);
        }

        public RedirectResult VkAuth()
        {
            vk.AppendRight(new[] { VkRight.Offline, VkRight.Notes });
            return Redirect(vk.GetCodeRedirectString());
        }

        public ActionResult VkAuthResponse(string code)
        {
            VkUser user;
            try
            {
                user = vk.GetUser(code);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { message = e.Message });
            }
            if (context.VkUser.Count() != 0)
            {
                context.VkUser.Remove(context.VkUser.First());
            }
            context.VkUser.Add(user);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public RedirectResult FacebookAuth()
        {
            string state = Guid.NewGuid().ToString("N");
            Session.Add("state", state);
            facebook.AppendRight(new[] { FacebookRight.PublishActions, FacebookRight.PublishStream, FacebookRight.ManagePages });
            return Redirect(facebook.GetRedirectString(state));
        }

        public ActionResult FacebookAuthResponse(string state, string code, string access_token, string error_reason)
        {
            FacebookUser user;
            try
            {
                user = facebook.GetUser(code);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { message = e.Message });
            }
            if (context.FacebookUser.Count() != 0)
            {
                context.FacebookUser.Remove(context.FacebookUser.First());
            }
            context.FacebookUser.Add(user);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ClearAuthorization()
        {
            if (context.FacebookUser.Any())
            {
                context.FacebookUser.Remove(context.FacebookUser.First());
            }
            if (context.VkUser.Any())
            {
                context.VkUser.Remove(context.VkUser.First());
            }
            if (context.TwitterUser.Any())
            {
                context.TwitterUser.Remove(context.TwitterUser.First());
            }
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult MessageRequest(string message, string titleMessage, string socialName)
        {
            var twitterUserId = (string) Session["userId"];
            switch (socialName)
            {
                case VkName:
                    return VkRequest(message, titleMessage);

                case FacebookName:
                    return FacebookRequest(message);

                case TwitterName:
                    return TwitterRequest(message);

                default:
                    return Json("");
            }
        }

        [NonAction]
        private JsonResult TwitterRequest(string message)
        {
            var result = new { status = "error", message = "twitter error: You must be authorized" };

            if (context.TwitterUser.Count() == 0)
            {
                return Json(result);
            }

            var user = context.TwitterUser.First();

            result = new { status = "success", message = "twitter message sent" };

            var command = new SendMessageCommand();
            command.Tokens = user.AccessTokens;
            command.Tokens.ConsumerKey = twitterCredentials.ConsumerKey;
            command.Tokens.ConsumerSecret = twitterCredentials.ConsumerSecret;
            command.Text = message;
            try
            {
                command.Execute();
            }
            catch(Exception e)
            {
                result = new { status = "error", message = "twitter error: " + e.Message };
            }

            return Json(result);
        }

        private JsonResult VkRequest(string message, string title)
        {
            if (context.VkUser.Count() == 0)
            {
                return Json(new { status = "error", message = "vk.com error: " + "You must be authorized" });
            }
            var publisher = new VkPublisher();
            publisher.User = context.VkUser.First();
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    title = TsSoft.Commons.Text.Formatter.ShortenTextByWord(message, 20);
                }
                var response = publisher.Publish(title, message);
            }
            catch (Exception e)
            {
                return Json(new { status = "error", message = "vk.com error: " + e.Message });
            }
            return Json(new { status = "success", message = "vk.com message sent" });
        }

        private JsonResult FacebookRequest(string message)
        {
            var publisher = new FacebookPublisher();
            if (context.FacebookUser.Count() == 0)
            {
                return Json(new { status = "error", message = "Facebook error: " + "You must be authorized" });
            }
            publisher.User = context.FacebookUser.First();
            if (publisher.User.ExpireTime < DateTime.Now)
            {
                context.FacebookUser.Remove(publisher.User);
                context.SaveChanges();
                return Json(new { status = "error", message = "Facebook error: " + "Токен истек. Авторизуйтесь заново" });
            }
            try
            {
                var response = publisher.Publish(message);
            }
            catch (Exception e)
            {
                return Json(new { status = "error", message = "Facebook error: " + e.Message });
            }
            return Json(new { status = "success", message = "facebook message sent" });
        }

        public ActionResult TwitterAuth()
        {
            var requestTokenCmd = new GetRequestTokenCommand();

            requestTokenCmd.Tokens.ConsumerKey = twitterCredentials.ConsumerKey;
            requestTokenCmd.Tokens.ConsumerSecret = twitterCredentials.ConsumerSecret;

            requestTokenCmd.CallbackUrl = Url.Action("TwitterCallback", null, null, Request.Url.Scheme);
            requestTokenCmd.Method = "POST";

            var response = requestTokenCmd.Execute();

            Session.Add("requestToken", response.Token);
            Session.Add("verifyString", response.Verifier);

            if (!string.IsNullOrEmpty(response.Token))
            {
                var authorizeCmd = new AuthorizeCommand();
                authorizeCmd.RequestTokens = response;
                return Redirect(authorizeCmd.Execute());
            }

            throw new ArgumentNullException("RequestTokens", "Не удалось получить токен запроса");
        }

        public ActionResult TwitterCallback()
        {
            if (Request["oauth_token"] == null)
            {
                throw new ArgumentNullException("oauth_token");
            }

            var getAccessTokenCmd = new GetAccessTokenCommand();

            getAccessTokenCmd.RequestTokens = new RequestCredentials
            {
                Token = (string)Request["oauth_token"],
                Verifier = (string)Request["oauth_verifier"]
            };

            getAccessTokenCmd.Tokens.ConsumerKey = twitterCredentials.ConsumerKey;
            getAccessTokenCmd.Tokens.ConsumerSecret = twitterCredentials.ConsumerSecret;

            var accessCredentials = getAccessTokenCmd.Execute();

            if (!string.IsNullOrEmpty(accessCredentials.AccessToken) && !string.IsNullOrEmpty(accessCredentials.AccessTokenSecret))
            {
                Session.Add("accessToken", accessCredentials.AccessToken);
                Session.Add("secret", accessCredentials.AccessTokenSecret);
            }

            if (context.TwitterUser.Count() > 0)
            {
                context.TwitterUser.Remove(context.TwitterUser.First());
            }

            var userId = Guid.NewGuid();
            Session.Add("userId", userId.ToString());


            context.TwitterUser.Add(new TwitterUser
            {
                AccessTokens = accessCredentials,
                Id = userId.ToString()
            });

            context.SaveChanges();

            return RedirectToAction("Index");
        }
        

    }
}