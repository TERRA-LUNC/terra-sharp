
## Preview Maui (Windows)

NET Multi-platform App UI (.NET MAUI) is a cross-platform framework for creating native mobile and desktop apps with C# and XAML.

Using .NET MAUI, you can develop apps that can run on Android, iOS, macOS, and Windows from a single shared code-base.

![alt text](https://github.com/TERRA-LUNC/terra-sharp/blob/dev/TerraSharp.Maui.Example/Resources/Images/maui.png)


.NET MAUI unifies Android, iOS, macOS, and Windows APIs into a single API that allows a write-once run-anywhere developer experience, while additionally providing deep access to every aspect of each native platform.

.NET 6 or greater provides a series of platform-specific frameworks for creating apps: .NET for Android, .NET for iOS, .NET for macOS, and Windows UI 3 (WinUI 3) library. These frameworks all have access to the same .NET Base Class Library (BCL). This library abstracts the details of the underlying platform away from your code. The BCL depends on the .NET runtime to provide the execution environment for your code. For Android, iOS, and macOS, the environment is implemented by Mono, an implementation of the .NET runtime. On Windows, .NET CoreCLR provides the execution environment.



Terra.Microsoft.Client is a C# SDK for writing applications that interact with the Terra blockchain from either the Web or Mobile, or .net environments and provides simple abstractions over core data structures, serialization, key management, and API request generation.

## Features

- **Written in C#**, with type definitions
- Versatile support for [key management](https://docs.terra.money/develop/feather-js/keys) solutions
- Works with Xamarin, MAUI, in the browser, and Mobile
- Exposes the Terra API through [`LCDClient`](https://docs.terra.money/develop/terra-py/client/lcd/lcdclient)
- Parses responses into native C# types

We highly suggest using Terra.Microsoft.Client in a code editor that has support for type declarations, so you can take advantage of the helpful type hints that are included with the package.

## Installation & Configuration

Grab the latest version off [Nuget](https://www.nuget.org/packages/Terra.Microsoft.Client/1.0.1)

```sh
dotnet add package Terra.Microsoft.Client
```

Please make sure to add the following nuget Packages into your .csproj file
```sh
<ItemGroup>
      <PackageReference Include="Cryptography.ECDSA.Secp256k1" Version="1.1.3" />
      <PackageReference Include="Microsoft.Extensions.Hosting" Version="6.0.1" />
      <PackageReference Include="Microsoft.Extensions.Http.Polly" Version="6.0.9" />
      <PackageReference Include="modernhttpclient-updated" Version="3.4.3" />
      <PackageReference Include="NBitcoin" Version="7.0.23" />
      <PackageReference Include="Newtonsoft.Json" Version="13.0.1" />
      <PackageReference Include="Ninject" Version="3.3.6" />
      <PackageReference Include="Polly" Version="7.2.3" />
      <PackageReference Include="Polly.Extensions.Http" Version="3.0.0" />
      <PackageReference Include="RandomStringCreator" Version="2.0.0" />
      <PackageReference Include="System.Security.Cryptography.Algorithms" Version="4.3.1" />
      <PackageReference Include="protobuf-net" Version="3.1.22" />
      <PackageReference Include="Protobuf.Grpc" Version="1.0.170" />
      <PackageReference Include="Terra.Microsoft.Extensions" Version="1.0.1" />
      <PackageReference Include="Terra.Microsoft.ProtoBufs" Version="1.0.1" />
      <PackageReference Include="Terra.Microsoft.Rest" Version="1.0.0" />
</ItemGroup>
```

Inside your Startup Class (Where you initialize your application), please call the following method, and configure your environment
```cs
// Here we're targeting the Classic Blockchain
TerraStartup.InitializeKernel(TerraEnvironment.Classic);
```
That's it! Now you're ready to start communicating with the blockchain! 

## Usage

Terra.Microsoft.Client can be used for Mobile & Web Developers. Supports all Microsoft Technologies from Xamarin, MAUI, ASP & Unity.

### Getting Blockchain data
:exclamation: Terra.Microsoft.Client can connect to both the terra-classic (LUNC/USTC) and LUNA2 networks. If you want to communicate with the classic chain, you have to set your Enviornment on **TerraStartup.InitializeKernel** to **TerraEnvironment.Classic**.

Below we're going to pull balance information on a sample wallet.
```cs
async void FetchBalanceInformation() {
    //fetch the LCDClient from the Kernel
    var lcd = TerraStartup.GetLCDClient();
    
    // get the current balance of "terra1x46rqay4d3cssq8gxxvqz8xt6nwlz4td20k38v"
    var balance = await lcd.bank.GetBalance("terra1x46rqay4d3cssq8gxxvqz8xt6nwlz4td20k38v");
    Console.WriteLine(balance);
}
```

### Broadcasting transactions

First, [get](https://faucet.terra.money/) some testnet tokens for "terra17lmam6zguazs5q5u6z5mmx76uj63gldnse2pdp".
Here we are going to send some tokens to a recipient address.

```cs
async void BroadcastTransaction() {
    // Create a key out of a mnemonic
    var mnemonic = new MnemonicKey("notice oak worry limit wrap speak medal online prefer cluster roof addict wrist behave treat actual wasp year salad speed social layer crew genius");

    // Define the recipient address
    var rAddr = "terra1x46rqay4d3cssq8gxxvqz8xt6nwlz4td20k38v";

    // Define your wallet -- The account that will handle the transactions
    var wallet = TerraStartup.GetLCDClient().CreateWallet("terra17lmam6zguazs5q5u6z5mmx76uj63gldnse2pdp", mnemonic);

    // Define your message to broadcast
    var send = new MsgSend(
       wallet.accAddress,
       rAddr,
       new List<Core.Coin>() { new Core.Coin(CoinDenoms.ULUNA, 20) });

    var msgs = new object[] { send };

    // Calculate the estimated Gas required for the transaction to succeed (it auto accounts for the new burn tax)
    var gas = await wallet.EstimateGasForTx(100000, msgs);
    
    // Calculate the fees required to handle the transaction
    var feeEstimate = await wallet.EstimateFeeForTx(100000, new CreateTxOptions()
    {
       gas = gas,
       feeDenom = CoinDenoms.ULUNA,
       gasAdjustment = 3,
    });
 
    // Broadcast the transaction 
    var broadcast = await wallet.broadcastTx.Broadcast(await wallet.CreateTxAndSignTx(feeEstimate, msgs));
    System.Diagnostics.Debug.WriteLine("Uploaded Tx Hash" + broadcast.Txhash);
}
```



## License

This software is licensed under the MIT license. See [LICENSE](./LICENSE) for full disclosure.

© 2022 TerraMystics & TERRA-LUNC.
