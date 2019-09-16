using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Modal
{
    public class ConfirmationEmailModal : EmailModalBase
    {
        public string DayOfWeek { get; set; }

        public string TimeOfDay { get; set; }

        public string FirstCollectionDate { get; set; }

        public string TrialEndDate { get; set; }

        public string BagImg { get; set; }

        public string DoorstepImg { get; set; }

        public string MatrixImg { get; set; }
    }
}
