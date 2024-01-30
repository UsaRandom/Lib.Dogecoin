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

			var p2pkhWatcher = new SimpleP2PKHWalletWatcher("address");

			p2pkhWatcher.AddUTXOToWatch(txid, vout);


			spvNode.Start();




			spvNode.Stop();


		}
	}
}
