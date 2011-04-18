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
    public class CalendarItem
    {
        public DateTime? EventDateStart { get; set; }

        public DateTime? EventDateEnd { get; set; }

        public string Slug { get; set; }

        public string Details { get; set; }

        public string Link { get; set; }
    }
}