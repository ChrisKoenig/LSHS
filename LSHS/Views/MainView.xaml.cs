using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using LSHS.Messages;
using Microsoft.Phone.Controls;

namespace LSHS.Views
{
    public partial class MainView : PhoneApplicationPage
    {
        public MainView()
        {
            InitializeComponent();
            Messenger.Default.Register<NetworkUnavailableMessage>(this, (message) =>
            {
                MessageBox.Show("Network is currently unavailable.  Please try again later.");
            });
            Messenger.Default.Register<ErrorMessage>(this, (message) =>
            {
                MessageBox.Show(message.Error.Message, "An Error Occurred", MessageBoxButton.OK);
            });
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            Messenger.Default.Send<RefreshDataMessage>(new RefreshDataMessage());
        }
    }
}