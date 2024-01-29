

using Lib.Dogecoin;

var ctx = LibDogecoinContext.CreateContext();

var client = ctx.CreateSPVClient();

ctx.SPVConnect(client);

ctx.RunSPVLoop(client);

Console.ReadLine();

