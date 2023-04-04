using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using Terra.Microsoft.Client;
using Terra.Microsoft.Client.Core;
using Terra.Microsoft.Client.Core.Bank.Msgs;
using Terra.Microsoft.Client.Core.Constants;
using Terra.Microsoft.Client.Key;
using Terra.Microsoft.Rest.Tx.Block;
using TerraSharp.Core;
using TerraSharp.Maui.Example.Data;
using TerraSharp.Maui.Example.Models;
using TerraSharp.Maui.Example.Pages.Components;
using TerraSharp.Maui.Example.ViewModels;
using TerraSharp.Maui.Example.ViewModels.Helpers;


namespace TerraSharp.Maui.Example.Pages
{
    public partial class MainPage : ContentPage
    {
        MainViewModel vm;
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            TerraStartup.InitializeKernel(Terra.Microsoft.Rest.Configuration.Environment.TerraEnvironment.LUNA2TestNet);

        }
        /// <summary>
        /// OnCounterClicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCounterClicked(object sender, EventArgs e)
        {
            BlockTxBroadcastResultDataArgs result = new BlockTxBroadcastResultDataArgs();
            Task.Run(async () =>
            {
                // Create a key out of a mnemonic
                var mnemonic = new TxMnemonic("desert excite employ minute exile flash finish inmate bleak alter bid raise resource spatial crumble spread toddler exit inflict soup real draft analyst illegal");
                // Define the recipient address
                var rAddr = "terra1g9flaqfprlx2a62nm4k9c80u5e4alv8ghk6nwr";
                // Define your wallet -- The account that will handle the transactions
                var wallet = TerraServices.CreateWallet(mnemonic);
                var send = new MsgSend(
                  mnemonic.AccAddress,
                  rAddr,
                   new List<Coin>() { new Coin(CoinDenoms.ULUNA, 1 * 1e6) });

                var msgs = new object[] { send };

                var gas = await wallet.EstimateGasForTx(msgs, 1 * 1e6);
                Debug.WriteLine($"Gas : {gas}");
                var feeEstimate = await wallet.EstimateFeeForTx(new CreateTxOptions()
                {
                    gas = gas,
                    feeDenom = CoinDenoms.ULUNA,
                });
                //vm.WalletAmounts.Add(new WalletTotal { Amount = 45, CoinDenoms = "Luna" });
                Debug.WriteLine($"feeEstimate : {feeEstimate.amount.First().amount}");
                var txAfterGas = await wallet.CreateTxAndSignTx(
                        feeEstimate,
                        msgs);
                Debug.WriteLine($"JSON: \n {JsonConvert.SerializeObject(txAfterGas)}");


                result = await wallet.broadcastTx.Broadcast(txAfterGas);

            }).Wait();
            DisplayAlert("Result", result.Raw_log.ToString(), "OK");
        }
        /// <summary>
        /// ContentPage_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            vm = new MainViewModel();
            Task.Run(async () =>
            {
                // Create a key out of a mnemonic
                var mnemonic = new TxMnemonic("desert excite employ minute exile flash finish inmate bleak alter bid raise resource spatial crumble spread toddler exit inflict soup real draft analyst illegal");

                // Define your wallet -- The account that will handle the transactions
                var wallet = TerraServices.CreateWallet(mnemonic);
                var closedOrders = CryptoExchangeServices.GetClosedOrdersAsync();

                foreach (var closedOrder in closedOrders.Result.Data.ToList())
                {

                    vm.Logs.Add(new Models.Log
                    {
                        Created = closedOrder.Timestamp,
                        Details = closedOrder.QuantityFilled.ToString(),
                        Message = closedOrder.Price.ToString(),
                        Type = LogTypes.Exchange,

                    });

                }

                var coins = await TerraServices.GetBalances(mnemonic.AccAddress);

                var stakedCoins = await TerraServices.GetStaked(mnemonic.AccAddress);
                vm.ObservableValueStaked.Value = stakedCoins.FirstOrDefault() == null ? 0 : stakedCoins.FirstOrDefault().balance.amount;
                vm.CurrentAddress = mnemonic.AccAddress;
                vm.Url = "https://finder.terra.money/classic/address/" + mnemonic.AccAddress;
                foreach (var coin in coins)
                {

                    var options = CoinDenomExtension.GetCoinDenomOptions(coin.denom);

                    vm.ObservableValueLunc.Value = coin.amount / 1e6;

                    if (options != null)
                    {

                        vm.Logs.Add(new Models.Log
                        {
                            Created = DateTime.Now,
                            Details = options.Description,
                            Message = Convert.ToDecimal(coin.amount / 1000000).ToString(),
                            Type = LogTypes.Bank,
                            Image = options.ImageUrl
                        });


                    }

                }
            }).Wait();

            vm.ObservableValueTotal.Value = vm.ObservableValueStaked.Value + vm.ObservableValueLunc.Value;

            collectionView.ItemsSource = vm.Logs;
            pieChart1.BindingContext = vm;
            pageHeader1.BindingContext = vm;



            WalletsDatabase wdb = new WalletsDatabase();
            //var existingLogs = await wdb.GetLogsAsync();

            //foreach (var existingLog in existingLogs)
            //{
            //    vm.Logs.Add(existingLog);
            //}

            //vm.Logs.Add(new Models.Log
            //{
            //    Created = DateTime.Now,
            //    Details = "Application started",
            //    Message = "Start",
            //    Type = LogTypes.Information,
            //});


            //await wdb.SaveLogItemAsync(new Models.Log
            //{
            //    Created = DateTime.Now,
            //    Details = "Application started",
            //    Message = "Start",
            //    Type = LogTypes.Information
            //});

        }
    }
}
