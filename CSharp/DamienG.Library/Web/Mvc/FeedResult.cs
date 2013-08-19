using System;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace DamienG.Web.Mvc
{
    /// <summary>
    /// ActionResult for System.Web.Mvc that turns SyndicationFeedFormatter into
    /// a RSS/Atom feed.
    /// </summary>
    public class FeedResult : ActionResult
    {
        private readonly SyndicationFeedFormatter feed;

        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }

        public FeedResult(SyndicationFeedFormatter feed)
        {
            this.feed = feed;
        }

        public SyndicationFeedFormatter Feed
        {
            get { return feed; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/rss+xml";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (feed != null)
                using (var xmlWriter = new XmlTextWriter(response.Output) { Formatting = Formatting.Indented })
                    feed.WriteTo(xmlWriter);
        }
    }
}