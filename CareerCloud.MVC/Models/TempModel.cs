using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareerCloud.MVC.Models
{
    public class TempModel
    {
        public Guid Applicant { get; set; }
        public string Job { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}
