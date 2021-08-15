using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Villa_Client.Model.ViewModel
{
    public class HomeViewModel
    {
        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; }

        public int NoOfNights { get; set; } = 1;
    }
}
