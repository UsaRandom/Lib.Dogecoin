using Lib.Dogecoin;
internal class Program
{
	private static void Main(string[] args)
	{
		using(var ctx = LibDogecoinContext.CreateContext())
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

			Assert.IsTrue(ctx.AddUTXO(transactionId, utxoTx, 1));
			Assert.IsTrue(ctx.AddOutput(transactionId, dest, ctx.KoinuToCoinString(13015210527)));

			ctx.FinalizeTransaction(transactionId, dest, "0.10000000", "130.25210527", keys.publicKey);

			ctx.SignTransaction(transactionId, ctx.AddressToPubKeyHash(dest), keys.privateKey);

			//	Assert.IsTrue(ctx.SignTransaction(transactionId, ctx.AddressToPubKeyHash(dest), keys.privateKey));
			//	Assert.IsTrue(ctx.SignTransactionWithPrivateKey(transactionId, 0, keys.privateKey));

			var tx = ctx.GetRawTransaction(transactionId);

			Console.WriteLine(tx);
		}
	}

	public class Assert
	{
		public static void IsTrue(bool value) {
			Console.WriteLine(value);
		}
	}
}