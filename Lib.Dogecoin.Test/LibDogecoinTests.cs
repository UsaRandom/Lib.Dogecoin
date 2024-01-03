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
		public void GenerateAddressAndPrivateKeyForSimpleSendingAndSigningTest()
		{

			//Generate a 24 word english mnemonic
			var mnemonic = ctx.GenerateRandomEnglishMnemonic(LibDogecoinContext.ENTROPY_SIZE_256);
			
			var masterKeys = ctx.GenerateHDMasterPubKeypairFromMnemonic(mnemonic);


			//verify master keys before using them to make our derived private key/address 
			if(ctx.VerifyHDMasterPubKeyPair(masterKeys.privateKey, masterKeys.publicKey))
			{
				//see: bip44
				var hdpath = "m/44'/3'/0'/0/0";

				//address we can use to receive dogecoin, and private key to spend it.
				var address = ctx.GetDerivedHDAddressByPath(masterKeys.privateKey, hdpath, false);
				var privateKeyWIF = ctx.GetHDNodePrivateKeyWIFByPath(masterKeys.publicKey, hdpath, true);
			}


		}

		[TestMethod]
		public void TestTPM()
		{
			var mnemonic = ctx.GenerateMnemonicEncryptWithTPM(5, false);

			var decryped = ctx.DecryptMnemonicWithTPM(5);


			Assert.AreEqual(mnemonic, decryped);
		}

	}
}