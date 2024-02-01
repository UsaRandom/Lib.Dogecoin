

using Lib.Dogecoin;

var node = new SPVNodeBuilder()
				.UseCheckpointFile("spv_checkpoint")
				.StartAt("cacda22e5e2c867676bd9d245f3bbbef58dc1349361182f3b790e3accd0c0a85", 50712229)
				.OnNextBlock((previous, next) =>
				{
					Console.WriteLine($"{next.BlockHeight} @ {next.Timestamp}: {next.Hash}");
				})
				.OnTransaction((tx) =>
				{
					foreach (var output in tx.Out)
					{
						if (output.ScriptPubKey.GetP2PKHAddress() == "D6hbn1AugHq3WVtTVSv1fZAg6atPAMtwuV")
						{
							Console.WriteLine($"Received Ð{output.Amount:#.000}!");
						}
					}
				})
				.UseMainNet()
				.Build();

node.Start();

Console.ReadLine();
Console.WriteLine("SPVNode stop requested");

node.Stop();

