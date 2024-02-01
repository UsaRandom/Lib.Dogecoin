

using Lib.Dogecoin;

var node = new SPVNode();

uint currentBlock = 0;

node.OnTransaction = (tx) =>
{
	if(tx.BlockHeight > currentBlock)
	{
		Console.WriteLine($"BlockHeight: {tx.BlockHeight}");
		currentBlock = tx.BlockHeight;
	}

	foreach(var output in tx.Out)
	{
		if (output.ScriptPubKey.GetP2PKHAddress() == "D6hbn1AugHq3WVtTVSv1fZAg6atPAMtwuV")
		{
			Console.WriteLine($"Received {output.Amount} dogecoin!");
			Console.WriteLine($"Block#{node.Blockheight} {tx.TxId}-{output.vOut} @ {tx.Timestamp}");
		}
	}
};

node.Start();

Console.ReadLine();
Console.WriteLine("Stopping SPV");

node.Stop();

Console.WriteLine("SPV Stopped");
Console.ReadLine();


