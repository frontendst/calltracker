using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallTracking.DB_Classes
{
    class Record
    {
        public Record() { }

        public int RecordId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StartTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime FinishTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string Comment { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual Config Config { get; set; }
    }
}
