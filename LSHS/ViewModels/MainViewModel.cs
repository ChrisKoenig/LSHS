using System;
using System.Collections.ObjectModel;
using System.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LSHS.Helpers;
using LSHS.Messages;
using LSHS.Models;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Tasks;

namespace LSHS.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly static string RSS_Director = "http://twitter.com/statuses/user_timeline/118956202.rss";
        private readonly static string RSS_Band = "http://lshs.chriskoenig.net/band.calendar";
        private readonly static string RSS_Guard = "http://lshs.chriskoenig.net/guard.calendar";
        private readonly static string RSS_FriscoISD = "http://twitter.com/statuses/user_timeline/24252117.rss";

        public MainViewModel()
        {
            MessengerInstance.Register<RefreshDataMessage>(this, (message) => RefreshData());

            LaunchBrowserCommand = new RelayCommand<string>((url) =>
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    WebBrowserTask task = new WebBrowserTask();
                    task.URL = url;
                    task.Show();
                }
                else
                {
                    MessengerInstance.Send<NetworkUnavailableMessage>(new NetworkUnavailableMessage());
                }
            });

            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real": Connect to service, etc...
                RefreshData();
            }
        }

        private void RefreshData()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                ThreadPool.QueueUserWorkItem((action) => RssHelper.LoadTwitterFeedIntoListBox(DirectorTweets, RSS_Director));
                ThreadPool.QueueUserWorkItem((action) => RssHelper.LoadCalendarFeedIntoListBox(BandEvents, RSS_Band));
                ThreadPool.QueueUserWorkItem((action) => RssHelper.LoadCalendarFeedIntoListBox(GuardEvents, RSS_Guard));
                ThreadPool.QueueUserWorkItem((action) => RssHelper.LoadTwitterFeedIntoListBox(DistrictFeed, RSS_FriscoISD));
            }
            else
            {
                MessengerInstance.Send<NetworkUnavailableMessage>(new NetworkUnavailableMessage());
            }
        }

        public RelayCommand<string> LaunchBrowserCommand { get; private set; }

        #region DirectorTweets property

        private ObservableCollection<Tweet> _directorTweets = new ObservableCollection<Tweet>();

        public ObservableCollection<Tweet> DirectorTweets
        {
            get
            {
                return _directorTweets;
            }

            set
            {
                if (_directorTweets == value)
                {
                    return;
                }

                var oldValue = _directorTweets;
                _directorTweets = value;
                RaisePropertyChanged(() => this.DirectorTweets);
            }
        }

        #endregion DirectorTweets property

        #region BandEvents property

        private ObservableCollection<CalendarItem> _bandEvents = new ObservableCollection<CalendarItem>();

        public ObservableCollection<CalendarItem> BandEvents
        {
            get
            {
                return _bandEvents;
            }

            set
            {
                if (_bandEvents == value)
                {
                    return;
                }

                var oldValue = _bandEvents;
                _bandEvents = value;
                RaisePropertyChanged(() => this.BandEvents);
            }
        }

        #endregion BandEvents property

        #region GuardEvents property

        private ObservableCollection<CalendarItem> _guardEvents = new ObservableCollection<CalendarItem>();

        public ObservableCollection<CalendarItem> GuardEvents
        {
            get
            {
                return _guardEvents;
            }

            set
            {
                if (_guardEvents == value)
                {
                    return;
                }

                var oldValue = _guardEvents;
                _guardEvents = value;
                RaisePropertyChanged(() => this.GuardEvents);
            }
        }

        #endregion GuardEvents property

        #region DistrictFeed property

        private ObservableCollection<Tweet> _districtFeed = new ObservableCollection<Tweet>();

        public ObservableCollection<Tweet> DistrictFeed
        {
            get
            {
                return _districtFeed;
            }

            set
            {
                if (_districtFeed == value)
                {
                    return;
                }

                var oldValue = _districtFeed;
                _districtFeed = value;
                RaisePropertyChanged(() => this.DistrictFeed);
            }
        }

        #endregion DistrictFeed property
    }
}