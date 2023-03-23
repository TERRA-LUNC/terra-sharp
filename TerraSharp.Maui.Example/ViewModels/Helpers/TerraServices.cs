using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terra.Microsoft.Client.Constants;
using Terra.Microsoft.Client;
using Terra.Microsoft.Client.Core;
using Terra.Microsoft.Client.Core.Constants;
using Terra.Microsoft.Client.Key;
using Terra.Microsoft.Client.Core.Bank.Msgs;
using Terra.Microsoft.Client.Client.Lcd;
using Terra.Microsoft.Keys;

using Delegation = Terra.Microsoft.Client.Core.Staking.Delegation;

namespace TerraSharp.Maui.Example.ViewModels.Helpers
{
    public static class TerraServices
    {
        /// <summary>
        /// GetBalances
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static async Task<Coin[]> GetBalances(string address)
        {
            var coins = await TerraStartup.GetLCDClient().bank.GetBalance(address);

            return coins;
        }

        /// <summary>
        /// CreateWallet
        /// </summary>
        /// <param name="mnemonic"></param>
        /// <returns></returns>

        public static Wallet CreateWallet(TxMnemonic mnemonic)
        {
            //// Define your wallet -- The account that will handle the transactions
            var wallet = TerraStartup.GetLCDClient().CreateWallet(mnemonic);

            return wallet;
        }

        /// <summary>
        /// GetStaked
        /// </summary>
        /// <param name="accAddress"></param>
        /// <returns></returns>
        public static Task<Delegation[]> GetStaked(string accAddress)
        {
            var stakedLunc = TerraStartup.GetLCDClient().staking.GetDelegations(accAddress);
            return stakedLunc;
        }

        public static Task GetHistory()
        {

            //var history = TerraStartup.GetLCDClient()//history.GetHistory();
            return null;

        }
    }
}
