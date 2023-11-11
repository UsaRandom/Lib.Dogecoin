using System.Security.Cryptography.X509Certificates;

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
		public void Transaction_Test()
		{
			var keys = (
				privateKey: "ckatr3VKCwMuSHtmNFhPjeLUkH2CuNksifYfJFvndrNzPTqmP6G1",
				publicKey: "nmc5oMsFroWXLUCejJUWRzHqxeySXqf6UL"
			);

			//paying them back
			var utxoTx = "9ceca587d3cd24acdd07e42e9d3b269abb7fe5051aba5d81ef4c51ca1d88eee4";
			var dest = "nokbHsGdAAMFVAhHDN9M8VNCafip7mvVB5";

			ctx.RemoveAllTransactions();

			var transactionId = ctx.StartTransaction();

			Assert.IsTrue(ctx.AddUTXO(transactionId, utxoTx, 0));
			Assert.IsTrue(ctx.AddOutput(transactionId, dest, ctx.KoinuToCoinString(13015210527)));
			Assert.IsTrue(ctx.SignTransactionWithPrivateKey(transactionId, 0, keys.privateKey));
		}
	}
}