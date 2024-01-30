using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Dogecoin
{
	public class UTXO
	{
		public string TxId { get; set; }
		
		public uint vOut { get; set; }

		public long? Amount { get; set; }

		public string? ScriptPubKey { get; set; }
	}
}
