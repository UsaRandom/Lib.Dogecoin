using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Dogecoin
{

	[StructLayout(LayoutKind.Sequential)]
	public struct dogecoin_spv_client
	{
		public IntPtr nodegroup;
		public ulong last_headersrequest_time;
		public ulong oldest_item_of_interest;
		[MarshalAs(UnmanagedType.U1)]
		public bool use_checkpoints;
		public IntPtr chainparams;
		public int stateflags;
		public ulong last_statecheck_time;
		[MarshalAs(UnmanagedType.U1)]
		public bool called_sync_completed;
		public IntPtr headers_db_ctx;
		public IntPtr headers_db;

		public delegate void header_connected_delegate(dogecoin_spv_client client);
		public delegate void sync_completed_delegate(dogecoin_spv_client client);
		public delegate bool header_message_processed_delegate(dogecoin_spv_client client, IntPtr node, IntPtr newtip);
		public delegate void sync_transaction_delegate(IntPtr ctx, IntPtr tx, uint pos, IntPtr blockindex);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public header_connected_delegate header_connected;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public sync_completed_delegate sync_completed;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public header_message_processed_delegate header_message_processed;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public sync_transaction_delegate sync_transaction;
		public IntPtr sync_transaction_ctx;
	}


	[StructLayout(LayoutKind.Sequential)]
	public struct dogecoin_tx_outpoint
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public byte[] hash;
		public uint n;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct dogecoin_tx_in
	{
		public dogecoin_tx_outpoint* prevout;
		public cstring* script_sig;
		public uint sequence;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct dogecoin_tx_out
	{
		public long value;
		public cstring* script_pubkey;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct dogecoin_tx
	{
		public int version;
		public vector* vin;
		public vector* vout;
		public uint locktime;
	}


	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct vector
	{
		public IntPtr* data;  // array of pointers
	
		public uint len;  // array element count
		public uint alloc; // allocated array elements

		public void* elem_free_f;
	}




	[StructLayout(LayoutKind.Sequential)]
	public struct cstring
	{
		public IntPtr str;    // string data, incl. NUL
		public uint len;   // length of string, not including NUL
		public uint alloc; // total allocated buffer length
	}


	public struct dogecoin_script_op
	{
		public opcodetype Op;  /* opcode found */
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
		public byte[] Data; /* associated data, if any */
		public UIntPtr DataLength;
	}

}
