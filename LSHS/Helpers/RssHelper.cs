using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using GalaSoft.MvvmLight.Threading;
using LSHS.Models;

namespace LSHS.Helpers
{
    public static class RssHelper
    {
        internal static void LoadTwitterFeedIntoListBox(ObservableCollection<Tweet> Items, string url)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() => Items.Clear());
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (sender, args) =>
            {
                if (args.Error == null)
                {
                    var xml = args.Result;
                    if (xml.Length == 0)
                        return;
                    XDocument doc = XDocument.Parse(xml);
                    var tweets = from item in doc.Root.Element("channel").Elements("item")
                                 select new Tweet()
                                 {
                                     Message = item.Element("title").Value,
                                     Permalink = item.Element("link").Value,
                                     PublishDate = item.Element("pubDate").Value.ParseDateTime(),
                                     UserName = "",
                                 };
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        foreach (var tweet in tweets)
                        {
                            Items.Add(tweet);
                        }
                    });
                }
                else
                {
                    throw args.Error;
                }
            };
            client.DownloadStringAsync(new Uri(url, UriKind.Absolute));
        }

        internal static void LoadCalendarFeedIntoListBox(ObservableCollection<CalendarItem> Items, string url)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() => Items.Clear());
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (sender, args) =>
            {
                if (args.Error == null)
                {
                    var xml = args.Result;
                    if (xml.Length == 0)
                        return;
                    XDocument doc = XDocument.Parse(xml);
                    var events = from item in doc.Root.Element("channel").Elements("item")
                                 select new CalendarItem()
                                 {
                                     Details = item.Element("description").Value,
                                     Slug = item.Element("title").Value,
                                     EventDateStart = item.Element("pubDate").Value.ParseDateTime(),
                                     Link = item.Element("link").Value,
                                 };
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        foreach (var @event in events)
                        {
                            Items.Add(@event);
                        }
                    });
                }
                else
                {
                    throw args.Error;
                }
            };
            client.DownloadStringAsync(new Uri(url, UriKind.Absolute));
        }
    }
}