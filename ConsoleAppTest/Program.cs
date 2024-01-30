

using Lib.Dogecoin;


var node = new SPVNode();

node.OnTransaction = (tx) =>
{
	Console.WriteLine($"Block#{node.Blockheight} {tx.TxId} @ {tx.Timestamp}");

	foreach(var inUTXO in tx.Out)
	{
		if(inUTXO.ScriptPubKey.GetP2PKHAddress() == "D6hbn1AugHq3WVtTVSv1fZAg6atPAMtwuV")
		{
			Console.WriteLine("FOUND ME");
		}
	}
};


node.Start();

Console.ReadLine();
Console.WriteLine("Stopping SPV");

node.Stop();

Console.WriteLine("SPV Stopped");
Console.ReadLine();