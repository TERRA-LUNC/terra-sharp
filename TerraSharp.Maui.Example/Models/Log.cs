using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace TerraSharp.Maui.Example.Models
{
    [Table("log")]
    public class Log
    {
        // PrimaryKey is typically numeric 
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Type { get; set; }

        [MaxLength(250)]
        public string? Message { get; set; }

        public DateTime Created { get; set; }

        [MaxLength(250)]
        public string? Details { get; set; }

        [Ignore]
        public string Image { get; set; }
    }

    public static class LogTypes
    {
        public const string Login = "Login";
        public const string Error = "Error";
        public const string Information = "Information";
        public const string Fatal = "Fatal";
        public const string Forbidden = "Forbidden";
        public const string Bank = "Bank";
        public const string Wallet = "Wallet";
        public const string Transaction = "Transaction";
        public const string Broadcast = "Broadcast";
    }
}
