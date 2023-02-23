using Terra.Microsoft.Client;
using Terra.Microsoft.Client.Core;
using Terra.Microsoft.Client.Core.Bank.Msgs;
using Terra.Microsoft.Client.Core.Constants;
using Terra.Microsoft.Client.Key;
using Terra.Microsoft.Rest.Tx.Block;
using TerraSharp.Maui.Example.Data;
using TerraSharp.Maui.Example.Models;
using TerraSharp.Maui.Example.ViewModels;
using TerraSharp.Maui.Example.ViewModels.Helpers;

namespace TerraSharp.Maui.Example.Pages
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        MainViewModel vm;
        WalletsDatabase database;
        public MainPage(WalletsDatabase walletsDatabase)
        {
            InitializeComponent();
            database = walletsDatabase;
            TerraStartup.InitializeKernel(Terra.Microsoft.Rest.Configuration.Environment.TerraEnvironment.LUNA2TestNet);
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
                var mnemonic = new TxMnemonic("notice oak worry limit wrap speak medal online prefer cluster " +
                    "roof addict wrist behave treat actual wasp year salad speed social layer crew genius");

                // Define the recipient address
                var rAddr = "terra17lmam6zguazs5q5u6z5mmx76uj63gldnse2pdp";

                // Define your wallet -- The account that will handle the transactions
                var wallet = TerraServices.CreateWallet(mnemonic);

                var coins = await TerraServices.GetBalances(mnemonic.AccAddress);


                foreach(var coin in coins)
                {
                    vm.Logs.Add(new Models.Log
                    {
                        Created = DateTime.Now,
                        Details = coin.denom.Trim('u').ToUpper(),
                        Message = Convert.ToDecimal(coin.amount/1000000).ToString(),
                        Type = LogTypes.Bank,
                    });
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

                var broadcast = await wallet.broadcastTx.Broadcast(txAfterGas);
                vm.Logs.Add(new Models.Log
                {
                    Created = DateTime.Now,
                    Details = broadcast.Raw_log.ToString(),
                    Message = "RawLog",
                    Type = LogTypes.Transaction,
                });
                //await DisplayAlert("Result", broadcast.Raw_log.ToString(), "OK");
            });

        }
        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            vm = new MainViewModel();
            collectionView.ItemsSource = vm.Logs;

            WalletsDatabase wdb = new WalletsDatabase();
            //var existingLogs = await wdb.GetLogsAsync();

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
