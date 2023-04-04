using Binance.Net.Clients;
using Binance.Net.Interfaces.Clients;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraSharp.Maui.Example.ViewModels.Helpers
{
    public static class CryptoExchangeServices
    {
        public static async Task<WebCallResult<IEnumerable<Order>>> GetClosedOrdersAsync(string symbol = null)
        {
            using (var client = new BinanceClient())
            {
                var spotTradeHistoryData = await client.SpotApi.CommonSpotClient.GetClosedOrdersAsync("LUNCBUSD");
                return spotTradeHistoryData;
            }

            
            
            
        }
    }
}
