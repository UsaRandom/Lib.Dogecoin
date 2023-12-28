

# Lib.Dogecoin

A simple .NET wrapper for libdogecoin

See: https://github.com/dogecoinfoundation/libdogecoin


### Usage

#### Generate Key/Address (quick and simple):
```csharp
using Lib.Dogecoin;

using (var ctx = LibDogecoinContext.CreateContext())
{
	//Generate a new WIF private key & address
	var keys = ctx.GeneratePrivPubKeyPair();

	Console.WriteLine($"Our new Dogecoin Address: {keys.publicKey}");
	Console.WriteLine($"Our new Private Key (keep secret): {keys.privateKey}");
}

```


#### Generate Key/Address using 24 Word Mnemonic (backup phrases):
```csharp
using Lib.Dogecoin;

using (var ctx = LibDogecoinContext.CreateContext())
{
	//Generate a 24 word english mnemonic 'backup phrases'
	var mnemonic = ctx.GenerateRandomEnglishMnemonic(LibDogecoinContext.ENTROPY_SIZE_256);

	var masterKeys = ctx.GenerateHDMasterPubKeypairFromMnemonic(mnemonic);

	//verify master keys before using them to make our derived private keys/addresses that are used for transactions.
	if (ctx.VerifyHDMasterPubKeyPair(masterKeys.privateKey, masterKeys.publicKey))
	{
		//see: bip44
		var hdpath = "m/44'/3'/0'/0/0";

		//address we can use to receive dogecoin, and private key in wallet import format to spend it.
		var address = ctx.GetDerivedHDAddressByPath(masterKeys.privateKey, hdpath, false);
		var privateKeyWIF = ctx.GetHDNodePrivateKeyWIFByPath(masterKeys.publicKey, hdpath, true);
	}
}
```

#### Build/Sign Transaction 
```csharp

using (var ctx = LibDogecoinContext.CreateContext())
{
	//use 12/24 word mnemonic
	var mnemonic = "wow wow wow wow wow wow wow wow...";

	var masterKeys = ctx.GenerateHDMasterPubKeypairFromMnemonic(mnemonic);

	//verify master keys before using them to make our derived private keys/addresses that are used for transactions.
	if (ctx.VerifyHDMasterPubKeyPair(masterKeys.privateKey, masterKeys.publicKey))
	{
		//see: bip44
		var hdpath = "m/44'/3'/0'/0/0";

		//address we can use to receive dogecoin, and private key in wallet import format to spend it.
		var address = ctx.GetDerivedHDAddressByPath(masterKeys.privateKey, hdpath, false);
		var privateKeyWIF = ctx.GetHDNodePrivateKeyWIFByPath(masterKeys.publicKey, hdpath, true);


		/*

		In this example, we have a UTXO worth 1.0 dogecoin and we want to send someone 0.5 dogecoin.

		*/


		var workingTransactionId = ctx.StartTransaction();

		//add input UTXOs, transaction id, the vout index
		ctx.AddUTXO(workingTransactionId, "TransactionIdOfUTXO", 0); // our 1.0 worth of dogecoin


		//The network fee is the difference between the value of the sum of the inputs and the sum of the outputs.
		//so in this case, we use 1.0 dogecoin, send someone 0.5, and send ourselves 0.48. 
		//The network fee paid is 0.02 dogecoin. 
		ctx.AddOutput(workingTransactionId, "DestinationAddress", "0.5");
		ctx.AddOutput(workingTransactionId, address, "0.48");


		//Each input utxo has to be signed, you will have to call this multiple times if you have multiple inputs.
		if (ctx.SignTransactionWithPrivateKey(workingTransactionId, 0, privateKeyWIF))
		{
			//this is what can be sent to the network.
			var rawSignedTransactionReadyToBroadcast = ctx.GetRawTransaction(workingTransactionId);

			//if you are building a wallet, it's a good idea to store the UTXO you created,
			//
			//our new utxo with 0.48 dogecoin to spend looks like this:
			var newTransactionId = ctx.GetTransactionIdFromRaw(rawSignedTransactionReadyToBroadcast);
			var newVOut = 1; //it's the second output 

		}

	}
}

```
