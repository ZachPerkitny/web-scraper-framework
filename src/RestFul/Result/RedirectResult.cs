using System;
using RestFul.Enum;
using RestFul.Http;

namespace RestFul.Result
{
    public class RedirectResult : Result
    {
        /// <summary>
        /// Get or Sets the Url to redirect to
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets a flag to indicate whether
        /// this is a temporary redirect (307) or
        /// permanent (308).
        /// </summary>
        public bool Permanent { get; set; }

        /// <summary>
        /// Initializes a RedirectResult with redirectUrl provided
        /// </summary>
        /// <param name="redirectUrl"></param>
        public RedirectResult(string redirectUrl)
        {
            RedirectUrl = redirectUrl;
        }

        /// <summary>
        /// Initializes a RedirectResult with the redirectUrl and
        /// and permanent flag.
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="permanent"></param>
        public RedirectResult(string redirectUrl, bool permanent)
            : this (redirectUrl)
        {
            Permanent = permanent;
        }

        public override void Execute(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Response.Redirect(RedirectUrl);
            // TODO(zvp): Handle 302
            context.Response.StatusCode = (Permanent) ?
                HttpStatusCode.PermanentRedirect : HttpStatusCode.TemporaryRedirect;
            context.Response.Send();
        }
    }
}
