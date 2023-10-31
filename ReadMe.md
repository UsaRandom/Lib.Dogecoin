

# Lib.Dogecoin

A simple .NET wrapper for libdogecoin

See: https://github.com/dogecoinfoundation/libdogecoin


## Usage

```csharp

	using(var ctx = LibDogecoinContext.CreateContext())
	{
		var masterKeys = ctx.GenerateHDMasterPubKeyPair();
		Console.WriteLine($"Master Private: {masterKeys.privateKey}");
		Console.WriteLine($"Master Public:  {masterKeys.publicKey}");

		var pubKeyByPath = ctx.GetDerivedHDAddressByPath(masterKeys.privateKey, "m/0/0/1", false);
		Console.WriteLine($"m/0/0/1 Public Key: {pubKeyByPath}");

		var mnemonic = ctx.GenerateRandomEnglishMnemonic(LibDogecoinContext.ENTROPY_SIZE_128);
		Console.WriteLine($"Mnemonic: {mnemonic}");

		var qrStr = ctx.P2pkhToQrString(masterKeys.publicKey);
		Console.WriteLine(qrStr);
	}

```
