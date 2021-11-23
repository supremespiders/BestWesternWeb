using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models
{

    [Index(nameof(TimeStamp), nameof(LogLevel))]
    public class Log
    {
        [Key]
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }
        public string LogLevel { get; set; }
        public string CallSite { get; set; }
        public string Message { get; set; }
    }

}