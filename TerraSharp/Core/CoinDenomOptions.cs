using System;
using System.Collections.Generic;
using System.Text;

namespace TerraSharp.Core
{
    public class CoinDenomOptions : Attribute
    {
        public string ImageUrl { get; set; }
        public bool MainnetSupport { get; set; }
        public bool TestNetSupport { get; set; }
        public double Decimals { get; set; }
        public CoinDenomOptions(string imageUrl, bool mainnetSupport, bool testNetSupport, double decimals) 
        {
            ImageUrl= imageUrl;
            MainnetSupport= mainnetSupport;
            TestNetSupport= testNetSupport;
            Decimals= decimals;
        }
    }
}
