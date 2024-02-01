using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Lib.Dogecoin.Interop;

namespace Lib.Dogecoin
{
    public class SPVNode
	{
		private Thread _thread;
		private IntPtr _spvNodeRef;

		public SPVNode(bool mainNet = true, string headerFile = null, bool useCheckpoints = true)
		{
			IsMainNet = true;
			HeaderFile = headerFile;
			UseCheckpoints = useCheckpoints;
		}

		public string HeaderFile { get; private set; }
		
		public bool IsMainNet { get; private set; }
		
		public uint Blockheight { get; private set; }

		public bool UseCheckpoints { get; private set; }
		
		public DateTime LatestTimestamp { get; private set; }

		public Action<SPVNodeTransaction> OnTransaction { get; set; }

		public void Start()
		{
			if(_thread != null && _thread.IsAlive)
			{
				throw new Exception("SPV Already Running!");
			}

			CreateSPVClient();

			_thread = new Thread(() =>
			{
				if(HeaderFile != null)
				{
					LibDogecoinInterop.dogecoin_spv_client_load(_spvNodeRef, HeaderFile.NullTerminate());
				}
				LibDogecoinInterop.dogecoin_spv_client_discover_peers(_spvNodeRef, null);


				unsafe
				{
					var client = Marshal.PtrToStructure<dogecoin_spv_client>(_spvNodeRef);

					var headerDb = *client.headers_db;

					if (headerDb.has_checkpoint_start(client.headers_db_ctx) == 0)
					{
						var header = "cacda22e5e2c867676bd9d245f3bbbef58dc1349361182f3b790e3accd0c0a85";

						var headerBytes = HexStringToLittleEndianByteArray(header);

						uint height = 50712229;

						headerDb.set_checkpoint_start(client.headers_db_ctx, headerBytes, height);
					}

				}

				LibDogecoinInterop.dogecoin_spv_client_runloop(_spvNodeRef);
			});

			_thread.Start();
		}

		public void Stop()
		{
			var spv = Marshal.PtrToStructure<dogecoin_spv_client>(_spvNodeRef);
			
			LibDogecoinInterop.dogecoin_node_group_shutdown(spv.nodegroup);
		}





		private unsafe void CreateSPVClient()
		{

			var net = IsMainNet ? LibDogecoinContext._mainChain : LibDogecoinContext._testChain;

			_spvNodeRef = LibDogecoinInterop.dogecoin_spv_client_new(net, false, HeaderFile == null, false, true);


			syncTransactionCallback = new dogecoin_spv_client.sync_transaction_delegate(SyncTransaction);

			Marshal.WriteIntPtr(_spvNodeRef,
				Marshal.OffsetOf(typeof(dogecoin_spv_client),
				nameof(dogecoin_spv_client.sync_transaction)).ToInt32(), Marshal.GetFunctionPointerForDelegate(syncTransactionCallback));


		}

		private static dogecoin_spv_client.sync_transaction_delegate syncTransactionCallback;



		private unsafe void SyncTransaction(IntPtr ctx, IntPtr tx, uint pos, IntPtr blockindex)
		{
			var transaction = Marshal.PtrToStructure<dogecoin_tx>(tx);

			byte[] txHashBytes = new byte[32];
			LibDogecoinInterop.dogecoin_tx_hash(tx, txHashBytes);
			var txId = ByteArrayToHexString(txHashBytes.Reverse().ToArray());

			var blockIdx = Marshal.PtrToStructure<dogecoin_blockindex>(blockindex);

			//might want to use datetimeoffset?
			var blockTimestamp = DateTimeOffset.FromUnixTimeSeconds(blockIdx.header.timestamp).DateTime;

			Blockheight = blockIdx.height;
			LatestTimestamp = blockTimestamp;
			

			var nodeTransaction = new SPVNodeTransaction();
			var inList = new List<UTXO>();
			var outList = new List<UTXO>();

			nodeTransaction.TxId = txId;
			nodeTransaction.BlockHeight = blockIdx.height;
			nodeTransaction.Timestamp = blockTimestamp;


			//handle inputs
			var vinList = *transaction.vin;
			for (var i = 0; i < vinList.len; i++)
			{
				dogecoin_tx_in vin = Marshal.PtrToStructure<dogecoin_tx_in>(*(vinList.data));

				inList.Add(new UTXO {
					TxId = ByteArrayToHexString(vin.prevout.hash.Reverse().ToArray()),
					vOut = vin.prevout.n
				});
			}


			//handle outputs
			var voutList = *transaction.vout;

			for (uint i = 0; i < voutList.len; i++)
			{
				dogecoin_tx_out vout = Marshal.PtrToStructure<dogecoin_tx_out>(*(voutList.data + i));
				
				outList.Add(new UTXO
				{
					TxId = txId,
					vOut = i,
					Amount = vout.value,
					ScriptPubKey = Marshal.PtrToStringAnsi((IntPtr)vout.script_pubkey->str)
				});
			}

			nodeTransaction.In = inList.ToArray();
			nodeTransaction.Out = outList.ToArray();

			if(OnTransaction != null)
			{
				OnTransaction(nodeTransaction);
			}
		}

		private static string ByteArrayToHexString(byte[] bytes)
		{
			var hex = new StringBuilder(bytes.Length * 2);
			foreach (byte b in bytes)
			{
				hex.AppendFormat("{0:x2}", b);
			}
			return hex.ToString();
		}

		public static byte[] HexStringToLittleEndianByteArray(string hex)
		{
			if (hex == null)
				return null;
			if (hex.Length % 2 == 1)
				hex = "0" + hex;

			byte[] bytes = new byte[hex.Length / 2];
			for (int i = 0; i < hex.Length; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
			}

			Array.Reverse(bytes);
			return bytes;
		}
	}
}
