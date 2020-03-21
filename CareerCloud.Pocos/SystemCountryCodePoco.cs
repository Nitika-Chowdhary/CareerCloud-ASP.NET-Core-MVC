﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CareerCloud.Pocos
{
    [Table("System_Country_Codes")]
    public class SystemCountryCodePoco
    {
        [Key]
        public String Code { get; set; }

        public String Name { get; set; }

        public virtual ApplicantProfilePoco ApplicantProfilePoco { get; set; }

        public virtual ICollection<ApplicantWorkHistoryPoco> ApplicantWorkHistoryPocos { get; set; }

    }
}
