using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LSHS.Models
{
    public class Tweet
    {
        public string UserName { get; set; }

        public string Message { get; set; }

        public DateTime PublishDate { get; set; }

        public string Permalink { get; set; }
    }
}