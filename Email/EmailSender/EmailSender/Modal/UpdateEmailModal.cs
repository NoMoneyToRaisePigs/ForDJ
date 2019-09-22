using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Modal
{
    public class UpdateEmailModal : EmailModalBase
    {
        public string LegendInfo { get; set; }

        public int PositiveRate { get; set; }

        public int NegativeRate { get; set; }

        public string ErrorImg { get; set; }

        public string StarFillImg { get; set; }

        public string StarStripeImg { get; set; }

        public RecycleItem[] UnRecyclables { get; set; }

        public RecycleItem[] Recyclables { get; set; }

        public double TotalWeight { get; set; }
    }

    public class RecycleItem
    {
        public string Name { get; set; }

        public double Weight { get; set; }

        public double Percent { get; set; }
    }

}
