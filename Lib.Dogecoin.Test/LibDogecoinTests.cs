namespace Lib.Dogecoin.Test
{
	[TestClass]
	public class LibDogecoinTests
	{
		[TestMethod]
		public void LibDogecoin_Test()
		{
			using(var ctx = LibDogecoinContext.CreateContext())
			{
				var masterKeys = ctx.GenerateHDMasterPubKeyPair();

				Console.WriteLine($"Master Private: {masterKeys.privateKey}");
				Console.WriteLine($"Master Public:  {masterKeys.publicKey}");


				var derivedPubKey = ctx.GenerateDerivedHDPubKey(masterKeys.privateKey);
				Console.WriteLine($"Derived Public: {derivedPubKey}");



				var byPathRes = ctx.GetDerivedHDAddressByPath(masterKeys.privateKey, "m/0/0/0", false);

				Console.WriteLine($"m/0/0/0 Public Key: {byPathRes}");


				byPathRes = ctx.GetDerivedHDAddressByPath(masterKeys.privateKey, "m/0/0/1", false);

				Console.WriteLine($"m/0/0/1 Public Key: {byPathRes}");


				var mnemonic = ctx.GenerateRandomEnglishMnemonic(LibDogecoinContext.ENTROPY_SIZE_128);

				Console.WriteLine($"Mnemonic: {mnemonic}");


				var qrStr = ctx.P2pkhToQrString(masterKeys.publicKey);

				Console.WriteLine(qrStr);

			}
		}
	}
}