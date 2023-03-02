using Newtonsoft.Json;
using System;
using System.IO;
using Terra.Microsoft.Client;
using Terra.Microsoft.Client.Core;
using Terra.Microsoft.Client.Core.Bank.Msgs;
using Terra.Microsoft.Client.Core.Constants;
using Terra.Microsoft.Client.Key;
using Terra.Microsoft.Rest.Tx.Block;
using TerraSharp.Core;
using TerraSharp.Maui.Example.Data;
using TerraSharp.Maui.Example.Models;
using TerraSharp.Maui.Example.ViewModels;
using TerraSharp.Maui.Example.ViewModels.Helpers;

namespace TerraSharp.Maui.Example.Pages
{
    public partial class MainPage : ContentPage
    {
        MainViewModel vm;
        WalletsDatabase database;
        public MainPage(WalletsDatabase walletsDatabase)
        {
            InitializeComponent();
            database = walletsDatabase;
            TerraStartup.InitializeKernel(Terra.Microsoft.Rest.Configuration.Environment.TerraEnvironment.Classic);
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {

                //Name = "test2",
                //Address = "terra17lmam6zguazs5q5u6z5mmx76uj63gldnse2pdp",
                //Pubkey = "terrapub1addwnpepqdw9s9agjmw4fgntfuytd2x7qha94zlvv0edntkt7g3amz4wg75ewy9755w",
                //Mnemonic = "quality vacuum heart guard buzz spike sight swarm shove special gym robust assume sudden deposit grid alcohol choice devote leader tilt noodle tide penalty"

                // Create a key out of a mnemonic
                var mnemonic = new TxMnemonic("desert excite employ minute exile flash finish inmate bleak alter bid raise resource spatial crumble spread toddler exit inflict soup real draft analyst illegal");

                // Define the recipient address
                var rAddr = "terra1g9flaqfprlx2a62nm4k9c80u5e4alv8ghk6nwr";

                // Define your wallet -- The account that will handle the transactions
                var wallet = TerraServices.CreateWallet(mnemonic);

                var coins = await TerraServices.GetBalances(mnemonic.AccAddress);

                
                foreach (var coin in coins)
                {
                    
                    var options = CoinDenomExtension.GetCoinDenomOptions(coin.denom);
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

                var send = new MsgSend(
                  mnemonic.AccAddress,
                  rAddr,
                   new List<Coin>() { new Coin(CoinDenoms.ULUNA, 1 * 1e6) });

                var msgs = new object[] { send };

                var gas = await wallet.EstimateGasForTx(msgs, 1 * 1e6);
                var feeEstimate = await wallet.EstimateFeeForTx(new CreateTxOptions()
                {
                    gas = gas,
                    feeDenom = CoinDenoms.ULUNA,
                });

                var txAfterGas = await wallet.CreateTxAndSignTx(
                        feeEstimate,
                        msgs);
                System.Diagnostics.Debug.WriteLine($"JSON: \n {JsonConvert.SerializeObject(txAfterGas)}");
                

                var broadcast = await wallet.broadcastTx.Broadcast(txAfterGas);
                vm.Logs.Add(new Models.Log
                {
                    Created = DateTime.Now,
                    Details = broadcast.Raw_log.ToString(),
                    Message = "RawLog",
                    Type = LogTypes.Broadcast,
                });
                //await DisplayAlert("Result", broadcast.Raw_log.ToString(), "OK");
            });

        }
        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            vm = new MainViewModel();
            collectionView.ItemsSource = vm.Logs;

            WalletsDatabase wdb = new WalletsDatabase();
            var existingLogs = await wdb.GetLogsAsync();

            //foreach (var existingLog in existingLogs)
            //{
            //    vm.Logs.Add(existingLog);
            //}

            vm.Logs.Add(new Models.Log
            {
                Created = DateTime.Now,
                Details = "Application started",
                Message = "Start",
                Type = LogTypes.Information,
            });


            await wdb.SaveLogItemAsync(new Models.Log
            {
                Created = DateTime.Now,
                Details = "Application started",
                Message = "Start",
                Type = LogTypes.Information
            });

        }
    }
}
