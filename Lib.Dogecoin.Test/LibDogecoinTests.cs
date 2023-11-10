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
					privateKey: ctx.GetDerivedHDAddressByPath(keys.privateKey, "m/0/0/4", true),
					publicKey: ctx.GetDerivedHDAddressByPath(keys.privateKey, "m/0/0/4", false)
			);

		}
	}
}