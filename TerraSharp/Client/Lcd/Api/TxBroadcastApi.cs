﻿using Terra.Microsoft.Rest.Tx.Transaction;
using Terra.Microsoft.Rest.Configuration.Environment;
using Terra.Microsoft.Rest.Util;
using PROTO = Terra.Microsoft.ProtoBufs.third_party.proto.cosmos.tx.v1beta1;
using Terra.Microsoft.Rest.Converters;
using Terra.Microsoft.Rest.Tx.Transaction.Upload;
using Terra.Microsoft.Rest.Tx.Block;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Terra.Microsoft.Extensions.StringExt;
using Terra.Microsoft.Extensions.ProtoBufs;
using Terra.Microsoft.Client.Core;
using Terra.Microsoft.Client.Client.Lcd.Constants;
using Terra.Microsoft.Client.Core.Constants;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Terra.Microsoft.Client.Client.Lcd.Api
{
    public class TxBroadcastApi : BaseAPI
    {
        private readonly TreasuryAPI treasury;
        public TxBroadcastApi(
          TerraRestfulService apiRequester,
          TreasuryAPI treasury) : base(apiRequester)
        {
            this.treasury = treasury;
        }

        private string Encode(PROTO.Tx tx)
        {
            return TerraStringExtensions.GetBase64FromBytes(ProtoExtensions.SerialiseFromData(tx));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signers"></param>
        /// <param name="options"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public async Task<Fee> EstimatedFeeWithBurnTax(CreateTxOptions options)
        {
            var feeDenom = options.feeDenom ?? CoinDenoms.UUSD;

            var burnTax = await this.treasury.GetTaxRate();
            var gasprices = await this.treasury.GetGasPriceForDenom(options.feeDenom);

            var estimatedGas = options.gas.Value;
            var taxWithBurnTax = estimatedGas * burnTax;
            var taxTotal = (estimatedGas + taxWithBurnTax) * gasprices;

            return new Fee((int)estimatedGas, new List<Coin>() { new Coin(feeDenom, (int)taxTotal) }, "", options.FeeGranter);
        }

        public async Task<Fee> EstimatedFeeWithoutBurnTax(CreateTxOptions options)
        {
            var feeDenom = options.feeDenom ?? CoinDenoms.UUSD;

            var gasprices = await this.treasury.GetGasPriceForDenom(options.feeDenom);

            var estimatedGas = options.gas.Value;
            var taxTotal = estimatedGas * gasprices;

            return new Fee((int)estimatedGas, new List<Coin>() { new Coin(feeDenom, (int)taxTotal) }, "", options.FeeGranter);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="tx"></param>
        ///// <param name="options"></param>
        ///// <returns></returns>
        ///// <exception cref="InvalidOperationException"></exception>
        public async Task<double> EstimateGas(Core.Tx simTx, double? gasAdjustment, object[] messages)
        {
            var gasAdjs = gasAdjustment ?? TerraClientConfiguration.LCDConfig.GasAdjustment.Value;

            string rootPath = string.Concat(TerraClientConfiguration.BlockchainResourcePath, CosmosBaseConstants.COSMOS_TX_ESTIMATE_GAS_USAGE);

            var dataEncoded = simTx.ToProtoWithType(messages);
            string data = this.Encode(dataEncoded);
            var response = await this.apiRequester.PostAsync(rootPath,
                new TxUploadContainerJSON()
                {
                    tx_bytes = data,
                });
            if (response.Successful)
            {
                return gasAdjs * double.Parse(response.Result.gas_info.gas_used);
            }

            throw new InvalidOperationException("cannot append signature");
        }

        public async Task<Core.Tx> CreateTx(
            Fee fee,
            double accNumber,
            string memo = "")
        {
            return new Tx(
                 new TxBody(null, memo, 0),
                 new AuthInfo(new List<SignerInfo>() { }, fee),
                 new List<string>(), accNumber);
        }

        private async Task<TxResponse> BroadcastShared(PROTO.Tx tx, PROTO.BroadcastMode mode)
        {
            string rootPath = string.Concat(
                TerraClientConfiguration.BlockchainResourcePath,
                CosmosBaseConstants.COSMOS_TX_TXS);


            Debug.WriteLine($"PROTO TX: {JsonConvert.SerializeObject(tx)}");
            var data = this.Encode(tx);

            var container = new TxContainerJSON()
            {
                mode = BroadcastModeConverter.GetFromEnum(mode),
                tx_bytes = data,
                tx = tx,
                sequences = new string[] { $"{tx.AuthInfo.SignerInfos[0].Sequence}" },
                fee_granter = tx.AuthInfo.Fee.Granter
            };

            Debug.WriteLine($"TX: {JsonConvert.SerializeObject(container)}");
            var response = await this.apiRequester.PostAsync(rootPath, container);

            if (response.Successful)
            {
                return response.Result.tx_response;
            }

            throw new ArgumentNullException($"");
        }

        public async Task<BlockTxBroadcastResultDataArgs> Broadcast(PROTO.Tx tx)
        {
            return BlockTxBroadcastResultDataArgs.FromJSON(await this.BroadcastShared(tx, PROTO.BroadcastMode.BroadcastModeBlock));
        }
        public async Task<BlockTxBroadcastResultDataArgs> BroadcastSync(PROTO.Tx tx)
        {
            return BlockTxBroadcastResultDataArgs.FromJSON(await this.BroadcastShared(tx, PROTO.BroadcastMode.BroadcastModeSync));
        }
        public async Task<BlockTxBroadcastResultDataArgs> BroadcastAsync(PROTO.Tx tx)
        {
            return BlockTxBroadcastResultDataArgs.FromJSON(await this.BroadcastShared(tx, PROTO.BroadcastMode.BroadcastModeAsync));
        }
    }
}
