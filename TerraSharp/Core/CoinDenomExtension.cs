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
        public static CoinDenomOptions GetBlockchainOptions(this CoinDenoms @this)
        {
            // Get Member Info
            var memberInfo = typeof(CoinDenoms).GetMember(@this.ToString()).FirstOrDefault();
            if (memberInfo == null) return null;

            // Get Options
            var options = (CoinDenomOptions)Attribute.GetCustomAttribute(memberInfo, typeof(CoinDenomOptions));
            return options;
        }
        public static string GetDescription(this Enum CoinDenoms)
        {
            Type genericEnumType = CoinDenoms.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(CoinDenoms.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return CoinDenoms.ToString();
        }
    }
}
