

# Lib.Dogecoin

A simple .NET wrapper for libdogecoin

See: https://github.com/dogecoinfoundation/libdogecoin


## Usage

csharp`

	using(var ctx = LibDogecoinContext.CreateContext())
	{
		var masterKeys = ctx.GenerateHDMasterPubKeyPair();

		Console.WriteLine($"Master Private: {masterKeys.privateKey}");
		Console.WriteLine($"Master Public:  {masterKeys.publicKey}");
	}

`