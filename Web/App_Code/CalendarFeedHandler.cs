using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using Google.GData.Calendar;

public class CalendarFeedHandler : IHttpHandler
{
    [DebuggerDisplay("\\{ Title = {Title}, StartTime = {StartTime}, EndTime = {EndTime}, Description = {Description}, Url = {Url} \\}")]
    public class EventItem
    {
        public EventItem()
        {
        }

        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }
    }

    private Dictionary<string, string> Urls = new Dictionary<string, string>();
    private const string BAND_Calendar = "https://www.google.com/calendar/feeds/561nkafpf3enpvbrf9h76h4o9c%40group.calendar.google.com/public/full?orderby=starttime&sortorder=ascending&futureevents=true";
    private const string GUARD_Calendar = "https://www.google.com/calendar/feeds/e86f81j4b3sh0p6n4harr6pr2o@group.calendar.google.com/public/full?orderby=starttime&sortorder=ascending&futureevents=true";

    public CalendarFeedHandler()
    {
        Urls.Add("band", BAND_Calendar);
        Urls.Add("guard", GUARD_Calendar);
    }

    public void ProcessRequest(HttpContext context)
    {
        HttpRequest Request = context.Request;
        HttpResponse Response = context.Response;
        // This handler is called whenever a file ending
        // in .sample is requested. A file with that extension
        // does not need to exist.
        // Create a CalenderService and authenticate
        CalendarService myService = new CalendarService("google_calendar");

        string key = Request.Url.Segments[Request.Url.Segments.Length - 1].Split(Char.Parse("."))[0];
        if (!Urls.ContainsKey(key))
            return;
        var url = Urls[key];
        var calendar = myService.Get(url);

        var entries = from entry in calendar.Feed.Entries
                      select entry as EventEntry;

        Response.Clear();
        Response.ContentType = "text/xml";

        XDocument document = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement("rss",
                 new XElement("channel",
                     new XElement("title", String.Format("Lone Star High School - {0} Calendar", key.ToUpper())),
                     new XElement("link", url),
                     new XElement("description", "Real RSS version of a Google Calendar feed as they obviously don't know how to build one"),
                    from entry in entries
                    where entry.Times.Count > 0
                    orderby entry.Times[0].StartTime
                    select new XElement("item",
                       new XElement("title", entry.Title.Text),
                       new XElement("description", entry.Content.Content),
                       new XElement("pubDate", entry.Times[0].StartTime.ToString("ddd, dd MMM yyyy H:mm:ss K")),
                       new XElement("link", entry.Links[0].HRef.Content)
                    ),
                 new XAttribute("version", "2.0"))));
        document.Save(Response.Output);
    }

    public bool IsReusable
    {
        // To enable pooling, return true here.
        // This keeps the handler in memory.
        get { return true; }
    }
}