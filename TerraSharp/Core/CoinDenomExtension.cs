using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Terra.Microsoft.Client.Core.Constants;

namespace TerraSharp.Core
{
    public static class CoinDenomExtension
    {
        /// <summary>
        /// GetCoinDenomOptions
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static CoinDenomOptions GetCoinDenomOptions(string @this)
        {
            // Get Member Info
            var memberInfo = typeof(CoinDenoms).GetMember(@this.ToString().ToUpper()).FirstOrDefault();
            if (memberInfo == null) return null;

            // Get Options
            var options = (CoinDenomOptions)Attribute.GetCustomAttribute(memberInfo, typeof(CoinDenomOptions));
            return options;
        }
       
    }
}
