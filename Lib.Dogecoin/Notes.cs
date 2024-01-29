using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Dogecoin
{
	internal class Notes
	{

		public void Test()
		{


			var spvNode = new SPVNode();

			var p2pkhWatcher = new SimpleP2PKHAddressWatcher();
			var utxoWatcher = new SimpleUTXOWatcher();

			p2pkhWatcher.AddAddress("address");

			foreach (var utxo in imaginaryWallet.GetUTXOs("address"))
			{
				utxoWatcher.AddUTXO(utxo.txid, utxo.vout);
			}


			spvNode.AddWatcher(p2pkhWatcher);
			spvNode.AddWatcher(utxoWatcher);


			spvNode.Start();




			spvNode.Stop();


		}
	}
}
