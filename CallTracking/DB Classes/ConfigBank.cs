using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.DB_Classes
{
    class ConfigBank
    {
     
        [Key]
        public int BankId { get; set; }
        [Key]
        public string EmailAdress { get; set; }

        public virtual ICollection<Config> Configs { get; set; }
        public virtual ICollection<Bank> Banks { get; set; }

    }
}
