using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public class Expense
    {
        public DateTime date { get; set; }
        public string name { get; set; }
        public Decimal price { get; set; }
        public Expense(DateTime date, string name, decimal price)    
        {
            this.date = date;
            this.name = name;
            this.price = price;
        }
    }
}
