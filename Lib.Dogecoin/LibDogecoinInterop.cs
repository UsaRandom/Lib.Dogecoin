﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Dogecoin
{
	internal static class LibDogecoinInterop
	{
		private const string DLL_NAME = "dogecoin";


		//!init static ecc context
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void dogecoin_ecc_start();

		//!destroys the static ecc context
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void dogecoin_ecc_stop();

		/* generates a private and public keypair (a wallet import format private key and a p2pkh ready-to-use corresponding dogecoin address)*/
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int generatePrivPubKeypair(
			[Out, MarshalAs(UnmanagedType.LPArray)] char[] wif_privkey,
			[Out, MarshalAs(UnmanagedType.LPArray)] char[] p2pkh_pubkey,
			[MarshalAs(UnmanagedType.I1)] bool is_testnet
		);

		/* generates a hybrid deterministic WIF master key and p2pkh ready-to-use corresponding dogecoin address. */
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int generateHDMasterPubKeypair(
			[Out, MarshalAs(UnmanagedType.LPArray)] char[] wif_privkey_master,
			[Out, MarshalAs(UnmanagedType.LPArray)] char[] p2pkh_pubkey_master,
			[MarshalAs(UnmanagedType.I1)] bool is_testnet
		);

		/* generates a new dogecoin address from a HD master key */
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int generateDerivedHDPubkey(
			[MarshalAs(UnmanagedType.LPArray)] char[] wif_privkey_master,
			[Out, MarshalAs(UnmanagedType.LPArray)] char[] p2pkh_pubkey
		);

		/* get derived hd address by custom path */
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int getDerivedHDAddressByPath(
			[MarshalAs(UnmanagedType.LPArray)] char[] masterkey,
			[MarshalAs(UnmanagedType.LPArray)] char[] derived_path,
			[Out, MarshalAs(UnmanagedType.LPArray)] char[] outaddress,
			[MarshalAs(UnmanagedType.I1)]bool outprivkey
		);


		/* verify that a private key and dogecoin address match */
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int verifyPrivPubKeypair(
			[MarshalAs(UnmanagedType.LPArray)] char[] wif_privkey,
			[MarshalAs(UnmanagedType.LPArray)] char[] p2pkh_pubkey,
			[MarshalAs(UnmanagedType.I1)] bool is_testnet
		);


		/* verify that a HD Master key and a dogecoin address matches */
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int verifyHDMasterPubKeypair(
			[MarshalAs(UnmanagedType.LPArray)] char[] wif_privkey_master,
			[MarshalAs(UnmanagedType.LPArray)] char[] p2pkh_pubkey_master,
			[MarshalAs(UnmanagedType.I1)] bool is_testnet
		);

		/* verify that a HD Master key and a dogecoin address matches */
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int verifyP2pkhAddress(
			[MarshalAs(UnmanagedType.LPArray)] char[] p2pkh_pubkey,
			uint len
		);

		/* Generates an English mnemonic phrase from given hex entropy */
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int generateEnglishMnemonic(
			[MarshalAs(UnmanagedType.LPArray)] char[] entropy,
			[MarshalAs(UnmanagedType.LPArray)] char[] size,
			[Out, MarshalAs(UnmanagedType.LPArray)] char[] mnemonic
		);

		/* Generates a random (e.g. "128" or "256") English mnemonic phrase */
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int generateRandomEnglishMnemonic(
			[MarshalAs(UnmanagedType.LPArray)] char[] size,
			[Out, MarshalAs(UnmanagedType.LPArray)] char[] mnemonic
		);


		/* Generates a HD master key and p2pkh ready-to-use corresponding dogecoin address from a mnemonic */
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int getDerivedHDAddressFromMnemonic(
			uint account,
			uint index,
			[MarshalAs(UnmanagedType.LPArray)] char[] change_level,
			[MarshalAs(UnmanagedType.LPArray)] char[] mnemonic,
			[MarshalAs(UnmanagedType.LPArray)] char[] pass,
			[Out, MarshalAs(UnmanagedType.LPArray)] char[] p2pkh_pubkey,
			[MarshalAs(UnmanagedType.I1)] bool is_testnet
		);


		/* sign a message with a private key */
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern char[] sign_message(
			[MarshalAs(UnmanagedType.LPArray)] char[] privkey,
			[MarshalAs(UnmanagedType.LPArray)] char[] msg
		);


		/* verify a message with a address */
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern bool verify_message(
			[MarshalAs(UnmanagedType.LPArray)] char[] sig,
			[MarshalAs(UnmanagedType.LPArray)] char[] msg,
			[MarshalAs(UnmanagedType.LPArray)] char[] address
		);


		/* create a QR text formatted string (with line breaks) from an incoming p2pkh*/
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int qrgen_p2pkh_to_qr_string(
			[MarshalAs(UnmanagedType.LPArray)] char[] in_p2pkh,
			[Out, MarshalAs(UnmanagedType.LPArray)] char[] outString
		);


		///* Creates a .png file with the filename outFilename, from string inString, w. size factor of SizeMultiplier.*/
		////int qrgen_string_to_qr_pngfile(const char* outFilename,
		////const char* inString, uint8_t sizeMultiplier);
		//[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		//internal static extern int qrgen_string_to_qr_pngfile(
		//	[MarshalAs(UnmanagedType.LPArray)] char[] outFilename,
		//	[MarshalAs(UnmanagedType.LPArray)] char[] inString,
		//	byte sizeMultiplier
		//);

		///* Creates a .jpg file with the filename outFilename, from string inString, w. size factor of SizeMultiplier.*/
		////int qrgen_string_to_qr_jpgfile(const char* outFilename, const char* inString, uint8_t sizeMultiplier);
		//[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		//internal static extern int qrgen_string_to_qr_jpgfile(
		//	[MarshalAs(UnmanagedType.LPArray)] char[] outFilename,
		//	[MarshalAs(UnmanagedType.LPArray)] char[] inString,
		//	byte sizeMultiplier
		//);


		/* create a new dogecoin transaction: Returns the (txindex) in memory of the transaction being worked on. */
		[DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int start_transaction();


		/* add a utxo to the transaction being worked on at (txindex), specifying the utxo's txid and vout. returns 1 if successful.*/
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int add_utxo(
			int txindex,
			[MarshalAs(UnmanagedType.LPArray)] char[] hex_utxo_txid,
			int vout
		);




		/* add an output to the transaction being worked on at (txindex) of amount (amount) in dogecoins, returns 1 if successful. */
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int add_output(
			int txindex,
			[MarshalAs(UnmanagedType.LPArray)] char[] destinationaddress,
			[MarshalAs(UnmanagedType.LPArray)] char[] amount
		);


		/* finalize the transaction being worked on at (txindex), with the (destinationaddress) paying a fee of (subtractedfee), */
		/* re-specify the amount in dogecoin for verification, and change address for change. If not specified, change will go to the first utxo's address. */
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern char[] finalize_transaction(
			int txindex,
			[MarshalAs(UnmanagedType.LPArray)] char[] destinationaddress,
			[MarshalAs(UnmanagedType.LPArray)] char[] subtractedfee,
			[Out, MarshalAs(UnmanagedType.LPArray)] char[] out_dogeamount_for_verification,
			[MarshalAs(UnmanagedType.LPArray)] char[] changeaddress
		);

		/* retrieve the raw transaction at (txindex) as a hex string (char*) */
		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		internal static extern char[] get_raw_transaction(
			int txindex
		);



	}
}