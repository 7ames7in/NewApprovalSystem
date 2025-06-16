using System;

namespace ApprovalPaging.Models;

public class Worker
{
    public string Name { get; set; }
    public decimal PayRate { get; set; }

    public static IComparer<Worker> RateComparer { get; } = new PayRateComparer();

    private class PayRateComparer : IComparer<Worker>
    {
        public int Compare(Worker x, Worker y)
        {
            return x.PayRate.CompareTo(y.PayRate);
        }
    }
}