using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLedgerApp
{

    /// <summary>
    /// Users class
    /// </summary>
    class Users
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<History> HistoryList { get; set; }
        public decimal CurrentBalance { get; set; }

    }

    /// <summary>
    /// History Class used to support Users Class
    /// </summary>
    class History
    {
        public string TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}
