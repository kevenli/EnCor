using System;

namespace EnCor.Wcf.Routing.Algorithms
{
    public class NodeInfo
    {
        private int _Load = 0;
        public string Name
        {
            get;
            internal set;
        }

        public string Action
        {
            get;
            internal set;
        }

        public string Address
        {
            get;
            internal set;
        }

        public Uri Uri
        {
            get
            {
                return new Uri(Address);
            }
        }

        public int Rate
        {
            get;
            internal set;
        }

        public int Load
        {
            get { return _Load; }
        }

        //AutoDelist FailTime, to record first failed time in the period
        public DateTime? FailTime { get; set; }

        //FailCounter in the period
        public int FailCount { get; set; }


        public DateTime? DelistedTime { get; set; }

        public void IncreaseLoad()
        {
            System.Threading.Interlocked.Add(ref _Load, 1);
        }

        public void DecreaseLoad()
        {
            System.Threading.Interlocked.Add(ref _Load, -1);
        }
    }
}
