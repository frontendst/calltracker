using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CallTracking.DB_Classes
{
    class Config
    {
        public Config()
        {
            this.Banks = new HashSet<Bank>();
            this.Records = new HashSet<Record>();
        }
        [Key]
        public string EmailAdress { get; set; }
        public string FullName { get; set; }
        public virtual ICollection<Bank> Banks { get; set; }
        public virtual ICollection<Record> Records { get; set; } 
    }
}

