using System.Collections.Generic;

namespace CallTracking.DB_Classes
{
    class Bank
    {
        public Bank()
        {
            this.Records = new List<Record>();
        }

        public int BankId { get; set; }
        public string BankName { get; set; }

        public virtual ICollection<Config> Configs { get; set; } 
        public virtual ICollection<Record> Records { get; set; } 
    }


}
