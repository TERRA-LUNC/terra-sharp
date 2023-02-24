using System;
using System.Collections.Generic;
using System.Text;

namespace TerraSharp.Core
{
    public class CoinDenomOptions : Attribute
    {
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool MainnetSupport { get; set; }
        public bool TestNetSupport { get; set; }
        public double Decimals { get; set; }

        /// <summary>
        /// CoinDenomOptions
        /// </summary>
        /// <param name="description"></param>
        /// <param name="imageUrl"></param>
        /// <param name="mainnetSupport"></param>
        /// <param name="testNetSupport"></param>
        /// <param name="decimals"></param>
        public CoinDenomOptions(string description,string imageUrl, bool mainnetSupport, bool testNetSupport, double decimals) 
        {
            Description = description;
            ImageUrl= imageUrl;
            MainnetSupport= mainnetSupport;
            TestNetSupport= testNetSupport;
            Decimals= decimals;
        }
    }
}
