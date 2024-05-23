using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theYesCoinConsole.Models
{
    public class Account
    {
        public Account(string userId, int userLevel, int rankingUser, int totalAmount, int currentAmount, string tokenAccount)
        {
            this.userId = userId;
            this.userLevel = userLevel;
            this.rankingUser = rankingUser;
            this.totalAmount = totalAmount;
            this.currentAmount = currentAmount;
            this.tokenAccount = tokenAccount;
        }

        public string userId { get; set; }
        public int userLevel { get; set; }
        public int rankingUser { get; set; }
        public int totalAmount { get; set; }
        public int currentAmount { get; set; }
        public string tokenAccount { get; set; }
    }
}
