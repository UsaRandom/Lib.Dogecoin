using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Dogecoin.Interop
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
        public delegate bool header_message_processed_delegate(dogecoin_spv_client client, IntPtr node, dogecoin_block_header newtip);
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
	public unsafe struct dogecoin_node_group
	{
		public IntPtr ctx; // flexible context useful in conjunction with the callbacks
		public IntPtr event_base;
		public vector* nodes; // the group's nodes
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
		public char[] clientstr;
		public int desired_amount_connected_nodes;
		public IntPtr chainparams;

		// callbacks
		public delegate int LogWriteCallback(string format, params object[] args);
		public LogWriteCallback log_write_cb;
		public delegate bool ParseCmdCallback(IntPtr node, IntPtr hdr, IntPtr buf);
		public ParseCmdCallback parse_cmd_cb;
		public delegate void PostCmdCallback(IntPtr node, IntPtr hdr, IntPtr buf);
		public PostCmdCallback postcmd_cb;
		public delegate void NodeConnectionStateChangedCallback(IntPtr node);
		public NodeConnectionStateChangedCallback node_connection_state_changed_cb;
		public delegate bool ShouldConnectToMoreNodesCallback(IntPtr node);
		public ShouldConnectToMoreNodesCallback should_connect_to_more_nodes_cb;
		public delegate void HandshakeDoneCallback(IntPtr node);
		public HandshakeDoneCallback handshake_done_cb;
		public delegate bool PeriodicTimerCallback(IntPtr node, ref ulong time); // return false will cancel the internal logic
		public PeriodicTimerCallback periodic_timer_cb;
	}


	[StructLayout(LayoutKind.Sequential)]
    public struct dogecoin_tx_outpoint
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] hash;
        public uint n;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct dogecoin_tx_in
    {
        public dogecoin_tx_outpoint prevout;
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
    public unsafe struct cstring
    {
        public char* str;    // string data, incl. NUL
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

	[StructLayout(LayoutKind.Sequential)]
	public struct auxpow
	{
		public byte _is;
        public IntPtr check;
		public IntPtr ctx;
	}

	[StructLayout(LayoutKind.Sequential, Size = 104)]
    public struct dogecoin_block_header
    {
        public int version;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] prev_block;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] merkle_root;
        public uint timestamp;
        public uint bits;
        public uint nonce;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
		public auxpow[] auxpow;
	}

    [StructLayout(LayoutKind.Sequential)]
    public struct dogecoin_blockindex
    {
        public uint height;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] hash; //32 byte array
		public dogecoin_block_header header; //size 104 bytes, alignment 8 bytes
        public IntPtr prev;
    }


}
