using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;

namespace Lib.Dogecoin.Test
{
	[TestClass]
	public class LibDogecoinTests
	{
		private LibDogecoinContext? ctx;

		[TestInitialize]
		public void BeforeTests()
		{
			ctx = LibDogecoinContext.CreateContext();
		}

		[TestCleanup]
		public void AfterTests()
		{
			ctx.Dispose();
		}

		[TestMethod]
		public void GeneratePrivPubKeyPair_Test()
		{
			var keys = ctx.GeneratePrivPubKeyPair(true);

			Assert.IsNotNull(keys);
			Assert.IsTrue(ctx.VerifyPrivPubKeyPair(keys.privateKey, keys.publicKey, true));
		}


		[TestMethod]
		public void GenerateHDMasterPubKeyPair_Test()
		{
			var keys = ctx.GenerateHDMasterPubKeyPair(true);

			Assert.IsNotNull(keys);
			Assert.IsTrue(ctx.VerifyHDMasterPubKeyPair(keys.privateKey, keys.publicKey, true));
		}

		[TestMethod]
		public void GetDerivedHDAddressByPath_Test()
		{
			var keys = ctx.GenerateHDMasterPubKeyPair(true);

			Assert.IsNotNull(keys);
			Assert.IsTrue(ctx.VerifyHDMasterPubKeyPair(keys.privateKey, keys.publicKey, true));


			var derivedKeys = (
				privateKey: ctx.GetDerivedHDAddressByPath(keys.privateKey, "m/0/0/0/4", true),
				publicKey: ctx.GetDerivedHDAddressByPath(keys.privateKey, "m/0/0/0/4", false)
			);

			Assert.IsNotNull(derivedKeys.publicKey);
			Assert.IsNotNull(derivedKeys.privateKey);
		}


		[TestMethod]
		public void SignMessage_Test()
		{
			var keys = ctx.GeneratePrivPubKeyPair(true);

			var msg = "Hello World!";
			var sig = ctx.SignMessage(keys.privateKey, msg);

			Assert.AreNotEqual(string.Empty, sig);
			
			var verified = ctx.VerifyMessage(sig, msg, keys.publicKey);

			Assert.IsTrue(verified);
		}


		[TestMethod]
		public void GenerateRandomEnglishMnemonic_Test()
		{
			//256 bits of entropy, 24 words
			var mnemonic = ctx.GenerateRandomEnglishMnemonic(LibDogecoinContext.ENTROPY_SIZE_256);

			Assert.AreEqual(mnemonic.Split(' ').Length, 24);
			
			//128 bits of entropy, 12 words
			mnemonic = ctx.GenerateRandomEnglishMnemonic(LibDogecoinContext.ENTROPY_SIZE_128);

			Assert.AreEqual(mnemonic.Split(' ').Length, 12);
		}


		[TestMethod]
		public void StringToQrPng_Test()
		{
			var keys = ctx.GeneratePrivPubKeyPair();

			var file = Path.Combine(Directory.GetCurrentDirectory(), "testqr.png");

			if(File.Exists(file))
			{
				File.Delete(file);
			}

			var result = ctx.StringToQrPng(keys.publicKey, file, 25);

			Assert.IsTrue(result);
			Assert.IsTrue(File.Exists(file));

			if (File.Exists(file))
			{
				File.Delete(file);
			}
		}

		[TestMethod]
		public void GetRawTransaction_Test()
		{
			int index = ctx.StartTransaction();

			var rawhex = ctx.GetRawTransaction(index);

			Console.WriteLine($"The transaction hex at index {index} is {rawhex}.");

			ctx.ClearTransaction(index);
		}


		[TestMethod]
		public void CreateNewKey()
		{

			var mnemonic = ctx.GenerateRandomEnglishMnemonic(LibDogecoinContext.ENTROPY_SIZE_256);
			Console.WriteLine(mnemonic);
			
			var keys = ctx.GenerateHDMasterPubKeypairFromMnemonic(mnemonic);

			Console.WriteLine(keys.privateKey);
			Console.WriteLine(keys.publicKey);


		}

			[TestMethod]
		public void Transaction_Test()
		{

			ctx.RemoveAllTransactions();

			string rpcUrl = "http://localhost:22555"; // Replace with your Dogecoin node's RPC URL
				string username = "...";
				string password = "...";

				var rpcClient = new DogecoinRpcClient(rpcUrl, username, password);


			var keys = (privateKey: "...",
						publicKey: "...");

			Console.WriteLine(keys.privateKey);
			Console.WriteLine(keys.publicKey);

			Console.WriteLine(rpcClient.AddAddressToWatch(keys.publicKey, "slot1"));

			var unspentRaw = rpcClient.ListUnspent(keys.publicKey);

			var unspentJson = JsonObject.Parse(unspentRaw);
			Console.WriteLine(unspentJson.ToString());

			var utxoJsonArray = unspentJson["result"].AsArray();


			var utxoString = new StringBuilder();
			foreach (var utxoJson in utxoJsonArray)
			{
				utxoString.Append($"{utxoJson["txid"].ToString()}|");
				utxoString.Append($"{utxoJson["vout"].ToString()}|");
				utxoString.Append($"{utxoJson["scriptPubKey"].ToString()}|");
				utxoString.AppendLine($"{utxoJson["amount"].ToString()}");
			}


			var resultString = utxoString.ToString();


			var utxos = resultString.Split('\n');

			var id = ctx.StartTransaction();


			foreach (var utxo in utxos)
			{
				if(string.IsNullOrEmpty(utxo))
				{
					continue;
				}
				var parts = utxo.Split('|');

				var txId = parts[0];
				var vout = Int32.Parse(parts[1]);
				var scriptPubKey = parts[2];
				var amount = parts[3];


				ctx.AddUTXO(id, txId, vout);
			}
			ctx.AddOutput(id, "...", "1.925");
			

			if(ctx.SignTransactionWithPrivateKey(id, 0, keys.privateKey) &&
				ctx.SignTransactionWithPrivateKey(id, 1, keys.privateKey))
			{
				
				var rawTransaction = ctx.GetRawTransaction(id);
				Console.WriteLine(rawTransaction);
				Console.WriteLine(rpcClient.BroadcastRawTransaction(rawTransaction));
			}

			ctx.RemoveAllTransactions();


		}
	}
}