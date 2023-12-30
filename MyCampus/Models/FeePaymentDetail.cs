using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus.Models
{
    public class FeePaymentDetail
    {
        public int FeeId { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
        public string ModeOfPayment { get; set; }
        public int StudentId { get; set; }
    }
}
