// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Originally published at http://damieng.com/blog/2010/04/26/creating-rss-feeds-in-asp-net-mvc

using System;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace DamienG.Web.Mvc
{
    /// <summary>
    /// ActionResult for System.Web.Mvc that turns SyndicationFeedFormatter into a RSS/Atom feed.
    /// </summary>
    public class FeedResult : ActionResult
    {
        readonly SyndicationFeedFormatter feed;

        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }

        public FeedResult(SyndicationFeedFormatter feed)
        {
            this.feed = feed ?? throw new ArgumentNullException(nameof(feed));
        }

        public SyndicationFeedFormatter Feed
        {
            get { return feed; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

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