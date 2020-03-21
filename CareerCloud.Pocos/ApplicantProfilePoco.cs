using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CareerCloud.Pocos
{
    [Table("Applicant_Profiles")]
    public class ApplicantProfilePoco: IPoco
    {
        [Key]
        public Guid Id { get; set; }

        public Guid Login { get; set; }

        [Column("Current_Salary")]
        public Decimal? CurrentSalary { get; set; }
        [Column("Current_Rate")]
        public Decimal? CurrentRate { get; set; }
        public String Currency { get; set; }

        [Column("Country_Code")]
        [ForeignKey("SystemCountryCodePoco")]
        public String Country { get; set; }

        [Column("State_Province_Code")]
        public String Province { get; set; }

        [Column("Street_Address")]
        public String Street { get; set; }

        [Column("City_Town")]
        public String City { get; set; }

        [Column("Zip_Postal_Code")]
        public String PostalCode { get; set; }

        [Column("Time_Stamp")]
        [NotMapped]
        public Byte[] TimeStamp { get; set; }

        public virtual  ICollection<ApplicantEducationPoco> ApplicantEducation { get; set; }

        [ForeignKey("Login")]
        public virtual SecurityLoginPoco SecurityLogin { get; set; }

        public virtual ICollection<ApplicantResumePoco> ApplicantResumes { get; set; }

        public virtual ICollection<ApplicantSkillPoco> ApplicantSkills { get; set; }

        public virtual ICollection<ApplicantWorkHistoryPoco> ApplicantWorkHistoryPocos { get; set; }

        public virtual SystemCountryCodePoco SystemCountryCodePoco { get; set; }

        public virtual ICollection<ApplicantJobApplicationPoco> ApplicantJobApplicationPocos { get; set; }
    }
}
